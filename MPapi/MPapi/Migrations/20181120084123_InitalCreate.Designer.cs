﻿// <auto-generated />
using MPapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MPapi.Migrations
{
    [DbContext(typeof(MPapiContext))]
    [Migration("20181120084123_InitalCreate")]
    partial class InitalCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("MPapi.Models.ListingItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Price");

                    b.Property<string>("Seller");

                    b.Property<string>("Title");

                    b.Property<string>("Uploaded");

                    b.Property<string>("Url");

                    b.Property<string>("email");

                    b.Property<string>("userId");

                    b.HasKey("Id");

                    b.ToTable("ListingItem");
                });
#pragma warning restore 612, 618
        }
    }
}