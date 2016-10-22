%{

#include "temp_sensor_tokens.h"

void yyerror(char *s, ...);

%}

%%

"C" { return CELSIUS; }
"F" { return FAHRENHEIT; }
[-]?[0-9]+[.][0-9]+ {
    yylval.reading = atof(yytext);
    return READING;
}
[0-9]+[H] {
    yylval.hour = atoi(yytext);
    return HOUR;
}
\n     { yylineno++; return EOL; }
[ \t]  { /* ignore whitespace */ }
.      { yyerror("Mystery character %c\n", *yytext); }

%%
