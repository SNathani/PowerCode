using System;
using System.Linq;
using System.Text;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public sealed class PropertyDeclarationBuilder : DeclarationBuilderBase
    {
        private string _accessModifier = "public";
        private bool _isInterface = false;

        public PropertyDeclarationBuilder(string template,
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
            var parts = _template.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //parts.Dump(_template);

            var result = new StringBuilder();
            foreach (var item in parts)
            {
                var tokens = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                var returnType = "string";
                var propertyName = tokens.First().Trim();
                var loopCount = 0;
                var formatName = false;

                if (propertyName.Contains(' '))
                {
                    var propParts = propertyName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (propParts.First().StartsWith("p"))
                    {
                        int.TryParse(propParts.First().Substring(1), out int loopCountInt);
                        loopCount = loopCountInt;
                        propertyName = propParts[1];
                        formatName = true;
                    }
                    else
                    {
                        //wrong syntax
                        propertyName = propertyName.Replace(' ', '_');
                    }
                }

                if (tokens.Length > 1)
                {
                    returnType = tokens[1].Trim();
                }
                else
                {
                    //infer from property name
                    var infers = Symbols.TypeInferences.Where(f => f.Value.Any(v => propertyName.Contains(v)));
                    if (infers.Count() > 0)
                    {
                        returnType = infers.First().Key;
                    }
                }

                var newPropertyName = propertyName;

                for (int i = 0; i <= loopCount; i++)
                {
                    if (formatName)
                    {
                        newPropertyName = GetFormattedName(propertyName, i);
                    }
                    result.AppendLine($"{_accessModifier} {returnType} {newPropertyName} {{get; set;}}");
                }

            }

            return result.ToString();
        }

        private string GetFormattedName(string name, int index)
        {
            var parts = name.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            var formattedIndex = index.ToString();

            if (parts.Length > 1)
            {
                var len = parts[1].Length;
                formattedIndex = string.Format("{0," + len.ToString() + ":" + parts[1] + "}", index);
            }

            if (parts[0].Contains("%") == false)
            {
                //Append at the end
                parts[0] += "%";
            }

            var result = parts[0].Replace("%", formattedIndex);
            return result;
        }
    }
}
