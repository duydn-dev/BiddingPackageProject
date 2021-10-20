﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neac.DataAccess;

namespace Neac.DataAccess.Migrations
{
    [DbContext(typeof(NeacDbContext))]
    partial class NeacDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Neac.DataAccess.BiddingPackage", b =>
                {
                    b.Property<Guid>("BiddingPackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BiddingPackageName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BiddingPackageId");

                    b.ToTable("BiddingPackage");
                });

            modelBuilder.Entity("Neac.DataAccess.BiddingPackageProject", b =>
                {
                    b.Property<Guid>("BiddingPackageProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BiddingPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BiddingPackageProjectId");

                    b.HasIndex("BiddingPackageId");

                    b.HasIndex("ProjectId");

                    b.ToTable("BiddingPackageProject");
                });

            modelBuilder.Entity("Neac.DataAccess.Document", b =>
                {
                    b.Property<Guid>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BiddingPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsCommon")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.HasIndex("BiddingPackageId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("Neac.DataAccess.GroupRole", b =>
                {
                    b.Property<Guid>("GroupRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupRoleCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupRoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupRoleId");

                    b.ToTable("GroupRole");
                });

            modelBuilder.Entity("Neac.DataAccess.MeetRoom", b =>
                {
                    b.Property<Guid>("MeetRoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DomainUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MemberNumberInRoom")
                        .HasColumnType("int");

                    b.Property<int?>("MemberOnline")
                        .HasColumnType("int");

                    b.Property<string>("RoomName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("MeetRoomId");

                    b.ToTable("MeetRoom");
                });

            modelBuilder.Entity("Neac.DataAccess.Project", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CurrentState")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProjectDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Neac.DataAccess.ProjectFlow", b =>
                {
                    b.Property<Guid>("ProjectFlowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BiddingPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentAbstract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProjectDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PromulgateUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegulationDocument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Signer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("ProjectFlowId");

                    b.ToTable("ProjectFlow");
                });

            modelBuilder.Entity("Neac.DataAccess.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("GroupRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.HasIndex("GroupRoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Neac.DataAccess.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumberPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserPositionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId");

                    b.HasIndex("UserPositionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Neac.DataAccess.UserPosition", b =>
                {
                    b.Property<Guid>("UserPositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsAdministrator")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserPositionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserPositionId");

                    b.ToTable("UserPosition");
                });

            modelBuilder.Entity("Neac.DataAccess.UserRole", b =>
                {
                    b.Property<Guid>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Neac.DataAccess.BiddingPackageProject", b =>
                {
                    b.HasOne("Neac.DataAccess.BiddingPackage", "BiddingPackage")
                        .WithMany("BiddingPackageProjects")
                        .HasForeignKey("BiddingPackageId");

                    b.HasOne("Neac.DataAccess.Project", "Project")
                        .WithMany("BiddingPackageProjects")
                        .HasForeignKey("ProjectId");

                    b.Navigation("BiddingPackage");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Neac.DataAccess.Document", b =>
                {
                    b.HasOne("Neac.DataAccess.BiddingPackage", "BiddingPackage")
                        .WithMany("Documents")
                        .HasForeignKey("BiddingPackageId");

                    b.Navigation("BiddingPackage");
                });

            modelBuilder.Entity("Neac.DataAccess.Role", b =>
                {
                    b.HasOne("Neac.DataAccess.GroupRole", "GroupRole")
                        .WithMany("Roles")
                        .HasForeignKey("GroupRoleId");

                    b.Navigation("GroupRole");
                });

            modelBuilder.Entity("Neac.DataAccess.User", b =>
                {
                    b.HasOne("Neac.DataAccess.UserPosition", "UserPosition")
                        .WithMany("Users")
                        .HasForeignKey("UserPositionId");

                    b.Navigation("UserPosition");
                });

            modelBuilder.Entity("Neac.DataAccess.UserRole", b =>
                {
                    b.HasOne("Neac.DataAccess.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("Neac.DataAccess.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Neac.DataAccess.BiddingPackage", b =>
                {
                    b.Navigation("BiddingPackageProjects");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("Neac.DataAccess.GroupRole", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Neac.DataAccess.Project", b =>
                {
                    b.Navigation("BiddingPackageProjects");
                });

            modelBuilder.Entity("Neac.DataAccess.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Neac.DataAccess.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Neac.DataAccess.UserPosition", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
