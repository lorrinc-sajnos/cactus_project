grammar Grammar;

//Parser Rules
tags: TAG+;

codefile: (globStatement)* EOF;

globStatement
    : preprocessor_stm
	| func_dcl
	| field_dcl EOS
	| struct_dcl
	| tags
	;

preprocessor_stm: ppc_include EOS;


filepath: OP_LST ID (OP_DIV ID)* OP_GRT;
ppc_include: KYW_INCLUDE filepath; //include <file>;


ptrLvl : OP_STAR+;
type: ID ptrLvl?;

//Function declaration
func_dcl: tags? returnType funcName PARN_OPN (param (COMMA param)*)? PARN_CLS codeBody;

returnType: type;
param: type ID;
funcName: ID;

//Field/Global variable declaration
field_dcl: tags? type ID (COMMA ID)*;

//Struct declaration
struct_dcl:
	tags? KYW_STRUCT ID BODY_OPN //
	struct_body//
	BODY_CLS;
struct_body: (func_dcl | (field_dcl EOS))*;

//Code
codeBody: BODY_OPN (statement)* BODY_CLS;

statement
    : varDcl EOS
    | expression EOS
    | ifStatement
    | forLoop
    | whileLoop
    | breakStatement EOS
    | continueStatement EOS
    | returnStatement EOS
    | tags
    ;

varDcl: type ID (OP_ASG expression)?;


expression
    :   primaryExp
        | expression op_MultLvl expression
        | expression op_AddLvl expression
        | expression op_BitLvl expression
        | expression op_CompLvl expression
    ;

//Operators
op_MultLvl:
	| OP_STAR
	| OP_DIV
	| OP_REM
	;

op_AddLvl
    : OP_ADD
    | OP_SUB
    ;

op_BitLvl
    : OP_B_AND
    | OP_B_OR
    | OP_B_XOR
    | OP_LSH
    | OP_RSH
    ;

op_CompLvl
    : OP_EQL
    | OP_NEQ
    | OP_B_AND
    | OP_B_OR
    ;

primaryExp
    : primaryExpVal op_rUn?
    | op_lUn primaryExpVal
    | miscPrimeExp
    ;

op_lUn
    : UNOP_DCR
    | UNOP_INC
    | LUNOP_B_NOT
    | LUNOP_L_NOT
    | LUNOP_SIZEOF
    | LUNOP_ADDR
    ;

op_rUn:
    UNOP_DCR
    | UNOP_INC;


primaryExpVal
    : parenthsExp
    | funcCall
    | literalExp
    ;

parenthsExp: PARN_OPN expression PARN_CLS;

miscPrimeExp //Miscellanious primary expressions
    : assignment
    | alloc
    | free
    ;

alloc: KYW_ALLOC (expression| type);
free: KYW_FREE varRef;

assignment: varRef OP_ASG expression;

//Function call
funcCall: funcRef PARN_OPN funcParamVals? PARN_CLS;

funcParamVals: expression (COMMA expression)*;
funcRef: varRef;

//Literals
literalExp
    : nullLiteral
    | numLiteral
    | strLiteral
    | charLiteral
    | boolLiteral
    | varRef
    ;

nullLiteral: KYW_NULL;

//Number Literals
numLiteral: intLiteral | floatLiteral;

intLiteral: (intDecLiteral | intHexLiteral | intBinLiteral  | intOctLiteral);
intDecLiteral: SIGN? INT_STR;
intHexLiteral: SIGN? HEX_STR;
intBinLiteral: SIGN? BIN_STR;
intOctLiteral: SIGN? OCT_STR;

floatLiteral: SIGN? INT_STR? '.' INT_STR;

//String Literal
strLiteral: STRING;

charLiteral: CHAR;

boolLiteral: KYW_TRUE | KYW_FALSE;

varRef: ID ((OP_ACC | OP_REF) ID)*;//The name of the variable can be refferenced eg.: car.driver:name

ifStatement: KYW_IF PARN_OPN expression PARN_CLS codeBody elseStatement?;
elseStatement: KYW_ELSE (ifStatement | codeBody);

returnStatement: KYW_RETURN expression;
continueStatement: KYW_CONTINUE;
breakStatement: KYW_BREAK;

forLoop: KYW_FOR PARN_OPN type ID EOS expression EOS expression PARN_CLS codeBody;
whileLoop: KYW_WHILE PARN_OPN expression PARN_CLS codeBody;






// Lexer Rules



//Global Statements

//Keywords:
KYW_INCLUDE: 'include';
KYW_STRUCT: 'struct';

//Literal keywords
KYW_TRUE: 'true';
KYW_FALSE: 'false';
KYW_NULL: 'nullptr';
KYW_IF: 'if';
KYW_ELSE: 'else';
KYW_FOR: 'for';
KYW_WHILE: 'while';
KYW_RETURN: 'return';
KYW_BREAK: 'break';
KYW_CONTINUE: 'continue';
KYW_FREE: 'free';
KYW_ALLOC: 'alloc';

TAG_PRFX: '@';

//Unary operators
UNOP_INC: '++';
UNOP_DCR: '--';
LUNOP_L_NOT: '!';
LUNOP_B_NOT: '~';
LUNOP_SIZEOF: 'sizeof';
LUNOP_ADDR: '&';


OP_ASG: '=';

//Binary operators, in (sort of) precedence order
OP_STAR: '*';
OP_DIV: '/' | '÷';
OP_REM: '%';

OP_ADD: '+';
OP_SUB: '-';

OP_B_AND: '&';
OP_B_OR: '|';
OP_B_XOR: '^';

//Bitshift
OP_LSH: '<<';
OP_RSH: '>>';

//Logical operators
OP_EQL: '==';
OP_L_AND: '&&';
OP_L_OR: '||';
OP_NEQ: '!=';
OP_LST: '<';
OP_GRT: '>';

//MIsc opr.

OP_ACC: ':'; //Access operator:     Car car1;           car1:pass_c = 2;
OP_REF: '.'; //Refference operator: Car* car1 = xyz;    car1.pass_c = 2;

COMMA: ',';
BODY_OPN: '{';
BODY_CLS: '}';
PARN_OPN: '(';
PARN_CLS: ')';
SQPN_OPN: '[';
SQPN_CLS: ']';



S_QT: '\'';
D_QT: '"';

EOS: ';';


//Literals
SIGN: [+-];
fragment Digit: [0-9];

fragment HEX_PRFX: '0x';
fragment BIN_PRFX: '0b';
fragment OCT_PRFX: '0o';

INT_STR: Digit+;
HEX_STR: HEX_PRFX (Digit | [a-f] | [A-F])+;
OCT_STR: OCT_PRFX [0-7]+;
BIN_STR: BIN_PRFX [01]+;


CHAR: '\'' (~['\\\r\n] | EscapeSequence) '\'';
//String
// Ellesve innen: https://github.com/antlr/grammars-v4/blob/master/c/C.g4
fragment EscapeSequence: SimpleEscapeSequence;
fragment SimpleEscapeSequence: '\\' ['"?abfnrtv\\];


fragment ValidStringChar: ~["\\\r\n];
fragment SChar:
	ValidStringChar
	| EscapeSequence
	| '\\\n' // Added line
	| '\\\r\n'; // Added line

STRING: '"'SChar*'"';

TAG: TAG_PRFX ID;


//ID
ID: Id_start (Id_lower | Id_upper | Digit | Id_misc)*;
fragment Id_start: (Id_lower | Id_upper | '_');
fragment Id_lower: [a-z];
fragment Id_upper: [A-Z];
fragment Id_misc: [_-];

COMMENT: ('/*' .*? '*/') -> skip;       //Purposefully left in!
LINE_COMMENT: ('//' .*? '\n') -> skip;


WHITESPACE: ('\n' | ' ' | '\t')+ -> skip;
NEWLINE: ('\r'? '\n' | '\r')+ -> skip;