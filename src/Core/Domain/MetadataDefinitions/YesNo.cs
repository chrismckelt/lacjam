using System.Collections.Generic;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class YesNo : PickList, IPredefinedValues
    {
        public YesNo()
        {
            Tag = "YesNo";
            Set = new List<string> {"Yes", "No"};
        }

        public IEnumerable<string> Set { get; private set; }
    }
}