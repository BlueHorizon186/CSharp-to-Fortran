/* Lisp-like Calculator */

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
%token ADD MUL PAR_LEFT PAR_RIGHT EOL
%token<dval> NUMBER
%type<dval> lisplist expression

%%

lisplist:
    /* nothing */ { }
    | lisplist expression EOL { printf("= %f\n", $2); }
;

expression:
    NUMBER /* default $$ = $1 */
    | PAR_LEFT ADD expression expression PAR_RIGHT { $$ = $3 + $4; }
    | PAR_LEFT MUL expression expression PAR_RIGHT { $$ = $3 * $4; }
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
