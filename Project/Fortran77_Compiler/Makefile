
csf77.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs \
	Node.cs SpecificNodes.cs SyntaxError.cs SemanticAnalyzer.cs \
	SemanticError.cs SymbolEntry.cs FunctionSymbolEntry.cs SymbolTable.cs Type.cs \
	CILGenerator.cs
	mcs -out:csf77.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs \
	Parser.cs Node.cs SpecificNodes.cs SyntaxError.cs SemanticAnalyzer.cs \
	SemanticError.cs SymbolEntry.cs FunctionSymbolEntry.cs SymbolTable.cs Type.cs \
	CILGenerator.cs

clean:
	rm csf77.exe
