namespace RocketApplication
{
    partial class AppForm
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
            labelRockets = new Label();
            listDataBase = new ListBox();
            listChanges = new ListBox();
            label2 = new Label();
            labelNextRockets = new Label();
            labeltextChanges = new Label();
            labelDataBase = new Label();
            plannedLabel = new Label();
            labelEmailSent = new Label();
            labelEmailPlannedRockets = new Label();
            testWarningEmail = new Button();
            testWeeklyEmail = new Button();
            SuspendLayout();
            // 
            // labelRockets
            // 
            labelRockets.AutoSize = true;
            labelRockets.BackColor = SystemColors.ButtonShadow;
            labelRockets.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelRockets.ForeColor = Color.Black;
            labelRockets.Location = new Point(80, 64);
            labelRockets.Margin = new Padding(2, 0, 2, 0);
            labelRockets.Name = "labelRockets";
            labelRockets.Size = new Size(0, 19);
            labelRockets.TabIndex = 1;
            // 
            // listDataBase
            // 
            listDataBase.FormattingEnabled = true;
            listDataBase.Location = new Point(22, 625);
            listDataBase.Margin = new Padding(2, 2, 2, 2);
            listDataBase.Name = "listDataBase";
            listDataBase.Size = new Size(870, 304);
            listDataBase.TabIndex = 2;
            // 
            // listChanges
            // 
            listChanges.FormattingEnabled = true;
            listChanges.Location = new Point(22, 438);
            listChanges.Margin = new Padding(2, 2, 2, 2);
            listChanges.Name = "listChanges";
            listChanges.Size = new Size(870, 124);
            listChanges.TabIndex = 6;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1402, 625);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 7;
            // 
            // labelNextRockets
            // 
            labelNextRockets.AutoSize = true;
            labelNextRockets.BackColor = SystemColors.ButtonShadow;
            labelNextRockets.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNextRockets.ForeColor = Color.Black;
            labelNextRockets.Location = new Point(928, 462);
            labelNextRockets.Margin = new Padding(2, 0, 2, 0);
            labelNextRockets.Name = "labelNextRockets";
            labelNextRockets.Size = new Size(0, 19);
            labelNextRockets.TabIndex = 1;
            // 
            // labeltextChanges
            // 
            labeltextChanges.AutoSize = true;
            labeltextChanges.BackColor = SystemColors.ButtonShadow;
            labeltextChanges.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labeltextChanges.ForeColor = Color.Black;
            labeltextChanges.Location = new Point(22, 411);
            labeltextChanges.Margin = new Padding(2, 0, 2, 0);
            labeltextChanges.Name = "labeltextChanges";
            labeltextChanges.Size = new Size(78, 19);
            labeltextChanges.TabIndex = 8;
            labeltextChanges.Text = "Changes:";
            // 
            // labelDataBase
            // 
            labelDataBase.AutoSize = true;
            labelDataBase.BackColor = SystemColors.ButtonShadow;
            labelDataBase.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDataBase.ForeColor = Color.Black;
            labelDataBase.Location = new Point(22, 602);
            labelDataBase.Margin = new Padding(2, 0, 2, 0);
            labelDataBase.Name = "labelDataBase";
            labelDataBase.Size = new Size(88, 19);
            labelDataBase.TabIndex = 9;
            labelDataBase.Text = "Data Base:";
            // 
            // plannedLabel
            // 
            plannedLabel.AutoSize = true;
            plannedLabel.BackColor = SystemColors.ButtonShadow;
            plannedLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            plannedLabel.ForeColor = Color.Maroon;
            plannedLabel.Location = new Point(928, 411);
            plannedLabel.Margin = new Padding(2, 0, 2, 0);
            plannedLabel.Name = "plannedLabel";
            plannedLabel.Size = new Size(280, 19);
            plannedLabel.TabIndex = 10;
            plannedLabel.Text = "Planned Rockets this and next week";
            // 
            // labelEmailSent
            // 
            labelEmailSent.AutoSize = true;
            labelEmailSent.BackColor = SystemColors.ButtonShadow;
            labelEmailSent.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEmailSent.ForeColor = Color.DarkRed;
            labelEmailSent.Location = new Point(624, 563);
            labelEmailSent.Margin = new Padding(2, 0, 2, 0);
            labelEmailSent.Name = "labelEmailSent";
            labelEmailSent.Size = new Size(0, 19);
            labelEmailSent.TabIndex = 11;
            // 
            // labelEmailPlannedRockets
            // 
            labelEmailPlannedRockets.AutoSize = true;
            labelEmailPlannedRockets.BackColor = SystemColors.ButtonShadow;
            labelEmailPlannedRockets.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEmailPlannedRockets.ForeColor = Color.DarkRed;
            labelEmailPlannedRockets.Location = new Point(1646, 908);
            labelEmailPlannedRockets.Margin = new Padding(2, 0, 2, 0);
            labelEmailPlannedRockets.Name = "labelEmailPlannedRockets";
            labelEmailPlannedRockets.Size = new Size(0, 19);
            labelEmailPlannedRockets.TabIndex = 12;
            // 
            // testWarningEmail
            // 
            testWarningEmail.Location = new Point(1090, 211);
            testWarningEmail.Name = "testWarningEmail";
            testWarningEmail.Size = new Size(180, 43);
            testWarningEmail.TabIndex = 13;
            testWarningEmail.Text = "Test warning email";
            testWarningEmail.UseVisualStyleBackColor = true;
            testWarningEmail.Click += testWarningEmail_Click;
            // 
            // testWeeklyEmail
            // 
            testWeeklyEmail.Location = new Point(1291, 211);
            testWeeklyEmail.Name = "testWeeklyEmail";
            testWeeklyEmail.Size = new Size(180, 43);
            testWeeklyEmail.TabIndex = 14;
            testWeeklyEmail.Text = "Test weekly email";
            testWeeklyEmail.UseVisualStyleBackColor = true;
            testWeeklyEmail.Click += testWeeklyEmail_Click;
            // 
            // AppForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(1539, 844);
            Controls.Add(testWeeklyEmail);
            Controls.Add(testWarningEmail);
            Controls.Add(labelEmailPlannedRockets);
            Controls.Add(labelEmailSent);
            Controls.Add(plannedLabel);
            Controls.Add(labelDataBase);
            Controls.Add(labeltextChanges);
            Controls.Add(labelNextRockets);
            Controls.Add(label2);
            Controls.Add(listChanges);
            Controls.Add(listDataBase);
            Controls.Add(labelRockets);
            Margin = new Padding(2, 2, 2, 2);
            Name = "AppForm";
            Text = "Rocket App";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label labelRockets;
        private ListBox listDataBase;
        private ListBox listChanges;
        private Label label2;
        private Label labelNextRockets;
        private Label labeltextChanges;
        private Label labelDataBase;
        private Label plannedLabel;
        private Label labelEmailSent;
        private Label labelEmailPlannedRockets;
        private Button testWarningEmail;
        private Button testWeeklyEmail;
    }
}
