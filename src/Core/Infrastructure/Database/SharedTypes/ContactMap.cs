namespace Structerre.MetaStore.Core.Infrastructure.Database.SharedTypes
{
    public class ContactMap : IAutoMappingOverride<Contact>
    {
        public void Override(AutoMapping<Contact> mapping)
        {
            mapping.Map(x => x.EmailAddress).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.HomeTelephone).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.WorkTelephone).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.MobileTelephone).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.HomeFaxNumber).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.WorkFaxNumber).CustomType("AnsiString").Length(50);
        }
    }
}
