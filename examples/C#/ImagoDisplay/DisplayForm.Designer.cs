namespace ImagoDisplay
{
    partial class DisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.lstDataEntity = new System.Windows.Forms.ComboBox();
            this.butDisplay = new System.Windows.Forms.Button();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.lstDataItems = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lstImageryTypes = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lstDataSeriesTypes = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lstDatasets = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lstProjects = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.butConnect = new System.Windows.Forms.Button();
            this.showProgress = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account name";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(209, 30);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(174, 20);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Text = "ImagoDemo1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(399, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(458, 30);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(174, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "ImagoDemo1";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // panelDisplay
            // 
            this.panelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.lstDataEntity);
            this.panelDisplay.Controls.Add(this.butDisplay);
            this.panelDisplay.Controls.Add(this.picDisplay);
            this.panelDisplay.Controls.Add(this.lstDataItems);
            this.panelDisplay.Controls.Add(this.label8);
            this.panelDisplay.Controls.Add(this.lstImageryTypes);
            this.panelDisplay.Controls.Add(this.label7);
            this.panelDisplay.Controls.Add(this.lstDataSeriesTypes);
            this.panelDisplay.Controls.Add(this.label6);
            this.panelDisplay.Controls.Add(this.label5);
            this.panelDisplay.Controls.Add(this.lstDatasets);
            this.panelDisplay.Controls.Add(this.label4);
            this.panelDisplay.Controls.Add(this.lstProjects);
            this.panelDisplay.Controls.Add(this.label3);
            this.panelDisplay.Location = new System.Drawing.Point(16, 80);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(988, 619);
            this.panelDisplay.TabIndex = 5;
            // 
            // lstDataEntity
            // 
            this.lstDataEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDataEntity.FormattingEnabled = true;
            this.lstDataEntity.Location = new System.Drawing.Point(114, 149);
            this.lstDataEntity.Name = "lstDataEntity";
            this.lstDataEntity.Size = new System.Drawing.Size(153, 21);
            this.lstDataEntity.TabIndex = 8;
            this.lstDataEntity.SelectedValueChanged += new System.EventHandler(this.lstDataEntity_SelectedValueChanged);
            // 
            // butDisplay
            // 
            this.butDisplay.Location = new System.Drawing.Point(192, 213);
            this.butDisplay.Name = "butDisplay";
            this.butDisplay.Size = new System.Drawing.Size(75, 23);
            this.butDisplay.TabIndex = 10;
            this.butDisplay.Text = "Display";
            this.butDisplay.UseVisualStyleBackColor = true;
            this.butDisplay.Click += new System.EventHandler(this.butDisplay_Click);
            // 
            // picDisplay
            // 
            this.picDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picDisplay.Location = new System.Drawing.Point(286, 3);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(697, 611);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 14;
            this.picDisplay.TabStop = false;
            // 
            // lstDataItems
            // 
            this.lstDataItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDataItems.FormattingEnabled = true;
            this.lstDataItems.Location = new System.Drawing.Point(114, 176);
            this.lstDataItems.Name = "lstDataItems";
            this.lstDataItems.Size = new System.Drawing.Size(153, 21);
            this.lstDataItems.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Data Items";
            // 
            // lstImageryTypes
            // 
            this.lstImageryTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstImageryTypes.FormattingEnabled = true;
            this.lstImageryTypes.Location = new System.Drawing.Point(114, 94);
            this.lstImageryTypes.Name = "lstImageryTypes";
            this.lstImageryTypes.Size = new System.Drawing.Size(153, 21);
            this.lstImageryTypes.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Imagery Type";
            // 
            // lstDataSeriesTypes
            // 
            this.lstDataSeriesTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDataSeriesTypes.FormattingEnabled = true;
            this.lstDataSeriesTypes.Location = new System.Drawing.Point(114, 67);
            this.lstDataSeriesTypes.Name = "lstDataSeriesTypes";
            this.lstDataSeriesTypes.Size = new System.Drawing.Size(153, 21);
            this.lstDataSeriesTypes.TabIndex = 6;
            this.lstDataSeriesTypes.SelectedValueChanged += new System.EventHandler(this.lstDataSeriesTypes_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Data Series Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Data Entity";
            // 
            // lstDatasets
            // 
            this.lstDatasets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDatasets.FormattingEnabled = true;
            this.lstDatasets.Location = new System.Drawing.Point(114, 40);
            this.lstDatasets.Name = "lstDatasets";
            this.lstDatasets.Size = new System.Drawing.Size(153, 21);
            this.lstDatasets.TabIndex = 5;
            this.lstDatasets.SelectedValueChanged += new System.EventHandler(this.lstDatasets_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Dataset";
            // 
            // lstProjects
            // 
            this.lstProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstProjects.FormattingEnabled = true;
            this.lstProjects.Location = new System.Drawing.Point(114, 13);
            this.lstProjects.Name = "lstProjects";
            this.lstProjects.Size = new System.Drawing.Size(153, 21);
            this.lstProjects.TabIndex = 4;
            this.lstProjects.SelectedValueChanged += new System.EventHandler(this.lstProjects_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Project";
            // 
            // butConnect
            // 
            this.butConnect.Location = new System.Drawing.Point(654, 28);
            this.butConnect.Name = "butConnect";
            this.butConnect.Size = new System.Drawing.Size(75, 23);
            this.butConnect.TabIndex = 3;
            this.butConnect.Text = "Connect";
            this.butConnect.UseVisualStyleBackColor = true;
            this.butConnect.Click += new System.EventHandler(this.butConnect_Click);
            // 
            // showProgress
            // 
            this.showProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showProgress.Location = new System.Drawing.Point(736, 27);
            this.showProgress.Name = "showProgress";
            this.showProgress.Size = new System.Drawing.Size(268, 23);
            this.showProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.showProgress.TabIndex = 6;
            this.showProgress.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(16, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // DisplayForm
            // 
            this.AcceptButton = this.butConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 711);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.showProgress);
            this.Controls.Add(this.panelDisplay);
            this.Controls.Add(this.butConnect);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label1);
            this.Name = "DisplayForm";
            this.Text = "Imago Display";
            this.Load += new System.EventHandler(this.DisplayForm_Load);
            this.panelDisplay.ResumeLayout(false);
            this.panelDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.ComboBox lstDataItems;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox lstImageryTypes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox lstDataSeriesTypes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox lstDatasets;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox lstProjects;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butDisplay;
        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.Button butConnect;
        private System.Windows.Forms.ComboBox lstDataEntity;
        private System.Windows.Forms.ProgressBar showProgress;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

