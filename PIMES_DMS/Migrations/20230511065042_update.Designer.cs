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
    [Migration("20230511065042_update")]
    partial class update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.Property<string>("CompName")
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

                    b.Property<string>("CoD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ControlNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAck")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DateFound")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateVdal")
                        .HasColumnType("datetime2");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasCR")
                        .HasColumnType("bit");

                    b.Property<string>("IssueCreator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueNo")
                        .IsRequired()
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

                    b.Property<int>("SerialNo")
                        .HasColumnType("int");

                    b.Property<bool>("ValidatedStatus")
                        .HasColumnType("bit");

                    b.Property<string>("ValidationRepSum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("IssueID");

                    b.ToTable("IssueDb");
                });
#pragma warning restore 612, 618
        }
    }
}
