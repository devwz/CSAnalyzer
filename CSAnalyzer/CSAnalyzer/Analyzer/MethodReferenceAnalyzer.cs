using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSAnalyzer.Analyzer
{
    public class MethodReferenceAnalyzer
    {
        private readonly SemanticModel _semanticModel;
        private readonly DgmlExporter _dgmlExporter;

        public MethodReferenceAnalyzer(SemanticModel semanticModel, DgmlExporter dgmlExporter)
        {
            _semanticModel = semanticModel;
            _dgmlExporter = dgmlExporter;
        }

        /// <summary>
        /// Analisar referências de um método específico.
        /// </summary>
        public void Analyze(SyntaxTree syntaxTree, List<string> nodeList)
        {
            var root = syntaxTree.GetRoot();

            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var methodDeclaration in methodDeclarations)
            {
                // string methodName = methodDeclaration.Identifier.Text;
                var methodSymbol = (IMethodSymbol)_semanticModel.GetDeclaredSymbol(methodDeclaration);
                string methodName = $"{methodSymbol.ContainingType}.{methodSymbol.Name}";

                nodeList.Add(methodName + "()");

                Console.WriteLine($"\n\nAnalyzing references for method '{methodName}':\n");

                // Adicionar o método como nó
                _dgmlExporter.AddNode(methodName);

                // Identificar quem o método chama
                Console.WriteLine("Methods called by this method:");
                AnalyzeMethodCalls(methodDeclaration, methodName);

                // Identificar quem chama o método
                Console.WriteLine("\nMethods that call this method:");
                AnalyzeMethodUsages(root, methodName);
            }
        }

        /// <summary>
        /// Identifica métodos que são chamados pelo método atual.
        /// </summary>
        private void AnalyzeMethodCalls(MethodDeclarationSyntax method, string callerMethod)
        {
            var methodCalls = method.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var call in methodCalls)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(call);
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

                if (methodSymbol != null)
                {
                    var calledMethod = $"{methodSymbol.ContainingType}.{methodSymbol.Name}";
                    // var calledMethod = $"{methodSymbol.Name}";

                    // Verifica se calledMethod pertence a uma lib nativa do C#
                    // ToDo: refatorar
                    var namespaceName = methodSymbol.ContainingNamespace.ToDisplayString();
                    var assemblyName = methodSymbol.ContainingAssembly.Name;

                    var isNativeLibrary = assemblyName == "mscorlib" ||
                          assemblyName == "System.Private.CoreLib" ||
                          assemblyName.StartsWith("System") ||
                          assemblyName.StartsWith("Microsoft") ||
                          assemblyName.StartsWith("Dapper");

                    if (!isNativeLibrary)
                    {
                        Console.WriteLine($"- {calledMethod}()");

                        // Adicionar nó e aresta no grafo
                        _dgmlExporter.AddNode(calledMethod);
                        _dgmlExporter.AddEdge(callerMethod, calledMethod);
                    }
                }
            }
        }

        /// <summary>
        /// Identifica quem está chamando o método atual em todo o código.
        /// </summary>
        private void AnalyzeMethodUsages(SyntaxNode root, string methodName)
        {
            var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var invocation in invocations)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(invocation);
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

                if (methodSymbol != null && methodSymbol.Name == methodName)
                {
                    var location = invocation.GetLocation();
                    Console.WriteLine($"- Called at {location.GetLineSpan().StartLinePosition}");
                }
            }
        }
    }
}
