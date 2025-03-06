grammar CactusGrammar;

//Parser Rules

codefile: (glob_statement)* EOF;

glob_statement:
	prepoc_statement
	| func_dcl
	| struct_dcl
	| field_dcl;

//Preproec statemens
INCLUDE_HEADER: 'include';
prepoc_statement: (INCLUDE_HEADER OP_LST FILEPATH OP_GRT SPRT); //include <file>;

//Function declaration
func_header: TAG* TYPE ID '(' (PARAM (',' PARAM)*)? ')';
func_dcl: func_header code_body;

//Field/Global variable declaration
field_dcl: TAG* TYPE ID SPRT;

//Struct declaration
struct_header: TAG* 'struct' ID;
struct_dcl:
	struct_header '{' //
	(func_dcl | field_dcl)* //
	'}';

//      Code
code_body: BODY_OPN (statement SPRT)* BODY_CLS;
statement: var_dcl | assign | expression;

var_name: ID ((OP_ACC | OP_REF) ID)*;
var_dcl: TYPE ID (OP_ASG value)?;

assign: var_name ASSIGN expression;

value: CONST | var_name | func_call | '(' expression ')';
func_call: var_name '(' (expression (',' expression)*)? ')';

expression: value (BIN_OP expression)?;
//
// Lexer Rules
fragment LowerCase: [a-z];
fragment UpperCase: [A-Z];
fragment MiscChar: [_-];
fragment IdStarChar: (LowerCase | UpperCase | '_');
fragment Ws: (' ' | '\t' | '\n');

fragment IdStr:
	IdStarChar (LowerCase | UpperCase | Digit | MiscChar)*;

ID: IdStr;

TYPE: (ID OP_STAR*);
PARAM: (TYPE ',' ID);
BODY_OPN: '{';
BODY_CLS: '}';

//Global Statements
SPRT: ';';
L_STRING: (LowerCase | UpperCase)+; //Logical string
FILEPATH: L_STRING ('/' L_STRING)*;
TAG: '@' ID;

//Constants
fragment Sign: [+-];
fragment Digit: [0-9];
fragment DigitHex: (Digit | [a-f] | [A-F]);
fragment DigitOct: ([0-7]);

CONST: (CONST_INT | CONST_FLOAT);

CONST_INT: CONST_INT_DEC | CONST_INT_HEX;
CONST_INT_DEC: Sign? Digit+;
CONST_INT_HEX: Sign? '0' [xX] DigitHex+;

CONST_FLOAT: Sign? Digit* '.' Digit+ [fF]?;

//String
// Ellesve innen: https://github.com/antlr/grammars-v4/blob/master/c/C.g4
fragment EscapeSequence: SimpleEscapeSequence;

fragment SimpleEscapeSequence: '\\' ['"?abfnrtv\\];

fragment SChar:
	~["\\\r\n]
	| EscapeSequence
	| '\\\n' // Added line
	| '\\\r\n'; // Added line

CONST_STRING: '"' SChar* '"';

//Operators
BIN_OP: VAL_OPERATOR | COMP_OPERATOR;

UNARY_OP: '++' | '--';

LUNOP: UNARY_OP;
RUNOP: UNARY_OP | OP_STAR;

VAL_OPERATOR:
	OP_ADD
	| OP_NEG
	| OP_STAR
	| OP_DIV
	| OP_AND
	| OP_OR
	| OP_XOR;

COMP_OPERATOR: OP_EQL | OP_BAND | OP_BOR | OP_NEQ | OP_NOT;
//

ASSIGN: (VAL_OPERATOR)? OP_ASG;
OP_ASG: '=';

OP_ADD: '+';
OP_NEG: '-';
OP_STAR: '*';
OP_DIV: '/' | '÷';
OP_AND: '&';
OP_OR: '|';
OP_XOR: '^';
//Bitshift
OP_LSH: '<<';
OP_RSH: '>>';
//Logical operators
OP_EQL: '==';
OP_BAND: '&&';
OP_BOR: '||';
OP_NEQ: '!=';
OP_NOT: '!';
OP_LST: '<';
OP_GRT: '>';

OP_ACC: ':'; //Access operator: car1:pass_c
OP_REF: '.'; //Refference operator: Car* car1... car1.

//Ellesve GitHubról Controll
WHITESPACE: (' ' | '\t')+ -> skip;
NEWLINE: ('\r'? '\n' | '\r')+;
COMMENT: ('/*' .*? '*/') | ('//' .*?);