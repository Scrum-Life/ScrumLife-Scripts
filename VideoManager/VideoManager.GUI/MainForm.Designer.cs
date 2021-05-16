
namespace VideoManager.GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GetAirtableDataBtn = new System.Windows.Forms.Button();
            this.VideoDGV = new System.Windows.Forms.DataGridView();
            this.UpdateLiveBtn = new System.Windows.Forms.Button();
            this.UpdateMainVideoBtn = new System.Windows.Forms.Button();
            this.UpdateAirtableForLiveDataBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.VideoDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // GetAirtableDataBtn
            // 
            this.GetAirtableDataBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.GetAirtableDataBtn.Image = ((System.Drawing.Image)(resources.GetObject("GetAirtableDataBtn.Image")));
            this.GetAirtableDataBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GetAirtableDataBtn.Location = new System.Drawing.Point(12, 12);
            this.GetAirtableDataBtn.Name = "GetAirtableDataBtn";
            this.GetAirtableDataBtn.Size = new System.Drawing.Size(233, 62);
            this.GetAirtableDataBtn.TabIndex = 0;
            this.GetAirtableDataBtn.Text = "Récupérer les données Airtable";
            this.GetAirtableDataBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.GetAirtableDataBtn.UseVisualStyleBackColor = true;
            this.GetAirtableDataBtn.Click += new System.EventHandler(this.GetAirtableDataBtn_Click);
            // 
            // VideoDGV
            // 
            this.VideoDGV.AllowUserToAddRows = false;
            this.VideoDGV.AllowUserToDeleteRows = false;
            this.VideoDGV.AllowUserToOrderColumns = true;
            this.VideoDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VideoDGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.VideoDGV.Location = new System.Drawing.Point(12, 80);
            this.VideoDGV.MultiSelect = false;
            this.VideoDGV.Name = "VideoDGV";
            this.VideoDGV.ReadOnly = true;
            this.VideoDGV.RowHeadersVisible = false;
            this.VideoDGV.RowTemplate.Height = 25;
            this.VideoDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VideoDGV.ShowEditingIcon = false;
            this.VideoDGV.Size = new System.Drawing.Size(776, 150);
            this.VideoDGV.TabIndex = 1;
            // 
            // UpdateLiveBtn
            // 
            this.UpdateLiveBtn.Location = new System.Drawing.Point(12, 295);
            this.UpdateLiveBtn.Name = "UpdateLiveBtn";
            this.UpdateLiveBtn.Size = new System.Drawing.Size(233, 56);
            this.UpdateLiveBtn.TabIndex = 6;
            this.UpdateLiveBtn.Text = "Mettre à jour le live sur YT";
            this.UpdateLiveBtn.UseVisualStyleBackColor = true;
            this.UpdateLiveBtn.Click += new System.EventHandler(this.UpdateLiveBtn_Click);
            // 
            // UpdateMainVideoBtn
            // 
            this.UpdateMainVideoBtn.Location = new System.Drawing.Point(12, 357);
            this.UpdateMainVideoBtn.Name = "UpdateMainVideoBtn";
            this.UpdateMainVideoBtn.Size = new System.Drawing.Size(233, 23);
            this.UpdateMainVideoBtn.TabIndex = 2;
            this.UpdateMainVideoBtn.Text = "Mettre à jour la vidéo principale";
            this.UpdateMainVideoBtn.UseVisualStyleBackColor = true;
            this.UpdateMainVideoBtn.Click += new System.EventHandler(this.UpdateVideosBtn_Click);
            // 
            // UpdateAirtableForLiveDataBtn
            // 
            this.UpdateAirtableForLiveDataBtn.Location = new System.Drawing.Point(12, 236);
            this.UpdateAirtableForLiveDataBtn.Name = "UpdateAirtableForLiveDataBtn";
            this.UpdateAirtableForLiveDataBtn.Size = new System.Drawing.Size(233, 53);
            this.UpdateAirtableForLiveDataBtn.TabIndex = 7;
            this.UpdateAirtableForLiveDataBtn.Text = "Récupère le lien du live, et met à jour dans Airtable";
            this.UpdateAirtableForLiveDataBtn.UseVisualStyleBackColor = true;
            this.UpdateAirtableForLiveDataBtn.Click += new System.EventHandler(this.UpdateAirtableForLiveDataBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UpdateAirtableForLiveDataBtn);
            this.Controls.Add(this.UpdateLiveBtn);
            this.Controls.Add(this.UpdateMainVideoBtn);
            this.Controls.Add(this.VideoDGV);
            this.Controls.Add(this.GetAirtableDataBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Video Manager";
            ((System.ComponentModel.ISupportInitialize)(this.VideoDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GetAirtableDataBtn;
        private System.Windows.Forms.DataGridView VideoDGV;
        private System.Windows.Forms.Button UpdateLiveBtn;
        private System.Windows.Forms.Button UpdateMainVideoBtn;
        private System.Windows.Forms.Button UpdateAirtableForLiveDataBtn;
    }
}

