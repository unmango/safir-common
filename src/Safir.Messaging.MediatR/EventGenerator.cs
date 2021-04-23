using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Safir.Messaging.MediatR
{
    [Generator]
    public class EventGenerator : ISourceGenerator
    {
        private const string Notification = @"
using MediatR;

namespace Safir.Agent.Protos.MediatR
{
    public partial class ${Event} : INotification { }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
#if DEBUG
            // place it inside obj so that source is not added to the project
            var generatedPath = Path.Combine("obj", "Debug", "Generated");
            // delete source generated by previous build
            if (Directory.Exists(generatedPath))
                Directory.Delete(generatedPath, true);
            Directory.CreateDirectory(generatedPath);
#endif
            
            if (context.SyntaxReceiver == null) return;
            
            var receiver = (SyntaxReceiver)context.SyntaxReceiver;

            foreach (var type in receiver.Types)
            {
                var name = type.Identifier.ValueText;
                var generated = Notification.Replace("${Event}", name);
#if DEBUG
                File.WriteAllText(Path.Combine(generatedPath, $"{name}.cs"), generated);
#endif
                context.AddSource($"{name}.cs", SourceText.From(generated, Encoding.UTF8));
            }
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            private readonly List<TypeDeclarationSyntax> _types = new();

            public IEnumerable<TypeDeclarationSyntax> Types => _types;
            
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is not ClassDeclarationSyntax classSyntax) return;
                if (classSyntax.BaseList == null) return;
                var events = classSyntax.BaseList.Types.Where(x =>
                        x is SimpleBaseTypeSyntax &&
                        x.Type is GenericNameSyntax { Identifier: { ValueText: "IEvent" } })
                    .Select(x => x.Type)
                    .Cast<GenericNameSyntax>()
                    .ToList();

                if (events.Count <= 0) return;
                
                _types.Add(classSyntax);
            }
        }
    }
}
