using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .HasColumnType("varchar()");

            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .HasColumnType("varchar");
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email LIKE '_%@_%._%' "); 
                tb.HasCheckConstraint("PhoneCheck", "Phone LIKE '010%' Or Phone LIKE '011%' Or Phone LIKE '012%' Or Phone LIKE '015%' "); 

            });

            builder.Property(x => x.Phone)
                .HasMaxLength(11)
                .HasColumnType("varchar");
            builder.HasIndex(x => x.Phone)
                .IsUnique();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
                address.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
                
            });
        }
    }
}
