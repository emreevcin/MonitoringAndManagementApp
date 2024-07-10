namespace SettingsApplication
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxService = new System.Windows.Forms.ComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.textBoxTimeInterval = new System.Windows.Forms.TextBox();
            this.labelTimeInterval = new System.Windows.Forms.Label();
            this.textBoxNumberOfRuns = new System.Windows.Forms.TextBox();
            this.labelNumberOfRuns = new System.Windows.Forms.Label();
            this.comboBoxLogLevel = new System.Windows.Forms.ComboBox();
            this.labelLogLevel = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelFolderPath = new System.Windows.Forms.Label();
            this.textBoxFolderPath = new System.Windows.Forms.TextBox();
            this.btnFolderPath = new System.Windows.Forms.Button();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // comboBoxService
            this.comboBoxService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxService.FormattingEnabled = true;
            this.comboBoxService.Location = new System.Drawing.Point(160, 30);
            this.comboBoxService.Name = "comboBoxService";
            this.comboBoxService.Size = new System.Drawing.Size(200, 21);
            this.comboBoxService.TabIndex = 0;

            // labelService
            this.labelService.AutoSize = true;
            this.labelService.Location = new System.Drawing.Point(50, 33);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(74, 13);
            this.labelService.TabIndex = 1;
            this.labelService.Text = "Service Name";

            // textBoxTimeInterval
            this.textBoxTimeInterval.Location = new System.Drawing.Point(160, 70);
            this.textBoxTimeInterval.Name = "textBoxTimeInterval";
            this.textBoxTimeInterval.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimeInterval.TabIndex = 2;

            // labelTimeInterval
            this.labelTimeInterval.AutoSize = true;
            this.labelTimeInterval.Location = new System.Drawing.Point(50, 73);
            this.labelTimeInterval.Name = "labelTimeInterval";
            this.labelTimeInterval.Size = new System.Drawing.Size(93, 13);
            this.labelTimeInterval.TabIndex = 3;
            this.labelTimeInterval.Text = "Monitor Interval (s)";

            // textBoxNumberOfRuns
            this.textBoxNumberOfRuns.Location = new System.Drawing.Point(160, 110);
            this.textBoxNumberOfRuns.Name = "textBoxNumberOfRuns";
            this.textBoxNumberOfRuns.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumberOfRuns.TabIndex = 4;

            // labelNumberOfRuns
            this.labelNumberOfRuns.AutoSize = true;
            this.labelNumberOfRuns.Location = new System.Drawing.Point(50, 113);
            this.labelNumberOfRuns.Name = "labelNumberOfRuns";
            this.labelNumberOfRuns.Size = new System.Drawing.Size(91, 13);
            this.labelNumberOfRuns.TabIndex = 5;
            this.labelNumberOfRuns.Text = "Number of Runs";

            // comboBoxLogLevel
            this.comboBoxLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogLevel.FormattingEnabled = true;
            this.comboBoxLogLevel.Items.AddRange(new object[] { "Debug", "Information", "Warning", "Error", "Fatal" });
            this.comboBoxLogLevel.Location = new System.Drawing.Point(160, 150);
            this.comboBoxLogLevel.Name = "comboBoxLogLevel";
            this.comboBoxLogLevel.Size = new System.Drawing.Size(100, 21);
            this.comboBoxLogLevel.TabIndex = 6;

            // labelLogLevel
            this.labelLogLevel.AutoSize = true;
            this.labelLogLevel.Location = new System.Drawing.Point(50, 153);
            this.labelLogLevel.Name = "labelLogLevel";
            this.labelLogLevel.Size = new System.Drawing.Size(58, 13);
            this.labelLogLevel.TabIndex = 7;
            this.labelLogLevel.Text = "Log Level";

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(160, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // labelFolderPath
            this.labelFolderPath.AutoSize = true;
            this.labelFolderPath.Location = new System.Drawing.Point(50, 193);
            this.labelFolderPath.Name = "labelFolderPath";
            this.labelFolderPath.Size = new System.Drawing.Size(61, 13);
            this.labelFolderPath.TabIndex = 9;
            this.labelFolderPath.Text = "Folder Path";
            this.labelFolderPath.Visible = false;

            // textBoxFolderPath
            this.textBoxFolderPath.Location = new System.Drawing.Point(160, 190);
            this.textBoxFolderPath.Name = "textBoxFolderPath";
            this.textBoxFolderPath.Size = new System.Drawing.Size(180, 20);
            this.textBoxFolderPath.TabIndex = 10;
            this.textBoxFolderPath.Visible = false;

            // btnFolderPath
            this.btnFolderPath.Location = new System.Drawing.Point(350, 188);
            this.btnFolderPath.Name = "btnFolderPath";
            this.btnFolderPath.Size = new System.Drawing.Size(35, 23);
            this.btnFolderPath.TabIndex = 11;
            this.btnFolderPath.Text = "...";
            this.btnFolderPath.UseVisualStyleBackColor = true;
            this.btnFolderPath.Visible = false;

            // labelUrl
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(50, 193);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(20, 13);
            this.labelUrl.TabIndex = 12;
            this.labelUrl.Text = "Url";
            this.labelUrl.Visible = false;

            // textBoxUrl
            this.textBoxUrl.Location = new System.Drawing.Point(160, 190);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(180, 20);
            this.textBoxUrl.TabIndex = 13;
            this.textBoxUrl.Visible = false;

            // SettingsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 281);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.btnFolderPath);
            this.Controls.Add(this.textBoxFolderPath);
            this.Controls.Add(this.labelFolderPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelLogLevel);
            this.Controls.Add(this.comboBoxLogLevel);
            this.Controls.Add(this.labelNumberOfRuns);
            this.Controls.Add(this.textBoxNumberOfRuns);
            this.Controls.Add(this.labelTimeInterval);
            this.Controls.Add(this.textBoxTimeInterval);
            this.Controls.Add(this.labelService);
            this.Controls.Add(this.comboBoxService);
            this.Name = "SettingsForm";
            this.Text = "Settings Form";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox comboBoxService;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.TextBox textBoxTimeInterval;
        private System.Windows.Forms.Label labelTimeInterval;
        private System.Windows.Forms.TextBox textBoxNumberOfRuns;
        private System.Windows.Forms.Label labelNumberOfRuns;
        private System.Windows.Forms.ComboBox comboBoxLogLevel;
        private System.Windows.Forms.Label labelLogLevel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelFolderPath;
        private System.Windows.Forms.TextBox textBoxFolderPath;
        private System.Windows.Forms.Button btnFolderPath;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
    }
}
