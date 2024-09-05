namespace AssistanceReporting
{
    partial class FormInit
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
            ButtomFileBiotimer = new Button();
            DataGridViewFileBiotimer = new DataGridView();
            ButtonFileAssistance = new Button();
            DataGridViewFileAssistance = new DataGridView();
            ButtonGenerateReport = new Button();
            FileNameBiotimer = new TextBox();
            FileNameAssistance = new TextBox();
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileBiotimer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileAssistance).BeginInit();
            SuspendLayout();
            // 
            // ButtomFileBiotimer
            // 
            ButtomFileBiotimer.Location = new Point(31, 29);
            ButtomFileBiotimer.Name = "ButtomFileBiotimer";
            ButtomFileBiotimer.Size = new Size(120, 30);
            ButtomFileBiotimer.TabIndex = 0;
            ButtomFileBiotimer.Text = "Archivo Biotimer";
            ButtomFileBiotimer.UseVisualStyleBackColor = true;
            ButtomFileBiotimer.Click += ButtomFileBiotimer_Click;
            // 
            // DataGridViewFileBiotimer
            // 
            DataGridViewFileBiotimer.AllowUserToAddRows = false;
            DataGridViewFileBiotimer.AllowUserToDeleteRows = false;
            DataGridViewFileBiotimer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewFileBiotimer.Location = new Point(31, 74);
            DataGridViewFileBiotimer.Name = "DataGridViewFileBiotimer";
            DataGridViewFileBiotimer.ReadOnly = true;
            DataGridViewFileBiotimer.RowTemplate.Height = 25;
            DataGridViewFileBiotimer.Size = new Size(725, 150);
            DataGridViewFileBiotimer.TabIndex = 1;
            // 
            // ButtonFileAssistance
            // 
            ButtonFileAssistance.Location = new Point(31, 242);
            ButtonFileAssistance.Name = "ButtonFileAssistance";
            ButtonFileAssistance.Size = new Size(120, 30);
            ButtonFileAssistance.TabIndex = 2;
            ButtonFileAssistance.Text = "Archivo Asistencia";
            ButtonFileAssistance.UseVisualStyleBackColor = true;
            ButtonFileAssistance.Click += ButtonFileAssistance_Click;
            // 
            // DataGridViewFileAssistance
            // 
            DataGridViewFileAssistance.AllowUserToAddRows = false;
            DataGridViewFileAssistance.AllowUserToDeleteRows = false;
            DataGridViewFileAssistance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewFileAssistance.Location = new Point(31, 289);
            DataGridViewFileAssistance.Name = "DataGridViewFileAssistance";
            DataGridViewFileAssistance.ReadOnly = true;
            DataGridViewFileAssistance.RowTemplate.Height = 25;
            DataGridViewFileAssistance.Size = new Size(725, 150);
            DataGridViewFileAssistance.TabIndex = 3;
            // 
            // ButtonGenerateReport
            // 
            ButtonGenerateReport.Location = new Point(31, 458);
            ButtonGenerateReport.Name = "ButtonGenerateReport";
            ButtonGenerateReport.Size = new Size(120, 30);
            ButtonGenerateReport.TabIndex = 4;
            ButtonGenerateReport.Text = "Generar Reporte";
            ButtonGenerateReport.UseVisualStyleBackColor = true;
            ButtonGenerateReport.Click += ButtonGenerateReport_Click;
            // 
            // FileNameBiotimer
            // 
            FileNameBiotimer.Enabled = false;
            FileNameBiotimer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FileNameBiotimer.Location = new Point(171, 30);
            FileNameBiotimer.Name = "FileNameBiotimer";
            FileNameBiotimer.Size = new Size(330, 29);
            FileNameBiotimer.TabIndex = 5;
            // 
            // FileNameAssistance
            // 
            FileNameAssistance.Enabled = false;
            FileNameAssistance.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FileNameAssistance.Location = new Point(171, 242);
            FileNameAssistance.Name = "FileNameAssistance";
            FileNameAssistance.Size = new Size(330, 29);
            FileNameAssistance.TabIndex = 6;
            // 
            // FormInit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(781, 512);
            Controls.Add(FileNameAssistance);
            Controls.Add(FileNameBiotimer);
            Controls.Add(ButtonGenerateReport);
            Controls.Add(DataGridViewFileAssistance);
            Controls.Add(ButtonFileAssistance);
            Controls.Add(DataGridViewFileBiotimer);
            Controls.Add(ButtomFileBiotimer);
            Name = "FormInit";
            Text = "AssistanceReporting";
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileBiotimer).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileAssistance).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtomFileBiotimer;
        private DataGridView DataGridViewFileBiotimer;
        private Button ButtonFileAssistance;
        private DataGridView DataGridViewFileAssistance;
        private Button ButtonGenerateReport;
        private TextBox FileNameBiotimer;
        private TextBox FileNameAssistance;
    }
}
