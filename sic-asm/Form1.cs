using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;
using System.Collections;

namespace sic_asm
{
    public partial class Form1 : Form
    {
        Editor edit;
        String regex_inst = "[ \t]+(ADD|AND|COMP|DIV|J|JEQ|JGT|JLT|JSUB|LDA|LDCH|LDL|LDX|MUL|OR|RD|RSUB|STA|STCH|STL|STSW|STX|SUB|TD|TIX|WD|add|and|comp|div|j|jeq|jgt|jlt|jsub|lda|ldch|ldl|ldx|mul|or|rd|rsub|sta|stch|stl|stsw|stx|sub|td|tix|wd)([ \t]+|(\r\n))";
        String regex_number = "[ \t]+(([a-fA-F0-9]+(h|H))|([0-9]+))([ \t]+|(\r\n))";
        String regex_directive = "[ \t]+(START|END|start|end)([ \t]+|(\r\n))";
        String regex_directive2 = "[ \t]+(BYTE|WORD|RESB|RESW|byte|word|resb|resw)([ \t]+|(\r\n))";
        Style blue_style;
        Style green_style;
        Style purple_style;
        Style orange_style;
        Style white_style;
        Style red_style;
        Style gray_style;
        Style yellow_style;

        SolidBrush blue_brush = new SolidBrush(Color.FromArgb(0x66, 0xD9, 0xEF));
        SolidBrush green_brush = new SolidBrush(Color.FromArgb(0xA6, 0xE2, 0x2E));
        SolidBrush purple_brush = new SolidBrush(Color.FromArgb(0xA3, 0x81, 0xFF));
        SolidBrush orange_brush = new SolidBrush(Color.FromArgb(0xFD, 0x97, 0x1F));
        SolidBrush red_brush = new SolidBrush(Color.FromArgb(0xF9, 0x26, 0x72));
        SolidBrush yellow_brush = new SolidBrush(Color.FromArgb(0xE6, 0xD8, 0x72));
        SolidBrush white_brush = new SolidBrush(Color.White);


        Boolean is_fileSave = false;
        Boolean is_fileOpen = false;
        Boolean is_fileEdited = false;
        string filenamepath = "";
        string filename="";
        string filepath = "";
        public Form1()
        {
            InitializeComponent();
            blue_style = new TextStyle(blue_brush, null, FontStyle.Bold);
            green_style = new TextStyle(green_brush, null, FontStyle.Italic);
            orange_style = new TextStyle(orange_brush, null, FontStyle.Bold);
            yellow_style = new TextStyle(yellow_brush, null, FontStyle.Bold);
            purple_style = new TextStyle(purple_brush, null, FontStyle.Bold);
            red_style = new TextStyle(red_brush, null, FontStyle.Bold);
            white_style = new TextStyle(white_brush, null, FontStyle.Italic);

            editor.BackColor = Color.FromArgb(0x27, 0x28, 0x22);
            editor.SelectionColor = Color.White;
            editor.ForeColor = Color.FromArgb(255,255,255);
            editor.CaretColor = Color.FromArgb(0xFD, 0x97, 0x1F);
            editor.CaretBlinking = false;




        } 

        private void editor_TextChanged(object sender, TextChangedEventArgs e)
        {

            //clear style of changed range
            e.ChangedRange.ClearStyle(white_style);
            //comment highlighting
            e.ChangedRange.SetStyle(blue_style, regex_inst, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(green_style, regex_number, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(red_style, regex_directive, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(blue_style, regex_directive2, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(orange_style, "[ \t]*(((c|C)'.+')|((x|X)'[a-fA-F0-9]+'))[ \t]*", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(purple_style, "[ \t]*,[ \t]*(X|x)[ \t]*", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(yellow_style, "[ \t]*(?!')[a-zA-Z0-9_]+[a-zA-Z0-9_]*(?!')[ \t]*", RegexOptions.Multiline);

            strip_label_charinfo.Text = "Lineas: " + editor.LinesCount + "  Caracteres: " + editor.Text.Length;

            is_fileEdited = true;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (filename!="" && is_fileEdited && !is_fileSave)
            {
               /*ALGORITMO PARA GUARDAR EL ARCHIVO*/
            }
            
            OpenFileDialog openfile = new OpenFileDialog();

            openfile.Filter = "ASM files (*.s)|*.s|All files (*.*)|*.*";

            openfile.ShowDialog();

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(openfile.FileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    String text = sr.ReadToEnd();
                    editor.Text = text;
                    is_fileOpen = true;
                    filenamepath = openfile.FileName;
                    filename = Path.GetFileNameWithoutExtension(filenamepath);
                    filepath = Path.GetDirectoryName(filenamepath);
                    this.Text = "SIC ASSEMBLER - " + filename;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo abrir el archivo:");
                Console.WriteLine(ex.Message);
            }
            
            
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void primeraPasadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SicAsm asm = new SicAsm();
            string program = editor.Text.ToUpper();
            
            if (editor.Text == "")
            {
                MessageBox.Show("No hay instrucciones a compilar.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            StreamReader inputStream = new StreamReader(Console.OpenStandardInput());
            AntlrInputStream input = new AntlrInputStream(program);
            SicAsmLexer lexer = new SicAsmLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            SicAsmParser parser = new SicAsmParser(tokens);
            IParseTree tree = parser.program();
            //Console.WriteLine(tree.ToStringTree(parser));
            String res = "" +  asm.Assemble(program);

            List<String> lines = new List<string>();

            for (int i = 0; i < editor.Lines.Count; i++)
            {
                lines.Add(editor.Lines[i].ToUpper());
            }

            String text = asm.SaveIntermediateFile(lines, filepath + "\\" + filename + ".lst");
            if (asm.Erros.Count != 0)
            {
                    text = "";
                for (int i = 0; i < lines.Count; i++)
                {
                    if(asm.Erros.ContainsKey(i))
                        text += "Linea " + i + ": "+(string)asm.Erros[i]+"\r\n";
                }


            }
            asm.SaveErrorFile(lines, filepath+"\\"+filename+".t");
            asm.SaveSymbolTable(filepath + "\\" + filename + ".sym");
            //if (asm.Erros.Count == 0)
                asm.SaveObjectProgram(filepath + "\\" + filename + ".obj");
            textBoxMensajes.Text = text;
            MessageBox.Show(res);

        }

    }
}
