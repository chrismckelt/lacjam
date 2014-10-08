using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityName
    {

        protected EntityName()
        {
        }

        public EntityName(string name)
        {

            if (String.IsNullOrWhiteSpace(name))
                throw new InvariantGuardFailureException("name");

            Name = name;
        }

        public string Name { get; private set; }
    }
}