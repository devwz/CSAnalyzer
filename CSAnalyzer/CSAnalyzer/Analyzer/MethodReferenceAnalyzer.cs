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

        public MethodReferenceAnalyzer(SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
        }

        /// <summary>
        /// Analisar referências de um método específico.
        /// </summary>
        public void Analyze(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetRoot();

            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var methodDeclaration in methodDeclarations)
            {
                string methodName = methodDeclaration.Identifier.Text;
                Console.WriteLine($"Analyzing references for method '{methodName}':\n");

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
        private void AnalyzeMethodCalls(MethodDeclarationSyntax method, string methodName)
        {
            var methodCalls = method.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var call in methodCalls)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(call);
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

                if (methodSymbol != null)
                {
                    // var calledMethod = $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name}";
                    var calledMethod = $"{methodSymbol.Name}";

                    Console.WriteLine($"- {calledMethod}()");
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
