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
        f function
        p property

        */



        public string Generate(string template)
        {
            var result = new StringBuilder();
            var lines = template.Split(Symbols.EOL, StringSplitOptions.RemoveEmptyEntries);
            //lines.Dump();

            var builders = new List<IDeclarationBuilder>();
            IDeclarationBuilder lastCb = null;

            foreach (var line in lines)
            {
                var parts = line.Trim().Split(Symbols.SPACE, 2, StringSplitOptions.RemoveEmptyEntries);
                //parts.Dump();

                if (parts.Length == 0) continue;

                var classifier = parts[0].Trim();

                switch (classifier)
                {
                    case "e":
                        if (parts.Length <= 1) break;
                        //result.AppendLine($"Enum {parts[1]}");
                        var eb = new EnumDeclarationBuilder(parts[1]);
                        builders.Add(eb);
                        break;

                    case "c":
                        if (parts.Length <= 1) break;
                        //result.AppendLine($"Class {parts[1]}");
                        lastCb = new ClassDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "i":
                        if (parts.Length <= 1) break;
                        //result.AppendLine($"Interface {parts[1]}");
                        lastCb = new InterfaceDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "s":
                        if (parts.Length <= 1) break;
                        //result.AppendLine($"Interface {parts[1]}");
                        lastCb = new StructDeclarationBuilder(parts[1]);
                        builders.Add(lastCb);
                        break;

                    case "m":
                        if (parts.Length <= 1) break;

                        //result.AppendLine($"Method {parts[1]}");
                        MethodDeclarationBuilder mb = new MethodDeclarationBuilder(parts[1]);

                        //lastCb = builders.Where(f => f.GetType().Equals(typeof(ClassDeclarationBuilder))).LastOrDefault();

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
                        //result.AppendLine($"Property {line}");
                        if (string.IsNullOrEmpty(line)) { break; }

                        var pb = new PropertyDeclarationBuilder(line);

                        //lastCb = builders.Where(f => f.GetType().Equals(typeof(ClassDeclarationBuilder))).LastOrDefault();

                        if (lastCb != null)
                        {
                            if (lastCb.GetType().Equals(typeof(InterfaceDeclarationBuilder)))
                            {
                                pb = new PropertyDeclarationBuilder(line, isInterface: true);
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
