namespace Test_Levenstein
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
            this.button1 = new System.Windows.Forms.Button();
            this.addWordTextbox = new System.Windows.Forms.TextBox();
            this.suggestionbox = new System.Windows.Forms.ListBox();
            this.SuggestingButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.DictionaryLabel = new System.Windows.Forms.Label();
            this.DictionaryTextBox = new System.Windows.Forms.TextBox();
            this.DictionaryButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 533);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // addWordTextbox
            // 
            this.addWordTextbox.Location = new System.Drawing.Point(24, 536);
            this.addWordTextbox.Name = "addWordTextbox";
            this.addWordTextbox.Size = new System.Drawing.Size(197, 20);
            this.addWordTextbox.TabIndex = 22;
            // 
            // suggestionbox
            // 
            this.suggestionbox.FormattingEnabled = true;
            this.suggestionbox.Location = new System.Drawing.Point(325, 359);
            this.suggestionbox.Name = "suggestionbox";
            this.suggestionbox.Size = new System.Drawing.Size(389, 225);
            this.suggestionbox.TabIndex = 21;
            // 
            // SuggestingButton
            // 
            this.SuggestingButton.Enabled = false;
            this.SuggestingButton.Location = new System.Drawing.Point(325, 316);
            this.SuggestingButton.Name = "SuggestingButton";
            this.SuggestingButton.Size = new System.Drawing.Size(104, 23);
            this.SuggestingButton.TabIndex = 20;
            this.SuggestingButton.Text = "Begin Suggesting";
            this.SuggestingButton.UseVisualStyleBackColor = true;
            this.SuggestingButton.Click += new System.EventHandler(this.SuggestingButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(326, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Browse for the file to check...";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(325, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(182, 20);
            this.textBox1.TabIndex = 18;
            // 
            // checkButton
            // 
            this.checkButton.Enabled = false;
            this.checkButton.Location = new System.Drawing.Point(513, 26);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 17;
            this.checkButton.Text = "Browse";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(325, 57);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(389, 253);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = "";
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(22, 57);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(259, 472);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // DictionaryLabel
            // 
            this.DictionaryLabel.AutoSize = true;
            this.DictionaryLabel.Location = new System.Drawing.Point(19, 16);
            this.DictionaryLabel.Name = "DictionaryLabel";
            this.DictionaryLabel.Size = new System.Drawing.Size(148, 13);
            this.DictionaryLabel.TabIndex = 15;
            this.DictionaryLabel.Text = "Browse for the dictionary file...";
            // 
            // DictionaryTextBox
            // 
            this.DictionaryTextBox.Location = new System.Drawing.Point(24, 30);
            this.DictionaryTextBox.Name = "DictionaryTextBox";
            this.DictionaryTextBox.Size = new System.Drawing.Size(182, 20);
            this.DictionaryTextBox.TabIndex = 13;
            // 
            // DictionaryButton
            // 
            this.DictionaryButton.Location = new System.Drawing.Point(209, 28);
            this.DictionaryButton.Name = "DictionaryButton";
            this.DictionaryButton.Size = new System.Drawing.Size(75, 23);
            this.DictionaryButton.TabIndex = 12;
            this.DictionaryButton.Text = "Browse";
            this.DictionaryButton.UseVisualStyleBackColor = true;
            this.DictionaryButton.Click += new System.EventHandler(this.DictionaryButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 603);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addWordTextbox);
            this.Controls.Add(this.suggestionbox);
            this.Controls.Add(this.SuggestingButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.DictionaryLabel);
            this.Controls.Add(this.DictionaryTextBox);
            this.Controls.Add(this.DictionaryButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox addWordTextbox;
        private System.Windows.Forms.ListBox suggestionbox;
        private System.Windows.Forms.Button SuggestingButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label DictionaryLabel;
        private System.Windows.Forms.TextBox DictionaryTextBox;
        private System.Windows.Forms.Button DictionaryButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}

