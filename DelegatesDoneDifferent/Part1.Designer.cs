namespace DelegatesDoneDifferent
{
    partial class Part1
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
            this.btnExample = new System.Windows.Forms.Button();
            this.btnExample2 = new System.Windows.Forms.Button();
            this.btnExample3 = new System.Windows.Forms.Button();
            this.btnExample4 = new System.Windows.Forms.Button();
            this.btnExample5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExample
            // 
            this.btnExample.Location = new System.Drawing.Point(15, 15);
            this.btnExample.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExample.Name = "btnExample";
            this.btnExample.Size = new System.Drawing.Size(119, 29);
            this.btnExample.TabIndex = 0;
            this.btnExample.Text = "Example 1.1";
            this.btnExample.UseVisualStyleBackColor = true;
            this.btnExample.Click += new System.EventHandler(this.btnExample_Click);
            // 
            // btnExample2
            // 
            this.btnExample2.Location = new System.Drawing.Point(15, 51);
            this.btnExample2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExample2.Name = "btnExample2";
            this.btnExample2.Size = new System.Drawing.Size(119, 29);
            this.btnExample2.TabIndex = 1;
            this.btnExample2.Text = "Example 1.2";
            this.btnExample2.UseVisualStyleBackColor = true;
            this.btnExample2.Click += new System.EventHandler(this.btnExample2_Click);
            // 
            // btnExample3
            // 
            this.btnExample3.Location = new System.Drawing.Point(15, 88);
            this.btnExample3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExample3.Name = "btnExample3";
            this.btnExample3.Size = new System.Drawing.Size(119, 29);
            this.btnExample3.TabIndex = 2;
            this.btnExample3.Text = "Example 1.3";
            this.btnExample3.UseVisualStyleBackColor = true;
            this.btnExample3.Click += new System.EventHandler(this.btnExample3_Click);
            // 
            // btnExample4
            // 
            this.btnExample4.Location = new System.Drawing.Point(15, 124);
            this.btnExample4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExample4.Name = "btnExample4";
            this.btnExample4.Size = new System.Drawing.Size(119, 29);
            this.btnExample4.TabIndex = 3;
            this.btnExample4.Text = "Example 1.4";
            this.btnExample4.UseVisualStyleBackColor = true;
            this.btnExample4.Click += new System.EventHandler(this.btnExample4_Click);
            // 
            // btnExample5
            // 
            this.btnExample5.Location = new System.Drawing.Point(15, 160);
            this.btnExample5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExample5.Name = "btnExample5";
            this.btnExample5.Size = new System.Drawing.Size(119, 29);
            this.btnExample5.TabIndex = 4;
            this.btnExample5.Text = "Example 1.5";
            this.btnExample5.UseVisualStyleBackColor = true;
            this.btnExample5.Click += new System.EventHandler(this.btnExample5_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(164, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 202);
            this.label1.TabIndex = 6;
            this.label1.Text = "This form supports the excersises shown in code samples";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Part1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 376);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExample5);
            this.Controls.Add(this.btnExample4);
            this.Controls.Add(this.btnExample3);
            this.Controls.Add(this.btnExample2);
            this.Controls.Add(this.btnExample);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Part1";
            this.Text = "Simple Test Form - Part 1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExample;
        private System.Windows.Forms.Button btnExample2;
        private System.Windows.Forms.Button btnExample3;
        private System.Windows.Forms.Button btnExample4;
        private System.Windows.Forms.Button btnExample5;
        private System.Windows.Forms.Label label1;
    }
}

