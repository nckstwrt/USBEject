
namespace USBEject
{
    partial class USBEjectForm
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
            this.comboBoxDrive = new System.Windows.Forms.ComboBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonEject = new System.Windows.Forms.Button();
            this.buttonCheckDisk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxDrive
            // 
            this.comboBoxDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDrive.FormattingEnabled = true;
            this.comboBoxDrive.Location = new System.Drawing.Point(12, 12);
            this.comboBoxDrive.Name = "comboBoxDrive";
            this.comboBoxDrive.Size = new System.Drawing.Size(216, 21);
            this.comboBoxDrive.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(12, 41);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonEject
            // 
            this.buttonEject.Location = new System.Drawing.Point(237, 12);
            this.buttonEject.Name = "buttonEject";
            this.buttonEject.Size = new System.Drawing.Size(75, 52);
            this.buttonEject.TabIndex = 2;
            this.buttonEject.Text = "Eject";
            this.buttonEject.UseVisualStyleBackColor = true;
            this.buttonEject.Click += new System.EventHandler(this.buttonEject_Click);
            // 
            // buttonCheckDisk
            // 
            this.buttonCheckDisk.Location = new System.Drawing.Point(153, 41);
            this.buttonCheckDisk.Name = "buttonCheckDisk";
            this.buttonCheckDisk.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckDisk.TabIndex = 3;
            this.buttonCheckDisk.Text = "Check Disk";
            this.buttonCheckDisk.UseVisualStyleBackColor = true;
            this.buttonCheckDisk.Click += new System.EventHandler(this.buttonCheckDisk_Click);
            // 
            // USBEjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 75);
            this.Controls.Add(this.buttonCheckDisk);
            this.Controls.Add(this.buttonEject);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.comboBoxDrive);
            this.Name = "USBEjectForm";
            this.Text = "USBEject";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDrive;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonEject;
        private System.Windows.Forms.Button buttonCheckDisk;
    }
}

