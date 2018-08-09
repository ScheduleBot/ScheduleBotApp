﻿// <auto-generated />
using System;
using Final401.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Final401.Migrations.ScheduleDB
{
    [DbContext(typeof(ScheduleDBContext))]
    [Migration("20180808180245_addedChatLog")]
    partial class addedChatLog
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Final401.Models.ChatLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Chat");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("ID");

                    b.ToTable("ChatLogs");
                });

            modelBuilder.Entity("Final401.Models.Schedule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Final401.Models.ScheduleItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Days");

                    b.Property<string>("Description");

                    b.Property<TimeSpan>("Length");

                    b.Property<int>("ScheduleID");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("ScheduleItems");
                });
#pragma warning restore 612, 618
        }
    }
}