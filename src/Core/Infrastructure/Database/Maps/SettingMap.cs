
using System.Data.Entity.ModelConfiguration;
using Lacjam.Core.Infrastructure.Settings;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
          //  Id(a => a.Name).GeneratedBy.Assigned();
        }
    }
}
