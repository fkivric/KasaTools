namespace VolantMusteriDuzel
{
    partial class VolantGrid
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colstokKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colstokAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colurunOzellik = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmiktar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colbirim = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfiyatTipiKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colodemePlani = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colodemePlaniTaksitSayisi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltutar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsatirIskOrn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsatirIskTut1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colkampanyaIskTut = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsatirTutar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colteslimTarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsevkSube = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsatisElemaniRef = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsatisElemaniKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colileriTeslim = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmiktar2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colbirim2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colshowFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsevkDepo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldeliverPayment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colshameProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltutarExch = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colexchVal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colexchRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colproSeri = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coloutDeliver = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colbazNetFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colorderDemand = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1121, 609);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 50;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colstokKodu,
            this.colstokAdi,
            this.colurunOzellik,
            this.colmiktar,
            this.colbirim,
            this.colfiyat,
            this.colfiyatTipiKodu,
            this.colodemePlani,
            this.colodemePlaniTaksitSayisi,
            this.coltutar,
            this.colsatirIskOrn1,
            this.colsatirIskTut1,
            this.colkampanyaIskTut,
            this.colsatirTutar,
            this.colteslimTarihi,
            this.colsevkSube,
            this.colsatisElemaniRef,
            this.colsatisElemaniKodu,
            this.colileriTeslim,
            this.colmiktar2,
            this.colbirim2,
            this.colshowFiyat,
            this.colsevkDepo,
            this.coldeliverPayment,
            this.colshameProduct,
            this.coltutarExch,
            this.colexchVal,
            this.colexchRate,
            this.colproSeri,
            this.coloutDeliver,
            this.colbazNetFiyat,
            this.colorderDemand});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.ViewCaption = "Satış Detayı";
            // 
            // colstokKodu
            // 
            this.colstokKodu.AppearanceHeader.Options.UseTextOptions = true;
            this.colstokKodu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colstokKodu.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colstokKodu.Caption = "Ürün Kodu";
            this.colstokKodu.FieldName = "stokKodu";
            this.colstokKodu.Name = "colstokKodu";
            this.colstokKodu.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colstokKodu.Visible = true;
            this.colstokKodu.VisibleIndex = 5;
            this.colstokKodu.Width = 120;
            // 
            // colstokAdi
            // 
            this.colstokAdi.AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.colstokAdi.AppearanceCell.Options.UseBackColor = true;
            this.colstokAdi.AppearanceHeader.Options.UseTextOptions = true;
            this.colstokAdi.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colstokAdi.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colstokAdi.Caption = "Ürün Adı";
            this.colstokAdi.FieldName = "stokAdi";
            this.colstokAdi.Name = "colstokAdi";
            this.colstokAdi.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colstokAdi.OptionsColumn.ReadOnly = true;
            this.colstokAdi.Visible = true;
            this.colstokAdi.VisibleIndex = 6;
            this.colstokAdi.Width = 207;
            // 
            // colurunOzellik
            // 
            this.colurunOzellik.AppearanceHeader.Options.UseTextOptions = true;
            this.colurunOzellik.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colurunOzellik.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colurunOzellik.Caption = "Sipariş Notu";
            this.colurunOzellik.FieldName = "urunOzellik";
            this.colurunOzellik.Name = "colurunOzellik";
            this.colurunOzellik.Visible = true;
            this.colurunOzellik.VisibleIndex = 8;
            this.colurunOzellik.Width = 65;
            // 
            // colmiktar
            // 
            this.colmiktar.AppearanceCell.BackColor = System.Drawing.Color.Bisque;
            this.colmiktar.AppearanceCell.Options.UseBackColor = true;
            this.colmiktar.AppearanceHeader.Options.UseTextOptions = true;
            this.colmiktar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colmiktar.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colmiktar.Caption = "Miktar";
            this.colmiktar.FieldName = "miktar";
            this.colmiktar.Name = "colmiktar";
            this.colmiktar.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colmiktar.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "miktar", "{0:n2}")});
            this.colmiktar.Visible = true;
            this.colmiktar.VisibleIndex = 11;
            this.colmiktar.Width = 45;
            // 
            // colbirim
            // 
            this.colbirim.AppearanceHeader.Options.UseTextOptions = true;
            this.colbirim.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colbirim.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colbirim.Caption = "Birim";
            this.colbirim.FieldName = "birim";
            this.colbirim.Name = "colbirim";
            this.colbirim.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colbirim.OptionsColumn.ReadOnly = true;
            this.colbirim.Visible = true;
            this.colbirim.VisibleIndex = 12;
            this.colbirim.Width = 32;
            // 
            // colfiyat
            // 
            this.colfiyat.AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.colfiyat.AppearanceCell.Options.UseBackColor = true;
            this.colfiyat.AppearanceHeader.Options.UseTextOptions = true;
            this.colfiyat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colfiyat.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colfiyat.Caption = "Liste Fiyatı";
            this.colfiyat.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.colfiyat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colfiyat.FieldName = "fiyat";
            this.colfiyat.Name = "colfiyat";
            this.colfiyat.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colfiyat.OptionsColumn.ReadOnly = true;
            this.colfiyat.Visible = true;
            this.colfiyat.VisibleIndex = 19;
            this.colfiyat.Width = 65;
            // 
            // colfiyatTipiKodu
            // 
            this.colfiyatTipiKodu.AppearanceHeader.Options.UseTextOptions = true;
            this.colfiyatTipiKodu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colfiyatTipiKodu.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colfiyatTipiKodu.Caption = "Fiyat Kodu";
            this.colfiyatTipiKodu.FieldName = "fiyatTipiKodu";
            this.colfiyatTipiKodu.Name = "colfiyatTipiKodu";
            this.colfiyatTipiKodu.OptionsColumn.ReadOnly = true;
            this.colfiyatTipiKodu.Visible = true;
            this.colfiyatTipiKodu.VisibleIndex = 13;
            this.colfiyatTipiKodu.Width = 65;
            // 
            // colodemePlani
            // 
            this.colodemePlani.AppearanceHeader.Options.UseTextOptions = true;
            this.colodemePlani.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colodemePlani.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colodemePlani.Caption = "Ödeme Plani";
            this.colodemePlani.FieldName = "odemePlaniRef";
            this.colodemePlani.Name = "colodemePlani";
            this.colodemePlani.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colodemePlani.Width = 67;
            // 
            // colodemePlaniTaksitSayisi
            // 
            this.colodemePlaniTaksitSayisi.AppearanceHeader.Options.UseTextOptions = true;
            this.colodemePlaniTaksitSayisi.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colodemePlaniTaksitSayisi.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colodemePlaniTaksitSayisi.Caption = "Taksit Sayısı";
            this.colodemePlaniTaksitSayisi.FieldName = "odemePlaniTaksitSayisi";
            this.colodemePlaniTaksitSayisi.Name = "colodemePlaniTaksitSayisi";
            this.colodemePlaniTaksitSayisi.Visible = true;
            this.colodemePlaniTaksitSayisi.VisibleIndex = 14;
            this.colodemePlaniTaksitSayisi.Width = 51;
            // 
            // coltutar
            // 
            this.coltutar.AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.coltutar.AppearanceCell.Options.UseBackColor = true;
            this.coltutar.AppearanceHeader.Options.UseTextOptions = true;
            this.coltutar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.coltutar.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coltutar.Caption = "Tutar";
            this.coltutar.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.coltutar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.coltutar.FieldName = "tutar";
            this.coltutar.Name = "coltutar";
            this.coltutar.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.coltutar.OptionsColumn.ReadOnly = true;
            this.coltutar.Visible = true;
            this.coltutar.VisibleIndex = 21;
            // 
            // colsatirIskOrn1
            // 
            this.colsatirIskOrn1.AppearanceHeader.Options.UseTextOptions = true;
            this.colsatirIskOrn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsatirIskOrn1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsatirIskOrn1.Caption = "İsk%";
            this.colsatirIskOrn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colsatirIskOrn1.FieldName = "satirIskOrn1";
            this.colsatirIskOrn1.Name = "colsatirIskOrn1";
            this.colsatirIskOrn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colsatirIskOrn1.Visible = true;
            this.colsatirIskOrn1.VisibleIndex = 22;
            this.colsatirIskOrn1.Width = 35;
            // 
            // colsatirIskTut1
            // 
            this.colsatirIskTut1.AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.colsatirIskTut1.AppearanceCell.Options.UseBackColor = true;
            this.colsatirIskTut1.AppearanceHeader.Options.UseTextOptions = true;
            this.colsatirIskTut1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsatirIskTut1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsatirIskTut1.Caption = "İskonto Tutarı";
            this.colsatirIskTut1.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.colsatirIskTut1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colsatirIskTut1.FieldName = "satirIskTut1";
            this.colsatirIskTut1.Name = "colsatirIskTut1";
            this.colsatirIskTut1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colsatirIskTut1.Visible = true;
            this.colsatirIskTut1.VisibleIndex = 23;
            this.colsatirIskTut1.Width = 54;
            // 
            // colkampanyaIskTut
            // 
            this.colkampanyaIskTut.AppearanceHeader.Options.UseTextOptions = true;
            this.colkampanyaIskTut.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colkampanyaIskTut.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colkampanyaIskTut.Caption = "Kampanya";
            this.colkampanyaIskTut.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.colkampanyaIskTut.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colkampanyaIskTut.FieldName = "kampanyaIskTut";
            this.colkampanyaIskTut.Name = "colkampanyaIskTut";
            this.colkampanyaIskTut.OptionsColumn.ReadOnly = true;
            this.colkampanyaIskTut.Width = 56;
            // 
            // colsatirTutar
            // 
            this.colsatirTutar.AppearanceCell.BackColor = System.Drawing.Color.LemonChiffon;
            this.colsatirTutar.AppearanceCell.Options.UseBackColor = true;
            this.colsatirTutar.AppearanceHeader.Options.UseTextOptions = true;
            this.colsatirTutar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsatirTutar.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsatirTutar.Caption = "Net Tutar";
            this.colsatirTutar.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.colsatirTutar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colsatirTutar.FieldName = "satirTutar";
            this.colsatirTutar.Name = "colsatirTutar";
            this.colsatirTutar.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colsatirTutar.OptionsColumn.ReadOnly = true;
            this.colsatirTutar.Visible = true;
            this.colsatirTutar.VisibleIndex = 24;
            // 
            // colteslimTarihi
            // 
            this.colteslimTarihi.AppearanceHeader.Options.UseTextOptions = true;
            this.colteslimTarihi.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colteslimTarihi.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colteslimTarihi.Caption = "Teslim Tarihi";
            this.colteslimTarihi.FieldName = "teslimTarihi";
            this.colteslimTarihi.Name = "colteslimTarihi";
            this.colteslimTarihi.Visible = true;
            this.colteslimTarihi.VisibleIndex = 25;
            this.colteslimTarihi.Width = 65;
            // 
            // colsevkSube
            // 
            this.colsevkSube.AppearanceHeader.Options.UseTextOptions = true;
            this.colsevkSube.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsevkSube.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsevkSube.Caption = "Sevk Mağaza";
            this.colsevkSube.FieldName = "sevkSube";
            this.colsevkSube.Name = "colsevkSube";
            this.colsevkSube.Visible = true;
            this.colsevkSube.VisibleIndex = 26;
            this.colsevkSube.Width = 45;
            // 
            // colsatisElemaniRef
            // 
            this.colsatisElemaniRef.AppearanceHeader.Options.UseTextOptions = true;
            this.colsatisElemaniRef.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsatisElemaniRef.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsatisElemaniRef.Caption = "Satıcı";
            this.colsatisElemaniRef.FieldName = "satisElemaniRef";
            this.colsatisElemaniRef.Name = "colsatisElemaniRef";
            // 
            // colsatisElemaniKodu
            // 
            this.colsatisElemaniKodu.AppearanceCell.BackColor = System.Drawing.Color.Khaki;
            this.colsatisElemaniKodu.AppearanceCell.Options.UseBackColor = true;
            this.colsatisElemaniKodu.AppearanceHeader.Options.UseTextOptions = true;
            this.colsatisElemaniKodu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colsatisElemaniKodu.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsatisElemaniKodu.Caption = "Satıcı";
            this.colsatisElemaniKodu.FieldName = "satisElemaniKodu";
            this.colsatisElemaniKodu.Name = "colsatisElemaniKodu";
            this.colsatisElemaniKodu.Visible = true;
            this.colsatisElemaniKodu.VisibleIndex = 28;
            this.colsatisElemaniKodu.Width = 48;
            // 
            // colileriTeslim
            // 
            this.colileriTeslim.AppearanceHeader.Options.UseTextOptions = true;
            this.colileriTeslim.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colileriTeslim.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colileriTeslim.Caption = "İleri Teslim";
            this.colileriTeslim.FieldName = "ileriTeslim";
            this.colileriTeslim.Name = "colileriTeslim";
            this.colileriTeslim.Visible = true;
            this.colileriTeslim.VisibleIndex = 0;
            this.colileriTeslim.Width = 43;
            // 
            // colmiktar2
            // 
            this.colmiktar2.AppearanceHeader.Options.UseTextOptions = true;
            this.colmiktar2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colmiktar2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colmiktar2.Caption = "İşlem Miktarı";
            this.colmiktar2.FieldName = "miktar2";
            this.colmiktar2.Name = "colmiktar2";
            this.colmiktar2.Visible = true;
            this.colmiktar2.VisibleIndex = 9;
            this.colmiktar2.Width = 46;
            // 
            // colbirim2
            // 
            this.colbirim2.AppearanceHeader.Options.UseTextOptions = true;
            this.colbirim2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colbirim2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colbirim2.Caption = "İşlem Birimi";
            this.colbirim2.FieldName = "birim2";
            this.colbirim2.Name = "colbirim2";
            this.colbirim2.Visible = true;
            this.colbirim2.VisibleIndex = 10;
            this.colbirim2.Width = 40;
            // 
            // colshowFiyat
            // 
            this.colshowFiyat.AppearanceHeader.Options.UseTextOptions = true;
            this.colshowFiyat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colshowFiyat.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colshowFiyat.Caption = "Sirkü Fiyat";
            this.colshowFiyat.DisplayFormat.FormatString = "###,##0.00;-###,##0.00;#";
            this.colshowFiyat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colshowFiyat.FieldName = "showFiyat";
            this.colshowFiyat.Name = "colshowFiyat";
            this.colshowFiyat.OptionsColumn.ReadOnly = true;
            this.colshowFiyat.Visible = true;
            this.colshowFiyat.VisibleIndex = 20;
            // 
            // colsevkDepo
            // 
            this.colsevkDepo.AppearanceHeader.Options.UseTextOptions = true;
            this.colsevkDepo.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colsevkDepo.Caption = "Sevk Depo";
            this.colsevkDepo.FieldName = "sevkDepo";
            this.colsevkDepo.Name = "colsevkDepo";
            this.colsevkDepo.OptionsColumn.ReadOnly = true;
            this.colsevkDepo.Visible = true;
            this.colsevkDepo.VisibleIndex = 27;
            this.colsevkDepo.Width = 46;
            // 
            // coldeliverPayment
            // 
            this.coldeliverPayment.AppearanceHeader.Options.UseTextOptions = true;
            this.coldeliverPayment.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coldeliverPayment.Caption = "Sevk Ödeme";
            this.coldeliverPayment.FieldName = "deliverPayment";
            this.coldeliverPayment.Name = "coldeliverPayment";
            this.coldeliverPayment.Visible = true;
            this.coldeliverPayment.VisibleIndex = 1;
            this.coldeliverPayment.Width = 42;
            // 
            // colshameProduct
            // 
            this.colshameProduct.AppearanceHeader.Options.UseTextOptions = true;
            this.colshameProduct.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colshameProduct.Caption = "Ayıplı Ürün";
            this.colshameProduct.FieldName = "shameProduct";
            this.colshameProduct.Name = "colshameProduct";
            this.colshameProduct.Visible = true;
            this.colshameProduct.VisibleIndex = 2;
            this.colshameProduct.Width = 42;
            // 
            // coltutarExch
            // 
            this.coltutarExch.AppearanceCell.BackColor = System.Drawing.Color.Khaki;
            this.coltutarExch.AppearanceCell.Options.UseBackColor = true;
            this.coltutarExch.AppearanceHeader.Options.UseTextOptions = true;
            this.coltutarExch.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coltutarExch.Caption = "Döviz Tutarı";
            this.coltutarExch.DisplayFormat.FormatString = "{0:n2}";
            this.coltutarExch.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.coltutarExch.FieldName = "bazFiyat";
            this.coltutarExch.Name = "coltutarExch";
            this.coltutarExch.Visible = true;
            this.coltutarExch.VisibleIndex = 15;
            // 
            // colexchVal
            // 
            this.colexchVal.AppearanceHeader.Options.UseTextOptions = true;
            this.colexchVal.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colexchVal.Caption = "Döviz Kodu";
            this.colexchVal.FieldName = "bazFiyatPbm";
            this.colexchVal.Name = "colexchVal";
            this.colexchVal.Visible = true;
            this.colexchVal.VisibleIndex = 17;
            this.colexchVal.Width = 38;
            // 
            // colexchRate
            // 
            this.colexchRate.AppearanceHeader.Options.UseTextOptions = true;
            this.colexchRate.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colexchRate.Caption = "Döviz Kuru";
            this.colexchRate.DisplayFormat.FormatString = "{0:n4}";
            this.colexchRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colexchRate.FieldName = "bazFiyatKur";
            this.colexchRate.Name = "colexchRate";
            this.colexchRate.Visible = true;
            this.colexchRate.VisibleIndex = 16;
            this.colexchRate.Width = 48;
            // 
            // colproSeri
            // 
            this.colproSeri.Caption = "Seri";
            this.colproSeri.FieldName = "proSeri";
            this.colproSeri.Name = "colproSeri";
            this.colproSeri.Visible = true;
            this.colproSeri.VisibleIndex = 7;
            this.colproSeri.Width = 135;
            // 
            // coloutDeliver
            // 
            this.coloutDeliver.AppearanceHeader.Options.UseTextOptions = true;
            this.coloutDeliver.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coloutDeliver.Caption = "Dış Teslim";
            this.coloutDeliver.FieldName = "outDeliver";
            this.coloutDeliver.Name = "coloutDeliver";
            this.coloutDeliver.Visible = true;
            this.coloutDeliver.VisibleIndex = 3;
            this.coloutDeliver.Width = 42;
            // 
            // colbazNetFiyat
            // 
            this.colbazNetFiyat.AppearanceCell.BackColor = System.Drawing.Color.Khaki;
            this.colbazNetFiyat.AppearanceCell.Options.UseBackColor = true;
            this.colbazNetFiyat.AppearanceHeader.Options.UseTextOptions = true;
            this.colbazNetFiyat.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colbazNetFiyat.Caption = "Döviz Tutarı Net";
            this.colbazNetFiyat.DisplayFormat.FormatString = "{0:n2}";
            this.colbazNetFiyat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colbazNetFiyat.FieldName = "bazNetFiyat";
            this.colbazNetFiyat.Name = "colbazNetFiyat";
            this.colbazNetFiyat.OptionsColumn.ReadOnly = true;
            this.colbazNetFiyat.Visible = true;
            this.colbazNetFiyat.VisibleIndex = 18;
            this.colbazNetFiyat.Width = 58;
            // 
            // colorderDemand
            // 
            this.colorderDemand.AppearanceHeader.Options.UseTextOptions = true;
            this.colorderDemand.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colorderDemand.Caption = "Sipariş Ver";
            this.colorderDemand.FieldName = "orderDemand";
            this.colorderDemand.Name = "colorderDemand";
            this.colorderDemand.Visible = true;
            this.colorderDemand.VisibleIndex = 4;
            this.colorderDemand.Width = 42;
            // 
            // VolantGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 609);
            this.Controls.Add(this.gridControl1);
            this.Name = "VolantGrid";
            this.Text = "VolantGrid";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colstokKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colstokAdi;
        private DevExpress.XtraGrid.Columns.GridColumn colurunOzellik;
        private DevExpress.XtraGrid.Columns.GridColumn colmiktar;
        private DevExpress.XtraGrid.Columns.GridColumn colbirim;
        private DevExpress.XtraGrid.Columns.GridColumn colfiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colfiyatTipiKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colodemePlani;
        private DevExpress.XtraGrid.Columns.GridColumn colodemePlaniTaksitSayisi;
        private DevExpress.XtraGrid.Columns.GridColumn coltutar;
        private DevExpress.XtraGrid.Columns.GridColumn colsatirIskOrn1;
        private DevExpress.XtraGrid.Columns.GridColumn colsatirIskTut1;
        private DevExpress.XtraGrid.Columns.GridColumn colkampanyaIskTut;
        private DevExpress.XtraGrid.Columns.GridColumn colsatirTutar;
        private DevExpress.XtraGrid.Columns.GridColumn colteslimTarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colsevkSube;
        private DevExpress.XtraGrid.Columns.GridColumn colsatisElemaniRef;
        private DevExpress.XtraGrid.Columns.GridColumn colsatisElemaniKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colileriTeslim;
        private DevExpress.XtraGrid.Columns.GridColumn colmiktar2;
        private DevExpress.XtraGrid.Columns.GridColumn colbirim2;
        private DevExpress.XtraGrid.Columns.GridColumn colshowFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colsevkDepo;
        private DevExpress.XtraGrid.Columns.GridColumn coldeliverPayment;
        private DevExpress.XtraGrid.Columns.GridColumn colshameProduct;
        private DevExpress.XtraGrid.Columns.GridColumn coltutarExch;
        private DevExpress.XtraGrid.Columns.GridColumn colexchVal;
        private DevExpress.XtraGrid.Columns.GridColumn colexchRate;
        private DevExpress.XtraGrid.Columns.GridColumn colproSeri;
        private DevExpress.XtraGrid.Columns.GridColumn coloutDeliver;
        private DevExpress.XtraGrid.Columns.GridColumn colbazNetFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colorderDemand;
    }
}