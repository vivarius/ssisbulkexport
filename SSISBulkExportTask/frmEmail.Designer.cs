namespace SSISBulkExportTask100
{
    partial class frmEmail
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
            this.cmbTo = new System.Windows.Forms.ComboBox();
            this.cmbFrom = new System.Windows.Forms.ComboBox();
            this.cmbSMTPSrv = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.txBody = new System.Windows.Forms.TextBox();
            this.btBody = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btSubject = new System.Windows.Forms.Button();
            this.txSubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStaticMessages = new System.Windows.Forms.ComboBox();
            this.btInsertExpression = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbTo
            // 
            this.cmbTo.FormattingEnabled = true;
            this.cmbTo.Location = new System.Drawing.Point(91, 59);
            this.cmbTo.Name = "cmbTo";
            this.cmbTo.Size = new System.Drawing.Size(345, 21);
            this.cmbTo.TabIndex = 77;
            // 
            // cmbFrom
            // 
            this.cmbFrom.FormattingEnabled = true;
            this.cmbFrom.Location = new System.Drawing.Point(91, 33);
            this.cmbFrom.Name = "cmbFrom";
            this.cmbFrom.Size = new System.Drawing.Size(345, 21);
            this.cmbFrom.TabIndex = 76;
            // 
            // cmbSMTPSrv
            // 
            this.cmbSMTPSrv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSMTPSrv.FormattingEnabled = true;
            this.cmbSMTPSrv.Location = new System.Drawing.Point(91, 6);
            this.cmbSMTPSrv.Name = "cmbSMTPSrv";
            this.cmbSMTPSrv.Size = new System.Drawing.Size(345, 21);
            this.cmbSMTPSrv.TabIndex = 75;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 13);
            this.label13.TabIndex = 74;
            this.label13.Text = "SMTP Server:";
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(227, 273);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 73;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(308, 273);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 72;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // txBody
            // 
            this.txBody.Location = new System.Drawing.Point(91, 135);
            this.txBody.Multiline = true;
            this.txBody.Name = "txBody";
            this.txBody.Size = new System.Drawing.Size(292, 132);
            this.txBody.TabIndex = 71;
            // 
            // btBody
            // 
            this.btBody.Location = new System.Drawing.Point(389, 134);
            this.btBody.Name = "btBody";
            this.btBody.Size = new System.Drawing.Size(47, 21);
            this.btBody.TabIndex = 70;
            this.btBody.Text = "f(x)";
            this.btBody.UseVisualStyleBackColor = true;
            this.btBody.Click += new System.EventHandler(this.btBody_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 69;
            this.label4.Text = "Body:";
            // 
            // btSubject
            // 
            this.btSubject.Location = new System.Drawing.Point(389, 83);
            this.btSubject.Name = "btSubject";
            this.btSubject.Size = new System.Drawing.Size(47, 23);
            this.btSubject.TabIndex = 68;
            this.btSubject.Text = "f(x)";
            this.btSubject.UseVisualStyleBackColor = true;
            this.btSubject.Click += new System.EventHandler(this.btSubject_Click);
            // 
            // txSubject
            // 
            this.txSubject.Location = new System.Drawing.Point(91, 85);
            this.txSubject.Name = "txSubject";
            this.txSubject.Size = new System.Drawing.Size(292, 20);
            this.txSubject.TabIndex = 67;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 65;
            this.label2.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "From:";
            // 
            // cmbStaticMessages
            // 
            this.cmbStaticMessages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStaticMessages.FormattingEnabled = true;
            this.cmbStaticMessages.Items.AddRange(new object[] {
            "File path",
            "File size",
            "Start Time",
            "End Time",
            "Execution Time"});
            this.cmbStaticMessages.Location = new System.Drawing.Point(199, 108);
            this.cmbStaticMessages.Name = "cmbStaticMessages";
            this.cmbStaticMessages.Size = new System.Drawing.Size(184, 21);
            this.cmbStaticMessages.TabIndex = 78;
            // 
            // btInsertExpression
            // 
            this.btInsertExpression.Location = new System.Drawing.Point(389, 108);
            this.btInsertExpression.Name = "btInsertExpression";
            this.btInsertExpression.Size = new System.Drawing.Size(47, 21);
            this.btInsertExpression.TabIndex = 79;
            this.btInsertExpression.Text = "insert";
            this.btInsertExpression.UseVisualStyleBackColor = true;
            this.btInsertExpression.Click += new System.EventHandler(this.btInsertExpression_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 80;
            this.label5.Text = "Insert dynamic items:";
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 301);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btInsertExpression);
            this.Controls.Add(this.cmbStaticMessages);
            this.Controls.Add(this.cmbTo);
            this.Controls.Add(this.cmbFrom);
            this.Controls.Add(this.cmbSMTPSrv);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.txBody);
            this.Controls.Add(this.btBody);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btSubject);
            this.Controls.Add(this.txSubject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmEmail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTo;
        private System.Windows.Forms.ComboBox cmbFrom;
        private System.Windows.Forms.ComboBox cmbSMTPSrv;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox txBody;
        private System.Windows.Forms.Button btBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSubject;
        private System.Windows.Forms.TextBox txSubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStaticMessages;
        private System.Windows.Forms.Button btInsertExpression;
        private System.Windows.Forms.Label label5;
    }
}