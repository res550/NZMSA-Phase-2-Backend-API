using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPapi.Models;

namespace MPapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly MPapiContext _context;

        public ListingController(MPapiContext context)
        {
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
        public async Task<IActionResult> GetListingItem([FromRoute] string id)
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
        public async Task<IActionResult> PutListingItem([FromRoute] string id, [FromBody] ListingItem listingItem)
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
        public async Task<IActionResult> DeleteListingItem([FromRoute] string id)
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

        private bool ListingItemExists(string id)
        {
            return _context.ListingItem.Any(e => e.Id == id);
        }
    }
}