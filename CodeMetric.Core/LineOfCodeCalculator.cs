using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetric.Core
{
    public class LineOfCodeCalculator : CSharpSyntaxWalker
    {
        private int _counter;

        public LineOfCodeCalculator()
            : base(SyntaxWalkerDepth.Node)
        {
            
        }

        public int Calculate(SyntaxNode node)
        {
            if(node != null)
            {
                Visit(node);
            }

            return _counter;
        }

        public override void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            base.VisitCheckedStatement(node);
            _counter++;
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            base.VisitDoStatement(node);
            _counter++;
        }

        public override void VisitEmptyStatement(EmptyStatementSyntax node)
        {
            base.VisitEmptyStatement(node);
            _counter++;
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            base.VisitExpressionStatement(node);
            _counter++;
        }

        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            if(node.Body == null)
            {
                _counter++;
            }

            base.VisitAccessorDeclaration(node);
        }

        public override void VisitFixedStatement(FixedStatementSyntax node)
        {
            base.VisitFixedStatement(node);
            _counter++;
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            base.VisitForEachStatement(node);
            _counter++;
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            base.VisitForStatement(node);
            _counter++;
        }

        public override void VisitGlobalStatement(GlobalStatementSyntax node)
        {
            base.VisitGlobalStatement(node);
            _counter++;
        }

        public override void VisitGotoStatement(GotoStatementSyntax node)
        {
            base.VisitGotoStatement(node);
            _counter++;
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            base.VisitIfStatement(node);
            _counter++;
        }

        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            base.VisitInitializerExpression(node);
            _counter += node.Expressions.Count;
        }

        public override void VisitLabeledStatement(LabeledStatementSyntax node)
        {
            base.VisitLabeledStatement(node);
            _counter++;
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            base.VisitLocalDeclarationStatement(node);
            if(!node.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                _counter++;
            }
        }

        public override void VisitLockStatement(LockStatementSyntax node)
        {
            base.VisitLockStatement(node);
            _counter++;
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            base.VisitReturnStatement(node);
            if(node.Expression != null)
            {
                _counter++;
            }
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            base.VisitSwitchStatement(node);
            _counter++;
        }

        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            base.VisitThrowStatement(node);
            _counter++;
        }

        public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            base.VisitUnsafeStatement(node);
            _counter++;
        }

        public override void VisitUsingStatement(UsingStatementSyntax node)
        {
            base.VisitUsingStatement(node);
            _counter++;
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            base.VisitConstructorDeclaration(node);
            _counter++;
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            base.VisitWhileStatement(node);
            _counter++;
        }

        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            base.VisitYieldStatement(node);
            _counter++;
        }
    }
}
