namespace SSISBulkExportTask100
{
    partial class frmPreview
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditProperties));
            this.btSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbSQLServer = new System.Windows.Forms.ComboBox();
            this.btGO = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSQL = new System.Windows.Forms.TabPage();
            this.txSQL = new System.Windows.Forms.TextBox();
            this.tabView = new System.Windows.Forms.TabPage();
            this.cmbViews = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabStoredProcedure = new System.Windows.Forms.TabPage();
            this.cmbStoredProcedures = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grdParameters = new System.Windows.Forms.DataGridView();
            this.grdColParams = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdColDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdColVars = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.grdColExpression = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabTables = new System.Windows.Forms.TabPage();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDestination = new System.Windows.Forms.ComboBox();
            this.cmdFileVariable = new System.Windows.Forms.Button();
            this.optFileConnection = new System.Windows.Forms.RadioButton();
            this.optFileVariable = new System.Windows.Forms.RadioButton();
            this.btPreview = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRightsCMDSHELL = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbPassword = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbLogin = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkTrustedConnection = new System.Windows.Forms.CheckBox();
            this.txRowTerminator = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txFieldTerminator = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkNativeDatabase = new System.Windows.Forms.CheckBox();
            this.txLastRow = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txFirstRow = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.optFileFormatVariable = new System.Windows.Forms.RadioButton();
            this.optFileFormatConnection = new System.Windows.Forms.RadioButton();
            this.cmdFileFormatVariable = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbFormatFile = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.tabSQL.SuspendLayout();
            this.tabView.SuspendLayout();
            this.tabStoredProcedure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParameters)).BeginInit();
            this.tabTables.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Location = new System.Drawing.Point(406, 563);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 33);
            this.btSave.TabIndex = 32;
            this.btSave.Text = "OK";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(487, 563);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbSQLServer
            // 
            this.cmbSQLServer.FormattingEnabled = true;
            this.cmbSQLServer.Location = new System.Drawing.Point(94, 25);
            this.cmbSQLServer.Name = "cmbSQLServer";
            this.cmbSQLServer.Size = new System.Drawing.Size(419, 21);
            this.cmbSQLServer.TabIndex = 0;
            // 
            // btGO
            // 
            this.btGO.Location = new System.Drawing.Point(519, 23);
            this.btGO.Name = "btGO";
            this.btGO.Size = new System.Drawing.Size(42, 23);
            this.btGO.TabIndex = 1;
            this.btGO.Text = "&Go";
            this.btGO.UseVisualStyleBackColor = true;
            this.btGO.Click += new System.EventHandler(this.btGO_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SQL Server:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSQL);
            this.tabControl.Controls.Add(this.tabView);
            this.tabControl.Controls.Add(this.tabStoredProcedure);
            this.tabControl.Controls.Add(this.tabTables);
            this.tabControl.Location = new System.Drawing.Point(16, 52);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(545, 172);
            this.tabControl.TabIndex = 33;
            // 
            // tabSQL
            // 
            this.tabSQL.Controls.Add(this.txSQL);
            this.tabSQL.Location = new System.Drawing.Point(4, 22);
            this.tabSQL.Name = "tabSQL";
            this.tabSQL.Padding = new System.Windows.Forms.Padding(3);
            this.tabSQL.Size = new System.Drawing.Size(537, 146);
            this.tabSQL.TabIndex = 0;
            this.tabSQL.Text = "SQL Statement";
            this.tabSQL.UseVisualStyleBackColor = true;
            // 
            // txSQL
            // 
            this.txSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txSQL.Location = new System.Drawing.Point(3, 3);
            this.txSQL.Multiline = true;
            this.txSQL.Name = "txSQL";
            this.txSQL.Size = new System.Drawing.Size(531, 140);
            this.txSQL.TabIndex = 0;
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.cmbViews);
            this.tabView.Controls.Add(this.label6);
            this.tabView.Location = new System.Drawing.Point(4, 22);
            this.tabView.Name = "tabView";
            this.tabView.Padding = new System.Windows.Forms.Padding(3);
            this.tabView.Size = new System.Drawing.Size(537, 146);
            this.tabView.TabIndex = 1;
            this.tabView.Text = "View";
            this.tabView.UseVisualStyleBackColor = true;
            // 
            // cmbViews
            // 
            this.cmbViews.FormattingEnabled = true;
            this.cmbViews.Location = new System.Drawing.Point(50, 14);
            this.cmbViews.Name = "cmbViews";
            this.cmbViews.Size = new System.Drawing.Size(480, 21);
            this.cmbViews.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Views:";
            // 
            // tabStoredProcedure
            // 
            this.tabStoredProcedure.Controls.Add(this.cmbStoredProcedures);
            this.tabStoredProcedure.Controls.Add(this.label5);
            this.tabStoredProcedure.Controls.Add(this.grdParameters);
            this.tabStoredProcedure.Location = new System.Drawing.Point(4, 22);
            this.tabStoredProcedure.Name = "tabStoredProcedure";
            this.tabStoredProcedure.Padding = new System.Windows.Forms.Padding(3);
            this.tabStoredProcedure.Size = new System.Drawing.Size(537, 146);
            this.tabStoredProcedure.TabIndex = 2;
            this.tabStoredProcedure.Text = "Stored Procedure";
            this.tabStoredProcedure.UseVisualStyleBackColor = true;
            // 
            // cmbStoredProcedures
            // 
            this.cmbStoredProcedures.FormattingEnabled = true;
            this.cmbStoredProcedures.Location = new System.Drawing.Point(111, 9);
            this.cmbStoredProcedures.Name = "cmbStoredProcedures";
            this.cmbStoredProcedures.Size = new System.Drawing.Size(420, 21);
            this.cmbStoredProcedures.TabIndex = 28;
            this.cmbStoredProcedures.Click += new System.EventHandler(this.cmbStoredProcedures_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Stored Procedures:";
            // 
            // grdParameters
            // 
            this.grdParameters.AllowUserToAddRows = false;
            this.grdParameters.AllowUserToDeleteRows = false;
            this.grdParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.grdColParams,
            this.grdColDirection,
            this.grdColVars,
            this.grdColExpression});
            this.grdParameters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdParameters.Location = new System.Drawing.Point(3, 36);
            this.grdParameters.Name = "grdParameters";
            this.grdParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdParameters.Size = new System.Drawing.Size(531, 107);
            this.grdParameters.TabIndex = 26;
            this.grdParameters.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdParameters_CellContentClick);
            // 
            // grdColParams
            // 
            this.grdColParams.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.grdColParams.Frozen = true;
            this.grdColParams.HeaderText = "Parameters";
            this.grdColParams.Name = "grdColParams";
            this.grdColParams.ReadOnly = true;
            this.grdColParams.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.grdColParams.Width = 66;
            // 
            // grdColDirection
            // 
            this.grdColDirection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.grdColDirection.FillWeight = 40F;
            this.grdColDirection.HeaderText = "Param Type";
            this.grdColDirection.Name = "grdColDirection";
            this.grdColDirection.ReadOnly = true;
            this.grdColDirection.Width = 89;
            // 
            // grdColVars
            // 
            this.grdColVars.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.grdColVars.DropDownWidth = 300;
            this.grdColVars.HeaderText = "Variables";
            this.grdColVars.MaxDropDownItems = 10;
            this.grdColVars.Name = "grdColVars";
            this.grdColVars.Sorted = true;
            this.grdColVars.Width = 240;
            // 
            // grdColExpression
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            this.grdColExpression.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdColExpression.HeaderText = "f(x)";
            this.grdColExpression.Name = "grdColExpression";
            this.grdColExpression.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.grdColExpression.Text = "f(x)";
            this.grdColExpression.ToolTipText = "Expressions...";
            this.grdColExpression.Width = 30;
            // 
            // tabTables
            // 
            this.tabTables.Controls.Add(this.cmbTables);
            this.tabTables.Controls.Add(this.label7);
            this.tabTables.Location = new System.Drawing.Point(4, 22);
            this.tabTables.Name = "tabTables";
            this.tabTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabTables.Size = new System.Drawing.Size(537, 146);
            this.tabTables.TabIndex = 3;
            this.tabTables.Text = "Tables";
            this.tabTables.UseVisualStyleBackColor = true;
            // 
            // cmbTables
            // 
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(47, 15);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(484, 21);
            this.cmbTables.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Tables:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 508);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Destination";
            // 
            // cmbDestination
            // 
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Location = new System.Drawing.Point(94, 505);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.Size = new System.Drawing.Size(420, 21);
            this.cmbDestination.TabIndex = 36;
            // 
            // cmdFileVariable
            // 
            this.cmdFileVariable.Location = new System.Drawing.Point(521, 503);
            this.cmdFileVariable.Name = "cmdFileVariable";
            this.cmdFileVariable.Size = new System.Drawing.Size(42, 23);
            this.cmdFileVariable.TabIndex = 38;
            this.cmdFileVariable.Text = "f(x)";
            this.cmdFileVariable.UseVisualStyleBackColor = true;
            this.cmdFileVariable.Click += new System.EventHandler(this.cmdFileVariable_Click);
            // 
            // optFileConnection
            // 
            this.optFileConnection.AutoSize = true;
            this.optFileConnection.Checked = true;
            this.optFileConnection.Location = new System.Drawing.Point(92, 532);
            this.optFileConnection.Name = "optFileConnection";
            this.optFileConnection.Size = new System.Drawing.Size(98, 17);
            this.optFileConnection.TabIndex = 39;
            this.optFileConnection.TabStop = true;
            this.optFileConnection.Text = "File Connection";
            this.optFileConnection.UseVisualStyleBackColor = true;
            this.optFileConnection.CheckedChanged += new System.EventHandler(this.optFileConnection_CheckedChanged);
            this.optFileConnection.Click += new System.EventHandler(this.optFileConnection_Click);
            // 
            // optFileVariable
            // 
            this.optFileVariable.AutoSize = true;
            this.optFileVariable.Location = new System.Drawing.Point(210, 532);
            this.optFileVariable.Name = "optFileVariable";
            this.optFileVariable.Size = new System.Drawing.Size(128, 17);
            this.optFileVariable.TabIndex = 40;
            this.optFileVariable.Text = "Variable / Expression ";
            this.optFileVariable.UseVisualStyleBackColor = true;
            this.optFileVariable.CheckedChanged += new System.EventHandler(this.optFileVariable_CheckedChanged);
            this.optFileVariable.Click += new System.EventHandler(this.optFileVariable_Click);
            // 
            // btPreview
            // 
            this.btPreview.Location = new System.Drawing.Point(485, 230);
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(75, 23);
            this.btPreview.TabIndex = 41;
            this.btPreview.Text = "Preview";
            this.btPreview.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRightsCMDSHELL);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.txRowTerminator);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txFieldTerminator);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkNativeDatabase);
            this.groupBox1.Controls.Add(this.txLastRow);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txFirstRow);
            this.groupBox1.Location = new System.Drawing.Point(16, 259);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 179);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // chkRightsCMDSHELL
            // 
            this.chkRightsCMDSHELL.AutoSize = true;
            this.chkRightsCMDSHELL.Location = new System.Drawing.Point(326, 73);
            this.chkRightsCMDSHELL.Name = "chkRightsCMDSHELL";
            this.chkRightsCMDSHELL.Size = new System.Drawing.Size(136, 17);
            this.chkRightsCMDSHELL.TabIndex = 10;
            this.chkRightsCMDSHELL.Text = "Activate | xp_cmdshell |";
            this.chkRightsCMDSHELL.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbPassword);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cmbLogin);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.chkTrustedConnection);
            this.groupBox2.Location = new System.Drawing.Point(4, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(535, 78);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database connection";
            // 
            // cmbPassword
            // 
            this.cmbPassword.Enabled = false;
            this.cmbPassword.FormattingEnabled = true;
            this.cmbPassword.Location = new System.Drawing.Point(330, 46);
            this.cmbPassword.Name = "cmbPassword";
            this.cmbPassword.Size = new System.Drawing.Size(199, 21);
            this.cmbPassword.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Enabled = false;
            this.label11.Location = new System.Drawing.Point(267, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Password:";
            // 
            // cmbLogin
            // 
            this.cmbLogin.Enabled = false;
            this.cmbLogin.FormattingEnabled = true;
            this.cmbLogin.Location = new System.Drawing.Point(47, 46);
            this.cmbLogin.Name = "cmbLogin";
            this.cmbLogin.Size = new System.Drawing.Size(199, 21);
            this.cmbLogin.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Location = new System.Drawing.Point(7, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Login:";
            // 
            // chkTrustedConnection
            // 
            this.chkTrustedConnection.AutoSize = true;
            this.chkTrustedConnection.Checked = true;
            this.chkTrustedConnection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrustedConnection.Location = new System.Drawing.Point(10, 19);
            this.chkTrustedConnection.Name = "chkTrustedConnection";
            this.chkTrustedConnection.Size = new System.Drawing.Size(262, 17);
            this.chkTrustedConnection.TabIndex = 0;
            this.chkTrustedConnection.Text = "Use a trusted connection using integrated security";
            this.toolTip1.SetToolTip(this.chkTrustedConnection, "Connects to SQL Server with a trusted connection using integrated security. The s" +
                    "ecurity credentials of the network user, login_id, and password are not required" +
                    ". \r\n");
            this.chkTrustedConnection.UseVisualStyleBackColor = true;
            this.chkTrustedConnection.Click += new System.EventHandler(this.chkTrustedConnection_Click);
            // 
            // txRowTerminator
            // 
            this.txRowTerminator.Location = new System.Drawing.Point(408, 47);
            this.txRowTerminator.Name = "txRowTerminator";
            this.txRowTerminator.Size = new System.Drawing.Size(76, 20);
            this.txRowTerminator.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(323, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Row terminator:";
            this.toolTip1.SetToolTip(this.label9, "Specifies the row terminator. The default is \\n (newline character). Use this par" +
                    "ameter to override the default row terminator.\r\n");
            // 
            // txFieldTerminator
            // 
            this.txFieldTerminator.AutoCompleteCustomSource.AddRange(new string[] {
            ";",
            "|",
            "|^"});
            this.txFieldTerminator.Location = new System.Drawing.Point(408, 21);
            this.txFieldTerminator.Name = "txFieldTerminator";
            this.txFieldTerminator.Size = new System.Drawing.Size(76, 20);
            this.txFieldTerminator.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(323, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Field terminator";
            this.toolTip1.SetToolTip(this.label4, "Specifies the field terminator. The default is \\t (tab character). Use this param" +
                    "eter to override the default field terminator. \r\n");
            // 
            // chkNativeDatabase
            // 
            this.chkNativeDatabase.AutoSize = true;
            this.chkNativeDatabase.Checked = true;
            this.chkNativeDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNativeDatabase.Location = new System.Drawing.Point(15, 73);
            this.chkNativeDatabase.Name = "chkNativeDatabase";
            this.chkNativeDatabase.Size = new System.Drawing.Size(254, 17);
            this.chkNativeDatabase.TabIndex = 4;
            this.chkNativeDatabase.Text = "Use the native (database) data types of the data";
            this.toolTip1.SetToolTip(this.chkNativeDatabase, "Performs the bulk-copy operation using the native (database) data types of the da" +
                    "ta. This option does not prompt for each field; it uses the native values.\r\n");
            this.chkNativeDatabase.UseVisualStyleBackColor = true;
            // 
            // txLastRow
            // 
            this.txLastRow.Location = new System.Drawing.Point(74, 47);
            this.txLastRow.Name = "txLastRow";
            this.txLastRow.Size = new System.Drawing.Size(76, 20);
            this.txLastRow.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Row:";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "First Row:";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // txFirstRow
            // 
            this.txFirstRow.Location = new System.Drawing.Point(74, 21);
            this.txFirstRow.Name = "txFirstRow";
            this.txFirstRow.Size = new System.Drawing.Size(76, 20);
            this.txFirstRow.TabIndex = 0;
            // 
            // optFileFormatVariable
            // 
            this.optFileFormatVariable.AutoSize = true;
            this.optFileFormatVariable.Location = new System.Drawing.Point(210, 481);
            this.optFileFormatVariable.Name = "optFileFormatVariable";
            this.optFileFormatVariable.Size = new System.Drawing.Size(128, 17);
            this.optFileFormatVariable.TabIndex = 47;
            this.optFileFormatVariable.Text = "Variable / Expression ";
            this.optFileFormatVariable.UseVisualStyleBackColor = true;
            this.optFileFormatVariable.CheckedChanged += new System.EventHandler(this.optFileFormatVariable_CheckedChanged);
            // 
            // optFileFormatConnection
            // 
            this.optFileFormatConnection.AutoSize = true;
            this.optFileFormatConnection.Checked = true;
            this.optFileFormatConnection.Location = new System.Drawing.Point(92, 481);
            this.optFileFormatConnection.Name = "optFileFormatConnection";
            this.optFileFormatConnection.Size = new System.Drawing.Size(98, 17);
            this.optFileFormatConnection.TabIndex = 46;
            this.optFileFormatConnection.TabStop = true;
            this.optFileFormatConnection.Text = "File Connection";
            this.optFileFormatConnection.UseVisualStyleBackColor = true;
            this.optFileFormatConnection.CheckedChanged += new System.EventHandler(this.optFileFormatConnection_CheckedChanged);
            // 
            // cmdFileFormatVariable
            // 
            this.cmdFileFormatVariable.Location = new System.Drawing.Point(521, 452);
            this.cmdFileFormatVariable.Name = "cmdFileFormatVariable";
            this.cmdFileFormatVariable.Size = new System.Drawing.Size(42, 23);
            this.cmdFileFormatVariable.TabIndex = 45;
            this.cmdFileFormatVariable.Text = "f(x)";
            this.cmdFileFormatVariable.UseVisualStyleBackColor = true;
            this.cmdFileFormatVariable.Click += new System.EventHandler(this.cmdFileFormatVariable_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 457);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 44;
            this.label12.Text = "Format file";
            // 
            // cmbFormatFile
            // 
            this.cmbFormatFile.FormattingEnabled = true;
            this.cmbFormatFile.Location = new System.Drawing.Point(94, 454);
            this.cmbFormatFile.Name = "cmbFormatFile";
            this.cmbFormatFile.Size = new System.Drawing.Size(420, 21);
            this.cmbFormatFile.TabIndex = 43;
            // 
            // frmEditProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 607);
            this.Controls.Add(this.optFileFormatVariable);
            this.Controls.Add(this.optFileFormatConnection);
            this.Controls.Add(this.cmdFileFormatVariable);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbFormatFile);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btPreview);
            this.Controls.Add(this.optFileVariable);
            this.Controls.Add(this.optFileConnection);
            this.Controls.Add(this.cmdFileVariable);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbDestination);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btGO);
            this.Controls.Add(this.cmbSQLServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit task properties";
            this.tabControl.ResumeLayout(false);
            this.tabSQL.ResumeLayout(false);
            this.tabSQL.PerformLayout();
            this.tabView.ResumeLayout(false);
            this.tabView.PerformLayout();
            this.tabStoredProcedure.ResumeLayout(false);
            this.tabStoredProcedure.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParameters)).EndInit();
            this.tabTables.ResumeLayout(false);
            this.tabTables.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbSQLServer;
        private System.Windows.Forms.Button btGO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSQL;
        private System.Windows.Forms.TabPage tabView;
        private System.Windows.Forms.ComboBox cmbViews;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabStoredProcedure;
        private System.Windows.Forms.ComboBox cmbStoredProcedures;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView grdParameters;
        private System.Windows.Forms.DataGridViewTextBoxColumn grdColParams;
        private System.Windows.Forms.DataGridViewTextBoxColumn grdColDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn grdColVars;
        private System.Windows.Forms.DataGridViewButtonColumn grdColExpression;
        private System.Windows.Forms.TextBox txSQL;
        private System.Windows.Forms.TabPage tabTables;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDestination;
        private System.Windows.Forms.Button cmdFileVariable;
        private System.Windows.Forms.RadioButton optFileConnection;
        private System.Windows.Forms.RadioButton optFileVariable;
        private System.Windows.Forms.Button btPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txRowTerminator;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txFieldTerminator;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkNativeDatabase;
        private System.Windows.Forms.TextBox txLastRow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txFirstRow;
        private System.Windows.Forms.CheckBox chkTrustedConnection;
        private System.Windows.Forms.ComboBox cmbPassword;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbLogin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkRightsCMDSHELL;
        private System.Windows.Forms.RadioButton optFileFormatVariable;
        private System.Windows.Forms.RadioButton optFileFormatConnection;
        private System.Windows.Forms.Button cmdFileFormatVariable;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbFormatFile;

    }
}