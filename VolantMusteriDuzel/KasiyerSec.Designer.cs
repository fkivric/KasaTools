namespace VolantMusteriDuzel
{
    partial class KasiyerSec2
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
            this.srcKasiyerKasa = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.srcKasiyerKasaView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.srcKasiyerKasa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcKasiyerKasaView)).BeginInit();
            this.SuspendLayout();
            // 
            // srcKasiyerKasa
            // 
            this.srcKasiyerKasa.Location = new System.Drawing.Point(17, 12);
            this.srcKasiyerKasa.Name = "srcKasiyerKasa";
            this.srcKasiyerKasa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.srcKasiyerKasa.Properties.NullText = "Seçiniz....!";
            this.srcKasiyerKasa.Properties.PopupView = this.srcKasiyerKasaView;
            this.srcKasiyerKasa.Size = new System.Drawing.Size(303, 20);
            this.srcKasiyerKasa.TabIndex = 0;
            // 
            // srcKasiyerKasaView
            // 
            this.srcKasiyerKasaView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.srcKasiyerKasaView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.srcKasiyerKasaView.Name = "srcKasiyerKasaView";
            this.srcKasiyerKasaView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.srcKasiyerKasaView.OptionsView.ShowGroupPanel = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(17, 38);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(303, 23);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Kasiyer Seç";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "CHSOCODE";
            this.gridColumn1.FieldName = "CHSOCODE";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "DSAFEID";
            this.gridColumn2.FieldName = "DSAFEID";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "DSAFEVAL";
            this.gridColumn3.FieldName = "DSAFEVAL";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "DSAFENAME";
            this.gridColumn4.FieldName = "DSAFENAME";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "KasiyerAdı";
            this.gridColumn5.FieldName = "KasiyerAdı";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            // 
            // KasiyerSec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 75);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.srcKasiyerKasa);
            this.IconOptions.Image = global::VolantMusteriDuzel.Properties.Resources.bodetails_32x32;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KasiyerSec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kasiyer Sec";
            this.Load += new System.EventHandler(this.KasiyerSec_Load);
            ((System.ComponentModel.ISupportInitialize)(this.srcKasiyerKasa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcKasiyerKasaView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SearchLookUpEdit srcKasiyerKasa;
        private DevExpress.XtraGrid.Views.Grid.GridView srcKasiyerKasaView;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}