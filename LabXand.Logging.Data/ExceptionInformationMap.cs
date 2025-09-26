using LabXand.Logging.Core;
using System.Data.Entity.ModelConfiguration;

namespace LabXand.Logging.Data
{
    public class ExceptionInformationMap : EntityTypeConfiguration<ExceptionInformation>
    {
        public ExceptionInformationMap()
        {
            // Primary Key
            HasKey(t => t.Id);
            Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");

            ToTable("ExceptionInformations");
        }
    }
}
