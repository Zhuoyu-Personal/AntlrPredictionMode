grammar PredictionDemo;

prog
    : stat* EOF
    ;

stat
    : expr ';'             #ExprStatement
    | expr '=' expr ';'    #AssignStatement
    ;

expr
    : Identifier                           #IdExpr
    | Integer                              #IntExpr
    | '(' expr ')'                         #ParenExpr
    | expr '(' expr ')'                    #CallExpr
    ;

Identifier : [a-zA-Z_] [a-zA-Z0-9_]* ;
Integer    : [0-9]+ ;
WS         : [ \t\r\n]+ -> skip ;
