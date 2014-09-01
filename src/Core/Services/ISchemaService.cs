using System;
using Lacjam.Framework.FP;

namespace Lacjam.Core.Services
{
    public interface ISchemaService
    {
        IMaybe<SchemaTransferObject> GetSchemaFor(Guid identity);
    }
}