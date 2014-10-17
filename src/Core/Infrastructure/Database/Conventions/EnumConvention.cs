using System;
using System.Data;

namespace Lacjam.Core.Infrastructure.Database.Conventions
{
    public class EnumConvention : IUserTypeConvention
    {
        private Type _type;
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum ||
           (x.Property.PropertyType.IsGenericType &&
            x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
            x.Property.PropertyType.GetGenericArguments()[0].IsEnum)
           );            
        }



        public void Apply(IPropertyInstance target)
        {
            target.CustomSqlType(@"varchar(25)"); //TODO CDM
            //Type proxyType = Type.GetType(target.Name);
            //target.CustomSqlType(new EnumMapper().SqlType.ToString());
        }
    }

    public class EnumMapper<T> : EnumStringType<T>
    {
        public override SqlType SqlType
        {
            get
            {
                return new SqlType(DbType.Object);
            }
        }

        public static IPropertyConvention Convention
        {
            get
            {
                return ConventionBuilder.Property.When(
                   c => c.Expect(x => x.Type == typeof(GenericEnumMapper<T>)),
                   x =>
                   {
                       x.CustomType<EnumMapper<T>>();
                       x.CustomSqlType((typeof(T).Name));
                   });
            }
        }
    }


}
