﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeMetric.Core.Halstead
{
    internal sealed class Operators
    {
        public static readonly IEnumerable<SyntaxKind> All = new[]
                                                                 {
                                                                     SyntaxKind.DotToken,
                                                                     SyntaxKind.EqualsToken,
                                                                     SyntaxKind.SemicolonToken,
                                                                     SyntaxKind.PlusPlusToken,
                                                                     SyntaxKind.PlusToken,
                                                                     SyntaxKind.PlusEqualsToken,
                                                                     SyntaxKind.MinusMinusToken,
                                                                     SyntaxKind.MinusToken,
                                                                     SyntaxKind.MinusEqualsToken,
                                                                     SyntaxKind.AsteriskToken,
                                                                     SyntaxKind.AsteriskEqualsToken,
                                                                     SyntaxKind.SlashToken,
                                                                     SyntaxKind.SlashEqualsToken,
                                                                     SyntaxKind.PercentToken,
                                                                     SyntaxKind.PercentEqualsToken,
                                                                     SyntaxKind.AmpersandToken,
                                                                     SyntaxKind.BarToken,
                                                                     SyntaxKind.CaretToken,
                                                                     SyntaxKind.TildeToken,
                                                                     SyntaxKind.ExclamationToken,
                                                                     SyntaxKind.ExclamationEqualsToken,
                                                                     SyntaxKind.GreaterThanToken,
                                                                     SyntaxKind.GreaterThanEqualsToken,
                                                                     SyntaxKind.LessThanToken,
                                                                     SyntaxKind.LessThanEqualsToken
                                                                 };
    }
}
