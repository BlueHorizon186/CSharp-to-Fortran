//---------------------------------------------------------
// Student Name: Ivan David Diaz Sanchez
// Student ID: A01371166
//---------------------------------------------------------

%{

#include <stdio.h>
#include <stdarg.h>

extern int yylex (void);
void yyerror(char *s, ...);
extern int yylineno;

%}

%union {
    int ival;
}

/* declare tokens */
%token ADD MUL PAR_LEFT PAR_RIGHT EOL
%token<ival> INTEGER
%type<ival> calclist exp compexp addexp multexp

%%

calclist:
    /* nothing */ { }                            /* matches at beginning of input */
    | calclist exp EOL { printf("%d\n> ", $2); } /* EOL is end of an expression */
;

exp:
    INTEGER /* default $$ = $1 */
    | PAR_LEFT compexp PAR_RIGHT { $$ = $2; }
;

compexp:
    ADD { $$ = 0; }
    | MUL { $$ = 1; }
    | ADD addexp { $$ = $2; }
    | MUL multexp { $$ = $2; }
    | ADD compexp { $$ = $2; }
    | MUL compexp { $$ = $2; }
    | ADD addexp compexp { $$ = $2 + $3; }
    | MUL multexp compexp { $$ = $2 * $3; }
;

addexp:
    INTEGER
    | addexp INTEGER { $$ = $1 + $2; }
    | PAR_LEFT ADD PAR_RIGHT { $$ = 0; }
    | PAR_LEFT MUL PAR_RIGHT { $$ = 1; }
    | PAR_LEFT ADD addexp PAR_RIGHT { $$ = $3; }
    | PAR_LEFT MUL multexp PAR_RIGHT { $$ = $3; }
    | addexp PAR_LEFT ADD PAR_RIGHT { $$ = $1; }
    | addexp PAR_LEFT MUL PAR_RIGHT { $$ = $1 + 1; }
    | addexp PAR_LEFT ADD addexp PAR_RIGHT { $$ = $1 + $4; }
    | addexp PAR_LEFT MUL multexp PAR_RIGHT { $$ = $1 + $4; }
;

multexp:
    INTEGER
    | multexp INTEGER { $$ = $1 * $2; }
    | PAR_LEFT ADD PAR_RIGHT { $$ = 0; }
    | PAR_LEFT MUL PAR_RIGHT { $$ = 1; }
    | PAR_LEFT ADD addexp PAR_RIGHT { $$ = $3; }
    | PAR_LEFT MUL multexp PAR_RIGHT { $$ = $3; }
    | multexp PAR_LEFT ADD PAR_RIGHT { $$ = 0; }
    | multexp PAR_LEFT MUL PAR_RIGHT { $$ = $1; }
    | multexp PAR_LEFT ADD addexp PAR_RIGHT { $$ = $1 * $4; }
    | multexp PAR_LEFT MUL multexp PAR_RIGHT { $$ = $1 * $4; }
;

%%

int main(int argc, char **argv) {
    printf("> ");
    yyparse();
    return 0;
}

void yyerror(char *s, ...) {
    va_list ap;
    va_start(ap, s);
    fprintf(stderr, "Line %d: ", yylineno);
    vfprintf(stderr, s, ap);
    fprintf(stderr, "\n");
}