using System;
using System.Collections.Generic;
using System.Text;

namespace PowerCode.CodeGeneration.Core
{

    public sealed class PowerCodeGenerator
    {
        /*

        //class modifiers

        @ abstract
        $ sealed

        //type specifiers

        n namespace
        c class
        e enum
        i interface
        s struct

        m method
        p property

        */

        public string Generate(string template)
        {
            var result = new StringBuilder();
            var lines = template.Split(Symbols.EOL, StringSplitOptions.RemoveEmptyEntries);

            var builders = new List<IDeclarationBuilder>();
            IDeclarationBuilder lastCb = null;
            
            string lineToParse = string.Empty;

            foreach (var line in lines)
            {
                lineToParse = line.Trim();
                if (lineToParse.StartsWith("*"))
                {
                    //Remove comment marker *
                    lineToParse = lineToParse.Substring(1).Trim();
                }

                var parts = lineToParse.Split(Symbols.SPACE, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) continue;

                var classifier = parts[0].Trim();

                switch (classifier)
                {
                    case "e":
                        if (parts.Length <= 1) break;

                        var eb = new EnumDeclarationBuilder(parts[1]);
                        builders.Add(eb);
                        break;

                    case "c":
                        if (parts.Length <= 1) break;

                        lastCb = new ClassDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "i":
                        if (parts.Length <= 1) break;

                        lastCb = new InterfaceDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "s":
                        if (parts.Length <= 1) break;

                        lastCb = new StructDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "m":
                        if (parts.Length <= 1) break;

                        MethodDeclarationBuilder mb = new MethodDeclarationBuilder(parts[1]);

                        if (lastCb != null)
                        {
                            if (lastCb.GetType().Equals(typeof(InterfaceDeclarationBuilder)))
                            {
                                mb = new MethodDeclarationBuilder(parts[1], isInterface: true);
                            }
                            lastCb.AddBuilder(mb);
                        }
                        else
                        {
                            result.AppendLine(mb.Build());
                        }

                        break;

                    default:
                        if (string.IsNullOrEmpty(lineToParse)) { break; }

                        var pb = new PropertyDeclarationBuilder(lineToParse);

                        if (lastCb != null)
                        {
                            if (lastCb.GetType().Equals(typeof(InterfaceDeclarationBuilder)))
                            {
                                pb = new PropertyDeclarationBuilder(lineToParse, isInterface: true);
                            }
                            lastCb.AddBuilder(pb);
                        }
                        else
                        {
                            result.AppendLine(pb.Build());
                        }
                        break;
                }
            }

            foreach (var item in builders)
            {
                result.AppendLine(item.Build());
            }

            //TODO: Use Roslyn to generate code from this string

            return result.ToString();
        }
    }
}
