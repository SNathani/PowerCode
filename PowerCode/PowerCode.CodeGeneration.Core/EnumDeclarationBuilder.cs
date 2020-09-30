using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------
    public sealed class EnumDeclarationBuilder : DeclarationBuilderBase
    {
        public EnumDeclarationBuilder(string template) : base(template)
        {

        }

        public override string Build()
        {
            if (string.IsNullOrEmpty(_template))
            {
                return string.Empty;
            }

            var enumTemplate = @"
        public enum $EnumName$ 
        {
            $Values$
        }
        ";

            var parts = _template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //parts.Dump("Enum");

            if (parts.Length == 0 || string.IsNullOrEmpty(parts.FirstOrDefault()))
            {
                return string.Empty;
            }

            var name = parts[0];
            var enumValues = new List<string>();
            foreach (var item in parts.Skip(1))
            {
                if (string.IsNullOrEmpty(item)) continue;
                enumValues.Add(item);
            }

            enumTemplate = enumTemplate.Replace("$EnumName$", name);
            enumTemplate = enumTemplate.Replace("$Values$", string.Join(",\r\n", enumValues));

            return enumTemplate;
        }
    }
}
