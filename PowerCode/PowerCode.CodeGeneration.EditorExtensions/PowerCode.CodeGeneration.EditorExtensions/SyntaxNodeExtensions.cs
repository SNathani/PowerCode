
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PowerCode.CodeGeneration.EditorExtensions
{
    internal static class SyntaxNodeExtensions
    {
        public static SyntaxNode NormalizeWhitespacesSingleLineProperties(this SyntaxNode node)
        {
            return node.NormalizeWhitespace().SingleLineProperties();
        }

        public static SyntaxNode SingleLineProperties(this SyntaxNode node)
        {
            return new SingleLinePropertyRewriter().Visit(node);
        }
    }
}
