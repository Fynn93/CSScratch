#region

using CSScratch.AST;
using CSScratch.Library;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpCompilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation;
using SyntaxTree = Microsoft.CodeAnalysis.SyntaxTree;

#endregion

namespace CSScratch;

public sealed class AstGenerator(SyntaxTree tree, CSharpCompilation compiler) : BaseGenerator(tree, compiler)
{
    public Ast GetAst()
    {
        return Visit<Ast>(Tree.GetRoot());
    }

    public override Ast VisitCompilationUnit(CompilationUnitSyntax node)
    {
        List<Statement> statements = [];
        statements.AddRange(node.Usings.Select(Visit<Include>).OfType<Include>());
        foreach (var member in node.Members)
        {
            var statement = Visit<Statement?>(member);
            if (statement == null)
                throw Logger.CompilerError($"Unhandled syntax node within {member.Kind()}:\n{member}");
            statements.Add(statement);
        }

        return new Ast(statements);
    }

    public override Include VisitUsingDirective(UsingDirectiveSyntax node)
    {
        var name = Visit<QualifiedName>(node.Name);
        return node.StaticKeyword.IsKind(SyntaxKind.StaticKeyword) ? new Include(name) : null!;
    }

    public override Name VisitPredefinedType(PredefinedTypeSyntax node)
    {
        return new IdentifierName(node.Keyword.Text);
    }

    public override Statement VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        if (!IsStatic(node) || node.Parent is not ClassDeclarationSyntax || node.Initializer == null) return new NoOp();

        var classDeclaration = (ClassDeclarationSyntax)node.Parent!;
        var initializer = Visit<Expression>(node.Initializer);
        return new ExpressionStatement(
            new Assignment(
                new MemberAccess(
                    AstUtility.CreateIdentifierName(classDeclaration),
                    AstUtility.CreateIdentifierName(node)
                ),
                initializer
            )
        );
    }

    public Statement VisitStructFieldDeclaration(FieldDeclarationSyntax node)
    {
        List<Statement> statements = [];
        statements.AddRange(
            (
                from declarator
                    in node.Declaration.Variables
                where declarator.Initializer == null
                select new Variable(
                    AstUtility.CreateIdentifierName(declarator),
                    false,
                    structEntry: true)
                )
            );

        return new Block(statements);
    }

    public override Statement VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        if (node.Parent is StructDeclarationSyntax)
            return VisitStructFieldDeclaration(node);

        if (!IsStatic(node) || node.Parent is not ClassDeclarationSyntax)
            return new NoOp();

        var classDeclaration = (ClassDeclarationSyntax)node.Parent!;

        List<Statement> statements = [];
        foreach (var declarator in node.Declaration.Variables)
        {
            if (declarator.Initializer == null) continue;

            var initializer = Visit<Expression>(declarator.Initializer);
            var type = Visit<IdentifierName>(node.Declaration.Type);
            statements.Add(new ExpressionStatement(
                new Assignment(
                    new MemberAccess(
                        AstUtility.CreateIdentifierName(classDeclaration),
                        AstUtility.CreateIdentifierName(declarator)
                    ),
                    initializer,
                    type
                )
            ));
        }

        return new Block(statements);
    }

    public override Function VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        var classDeclaration = (ClassDeclarationSyntax)node.Parent!;
        var parameterList = Visit<ParameterList>(node.ParameterList);
        var body = Visit<Block?>(node.Body);
        var attributeLists = node.AttributeLists.Select(Visit<AttributeList>).ToList();
        return GenerateConstructor(classDeclaration, parameterList, body, attributeLists);
    }

    public override Statement VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var name = AstUtility.CreateIdentifierName(node);
        var className = AstUtility.CreateIdentifierName(node.Parent!);
        var fullName = new QualifiedName(className, name, IsStatic(node) ? '.' : ':');
        var parameterList = Visit<ParameterList>(node.ParameterList);
        var returnType = AstUtility.CreateTypeRef(Visit<Name>(node.ReturnType).ToString());
        var body = Visit<Block?>(node.Body);
        var attributeLists = node.AttributeLists.Select(Visit<AttributeList>).ToList();
        var isPublic = node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));
        return new Function(fullName, false, parameterList, returnType, body, attributeLists, isPublic);
    }

    public override IdentifierName VisitThisExpression(ThisExpressionSyntax node)
    {
        return new IdentifierName(node.ToString());
    }

    public override Node? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        var name = AstUtility.CreateIdentifierName(node);
        var members = node.Members.Select(Visit<Statement>).ToList();
        return new Struct(name, members);
    }

    public override Block VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var members = node.Members.Select(Visit<Statement>).ToList();
        var className = AstUtility.CreateIdentifierName(node);
        if (className.Text == "Stage")
            members.Insert(0, new Initializer());

        var triviaList = node.GetLeadingTrivia();
        members.InsertRange(0, (
            from text in
                from trivia in triviaList
                where trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
                select trivia.ToString()
                into text
                where text.StartsWith("gs")
                select text[3..]
            select new GoboscriptCommand(text.Replace("///gs ", ""))
        ));

        return new Block(members);
    }

    public override Block VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        var members = new Block(node.Members.Select(Visit<Statement>).ToList());
        return members;
    }

    public override Block VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        var members = new Block(node.Members.Select(Visit<Statement>).ToList());
        return members;
    }

    public override Repeat VisitDoStatement(DoStatementSyntax node)
    {
        var condition = Visit<Expression>(node.Condition);
        var body = Visit<Statement>(node.Statement);
        return new Repeat(new UnaryOperator("not ", condition), body);
    }

    public override While VisitWhileStatement(WhileStatementSyntax node)
    {
        var condition = Visit<Expression>(node.Condition);
        var body = Visit<Statement>(node.Statement);
        return new While(condition, body);
    }

    public override ExpressionalIf VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        var condition = Visit<Expression>(node.Condition);
        var body = Visit<Expression>(node.WhenTrue);
        var elseBranch = Visit<Expression>(node.WhenFalse);
        return new ExpressionalIf(condition, body, elseBranch);
    }

    public override If VisitIfStatement(IfStatementSyntax node)
    {
        var condition = Visit<Expression>(node.Condition);
        var body = Visit<Statement>(node.Statement);
        var elseBranch = Visit<Statement?>(node.Else?.Statement);
        return new If(condition, body, elseBranch);
    }

    public override NumericalFor VisitForStatement(ForStatementSyntax node)
    {
        var initializer = Visit<VariableList?>(node.Declaration)?.Variables.FirstOrDefault();
        var incrementBy = Visit<Expression?>(node.Incrementors.FirstOrDefault());
        var condition = Visit<Expression?>(node.Condition);
        var body = Visit<Statement>(node.Statement);
        return new NumericalFor(initializer, incrementBy, condition, body);
    }

    public override For VisitForEachStatement(ForEachStatementSyntax node)
    {
        List<IdentifierName> names = [AstUtility.CreateIdentifierName(node)];
        var iterator = Visit<Expression>(node.Expression);
        var body = Visit<Statement>(node.Statement);
        return new For(names, iterator, body);
    }

    public override Node VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
    {
        var variableList = Visit<Statement>(node.Variable);
        if (variableList is Variable variable) variableList = new VariableList([variable]);

        var names = ((VariableList)variableList).Variables.Select(variable1 => variable1.Name).ToList();
        var iterator = Visit<Expression>(node.Expression);
        var body = Visit<Statement>(node.Statement);
        return new For(names, iterator, body);
    }

    public override Node? VisitDeclarationExpression(DeclarationExpressionSyntax node)
    {
        return Visit(node.Designation);
    }

    public override Variable VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
    {
        return new Variable(AstUtility.CreateIdentifierName(node), true);
    }

    public override VariableList VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
    {
        var variableNodes = node.Variables.Select(Visit)
            .Where(variableNode => variableNode != null)
            .Select(variableNode => variableNode!)
            .SelectMany(variableNode =>
            {
                if (variableNode is VariableList variableList) return variableList.Variables;
                return [(Variable)variableNode];
            })
            .ToList();

        return new VariableList(variableNodes);
    }

    public override TableInitializer VisitTypeOfExpression(TypeOfExpressionSyntax node)
    {
        var typeSymbol = SemanticModel.GetTypeInfo(node.Type).Type;
        if (typeSymbol == null)
        {
            Logger.CodegenError(node, "Unable to resolve type symbol of the type provided to typeof()");
            return null!;
        }

        var fullyQualifiedName = GetFullSymbolName(typeSymbol);
        var type = GetRuntimeType(node, fullyQualifiedName);
        return AstUtility.CreateTypeInfo(type);
    }

    public override Call VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        // TODO: handle null initializer
        var expression = Visit<Name>(node.Type);
        var argumentList = Visit<ArgumentList>(node.ArgumentList);
        var callee = new QualifiedName(expression, new IdentifierName("new"));
        return new Call(callee, argumentList);
    }

    public override Node VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var methodSymbolInfo = SemanticModel.GetSymbolInfo(node.Expression);
        if (methodSymbolInfo.Symbol == null &&
            methodSymbolInfo.CandidateSymbols.IsEmpty &&
            methodSymbolInfo.CandidateReason == CandidateReason.None)
            if (node.Expression is IdentifierNameSyntax identifier &&
                identifier.Identifier.IsKind(SyntaxKind.IdentifierToken))
                switch (identifier.Identifier.Text)
                {
                    case "nameof":
                        return new Literal('"' + node.ArgumentList.Arguments.First().Expression.ToString() + '"');
                }

        var callee = Visit<Expression>(node.Expression);
        if (callee is MemberAccess memberAccess)
        {
            if (methodSymbolInfo.Symbol is not null)
                memberAccess.Operator = methodSymbolInfo.Symbol!.IsStatic ? '.' : ':';
            else
                memberAccess.Operator = ':';
        }

        var argumentList = Visit<ArgumentList>(node.ArgumentList);
        return new Call(callee, argumentList);
    }

    public override ArgumentList VisitArgumentList(ArgumentListSyntax node)
    {
        var arguments = node.Arguments.Select(Visit<Argument>).ToList();
        foreach (var argument in arguments)
            if (
                argument.Expression is BinaryOperator binaryOperator &&
                !(
                    int.TryParse(binaryOperator.Left.ToString(), out _) &&
                    int.TryParse(binaryOperator.Right.ToString(), out _)
                )
            )
                argument.Expression = Utility.FixArgumentListConcatenationOperators(binaryOperator);

        return new ArgumentList(arguments);
    }

    public override Argument VisitArgument(ArgumentSyntax node)
    {
        var expression = Visit<Expression>(node.Expression);
        return new Argument(expression);
    }

    public override Variable VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
    {
        var name = Visit<IdentifierName?>(node.NameEquals?.Name);
        if (name == null)
        {
            var expressionName = SemanticModel.GetSymbolInfo(node.Expression).Symbol!.Name;
            if (expressionName.Contains('.')) expressionName = expressionName.Split('.').Last();
            name = AstUtility.CreateIdentifierName(node, expressionName);
        }

        var value = Visit<Expression?>(node.Expression);
        return new Variable(name, true, value);
    }

    public override Node VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        var name = Visit<Expression>(node.Left);
        var value = Visit<Expression>(node.Right);
        if (node.IsKind(SyntaxKind.SimpleAssignmentExpression)) return new Assignment(name, value);
        var mappedOperator = Utility.GetMappedOperator(node.OperatorToken.Text);
        return new BinaryOperator(name, mappedOperator, value);
    }

    public override TableInitializer VisitAnonymousObjectCreationExpression(
        AnonymousObjectCreationExpressionSyntax node)
    {
        List<Expression> values = [];
        List<Expression> keys = [];
        foreach (var member in node.Initializers)
        {
            var declaration = Visit<Variable>(member);
            var key = new Literal($"\"{declaration.Name}\"");
            var value = declaration.Initializer!;
            keys.Add(key);
            values.Add(value);
        }

        return new TableInitializer(values, keys);
    }

    public override Node VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        var expression = Visit<Expression>(node.Expression);
        var name = Visit<IdentifierName>(node.Name);
        var memberAccess = new MemberAccess(expression, name);
        if (node.Parent is AssignmentExpressionSyntax assignment && assignment.Left == node)
            return AstUtility.QualifiedNameFromMemberAccess(memberAccess);

        return AstUtility.DiscardVariableIfExpressionStatement(node, memberAccess, node.Parent);
    }

    public override Node VisitElementAccessExpression(ElementAccessExpressionSyntax node)
    {
        var expression = Visit<Expression>(node.Expression);
        var index = Visit<Expression>(node.ArgumentList.Arguments.First().Expression);
        var elementAccess = new ElementAccess(expression, index);
        return AstUtility.DiscardVariableIfExpressionStatement(node, elementAccess, node.Parent);
    }

    public override QualifiedName VisitQualifiedName(QualifiedNameSyntax node)
    {
        var left = Visit<Name>(node.Left);
        var right = Visit<IdentifierName>(node.Right);
        return new QualifiedName(left, right);
    }

    public override Name VisitIdentifierName(IdentifierNameSyntax node)
    {
        var classDeclaration = FindFirstAncestor<ClassDeclarationSyntax>(node);
        if (classDeclaration is null)
            return new IdentifierName(node.Identifier.Text);

        var className = AstUtility.CreateIdentifierName(classDeclaration!);
        var isClassMember = classDeclaration.Members.Any(member =>
                                member is not ConstructorDeclarationSyntax && TryGetName(member) == GetName(node))
                            && (node.Parent is not MemberAccessExpressionSyntax memberAccess ||
                                memberAccess.Expression is not ThisExpressionSyntax);

        var name = AstUtility.CreateIdentifierName(node);
        return isClassMember ? new QualifiedName(className, name) : name;
    }

    public override Break VisitBreakStatement(BreakStatementSyntax node)
    {
        return new Break();
    }

    public override Continue VisitContinueStatement(ContinueStatementSyntax node)
    {
        return new Continue();
    }

    public override Return VisitReturnStatement(ReturnStatementSyntax node)
    {
        return new Return(Visit<Expression?>(node.Expression));
    }

    public override Block VisitBlock(BlockSyntax node)
    {
        return new Block(node.Statements.Select(Visit).OfType<Statement>().ToList());
    }

    public override Node VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        var left = Visit<Expression>(node.Left);
        var right = Visit<Expression>(node.Right);
        var mappedOperator = Utility.GetMappedOperator(node.OperatorToken.Text);

        return new BinaryOperator(left, mappedOperator, right);
    }

    public override Node VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
    {
        var operand = Visit<Expression>(node.Operand);
        var operandType = SemanticModel.GetTypeInfo(node.Operand).Type!;
        if (node.OperatorToken.Text == "!")
            return new TypeCast(operand, AstUtility.CreateTypeRef(operandType.Name.Replace("?", ""))!);

        var mappedOperator = Utility.GetMappedOperator(node.OperatorToken.Text);
        return new BinaryOperator(operand, mappedOperator, new Literal("1"));
    }

    public override Node? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
    {
        var operatorText = node.OperatorToken.Text;
        if (operatorText == "^")
        {
            Logger.UnsupportedError(node, "'^' unary operator", true);
            return null;
        }

        var operand = Visit<Expression>(node.Operand);
        if (operatorText == "+") return operand;

        var mappedOperator = Utility.GetMappedOperator(operatorText);

        return new UnaryOperator(mappedOperator, operand);
    }

    public override Parenthesized VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
    {
        var expression = Visit<Expression>(node.Expression);
        return new Parenthesized(expression);
    }

    public override AnonymousFunction VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        var parameterList = Visit<ParameterList?>(node.ParameterList) ?? new ParameterList([]);
        var body = node.ExpressionBody != null
            ? new Block([new ExpressionStatement(Visit<Expression>(node.ExpressionBody))])
            : Visit<Block?>(node.Block);

        return new AnonymousFunction(parameterList, body);
    }

    public override AnonymousFunction VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        var parameterList = new ParameterList([Visit<Parameter>(node.Parameter)]);
        var body = node.ExpressionBody != null
            ? new Block([new ExpressionStatement(Visit<Expression>(node.ExpressionBody))])
            : Visit<Block?>(node.Block);

        return new AnonymousFunction(parameterList, body);
    }

    public override AnonymousFunction VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
    {
        var parameterList = Visit<ParameterList?>(node.ParameterList) ?? new ParameterList([]);
        var body = node.ExpressionBody != null
            ? new Block([new ExpressionStatement(Visit<Expression>(node.ExpressionBody))])
            : Visit<Block?>(node.Block);

        return new AnonymousFunction(parameterList, body);
    }

    public override Function VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        var name = AstUtility.CreateIdentifierName(node);
        var parameterList = Visit<ParameterList?>(node.ParameterList) ?? new ParameterList([]);
        var returnType = AstUtility.CreateTypeRef(node.ReturnType);
        var body = node.ExpressionBody != null
            ? new Block([new ExpressionStatement(Visit<Expression>(node.ExpressionBody.Expression))])
            : Visit<Block?>(node.Body);

        var attributeLists = node.AttributeLists.Select(Visit<AttributeList>).ToList();
        return new Function(name, true, parameterList, returnType, body, attributeLists);
    }

    public override Parameter VisitParameter(ParameterSyntax node)
    {
        var name = AstUtility.CreateIdentifierName(node);
        var returnType = AstUtility.CreateTypeRef(node.Type);
        var initializer = Visit<Expression?>(node.Default);
        var isParams = HasSyntax(node.Modifiers, SyntaxKind.ParamsKeyword);
        return new Parameter(name, isParams, initializer, returnType);
    }

    public override Node? VisitAttribute(AttributeSyntax node)
    {
        switch (GetName(node))
        {
            case "ScratchNoWarp":
                return new ScratchNoWarpAttribute();
            case "ScratchEvent":
            {
                var type = ScratchEventType.None;
                string? extra = null!;
                switch (node.ArgumentList!.Arguments.Count)
                {
                    case 1:
                    {
                        var argument = node.ArgumentList.Arguments.First();
                        var typeName = Visit<MemberAccess>(argument.Expression);
                        Enum.TryParse(typeName.Name.Text, out type);
                        break;
                    }
                    case 2:
                    {
                        var typeName = Visit<MemberAccess>(node.ArgumentList.Arguments.First().Expression);
                        Enum.TryParse(typeName.Name.Text, out type);
                        extra = node.ArgumentList.Arguments.Last().Expression.ToString().Replace("\"", "");
                        break;
                    }
                }

                return new ScratchEventAttribute(type, extra!);

            }
        }

        Logger.UnsupportedError(node, "Non-builtin attributes");
        return null;
    }

    public override AttributeList VisitAttributeList(AttributeListSyntax node)
    {
        List<BaseAttribute> attributes = [];
        attributes.AddRange(node.Attributes.Select(Visit<BaseAttribute>));
        return new AttributeList(attributes, false);
    }

    public override Statement VisitGlobalStatement(GlobalStatementSyntax node)
    {
        var luauNode = Visit<Statement>(node.Statement);
        if (HasSyntax(node.Modifiers, SyntaxKind.PublicKeyword))
        {
        }

        return luauNode;
    }

    public override ParameterList VisitParameterList(ParameterListSyntax node)
    {
        return new ParameterList(node.Parameters.Select(Visit).OfType<Parameter>().ToList());
    }

    public override Statement VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
        return Visit<Statement>(node.Declaration);
    }

    public override VariableList VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        var typeRef = AstUtility.CreateTypeRef(node.Type);
        var variables = node.Variables.Select(Visit).OfType<Variable>().ToList();
        return new VariableList(variables);
    }

    public override Variable VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        var declaration = node.Parent as VariableDeclarationSyntax;
        var initializer = node.Initializer != null ? Visit<Expression>(node.Initializer) : null;
        return new Variable(AstUtility.CreateIdentifierName(node), true, initializer,
            AstUtility.CreateTypeRef(declaration?.Type));
    }

    public override Node? VisitEqualsValueClause(EqualsValueClauseSyntax node)
    {
        return Visit(node.Value);
    }

    public override Node VisitExpressionStatement(ExpressionStatementSyntax node)
    {
        var expressionNode = Visit<Node>(node.Expression);
        if (expressionNode is Expression expression)
            return new ExpressionStatement(expression);
        return expressionNode;
    }

    public override Literal VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        var valueText = "";
        switch (node.Kind())
        {
            case SyntaxKind.StringLiteralExpression:
            case SyntaxKind.Utf8StringLiteralExpression:
            case SyntaxKind.CharacterLiteralExpression:
                valueText = $"\"{node.Token.ValueText}\"";
                break;

            case SyntaxKind.NullLiteralExpression:
                valueText = "void";
                break;

            case SyntaxKind.DefaultLiteralExpression:
                var typeSymbol = SemanticModel.GetTypeInfo(node).Type;
                if (typeSymbol == null) break;

                valueText = Utility.GetDefaultValueForType(typeSymbol.Name);
                break;

            default:
                valueText = node.Token.ValueText;
                break;
        }

        return new Literal(valueText);
    }
}