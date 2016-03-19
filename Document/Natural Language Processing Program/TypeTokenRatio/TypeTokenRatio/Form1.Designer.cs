namespace TypeTokenRatio
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTokens = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTypes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTTR = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btnGo
            // 
            this.btnGo.AutoSize = true;
            this.btnGo.Location = new System.Drawing.Point(26, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(113, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Select a File and Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of words (Tokens)";
            // 
            // textBoxTokens
            // 
            this.textBoxTokens.Location = new System.Drawing.Point(197, 113);
            this.textBoxTokens.Name = "textBoxTokens";
            this.textBoxTokens.Size = new System.Drawing.Size(100, 20);
            this.textBoxTokens.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of distinct words (Types)";
            // 
            // textBoxTypes
            // 
            this.textBoxTypes.Location = new System.Drawing.Point(197, 146);
            this.textBoxTypes.Name = "textBoxTypes";
            this.textBoxTypes.Size = new System.Drawing.Size(100, 20);
            this.textBoxTypes.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Type token ratio";
            // 
            // textBoxTTR
            // 
            this.textBoxTTR.Location = new System.Drawing.Point(197, 186);
            this.textBoxTTR.Name = "textBoxTTR";
            this.textBoxTTR.Size = new System.Drawing.Size(100, 20);
            this.textBoxTTR.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "File";
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(52, 57);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(245, 20);
            this.textBoxFile.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 236);
            this.Controls.Add(this.textBoxTTR);
            this.Controls.Add(this.textBoxTypes);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.textBoxTokens);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGo);
            this.Name = "Form1";
            this.Text = "Type token ratio computer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTokens;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTypes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTTR;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFile;
    }
}

