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
    public class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(z => z.BloodType)
                .HasMaxLength(5);
            builder.Property(z => z.Note)
                .HasMaxLength(500);
                
        }
    }
}
