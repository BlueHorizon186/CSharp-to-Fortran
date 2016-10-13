/* simplest version of calculator */
%{

#include <stdio.h>
#include <stdarg.h>

extern int yylex (void);
void yyerror(char *s, ...);
extern int yylineno;

%}

%union {
    double dval;
}

/* declare tokens */
%token ADD MUL NEG PAR_LEFT PAR_RIGHT EOL
%token<dval> NUMBER
%type<dval> calclist exp factor term

%%

calclist:
    /* nothing */ { }                        /* matches at beginning of input */
    | calclist exp EOL { printf("= %f\n", $2); } /* EOL is end of an expression */
;

exp:
    factor /* default $$ = $1 */
    | exp ADD factor { $$ = $1 + $3; }
;

factor:
    term  /* default $$ = $1 */
    | factor MUL term { $$ = $1 * $3; }
;

term:
    NUMBER /* default $$ = $1 */
    | PAR_LEFT exp PAR_RIGHT { $$ = $2; }
    | NEG term { $$ = -($2); }
;

%%

int main(int argc, char **argv) {
    yyparse();
    return 0;
}

void yyerror(char *s, ...) {
    va_list ap;
    va_start(ap, s);
    fprintf(stderr, "%d: error: ", yylineno);
    vfprintf(stderr, s, ap);
    fprintf(stderr, "\n");
}
