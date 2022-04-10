using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Library
{
    public class SyntaxVisitor : CSharpSyntaxVisitor
    {
        private readonly SemanticModel semanticModel;

        public SyntaxVisitor(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var sm = semanticModel.Compilation.GetSemanticModel(node.SyntaxTree);
            var symbolInfo = sm.GetSymbolInfo(node);
            var declaredSymbol = sm.GetDeclaredSymbol(node);
            var typeInfo = sm.GetTypeInfo(node);

            Console.WriteLine("VisitObjectCreationExpression " + node.Type.ToString());
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var sm = semanticModel.Compilation.GetSemanticModel(node.SyntaxTree);
            var symbolInfo = sm.GetSymbolInfo(node);
            var declaredSymbol = sm.GetDeclaredSymbol(node);
            var typeInfo = sm.GetTypeInfo(node);

            Console.WriteLine("VisitInvocationExpression " + node.ToString());
        }

        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            Console.WriteLine("VisitArrayCreationExpression " + node.Type.ToString());
        }

        public override void VisitUnaryPattern(UnaryPatternSyntax node)
        {
            Console.WriteLine(node.ToString());
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            Console.WriteLine(node.ToString());
        }

        public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            Console.WriteLine("VisitTypeOfExpression " + node.Type.ToString());
        }

        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            Console.WriteLine("VisitCastExpression " + node.Type.ToString());
        }
    }
}
