using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OnionArchitectures.Repository;

namespace OnionArchitectures.Repository.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OnionArchitectures.Data.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("IpAddress");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OnionArchitectures.Data.UserProfile", b =>
                {
                    b.Property<long>("Id");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("IpAddress");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.HasKey("Id");

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("OnionArchitectures.Data.UserProfile", b =>
                {
                    b.HasOne("OnionArchitectures.Data.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("OnionArchitectures.Data.UserProfile", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
