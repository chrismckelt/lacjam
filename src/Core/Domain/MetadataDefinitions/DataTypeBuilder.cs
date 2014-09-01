using System;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public static class DataTypeBuilder
    {
        public static IDataType Create(string tag)
        {
            switch (tag)
            {
                case "YesNo":
                    return new YesNo();
                case "Character":
                    return new Character();
                case "ComboBox":
                    return new ComboBox();
                case "FloatingPoint":
                    return new FloatingPoint();
                case "Text":
                    return new Text();
                case "Integer":
                    return new Integer();
                case "PickList":
                    return new PickList();
            }

            throw new Exception();
        }
    }
}