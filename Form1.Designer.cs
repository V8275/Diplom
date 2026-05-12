namespace WinFormsOpenTK
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _glControl = new OpenTK.GLControl.GLControl();
            panel1 = new Panel();
            LightIntense = new NumericUpDown();
            button2 = new Button();
            button1 = new Button();
            panel4 = new Panel();
            checkBox4 = new CheckBox();
            label9 = new Label();
            massText = new TextBox();
            checkBox3 = new CheckBox();
            checkBox1 = new CheckBox();
            panel3 = new Panel();
            label5 = new Label();
            xScale = new TextBox();
            label6 = new Label();
            yScale = new TextBox();
            label7 = new Label();
            zScale = new TextBox();
            label8 = new Label();
            panel2 = new Panel();
            label4 = new Label();
            xCoord = new TextBox();
            label3 = new Label();
            yCoord = new TextBox();
            label2 = new Label();
            zCoord = new TextBox();
            label1 = new Label();
            LoadButton = new Button();
            fPSCounter = new Label();
            LoadPreset = new Label();
            comboBox1 = new ComboBox();
            DeleteModelsBtn = new Button();
            listBox1 = new ListBox();
            DeleteDBData = new Button();
            SaveDB = new Button();
            dataGridViewModels = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LightIntense).BeginInit();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewModels).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // _glControl
            // 
            _glControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            _glControl.APIVersion = new Version(3, 3, 0, 0);
            _glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            _glControl.IsEventDriven = true;
            _glControl.Location = new Point(3, 2);
            _glControl.Margin = new Padding(3, 2, 3, 2);
            _glControl.Name = "_glControl";
            _glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            _glControl.SharedContext = null;
            _glControl.Size = new Size(936, 752);
            _glControl.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(LightIntense);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(LoadButton);
            panel1.Controls.Add(fPSCounter);
            panel1.Controls.Add(LoadPreset);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(DeleteModelsBtn);
            panel1.Controls.Add(listBox1);
            panel1.Location = new Point(6, 5);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(383, 712);
            panel1.TabIndex = 1;
            // 
            // LightIntense
            // 
            LightIntense.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LightIntense.DecimalPlaces = 2;
            LightIntense.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            LightIntense.Location = new Point(18, 640);
            LightIntense.Name = "LightIntense";
            LightIntense.Size = new Size(120, 23);
            LightIntense.TabIndex = 19;
            LightIntense.Value = new decimal(new int[] { 5, 0, 0, 0 });
            LightIntense.ValueChanged += LightIntense_ValueChanged;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.BackColor = Color.Red;
            button2.Location = new Point(207, 595);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(162, 39);
            button2.TabIndex = 18;
            button2.Text = "Unbind camera";
            button2.UseVisualStyleBackColor = false;
            button2.Click += UnbindCam_Check;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.BackColor = Color.Yellow;
            button1.Location = new Point(13, 595);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(163, 39);
            button1.TabIndex = 17;
            button1.Text = "Bind Camera To Selected";
            button1.UseVisualStyleBackColor = false;
            button1.Click += BindCamToSelect_Check;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = SystemColors.ControlDarkDark;
            panel4.Controls.Add(checkBox4);
            panel4.Controls.Add(label9);
            panel4.Controls.Add(massText);
            panel4.Controls.Add(checkBox3);
            panel4.Controls.Add(checkBox1);
            panel4.Location = new Point(13, 150);
            panel4.Name = "panel4";
            panel4.Size = new Size(356, 94);
            panel4.TabIndex = 16;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(155, 9);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(99, 19);
            checkBox4.TabIndex = 16;
            checkBox4.Text = "Use Collisions";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += isCollision_Check;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.ForeColor = SystemColors.ButtonFace;
            label9.Location = new Point(11, 58);
            label9.Name = "label9";
            label9.Size = new Size(41, 19);
            label9.TabIndex = 15;
            label9.Text = "Mass";
            // 
            // massText
            // 
            massText.ImeMode = ImeMode.NoControl;
            massText.Location = new Point(56, 58);
            massText.Margin = new Padding(3, 2, 3, 2);
            massText.Name = "massText";
            massText.Size = new Size(92, 23);
            massText.TabIndex = 15;
            massText.Text = "10";
            massText.KeyPress += Coord_KeyPress;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(10, 34);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(90, 19);
            checkBox3.TabIndex = 2;
            checkBox3.Text = "Is Kinematic";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += isObjectKinematic_Check;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(10, 9);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(87, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Use Physics";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += isPhysicsAdded_Check;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel3.BackColor = SystemColors.ControlDarkDark;
            panel3.Controls.Add(label5);
            panel3.Controls.Add(xScale);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(yScale);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(zScale);
            panel3.Controls.Add(label8);
            panel3.Location = new Point(195, 66);
            panel3.Name = "panel3";
            panel3.Size = new Size(179, 74);
            panel3.TabIndex = 15;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(70, 7);
            label5.Name = "label5";
            label5.Size = new Size(39, 19);
            label5.TabIndex = 14;
            label5.Text = "Scale";
            // 
            // xScale
            // 
            xScale.Anchor = AnchorStyles.Bottom;
            xScale.ImeMode = ImeMode.NoControl;
            xScale.Location = new Point(3, 49);
            xScale.Margin = new Padding(3, 2, 3, 2);
            xScale.Name = "xScale";
            xScale.Size = new Size(54, 23);
            xScale.TabIndex = 8;
            xScale.Text = "1";
            xScale.KeyPress += Coord_KeyPress;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.ForeColor = SystemColors.ButtonFace;
            label6.Location = new Point(143, 27);
            label6.Name = "label6";
            label6.Size = new Size(17, 19);
            label6.TabIndex = 13;
            label6.Text = "Z";
            // 
            // yScale
            // 
            yScale.Anchor = AnchorStyles.Bottom;
            yScale.Location = new Point(63, 49);
            yScale.Margin = new Padding(3, 2, 3, 2);
            yScale.Name = "yScale";
            yScale.Size = new Size(56, 23);
            yScale.TabIndex = 9;
            yScale.Text = "1";
            yScale.KeyPress += Coord_KeyPress;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(80, 27);
            label7.Name = "label7";
            label7.Size = new Size(17, 19);
            label7.TabIndex = 12;
            label7.Text = "Y";
            // 
            // zScale
            // 
            zScale.Anchor = AnchorStyles.Bottom;
            zScale.Location = new Point(125, 49);
            zScale.Margin = new Padding(3, 2, 3, 2);
            zScale.Name = "zScale";
            zScale.Size = new Size(47, 23);
            zScale.TabIndex = 10;
            zScale.Text = "1";
            zScale.KeyPress += Coord_KeyPress;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom;
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(19, 27);
            label8.Name = "label8";
            label8.Size = new Size(17, 19);
            label8.TabIndex = 11;
            label8.Text = "X";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlDarkDark;
            panel2.Controls.Add(label4);
            panel2.Controls.Add(xCoord);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(yCoord);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(zCoord);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(9, 66);
            panel2.Name = "panel2";
            panel2.Size = new Size(182, 74);
            panel2.TabIndex = 14;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.ForeColor = SystemColors.ButtonFace;
            label4.Location = new Point(62, 9);
            label4.Name = "label4";
            label4.Size = new Size(57, 19);
            label4.TabIndex = 14;
            label4.Text = "Position";
            // 
            // xCoord
            // 
            xCoord.Anchor = AnchorStyles.Bottom;
            xCoord.ImeMode = ImeMode.NoControl;
            xCoord.Location = new Point(3, 49);
            xCoord.Margin = new Padding(3, 2, 3, 2);
            xCoord.Name = "xCoord";
            xCoord.Size = new Size(54, 23);
            xCoord.TabIndex = 8;
            xCoord.Text = "0";
            xCoord.KeyPress += Coord_KeyPress;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(136, 27);
            label3.Name = "label3";
            label3.Size = new Size(17, 19);
            label3.TabIndex = 13;
            label3.Text = "Z";
            // 
            // yCoord
            // 
            yCoord.Anchor = AnchorStyles.Bottom;
            yCoord.Location = new Point(63, 49);
            yCoord.Margin = new Padding(3, 2, 3, 2);
            yCoord.Name = "yCoord";
            yCoord.Size = new Size(49, 23);
            yCoord.TabIndex = 9;
            yCoord.Text = "0";
            yCoord.KeyPress += Coord_KeyPress;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(78, 27);
            label2.Name = "label2";
            label2.Size = new Size(17, 19);
            label2.TabIndex = 12;
            label2.Text = "Y";
            // 
            // zCoord
            // 
            zCoord.Anchor = AnchorStyles.Bottom;
            zCoord.Location = new Point(117, 49);
            zCoord.Margin = new Padding(3, 2, 3, 2);
            zCoord.Name = "zCoord";
            zCoord.Size = new Size(55, 23);
            zCoord.TabIndex = 10;
            zCoord.Text = "0";
            zCoord.KeyPress += Coord_KeyPress;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(22, 27);
            label1.Name = "label1";
            label1.Size = new Size(17, 19);
            label1.TabIndex = 11;
            label1.Text = "X";
            // 
            // LoadButton
            // 
            LoadButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LoadButton.BackColor = Color.YellowGreen;
            LoadButton.Location = new Point(12, 249);
            LoadButton.Margin = new Padding(3, 2, 3, 2);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(356, 50);
            LoadButton.TabIndex = 7;
            LoadButton.Text = "Add Model To Scene";
            LoadButton.UseVisualStyleBackColor = false;
            LoadButton.Click += BtnAddObject_Click;
            // 
            // fPSCounter
            // 
            fPSCounter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fPSCounter.AutoSize = true;
            fPSCounter.Font = new Font("Segoe UI", 20F);
            fPSCounter.Location = new Point(149, 659);
            fPSCounter.Name = "fPSCounter";
            fPSCounter.Size = new Size(94, 37);
            fPSCounter.TabIndex = 5;
            fPSCounter.Text = "FPS: --";
            // 
            // LoadPreset
            // 
            LoadPreset.AutoSize = true;
            LoadPreset.Font = new Font("Segoe UI", 20F);
            LoadPreset.Location = new Point(76, 0);
            LoadPreset.Name = "LoadPreset";
            LoadPreset.Size = new Size(250, 37);
            LoadPreset.TabIndex = 4;
            LoadPreset.Text = "Load preset models";
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(13, 36);
            comboBox1.Margin = new Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(356, 23);
            comboBox1.TabIndex = 3;
            comboBox1.Text = "Models";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // DeleteModelsBtn
            // 
            DeleteModelsBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DeleteModelsBtn.BackColor = Color.Tomato;
            DeleteModelsBtn.Location = new Point(13, 535);
            DeleteModelsBtn.Margin = new Padding(3, 2, 3, 2);
            DeleteModelsBtn.Name = "DeleteModelsBtn";
            DeleteModelsBtn.Size = new Size(356, 56);
            DeleteModelsBtn.TabIndex = 2;
            DeleteModelsBtn.Text = "Delete selected models from scene";
            DeleteModelsBtn.UseVisualStyleBackColor = false;
            DeleteModelsBtn.Click += DeleteModels_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(13, 317);
            listBox1.Margin = new Padding(3, 2, 3, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(356, 214);
            listBox1.TabIndex = 1;
            // 
            // DeleteDBData
            // 
            DeleteDBData.Location = new Point(207, 411);
            DeleteDBData.Name = "DeleteDBData";
            DeleteDBData.Size = new Size(182, 39);
            DeleteDBData.TabIndex = 22;
            DeleteDBData.Text = "Delete Data";
            DeleteDBData.UseVisualStyleBackColor = true;
            DeleteDBData.Click += DeleteDBData_Click;
            // 
            // SaveDB
            // 
            SaveDB.Location = new Point(6, 411);
            SaveDB.Name = "SaveDB";
            SaveDB.Size = new Size(182, 39);
            SaveDB.TabIndex = 21;
            SaveDB.Text = "Save DB";
            SaveDB.UseVisualStyleBackColor = true;
            SaveDB.Click += SaveDB_Click;
            // 
            // dataGridViewModels
            // 
            dataGridViewModels.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewModels.BackgroundColor = SystemColors.ControlDarkDark;
            dataGridViewModels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewModels.Location = new Point(6, 6);
            dataGridViewModels.Name = "dataGridViewModels";
            dataGridViewModels.Size = new Size(383, 399);
            dataGridViewModels.TabIndex = 20;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 69.765686F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.2343159F));
            tableLayoutPanel1.Controls.Add(_glControl, 0, 0);
            tableLayoutPanel1.Controls.Add(tabControl1, 1, 0);
            tableLayoutPanel1.Location = new Point(1, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1351, 756);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(945, 3);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(403, 750);
            tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(395, 722);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Scene Manager";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(DeleteDBData);
            tabPage2.Controls.Add(dataGridViewModels);
            tabPage2.Controls.Add(SaveDB);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(395, 722);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "DB Manager";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1349, 759);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "OTK 3D";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LightIntense).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewModels).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private OpenTK.GLControl.GLControl _glControl;
        private Panel panel1;
        private Button DeleteModelsBtn;
        private ListBox listBox1;
        private Label LoadPreset;
        private Label fPSCounter;
        private Button LoadButton;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox zCoord;
        private TextBox yCoord;
        private TextBox xCoord;
        private Panel panel2;
        private Panel panel3;
        private Label label5;
        private TextBox xScale;
        private Label label6;
        private TextBox yScale;
        private Label label7;
        private TextBox zScale;
        private Label label8;
        private Label label4;
        private Panel panel4;
        private CheckBox checkBox1;
        private CheckBox checkBox3;
        private Label label9;
        private TextBox massText;
        private CheckBox checkBox4;
        private Button button2;
        private Button button1;
        private NumericUpDown LightIntense;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox comboBox1;
        private DataGridView dataGridViewModels;
        private Button DeleteDBData;
        private Button SaveDB;
        private ToolStripContainer toolStripContainer1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}
