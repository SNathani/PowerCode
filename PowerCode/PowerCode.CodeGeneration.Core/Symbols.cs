using System;
using System.Collections.Generic;

namespace PowerCode.CodeGeneration.Core
{
    public static class Symbols
    {
        public static readonly string[] SPACE = new string[] { " " };
        public static readonly string[] EOL = new string[] { Environment.NewLine, "|" };

        public static readonly Dictionary<string, List<string>> TypeInferences =
                    new Dictionary<string, System.Collections.Generic.List<string>>(){
                        {"bool", new List<string> {"Is","Has","Can"}},
                        {"int", new List<string> {"Id","ID"}},
                        {"DateTime", new List<string> {"Date"}}
                    };

    }
}
