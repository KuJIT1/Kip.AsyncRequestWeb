﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Kip.AsyncReport.Infrastructure;

#nullable disable

namespace Kip.AsyncReport.Migrations
{
    [DbContext(typeof(ReportTaskContext))]
    [Migration("20240313172407_ReportTaskUserIdTypeChanged")]
    partial class ReportTaskUserIdTypeChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kip.AsyncReport.Model.ReportResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CountSignIn")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ReportResult");
                });

            modelBuilder.Entity("Kip.AsyncReport.Model.ReportTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Percent")
                        .HasColumnType("int");

                    b.Property<Guid?>("ReportResultId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.ComplexProperty<Dictionary<string, object>>("Request", "Kip.AsyncReport.Model.ReportTask.Request#ReportRequest", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateTime>("PeriodEnd")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("PeriodStart")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");
                        });

                    b.HasKey("Id");

                    b.HasIndex("ReportResultId");

                    b.ToTable("ReportTasks");
                });

            modelBuilder.Entity("Kip.AsyncReport.Model.ReportTask", b =>
                {
                    b.HasOne("Kip.AsyncReport.Model.ReportResult", "ReportResult")
                        .WithMany()
                        .HasForeignKey("ReportResultId");

                    b.Navigation("ReportResult");
                });
#pragma warning restore 612, 618
        }
    }
}
