namespace test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.rsMenu1 = new RsMenu.RsMenu();
            this.SuspendLayout();
            // 
            // rsMenu1
            // 
            this.rsMenu1.FolderImage = ((System.Drawing.Image)(resources.GetObject("rsMenu1.FolderImage")));
            this.rsMenu1.Location = new System.Drawing.Point(0, 0);
            this.rsMenu1.Name = "rsMenu1";
            this.rsMenu1.ReportImage = ((System.Drawing.Image)(resources.GetObject("rsMenu1.ReportImage")));
            this.rsMenu1.Reportpath = null;
            this.rsMenu1.Size = new System.Drawing.Size(292, 24);
            this.rsMenu1.TabIndex = 0;
            this.rsMenu1.Text = "rsMenu1";
            this.rsMenu1.UseDefaultCredentials = false;
            this.rsMenu1.WebServiceUrl = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.rsMenu1);
            this.MainMenuStrip = this.rsMenu1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RsMenu.RsMenu rsMenu1;


    }
}

