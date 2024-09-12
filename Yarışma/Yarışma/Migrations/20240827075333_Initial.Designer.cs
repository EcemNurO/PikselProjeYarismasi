﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Yarışma.Models;

#nullable disable

namespace Yarışma.Migrations
{
    [DbContext(typeof(CompetitionDbContext))]
    [Migration("20240827075333_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Yarışma.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Yarışma.Models.Contestant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContestantCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ContestantProfilId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContestantCategoryId");

                    b.HasIndex("ContestantProfilId");

                    b.ToTable("Contestants");
                });

            modelBuilder.Entity("Yarışma.Models.ContestantCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContestantCategories");
                });

            modelBuilder.Entity("Yarışma.Models.ContestantProfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Biografy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("Univercity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContestantProfils");
                });

            modelBuilder.Entity("Yarışma.Models.Judge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("JudgeCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("JudgeProfilId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectEvaluationId")
                        .HasColumnType("int");

                    b.Property<int>("contestantProfilId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JudgeCategoryId");

                    b.HasIndex("JudgeProfilId");

                    b.HasIndex("ProjectCategoryId");

                    b.HasIndex("contestantProfilId");

                    b.ToTable("Judges");
                });

            modelBuilder.Entity("Yarışma.Models.JudgeCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JudgeCategories");
                });

            modelBuilder.Entity("Yarışma.Models.JudgeProfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Biografy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("Univercity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JudgeProfils");
                });

            modelBuilder.Entity("Yarışma.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContestantId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectQuestionId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ContestantId");

                    b.HasIndex("ProjectCategoryId");

                    b.HasIndex("ProjectQuestionId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Yarışma.Models.ProjectCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProjectCategories");
                });

            modelBuilder.Entity("Yarışma.Models.ProjectEvaluation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("JudgeId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JudgeId")
                        .IsUnique();

                    b.ToTable("ProjectEvaluations");
                });

            modelBuilder.Entity("Yarışma.Models.ProjectQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProjectQuestions");
                });

            modelBuilder.Entity("Yarışma.Models.Contestant", b =>
                {
                    b.HasOne("Yarışma.Models.ContestantCategory", "ContestantCategory")
                        .WithMany("Contestants")
                        .HasForeignKey("ContestantCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.ContestantProfil", "contestantProfil")
                        .WithMany("Contestants")
                        .HasForeignKey("ContestantProfilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContestantCategory");

                    b.Navigation("contestantProfil");
                });

            modelBuilder.Entity("Yarışma.Models.Judge", b =>
                {
                    b.HasOne("Yarışma.Models.JudgeCategory", "JudgeCategory")
                        .WithMany("Judges")
                        .HasForeignKey("JudgeCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.JudgeProfil", null)
                        .WithMany("Judges")
                        .HasForeignKey("JudgeProfilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.ProjectCategory", "ProjectCategory")
                        .WithMany("Judges")
                        .HasForeignKey("ProjectCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.ContestantProfil", "contestantProfil")
                        .WithMany("Judges")
                        .HasForeignKey("contestantProfilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JudgeCategory");

                    b.Navigation("ProjectCategory");

                    b.Navigation("contestantProfil");
                });

            modelBuilder.Entity("Yarışma.Models.Project", b =>
                {
                    b.HasOne("Yarışma.Models.Contestant", "Contestant")
                        .WithMany("Projects")
                        .HasForeignKey("ContestantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.ProjectCategory", "ProjectCategory")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectCategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Yarışma.Models.ProjectQuestion", "Question")
                        .WithMany()
                        .HasForeignKey("ProjectQuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Contestant");

                    b.Navigation("ProjectCategory");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Yarışma.Models.ProjectEvaluation", b =>
                {
                    b.HasOne("Yarışma.Models.Judge", "Judge")
                        .WithOne("ProjectEvaluation")
                        .HasForeignKey("Yarışma.Models.ProjectEvaluation", "JudgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Judge");
                });

            modelBuilder.Entity("Yarışma.Models.Contestant", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("Yarışma.Models.ContestantCategory", b =>
                {
                    b.Navigation("Contestants");
                });

            modelBuilder.Entity("Yarışma.Models.ContestantProfil", b =>
                {
                    b.Navigation("Contestants");

                    b.Navigation("Judges");
                });

            modelBuilder.Entity("Yarışma.Models.Judge", b =>
                {
                    b.Navigation("ProjectEvaluation");
                });

            modelBuilder.Entity("Yarışma.Models.JudgeCategory", b =>
                {
                    b.Navigation("Judges");
                });

            modelBuilder.Entity("Yarışma.Models.JudgeProfil", b =>
                {
                    b.Navigation("Judges");
                });

            modelBuilder.Entity("Yarışma.Models.ProjectCategory", b =>
                {
                    b.Navigation("Judges");

                    b.Navigation("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}
