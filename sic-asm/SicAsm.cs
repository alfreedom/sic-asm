using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.IO;
using System.Windows.Forms;

namespace sic_asm
{
    
    class SicAsm : SicAsmBaseVisitor<string>
    {
        private Hashtable _symtab;
        private Hashtable _lines_pc;
        private Hashtable _lines_opcode;
        private Hashtable _opcodes;
        private Hashtable _error_list;
        private List<String> _obj_registers;
        private int _line_count;
        private int _program_counter;
        private Boolean _is_first_pass;
        private string _prog_name;
        private int _prog_size;
        private const int MAX_ADDR = 0x7FFF;
        private int _first_inst_addr;                                                     

        /// <summary>
        /// Devuelve la tabla de símbolos generada en la primera pasada.
        /// </summary>
        public Hashtable SymTab
        {
            get { return _symtab; }
        }

        /// <summary>
        /// Devuelve la lista de errores del archivo ASM.
        /// La lista de errores depende de la pasada que se haga.
        /// Después de la primera pasada, este archivo contiene los errores de
        /// sintaxis y de simbolos duplicados.
        /// Despues de la segunda pasada devuelve los errores logicos de etiquetas.
        /// </summary>
        public Hashtable Erros
        {
            get { return _error_list; }
        }

        /// <summary>
        /// Ensambla un 
        /// </summary>
        /// <param name="tree"></param>
        public String Assemble(String ProgramText)
        {
            //string res_start = Visit(context.start());
            _symtab = new Hashtable();
            _opcodes = new Hashtable();
            _error_list = new Hashtable();
            _lines_pc = new Hashtable();
            _lines_opcode = new Hashtable();
            _obj_registers = new List<string>();

            _opcodes.Add("ADD", "18");
            _opcodes.Add("AND", "40");
            _opcodes.Add("COMP", "28");
            _opcodes.Add("DIV", "24");
            _opcodes.Add("J", "3C");
            _opcodes.Add("JEQ", "30");
            _opcodes.Add("JGT", "34");
            _opcodes.Add("JLT", "38");
            _opcodes.Add("JSUB", "48");
            _opcodes.Add("LDA", "00");
            _opcodes.Add("LDCH", "50");
            _opcodes.Add("LDL", "08");
            _opcodes.Add("LDX", "04");
            _opcodes.Add("MUL", "20");
            _opcodes.Add("OR", "44");
            _opcodes.Add("RD", "D8");
            _opcodes.Add("RSUB", "4C");
            _opcodes.Add("STA", "0C");
            _opcodes.Add("STCH", "54");
            _opcodes.Add("STL", "14");
            _opcodes.Add("STSW", "E8");
            _opcodes.Add("STX", "10");
            _opcodes.Add("SUB", "1C");
            _opcodes.Add("TD", "E0");
            _opcodes.Add("TIX", "2C");
            _opcodes.Add("WD", "DC");
            _line_count = 0;
            _program_counter = 0;
            _first_inst_addr = -1;

            String result = this.Pass1(ProgramText);
            if (_error_list.Count != 0)
                result = "Error al ensamblar el programa (Pasada 1)\r\n";
               

            Pass2(ProgramText);
            if (_error_list.Count == 0)
                result = "Programa ensamblado sin errores.";
            else
                result += "Error al ensamblar el programa (Pasada 2)";

            return result;
        }

        /// <summary>
        /// Realiza el paso 1 del ensamblado, genera el archivo de errores
        /// y la tabla de simbolos.
        /// </summary>
        /// <param name="tree">Arbol de análisis sintáctico generado con ANTLR</param>
        /// <returns></returns>
        public string Pass1(String ProgramText)
        {
            StreamReader inputStream = new StreamReader(Console.OpenStandardInput());
            AntlrInputStream input = new AntlrInputStream(ProgramText);
            SicAsmLexer lexer = new SicAsmLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            SicAsmParser parser = new SicAsmParser(tokens);
            IParseTree tree = parser.program();

            // Bandera indica primera pasada.
            _is_first_pass = true;
            _line_count = 0;
            string result = this.Visit(tree);

            if (!result.Contains("Error"))
                result = "OK! Primera pasada completa";

            return result;
        }

        public string Pass2(String ProgramText)
        {
            StreamReader inputStream = new StreamReader(Console.OpenStandardInput());
            AntlrInputStream input = new AntlrInputStream(ProgramText);
            SicAsmLexer lexer = new SicAsmLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            SicAsmParser parser = new SicAsmParser(tokens);
            IParseTree tree = parser.program();

            // Bandera indica segunda pasada.
            _is_first_pass = false;
            _line_count = 0;

            string result = this.Visit(tree);

            if (!result.Contains("Error"))
                result = "OK! Segunda pasada completa";

            return result;
        }

        /// <summary>
        /// Guarda los errores de la primera pasada en un archivo destino.
        /// </summary>
        /// <param name="lines">Arreglo de lineas que conforman el archivo.</param>
        /// <param name="filename">Nombre del archivo de salida.</param>
        /// <returns>Devuelve 1 si se creo correctamente el archivo, de lo contrario 0.</returns>
        public int  SaveErrorFile(List<String> lines, string filename)
        {
            StreamWriter file = new StreamWriter(filename);
            int msg_column = 40;
            if (file == null)
                return 0;
            for (int i = 0; i < lines.Count; i++)
            {
                string line = String.Format("{0:0000}:  ", i + 1) + lines[i];
                int res = msg_column - line.Length;
                for (int j = 0; j < res; j++)
                    line += " ";

                if (_error_list.ContainsKey(i + 1))
                    file.WriteLine(line+ _error_list[i+1]);
                else
                    file.WriteLine(line+"OK!!");

            }
            file.Close();
            return 1;
        }

        public string SaveIntermediateFile(List<String> lines, string filename)
        {
            StreamWriter file = new StreamWriter(filename);
            string ret = "";
            if (file == null)
                return ret;

           int msg_column = 50;

            ret += "PROGRAMA: " + _prog_name + "\r\nTAMAÑO: (HEX)=" + _prog_size.ToString("X4") + " (DEC)=" + _prog_size + "  bytes"+"\r\n";
            ret += "-----------------------------------------------------------------" + "\r\n";
            ret += "Linea    CP(hex)    Inst\t\t\t  Código Objeto" + "\r\n";
            ret += "-----------------------------------------------------------------" + "\r\n";

            file.WriteLine("PROGRAMA: "+ _prog_name +"\r\nTAMAÑO: (HEX)="+_prog_size.ToString("X4")+" (DEC)="+_prog_size+"  bytes" );
            file.WriteLine("--------------------------------------------------------------");
            file.WriteLine("Linea    CP(hex)    Inst\t\t\t\t\t\tCódigo Objeto");
            file.WriteLine("--------------------------------------------------------------");
            for (int i = 0; i < lines.Count; i++)
            {
                String line;
                if(_lines_pc.Contains(i+1))
                    line = String.Format("{0:0000}:    ", i + 1)+ ((int)_lines_pc[i + 1]).ToString("X4")+"     " + lines[i];
                else
                    line = String.Format("{0:0000}:    ", i + 1)+ "           " + lines[i]; 
                int res = msg_column - line.Length;
                for (int j = 0; j < res; j++)
                    line += " ";

                if (_error_list.ContainsKey(i + 1))
                {
                    file.WriteLine(line + _error_list[i + 1]);
                    ret += "Error en la linea " + line + _error_list[i + 1] + "\r\n";

                }
                else
                { 
                    file.WriteLine(line + (string)_lines_opcode[i+1]);
                    ret += line + (string)_lines_opcode[i + 1] + "\r\n";

                }

            }

            file.WriteLine("\r\n\r\nTABLA DE SIMBOLOS\r\n---------------------------\r\nSimbolo\t\tDirección (hex)\r\n---------------------------");
            ret += "\r\n\r\nTABLA DE SIMBOLOS\r\n--------------------------------\r\nSimbolo\t\tDirección (hex)\r\n--------------------------------" + "\r\n";

            foreach (object key in _symtab.Keys)
            {
                string line = (string)key + "\t\t    " + ((int)_symtab[key]).ToString("X4");
                file.WriteLine(line);
                ret +=  line + "\r\n";

            }
            file.Close();
            return ret;
        }

        public string SaveSymbolTable(String filename)
        {

            StreamWriter file = new StreamWriter(filename);
            string ret = "";
            if (file == null)
                return ret;

            ret += "Simbolo\t\tDirección (hex)\r\n---------------------------\r\n";
            file.WriteLine("Simbolo\t\tDirección (hex)\r\n---------------------------");
            foreach (object key in _symtab.Keys)
            {
                string line = (string)key + "\t\t    " + ((int)_symtab[key]).ToString("X4");
                ret += line+"\r\n";
                file.WriteLine(line);
            }
            file.Close();
            return ret;
        }

        public string SaveObjectProgram(String filename)
        {
            string ret = "";
            int reg_count=0;
            const int reg_size = 60;
            string reg="";
            string addr="";
            bool is_new_reg = true;
            for (int i = 0; i < 6; i++)
            {
                if (i < _prog_name.Length)
                    ret += _prog_name[i];
                else
                    ret += "0";
            }

            _obj_registers.Add("H" + ret + _program_counter.ToString("X6") + _prog_size.ToString("X6"));

            for (int i = 0; i < _line_count; i++)
            {
                if (_lines_opcode.ContainsKey(i + 1))
                {
                    

                    string s =(string)_lines_opcode[i + 1];

                    if (s == "------")
                    {
                        if(!is_new_reg)
                        {
                            _obj_registers.Add("T" + addr + (reg_count/2).ToString("X2") + reg);
                            reg_count = 0;
                            reg = "";
                            addr = "000000";
                            is_new_reg = true;
                        }

                    }
                    else
                    {
                        if (is_new_reg)
                        {
                            is_new_reg = false;
                            addr = "" + ((int)_lines_pc[i + 1]).ToString("X6");
                        }
                        /* si el codigo de operacion cabe en el registro*/
                        if (reg_size - reg_count >= s.Length)
                        {
                            reg += s;
                            reg_count += s.Length;
                        }
                        else
                        {
                            _obj_registers.Add("T" + addr+(reg_count/2).ToString("X2")+reg);
                            reg_count = s.Length;
                            reg = s;
                            addr =""+((int)_lines_pc[i + 1]).ToString("X6");
                        }
                    }
                }
            }
            _obj_registers.Add("E" + _first_inst_addr.ToString("X6"));

            StreamWriter file = new StreamWriter(filename);
            if (file == null)
                return "";

            foreach (String register in _obj_registers)
            {
                file.WriteLine(register);
            }
            file.Close();
            return ret;
        }
        /* Funciones para las proposiciones de la gramática */
        public override string VisitDoPrograma([NotNull] SicAsmParser.DoProgramaContext context)
        {
            
            string result="Ok";
            bool has_start = false;

            if(context.vacio() != null)
                result = Visit(context.vacio());

            result = Visit(context.start());

            _line_count++;
            if(!_lines_pc.ContainsKey(_line_count))
                _lines_pc.Add(_line_count, _program_counter);

            if (result == null)
                result = "Error: No se encontró la directiva START.";
            else
                has_start = true;

            if (result.Contains("Error") && !_error_list.Contains(_line_count))
                _error_list.Add(_line_count, result);

            result = Visit(context.instrs());

            result = Visit(context.end());
            if(result!=null)
            _line_count++;
            if (result == null)
                result = "Error: No se encontró la directiva END.";

            if (result.Contains("Error"))
            {
                if (_error_list.ContainsKey(_line_count))
                    _error_list.Remove(_line_count);
                else
                    _line_count++;

                if (!has_start)
                    _line_count--;

                _error_list.Add(_line_count, result);
            }
            return result;
        }
        public override string VisitVacio1([NotNull] SicAsmParser.Vacio1Context context)
        {
            String result=Visit(context.vacio());
            _line_count++;                           
            return "";
        }
        public override string VisitVacio2([NotNull] SicAsmParser.Vacio2Context context)
        {
            _line_count++;
            return "";
        }
        public override string VisitDirectivaInicio([NotNull] SicAsmParser.DirectivaInicioContext context)
        {
            string label = context.LABEL().GetText();
            int addr_value = 0;

            if (context.INT() != null)
            {
                addr_value = int.Parse(context.INT().GetText());
            }
            if (context.HEX() != null)
            {
                string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                addr_value = int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
            }

            if (_is_first_pass)
            {
                _prog_name = label;
                _prog_size = addr_value;
            }


            if (addr_value <= MAX_ADDR)
                _program_counter = addr_value;
            else
                return "Error, dirección fuera de rango, debe ser menor o igual a 0x7FFF";
            
            
            /* cehcar si la direccion es menor a 0x7FFF*/

            return label + addr_value;
        }
        public override string VisitDirectivaFin([NotNull] SicAsmParser.DirectivaFinContext context)
        {
            string label="----";
            string id;
            if (context.LABEL(0) != null)
                label = "Label = " + context.LABEL(0).GetText() + " START addr = ";

            int addr_value = 0;
            if (context.num_type != null)
            {
                if (context.num_type.Type == SicAsmParser.LABEL)
                {
                    if (context.LABEL().Length == 2)
                        id = context.LABEL(1).GetText();
                    else
                    {
                        label = "----";
                        id = context.LABEL(0).GetText();

                    }
                    
                    // Si la etiqueta existe en la tabla de simbolos, obtiene su dirección.
                    if (_symtab.ContainsKey(id))
                    {
                        addr_value = (int)_symtab[id];
                    }
                    else // si no existe en la tabla de simbolos, la dirección es -1.
                        addr_value = MAX_ADDR;
                }
                else
                if (context.num_type.Type == SicAsmParser.INT)
                {
                    id = context.INT().GetText();
                    addr_value = int.Parse(id);
                }
                else
                {
                    id = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    addr_value = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

                }

                _first_inst_addr = addr_value;
            }

            if (addr_value > MAX_ADDR)
                return "Error, dirección fuera de rango, debe ser menor a 0x7FFF";

            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                _prog_size = _program_counter - _prog_size;
            }
            else /* Si es la segunda pasada agrega el registro E al programa objeto */
            {
                _lines_opcode.Add(_line_count + 1, "------"); 
            }

            return label + addr_value;
        }
        public override string VisitInstrucciones1([NotNull] SicAsmParser.Instrucciones1Context context)
        {
            string result = Visit(context.instrs());
            result = Visit(context.prop());


            _line_count++;

            if (result == null)
                result = "Error: Código de operación no válido o inexistente.";

            if (result.Contains("Error") && !_error_list.Contains(_line_count))
                _error_list.Add(_line_count, result);


            return "";
        }
        public override string VisitInstrucciones2([NotNull] SicAsmParser.Instrucciones2Context context)
        {
            string result = Visit(context.prop());
            if (result == null)
                result = "Error: Código de operación no válido o inexistente.";

            _line_count++;                                                      

            if (result.Contains("Error") && !_error_list.Contains(_line_count))
                _error_list.Add(_line_count, result);
            return result;
        }
        public override string VisitNuevaLinea([NotNull] SicAsmParser.NuevaLineaContext context)
        {
            // Si es segunda pasada
            if(!_is_first_pass)
                _lines_opcode.Add(_line_count + 1, "");
            return "";
        }
        public override string VisitPropInstruccion([NotNull] SicAsmParser.PropInstruccionContext context)
        {
            string result = Visit(context.inst());
            if (result == null)
                result =  "Error: Error de sintaxis, instruccion no válida";
            return result;
        }

       
        /* Coincidencias con las instrucciones */
        public override string VisitInstruccion([NotNull] SicAsmParser.InstruccionContext context)
        {
            string addr_mode = "D";
            string label = "----";
            string inst = "";
            string id = "";
            int cp = _program_counter;
            int addr_value = 0;
            string opcode;
            inst = context.opcode.Text;
            opcode = (string)_opcodes[inst];

            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            switch (context.num_type.Type)
            {
                case SicAsmParser.LABEL:
                    id = context.LABEL().GetText();

                    /*Si es segunda pasada...*/
                    if (!_is_first_pass)
                    {
                        if (_symtab.ContainsKey(id))
                        {
                            addr_value = (int)_symtab[id];
                        }
                        else
                        {
                            addr_value = MAX_ADDR;
                            
                            _lines_opcode.Add(_line_count+1, opcode+addr_value.ToString("X4"));
                            return "Error: símbolo \"" + id + "\" No definido";
                        }
                    }

                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    id = "" + int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
                    addr_value = int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();
                    addr_value = int.Parse(id);
                    
                    break;

            }

            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                _program_counter += 3;
            }
            else /* Si es la segunda pasada */
            {
                if (addr_value > MAX_ADDR)
                {
                    addr_value = 0x7FFF;
                    _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                    return "Error, dirección fuera de rango, debe ser menor o igual a 0x7FFF";
                }
                _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));

            }

            return "PC= "+ cp+ " LABEL=" + label + " OPCODE=" + inst + " ID=" + id + " M:" + addr_mode;

        }

        public override string VisitInstruccionCompleta([NotNull] SicAsmParser.InstruccionCompletaContext context)
        {
            string addr_mode = "D";
            string label = "----";
            string inst = "";
            string id = "";
            int cp = _program_counter;
            int addr_value =0;
            String opcode;

            label = context.LABEL(0).GetText();
            inst = context.opcode.Text;
            opcode = (string)_opcodes[inst];

            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            switch (context.num_type.Type)
            {
                case SicAsmParser.LABEL:
                    id = context.LABEL(1).GetText();

                    /*Si es segunda pasada...*/
                    if (!_is_first_pass)
                    {
                        if (_symtab.ContainsKey(id))
                        {
                            addr_value = (int)_symtab[id];
                        }
                        else
                        {
                            addr_value = MAX_ADDR;

                            _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                            return "Error: símbolo \"" + id + "\" No definido";
                        }
                    }

                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    id = "" + int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
                    addr_value = int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);  
                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();
                    addr_value = int.Parse(id);
                    break;

            }

            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                if (!_symtab.ContainsKey(label))
                {
                    _symtab.Add(label, _program_counter);
                }
                else
                {
                    _program_counter += 3;
                    return "Error: el símbolo \"" + label + "\" ya está definido.";
                }
                _program_counter += 3;

            }
            else
            {
                if (addr_value > MAX_ADDR)
                {
                    addr_value = 0x7FFF;
                    _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                    return "Error, dirección fuera de rango, debe ser menor o igual a 0x7FFF";
                }
                _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
            }
            return "PC= "+cp +" LABEL=" + label + " OPCODE=" + inst + " ID=" + id + " M:" + addr_mode;
        }

        public override string VisitInstruccionIndexada([NotNull] SicAsmParser.InstruccionIndexadaContext context)
        {
            string addr_mode = "I";
            string label = "----";
            string inst = "";
            string id = "";
            int addr_value = 0;
            int cp = _program_counter;
            string opcode;

            inst = context.opcode.Text;
            opcode = (string)_opcodes[inst];

            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            switch (context.num_type.Type)
            {
                case SicAsmParser.LABEL:
                    id = context.LABEL().GetText();

                    /*Si es segunda pasada...*/
                    if (!_is_first_pass)
                    {
                        if (_symtab.ContainsKey(id))
                        {
                            addr_value = (int)_symtab[id];
                        }
                        else
                        {
                            addr_value = MAX_ADDR;
                            _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                            return "Error: símbolo \"" + id + "\" No definido";
                        }
                    }

                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    id = "" + int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
                    addr_value = int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);  
                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();     
                    addr_value = int.Parse(id);
                    break;

            }

            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                _program_counter += 3;
            }
            else /* Si es la segunda pasada */
            {
                if (addr_value > MAX_ADDR)
                {
                    addr_value = 0xFFFF;
                    _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                    return "Error, dirección fuera de rango, debe ser menor o igual a 0x7FFF";
                }
                addr_value |= 0x8000;
                _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
            }

            return "PC= " + cp + " LABEL=" + label + " OPCODE=" + inst + " ID=" + id + " M:" + addr_mode;

        }

        public override string VisitInstruccionIndexadaCompleta([NotNull] SicAsmParser.InstruccionIndexadaCompletaContext context)
        {
            string addr_mode = "I";
            string label = "----";
            string inst = "";
            string id = "";
            int addr_value = 0;
            int cp = _program_counter;
            String opcode;

            label = context.LABEL(0).GetText();
            inst = context.opcode.Text;
            opcode = (string)_opcodes[inst];

            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            switch (context.num_type.Type)
            {
                case SicAsmParser.LABEL:
                    id = context.LABEL(1).GetText();

                    /*Si es segunda pasada...*/
                    if (!_is_first_pass)
                    {
                        if (_symtab.ContainsKey(id))
                        {
                            addr_value = (int)_symtab[id];
                        }
                        else
                        {
                            addr_value = MAX_ADDR;
                            _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                            return "Error: símbolo \"" + id + "\" No definido";
                        }
                    }

                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    id = "" + int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);             
                    addr_value = int.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);
                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();                     
                    addr_value = int.Parse(id);          
                    break;

            }

            /* Si es primera pasada genera tabla de simbolos y contador de programa*/
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                if (!_symtab.ContainsKey(label))
                { 
                    _symtab.Add(label, _program_counter);
                }
                else
                {
                    _program_counter += 3;
                    return "Error: símbolo \"" + label + "\" ya está definido.";
                }
                _program_counter += 3;
            }
            else /* Si es la segunda pasada */
            {
                if (addr_value > MAX_ADDR)
                {
                    addr_value = 0xFFFF;
                    _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
                    return "Error, dirección fuera de rango, debe ser menor o igual a 0x7FFF";
                }
                addr_value |= 0x8000;
                _lines_opcode.Add(_line_count + 1, opcode + addr_value.ToString("X4"));
            }

            return "PC= " + _program_counter + " LABEL=" + label + " OPCODE=" + inst + " ID=" + id + " M:" + addr_mode;

        }

        public override string VisitInstruccionRsub([NotNull] SicAsmParser.InstruccionRsubContext context)
        {
            int cp = _program_counter;
            /* Si es primera pasada*/
            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                _program_counter += 3;
            }
            else
                _lines_opcode.Add(_line_count + 1, _opcodes["RSUB"] + 0.ToString("X4"));

            return "PC= " + cp + " LABEL=----" + " OPCODE=RSUB ID=----" + " M: Direct";

        }
              
        public override string VisitInstruccionRsubCompleta([NotNull] SicAsmParser.InstruccionRsubCompletaContext context)
        {
            int cp = _program_counter;
            string label = context.LABEL().GetText();

            if (_first_inst_addr == -1)
                _first_inst_addr = cp;

            /* Si es primera pasada*/
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                if (!_symtab.ContainsKey(label))
                {
                    _symtab.Add(label, _program_counter);
                }
                else
                {
                    _program_counter += 3;
                    return "Error: símbolo \"" + label + "\" ya está definido.";
                }
                _program_counter += 3;
            }
            else
                _lines_opcode.Add(_line_count + 1, _opcodes["RSUB"] + 0.ToString("X4"));

            return "PC= " + cp +"Etiqueta: " +label + " Instruccion: RSUB";
        }

     
        /* Coincidencias con las Directivas */
        public override string VisitDirectiva([NotNull] SicAsmParser.DirectivaContext context)
        {
            string label = "----";
            string dir = context.directive.Text;
            string id = "";
            int cp = _program_counter;
            int increment = 0;
            switch (context.num_type.Type)
            {
                case SicAsmParser.CONSTCAD:
                    id = context.CONSTCAD().GetText();
                    if(dir == "BYTE")
                    {
                        increment = id.Length-3;
                    }
                    else
                        return "Error: las constantes solo son para la directiva BYTE.";
                    id = id.Remove(0, 2);
                    id = id.Remove(id.Length-1);
                    /*Convertir a codigo ascii*/
                    string ascii = "";
                    for (int i = 0; i < id.Length; i++)
                    {
                        ascii += ((int)id[i]).ToString("X2");
                    }
                    id = ascii;
                    break;
                case SicAsmParser.CONSTHEX:
                    
                    id = context.CONSTHEX().GetText();
                    if (dir == "BYTE")
                    {
                        int n_chars = id.Length - 3;
                        increment = (n_chars + (n_chars % 2)) / 2;
                    }
                    else
                        return "Error: las constantes solo son para la directiva BYTE.";
                    id = id.Remove(0, 2);
                    id = id.Remove(id.Length - 1);
                    if (id.Length % 2 != 0)
                        id = id.Insert(0, "0");

                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();
                    uint value_int = uint.Parse(id);

                    if (dir == "BYTE")
                    {
                        if (value_int > 0xFF)
                            return "Error: El valor de la directiva BYTE esta fuera de rango.";

                        id = value_int.ToString("X2");
                        increment = 1;
                       
                    }
                    else
                    if (dir == "WORD")
                    {
                        if (value_int > 0xFFFFFF)
                            return "Error: El valor de la directiva WORD esta fuera de rango.";
                        increment = 3;
                        id = value_int.ToString("X6");

                    }
                    else
                    if (dir == "RESB")
                    {
                        if (value_int > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value_int;
                        id = "------";
                    }
                    else
                    if (dir == "RESW")
                    {
                        if (value_int * 3 > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value_int * 3;
                        id = "------";

                    }
                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    uint value = uint.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);

                    if (dir == "BYTE")
                    {
                        if (value > 0xFF)
                            return "Error: El valor de la directiva BYTE esta fuera de rango.";
                        increment = 1;
                        id = value.ToString("X2");

                    }
                    else
                    if (dir == "WORD")
                    {
                        if (value > 0xFFFFFF)
                            return "Error: El valor de la directiva WORD esta fuera de rango.";
                        increment = 3;
                        id = value.ToString("X6");

                    }
                    else
                    if (dir == "RESB")
                    {
                         if(value > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value;
                        id = "-----";
                    }
                    else
                    if (dir == "RESW")
                    {
                        if ((value*3) > 0x7FFF)
                            return "Error: El valor de la directiva RESW esta fuera de rango.";
                        increment = (int)value*3;
                        id = "-----";

                    }
                    break;
            }

            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                _program_counter += increment;
            }
            else /* Si es la segunda pasada */
            {
                _lines_opcode.Add(_line_count + 1, id);

            }
            return "PC= " + cp + " LABEL=" + label + " OPCODE=" + dir + " ID=" + id;
        }

        public override string VisitDirectivaCompleta([NotNull] SicAsmParser.DirectivaCompletaContext context)
        {
            string label = "----";
            label = context.LABEL().GetText();
            string dir = context.directive.Text;
            string id = "";
            int cp = _program_counter;
            int increment = 0;
            switch (context.num_type.Type)
            {
                case SicAsmParser.CONSTCAD:
                    id = context.CONSTCAD().GetText();
                    if (dir == "BYTE")
                    {
                        increment = id.Length - 3;
                    }
                    else
                        return "Error: las constantes solo son para la directiva BYTE.";
                    id = id.Remove(0, 2);
                    id = id.Remove(id.Length - 1);
                    /*Convertir a codigo ascii*/
                    string ascii = "";
                    for (int i = 0; i < id.Length; i++)
                    {
                        ascii += ((int)id[i]).ToString("X2");
                    }
                    id = ascii;
                    break;
                case SicAsmParser.CONSTHEX:

                    id = context.CONSTHEX().GetText();
                    if (dir == "BYTE")
                    {
                        int n_chars = id.Length - 3;
                        increment = (n_chars + (n_chars % 2))/2;
                    }
                    else
                        return "Error: las constantes solo son para la directiva BYTE.";

                    id = id.Remove(0, 2);
                    id = id.Remove(id.Length - 1);
                    if (id.Length % 2 != 0)
                        id = id.Insert(0, "0");

                    break;
                case SicAsmParser.INT:
                    id = context.INT().GetText();
                    uint value_int = uint.Parse(id);

                    if (dir == "BYTE")
                    {
                        if (value_int > 0xFF)
                            return "Error: El valor de la directiva BYTE esta fuera de rango.";
                        increment = 1;
                        id = value_int.ToString("X2");

                    }
                    else
                    if (dir == "WORD")
                    {
                        if (value_int > 0xFFFFFF)
                            return "Error: El valor de la directiva WORD esta fuera de rango.";
                        increment = 3;
                        id = value_int.ToString("X6");

                    }
                    else
                    if (dir == "RESB")
                    {
                        if (value_int > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value_int;
                        id = "------";
                    }
                    else
                    if (dir == "RESW")
                    {
                        if (value_int * 3 > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value_int * 3;
                        id = "------";
                    }
                    break;
                case SicAsmParser.HEX:
                    string hex_str = context.HEX().GetText().Remove(context.HEX().GetText().Length - 1);
                    uint value = uint.Parse(hex_str, System.Globalization.NumberStyles.HexNumber);

                    if (dir == "BYTE")
                    {
                        if (value > 0xFF)
                            return "Error: El valor de la directiva BYTE esta fuera de rango.";
                        increment = 1;
                        id = value.ToString("X2");

                    }
                    else
                    if (dir == "WORD")
                    {
                        if (value > 0xFFFFFF)
                            return "Error: El valor de la directiva WORD esta fuera de rango.";
                        increment = 3;
                        id = value.ToString("X6");

                    }
                    else
                    if (dir == "RESB")
                    {
                        if (value > 0x7FFF)
                            return "Error: El valor de la directiva RESB esta fuera de rango.";
                        increment = (int)value;
                        id = "------";
                    }
                    else
                    if (dir == "RESW")
                    {
                        if ((value * 3) > 0x7FFF)
                            return "Error: El valor de la directiva RESW esta fuera de rango.";
                        increment = (int)value * 3;
                        id = "------";
                    }
                    break;
            }

            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
                if (!_symtab.ContainsKey(label))
                {
                    _symtab.Add(label, _program_counter);
                }
                else
                {
                    _program_counter += increment;
                    return "Error: símbolo \"" + label + "\" ya está definido.";
                }
                _program_counter += increment;
            }
            else /* Si es la segunda pasada */
            {
                _lines_opcode.Add(_line_count + 1, id);

            }
            return "PC= " + cp + " LABEL=" + label + " OPCODE=" + dir + " ID=" + id;
        }


        /* Manejo de errores para instrucciones y directivas */
        public override string VisitInstruccionInvalida1([NotNull] SicAsmParser.InstruccionInvalida1Context context)
        {
            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
            }
            else /* Si es la segunda pasada */
            {
                _lines_opcode.Add(_line_count + 1, "------");
            }
            return "Error: Error de sintaxis, instruccion no válida";
        }

        public override string VisitInstruccionRsubInvalida([NotNull] SicAsmParser.InstruccionRsubInvalidaContext context)
        {
            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
            }
            else /* Si es la segunda pasada */
            {
                _lines_opcode.Add(_line_count + 1, "------");
            }
            return "Error: La instruccion RSUB debe ir sola.";
        }

        public override string VisitDirectivaInvalida1([NotNull] SicAsmParser.DirectivaInvalida1Context context)
        {
            /* Si es la primera pasada */
            if (_is_first_pass)
            {
                _lines_pc.Add(_line_count + 1, _program_counter);
            }
            else /* Si es la segunda pasada */
            {
                _lines_opcode.Add(_line_count + 1, "------");
            }
            return "Error: Error de directiva, constante no válida";

        }


    }
}
