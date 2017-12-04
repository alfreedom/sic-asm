grammar SicAsm;

@parser::members
{
	protected const int EOF=Eof;
}
@lexer::members
{
	protected const int EOF=Eof;
	protected const int HIDDEN=Hidden;
}

/*
 * Parser Rules
 */


program: vacio? start instrs end EOF 			#DoPrograma
       
;

vacio: vacio NEWLINE	#Vacio1
	 | NEWLINE			#Vacio2
;

start: LABEL 'START' INT NEWLINE    #DirectivaInicio
     | LABEL 'START' HEX NEWLINE    #DirectivaInicio
;
end: LABEL 'END' num_type=(INT|HEX|LABEL)? NEWLINE    #DirectivaFin
   | 'END' num_type=(INT|HEX|LABEL)?     #DirectivaFin
;
instrs: instrs prop 	#Instrucciones1
	  | prop			#Instrucciones2
;

prop: inst		#PropInstruccion
	| NEWLINE	#NuevaLinea
;

inst: opcode=('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD') num_type=(INT|HEX|LABEL) NEWLINE			#Instruccion
	| LABEL opcode=('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD') num_type=(INT|HEX|LABEL) NEWLINE	#InstruccionCompleta
	| opcode=('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD') num_type=(INT|HEX|LABEL) INDEXED NEWLINE	#InstruccionIndexada
	| LABEL opcode=('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD') num_type=(INT|HEX|LABEL) INDEXED NEWLINE	#InstruccionIndexadaCompleta
	| 'RSUB' NEWLINE	#InstruccionRsub
	| LABEL 'RSUB' NEWLINE	#InstruccionRsubCompleta
	| LABEL directive=('WORD'|'BYTE'|'RESB'|'RESW')  num_type=(INT|HEX|CONSTCAD|CONSTHEX)  NEWLINE       #DirectivaCompleta
	| directive=('WORD'|'BYTE'|'RESB'|'RESW')  num_type=(INT|HEX|CONSTCAD|CONSTHEX)  NEWLINE       #Directiva
	
	| 'RSUB' (INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE	#InstruccionRsubInvalida
	| LABEL ('WORD'|'BYTE'|'RESB'|'RESW') (INT|HEX|CONSTCAD|CONSTHEX)       #DirectivaInvalida1
	| LABEL ('WORD'|'BYTE'|'RESB'|'RESW') (INT|HEX|CONSTCAD|CONSTHEX) (INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE       #DirectivaInvalida1
	| LABEL ('WORD'|'BYTE'|'RESB'|'RESW') (LABEL|INVALID|',')+ NEWLINE       #DirectivaInvalida1  
	| ('WORD'|'BYTE'|'RESB'|'RESW') (INT|HEX|CONSTCAD|CONSTHEX)       #DirectivaInvalida1
	| ('WORD'|'BYTE'|'RESB'|'RESW') (INT|HEX|CONSTCAD|CONSTHEX) (INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE       #DirectivaInvalida1
	| ('WORD'|'BYTE'|'RESB'|'RESW') (LABEL|INVALID|',')+ NEWLINE       #DirectivaInvalida1  
	
	| LABEL ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL)+ ','+ num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE?	#InstruccionInvalida1
	| LABEL ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL)+ ',' NEWLINE?	#InstruccionInvalida1
	| ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL) INDEXED NEWLINE?	#InstruccionInvalida1
	| ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL)+ INDEXED+ num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE?	#InstruccionInvalida1
	| LABEL ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL)+ INDEXED+ num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+ NEWLINE?	#InstruccionInvalida1
	| ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL) ',' num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',') NEWLINE?	#InstruccionInvalida1
	| ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL) num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',') NEWLINE?			#InstruccionInvalida1
	| invalid=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',') opcode=('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ num_type=(INT|HEX|LABEL) num2_type=(INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',') NEWLINE?			#InstruccionInvalida1
	| LABEL ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+ NEWLINE?			#InstruccionInvalida1
	| ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD')+  (INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')* NEWLINE			#InstruccionInvalida1
	| (INT|HEX|CONSTCAD|CONSTHEX|LABEL|INVALID|',')+	NEWLINE		#InstruccionInvalida1
;	

/*
 * Lexer Rules
 */

INT: [0-9]+;
HEX: [A-F0-9]+'H';
NEMONICO: ('ADD'|'AND'|'COMP'|'DIV'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'|'LDA'|'LDCH'|'LDL'|'LDX'|'MUL'|'OR'|'RD'|'RSUB'|'STA'|'STCH'|'STL'|'STSW'|'STX'|'SUB'|'TD'|'TIX'|'WD');
CONSTCAD:'C\''[a-ZA-Z0-9_-+!#$%&/()\.;:ñ{}\"\'\\]+'\'';
CONSTHEX:'X\''[A-F0-9]+'\'';
INDEXED: (','(' '|'\t')*'X');
LABEL: [A-Z][A-Z0-9]+;
NEWLINE: ('\r\n' | '\n');
WS: (' ' | '\t'	)->channel(HIDDEN);
INVALID: [A-Z0-9_-+!#$%&/()\.;:ñ{}\"\'\\]+;