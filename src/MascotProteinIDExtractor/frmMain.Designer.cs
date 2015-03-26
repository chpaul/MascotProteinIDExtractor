namespace MascotProteinIDExtractor
{
    partial class frmMain
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
            this.btnDATBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDAT = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtRaw = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRawBrowse = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtMinMascotScore = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDATBrowse
            // 
            this.btnDATBrowse.Location = new System.Drawing.Point(539, 17);
            this.btnDATBrowse.Name = "btnDATBrowse";
            this.btnDATBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnDATBrowse.TabIndex = 0;
            this.btnDATBrowse.Text = "...";
            this.btnDATBrowse.UseVisualStyleBackColor = true;
            this.btnDATBrowse.Click += new System.EventHandler(this.btnDATBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mascot DAT file:";
            // 
            // txtDAT
            // 
            this.txtDAT.Location = new System.Drawing.Point(104, 17);
            this.txtDAT.Name = "txtDAT";
            this.txtDAT.Size = new System.Drawing.Size(429, 20);
            this.txtDAT.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtRaw
            // 
            this.txtRaw.Location = new System.Drawing.Point(104, 43);
            this.txtRaw.Name = "txtRaw";
            this.txtRaw.Size = new System.Drawing.Size(429, 20);
            this.txtRaw.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Raw File:";
            // 
            // btnRawBrowse
            // 
            this.btnRawBrowse.Location = new System.Drawing.Point(539, 42);
            this.btnRawBrowse.Name = "btnRawBrowse";
            this.btnRawBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnRawBrowse.TabIndex = 5;
            this.btnRawBrowse.Text = "...";
            this.btnRawBrowse.UseVisualStyleBackColor = true;
            this.btnRawBrowse.Click += new System.EventHandler(this.btnRawBrowse_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(493, 71);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 6;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // txtMinMascotScore
            // 
            this.txtMinMascotScore.Location = new System.Drawing.Point(142, 68);
            this.txtMinMascotScore.Name = "txtMinMascotScore";
            this.txtMinMascotScore.Size = new System.Drawing.Size(44, 20);
            this.txtMinMascotScore.TabIndex = 7;
            this.txtMinMascotScore.Text = "15";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Minimum Mascot Score:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 100);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMinMascotScore);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnRawBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRaw);
            this.Controls.Add(this.txtDAT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDATBrowse);
            this.Name = "frmMain";
            this.Text = "Mascot Protein ID result Extractor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDATBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDAT;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtRaw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRawBrowse;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtMinMascotScore;
        private System.Windows.Forms.Label label3;
    }
}

