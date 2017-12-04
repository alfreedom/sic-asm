using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sic_asm
{
    class Editor
    {
        private Color editor_bk_color = Color.FromArgb(0x27, 0x28, 0x22);   // GRIS
        private Color editor_other_color = Color.FromArgb(0xF9, 0x26, 0x72);  // ROJO-ROSA
        private Color editor_number_color = Color.FromArgb(0xA6, 0xE2, 0x2E);   // VERDE
        private Color editor_inst_color = Color.FromArgb(0x66, 0xD9, 0xEF); // AZUL
        private Color editor_format_color = Color.FromArgb(0xE6, 0xD8, 0x72); // AMARILLO
        private Color editor_directive_color = Color.FromArgb(0xA3, 0x81, 0xFF); // MORADO
        private Color editor_label_color = Color.FromArgb(0xFD, 0x97, 0x1F); // NARANJA
        private Color editor_selection_color = Color.FromArgb(0xF8, 0xf8, 0xf2);
        private Color editor_selected_text = Color.White;

        RichTextBox _editor;
        
        String regex_inst = "[ \t]*?(ADD|AND|COMP|DIV|J|JEQ|JGT|JLT|JSUB|LDA|LDCH|LDL|LDX|MUL|OR|RD|RSUB|STA|STCH|STL|STSW|STX|SUB|TD|TIX|WD)(([ \t]+)|(\n))";
        String regex_number = "[ \t]+?(([0-9]+)|([a-fA-F0-9]+(h|H)))(([ \t]+)|(\n))";
        String regex_directive = "[ \t]+?(START|END|BYTE|WORD|RESB|RESW)(([ \t]+)|(\n))";

        Regex regExp_inst;
        Regex regExp_number;
        Regex regExp_directive;

        public Editor(RichTextBox tb)
        {
            _editor = tb;
            _editor.BackColor = editor_bk_color;
            _editor.SelectionColor = editor_selected_text;
            _editor.ForeColor = editor_label_color;

            regExp_inst = new Regex(regex_inst);
            regExp_directive = new Regex(regex_directive);
            regExp_number = new Regex(regex_number);
        }

        public void ColoreaEditor()
        {
           
            int index = _editor.SelectionStart;                                                
      
            

            _editor.SelectionBackColor = Color.FromArgb(0, editor_bk_color);


           

            foreach (Match match in regExp_inst.Matches(_editor.Text))
            {
                _editor.Select(match.Index, match.Length);
                _editor.SelectionColor = editor_inst_color;          
                _editor.SelectionLength = 0;
                _editor.SelectionStart = index;
            }

            foreach (Match match in regExp_number.Matches(_editor.Text))
            {
                _editor.Select( match.Index, match.Length);
                _editor.SelectionColor = editor_number_color;
                _editor.SelectionLength = 0;
                _editor.SelectionStart = index;
            }

            foreach (Match match in regExp_directive.Matches(_editor.Text))
            {
                _editor.Select(match.Index, match.Length);
                _editor.SelectionColor = editor_directive_color;
                _editor.SelectionLength = 0;
                _editor.SelectionStart = index;
            }
            

            _editor.ForeColor = editor_label_color;
            _editor.SelectionColor = editor_label_color;

        }
    }
}
