using System;
using System.Linq;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public sealed class StructDeclarationBuilder : DeclarationBuilderBase
    {
        public StructDeclarationBuilder(string template) : base(template)
        {

        }

        public override string Build()
        {
            if (string.IsNullOrEmpty(_template))
            {
                return string.Empty;
            }

            var template = @"
        public struct $StructName$ 
        {
            $Body$
        }
        ";

            var parts = _template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //parts.Dump("Stuct");

            if (parts.Length == 0 || string.IsNullOrEmpty(parts.FirstOrDefault()))
            {
                return string.Empty;
            }

            var name = parts[0];

            if (parts.Length > 1)
            {
                foreach (var item in parts.Skip(1))
                {
                    var itemParts = item.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
                    //itemParts.Dump();

                    switch (itemParts[0].Trim())
                    {
                        case "m":
                            AddBuilder(new MethodDeclarationBuilder(itemParts[1]));
                            break;
                        default:
                            AddBuilder(new PropertyDeclarationBuilder(itemParts[0]));
                            break;
                    }
                }
            }

            var body = BuildBody();
            template = template.Replace("$StructName$", name);
            template = template.Replace("$Body$", body);
            return template;
        }
    }
}
