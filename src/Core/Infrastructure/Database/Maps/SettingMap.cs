using FluentNHibernate.Mapping;
using Lacjam.Core.Infrastructure.Settings;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class SettingMap : ClassMap<Setting>
    {
        public SettingMap()
        {
            Id(a => a.Name).GeneratedBy.Assigned();
        }
    }
}
