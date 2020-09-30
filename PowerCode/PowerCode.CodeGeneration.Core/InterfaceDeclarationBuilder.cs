using System;
using System.Linq;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public sealed class InterfaceDeclarationBuilder : DeclarationBuilderBase
    {
        public InterfaceDeclarationBuilder(string template) : base(template)
        {

        }

        public override string Build()
        {
            if (string.IsNullOrEmpty(_template))
            {
                return string.Empty;
            }

            var template = @"
		public interface $Name$ 
		{
			$Body$
		}
		";

            var parts = _template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //parts.Dump("Interface");

            if (parts.Length == 0 || string.IsNullOrEmpty(parts.FirstOrDefault()))
            {
                return string.Empty;
            }

            var name = parts.First();

            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            string methodTemplate = string.Empty;

            foreach (var item in parts.Skip(1))
            {
                var members = item.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
                //members.Dump(item);

                var classifer = members.First().Trim();

                switch (classifer)
                {
                    case "p":
                        AddBuilder(new PropertyDeclarationBuilder(members[1], isInterface: true));
                        break;

                    case "m":
                        methodTemplate = members[1].Replace(" ", ",");
                        //members.Dump(methodTemplate);

                        AddBuilder(new MethodDeclarationBuilder(methodTemplate, isInterface: true));
                        break;

                    default:
                        methodTemplate = item.Trim().Replace(" ", ",");
                        //members.Dump(methodTemplate);

                        AddBuilder(new MethodDeclarationBuilder(methodTemplate, isInterface: true));
                        break;
                }
            }

            var body = BuildBody();

            template = template.Replace("$Name$", name);
            template = template.Replace("$Body$", body);

            return template;
        }
    }
}
