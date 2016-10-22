all: english.exe sexp

english.exe: English.cs
	mcs -out:english.exe English.cs

sexp:  sexp_lex.l sexp.y
	bison --defines=sexp_tokens.h -o sexp.c sexp.y
	flex -o sexp_lex.c sexp_lex.l
	gcc -o sexp sexp.c sexp_lex.c -lfl

clean:
	rm english.exe sexp sexp.c sexp_lex.c sexp_tokens.h
