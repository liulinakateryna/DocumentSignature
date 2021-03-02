﻿// <auto-generated />
using System;
using Database.KeyCenter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.KeyCenter.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.KeyCenter.Entity.Keys", b =>
                {
                    b.Property<int>("KeysId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("D");

                    b.Property<byte[]>("DP");

                    b.Property<byte[]>("DQ");

                    b.Property<byte[]>("Exponent");

                    b.Property<byte[]>("InverseQ");

                    b.Property<byte[]>("Modulus");

                    b.Property<byte[]>("P");

                    b.Property<byte[]>("Q");

                    b.HasKey("KeysId");

                    b.ToTable("Keys");
                });

            modelBuilder.Entity("Database.KeyCenter.Entity.PrivateData", b =>
                {
                    b.Property<int>("PrivateDataId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Fingerprint");

                    b.Property<int>("KeysId");

                    b.Property<string>("UserId");

                    b.HasKey("PrivateDataId");

                    b.HasIndex("KeysId")
                        .IsUnique();

                    b.ToTable("PrivateData");
                });

            modelBuilder.Entity("Database.KeyCenter.Entity.PrivateData", b =>
                {
                    b.HasOne("Database.KeyCenter.Entity.Keys", "RsaParameters")
                        .WithOne("PrivateData")
                        .HasForeignKey("Database.KeyCenter.Entity.PrivateData", "KeysId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
