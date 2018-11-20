using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using MPapi.Helpers;
using MPapi.Models;

namespace MPapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly MPapiContext _context;
        private IConfiguration _configuration;
        public ListingController(MPapiContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: api/Listing
        [HttpGet]
        public IEnumerable<ListingItem> GetListingItem()
        {
            return _context.ListingItem;
        }

        // GET: api/Listing/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListingItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listingItem = await _context.ListingItem.FindAsync(id);

            if (listingItem == null)
            {
                return NotFound();
            }

            return Ok(listingItem);
        }

        // PUT: api/Listing/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListingItem([FromRoute] int id, [FromBody] ListingItem listingItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != listingItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(listingItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Listing
        [HttpPost]
        public async Task<IActionResult> PostListingItem([FromBody] ListingItem listingItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ListingItem.Add(listingItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListingItem", new { id = listingItem.Id }, listingItem);
        }

        // DELETE: api/Listing/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListingItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listingItem = await _context.ListingItem.FindAsync(id);
            if (listingItem == null)
            {
                return NotFound();
            }

            _context.ListingItem.Remove(listingItem);
            await _context.SaveChangesAsync();

            return Ok(listingItem);
        }

        private bool ListingItemExists(int id)
        {
            return _context.ListingItem.Any(e => e.Id == id);
        }
        // GET: api/Listing/Ids
        [Route("Ids")]
        [HttpGet]
        public async Task<List<string>> GetTags()
            {
            var listing = (from m in _context.ListingItem
                           select m.userId).Distinct();

            var returned = await listing.ToListAsync();
            return returned;
            }

        // GET: api/Listing/Ids
        [HttpGet("search/{title}")]
        public async Task<List<ListingItem>> GetSpecifiedTitles([FromRoute] string title)
            {
            if (string.IsNullOrEmpty(title))
                {
                throw new ArgumentException("message", nameof(title));
                }

            var item = (from m in _context.ListingItem
                        where checkSub(m.Title,title)
                        select m);
            var returned = await item.ToListAsync();
            return returned;
            }

        private Boolean checkSub(string title, string search)
            {
            title = title.ToLower();
            search = search.ToLower();
            char[] tarray = title.ToCharArray();
            char[] sarray = search.ToCharArray();
            if (sarray.Length > tarray.Length)
                {
                return false;
                }
            for (int i = 0; i < sarray.Length; i++)
                {
                if (tarray[i] != sarray[i])
                    {
                    return false;
                    }
                }
            return true;
            }

        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]ImageItem item)
            {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                {
                return BadRequest($"Expected a multipart request,but got {Request.ContentType}");
                }
            try
                {
                using (var stream = item.Image.OpenReadStream())
                    {
                    var cloudBlock = await UploadToBlob(item.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                        {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                        }

                    ListingItem listingItem = new ListingItem();
                    listingItem.Title = item.Title;
                    listingItem.Seller = item.Seller;
                    listingItem.Price = item.Price;
                    listingItem.Description = item.Description;
                    listingItem.email = item.email;
                    listingItem.userId = item.userId;

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    listingItem.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;
                    listingItem.Uploaded = DateTime.Now.ToString();

                    _context.ListingItem.Add(listingItem);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {item.Title} has successfully uploaded");
                    }
                }
            catch (Exception ex)
                {
                return BadRequest($"An error has occured. Details: {ex.Message}");
                }
            }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
            {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("images");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
                {
                try
                    {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                        {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                        }
                    else
                        {
                        return new CloudBlockBlob(new Uri(""));
                        }

                    return cloudBlockBlob;
                    }
                catch (StorageException ex)
                    {
                    return new CloudBlockBlob(new Uri(""));
                    }
                }
            else
                {
                return new CloudBlockBlob(new Uri(""));
                }

            }

        private string GetFileExtention(string fileName)
            {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
                {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
                }
            }
        }
    }