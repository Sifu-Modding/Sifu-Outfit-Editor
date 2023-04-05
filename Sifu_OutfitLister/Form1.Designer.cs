namespace Sifu_OutfitLister
{
    partial class Frm_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            btnGenerate = new Button();
            LsbOutfits = new ListBox();
            BtnAdd = new Button();
            BtnRemove = new Button();
            txbName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txbDescription = new TextBox();
            label3 = new Label();
            txbSKPath = new TextBox();
            SuspendLayout();
            // 
            // btnGenerate
            // 
            btnGenerate.Enabled = false;
            btnGenerate.Location = new Point(452, 261);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(126, 23);
            btnGenerate.TabIndex = 6;
            btnGenerate.Text = "Generate UAssets";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // LsbOutfits
            // 
            LsbOutfits.FormattingEnabled = true;
            LsbOutfits.ItemHeight = 15;
            LsbOutfits.Location = new Point(12, 12);
            LsbOutfits.Name = "LsbOutfits";
            LsbOutfits.Size = new Size(205, 274);
            LsbOutfits.TabIndex = 7;
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(223, 12);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(142, 23);
            BtnAdd.TabIndex = 8;
            BtnAdd.Text = "Add New Outfit";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // BtnRemove
            // 
            BtnRemove.Location = new Point(223, 41);
            BtnRemove.Name = "BtnRemove";
            BtnRemove.Size = new Size(142, 23);
            BtnRemove.TabIndex = 9;
            BtnRemove.Text = "Remove Selected Outfit";
            BtnRemove.UseVisualStyleBackColor = true;
            BtnRemove.Click += BtnRemove_Click;
            // 
            // txbName
            // 
            txbName.Location = new Point(223, 101);
            txbName.Name = "txbName";
            txbName.PlaceholderText = "The name of the outfit";
            txbName.Size = new Size(355, 23);
            txbName.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(223, 83);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 11;
            label1.Text = "Outfit Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(223, 142);
            label2.Name = "label2";
            label2.Size = new Size(101, 15);
            label2.TabIndex = 12;
            label2.Text = "Outfit Description";
            // 
            // txbDescription
            // 
            txbDescription.Location = new Point(223, 160);
            txbDescription.Name = "txbDescription";
            txbDescription.PlaceholderText = "This is a description";
            txbDescription.Size = new Size(355, 23);
            txbDescription.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(225, 204);
            label3.Name = "label3";
            label3.Size = new Size(140, 15);
            label3.TabIndex = 14;
            label3.Text = "Outfit Skeletal Mesh Path";
            // 
            // txbSKPath
            // 
            txbSKPath.Location = new Point(223, 222);
            txbSKPath.Name = "txbSKPath";
            txbSKPath.PlaceholderText = "/Game/Characters/Boss/Sifu/Meshes/SK_Sifu_01.SK_Sifu_0";
            txbSKPath.Size = new Size(355, 23);
            txbSKPath.TabIndex = 15;
            // 
            // Frm_Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(593, 296);
            Controls.Add(txbSKPath);
            Controls.Add(label3);
            Controls.Add(txbDescription);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txbName);
            Controls.Add(BtnRemove);
            Controls.Add(BtnAdd);
            Controls.Add(LsbOutfits);
            Controls.Add(btnGenerate);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Frm_Main";
            Text = "Sifu Outfit Lister";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnGenerate;
        private ListBox LsbOutfits;
        private Button BtnAdd;
        private Button BtnRemove;
        private TextBox txbName;
        private Label label1;
        private Label label2;
        private TextBox txbDescription;
        private Label label3;
        private TextBox txbSKPath;
    }
}