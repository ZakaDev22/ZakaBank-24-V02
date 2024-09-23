namespace ZakaBank_24.Client_Forms
{
    partial class ctrlClientinfoCardWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbFilters = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtFilterBy = new Guna.UI2.WinForms.Guna2TextBox();
            this.gbPersoInfo = new Guna.UI2.WinForms.Guna2GroupBox();
            this.gbClientInfo = new Guna.UI2.WinForms.Guna2GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbClientID = new System.Windows.Forms.Label();
            this.lbAccountNo = new System.Windows.Forms.Label();
            this.lbPinCode = new System.Windows.Forms.Label();
            this.lbIsDeleted = new System.Windows.Forms.Label();
            this.lbAccountType = new System.Windows.Forms.Label();
            this.lbAddedBy = new System.Windows.Forms.Label();
            this.lbBalance = new System.Windows.Forms.Label();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2GradientCircleButton();
            this.btnFind = new Guna.UI2.WinForms.Guna2GradientCircleButton();
            this.ctrlPersonInfoCard1 = new ZakaBank_24.People_Forms.ctrlPersonInfoCard();
            this.gbFilters.SuspendLayout();
            this.gbPersoInfo.SuspendLayout();
            this.gbClientInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFilters
            // 
            this.gbFilters.BorderColor = System.Drawing.Color.Black;
            this.gbFilters.BorderRadius = 20;
            this.gbFilters.BorderThickness = 5;
            this.gbFilters.Controls.Add(this.btnAddNew);
            this.gbFilters.Controls.Add(this.btnFind);
            this.gbFilters.Controls.Add(this.txtFilterBy);
            this.gbFilters.CustomBorderColor = System.Drawing.Color.Silver;
            this.gbFilters.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFilters.ForeColor = System.Drawing.Color.Black;
            this.gbFilters.Location = new System.Drawing.Point(26, 3);
            this.gbFilters.Name = "gbFilters";
            this.gbFilters.Size = new System.Drawing.Size(794, 115);
            this.gbFilters.TabIndex = 1;
            this.gbFilters.Text = "Client Filter Card";
            this.gbFilters.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFilterBy
            // 
            this.txtFilterBy.Animated = true;
            this.txtFilterBy.AutoRoundedCorners = true;
            this.txtFilterBy.BackColor = System.Drawing.Color.Transparent;
            this.txtFilterBy.BorderColor = System.Drawing.Color.Black;
            this.txtFilterBy.BorderRadius = 20;
            this.txtFilterBy.BorderThickness = 2;
            this.txtFilterBy.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilterBy.DefaultText = "";
            this.txtFilterBy.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtFilterBy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtFilterBy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFilterBy.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFilterBy.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFilterBy.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.txtFilterBy.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFilterBy.Location = new System.Drawing.Point(232, 56);
            this.txtFilterBy.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtFilterBy.Name = "txtFilterBy";
            this.txtFilterBy.PasswordChar = '\0';
            this.txtFilterBy.PlaceholderText = "Enter The Client ID Here";
            this.txtFilterBy.SelectedText = "";
            this.txtFilterBy.Size = new System.Drawing.Size(286, 42);
            this.txtFilterBy.TabIndex = 0;
            this.txtFilterBy.TextChanged += new System.EventHandler(this.txtFilterBy_TextChanged);
            this.txtFilterBy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterBy_KeyPress);
            // 
            // gbPersoInfo
            // 
            this.gbPersoInfo.BorderColor = System.Drawing.Color.Red;
            this.gbPersoInfo.BorderRadius = 20;
            this.gbPersoInfo.BorderThickness = 2;
            this.gbPersoInfo.Controls.Add(this.ctrlPersonInfoCard1);
            this.gbPersoInfo.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.gbPersoInfo.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPersoInfo.ForeColor = System.Drawing.Color.DarkRed;
            this.gbPersoInfo.Location = new System.Drawing.Point(3, 124);
            this.gbPersoInfo.Name = "gbPersoInfo";
            this.gbPersoInfo.Size = new System.Drawing.Size(854, 374);
            this.gbPersoInfo.TabIndex = 2;
            this.gbPersoInfo.Text = "Person Info";
            // 
            // gbClientInfo
            // 
            this.gbClientInfo.BorderColor = System.Drawing.Color.Lime;
            this.gbClientInfo.BorderRadius = 20;
            this.gbClientInfo.BorderThickness = 2;
            this.gbClientInfo.Controls.Add(this.label6);
            this.gbClientInfo.Controls.Add(this.label3);
            this.gbClientInfo.Controls.Add(this.label5);
            this.gbClientInfo.Controls.Add(this.label2);
            this.gbClientInfo.Controls.Add(this.lbBalance);
            this.gbClientInfo.Controls.Add(this.lbAddedBy);
            this.gbClientInfo.Controls.Add(this.lbAccountType);
            this.gbClientInfo.Controls.Add(this.lbIsDeleted);
            this.gbClientInfo.Controls.Add(this.lbPinCode);
            this.gbClientInfo.Controls.Add(this.lbAccountNo);
            this.gbClientInfo.Controls.Add(this.lbClientID);
            this.gbClientInfo.Controls.Add(this.label7);
            this.gbClientInfo.Controls.Add(this.label4);
            this.gbClientInfo.Controls.Add(this.label1);
            this.gbClientInfo.CustomBorderColor = System.Drawing.Color.MediumSpringGreen;
            this.gbClientInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbClientInfo.ForeColor = System.Drawing.Color.DarkRed;
            this.gbClientInfo.Location = new System.Drawing.Point(7, 504);
            this.gbClientInfo.Name = "gbClientInfo";
            this.gbClientInfo.Size = new System.Drawing.Size(854, 154);
            this.gbClientInfo.TabIndex = 3;
            this.gbClientInfo.Text = "Client Info";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(31, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client ID : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(15, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Account No : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(29, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pin Code  : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(307, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Is Deleted  : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(576, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "Acc. Type : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(576, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 19);
            this.label6.TabIndex = 0;
            this.label6.Text = "Added By :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(584, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 19);
            this.label7.TabIndex = 0;
            this.label7.Text = "Balance : ";
            // 
            // lbClientID
            // 
            this.lbClientID.AutoSize = true;
            this.lbClientID.BackColor = System.Drawing.Color.Transparent;
            this.lbClientID.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbClientID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbClientID.Location = new System.Drawing.Point(131, 61);
            this.lbClientID.Name = "lbClientID";
            this.lbClientID.Size = new System.Drawing.Size(66, 18);
            this.lbClientID.TabIndex = 0;
            this.lbClientID.Text = "[????]";
            // 
            // lbAccountNo
            // 
            this.lbAccountNo.AutoSize = true;
            this.lbAccountNo.BackColor = System.Drawing.Color.Transparent;
            this.lbAccountNo.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAccountNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbAccountNo.Location = new System.Drawing.Point(131, 92);
            this.lbAccountNo.Name = "lbAccountNo";
            this.lbAccountNo.Size = new System.Drawing.Size(66, 18);
            this.lbAccountNo.TabIndex = 0;
            this.lbAccountNo.Text = "[????]";
            // 
            // lbPinCode
            // 
            this.lbPinCode.AutoSize = true;
            this.lbPinCode.BackColor = System.Drawing.Color.Transparent;
            this.lbPinCode.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPinCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbPinCode.Location = new System.Drawing.Point(131, 121);
            this.lbPinCode.Name = "lbPinCode";
            this.lbPinCode.Size = new System.Drawing.Size(66, 18);
            this.lbPinCode.TabIndex = 0;
            this.lbPinCode.Text = "[????]";
            // 
            // lbIsDeleted
            // 
            this.lbIsDeleted.AutoSize = true;
            this.lbIsDeleted.BackColor = System.Drawing.Color.Transparent;
            this.lbIsDeleted.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIsDeleted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbIsDeleted.Location = new System.Drawing.Point(420, 61);
            this.lbIsDeleted.Name = "lbIsDeleted";
            this.lbIsDeleted.Size = new System.Drawing.Size(66, 18);
            this.lbIsDeleted.TabIndex = 0;
            this.lbIsDeleted.Text = "[????]";
            // 
            // lbAccountType
            // 
            this.lbAccountType.AutoSize = true;
            this.lbAccountType.BackColor = System.Drawing.Color.Transparent;
            this.lbAccountType.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAccountType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbAccountType.Location = new System.Drawing.Point(681, 92);
            this.lbAccountType.Name = "lbAccountType";
            this.lbAccountType.Size = new System.Drawing.Size(66, 18);
            this.lbAccountType.TabIndex = 0;
            this.lbAccountType.Text = "[????]";
            // 
            // lbAddedBy
            // 
            this.lbAddedBy.AutoSize = true;
            this.lbAddedBy.BackColor = System.Drawing.Color.Transparent;
            this.lbAddedBy.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAddedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbAddedBy.Location = new System.Drawing.Point(681, 121);
            this.lbAddedBy.Name = "lbAddedBy";
            this.lbAddedBy.Size = new System.Drawing.Size(66, 18);
            this.lbAddedBy.TabIndex = 0;
            this.lbAddedBy.Text = "[????]";
            // 
            // lbBalance
            // 
            this.lbBalance.AutoSize = true;
            this.lbBalance.BackColor = System.Drawing.Color.Transparent;
            this.lbBalance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbBalance.Location = new System.Drawing.Point(681, 61);
            this.lbBalance.Name = "lbBalance";
            this.lbBalance.Size = new System.Drawing.Size(66, 18);
            this.lbBalance.TabIndex = 0;
            this.lbBalance.Text = "[????]";
            // 
            // btnAddNew
            // 
            this.btnAddNew.BackColor = System.Drawing.Color.Transparent;
            this.btnAddNew.BorderThickness = 1;
            this.btnAddNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddNew.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddNew.FillColor2 = System.Drawing.Color.Maroon;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddNew.ForeColor = System.Drawing.Color.White;
            this.btnAddNew.Image = global::ZakaBank_24.Properties.Resources.Add_128;
            this.btnAddNew.ImageSize = new System.Drawing.Size(39, 35);
            this.btnAddNew.Location = new System.Drawing.Point(588, 46);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnAddNew.Size = new System.Drawing.Size(56, 52);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BorderThickness = 1;
            this.btnFind.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFind.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFind.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFind.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFind.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFind.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnFind.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Image = global::ZakaBank_24.Properties.Resources.Search_32;
            this.btnFind.ImageSize = new System.Drawing.Size(35, 35);
            this.btnFind.Location = new System.Drawing.Point(526, 46);
            this.btnFind.Name = "btnFind";
            this.btnFind.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnFind.Size = new System.Drawing.Size(56, 52);
            this.btnFind.TabIndex = 1;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // ctrlPersonInfoCard1
            // 
            this.ctrlPersonInfoCard1.Location = new System.Drawing.Point(4, 44);
            this.ctrlPersonInfoCard1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlPersonInfoCard1.Name = "ctrlPersonInfoCard1";
            this.ctrlPersonInfoCard1.Size = new System.Drawing.Size(842, 320);
            this.ctrlPersonInfoCard1.TabIndex = 0;
            // 
            // ctrlClientinfoCardWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbClientInfo);
            this.Controls.Add(this.gbPersoInfo);
            this.Controls.Add(this.gbFilters);
            this.Name = "ctrlClientinfoCardWithFilter";
            this.Size = new System.Drawing.Size(864, 658);
            this.gbFilters.ResumeLayout(false);
            this.gbPersoInfo.ResumeLayout(false);
            this.gbClientInfo.ResumeLayout(false);
            this.gbClientInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GroupBox gbFilters;
        private Guna.UI2.WinForms.Guna2GradientCircleButton btnAddNew;
        private Guna.UI2.WinForms.Guna2GradientCircleButton btnFind;
        private Guna.UI2.WinForms.Guna2TextBox txtFilterBy;
        private Guna.UI2.WinForms.Guna2GroupBox gbPersoInfo;
        private People_Forms.ctrlPersonInfoCard ctrlPersonInfoCard1;
        private Guna.UI2.WinForms.Guna2GroupBox gbClientInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbBalance;
        private System.Windows.Forms.Label lbAddedBy;
        private System.Windows.Forms.Label lbAccountType;
        private System.Windows.Forms.Label lbIsDeleted;
        private System.Windows.Forms.Label lbPinCode;
        private System.Windows.Forms.Label lbAccountNo;
        private System.Windows.Forms.Label lbClientID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
    }
}
