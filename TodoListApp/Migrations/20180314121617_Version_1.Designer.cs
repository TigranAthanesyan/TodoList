﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TodoListApp.Models;

namespace TodoListApp.Migrations
{
    [DbContext(typeof(TodoContext))]
    [Migration("20180314121617_Version_1")]
    partial class Version_1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TodoListApp.Models.Person", b =>
                {
                    b.Property<string>("UserName")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.HasKey("UserName");

                    b.ToTable("People");
                });

            modelBuilder.Entity("TodoListApp.Models.Todo", b =>
                {
                    b.Property<string>("UserName");

                    b.Property<string>("What");

                    b.Property<DateTime>("ActualDate");

                    b.Property<DateTime>("DeadLine");

                    b.HasKey("UserName", "What");

                    b.ToTable("TodoList");
                });
#pragma warning restore 612, 618
        }
    }
}
