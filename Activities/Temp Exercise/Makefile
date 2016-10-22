calc: temp_lex.l temp.y
	bison --defines=temp_tokens.h -o temp.c temp.y
	flex -o temp_lex.c temp_lex.l
	gcc -o temp temp.c temp_lex.c -lfl

clean:
	rm temp temp.c temp_lex.c temp_tokens.h
