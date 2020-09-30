using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public sealed class ClassDeclarationBuilder : DeclarationBuilderBase
    {

        const string CTR_INDICATOR = ".";

        public ClassDeclarationBuilder(string template) : base(template)
        {
        }

        public override string Build()
        {
            var result = new StringBuilder();
            var classTemplate = GetClassTemplate(_template);

            var props = BuildProperties();
            classTemplate = classTemplate.Replace("$Properties$", props);

            var methods = BuildMethods();
            classTemplate = classTemplate.Replace("$Methods$", methods);

            result.Append(classTemplate);

            return result.ToString();
        }

        private string GetClassTemplate(string template)
        {
            var classTemplate = @"
		public class $ClassName$ $BaseClass$ $Interfaces$
		{
			public $ClassName$($ClassArgs$) 
			{
			}
			
			$Properties$
			
			$Methods$
		}
		";

            var parts = template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //parts.Dump();

            var name = parts[0];
            var baseClass = string.Empty;

            if (name.Contains(":"))
            {
                var nameParts = name.Split(new string[] { ":" }, StringSplitOptions.None);
                name = nameParts[0];
                baseClass = nameParts[1];
                if (baseClass.Trim() == ",")
                { baseClass = string.Empty; }

                if (string.IsNullOrEmpty(baseClass) == false)
                {
                    baseClass = ":" + baseClass;
                }
            }

            var interfaces = parts.Skip(1).Where(f => f.Trim().StartsWith(CTR_INDICATOR) == false);
            var classArgs = parts.Skip(1).Where(f => f.Trim().StartsWith(CTR_INDICATOR) == true);

            classTemplate = classTemplate.Replace("$ClassName$", name.Trim());
            classTemplate = classTemplate.Replace("$BaseClass$", baseClass.Trim());

            var interfaceString = string.Join(",", interfaces).Trim();
            if (string.IsNullOrEmpty(interfaceString) == false)
            {
                interfaceString = ", " + interfaceString;
            }

            classTemplate = classTemplate.Replace("$Interfaces$", interfaceString);

            var ctorArgs = GetConstructorArgs(classArgs);
            classTemplate = classTemplate.Replace("$ClassArgs$", ctorArgs);

            return classTemplate;
        }

        private string GetConstructorArgs(IEnumerable<string> args)
        {
            var result = new List<string>();
            foreach (var arg in args)
            {
                var argParts = (arg.Contains(":") ?
                                    arg.Split(new string[] { ":" }, StringSplitOptions.None) :
                                    new string[] { arg, "string" });

                result.Add($"{argParts[1]} {argParts[0].Replace(CTR_INDICATOR, "")}");
            }
            return (result.Count > 0 ? string.Join(", ", result) : string.Empty);
        }

        private string BuildProperties()
        {
            var propBuilders = _builders.Where(f => f.GetType().Equals(typeof(PropertyDeclarationBuilder)));
            var result = new StringBuilder();

            foreach (var item in propBuilders)
            {
                result.Append(item.Build());
            }

            return result.ToString();
        }

        private string BuildMethods()
        {
            var methodBuilders = _builders.Where(f => f.GetType().Equals(typeof(MethodDeclarationBuilder)));
            var result = new StringBuilder();

            foreach (var item in methodBuilders)
            {
                result.Append(item.Build());
            }

            return result.ToString();
        }
    }
}
