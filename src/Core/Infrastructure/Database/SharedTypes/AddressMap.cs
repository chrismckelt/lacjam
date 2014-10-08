namespace Structerre.MetaStore.Core.Infrastructure.Database.SharedTypes
{
    public class AddressMap : IAutoMappingOverride<Address>
    {
        public void Override(AutoMapping<Address> mapping)
        {
            mapping.Map(x => x.UnitNumber).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.StreetNumber).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.AddressLine1).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.AddressLine2).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.AddressLine3).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.AddressLine4).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.Suburb).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.State).CustomType("AnsiString").Length(50);
            mapping.Map(x => x.PostCode).CustomType("AnsiString").Length(10);
            mapping.Map(x => x.CountryCode).CustomType("AnsiString").Length(3);
        }
    }
}
