using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public sealed class MethodDeclarationBuilder : DeclarationBuilderBase
    {
        private string _accessModifier = "public";
        private bool _isInterface = false;

        public MethodDeclarationBuilder(string template,
                    string accessModifier = "public",
                    bool isInterface = false) : base(template)
        {
            _accessModifier = accessModifier;
            _isInterface = isInterface;

            if (_isInterface)
            {
                _accessModifier = string.Empty;
            }
        }

        public override string Build()
        {
            if (string.IsNullOrEmpty(_template))
            {
                return string.Empty;
            }

            var result = new StringBuilder();

            var parts = _template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var methodName = parts[0];
            var returnType = "void";
            var methodBody = "";

            if (methodName.Contains(":"))
            {
                var nameParts = methodName.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                methodName = nameParts[0];
                returnType = nameParts[1];
                methodBody = "return default;";
            }
            result.Append($"{_accessModifier} {returnType} {methodName}(".Trim());

            //extract parameters
            if (parts.Count() > 1)
            {
                var args = parts.Skip(1);
                var argList = new List<string>();
                foreach (var arg in args)
                {
                    var argName = arg;
                    if (argName.Contains(":"))
                    {
                        var argParts = argName.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (argParts.Count() > 1)
                        {
                            argList.Add($"{argParts[1]} {argParts[0]}");
                        }
                        else
                        {
                            argList.Add($"string {argParts[0]}");
                        }
                    }
                    else
                    {
                        argList.Add($"string {arg}");
                    }
                }
                result.Append(string.Join(", ", argList.ToArray()));
            }

            //end method signature
            result.AppendLine(")" + (_isInterface ? ";" : string.Empty));

            if (_isInterface == false)
            {   //add body if not an interface method
                result.AppendLine($"{{\r\n\t{methodBody}\r\n}}");
            }
            return result.ToString();
        }
    }
}
