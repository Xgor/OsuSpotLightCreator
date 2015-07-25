namespace osu_Spotlight_Creator
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button generateButton;
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LoadMap = new System.Windows.Forms.Button();
            this.openMapDialog = new System.Windows.Forms.OpenFileDialog();
            this.mapTextBox = new System.Windows.Forms.TextBox();
            this.imgTextBox = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.LoadImg = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBeCo = new System.Windows.Forms.CheckBox();
            this.checkKiai = new System.Windows.Forms.CheckBox();
            generateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            generateButton.Location = new System.Drawing.Point(137, 207);
            generateButton.Name = "generateButton";
            generateButton.Size = new System.Drawing.Size(75, 23);
            generateButton.TabIndex = 5;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(364, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // LoadMap
            // 
            this.LoadMap.Location = new System.Drawing.Point(278, 62);
            this.LoadMap.Name = "LoadMap";
            this.LoadMap.Size = new System.Drawing.Size(75, 23);
            this.LoadMap.TabIndex = 2;
            this.LoadMap.Text = "Load Map";
            this.LoadMap.UseVisualStyleBackColor = true;
            this.LoadMap.Click += new System.EventHandler(this.LoadMap_Click);
            // 
            // openMapDialog
            // 
            this.openMapDialog.DefaultExt = "osu";
            this.openMapDialog.FileName = "openMapDialog";
            this.openMapDialog.RestoreDirectory = true;
            // 
            // mapTextBox
            // 
            this.mapTextBox.AcceptsReturn = true;
            this.mapTextBox.Location = new System.Drawing.Point(12, 62);
            this.mapTextBox.Name = "mapTextBox";
            this.mapTextBox.Size = new System.Drawing.Size(260, 20);
            this.mapTextBox.TabIndex = 3;
            this.mapTextBox.TextChanged += new System.EventHandler(this.mapTextBox_TextChanged);
            // 
            // imgTextBox
            // 
            this.imgTextBox.Enabled = false;
            this.imgTextBox.Location = new System.Drawing.Point(12, 88);
            this.imgTextBox.Name = "imgTextBox";
            this.imgTextBox.Size = new System.Drawing.Size(260, 20);
            this.imgTextBox.TabIndex = 4;
            this.imgTextBox.TextChanged += new System.EventHandler(this.imgTextBox_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(125, 236);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // LoadImg
            // 
            this.LoadImg.Enabled = false;
            this.LoadImg.Location = new System.Drawing.Point(278, 86);
            this.LoadImg.Name = "LoadImg";
            this.LoadImg.Size = new System.Drawing.Size(75, 23);
            this.LoadImg.TabIndex = 7;
            this.LoadImg.Text = "Load Image";
            this.LoadImg.UseVisualStyleBackColor = true;
            this.LoadImg.Click += new System.EventHandler(this.LoadImg_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "osu";
            this.openFileDialog1.FileName = "openMapDialog";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // checkBeCo
            // 
            this.checkBeCo.AutoSize = true;
            this.checkBeCo.Location = new System.Drawing.Point(12, 114);
            this.checkBeCo.Name = "checkBeCo";
            this.checkBeCo.Size = new System.Drawing.Size(137, 17);
            this.checkBeCo.TabIndex = 8;
            this.checkBeCo.Text = "Show between combos";
            this.checkBeCo.UseVisualStyleBackColor = true;
            // 
            // checkKiai
            // 
            this.checkKiai.AutoSize = true;
            this.checkKiai.Location = new System.Drawing.Point(12, 138);
            this.checkKiai.Name = "checkKiai";
            this.checkKiai.Size = new System.Drawing.Size(132, 17);
            this.checkKiai.TabIndex = 9;
            this.checkKiai.Text = "Only show on Kiai time";
            this.checkKiai.UseVisualStyleBackColor = true;
            this.checkKiai.CheckedChanged += new System.EventHandler(this.checkKiai_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 261);
            this.Controls.Add(this.checkKiai);
            this.Controls.Add(this.checkBeCo);
            this.Controls.Add(this.LoadImg);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(generateButton);
            this.Controls.Add(this.imgTextBox);
            this.Controls.Add(this.mapTextBox);
            this.Controls.Add(this.LoadMap);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "osu!Light Creator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button LoadMap;
        private System.Windows.Forms.OpenFileDialog openMapDialog;
        private System.Windows.Forms.TextBox mapTextBox;
        private System.Windows.Forms.TextBox imgTextBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button LoadImg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkKiai;
        private System.Windows.Forms.CheckBox checkBeCo;

    }
}

