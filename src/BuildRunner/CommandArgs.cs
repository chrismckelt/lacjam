﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.BuildRunner
{
    public class CommandArgs
    {
        public bool Help { get; set; }
        public string Connectionstring { get; set; }
        public bool BuildDatabase { get; set; }
        public bool UpdateDatabase { get; set; }
        public string PlayEvents { get; set; }
        public bool DeleteTables { get; set; }
    }
}
