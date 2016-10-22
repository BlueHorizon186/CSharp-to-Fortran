/* Temperature Sensor Readings */

%{

#include <stdio.h>
#include <stdarg.h>

extern int yylex (void);
void yyerror(char *s, ...);
extern int yylineno;

int total_readings = 0;

%}

%union {
    double reading;
    int hour;
}

/* Declare Tokens */
%token
%token<reading> READING
%token<hour> HOUR
%type<reading> temp_list
%start s

%%

s:
    /* nothing */ { }
    | temp_list {
        printf("Average temperature: %f F\n", $1/(double)total_readings);
    }
;

temp_list:
    temp_list measurement { $$ = $1 + $2; total_readings++; }
    | measurement { $$ = $1; total_readings++; }
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
