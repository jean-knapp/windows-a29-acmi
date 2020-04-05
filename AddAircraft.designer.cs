namespace windows_a29_acmi
{
    partial class AddAircraft
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAircraft));
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.tailnumberEdit = new DevExpress.XtraEditors.TextEdit();
            this.coalitionEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.callsignEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.colorEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.pilotEdit = new DevExpress.XtraEditors.TextEdit();
            this.addButton = new DevExpress.XtraEditors.SimpleButton();
            this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tailnumberEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coalitionEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.callsignEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pilotEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 171);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 13);
            this.labelControl6.TabIndex = 27;
            this.labelControl6.Text = "Tail Number:";
            // 
            // tailnumberEdit
            // 
            this.tailnumberEdit.Location = new System.Drawing.Point(78, 168);
            this.tailnumberEdit.Name = "tailnumberEdit";
            this.tailnumberEdit.Size = new System.Drawing.Size(100, 20);
            this.tailnumberEdit.TabIndex = 5;
            // 
            // coalitionEdit
            // 
            this.coalitionEdit.Location = new System.Drawing.Point(78, 116);
            this.coalitionEdit.Name = "coalitionEdit";
            this.coalitionEdit.Size = new System.Drawing.Size(100, 20);
            this.coalitionEdit.TabIndex = 3;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 119);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(45, 13);
            this.labelControl5.TabIndex = 24;
            this.labelControl5.Text = "Coalition:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(44, 13);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "Call Sign:";
            // 
            // callsignEdit
            // 
            this.callsignEdit.Location = new System.Drawing.Point(78, 12);
            this.callsignEdit.Name = "callsignEdit";
            this.callsignEdit.Size = new System.Drawing.Size(100, 20);
            this.callsignEdit.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 145);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(33, 13);
            this.labelControl3.TabIndex = 21;
            this.labelControl3.Text = "Group:";
            // 
            // groupEdit
            // 
            this.groupEdit.Location = new System.Drawing.Point(78, 142);
            this.groupEdit.Name = "groupEdit";
            this.groupEdit.Size = new System.Drawing.Size(100, 20);
            this.groupEdit.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 67);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(29, 13);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "Color:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 13);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "Pilot:";
            // 
            // colorEdit
            // 
            this.colorEdit.EditValue = "Blue";
            this.colorEdit.Location = new System.Drawing.Point(78, 64);
            this.colorEdit.Name = "colorEdit";
            this.colorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorEdit.Properties.Items.AddRange(new object[] {
            "Blue",
            "Green",
            "Orange",
            "Red",
            "Violet"});
            this.colorEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.colorEdit.Size = new System.Drawing.Size(100, 20);
            this.colorEdit.TabIndex = 2;
            // 
            // pilotEdit
            // 
            this.pilotEdit.Location = new System.Drawing.Point(78, 38);
            this.pilotEdit.Name = "pilotEdit";
            this.pilotEdit.Size = new System.Drawing.Size(100, 20);
            this.pilotEdit.TabIndex = 1;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(103, 220);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(22, 220);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // AddAircraft
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(191, 258);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.tailnumberEdit);
            this.Controls.Add(this.coalitionEdit);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.callsignEdit);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.groupEdit);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.colorEdit);
            this.Controls.Add(this.pilotEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAircraft";
            this.Text = "Add Aircraft";
            ((System.ComponentModel.ISupportInitialize)(this.tailnumberEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coalitionEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.callsignEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pilotEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit tailnumberEdit;
        private DevExpress.XtraEditors.TextEdit coalitionEdit;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit callsignEdit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit groupEdit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit colorEdit;
        private DevExpress.XtraEditors.TextEdit pilotEdit;
        private DevExpress.XtraEditors.SimpleButton addButton;
        private DevExpress.XtraEditors.SimpleButton cancelButton;
    }
}