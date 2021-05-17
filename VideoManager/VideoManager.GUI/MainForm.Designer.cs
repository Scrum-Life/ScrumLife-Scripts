
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
            this.UploadMainVideoBtn = new System.Windows.Forms.Button();
            this.UpdateAirtableForLiveDataBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.VideoFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            // UploadMainVideoBtn
            // 
            this.UploadMainVideoBtn.Location = new System.Drawing.Point(12, 425);
            this.UploadMainVideoBtn.Name = "UploadMainVideoBtn";
            this.UploadMainVideoBtn.Size = new System.Drawing.Size(233, 40);
            this.UploadMainVideoBtn.TabIndex = 2;
            this.UploadMainVideoBtn.Text = "Mettre en ligne la vidéo principale...";
            this.UploadMainVideoBtn.UseVisualStyleBackColor = true;
            this.UploadMainVideoBtn.Click += new System.EventHandler(this.UploadMainVideoBtn_Click);
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
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(12, 357);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(233, 62);
            this.button1.TabIndex = 8;
            this.button1.Text = "Récupérer les données Airtable";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.GetAirtableDataBtn_Click);
            // 
            // VideoFileDialog
            // 
            this.VideoFileDialog.Filter = "Vidéos|*.mov,*.mp4|Tous les fichiers|*.*";
            this.VideoFileDialog.Title = "Sélectionner la vidéo à uploader";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 497);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UpdateAirtableForLiveDataBtn);
            this.Controls.Add(this.UpdateLiveBtn);
            this.Controls.Add(this.UploadMainVideoBtn);
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
        private System.Windows.Forms.Button UploadMainVideoBtn;
        private System.Windows.Forms.Button UpdateAirtableForLiveDataBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog VideoFileDialog;
    }
}

