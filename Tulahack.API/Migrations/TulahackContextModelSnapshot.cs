﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tulahack.API.Context;

#nullable disable

namespace Tulahack.API.Migrations
{
    [DbContext(typeof(TulahackContext))]
    partial class TulahackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Tulahack.Model.AcceptanceCriteria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid?>("ContestCaseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Criteria")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "criteria");

                    b.HasKey("Id");

                    b.HasIndex("ContestCaseId");

                    b.ToTable("AcceptanceCriteria");

                    b.HasAnnotation("Relational:JsonPropertyName", "acceptanceCriteria");
                });

            modelBuilder.Entity("Tulahack.Model.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.HasKey("Id");

                    b.ToTable("Companies", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "company");
                });

            modelBuilder.Entity("Tulahack.Model.ContestApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "creationDate");

                    b.Property<string>("Justification")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "justification");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "userId");

                    b.HasKey("Id");

                    b.ToTable("ContestApplications");
                });

            modelBuilder.Entity("Tulahack.Model.ContestCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TeamId");

                    b.ToTable("ContestCases", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "contestCases");
                });

            modelBuilder.Entity("Tulahack.Model.PersonBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("About")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "about");

                    b.Property<bool>("Blocked")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "blocked");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "email");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "firstname");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "lastname");

                    b.Property<string>("Middlename")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "middlename");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "phoneNumber");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "photoUrl");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "role");

                    b.Property<string>("TelegramAccount")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "telegramAccount");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);

                    b.HasDiscriminator<int>("Role").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Tulahack.Model.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid?>("ContestantId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ExpertId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SkillName")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "skillName");

                    b.HasKey("Id");

                    b.HasIndex("ContestantId");

                    b.HasIndex("ExpertId");

                    b.ToTable("Skills", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "skills");
                });

            modelBuilder.Entity("Tulahack.Model.StorageFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "creationDate");

                    b.Property<string>("Filename")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "filename");

                    b.Property<string>("Filepath")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "filepath");

                    b.Property<Guid>("Owner")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "owner");

                    b.Property<int>("Purpose")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "purpose");

                    b.Property<int>("Revision")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "revision");

                    b.HasKey("Id");

                    b.ToTable("StorageFiles", (string)null);
                });

            modelBuilder.Entity("Tulahack.Model.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "additionalInfo");

                    b.Property<string>("AdditionalRepositoryLink")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "additionalRepositoryLink");

                    b.Property<Guid?>("ExpertId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FormOfParticipation")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "formOfParticipation");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("OpenUniversityProjectLink")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "openUniversityProjectLink");

                    b.Property<string>("PresentationLink")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "presentationLink");

                    b.Property<string>("RepositoryLink")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "repositoryLink");

                    b.Property<int>("Section")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "section");

                    b.Property<string>("Teaser")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "teaser");

                    b.Property<string>("VideoLink")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "videoLink");

                    b.HasKey("Id");

                    b.HasIndex("ExpertId");

                    b.ToTable("Teams", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "pickedTeams");
                });

            modelBuilder.Entity("Tulahack.Model.Contestant", b =>
                {
                    b.HasBaseType("Tulahack.Model.PersonBase");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "age");

                    b.Property<bool>("ApplicationConfirmationStatus")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "applicationConfirmationStatus");

                    b.Property<Guid>("ApplicationNumber")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "applicationNumber");

                    b.Property<DateTime>("ApplicationSubmissionDate")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "applicationSubmissionDate");

                    b.Property<string>("AttendanceType")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "attendanceType");

                    b.Property<bool>("AttendingFirstTime")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "attendingFirstTime");

                    b.Property<bool>("CertificateNeeded")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "certificateNeeded");

                    b.Property<string>("City")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "city");

                    b.Property<string>("CompanyName")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "companyName");

                    b.Property<int>("ContestantNumber")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "contestantNumber");

                    b.Property<string>("EducationType")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "educationType");

                    b.Property<string>("GithubUrl")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "githubUrl");

                    b.Property<string>("JobTitle")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "jobTitle");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "region");

                    b.Property<string>("Scholarship")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "scholarship");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("TEXT");

                    b.Property<string>("University")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "university");

                    b.Property<string>("UniversityAttendanceFormat")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "universityAttendanceFormat");

                    b.Property<string>("UniversityDepartment")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "universityDepartment");

                    b.HasIndex("TeamId");

                    b.HasDiscriminator().HasValue(1);

                    b.HasAnnotation("Relational:JsonPropertyName", "contestants");
                });

            modelBuilder.Entity("Tulahack.Model.Expert", b =>
                {
                    b.HasBaseType("Tulahack.Model.PersonBase");

                    b.Property<string>("AdditionalContactPerson")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "additionalContactPerson");

                    b.Property<bool>("Badge")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "badge");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "confirmed");

                    b.Property<string>("WorkPosition")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "workPosition");

                    b.HasIndex("CompanyId");

                    b.HasDiscriminator().HasValue(2);

                    b.HasAnnotation("Relational:JsonPropertyName", "experts");
                });

            modelBuilder.Entity("Tulahack.Model.Moderator", b =>
                {
                    b.HasBaseType("Tulahack.Model.PersonBase");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Tulahack.Model.Superuser", b =>
                {
                    b.HasBaseType("Tulahack.Model.PersonBase");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("Tulahack.Model.AcceptanceCriteria", b =>
                {
                    b.HasOne("Tulahack.Model.ContestCase", null)
                        .WithMany("AcceptanceCriteria")
                        .HasForeignKey("ContestCaseId");
                });

            modelBuilder.Entity("Tulahack.Model.ContestCase", b =>
                {
                    b.HasOne("Tulahack.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("Tulahack.Model.Team", null)
                        .WithMany("ContestCases")
                        .HasForeignKey("TeamId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Tulahack.Model.Skill", b =>
                {
                    b.HasOne("Tulahack.Model.Contestant", null)
                        .WithMany("Skills")
                        .HasForeignKey("ContestantId");

                    b.HasOne("Tulahack.Model.Expert", null)
                        .WithMany("Skills")
                        .HasForeignKey("ExpertId");
                });

            modelBuilder.Entity("Tulahack.Model.Team", b =>
                {
                    b.HasOne("Tulahack.Model.Expert", null)
                        .WithMany("PickedTeams")
                        .HasForeignKey("ExpertId");
                });

            modelBuilder.Entity("Tulahack.Model.Contestant", b =>
                {
                    b.HasOne("Tulahack.Model.Team", "Team")
                        .WithMany("Contestants")
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Tulahack.Model.Expert", b =>
                {
                    b.HasOne("Tulahack.Model.Company", "Company")
                        .WithMany("Experts")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Tulahack.Model.Company", b =>
                {
                    b.Navigation("Experts");
                });

            modelBuilder.Entity("Tulahack.Model.ContestCase", b =>
                {
                    b.Navigation("AcceptanceCriteria");
                });

            modelBuilder.Entity("Tulahack.Model.Team", b =>
                {
                    b.Navigation("ContestCases");

                    b.Navigation("Contestants");
                });

            modelBuilder.Entity("Tulahack.Model.Contestant", b =>
                {
                    b.Navigation("Skills");
                });

            modelBuilder.Entity("Tulahack.Model.Expert", b =>
                {
                    b.Navigation("PickedTeams");

                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
