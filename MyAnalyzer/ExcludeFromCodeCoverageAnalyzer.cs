using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MyAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
// ReSharper disable once UnusedType.Global
public class ExcludeFromCodeCoverageAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics | GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.Attribute);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var attribute = (AttributeSyntax)context.Node;

        if (attribute.Name.ToString() == nameof(ExcludeFromCodeCoverageAttribute) ||
            attribute.Name.ToString() == "ExcludeFromCodeCoverage")
        {
            var diagnostic = Diagnostic.Create(Rule, attribute.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }

    // Metadata of the analyzer
    public const string DiagnosticId = "ExcludeFromCodeCoverageAnalyzer";

    // You could use LocalizedString but it's a little more complicated for this sample
    private static readonly string Title = "Using ExcludeFromCodeCoverage";
    private static readonly string MessageFormat = "Using ExcludeFromCodeCoverage, might be a 🚩";
    private static readonly string Description = "I want to see ExcludeFromCodeCoverage as a warning";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule 
        = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
        => ImmutableArray.Create(Rule);
}