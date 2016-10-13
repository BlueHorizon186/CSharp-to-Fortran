lisp_calc:  lisp_calc_lex.l lisp_calc.y
	bison --defines=lisp_calc_tokens.h -o lisp_calc.c lisp_calc.y
	flex -o lisp_calc_lex.c lisp_calc_lex.l
	gcc -o lisp_calc lisp_calc.c lisp_calc_lex.c -lfl
	
clean: 
	rm lisp_calc lisp_calc.c lisp_calc_lex.c lisp_calc_tokens.h
