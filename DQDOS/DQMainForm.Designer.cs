namespace DQDOS
{
    partial class DQMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DQMainForm));
            this.DQHideButton = new System.Windows.Forms.Button();
            this.DQQuitButton = new System.Windows.Forms.Button();
            this.DQNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.DQNotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dvorakQwertyKeyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dvorakOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disabledQwertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DQIconList = new System.Windows.Forms.ImageList(this.components);
            this.DQModeGroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DQModeFullEnableRadio = new System.Windows.Forms.RadioButton();
            this.DQModeDvorakOnlyRadio = new System.Windows.Forms.RadioButton();
            this.DQModeDisabledRadio = new System.Windows.Forms.RadioButton();
            this.DQCurrentKBTextBox = new System.Windows.Forms.TextBox();
            this.GetCurrentLayoutButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyboardTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.SecondaryKBTextBox = new System.Windows.Forms.TextBox();
            this.PrimaryKBTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ControlKeysGroupBox = new System.Windows.Forms.GroupBox();
            this.WindowsKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.AltKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.ControlKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.DQNotifyIconContextMenu.SuspendLayout();
            this.DQModeGroupBox.SuspendLayout();
            this.KeyboardTypeGroupBox.SuspendLayout();
            this.ControlKeysGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // DQHideButton
            // 
            this.DQHideButton.Location = new System.Drawing.Point(296, 373);
            this.DQHideButton.Name = "DQHideButton";
            this.DQHideButton.Size = new System.Drawing.Size(103, 30);
            this.DQHideButton.TabIndex = 0;
            this.DQHideButton.Text = "Hide GUI";
            this.DQHideButton.UseVisualStyleBackColor = true;
            this.DQHideButton.Click += new System.EventHandler(this.DQHideButton_Click);
            // 
            // DQQuitButton
            // 
            this.DQQuitButton.Location = new System.Drawing.Point(445, 373);
            this.DQQuitButton.Name = "DQQuitButton";
            this.DQQuitButton.Size = new System.Drawing.Size(103, 30);
            this.DQQuitButton.TabIndex = 1;
            this.DQQuitButton.Text = "Quit DQDOS";
            this.DQQuitButton.UseVisualStyleBackColor = true;
            this.DQQuitButton.Click += new System.EventHandler(this.DQQuitButton_Click);
            // 
            // DQNotifyIcon
            // 
            this.DQNotifyIcon.ContextMenuStrip = this.DQNotifyIconContextMenu;
            this.DQNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("DQNotifyIcon.Icon")));
            this.DQNotifyIcon.Text = "Dvorak-Qwerty";
            this.DQNotifyIcon.Visible = true;
            this.DQNotifyIcon.DoubleClick += new System.EventHandler(this.DQNotifyIcon_DoubleClick);
            // 
            // DQNotifyIconContextMenu
            // 
            this.DQNotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dvorakQwertyKeyboardToolStripMenuItem,
            this.dvorakOnlyToolStripMenuItem,
            this.disabledQwertyToolStripMenuItem,
            this.toolStripSeparator1,
            this.openGUIToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.DQNotifyIconContextMenu.Name = "DQNotifyIconContextMenu";
            this.DQNotifyIconContextMenu.Size = new System.Drawing.Size(207, 120);
            // 
            // dvorakQwertyKeyboardToolStripMenuItem
            // 
            this.dvorakQwertyKeyboardToolStripMenuItem.Name = "dvorakQwertyKeyboardToolStripMenuItem";
            this.dvorakQwertyKeyboardToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.dvorakQwertyKeyboardToolStripMenuItem.Text = "Dvorak-Qwerty keyboard";
            this.dvorakQwertyKeyboardToolStripMenuItem.Click += new System.EventHandler(this.dvorakQwertyKeyboardToolStripMenuItem_Click);
            // 
            // dvorakOnlyToolStripMenuItem
            // 
            this.dvorakOnlyToolStripMenuItem.Name = "dvorakOnlyToolStripMenuItem";
            this.dvorakOnlyToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.dvorakOnlyToolStripMenuItem.Text = "Dvorak only";
            this.dvorakOnlyToolStripMenuItem.Click += new System.EventHandler(this.dvorakOnlyToolStripMenuItem_Click);
            // 
            // disabledQwertyToolStripMenuItem
            // 
            this.disabledQwertyToolStripMenuItem.Name = "disabledQwertyToolStripMenuItem";
            this.disabledQwertyToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.disabledQwertyToolStripMenuItem.Text = "Disabled (Qwerty)";
            this.disabledQwertyToolStripMenuItem.Click += new System.EventHandler(this.disabledQwertyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // openGUIToolStripMenuItem
            // 
            this.openGUIToolStripMenuItem.Name = "openGUIToolStripMenuItem";
            this.openGUIToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.openGUIToolStripMenuItem.Text = "Open GUI";
            this.openGUIToolStripMenuItem.Click += new System.EventHandler(this.openGUIToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // DQIconList
            // 
            this.DQIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DQIconList.ImageStream")));
            this.DQIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.DQIconList.Images.SetKeyName(0, "DQEnabled16x16.ico");
            this.DQIconList.Images.SetKeyName(1, "DQDisabled16x16.ico");
            this.DQIconList.Images.SetKeyName(2, "DQEnabled32x32.ico");
            this.DQIconList.Images.SetKeyName(3, "DQDisabled32x32.ico");
            this.DQIconList.Images.SetKeyName(4, "DQEnabled64x64.ico");
            this.DQIconList.Images.SetKeyName(5, "DQDisabled64x64.ico");
            // 
            // DQModeGroupBox
            // 
            this.DQModeGroupBox.Controls.Add(this.label8);
            this.DQModeGroupBox.Controls.Add(this.label9);
            this.DQModeGroupBox.Controls.Add(this.label6);
            this.DQModeGroupBox.Controls.Add(this.label7);
            this.DQModeGroupBox.Controls.Add(this.label5);
            this.DQModeGroupBox.Controls.Add(this.label4);
            this.DQModeGroupBox.Controls.Add(this.DQModeFullEnableRadio);
            this.DQModeGroupBox.Controls.Add(this.DQModeDvorakOnlyRadio);
            this.DQModeGroupBox.Controls.Add(this.DQModeDisabledRadio);
            this.DQModeGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DQModeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.DQModeGroupBox.Name = "DQModeGroupBox";
            this.DQModeGroupBox.Size = new System.Drawing.Size(277, 228);
            this.DQModeGroupBox.TabIndex = 2;
            this.DQModeGroupBox.TabStop = false;
            this.DQModeGroupBox.Text = "DQ Keyboard Mode";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(60, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "-Effectively disabled.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(60, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(181, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "-Enables secondary keyboard layout.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(60, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "-NO secondary layout.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(60, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(165, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "-Enables primary keyboard layout.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(60, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "-Enables secondary layout on control keys.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "-Enables primary keyboard layout.";
            // 
            // DQModeFullEnableRadio
            // 
            this.DQModeFullEnableRadio.AutoSize = true;
            this.DQModeFullEnableRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DQModeFullEnableRadio.Location = new System.Drawing.Point(27, 29);
            this.DQModeFullEnableRadio.Name = "DQModeFullEnableRadio";
            this.DQModeFullEnableRadio.Size = new System.Drawing.Size(238, 17);
            this.DQModeFullEnableRadio.TabIndex = 2;
            this.DQModeFullEnableRadio.Text = "Dvorak-Qwerty (Primary keyboard with filters).";
            this.DQModeFullEnableRadio.UseVisualStyleBackColor = true;
            this.DQModeFullEnableRadio.CheckedChanged += new System.EventHandler(this.DQModeFullEnableRadio_CheckedChanged);
            // 
            // DQModeDvorakOnlyRadio
            // 
            this.DQModeDvorakOnlyRadio.AutoSize = true;
            this.DQModeDvorakOnlyRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DQModeDvorakOnlyRadio.Location = new System.Drawing.Point(27, 94);
            this.DQModeDvorakOnlyRadio.Name = "DQModeDvorakOnlyRadio";
            this.DQModeDvorakOnlyRadio.Size = new System.Drawing.Size(190, 17);
            this.DQModeDvorakOnlyRadio.TabIndex = 1;
            this.DQModeDvorakOnlyRadio.Text = "Dvorak only (Primary and NO filter).";
            this.DQModeDvorakOnlyRadio.UseVisualStyleBackColor = true;
            this.DQModeDvorakOnlyRadio.CheckedChanged += new System.EventHandler(this.DQModeDvorakOnlyRadio_CheckedChanged);
            // 
            // DQModeDisabledRadio
            // 
            this.DQModeDisabledRadio.AutoSize = true;
            this.DQModeDisabledRadio.Checked = true;
            this.DQModeDisabledRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DQModeDisabledRadio.Location = new System.Drawing.Point(27, 159);
            this.DQModeDisabledRadio.Name = "DQModeDisabledRadio";
            this.DQModeDisabledRadio.Size = new System.Drawing.Size(168, 17);
            this.DQModeDisabledRadio.TabIndex = 0;
            this.DQModeDisabledRadio.TabStop = true;
            this.DQModeDisabledRadio.Text = "Qwerty (Secondary keyboard).";
            this.DQModeDisabledRadio.UseVisualStyleBackColor = true;
            this.DQModeDisabledRadio.CheckedChanged += new System.EventHandler(this.DQModeDisabledRadio_CheckedChanged);
            // 
            // DQCurrentKBTextBox
            // 
            this.DQCurrentKBTextBox.Location = new System.Drawing.Point(437, 288);
            this.DQCurrentKBTextBox.Name = "DQCurrentKBTextBox";
            this.DQCurrentKBTextBox.Size = new System.Drawing.Size(103, 20);
            this.DQCurrentKBTextBox.TabIndex = 4;
            // 
            // GetCurrentLayoutButton
            // 
            this.GetCurrentLayoutButton.Location = new System.Drawing.Point(304, 282);
            this.GetCurrentLayoutButton.Name = "GetCurrentLayoutButton";
            this.GetCurrentLayoutButton.Size = new System.Drawing.Size(103, 30);
            this.GetCurrentLayoutButton.TabIndex = 5;
            this.GetCurrentLayoutButton.Text = "Current Keyboard";
            this.GetCurrentLayoutButton.UseVisualStyleBackColor = true;
            this.GetCurrentLayoutButton.Click += new System.EventHandler(this.GetCurrentLayoutButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(409, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "-->";
            // 
            // KeyboardTypeGroupBox
            // 
            this.KeyboardTypeGroupBox.Controls.Add(this.SecondaryKBTextBox);
            this.KeyboardTypeGroupBox.Controls.Add(this.PrimaryKBTextBox);
            this.KeyboardTypeGroupBox.Controls.Add(this.label3);
            this.KeyboardTypeGroupBox.Controls.Add(this.label2);
            this.KeyboardTypeGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyboardTypeGroupBox.Location = new System.Drawing.Point(295, 12);
            this.KeyboardTypeGroupBox.Name = "KeyboardTypeGroupBox";
            this.KeyboardTypeGroupBox.Size = new System.Drawing.Size(253, 100);
            this.KeyboardTypeGroupBox.TabIndex = 7;
            this.KeyboardTypeGroupBox.TabStop = false;
            this.KeyboardTypeGroupBox.Text = "Keyboard Layouts";
            // 
            // SecondaryKBTextBox
            // 
            this.SecondaryKBTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondaryKBTextBox.Location = new System.Drawing.Point(140, 60);
            this.SecondaryKBTextBox.Name = "SecondaryKBTextBox";
            this.SecondaryKBTextBox.Size = new System.Drawing.Size(103, 20);
            this.SecondaryKBTextBox.TabIndex = 10;
            this.SecondaryKBTextBox.Text = "Qwerty";
            this.SecondaryKBTextBox.Leave += new System.EventHandler(this.SecondardKBTextBox_Leave);
            // 
            // PrimaryKBTextBox
            // 
            this.PrimaryKBTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrimaryKBTextBox.Location = new System.Drawing.Point(140, 26);
            this.PrimaryKBTextBox.Name = "PrimaryKBTextBox";
            this.PrimaryKBTextBox.Size = new System.Drawing.Size(103, 20);
            this.PrimaryKBTextBox.TabIndex = 8;
            this.PrimaryKBTextBox.Text = "Dvorak";
            this.PrimaryKBTextBox.Leave += new System.EventHandler(this.PrimaryKBTextBox_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Secondary Keyboard:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Primary Keyboard:";
            // 
            // ControlKeysGroupBox
            // 
            this.ControlKeysGroupBox.Controls.Add(this.WindowsKeyCheckBox);
            this.ControlKeysGroupBox.Controls.Add(this.AltKeyCheckBox);
            this.ControlKeysGroupBox.Controls.Add(this.ControlKeyCheckBox);
            this.ControlKeysGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlKeysGroupBox.Location = new System.Drawing.Point(296, 119);
            this.ControlKeysGroupBox.Name = "ControlKeysGroupBox";
            this.ControlKeysGroupBox.Size = new System.Drawing.Size(252, 121);
            this.ControlKeysGroupBox.TabIndex = 8;
            this.ControlKeysGroupBox.TabStop = false;
            this.ControlKeysGroupBox.Text = "Enabled Control Keys";
            // 
            // WindowsKeyCheckBox
            // 
            this.WindowsKeyCheckBox.AutoSize = true;
            this.WindowsKeyCheckBox.Checked = true;
            this.WindowsKeyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WindowsKeyCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowsKeyCheckBox.Location = new System.Drawing.Point(22, 90);
            this.WindowsKeyCheckBox.Name = "WindowsKeyCheckBox";
            this.WindowsKeyCheckBox.Size = new System.Drawing.Size(90, 17);
            this.WindowsKeyCheckBox.TabIndex = 2;
            this.WindowsKeyCheckBox.Text = "Windows key";
            this.WindowsKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // AltKeyCheckBox
            // 
            this.AltKeyCheckBox.AutoSize = true;
            this.AltKeyCheckBox.Checked = true;
            this.AltKeyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AltKeyCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltKeyCheckBox.Location = new System.Drawing.Point(22, 58);
            this.AltKeyCheckBox.Name = "AltKeyCheckBox";
            this.AltKeyCheckBox.Size = new System.Drawing.Size(58, 17);
            this.AltKeyCheckBox.TabIndex = 1;
            this.AltKeyCheckBox.Text = "Alt key";
            this.AltKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // ControlKeyCheckBox
            // 
            this.ControlKeyCheckBox.AutoSize = true;
            this.ControlKeyCheckBox.Checked = true;
            this.ControlKeyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ControlKeyCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlKeyCheckBox.Location = new System.Drawing.Point(22, 26);
            this.ControlKeyCheckBox.Name = "ControlKeyCheckBox";
            this.ControlKeyCheckBox.Size = new System.Drawing.Size(79, 17);
            this.ControlKeyCheckBox.TabIndex = 0;
            this.ControlKeyCheckBox.Text = "Control key";
            this.ControlKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // DQMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 427);
            this.Controls.Add(this.ControlKeysGroupBox);
            this.Controls.Add(this.KeyboardTypeGroupBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GetCurrentLayoutButton);
            this.Controls.Add(this.DQCurrentKBTextBox);
            this.Controls.Add(this.DQModeGroupBox);
            this.Controls.Add(this.DQQuitButton);
            this.Controls.Add(this.DQHideButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DQMainForm";
            this.Text = "Derick\'s Dvorak-Qwerty Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DQMainForm_FormClosing);
            this.DQNotifyIconContextMenu.ResumeLayout(false);
            this.DQModeGroupBox.ResumeLayout(false);
            this.DQModeGroupBox.PerformLayout();
            this.KeyboardTypeGroupBox.ResumeLayout(false);
            this.KeyboardTypeGroupBox.PerformLayout();
            this.ControlKeysGroupBox.ResumeLayout(false);
            this.ControlKeysGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DQHideButton;
        private System.Windows.Forms.Button DQQuitButton;
        private System.Windows.Forms.NotifyIcon DQNotifyIcon;
        private System.Windows.Forms.ImageList DQIconList;
        private System.Windows.Forms.GroupBox DQModeGroupBox;
        private System.Windows.Forms.RadioButton DQModeFullEnableRadio;
        private System.Windows.Forms.RadioButton DQModeDvorakOnlyRadio;
        private System.Windows.Forms.RadioButton DQModeDisabledRadio;
        private System.Windows.Forms.ContextMenuStrip DQNotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem dvorakQwertyKeyboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dvorakOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disabledQwertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openGUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox DQCurrentKBTextBox;
        private System.Windows.Forms.Button GetCurrentLayoutButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox KeyboardTypeGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SecondaryKBTextBox;
        private System.Windows.Forms.TextBox PrimaryKBTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox ControlKeysGroupBox;
        private System.Windows.Forms.CheckBox WindowsKeyCheckBox;
        private System.Windows.Forms.CheckBox AltKeyCheckBox;
        private System.Windows.Forms.CheckBox ControlKeyCheckBox;
    }
}

