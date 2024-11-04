﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBetter.Infrastructure.Migrations
{
    [DbContext(typeof(DBetterContext))]
    [Migration("20241104210809_User_Addons")]
    partial class User_Addons
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DBetter.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("_passwordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("_passwordSalt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PasswordSalt");

                    b.ComplexProperty<Dictionary<string, object>>("_refreshToken", "DBetter.Domain.Users.User._refreshToken#RefreshToken", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("DBetter.Domain.Users.User", b =>
                {
                    b.OwnsMany("DBetter.Domain.Users.ValueObjects.Discount", "_discounts", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("BoughtAtUtc")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<int>("Class")
                                .HasColumnType("integer");

                            b1.Property<int>("Type")
                                .HasColumnType("integer");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime?>("ValidUntilUtc")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("Discounts", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("_discounts");
                });
#pragma warning restore 612, 618
        }
    }
}