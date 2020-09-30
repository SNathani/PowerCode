using System.Collections.Generic;
using System.Text;

namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public abstract class DeclarationBuilderBase : IDeclarationBuilder
    {
        protected string _template;

        protected List<IDeclarationBuilder> _builders = new List<IDeclarationBuilder>();

        public DeclarationBuilderBase(string template)
        {
            _template = template;
        }

        public void AddBuilder(IDeclarationBuilder builder)
        {
            _builders.Add(builder);
        }

        public string BuildBody()
        {
            var result = new StringBuilder();
            foreach (var item in _builders)
            {
                result.AppendLine(item.Build());
            }
            return result.ToString();
        }

        public abstract string Build();
    }
}
