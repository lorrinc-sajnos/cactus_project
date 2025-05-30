grammar Grammar;


codefile: (fileStatement)* EOF;

//Parser Rules
tags: tag+;
tag: TAG;

fileStatement
    : (tags EOS)
	//| tags EOS
	| (fileVarDcl EOS)
	| (funcDcl)
	| (structDcl)
	//| (classDcl)
	;

preprocessor_stm: (ppc__Include);


filepath: OP_LST ID (OP_DIV ID)* OP_GRT;
ppc__Include: KYW_INCLUDE filepath; //include <file>;

type: ID | PTR_ID;

fileVarDcl: varDcl;

//Function declaration

funcDclHeader: tags? returnType funcName PARN_OPN (param (COMMA param)*)? PARN_CLS;

funcDcl: funcDclHeader(
        codeBody
        | funcLamdBody EOS
        | ppc__C_Code_Body
        | ppc__C_Func_Map EOS
    );

funcLamdBody: OP_LAMDA expression;

returnType: type;
param: type paramName;
paramName: ID;
funcName: ID;
    varName: ID;

//Field/Global variable declaration
fieldDcl: varDcl;

//Struct declaration
structName: ID;
structDcl:
	tags? KYW_STRUCT structName BODY_OPN //
	structBody//
	BODY_CLS;
	
structBody
    :(
        funcDcl
        | (fieldDcl EOS)
    )*;

//Class declaration

//Struct declaration
className: ID;
classDcl:
	tags? KYW_CLASS className (OP_ACC parentClass)? (KYW_IMPLEMENTS interfaces)? BODY_OPN //
	classBody//
	BODY_CLS;
parentClass: ID;
interface: ID;
interfaces: interface (COMMA interface)*;

classBody: (funcDcl | (fieldDcl EOS))*;

//Code
codeBody: BODY_OPN (statement)* BODY_CLS;

statement
    : tags?
    (varDcl EOS
    | expressionStatement EOS
    | forLoop
    | breakStatement EOS
    | continueStatement EOS
    | returnStatement EOS
    | free EOS
    | tags 
    );

varDcl: tags? type varDclBody (COMMA varDclBody)*;

varDclBody: varName (OP_ASG expression)?;

expressionStatement: expression;
expression
    :   primaryExp
        | expression opMultLvl expression
        | expression opAddLvl expression
        | expression opBitLvl expression
        | expression opCompLvl expression
        | expression opAssignmentLvl expression
    ;

//Operators
opMultLvl
    : OP_STAR
	| OP_DIV
	| OP_REM
	;

opAddLvl
    : OP_ADD
    | OP_SUB
    ;

opBitLvl
    : OP_B_AND
    | OP_B_OR
    | OP_B_XOR
    | OP_LSH
    | OP_RSH
    ;

opCompLvl
    : OP_EQL
    | OP_NEQ
    | OP_L_AND
    | OP_L_OR
    | OP_LST
    | OP_GRT
    | OP_GREQ
    | OP_LSEQ
    ;


opLeftUn
    : UNOP_DCR
    | UNOP_INC
    | LUNOP_B_NOT
    | LUNOP_L_NOT
    | LUNOP_SIZEOF
    | OP_B_AND //Adress operator
    | OP_STAR //Dereference operator
    | explicitCast
    ;

opRightUn
    : UNOP_DCR
    | UNOP_INC
    ;
    
    
opAssignmentLvl
    : OP_ASG
    ;
    
primaryExp
    //value primary expression
    : primaryExpVal opRightUn?
    | opLeftUn primaryExpVal
    //| memoryAccesExp
    //referrences
    | primaryExp objFuncCall
    | primaryExp objFieldRef
    //Other primary expressions
    | alloc
    ;

    //| ppc__Expression

objFuncCall: accOp funcCall;
objFieldRef: accOp fieldRef;



primaryExpVal
    : parenthsExp
    | funcCall
    | varRef
    | literalExp
    | ppc__funcCall
    ;



accOp: OP_ACC | OP_REF;
fieldRef: varRef;


parenthsExp: PARN_OPN expression PARN_CLS;

alloc: KYW_ALLOC type;
//alloc: KYW_ALLOC (expression | type);
free: KYW_FREE expression;
explicitCast: PARN_OPN type PARN_CLS;

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

    | ppc__varRef
    ;

nullLiteral: KYW_NULL;

//Number Literals

numSign: OP_ADD | OP_SUB;
numLiteral: numSign? intLiteral | floatLiteral;

intLiteral: (intDecLiteral | intHexLiteral | intBinLiteral  | intOctLiteral);
intDecLiteral: INT_STR;

intHexLiteral: HEX_STR;

intBinLiteral: BIN_STR;

intOctLiteral: OCT_STR;

//Float
floatLiteral: FLOAT_STR;



//String Literal
strLiteral: STRING;

charLiteral: CHAR;

boolLiteral: KYW_TRUE | KYW_FALSE;

varRef: ID;//The name of the variable can be refferenced eg.: car.driver:name

ifStatement: KYW_IF PARN_OPN expression PARN_CLS codeBody elseStatement?;
elseStatement: KYW_ELSE (ifStatement | codeBody);

returnStatement: KYW_RETURN expression?;
continueStatement: KYW_CONTINUE;
breakStatement: KYW_BREAK;

//FOR loop
loopDecl : varDcl;
loopCond : expression;
endExp: expression;

forLoop: KYW_FOR PARN_OPN loopDecl EOS loopCond EOS endExp PARN_CLS codeBody;


whileLoop: KYW_WHILE PARN_OPN loopCond PARN_CLS codeBody;

//Preprocessor magic

ppc__C_Code_Body: PPC_C_CODE;
ppc__C_id: ID;
ppc__C_Func: ppc__C_id PARN_OPN funcParamVals? PARN_CLS;
ppc__C_Func_Map:  PPC_C_LAMDA ppc__C_Func;

ppc__varRef: PPC_ID;
ppc__funcRef: (PPC_ID | PPC_DEEP_ID);
ppc__funcCall: ppc__funcRef PARN_OPN funcParamVals? PARN_CLS;

//ppc__Expression: ppc__Token{1,8};



// Lexer Rules



//Global Statements

//Keywords:
KYW_INCLUDE: 'include';
KYW_STRUCT: 'struct';
KYW_IMPLEMENTS: 'implements';

//class keywords:

KYW_CLASS: 'class';
KYW_STATIC: 'static';

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

OP_ASG: '=';

//Unary operators
UNOP_INC: '++';
UNOP_DCR: '--';
LUNOP_L_NOT: '!';
LUNOP_B_NOT: '~';
LUNOP_SIZEOF: 'sizeof';


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
OP_LSEQ: '<=';
OP_GREQ: '>=';

OP_LAMDA: '=>';

//Misc opr.

OP_ACC: ':'; //Access operator:     Car car1;           car1:pass_c = 2;
OP_REF: '.'; //Refference operator: Car* car1 = xyz;    car1.pass_c = 2;

COMMA: ',';
BODY_OPN: '{';
BODY_CLS: '}';
PARN_OPN: '(';
PARN_CLS: ')';
SQPN_OPN: '[';
SQPN_CLS: ']';

//Literal operators:


//Preprocessor magic
fragment PPC_PRFX: '$';
fragment PPC_DEEP_PRFX: '$$';
fragment PPC_C_PRFX: PPC_DEEP_PRFX 'C';

PPC_C_BODY_OPN: PPC_C_PRFX BODY_OPN;
PPC_C_BODY_CLS: BODY_CLS PPC_C_PRFX;
PPC_C_CODE: PPC_C_BODY_OPN (().)* PPC_C_BODY_CLS;
PPC_C_LAMDA: PPC_C_PRFX OP_LAMDA;


S_QT: '\'';
D_QT: '"';

EOS: ';';


//Literals
fragment Digit: [0-9];

fragment HEX_PRFX: '0x';
fragment BIN_PRFX: '0b';
fragment OCT_PRFX: '0o';

INT_STR: Digit+;
HEX_STR: HEX_PRFX (Digit | [a-f] | [A-F])+;
OCT_STR: OCT_PRFX [0-7]+;
BIN_STR: BIN_PRFX [01]+;

fragment FLOAT_TYPE: 'f' | 'F' | 'd' | 'D';
FLOAT_STR: INT_STR? '.' INT_STR FLOAT_TYPE?;

CHAR: '\''(~['\\\r\n] | EscapeSequence)'\'';
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
ID: Id_start Id_chars*;
PTR_ID: ID OP_STAR*; //Force pointer starts to the type

fragment Id_start: ([a-z]|[A-Z]|'_');
fragment Id_chars: Id_start | Digit;

PPC_ID: PPC_PRFX ID;
PPC_DEEP_ID: PPC_DEEP_PRFX ID;


COMMENT: ('/*' .*? '*/') -> channel(HIDDEN);       //Purposefully left in!
LINE_COMMENT: ('//' .*? '\n') -> channel(HIDDEN);


WHITESPACE: ('\n' | ' ' | '\t')+ -> channel(HIDDEN);
NEWLINE: ('\r'? '\n' | '\r')+ -> channel(HIDDEN);