%{

#include "sexp_tokens.h"

void yyerror(char *s, ...);

%}

%%

"+" { return ADD; }
"*" { return MUL; }
"(" { return PAR_LEFT; }
")" { return PAR_RIGHT; }
[0-9]+ {
        yylval.ival = atoi(yytext);
        return INTEGER;
}
\n     { yylineno++; return EOL; }
[ \t]  { /* ignore whitespace */ }
.      { yyerror("Mystery character %c\n", *yytext); }

%%
