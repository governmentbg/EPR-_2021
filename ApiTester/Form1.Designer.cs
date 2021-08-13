
namespace ApiTester
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lbBaseUrl = new System.Windows.Forms.Label();
            this.tbMethod = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbRequest
            // 
            this.tbRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRequest.Location = new System.Drawing.Point(12, 30);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.Size = new System.Drawing.Size(974, 481);
            this.tbRequest.TabIndex = 0;
            this.tbRequest.Text = resources.GetString("tbRequest.Text");
            this.tbRequest.WordWrap = false;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.Location = new System.Drawing.Point(12, 517);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(128, 27);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Изпрати";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbResponse
            // 
            this.tbResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResponse.Location = new System.Drawing.Point(12, 550);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.Size = new System.Drawing.Size(974, 162);
            this.tbResponse.TabIndex = 2;
            this.tbResponse.WordWrap = false;
            // 
            // lbBaseUrl
            // 
            this.lbBaseUrl.AutoSize = true;
            this.lbBaseUrl.Location = new System.Drawing.Point(12, 9);
            this.lbBaseUrl.Name = "lbBaseUrl";
            this.lbBaseUrl.Size = new System.Drawing.Size(56, 15);
            this.lbBaseUrl.TabIndex = 3;
            this.lbBaseUrl.Text = "lbBaseUrl";
            // 
            // tbMethod
            // 
            this.tbMethod.Location = new System.Drawing.Point(217, 1);
            this.tbMethod.Name = "tbMethod";
            this.tbMethod.Size = new System.Drawing.Size(441, 23);
            this.tbMethod.TabIndex = 4;
            this.tbMethod.Text = "api/Dismissal/ReplaceUpdate";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 724);
            this.Controls.Add(this.tbMethod);
            this.Controls.Add(this.lbBaseUrl);
            this.Controls.Add(this.tbResponse);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbRequest);
            this.Name = "Form1";
            this.Text = "ЕПРО - Api тестер";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbRequest;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.Label lbBaseUrl;
        private System.Windows.Forms.TextBox tbMethod;
    }
}

