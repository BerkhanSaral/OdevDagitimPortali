﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OdevDagitimPortali.Repository;

#nullable disable

namespace OdevDagitimPortali.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241209184213_mig3")]
    partial class mig3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OdevDagitimPortali.Models.Assignment", b =>
                {
                    b.Property<int>("assignment_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("assignment_id"));

                    b.Property<string>("created_date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dead_line")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("user_ID")
                        .HasColumnType("int");

                    b.HasKey("assignment_id");

                    b.HasIndex("user_ID");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("OdevDagitimPortali.Models.Submission", b =>
                {
                    b.Property<int>("submission_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("submission_id"));

                    b.Property<int>("assignment_ID")
                        .HasColumnType("int");

                    b.Property<string>("file_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("submission_date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("user_ID")
                        .HasColumnType("int");

                    b.HasKey("submission_id");

                    b.HasIndex("assignment_ID");

                    b.HasIndex("user_ID");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("OdevDagitimPortali.Models.user.Users", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("user_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("role")
                        .HasColumnType("int");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OdevDagitimPortali.Models.Assignment", b =>
                {
                    b.HasOne("OdevDagitimPortali.Models.user.Users", "User")
                        .WithMany("Assignments")
                        .HasForeignKey("user_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OdevDagitimPortali.Models.Submission", b =>
                {
                    b.HasOne("OdevDagitimPortali.Models.Assignment", "Assignment")
                        .WithMany()
                        .HasForeignKey("assignment_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OdevDagitimPortali.Models.user.Users", "User")
                        .WithMany("Submissions")
                        .HasForeignKey("user_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OdevDagitimPortali.Models.user.Users", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Submissions");
                });
#pragma warning restore 612, 618
        }
    }
}
