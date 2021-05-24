
namespace IceCreamShopView
{
    partial class FormMails
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonPage1 = new System.Windows.Forms.Button();
            this.buttonPage2 = new System.Windows.Forms.Button();
            this.buttonPage3 = new System.Windows.Forms.Button();
            this.buttonPage4 = new System.Windows.Forms.Button();
            this.buttonPage5 = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(619, 356);
            this.dataGridView.TabIndex = 0;
            // 
            // buttonPage1
            // 
            this.buttonPage1.Location = new System.Drawing.Point(171, 377);
            this.buttonPage1.Name = "buttonPage1";
            this.buttonPage1.Size = new System.Drawing.Size(34, 30);
            this.buttonPage1.TabIndex = 1;
            this.buttonPage1.Text = "1";
            this.buttonPage1.UseVisualStyleBackColor = true;
            // 
            // buttonPage2
            // 
            this.buttonPage2.Location = new System.Drawing.Point(238, 377);
            this.buttonPage2.Name = "buttonPage2";
            this.buttonPage2.Size = new System.Drawing.Size(34, 30);
            this.buttonPage2.TabIndex = 2;
            this.buttonPage2.Text = "2";
            this.buttonPage2.UseVisualStyleBackColor = true;
            // 
            // buttonPage3
            // 
            this.buttonPage3.Location = new System.Drawing.Point(301, 377);
            this.buttonPage3.Name = "buttonPage3";
            this.buttonPage3.Size = new System.Drawing.Size(34, 30);
            this.buttonPage3.TabIndex = 3;
            this.buttonPage3.Text = "3";
            this.buttonPage3.UseVisualStyleBackColor = true;
            // 
            // buttonPage4
            // 
            this.buttonPage4.Location = new System.Drawing.Point(366, 377);
            this.buttonPage4.Name = "buttonPage4";
            this.buttonPage4.Size = new System.Drawing.Size(34, 30);
            this.buttonPage4.TabIndex = 4;
            this.buttonPage4.Text = "4";
            this.buttonPage4.UseVisualStyleBackColor = true;
            // 
            // buttonPage5
            // 
            this.buttonPage5.Location = new System.Drawing.Point(429, 377);
            this.buttonPage5.Name = "buttonPage5";
            this.buttonPage5.Size = new System.Drawing.Size(34, 30);
            this.buttonPage5.TabIndex = 5;
            this.buttonPage5.Text = "5";
            this.buttonPage5.UseVisualStyleBackColor = true;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(506, 381);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "Следующая";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(52, 381);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(84, 23);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "Предыдущая";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // FormMails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 419);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPage5);
            this.Controls.Add(this.buttonPage4);
            this.Controls.Add(this.buttonPage3);
            this.Controls.Add(this.buttonPage2);
            this.Controls.Add(this.buttonPage1);
            this.Controls.Add(this.dataGridView);
            this.Name = "FormMails";
            this.Text = "FormMails";
            this.Load += new System.EventHandler(this.FormMails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonPage1;
        private System.Windows.Forms.Button buttonPage2;
        private System.Windows.Forms.Button buttonPage3;
        private System.Windows.Forms.Button buttonPage4;
        private System.Windows.Forms.Button buttonPage5;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
    }
}