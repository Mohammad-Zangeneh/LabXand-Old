using LabXand.Infrastructure.Data;
using LabXand.Infrastructure.Data.EF;
using LabXand.Logging.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Data
{
    public class DetailsLogEntryMap : EntityTypeConfiguration<DetailsLogEntry>
    {
        public DetailsLogEntryMap()
        {
            // Primary Key
            HasKey(t => t.Id);
            Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");

            
            ToTable("LogDetails");
        }
    }
}
