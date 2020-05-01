﻿// <auto-generated />
using System;
using LibraryManagement.DatabaseEntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibraryManagement.Migrations
{
    [DbContext(typeof(LibraryDatabaseContext))]
    [Migration("20200428233818_AddedBooks")]
    partial class AddedBooks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-preview.3.20181.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LibraryManagement.Models.DatabaseModels.BookDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserDatabaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserDatabaseId");

                    b.ToTable("Books");

                    b.HasCheckConstraint("CK_Books_Type_Enum_Constraint", "[Type] IN(0, 1)");
                });

            modelBuilder.Entity("LibraryManagement.Models.DatabaseModels.UserDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Validity")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasCheckConstraint("CK_Users_Gender_Enum_Constraint", "[Gender] IN(0, 1, 2)");

                    b.HasCheckConstraint("CK_Users_Role_Enum_Constraint", "[Role] IN(0, 1)");
                });

            modelBuilder.Entity("LibraryManagement.Models.DatabaseModels.BookDatabase", b =>
                {
                    b.HasOne("LibraryManagement.Models.DatabaseModels.UserDatabase", null)
                        .WithMany("Books")
                        .HasForeignKey("UserDatabaseId");
                });
#pragma warning restore 612, 618
        }
    }
}
