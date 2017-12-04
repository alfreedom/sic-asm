namespace sic_asm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ensamblarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primeraPasadaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.segundaPasadaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ensamblarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarEnSimuladorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.strip_label_charinfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.editor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxMensajes = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.ensamblarToolStripMenuItem,
            this.cargarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(583, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.guardarToolStripMenuItem,
            this.guardarComoToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // guardarComoToolStripMenuItem
            // 
            this.guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            this.guardarComoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.guardarComoToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.guardarComoToolStripMenuItem.Text = "Guardar como...";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // ensamblarToolStripMenuItem
            // 
            this.ensamblarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.primeraPasadaToolStripMenuItem,
            this.segundaPasadaToolStripMenuItem,
            this.ensamblarToolStripMenuItem1});
            this.ensamblarToolStripMenuItem.Name = "ensamblarToolStripMenuItem";
            this.ensamblarToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.ensamblarToolStripMenuItem.Text = "Ensamblador";
            // 
            // primeraPasadaToolStripMenuItem
            // 
            this.primeraPasadaToolStripMenuItem.Name = "primeraPasadaToolStripMenuItem";
            this.primeraPasadaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.primeraPasadaToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.primeraPasadaToolStripMenuItem.Text = "Primera pasada";
            this.primeraPasadaToolStripMenuItem.Click += new System.EventHandler(this.primeraPasadaToolStripMenuItem_Click);
            // 
            // segundaPasadaToolStripMenuItem
            // 
            this.segundaPasadaToolStripMenuItem.Name = "segundaPasadaToolStripMenuItem";
            this.segundaPasadaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.segundaPasadaToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.segundaPasadaToolStripMenuItem.Text = "Segunda Pasada";
            // 
            // ensamblarToolStripMenuItem1
            // 
            this.ensamblarToolStripMenuItem1.Name = "ensamblarToolStripMenuItem1";
            this.ensamblarToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.ensamblarToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.ensamblarToolStripMenuItem1.Text = "Ensamblar";
            // 
            // cargarToolStripMenuItem
            // 
            this.cargarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cargarEnSimuladorToolStripMenuItem});
            this.cargarToolStripMenuItem.Name = "cargarToolStripMenuItem";
            this.cargarToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.cargarToolStripMenuItem.Text = "Cargar";
            // 
            // cargarEnSimuladorToolStripMenuItem
            // 
            this.cargarEnSimuladorToolStripMenuItem.Name = "cargarEnSimuladorToolStripMenuItem";
            this.cargarEnSimuladorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.cargarEnSimuladorToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.cargarEnSimuladorToolStripMenuItem.Text = "Cargar en simulador";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.strip_label_charinfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 439);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(583, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // strip_label_charinfo
            // 
            this.strip_label_charinfo.Name = "strip_label_charinfo";
            this.strip_label_charinfo.Size = new System.Drawing.Size(0, 17);
            // 
            // editor
            // 
            this.editor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.editor.AutoScrollMinSize = new System.Drawing.Size(29, 36);
            this.editor.BackBrush = null;
            this.editor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.editor.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.editor.CharHeight = 16;
            this.editor.CharWidth = 9;
            this.editor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.Font = new System.Drawing.Font("Courier New", 11.25F);
            this.editor.HighlightingRangeType = FastColoredTextBoxNS.HighlightingRangeType.AllTextRange;
            this.editor.IsReplaceMode = false;
            this.editor.LineNumberColor = System.Drawing.Color.Black;
            this.editor.Location = new System.Drawing.Point(0, 0);
            this.editor.Name = "editor";
            this.editor.PaddingBackColor = System.Drawing.SystemColors.WindowFrame;
            this.editor.Paddings = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.editor.SelectionColor = System.Drawing.Color.Transparent;
            this.editor.SelectionHighlightingForLineBreaksEnabled = false;
            this.editor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editor.ServiceColors")));
            this.editor.Size = new System.Drawing.Size(583, 312);
            this.editor.TabIndex = 3;
            this.editor.TextAreaBorderColor = System.Drawing.Color.Gray;
            this.editor.VirtualSpace = true;
            this.editor.Zoom = 100;
            this.editor.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.editor_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.editor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxMensajes);
            this.splitContainer1.Size = new System.Drawing.Size(583, 415);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 4;
            // 
            // textBoxMensajes
            // 
            this.textBoxMensajes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMensajes.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMensajes.Location = new System.Drawing.Point(0, 0);
            this.textBoxMensajes.Multiline = true;
            this.textBoxMensajes.Name = "textBoxMensajes";
            this.textBoxMensajes.ReadOnly = true;
            this.textBoxMensajes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMensajes.Size = new System.Drawing.Size(583, 99);
            this.textBoxMensajes.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 461);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "Form1";
            this.Text = "SIC ASSEMBLER";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editor)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private FastColoredTextBoxNS.FastColoredTextBox editor;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ensamblarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primeraPasadaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem segundaPasadaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ensamblarToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel strip_label_charinfo;
        private System.Windows.Forms.ToolStripMenuItem cargarEnSimuladorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxMensajes;
    }
}

