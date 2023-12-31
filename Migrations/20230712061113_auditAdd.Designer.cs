﻿// <auto-generated />
using System;
using LMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMS.Migrations
{
    [DbContext(typeof(LmsContext))]
    [Migration("20230712061113_auditAdd")]
    partial class auditAdd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LMS.Book", b =>
                {
                    b.Property<string>("Iban")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("IBAN");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.HasKey("Iban")
                        .HasName("PK__tmp_ms_x__8235CCBD1D95768C");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("LMS.IssuedBook", b =>
                {
                    b.Property<string>("Iban")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("IBAN");

                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("userID");

                    b.Property<DateTime>("IssuedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("issuedDate")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("Iban", "UserId")
                        .HasName("pk_issuedBooks");

                    b.ToTable("IssuedBooks");
                });

            modelBuilder.Entity("LMS.RequestBook", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("userID");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("IBAN");

                    b.HasKey("UserId")
                        .HasName("PK__tmp_ms_x__CB9A1CDFE3387305");

                    b.ToTable("RequestBook", (string)null);
                });

            modelBuilder.Entity("LMS.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("Admin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("admin")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Password")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("password");

                    b.HasKey("Id")
                        .HasName("PK__Users__3214EC07E19D8C7B");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LMS.RequestBook", b =>
                {
                    b.HasOne("LMS.User", "User")
                        .WithOne("RequestBook")
                        .HasForeignKey("LMS.RequestBook", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_RequestBooks");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMS.User", b =>
                {
                    b.Navigation("RequestBook");
                });
#pragma warning restore 612, 618
        }
    }
}
