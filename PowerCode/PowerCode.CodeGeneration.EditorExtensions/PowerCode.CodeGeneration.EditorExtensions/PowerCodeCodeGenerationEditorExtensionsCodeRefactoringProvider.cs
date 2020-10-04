using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

using PowerCode.CodeGeneration.Extensions;

namespace PowerCode.CodeGeneration.EditorExtensions
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(PowerCodeCodeGenerationEditorExtensionsCodeRefactoringProvider)), Shared]
    internal class PowerCodeCodeGenerationEditorExtensionsCodeRefactoringProvider : CodeRefactoringProvider
    {
        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            // TODO: Replace the following code with your own analysis, generating a CodeAction for each refactoring to offer

            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // Find the node at the selection.
            var trivia = root.FindTrivia(context.Span.Start);

            if (trivia == null)
            {
                return;
            }

            if (!IsCommentTrivia(trivia))
            {
                return;
            }

            //Code action for the refactoring
            var action = CodeAction.Create("Generate from PowerCode", c => InsertCodeFromPowerCodeAsync(context, trivia, c));

            // Register this code action.
            context.RegisterRefactoring(action);
        }
        private static bool IsCommentTrivia(SyntaxTrivia trivia)
        {
            return (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia ||
                            trivia.Kind() == SyntaxKind.SingleLineCommentTrivia);
        }

        private async Task<Document> InsertCodeFromPowerCodeAsync(CodeRefactoringContext context, SyntaxTrivia trivia, CancellationToken cancellationToken)
        {
            var newDocument = context.Document;
            try
            {
                var commentText = GetCommentText(trivia);
                var generatedCode = GetGeneratedCode(commentText);
                var csTree = SyntaxFactory.ParseSyntaxTree(generatedCode);
                var newNode = csTree.GetRoot().NormalizeWhitespacesSingleLineProperties();

                //Insert the generatedCode after this node
                var root = await context.Document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

                var newTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.MultiLineCommentTrivia,
                                        //Add original comment + newly generated code underneath it
                                        trivia.ToString() + "\r\n" + newNode.ToString());

                var newRoot = root.ReplaceTrivia(trivia, newTrivia);

                newDocument = newDocument.WithSyntaxRoot(newRoot);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return await Task.FromResult(newDocument);
        }

        private static string GetGeneratedCode(string powerCode)
        {
            var generatedCode = powerCode.ExecutePowerCode();
            return generatedCode;
        }

        private string GetCommentText(SyntaxTrivia trivia)
        {
            var result = string.Empty;

            var commentText = trivia.ToString();

            switch (trivia.Kind())
            {
                case SyntaxKind.SingleLineCommentTrivia:
                    result = commentText.TrimStart('/');
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                    result = commentText.Substring(2, commentText.Length - 4);
                    break;
            }

            return result.Trim();
        }
    }
}
