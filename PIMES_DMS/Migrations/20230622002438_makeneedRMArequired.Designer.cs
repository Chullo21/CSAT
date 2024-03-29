﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PIMES_DMS.Data;

#nullable disable

namespace PIMES_DMS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230622002438_makeneedRMArequired")]
    partial class makeneedRMArequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PIMES_DMS.Models.ART_8DModel", b =>
                {
                    b.Property<int>("ARTID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ARTID"));

                    b.Property<string>("ControlNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateValidated")
                        .HasColumnType("datetime2");

                    b.HasKey("ARTID");

                    b.ToTable("ART_8D");
                });

            modelBuilder.Entity("PIMES_DMS.Models.AccountsModel", b =>
                {
                    b.Property<int>("AccID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccID"));

                    b.Property<string>("AccName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccUCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("AccID");

                    b.ToTable("AccountsDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.ActionModel", b =>
                {
                    b.Property<int>("ActionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActionID"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ControlNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasVer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TESID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActionID");

                    b.ToTable("ActionDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.ERModel", b =>
                {
                    b.Property<int>("ERID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ERID"));

                    b.Property<string>("ControlNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("FGDis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FGGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FGNOGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FGSOH")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IQADis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IQAGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IQANOGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IQASOH")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("RMAno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Rep")
                        .HasColumnType("bit");

                    b.Property<string>("WHSEDis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WHSEGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WHSENOGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WHSESOH")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WIPDis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WIPGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WIPNOGOOD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WIPSOH")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ERID");

                    b.ToTable("ERDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.IssueModel", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IssueID"));

                    b.Property<bool>("Acknowledged")
                        .HasColumnType("bit");

                    b.Property<string>("AffectedPN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ClientRep")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ControlNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAck")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateFound")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateVdal")
                        .HasColumnType("datetime2");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasCR")
                        .HasColumnType("bit");

                    b.Property<bool>("HasTES")
                        .HasColumnType("bit");

                    b.Property<string>("IssueCreator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NRMA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NeedRMA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProbDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Qty")
                        .HasColumnType("int");

                    b.Property<byte[]>("Report")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SerialNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ValNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ValRes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ValidatedStatus")
                        .HasColumnType("bit");

                    b.Property<string>("ValidationRepSum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("IssueID");

                    b.ToTable("IssueDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.NotifModel", b =>
                {
                    b.Property<int>("NotifID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotifID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotifID");

                    b.ToTable("NotifDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.SmtpEmailsModel", b =>
                {
                    b.Property<int>("SMTPID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SMTPID"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("SmtpServer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SMTPID");

                    b.ToTable("SEDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.TESModel", b =>
                {
                    b.Property<int>("TESID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TESID"));

                    b.Property<string>("ControlNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ECWhy1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ECWhy2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ECWhy3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ECWhy4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ECWhy5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SCWhy1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SCWhy2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SCWhy3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SCWhy4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SCWhy5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SRC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TCWhy1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TCWhy2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TCWhy3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TCWhy4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TCWhy5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TRC")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TESID");

                    b.ToTable("TESDb");
                });

            modelBuilder.Entity("PIMES_DMS.Models.Vermodel", b =>
                {
                    b.Property<int>("VerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VerID"));

                    b.Property<int>("ActionID")
                        .HasColumnType("int");

                    b.Property<string>("ControlNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateVer")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Files")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("RCType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Verificator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VerID");

                    b.ToTable("VerDb");
                });
#pragma warning restore 612, 618
        }
    }
}
