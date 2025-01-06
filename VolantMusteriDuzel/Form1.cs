using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Data.Mask;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using VolantMusteriDuzel.Class;
using VolantMusteriDuzel.Properties;
using YONAVMSiparis;
using static VolantMusteriDuzel.Class.ApiClient;
using static VolantMusteriDuzel.Class.TableClass;
using static VolantMusteriDuzel.Class.Volant;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VolantMusteriDuzel
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public class Kasaci
        {
            public string sira { get; set; }
            public string kasaci { get; set; }
        }
        string version;
        SqlConnection sql = new SqlConnection(Settings.Default.connectionstring);
        SqlConnection MDE = new SqlConnection(Settings.Default.connectionstring2);
        SqlConnectionObject conn = new SqlConnectionObject();
        List<Kasaci> sorgu = new List<Kasaci>();
        ApiClient SMS = new ApiClient();
        ListtoDataTableConverter converter = new ListtoDataTableConverter();
        public static string MGZLIST = "";
        public Form1()
        {
            try
            {
                SplashScreen();
                Thread.Sleep(3000);
                //Properties.Settings.Default.connectionstring = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
                Properties.Settings.Default.connectionstring2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
                Properties.Settings.Default.Save();
                //var s1 = new Kasaci()
                //{
                //    sira = "1",
                //    kasaci = "Emine BİLGİÇ"
                //};
                //sorgu.Add(s1);
                //var s2 = new Kasaci()
                //{
                //    sira = "2",
                //    kasaci = "İris KISACIK"
                //};
                //sorgu.Add(s2);
                //var s3 = new Kasaci()
                //{
                //    sira = "3",
                //    kasaci = "Volkan Üstündağ"
                //};
                //sorgu.Add(s3);
                kasaci();
                InitializeComponent();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 300, this);
            }

        }
        void kasaci()
        {
            sorgu = conn.GetData("select SOCODE as sira,SONAME + space(1) + SOSURNAME as kasaci from SOCIAL where SOSTS = 1 and SOCODE like '00KS%'", sql).DataTableToList<Kasaci>();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var token = SMS.SMSToken();
            Properties.Settings.Default.SmsToken = token;
            Properties.Settings.Default.Save();
            string q = String.Format(@"
            declare @WH varchar(2000)
             if isnull(@WH,'') = ''
             begin 
              select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
              from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
             end 
             select 'in (''' + @WH +')'as Magazalar", frmLogin.userID);
            MGZLIST = conn.GetValue(q).Replace(")", "')");

            if (frmLogin.userID == "00KS002")
            {
                cmbMagaza.EditValue = "1";
                cmbMagaza.Enabled = false;
                cmbKasaTutar.EditValue = "1";
                cmbKasaTutar.Enabled = false;
                cmbMagaza2.EditValue = "1";
                cmbMagaza2.Enabled = false;
                tileBarItem4.Enabled = false;
                tileBarItem7.Enabled = false;
                tileBarItem8.Enabled = false;
                tileBarItem11.Visible = false;
            }
            else if (frmLogin.userID == "00KS003")
            {
                cmbMagaza.EditValue = "2";
                cmbMagaza.Enabled = false;
                cmbMagaza2.EditValue = "2";
                cmbMagaza2.Enabled = false;
                cmbKasaTutar.EditValue = "2";
                cmbKasaTutar.Enabled = false;
                tileBarItem4.Enabled = false;
                tileBarItem7.Enabled = false;
                tileBarItem8.Enabled = false;
                tileBarItem11.Visible = false;
            }
            else if (frmLogin.userID == "00KS004")
            {

                cmbMagaza.EditValue = "3";
                cmbMagaza.Enabled = false;
                cmbMagaza2.EditValue = "3";
                cmbMagaza2.Enabled = false;
                cmbKasaTutar.EditValue = "3";
                cmbKasaTutar.Enabled = false;
                tileBarItem4.Enabled = false;
                tileBarItem7.Enabled = false;
                tileBarItem8.Enabled = false;
                tileBarItem11.Visible = false;
            }
            else
            {
                tileBarItem4.Enabled = true;
                tileBarItem7.Enabled = true;
                tileBarItem8.Enabled = true;

            }

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                this.Text = "Version : " + ad.CurrentVersion.Major + "." + ad.CurrentVersion.Minor + "." + ad.CurrentVersion.Build + "." + ad.CurrentVersion.Revision;
                version = ad.CurrentVersion.Revision.ToString();
            }
            else
            {
                string _s1 = System.Windows.Forms.Application.ProductVersion; // versiyon
                this.Text = "Version : " + _s1;
            }
            // Optional: Para birimi sembolünü "₺" olarak ayarlamak için kültür bilgisini ayarlayabilirsiniz 
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            customCulture.NumberFormat.CurrencySymbol = "TL";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
        void SplashScreen()
        {
            FluentSplashScreenOptions splashScreen = new FluentSplashScreenOptions();
            splashScreen.Title = "ENTEGREF";
            splashScreen.Subtitle = "YÖN avm® Volant İşlem Düzeltme";
            splashScreen.RightFooter = "Başlıyor...";
            splashScreen.LeftFooter = $"CopyRight ® 2023 {Environment.NewLine} Tüm Hahkları Saklıdır.";
            splashScreen.LoadingIndicatorType = FluentLoadingIndicatorType.Dots;
            splashScreen.OpacityColor = System.Drawing.Color.FromArgb(16, 110, 190);
            splashScreen.Opacity = 90;
            splashScreen.AppearanceLeftFooter.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowFluentSplashScreen(splashScreen, parentForm: this, useFadeIn: true, useFadeOut: true);
        }

        //devexpress lisanlandı
        //arka plan işlemleri
        private BackgroundWorker _backgroundWorker;
        private ManualResetEvent _workerCompletedEvent = new ManualResetEvent(false);
        private const string READY_TEXT = "Hazır";
        private void executeBackground(Action doWorkAction, Action progressAction = null, Action completedAction = null)
        {
            try
            {
                if (_backgroundWorker != null)
                {


                    if (_backgroundWorker.IsBusy)
                    {
                        XtraMessageBox.Show("Her oturum açıldığında 1 işlem yapacak. Eğer bu girişteki ilk işlemse uygulama çalışmaktadır. Lütfen Bekleyiniz");
                        return;
                    }
                }
                _backgroundWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };
                _backgroundWorker.DoWork += (x, y) =>
                {
                    try
                    {
                        doWorkAction.Invoke();
                    }
                    catch (Exception ex)
                    {
                        y.Cancel = true;
                        XtraMessageBox.Show("Bilinmeyen Hata. Detay : " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // throw;
                    }
                };
                if (progressAction != null)
                {
                    _backgroundWorker.ProgressChanged += (x, y) =>
                    {
                        progressAction.Invoke();
                    };
                }
                if (completedAction != null)
                {
                    _backgroundWorker.RunWorkerCompleted += (x, y) =>
                    {
                        completedAction.Invoke();
                    };
                }
                _backgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {

            }

        }
        private void completeProgress()
        {
            try
            {
                _backgroundWorker.Dispose();
                _backgroundWorker = null;
                if (!this.Enabled)
                {
                    this.Enabled = true;
                }

            }
            finally
            {
                //this.Cursor = Cursors.Default;
                _workerCompletedEvent.Set();

            }
        }
        private void directoryCreator(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            if (navigationFrame.SelectedPage == navigationPage2)
            {
                dteSonucStart.Format = DateTimePickerFormat.Custom;
                dteSonucStart.CustomFormat = "MM-yyyy";
                dteSonucStart.ShowUpDown = true;
            }
            if (navigationFrame.SelectedPage == navigationPage3)
            {
                cmbMagaza.Properties.DataSource = sorgu;
                cmbMagaza.Properties.ValueMember = "sira";
                cmbMagaza.Properties.DisplayMember = "kasaci";
                bool var = false;
                for (int i = 0; i < sorgu.Count; i++)
                {
                    if (sorgu[i].sira == frmLogin.userID.ToString())
                    {
                        var = true;
                    }
                }
                if (var)
                {
                    cmbMagaza.EditValue = frmLogin.userID;
                }
                //cmbMagaza.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                //cmbMagaza.Properties.ValueMember = "DIVVAL";
                //cmbMagaza.Properties.DisplayMember = "DIVNAME";
                dteKasaTarih.Value = DateTime.Now.AddDays(-1);
                groupControl10.Enabled = false;
            }
            if (navigationFrame.SelectedPage == navigationPage4)
            {
                cmbMagaza2.Properties.DataSource = sorgu;
                cmbMagaza2.Properties.ValueMember = "sira";
                cmbMagaza2.Properties.DisplayMember = "kasaci";

                dteKrediTarihStart.MaxDate = DateTime.Now.AddDays(-1);
                dteKrediTarihStart.MinDate = new DateTime(2024, 1, 1);

                dteKrediTarihEnd.MaxDate = DateTime.Now.AddDays(-1);
                dteKrediTarihEnd.MinDate = new DateTime(2024, 1, 1);
            }
            if (navigationFrame.SelectedPage == navigationPage6)
            {
                cmbKasaTarihMagaza.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbKasaTarihMagaza.Properties.ValueMember = "DIVVAL";
                cmbKasaTarihMagaza.Properties.DisplayMember = "DIVNAME";

                //cmbKasaTutar.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                //cmbKasaTutar.Properties.ValueMember = "DIVVAL";
                //cmbKasaTutar.Properties.DisplayMember = "DIVNAME";


                cmbKasaSilMagaza.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbKasaSilMagaza.Properties.ValueMember = "DIVVAL";
                cmbKasaSilMagaza.Properties.DisplayMember = "DIVNAME";



                cmbKasaTutar.Properties.DataSource = sorgu;
                cmbKasaTutar.Properties.ValueMember = "sira";
                cmbKasaTutar.Properties.DisplayMember = "kasaci";

                dteKasaTarihStart.EditValue = DateTime.Now.AddDays(-5);
                dteKasaTarihStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteKasaTarihStart.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteKasaTarihEnd.EditValue = DateTime.Now.AddDays(-1);
                dteKasaTarihEnd.Properties.MaxValue = DateTime.Now.AddDays(-1);


                dteKasaSilStart.EditValue = DateTime.Now.AddDays(-5);
                dteKasaSilStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteKasaSilStart.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteKasaSilEnd.EditValue = DateTime.Now.AddDays(-1);
                dteKasaSilEnd.Properties.MaxValue = DateTime.Now.AddDays(-1);

                dteKasaTutar.EditValue = DateTime.Now.AddDays(-1);
                dteKasaTutar.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteKasaTutar.Properties.MinValue = new DateTime(2023, 4, 1);


                dteKasaTutarNewTarih.EditValue = DateTime.Now.AddDays(-1);
                dteKasaTutarNewTarih.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteKasaTutarNewTarih.Properties.MinValue = new DateTime(2024, 1, 1);



                cmbKasaMerkezDuzeltMagaza.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbKasaMerkezDuzeltMagaza.Properties.ValueMember = "DIVVAL";
                cmbKasaMerkezDuzeltMagaza.Properties.DisplayMember = "DIVNAME";


                dteKasaMerkezDuzeltStart.EditValue = DateTime.Now.AddDays(-5);
                dteKasaMerkezDuzeltStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteKasaMerkezDuzeltStart.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteKasaMerkezDuzeltEnd.EditValue = DateTime.Now.AddDays(-1);
                dteKasaMerkezDuzeltEnd.Properties.MaxValue = DateTime.Now.AddDays(-1);

                cmbBankFark.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbBankFark.Properties.ValueMember = "DIVVAL";
                cmbBankFark.Properties.DisplayMember = "DIVNAME";

                dteBankFarkStart.EditValue = new DateTime(Convert.ToDateTime(DateTime.Now).Year, Convert.ToDateTime(DateTime.Now).Month, 1);
                dteBankFarkStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteBankFarkStart.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteBankFarkEnd.EditValue = DateTime.Now.AddDays(-1);
                dteBankFarkEnd.Properties.MaxValue = DateTime.Now.AddDays(-1);




                cmbGunToplam.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbGunToplam.Properties.ValueMember = "DIVVAL";
                cmbGunToplam.Properties.DisplayMember = "DIVNAME";


                dteGunToplamStart.EditValue = new DateTime(Convert.ToDateTime(DateTime.Now).Year, Convert.ToDateTime(DateTime.Now).Month, 1);
                dteGunToplamStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteGunToplamStart.Properties.MaxValue = DateTime.Now.AddDays(-1);
                dteGunToplamEnd.EditValue = DateTime.Now.AddDays(-1);
                dteGunToplamEnd.Properties.MaxValue = DateTime.Now.AddDays(-1);

                cmbGunToplamYeni.Properties.DataSource = DataDonen("select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1");
                cmbGunToplamYeni.Properties.ValueMember = "DIVVAL";
                cmbGunToplamYeni.Properties.DisplayMember = "DIVNAME";

                dteGunToplamYeni.EditValue = new DateTime(Convert.ToDateTime(DateTime.Now).Year, Convert.ToDateTime(DateTime.Now).Month, 1);
                dteGunToplamStart.Properties.MinValue = new DateTime(2023, 4, 1);
                dteGunToplamStart.Properties.MaxValue = DateTime.Now.AddDays(-1);

            }
            if (navigationFrame.SelectedPage == navigationPage8)
            {


                dteIys.EditValue = DateTime.Now.AddDays(-1);
                dteIys.Properties.MaxValue = DateTime.Now.AddDays(-1);
                srcIys.Properties.DataSource = DataDonen("select '' as DIVVAL, 'Tümü' as DIVNAME union select DIVVAL,DIVNAME from DIVISON where DIVSALESTS = 1 order by 1");
                srcIys.Properties.ValueMember = "DIVVAL";
                srcIys.Properties.DisplayMember = "DIVNAME";
            }
        }

        private void btnIadeControl_Click(object sender, EventArgs e)
        {
            btnIadeControl.Enabled = false;
            btnIadeControl2.Enabled = false;
            txtMusteriNo.Text = "";
            txtMusteriNo.ReadOnly = true;
            txtMusteriNo.Enabled = false;
            ViewIadeTaksitSorunlular.OptionsView.ShowIndicator = true;
            ViewIadeTaksitSorunlular.IndicatorWidth = 40;
            string q = @"select top 100 iade.INSCINSID
            ,CURVAL as [Musteri_Kodu]
            ,CURNAME as [Muster_Adi]
            ,hata.SALDATE as [Iade_Tarihi]
            ,hata.SALID as Iade_ID
            ,s.SALDATE as Hatali_Satis_Tarihi
            ,hata.SALCANSALID as Hatali_Satis_ID
            ,s.SALID as Dogru_Satis_ID
            ,INSAMOUNT as Taksit_Tutari
            ,SALCAMOUNT as Iade_Tutarı, '' as Sonuç from (
            select INSCINSID from (
            select COUNT(INSCINSID) as adet,INSCINSID from INSTALMENTCANCEL with (nolock)
            group by INSCINSID
            ) net
            where adet = 12) iade
            left outer join INSTALMENT on INSID = INSCINSID
            left outer join CURRENTS on CURID = INSCURID
            left outer join SALES s on s.SALID = INSSALID
            outer apply (select SALID,SALCANSALID,SALDATE,sum(INSCAMOUNT) SALCAMOUNT from INSTALMENTCANCEL 
                        left outer join SALES on SALID = INSCSALID
                        where INSCINSID = iade.INSCINSID 
                        group by SALCANSALID,SALDATE,SALID) hata

            --where iade.INSCINSID = 60142897

            option (fast 100)";
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridIadeTaksitSorunlular.DataSource = dt;
        }
        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = ViewIadeTaksitSorunlular.GetSelectedRows();
            btnIadeControl2.Enabled = false;
            txtMusteriNo.ReadOnly = true;
            if (selectedRows.Length > 0)
            {
                for (int i = 0; i < selectedRows.Length; i++)
                {
                    try
                    {
                        var INSCINSID = ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "INSCINSID").ToString();
                        var CURVAL = ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Musteri_Kodu").ToString();
                        Islemler tablo = new Islemler();
                        List<Islemler> ıslemlers = new List<Islemler>();
                        tablo.INSCINSID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "INSCINSID").ToString());
                        tablo.Musteri_Kodu = ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Musteri_Kodu").ToString();
                        tablo.Muster_Adi = ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Muster_Adi").ToString();
                        tablo.Iade_Tarihi = Convert.ToDateTime(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Iade_Tarihi").ToString());
                        tablo.Iade_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Iade_ID").ToString());
                        tablo.Hatali_Satis_Tarihi = Convert.ToDateTime(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Hatali_Satis_Tarihi").ToString());
                        tablo.Hatali_Satis_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Hatali_Satis_ID").ToString());
                        tablo.Dogru_Satis_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Dogru_Satis_ID").ToString());
                        tablo.Taksit_Tutari = decimal.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Taksit_Tutari").ToString());
                        tablo.Iade_Tutarı = decimal.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Iade_Tutarı").ToString());
                        tablo.Bakiye = null;
                        ıslemlers.Add(tablo);
                        ListtoDataTableConverter converter = new ListtoDataTableConverter();
                        DataTable dt = converter.ToDataTable(ıslemlers);
                        MDE.Open();
                        string sonuc = BulkInsert(dt, "IadeBakiyeDuzeltilenler");
                        MDE.Close();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@CURVAL", CURVAL);
                        conn.Insert("FK_BALANCECREATE", dic);


                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                        keyValuePairs.Add("@CURVAL", CURVAL);
                        conn.Insert("FK_INSTALMENTCANCELRECREATENEW", keyValuePairs);
                        ViewIadeTaksitSorunlular.SetRowCellValue(selectedRows[i], "Sonuç", "Başarılı");
                        ViewIadeTaksitSorunlular.RefreshRow(selectedRows[i]);
                    }
                    catch (Exception ex)
                    {

                        ViewIadeTaksitSorunlular.SetRowCellValue(selectedRows[i], "Sonuç", ex.Message);
                        ViewIadeTaksitSorunlular.RefreshRow(selectedRows[i]);
                    }
                    XtraMessageBox.Show(ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "INSCINSID").ToString() + "\r\n" + ViewIadeTaksitSorunlular.GetRowCellValue(selectedRows[i], "Sonuç").ToString());

                }

            }
            else
            {
                {
                    try
                    {
                        var INSCINSID = ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "INSCINSID").ToString();
                        var CURVAL = ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Musteri_Kodu").ToString();
                        Islemler tablo = new Islemler();
                        List<Islemler> ıslemlers = new List<Islemler>();
                        tablo.INSCINSID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "INSCINSID").ToString());
                        tablo.Musteri_Kodu = ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Musteri_Kodu").ToString();
                        tablo.Muster_Adi = ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Muster_Adi").ToString();
                        tablo.Iade_Tarihi = Convert.ToDateTime(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Iade_Tarihi").ToString());
                        tablo.Iade_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Iade_ID").ToString());
                        tablo.Hatali_Satis_Tarihi = Convert.ToDateTime(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Hatali_Satis_Tarihi").ToString());
                        tablo.Hatali_Satis_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Hatali_Satis_ID").ToString());
                        tablo.Dogru_Satis_ID = int.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Dogru_Satis_ID").ToString());
                        tablo.Taksit_Tutari = decimal.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Taksit_Tutari").ToString());
                        tablo.Iade_Tutarı = decimal.Parse(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Iade_Tutarı").ToString());
                        tablo.Bakiye = null;
                        ıslemlers.Add(tablo);
                        ListtoDataTableConverter converter = new ListtoDataTableConverter();
                        DataTable dt = converter.ToDataTable(ıslemlers);
                        MDE.Open();
                        var sonuc = BulkInsert(dt, "IadeBakiyeDuzeltilenler");
                        MDE.Close();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@CURVAL", CURVAL);
                        conn.Insert("FK_BALANCECREATE", dic);


                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                        keyValuePairs.Add("@CURVAL", CURVAL);
                        conn.Insert("FK_INSTALMENTCANCELRECREATENEW", keyValuePairs);
                        ViewIadeTaksitSorunlular.SetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Sonuç", "Başarılı");
                        ViewIadeTaksitSorunlular.RefreshRow(ViewIadeTaksitSorunlular.FocusedRowHandle);
                    }
                    catch (Exception exx)
                    {

                        ViewIadeTaksitSorunlular.SetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Sonuç", exx.Message);
                        ViewIadeTaksitSorunlular.RefreshRow(ViewIadeTaksitSorunlular.FocusedRowHandle); ;
                    }
                    XtraMessageBox.Show(ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "INSCINSID").ToString() + "\r\n" + ViewIadeTaksitSorunlular.GetRowCellValue(ViewIadeTaksitSorunlular.FocusedRowHandle, "Sonuç").ToString());
                }
            }
        }
        public string BulkInsert(DataTable dt, string KaydedilecekTAbloAdı)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(MDE))
            {
                bulkCopy.DestinationTableName = KaydedilecekTAbloAdı;
                try
                {
                    bulkCopy.WriteToServer(dt);
                    return ("Aktarım Tamamlandı");
                }
                catch (Exception ex)
                {
                    return (ex.Message);
                }
            }
        }
        public DataTable DataDonen(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }
        public DataTable DataDonen2(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, MDE);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }
        private void btnIadeControl2_Click(object sender, EventArgs e)
        {
            btnIadeControl2.Enabled = false;
            txtMusteriNo.ReadOnly = true;
            txtMusteriNo.Enabled = false;
            ViewIadeTaksitSorunlular.OptionsView.ShowIndicator = true;
            ViewIadeTaksitSorunlular.IndicatorWidth = 40;
            gridIadeTaksitSorunlular.DataSource = null;
            string q = string.Format("select top 100 iade.INSCINSID \r\n" +
            ",CURVAL as [Musteri_Kodu]  \r\n " +
            ", CURNAME as [Muster_Adi]  \r\n " +
            ", hata.SALDATE as [Iade_Tarihi]  \r\n " +
            ", hata.SALID as Iade_ID  \r\n " +
            ", s.SALDATE as Hatali_Satis_Tarihi  \r\n " +
            ", hata.SALCANSALID as Hatali_Satis_ID  \r\n " +
            ", s.SALID as Dogru_Satis_ID  \r\n " +
            ", INSAMOUNT as Taksit_Tutari  \r\n " +
            ", SALCAMOUNT as Iade_Tutarı, '' as Sonuç from( \r\n " +
            "select INSCINSID from( \r\n " +
            "select COUNT(INSCINSID) as adet,INSCINSID from INSTALMENTCANCEL with (nolock) \r\n " +
            "                                          group by INSCINSID  \r\n " +
            ") net  \r\n " +
            ") iade  \r\n " +
            "left outer join INSTALMENT on INSID = INSCINSID  \r\n " +
            "left outer join CURRENTS on CURID = INSCURID  \r\n " +
            "left outer join SALES s on s.SALID = INSSALID  \r\n " +
            "outer apply (select SALID,SALCANSALID,SALDATE,sum(INSCAMOUNT) SALCAMOUNT from INSTALMENTCANCEL   \r\n " +
            "            left outer join SALES on SALID = INSCSALID  \r\n " +
            "            where INSCINSID = iade.INSCINSID   \r\n " +
            "            group by SALCANSALID,SALDATE,SALID) hata  \r\n " +
            "where CURVAL = '{0}' and INSAMOUNT != SALCAMOUNT \r\n " +
            "option (fast 100)", txtMusteriNo.Text);
            var Sorgu = DataDonen(q);
            gridIadeTaksitSorunlular.DataSource = Sorgu;
            txtMusteriNo.Text = "";
        }
        private static double LastPCDSAMOUNT = 0;
        private static double KalanPCDSAMOUNT = 0;
        private void btnBakiyeMusteriNo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBakiyeMusteriNo.Text))
            {
                ViewBakiyeMusteriNo.OptionsView.ShowIndicator = true;
                ViewBakiyeMusteriNo.IndicatorWidth = 40;
                gridBakiyeMusteriNo.DataSource = null;
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("@CURVAL", txtBakiyeMusteriNo.Text);
                var Sonuc = conn.Query("FK_BALANCE", keyValues);
                gridBakiyeMusteriNo.DataSource = Sonuc;
                tileBarItem2.Enabled = true;
            }
            if (!string.IsNullOrEmpty(txtSatısID.Text))
            {
                var CURID = conn.GetValue($"select SALCURID from SALES where SALID = '{txtSatısID.Text}'");
                gridOdemeDuzeltme.DataSource = conn.GetData(String.Format(@"select PCDSCHID,PCDSCHAMOUNT,DPYMVAL,DPYMNAME,PROCEEDS.* from PROCEEDS
                left outer join PROCEEDSCHILD on PCDSID = PCDSCHPCDSID
                left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                where PCDSCURID = {0}
                and PCDSSALID = {1}
                and not exists (select * from SALES i where i.SALCANSALID = PCDSSALID)", CURID, txtSatısID.Text), sql);
                srcOdemeTipi.DataSource = conn.GetData("select DPYMVAL,DPYMNAME from DEFPAYMENTKIND where DPYMSTS = 1", sql);
                srcOdemeTipi.ValueMember = "DPYMVAL";
                srcOdemeTipi.DisplayMember = "DPYMNAME";
                tileBarItem12.Enabled = true;
                if (srcOdemeTipi.DataSource != null)
                {
                    srcOdemeTipi.DataSource = conn.GetData("select DPYMVAL,DPYMNAME from DEFPAYMENTKIND where DPYMSTS = 1", sql);
                    srcOdemeTipi.ValueMember = "DPYMVAL";
                    srcOdemeTipi.DisplayMember = "DPYMNAME";
                }
            }
            if (!string.IsNullOrEmpty(txtOdemeID.Text))
            {
                txtOdemeTutar.Text = conn.GetValue($"select PCDSAMOUNT from PROCEEDS where PCDSID = {txtOdemeID.Text}");
                txtVadeFarki.Text = conn.GetValue($"select PCDSLATEINCOME from PROCEEDS where PCDSID = {txtOdemeID.Text}");
                txtErkenOdeme.Text = conn.GetValue($"select PCDSEARLYPAYDISC from PROCEEDS where PCDSID = {txtOdemeID.Text}");
                LastPCDSAMOUNT = (ParseToDouble(txtOdemeTutar.Text)+ParseToDouble(txtErkenOdeme.Text))-ParseToDouble(txtVadeFarki.Text);
                grpOdemeID.Enabled = true;
                string q = String.Format(@"select PCDSCHID,PCDSCHAMOUNT,DPYMVAL,DPYMNAME from PROCEEDSCHILD 
                left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                where PCDSCHPCDSID = {0}", txtOdemeID.Text);
                gridTaksitDetayi.DataSource = conn.GetData(q, sql);
                if (srcOdemeTipi2.DataSource != null)
                {
                    srcOdemeTipi2.DataSource = conn.GetData("select DPYMVAL,DPYMNAME from DEFPAYMENTKIND where DPYMSTS = 1 and DPYMCANDOPROCEED = 1", sql);
                    srcOdemeTipi2.ValueMember = "DPYMVAL";
                    srcOdemeTipi2.DisplayMember = "DPYMNAME";
                }
                btnOdemeDetayı.Enabled = true;
                btnAddRow.Enabled = true;
                //ViewTaksitDetayi.AddNewRow();
                //gridView1.ShowEditForm(); // Yeni eklenen satırın düzenleme modunda açılması

            }
            else
            {
                txtOdemeTutar.Text = conn.GetValue($"select PCDSAMOUNT from PROCEEDS where PCDSSALID  = {txtSatısID.Text}");
                txtVadeFarki.Text = conn.GetValue($"select PCDSLATEINCOME from PROCEEDS where PCDSSALID  = {txtSatısID.Text}");
                txtErkenOdeme.Text = conn.GetValue($"select PCDSEARLYPAYDISC from PROCEEDS where PCDSSALID  = {txtSatısID.Text}");
                LastPCDSAMOUNT = (ParseToDouble(txtOdemeTutar.Text) + ParseToDouble(txtErkenOdeme.Text)) - ParseToDouble(txtVadeFarki.Text);
                grpOdemeID.Enabled = true;
                string q = String.Format(@"select PCDSCHID,PCDSCHAMOUNT,DPYMVAL,DPYMNAME from PROCEEDSCHILD 
                left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                where PCDSCHPCDSID in (select PCDSID from PROCEEDS where PCDSSALID = {0})", txtSatısID.Text);
                gridTaksitDetayi.DataSource = conn.GetData(q, sql);
                if (srcOdemeTipi2.DataSource == null)
                {
                    srcOdemeTipi2.DataSource = conn.GetData("select DPYMVAL,DPYMNAME from DEFPAYMENTKIND where DPYMSTS = 1", sql);
                    srcOdemeTipi2.ValueMember = "DPYMVAL";
                    srcOdemeTipi2.DisplayMember = "DPYMNAME";
                }
                btnOdemeDetayı.Enabled = true;
                btnAddRow.Enabled = true;

            }
            txtSatısID.Enabled = false;
            txtBakiyeMusteriNo.Enabled = false;
            btnBakiyeMusteriNo.Enabled = false;
            txtOdemeID.Enabled = false;
        }

        private void tileBarItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                string w = string.Format(" select top 1 INSID from CURRENTS \r\n " +
                "left outer join INSTALMENT on INSCURID = CURID \r\n " +
                "where CURVAL = '{0}'", txtBakiyeMusteriNo.Text);
                var Sorgu = DataDonen(w);
                var INSID = Sorgu.Rows[0][0].ToString();
                string CURVAL = ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "Musteri_Kodu").ToString();
                IadeBakiyeDuzeltilenlerBakiye tablo = new IadeBakiyeDuzeltilenlerBakiye();
                List<IadeBakiyeDuzeltilenlerBakiye> ıslemlers = new List<IadeBakiyeDuzeltilenlerBakiye>();
                tablo.Musteri_Kodu = ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "Musteri_Kodu").ToString();
                tablo.SATISTOPLAM = decimal.Parse(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "SATISTOPLAM").ToString());
                tablo.PESINATTAHSILAT = decimal.Parse(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "PESINATTAHSILAT").ToString());
                tablo.TAKSITTAHSILAT = decimal.Parse(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "TAKSITTAHSILAT").ToString());
                tablo.TAKSITBAKIYE = decimal.Parse(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "TAKSITBAKIYE").ToString());
                tablo.TAKSITSAYISI = int.Parse(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "TAKSITSAYISI").ToString());
                tablo.MINVADETARIHI = Convert.ToDateTime(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "MINVADETARIHI").ToString());

                if (ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "SONODEMEMTARIHI").ToString() == "")
                {
                    tablo.SONODEMEMTARIHI = DateTime.Now;
                }
                else
                {
                    tablo.SONODEMEMTARIHI = Convert.ToDateTime(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "SONODEMEMTARIHI").ToString());
                }
                tablo.SONALISVERISTARIHI = Convert.ToDateTime(ViewBakiyeMusteriNo.GetRowCellValue(ViewBakiyeMusteriNo.FocusedRowHandle, "SONALISVERISTARIHI").ToString());
                ıslemlers.Add(tablo);
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dt = converter.ToDataTable(ıslemlers);
                MDE.Open();
                var sonuc = BulkInsert(dt, "IadeBakiyeDuzeltilenlerBakiye");
                MDE.Close();

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("@CURVAL", CURVAL);
                conn.Insert("FK_BALANCECREATE", dic);


                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("@CURVAL", CURVAL);
                conn.Insert("FK_INSTALMENTCANCELRECREATENEW", keyValues);
                btnYeni_Click(null, null);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            txtOdemeID.Text = "";
            txtSatısID.Text = "";
            txtBakiyeMusteriNo.Text = "";
            txtOdemeTutar.Text = "0";
            txtErkenOdeme.Text = "0";
            txtVadeFarki.Text = "0";
            gridBakiyeMusteriNo.DataSource = null;
            txtBakiyeMusteriNo.ReadOnly = false;
            txtBakiyeMusteriNo.Enabled = true;
            btnBakiyeMusteriNo.Enabled = true;
            btnIadeControl2.Enabled = true;
            txtOdemeID.Enabled = true;
            txtSatısID.Enabled = true;
            grpOdemeID.Enabled = false;
            gridOdemeDuzeltme.DataSource = null;
            gridTaksitDetayi.DataSource = null;
            tileBarItem2.Enabled = false;
            tileBarItem12.Enabled = false;
            btnOdemeDetayı.Enabled = false;
            btnAddRow.Enabled = false;


        }

        private void ViewIadeTaksitSorunlular_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (ViewIadeTaksitSorunlular.DataRowCount > 0)
            {
                if (ViewIadeTaksitSorunlular.GetRowCellValue(e.RowHandle, "Sonuç") != null)
                {
                    var renkKodu = ViewIadeTaksitSorunlular.GetRowCellValue(e.RowHandle, "Sonuç").ToString();
                    if (renkKodu == "")
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#fff ");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fff ");
                    }
                    else if (renkKodu == "Başarılı")
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#444d16");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#444d16");
                        e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                    }
                    else
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    }
                }

            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    System.Environment.Exit(0);
                else e.Cancel = true;
            }
            catch (Exception eex)
            {
                XtraMessageBox.Show(eex.Message);
            }

        }

        private void btnRaporListele_Click(object sender, EventArgs e)
        {
            ViewRapor.OptionsView.ShowIndicator = true;
            ViewRapor.IndicatorWidth = 40;
            gridRapor.DataSource = null;
            string x = @"SELECT [INSCINSID]
                  ,d.[Musteri_Kodu]
                  ,[Muster_Adi]
                  ,[Iade_Tarihi]
                  ,[Iade_ID]
                  ,[Hatali_Satis_Tarihi]
                  ,[Hatali_Satis_ID]
                  ,[Dogru_Satis_ID]
                  ,[Taksit_Tutari]
                  ,[Iade_Tutarı]
                  ,[Bakiye]
                  ,[SATISTOPLAM]
                  ,[PESINATTAHSILAT]
                  ,[TAKSITTAHSILAT]
                  ,[TAKSITBAKIYE]
                  ,[TAKSITSAYISI]
                  ,[MINVADETARIHI]
                  ,[SONODEMEMTARIHI]
                  ,[SONALISVERISTARIHI]
              FROM [MDE_GENEL].[dbo].[IadeBakiyeDuzeltilenler] d
              inner join IadeBakiyeDuzeltilenlerBakiye t on d.Musteri_Kodu = t.Musteri_Kodu";
            var Sorgu = DataDonen2(x);
            gridRapor.DataSource = Sorgu;
            ViewRapor.OptionsView.BestFitMaxRowCount = -1;
            ViewRapor.BestFitColumns(true);
        }

        private void ViewRapor_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void ViewIadeTaksitSorunlular_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }

        }

        private void ViewBakiyeMusteriNo_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            btnIadeControl2.Enabled = true;
            btnIadeControl.Enabled = true;
            gridIadeTaksitSorunlular.DataSource = null;
            txtMusteriNo.ReadOnly = false;
            txtMusteriNo.Enabled = true;
            txtMusteriNo.Text = "";
            tileBarItem1.Enabled = true;
        }

        private void btnPesinatPrim_Click(object sender, EventArgs e)
        {
            var bas = new DateTime(dteSonucStart.Value.Year, dteSonucStart.Value.Month, 1);
            var bit = new DateTime(dteSonucStart.Value.Year, dteSonucStart.Value.Month, DateTime.DaysInMonth(dteSonucStart.Value.Year, dteSonucStart.Value.Month));
            var bastarih = Convert.ToDateTime(bas).ToString("yyyy-MM-dd");
            var bittarih = Convert.ToDateTime(bit).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd");
            string kota;
            if (txtKota.Text == "")
            {
                kota = "0";
            }
            else
            {
                kota = txtKota.Text;
            }
            string q = string.Format(@"select DIVVAL from PROCEEDS 
            left outer join PROCEEDSCHILD on PCDSCHPCDSID = PCDSID
            left outer join SALES satis on SALID = PCDSSALID
            left outer join CASHIER on PCDSCASHIER = CHVAL
            left outer join SOCIAL on SOCODE = CHSOCODE
            left outer join DIVISON on DIVVAL = PCDSDIVISON
            left outer join CURRENTS on CURID = PCDSCURID
            left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
            where PCDSKIND = 2
            and PCDSDATE between '{0}' and '{1}'
            and SALSALEKIND = 'T'
            and PCDSCHDPYMID  = 2
            and not exists (select * from SALES iade where iade.SALCANSALID = satis.SALID )
            group by DIVVAL,DIVNAME,DPYMNAME
            having SUM(PCDSCHAMOUNT) > '{2}'", bastarih, bittarih, kota);

            var gecenmagazalar = DataDonen(q);

            string magazalar = "";
            if (gecenmagazalar.Rows.Count > 0)
            {
                for (int i = 0; i < gecenmagazalar.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        magazalar = "'" + gecenmagazalar.Rows[i]["DIVVAL"].ToString() + "'";
                    }
                    else
                    {
                        magazalar = magazalar + "," + "'" + gecenmagazalar.Rows[i]["DIVVAL"].ToString() + "'";
                    }
                }
                string w = string.Format(@"select DIVVAL,DIVNAME,
                convert(char(10),PCDSDATE,103) as odeme_tarih,
                SOCODE,SONAME + SPACE(1) + SOSURNAME as kasiyer, 
                CURVAL,CURNAME ,PCDSCHAMOUNT,  SALAMOUNT,DPYMNAME,
                PCDSCHAMOUNT *0.01 as prim
                from PROCEEDS 
                left outer join PROCEEDSCHILD on PCDSCHPCDSID = PCDSID
                left outer join SALES satis on SALID = PCDSSALID
                left outer join CASHIER on PCDSCASHIER = CHVAL
                left outer join SOCIAL on SOCODE = CHSOCODE
                left outer join DIVISON on DIVVAL = PCDSDIVISON
                left outer join CURRENTS on CURID = PCDSCURID
                left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                --outer apply (select PCDSAMOUNT as iadettuar from SALES iade 
                --			left outer join PROCEEDS on PCDSSALID = iade.SALID
                --			where iade.SALID < 0  and iade.SALDATE = satis.SALDATE and iade.SALCURID = satis.SALCURID) iade
                where PCDSKIND = 2
                and PCDSCHDPYMID = 2
                and PCDSDATE between '{0}' and '{1}'
                and SALSALEKIND = 'T'
                AND DIVVAL in ({2})
                --and CURVAL = 'M005052'
                and not exists (select * from SALES iade where iade.SALCANSALID = satis.SALID )
                order by DIVVAL,SOCODE ", bastarih, bittarih, magazalar);
                gridPesinatPrim.DataSource = DataDonen(w);
            }
            else
            {
                XtraMessageBox.Show("Geçen Mağaza Yok", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnKasaListele_Click(object sender, EventArgs e)
        {
            string magazalar = "";
            if (cmbMagaza.EditValue != null)
            {

                string q = String.Format(@"
                    declare @WH varchar(2000)
                     if isnull(@WH,'') = ''
                     begin 
                      select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
                      from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
                     end 
                     select 'in (''' + @WH +')'as Magazalar", cmbMagaza.EditValue.ToString());
                magazalar = conn.GetValue(q).Replace(")", "')");

     //           if (cmbMagaza.EditValue.ToString() == "1")
     //           {
     //               string q = String.Format(@"
                    //declare @WH varchar(2000)
                    // if isnull(@WH,'') = ''
                    // begin 
                    //  select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
                    //  from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
                    // end 
                    // select 'in (''' + @WH +')'as Magazalar", cmbMagaza.EditValue.ToString());
     //               magazalar = conn.GetValue(q).Replace(")", "')");

     //           }
     //           else if (cmbMagaza.EditValue.ToString() == "2")
     //           {
     //               string q = String.Format(@"
                    //declare @WH varchar(2000)
                    // if isnull(@WH,'') = ''
                    // begin 
                    //  select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
                    //  from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
                    // end 
                    // select 'in (''' + @WH +')'as Magazalar", "00KS003");
     //               magazalar = conn.GetValue(q).Replace(")", "')");

     //           }
     //           else if (cmbMagaza.EditValue.ToString() == "3")
     //           {

     //               string q = String.Format(@"
                    //declare @WH varchar(2000)
                    // if isnull(@WH,'') = ''
                    // begin 
                    //  select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
                    //  from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
                    // end 
                    // select 'in (''' + @WH +')'as Magazalar", "00KS004");
     //               magazalar = conn.GetValue(q).Replace(")", "')");
     //           }
            }
            else
            {
                magazalar = "not in ('00')";
            }
            string kasasorgu = string.Format(@"select DIVVAL,DIVNAME,NAKİT,
            isnull([Nakit Dekont],0) [Banka Dekont],
            convert(varchar(20),FARK) as FARK,
            isnull(GARANTİ,0) [GARANTİ],
            isnull(VAKIF,0) [VAKIF],
            isnull(AKBANK,0) [AKBANK],
            isnull(HALKBANK,0) [HALKBANK],
            isnull(FİNANSBANK,0) [FİNANSBANK],
            isnull(TEB,0) [TEB],
            isnull(YAPIKREDİ,0) [YAPIKREDİ],
            isnull(İŞBANKASI,0) [İŞBANKASI],
            isnull(İNG,0) [İNG],
            isnull([VAKIF KATILIM],0) [VAKIF KATILIM],
            isnull([ZIRAT BANKASI],0) [ZIRAT BANKASI],
            isnull([ZIRAAT KATILIM],0) [ZIRAAT KATILIM],
            isnull([AVUKAT ÖDEMSİ],0) [AVUKAT ÖDEMSİ],
            isnull([NKOLAY SANAL POS],0) [NKOLAY SANAL POS],
            convert(varchar(20),[KREDİ KARTI TOPLAMI]) as [KREDİ KARTI TOPLAMI],
            isnull([GARANTİ SANAL POS],0) [GARANTİ SANAL POS],
            isnull(FİBA,0) [FİBA],
            isnull([TÜRKİYE FİNANS],0) [TÜRKİYE FİNANS],
            isnull([KREDİ TOPLAMI],0) [TÜRKİYE FİNANS]
            from (
            select * from (
            select DIVVAL,DIVNAME,'NAKİT' as [Ödeme Tipi],isnull(nakit,0)- isnull([Masraf],0) as Giren
            FROM DIVISON 
            outer apply (select SUM(case when PCDSKIND >0 then PCDSCHAMOUNT else  PCDSCHAMOUNT*-1 end ) as nakit 
                FROM PROCEEDS
                        LEFT OUTER JOIN SALES ON SALID=PCDSSALID
                        LEFT OUTER JOIN PROCEEDSCHILD ON PCDSCHPCDSID = PCDSID
                        LEFT OUTER JOIN DEFPAYMENTKIND ON DPYMID=PCDSCHDPYMID
                        WHERE PCDSDIVISON = DIVVAL
                        AND PCDSCOMPANY = '01'  
                        AND PCDSDATE  = '{0}'
                        AND DPYMKIND = 'N'
                        ) sonuc
            outer apply (select SUM(isnull(SABHEXCHAMOUNT,0)) as [Masraf] FROM SAFEBEHAVE iade
                                        WHERE SABHCOMPANY = '01'  
                                        AND iade.SABHDATE  = '{0}'
                                        AND iade.SABHDIVISON = DIVVAL
                                        AND SABHSOURCE = 'ML'
            ) msf
            where DIVSTS  = 1 and DIVSALESTS = 1
            union

            select DIVVAL,DIVNAME, kartlar.[Ödeme Tipi],kartlar.Tutar from DIVISON
            outer apply (select 
                        replace(replace(upper(isnull(DPYMNAME,'NAKİT')),' KREDİSİ',''),' KREDİ KARTI','') as [Ödeme Tipi], 
                        SUM(case when rtrim(ltrim(PCDSDC)) = 0 then PCDSCHAMOUNT else PCDSCHAMOUNT *-1 end ) as Tutar
                        from PROCEEDS			
                        left outer join PROCEEDSCHILD on PCDSID = PCDSCHPCDSID
                        left outer join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                        where PCDSCOMPANY = '01'  
                        AND not exists (select * FROM SAFEBEHAVE where PCDSCHID = SABHPCDSCHID)
                        AND PCDSDATE  = '{0}'
                        AND DIVVAL = PCDSDIVISON
                        group by DPYMNAME,DPYMKIND) kartlar
            where DIVSTS  = 1 and DIVSALESTS = 1
            ) pvt
                                        pivot
                                        (
                                            sum(Giren) for [Ödeme Tipi] in ([NAKİT],[Nakit Dekont],[FARK],[GARANTİ],[VAKIF],[AKBANK],[HALKBANK],[FİNANSBANK],[TEB],[YAPIKREDİ],[İŞBANKASI],[İNG],[VAKIF KATILIM],[ZIRAT BANKASI],[ZIRAAT KATILIM],[AVUKAT ÖDEMSİ],[KREDİ KARTI TOPLAMI],[GARANTİ SANAL POS],[FİBA],[TÜRKİYE FİNANS],[NKOLAY SANAL POS],
                                            [KREDİ TOPLAMI])
                                        )pivotsonuc
            where DIVVAL not in ('I0','WB','00','00')
            and DIVVAL {1}
            ) sonuc", Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy-MM-dd"), magazalar);
        //    if (cmbMagaza.EditValue != null)
        //    {
        //        if (cmbMagaza.EditValue.ToString() == "1")
        //        {
        //            kasasorgu = kasasorgu + String.Format(@" union select DIVVAL,'KAMALAR ' + DIVNAME,NAKİT,
                    //isnull([Nakit Dekont],0) [Banka Dekont],
                    //convert(varchar(20),FARK) as FARK,
                    //isnull(GARANTİ,0) [GARANTİ],
                    //isnull(VAKIF,0) [VAKIF],
                    //isnull(AKBANK,0) [AKBANK],
                    //isnull(HALKBANK,0) [HALKBANK],
                    //isnull(FİNANSBANK,0) [FİNANSBANK],
                    //isnull(TEB,0) [TEB],
                    //isnull(YAPIKREDİ,0) [YAPIKREDİ],
                    //isnull(İŞBANKASI,0) [İŞBANKASI],
                    //isnull(İNG,0) [İNG],
                    //isnull([VAKIF KATILIM],0) [VAKIF KATILIM],
                    //isnull([ZIRAT BANKASI],0) [ZIRAT BANKASI],
                    //isnull([ZIRAAT KATILIM],0) [ZIRAAT KATILIM],
                    //isnull([AVUKAT ÖDEMSİ],0) [AVUKAT ÖDEMSİ],
                 //   isnull([NKOLAY SANAL POS],0) [NKOLAY SANAL POS],
                    //convert(varchar(20),[KREDİ KARTI TOPLAMI]) as [KREDİ KARTI TOPLAMI],
                    //isnull([GARANTİ SANAL POS],0) [GARANTİ SANAL POS],
                    //isnull(FİBA,0) [FİBA],
                    //isnull([TÜRKİYE FİNANS],0) [TÜRKİYE FİNANS],
                    //isnull([KREDİ TOPLAMI],0) [TÜRKİYE FİNANS]
                    //from (
                    //select * from (
                    //select DIVVAL,DIVNAME,'NAKİT' as [Ödeme Tipi],isnull(nakit,0)- isnull([Masraf],0) as Giren
                    //FROM VDB_KAMALAR01.dbo.DIVISON 
                    //outer apply (select SUM(case when PCDSKIND >0 then PCDSCHAMOUNT else  PCDSCHAMOUNT*-1 end ) as nakit 
                       // FROM VDB_KAMALAR01.dbo.PROCEEDS
                             //   LEFT OUTER JOIN VDB_KAMALAR01.dbo.SALES ON SALID=PCDSSALID
                             //   LEFT OUTER JOIN VDB_KAMALAR01.dbo.PROCEEDSCHILD ON PCDSCHPCDSID = PCDSID
                             //   LEFT OUTER JOIN VDB_KAMALAR01.dbo.DEFPAYMENTKIND ON DPYMID=PCDSCHDPYMID
                             //   WHERE PCDSDIVISON = DIVVAL
                             //   AND PCDSCOMPANY = '01'  
                             //   AND PCDSDATE  = '{0}'
                             //   AND DPYMKIND = 'N'
                             //   ) sonuc
                    //outer apply (select SUM(isnull(SABHEXCHAMOUNT,0)) as [Masraf] FROM VDB_KAMALAR01.dbo.SAFEBEHAVE iade
                                //			    WHERE SABHCOMPANY = '01'  
                                //			    AND iade.SABHDATE  = '{0}'
                                //			    AND iade.SABHDIVISON = DIVVAL
                                //			    AND SABHSOURCE = 'ML'
                    //) msf
                    //where DIVSTS  = 1 and DIVSALESTS = 1
                    //union

                    //select DIVVAL,DIVNAME, kartlar.[Ödeme Tipi],kartlar.Tutar from VDB_KAMALAR01.dbo.DIVISON
                    //outer apply (select 
                             //   replace(replace(upper(isnull(DPYMNAME,'NAKİT')),' KREDİSİ',''),' KREDİ KARTI','') as [Ödeme Tipi], 
                             //   SUM(case when rtrim(ltrim(PCDSDC)) = 0 then PCDSCHAMOUNT else PCDSCHAMOUNT *-1 end ) as Tutar
                             //   from VDB_KAMALAR01.dbo.PROCEEDS			
                             //   left outer join VDB_KAMALAR01.dbo.PROCEEDSCHILD on PCDSID = PCDSCHPCDSID
                             //   left outer join VDB_KAMALAR01.dbo.DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                             //   where PCDSCOMPANY = '01'  
                             //   AND not exists (select * FROM VDB_KAMALAR01.dbo.SAFEBEHAVE where PCDSCHID = SABHPCDSCHID)
                             //   AND PCDSDATE  = '{0}'
                             //   AND DIVVAL = PCDSDIVISON
                             //   group by DPYMNAME,DPYMKIND) kartlar
                    //where DIVSTS  = 1 and DIVSALESTS = 1

                    //) pvt
                                //			    pivot
                                //			    (
                                //				    sum(Giren) for [Ödeme Tipi] in ([NAKİT],[Nakit Dekont],[FARK],[GARANTİ],[VAKIF],[AKBANK],[HALKBANK],[FİNANSBANK],[TEB],[YAPIKREDİ],[İŞBANKASI],[İNG],[VAKIF KATILIM],[ZIRAT BANKASI],[ZIRAAT KATILIM],[AVUKAT ÖDEMSİ],[KREDİ KARTI TOPLAMI],[GARANTİ SANAL POS],[FİBA],[TÜRKİYE FİNANS],[NKOLAY SANAL POS],
                                //				    [KREDİ TOPLAMI])
                                //			    )pivotsonuc
                    //where DIVVAL not in ('I0','WB','00','00')
                    //) sonuc", Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy-MM-dd"));
        //        }
        //    }
            var dt = DataDonen(kasasorgu);
            gridKasa.DataSource = dt;
            viewKasa.Columns["FARK"].ColumnType.ToString();
            viewKasa.Columns["KREDİ KARTI TOPLAMI"].ColumnType.ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int sira = i + 2;
                string fark = string.Format(@"+D{0}-C{0}", sira.ToString());
                string ktoplam = string.Format(@"=TOPLA(F{0}:R{0})", sira);
                viewKasa.SetRowCellValue(i, viewKasa.Columns["FARK"], fark);
                viewKasa.SetRowCellValue(i, viewKasa.Columns["KREDİ KARTI TOPLAMI"], ktoplam);
            }
            //viewKasa.Columns["FARK"].UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            //viewKasa.Columns["FARK"].UnboundExpression = "[NAKİT]-[Banka Dekont]";
            string DiskPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Kasa Kontrol", cmbMagaza.Text);
            //string fullPath = Path.Combine(dosyayolu, Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy-MM-dd"));
            string ftpPath = string.Format(@"/{0}//{1}", "Kasa Kontrol", cmbMagaza.Text);
            string KasaAy = Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy") + "-" + Convert.ToDateTime(dteKasaTarih.Value).ToString("MMMM");
            string FullDiskPath = Path.Combine(DiskPath, KasaAy);
            string FullFtpPath = string.Format("{0}//{1}", ftpPath, KasaAy);
            viewKasa.OptionsView.BestFitMaxRowCount = -1;
            viewKasa.BestFitColumns(true);
            CreateDirectoryIfNotExists(txtPart.Text = FullDiskPath);
            CreateFolderFTP("/Kasa Kontrol");
            CreateFolderFTP(ftpPath);
            CreateFolderFTP(FullFtpPath);
            folder = FullFtpPath;
            txtPart.Text = FullDiskPath;
            groupControl10.Enabled = false;
        }
        string pass = "Kama";
        string uys = "Kama2023!";
        string url = "ftp://192.168.4.22//PaylasilanKasalar/";
        string local = "ftp://192.168.4.21//PaylasilanKasalar/";
        string folder = "";
        private void btnExcel_Click(object sender, EventArgs e)
        {

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string filePath1 = saveFileDialog.FileName;
            //}
            string filePath = Path.Combine(txtPart.Text, Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy-MM-dd") + ".xls");

            viewKasa.ExportToXls(filePath, new XlsExportOptions
            {
                ExportMode = XlsExportMode.SingleFile,
                TextExportMode = TextExportMode.Value,
                ShowGridLines = true,
                FitToPrintedPageWidth = true,
                FitToPrintedPageHeight = true,

            });
            //FtpTransfer(filePath,folder);
            #region eski olanlar
            //string path = Path.Combine(txtPart.Text, Convert.ToDateTime(dteKasaTarih.Value).ToString("yyyy-MM-dd") + ".xlsx");
            //viewKasa.ExportToXlsx(path);

            //FtpTransfer(path, folder);
            //Process.Start(path);
            #endregion
        }
        OpenFileDialog file = new OpenFileDialog();
        public void FtpTransfer(string fullPath)
        {

            string From = folder + "/" + Path.GetFileName(fullPath);
            string To = url + From;

            //string newName = "";
            //string s1 = fullPath;


            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(pass, uys);
                client.UploadFile(To, WebRequestMethods.Ftp.UploadFile, fullPath);
                //client.UploadFileAsync(new Uri(To), newName, fullPath);


                //FtpWebRequest FTP;
                //try
                //{
                //    FTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(To));
                //    FTP.UseBinary = true;
                //    string yeniisim = newName;
                //    FTP.RenameTo = yeniisim;
                //    FTP.Credentials = new NetworkCredential(pass, uys);
                //    FTP.Method = WebRequestMethods.Ftp.Rename;
                //    FtpWebResponse response = (FtpWebResponse)FTP.GetResponse();
                //    Console.WriteLine(response.StatusDescription);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}


            }

        }

        public void CreateFolderFTP(string directoryPath)
        {
            try
            {
                string path = directoryPath; //_magaza.Replace(" ", "") + "/" + Convert.ToDateTime(_tarih).ToString("yyyy") + "-" + Convert.ToDateTime(_tarih).ToString("MMMM");
                string xmlPath = url + path;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(xmlPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(pass, uys);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {

            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridKasa.DataSource = null;
            groupControl10.Enabled = true;
            panelControl7.Enabled = true;
        }

        string Fullpath = "";
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            file.InitialDirectory = "C://Desktop";
            //Your opendialog box title name.
            file.Title = "Yüklenecek Dosya Seçin.";
            //which type file format you want to upload in database. just add them. default =  "Select Valid Document(*.pdf; *.Jpg; *.JPEG)|*.pdf; *.docx; *.xlsx; *.html";
            file.Filter = "*.xlsx; *.xls|*.xlsx; *.xls;";
            //FilterIndex property represents the index of the filter currently selected in the file dialog box.
            file.FilterIndex = 1;
            try
            {
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (file.CheckFileExists)
                    {
                        Fullpath = System.IO.Path.GetFullPath(file.FileName);
                        txtPart.Text = Fullpath;
                    }
                }
                else
                {
                    MessageBox.Show("Dosya Seçilmedi");
                    txtPart.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                    MessageBox.Show($"'{directoryPath}' dizini oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"'{directoryPath}' dizini oluşturulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (Fullpath == "")
            {
                if (txtPart.Text != "")
                {
                    Fullpath = txtPart.Text;
                    FtpTransfer(Fullpath);
                }
                else
                {
                    MessageBox.Show("Dosya Seçilmedi");
                }                
            }
            else
            {
                FtpTransfer(Fullpath);
            }
        }

        private void btnExcelPrim_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Kasiyer Primi", dteSonucStart.Value.Month.ToString() + " Ay");
            CreateDirectoryIfNotExists(path);
            string file = Path.Combine(path, dteSonucStart.Text.ToString() + " Ayı " + "Kasiyer Peşinat Primi.xlsx");
            viewPesinatPrim.ExportToXlsx(file);
            //Process.Start(path + ".xlsx");
        }

        private void btnKrediListele_Click(object sender, EventArgs e)
        {

            string magazalar = "";
            if (cmbMagaza2.EditValue == null)
            {
                magazalar = "not in ('00')";
            }
            else
            {
                string w = String.Format(@"
                    declare @WH varchar(2000)
                     if isnull(@WH,'') = ''
                     begin 
                      select @WH = COALESCE(@WH + ''',''','') + rtrim(POTNOTES1)
                      from (select POTNOTES1 from POTENCY where POTSOURCE = 'DIVISON' and POTDEPART like '{0}%' and POTSTS = 1) T
                     end 
                     select 'in (''' + @WH +')'as Magazalar", cmbMagaza2.EditValue.ToString());
                magazalar = conn.GetValue(w).Replace(")", "')");
                //magazalar = MGZLIST;

            }

            string q = string.Format(@"select DIVVAL,SALDATE,DIVNAME,CURVAL,CURNAME,
            case when SALSHIPKIND='S' then 'SONRA TESLİM' else 'HEMEN TESLİM' end as [SATIŞ ŞEKLİ],
            DPRNAME as [SATIŞ TİPİ],
            CONVERT(VARCHAR(15), CAST(sum(SATISTUTAR)+sum(IADETUTAR) AS MONEY), 1) as [NET TUTAR],
            DPYMNAME as [ÖDEME ŞEKLİ],
            CONVERT(VARCHAR(15), CAST(sum(isnull(ODEME,0)) AS MONEY), 1) as [ÖDEME TUTARI],
            CONVERT(VARCHAR(15), CAST(convert(numeric(18,0),sum(isnull(ODEME,0))*0.02) AS MONEY), 1) as [Verilmesi Gereken],
            CONVERT(VARCHAR(15), CAST([Masraf] AS MONEY), 1) as Ödenen
            from CURRENTS
            inner join
            (
            select SALCURID, SALID,SALDATE,SALCANSALID,SALDIVISON, SALSHIPKIND,DPRNAME,
              SUM(CASE WHEN SALES.SALID >= 0 THEN INVCHBALANCE - CASE WHEN 1 = 1 THEN 0 ELSE INVCHVAT END ELSE 0 END) SATISTUTAR, 
              SUM(CASE WHEN SALES.SALID < 0 THEN INVCHBALANCE - CASE WHEN 1 = 1 THEN 0 ELSE INVCHVAT END ELSE 0 END)*-1 IADETUTAR,
              CASE WHEN SALES.SALID > 0 THEN isnull(odm,pesinattuar) ELSE isnull(odm*-1,pesinattuar*-1) END as ODEME,
              isnull(DPYMNAME,pesinattipi) as DPYMNAME
            from SALES
              LEFT OUTER JOIN INVOICE WITH (NOLOCK) ON INVSALID = SALES.SALID 
              LEFT OUTER JOIN INVOICECHILD WITH (NOLOCK) ON INVCHINVID = INVID 
              LEFT OUTER JOIN INVOICECHILDPROBH WITH (NOLOCK) ON INVOICECHILDPROBH.INVCHPBHID = INVOICECHILD.INVCHID 
              LEFT OUTER JOIN DEFCANCEL ON DCANID = SALES.SALCANID   
              LEFT OUTER JOIN SALESPAYMENTKINDBH WITH (NOLOCK) ON SALPKBHSALID = SALES.SALID 
              LEFT OUTER JOIN DEFPAYMENTKINDFORPROCEED WITH (NOLOCK) ON DPFPID = SALPKBHDPFPID 
              LEFT OUTER JOIN DEFPRICE WITH (NOLOCK) ON DEFPRICE.DPRID = INVOICECHILD.INVCHDPRID 
              LEFT OUTER JOIN DEFPAYMENTPLAN WITH (NOLOCK) ON DPAYPID = INVOICECHILD.INVCHPAYPID 
              outer apply (select sum(PCDSAMOUNT) as pesinattuar, DPYMNAME as pesinattipi from PROCEEDS 			   
                           inner join PROCEEDSCHILD on PCDSID = PCDSCHPCDSID
                           inner join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                           where PCDSSALID = SALID 
                           group by DPYMNAME) as PROCEEDS
              outer apply (select INSSALID,DPYMNAME,sum(INSPCDAMOUNT) as odm from INSTALMENT
                               left outer join INSTALMENTPROCEEDS on INSPCDINSID = INSID
                               left outer join DEFPAYMENTKIND on DPYMID = INSPCDDPYMID
                               where INSSALID = SALID --and substring(DPYMVAL,1,2)=substring(DPRVAL,1,2)
                               group by INSSALID,DPYMNAME) odeme 
            WHERE 
              SALES.SALCOMPANY = '01' 
              AND SALES.SALSHIPKIND = 'H' 
              AND SALES.SALDATE between '{1}' and '{2}'
              AND (DPRVAL like 'TF%' or DPRVAL like 'FB%')
              --AND SALCURID = 1788343
              group by SALCURID,SALID,SALDATE,SALSHIPKIND,DPRNAME,odm,pesinattuar,pesinattipi,DPYMNAME,SALCANSALID,SALDIVISON

              union

            select SALCURID,SALID,SALDATE,SALCANSALID, SALDIVISON,SALSHIPKIND,DPRNAME,
              SUM(CASE WHEN ORDERS.ORDID >= 0 THEN ORDCHBALANCE - CASE WHEN 1 = 1 THEN 0 ELSE ORDCHVAT END ELSE 0 END) SATISTUTAR, 
              SUM(CASE WHEN ORDERS.ORDID < 0 THEN ORDCHBALANCE - CASE WHEN 1 = 1 THEN 0 ELSE ORDCHVAT END ELSE 0 END)*-1 IADETUTAR,
              isnull(CASE WHEN SALES.SALID > 0 THEN isnull(odeme.odm,pesinattuar) ELSE isnull(odeme.odm*-1,pesinattuar*-1) END,
              CASE WHEN SALES.SALID > 0 THEN isnull(nakit.odm,pesinattuar) ELSE isnull(nakit.odm*-1,pesinattuar*-1)end) as ODEME,
              isnull(isnull(odeme.DPYMNAME,pesinattipi),nakit.DPYMNAME) as DPYMNAME
              from SALES
              LEFT OUTER JOIN ORDERS WITH (NOLOCK) ON ORDERS.ORDSALID = SALES.SALID 
              LEFT OUTER JOIN ORDERSCHILD WITH (NOLOCK) ON ORDCHORDID = ORDERS.ORDID 
              LEFT OUTER JOIN DEFCANCEL ON DCANID = SALES.SALCANID   
              LEFT OUTER JOIN SALESPAYMENTKINDBH WITH (NOLOCK) ON SALPKBHSALID = SALES.SALID 
              LEFT OUTER JOIN DEFPAYMENTKINDFORPROCEED WITH (NOLOCK) ON DPFPID = SALPKBHDPFPID 
              LEFT OUTER JOIN DEFPRICE WITH (NOLOCK) ON DEFPRICE.DPRID = ORDERSCHILD.ORDCHDPRID 
              LEFT OUTER JOIN DEFPAYMENTPLAN WITH (NOLOCK) ON DPAYPID = ORDERSCHILD.ORDCHPAYPID  
              outer apply (select sum(PCDSAMOUNT) as pesinattuar, DPYMNAME as pesinattipi from PROCEEDS 			   
                           inner join PROCEEDSCHILD on PCDSID = PCDSCHPCDSID
                           inner join DEFPAYMENTKIND on DPYMID = PCDSCHDPYMID
                           where PCDSSALID = SALID 
                           group by DPYMNAME) as PROCEEDS
              outer apply (select INSSALID,DPYMNAME,sum(INSPCDAMOUNT) as odm from INSTALMENT
                               left outer join INSTALMENTPROCEEDS on INSPCDINSID = INSID
                               left outer join DEFPAYMENTKIND on DPYMID = INSPCDDPYMID
                               where INSSALID = isnull(SALCANSALID,SALID) and substring(DPYMVAL,1,2)=substring(DPRVAL,1,2)
                               group by INSSALID,DPYMNAME) odeme 
              outer apply (select INSSALID,DPYMNAME,sum(INSPCDAMOUNT) as odm from INSTALMENT
                               left outer join INSTALMENTPROCEEDS on INSPCDINSID = INSID
                               left outer join DEFPAYMENTKIND on DPYMID = INSPCDDPYMID
                               where INSSALID = isnull(SALCANSALID,SALID) and substring(DPYMVAL,1,2)!=substring(DPRVAL,1,2)
                               group by INSSALID,DPYMNAME) nakit 
            WHERE 
              SALES.SALCOMPANY = '01' 
              AND SALES.SALSHIPKIND = 'S' 
              AND SALES.SALDATE between '{1}' and '{2}'
              AND (DPRVAL like 'TF%' or DPRVAL like 'FB%')
              --and SALCURID = 1788343
              group by SALCURID,SALID,SALDATE,SALSHIPKIND,DPRNAME,odeme.odm,odeme.DPYMNAME,pesinattuar,pesinattipi,SALCANSALID,nakit.DPYMNAME,nakit.odm,SALDIVISON
            ) son on SALCURID = CURID
            left outer join DIVISON on DIVVAL = son.SALDIVISON
            outer apply(select SUM(isnull(SABHEXCHAMOUNT,0)) as [Masraf] from SAFEBEHAVE where SABHCOMPANY = '01' AND SABHDATE  between '{1}' and '{2}' AND SABHDIVISON = DIVVAL AND SABHSOURCE = 'ML' AND SABHVERSENAME like '%'+CURVAL+'%') masraf
            where DIVVAL {0}
            group by DIVVAL,SALDATE,DIVNAME,CURVAL,CURNAME,SALSHIPKIND,DPRNAME,DPYMNAME,[Masraf]
            having sum(SATISTUTAR)+sum(IADETUTAR) != 0
              ORDER BY 
              DIVVAL,DPRNAME,DPYMNAME desc,CURVAL
            OPTION(RECOMPILE)", magazalar, Convert.ToDateTime(dteKrediTarihStart.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteKrediTarihEnd.Value).ToString("yyyy-MM-dd"));
            var dt = DataDonen(q);
            gridKredi.DataSource = dt;
        }

        private void ViewKredi_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {

                if (ViewKredi.DataRowCount > 0)
                {
                    if (ViewKredi.GetRowCellValue(e.RowHandle, "NET TUTAR") != null)
                    {
                        var renkKodu = View.GetRowCellValue(e.RowHandle, "NET TUTAR").ToString();
                        var prialdı = View.GetRowCellValue(e.RowHandle, "Ödenen").ToString();
                        var odeme = View.GetRowCellValue(e.RowHandle, "ÖDEME TUTARI").ToString();
                        var renkler = double.Parse(renkKodu);
                        if (renkler <= 0)
                        {

                            if (prialdı != "")
                            {
                                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#fc3ccf");
                                e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fc3c3c");
                            }
                            else
                            {
                                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#021bfa");
                                e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fc3c3c");
                            }
                        }
                        else if (renkKodu == "3")
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#f8f63f");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#f8f63f");
                        }
                        else if (renkKodu != "0.00")
                        {
                            if (prialdı != "")
                            {
                                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                            {
                                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#021bfa");
                                e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#021bfa");
                                e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c0cc1b");

                            }
                            if (odeme == "0.00")
                            {
                                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#fa0000");
                                e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fa0000");
                                e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            }
                        }
                    }


                }
            }
        }


        private void btnTeslimatKaldir_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select 
            CDRID,
            ORDCHID,
            PROVAL,
            PRONAME,
            ORDCHBALANCE, 
            ORDCHBALANCEQUAN , 
            ORDCHIRSQUAN
            FROM  CUSDELIVER
            LEFT OUTER JOIN ORDERSCHILD WITH (NOLOCK) ON ORDCHID = CDRORDCHID
            LEFT OUTER JOIN PRODUCTS WITH (NOLOCK) ON ORDCHPROID = PROID
            LEFT OUTER JOIN DEEDS WITH (NOLOCK) ON DEEDID=CDRDEEDID
            LEFT OUTER JOIN PRODUCTSBEHAVE B WITH (NOLOCK) ON B.PROBHDEEDID= DEEDID AND B.PROBHORDCHID=CDRORDCHID
            WHERE CUSDELIVER.CDRSALID = {0}", txtTeslimatKaldir.Text);
            var dt = DataDonen(q);
            gridTeslimatKaldir.DataSource = dt;
        }


        private void btnTeslimatKaldirYeni_Click(object sender, EventArgs e)
        {

        }

        private void btnTeslimatKaldır_Click(object sender, EventArgs e)
        {
            var selectrow = ViewTeslimatKaldir.GetSelectedRows();
            for (int i = 0; i < selectrow.Length; i++)
            {
                var id = ViewTeslimatKaldir.GetRowCellValue(selectrow[i], "CDRID");
                var ordchid = ViewTeslimatKaldir.GetRowCellValue(selectrow[i], "ORDCHID");
                if (id != null)
                {
                    SqlCommand cmd = new SqlCommand(string.Format("delete FROM CUSDELIVER WHERE CDRID={0} AND CDRSTS='99' AND CDRDEEDID IS NULL", id), sql);
                    cmd.CommandType = CommandType.Text;
                    sql.Open();
                    cmd.ExecuteNonQuery();
                    SqlCommand upd = new SqlCommand(string.Format("update ORDERSCHILD set ORDCHBALANCEQUAN = 0,ORDCHIRSQUAN=0,ORDCHEXCH = 'TL', ORDCHBEYOND = 0   where ORDCHID = {0}", ordchid), sql);
                    upd.CommandType = CommandType.Text;
                    upd.ExecuteNonQuery();
                    sql.Close();
                }
            }
        }

        private void btnKasaTarihListele_Click(object sender, EventArgs e)
        {
            if (cmbKasaTarihMagaza.EditValue != null)
            {
                gridKasaTarih.DataSource = null;
                ViewKasaTarih.Columns.Clear();
                string q = string.Format(@"select SABHID,SABHDATE as [İşlenen Kasa Tarihi],SABHAMOUNT as [Kasa Toplam Tutarı],SFCOCEDSDATE as [Girilmesi Gereken Tarih], '' Sonuc
                from SAFEBEHAVE
                inner join SAFECONTROLCENTERDAYSUM on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1
                where SABHDIVISON = '{0}'
                and SFCOCEDSDATE between '{1}' and '{2}' and SABHSOURCE = 'BN1'
                and SFCOCEDSSABHCREATED = 1
                and SABHDATE != SFCOCEDSDATE
                order by 4", cmbKasaTarihMagaza.EditValue, Convert.ToDateTime(dteKasaTarihStart.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteKasaTarihEnd.EditValue).ToString("yyyy-MM-dd"));
                gridKasaTarih.DataSource = DataDonen(q);
            }
            else
            {
                gridKasaTarih.DataSource = null;
                ViewKasaTarih.Columns.Clear();
                string q = string.Format(@"select SABHID,DIVVAL as [Mağaza kodu],DIVNAME as [Mağaza Adı],SABHDATE as [İşlenen Kasa Tarihi],SABHAMOUNT as [Kasa Toplam Tutarı],SFCOCEDSDATE as [Girilmesi Gereken Tarih], '' Sonuc
                from SAFEBEHAVE
                inner join DIVISON on DIVVAL = SABHDIVISON
                inner join SAFECONTROLCENTERDAYSUM on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1
                where SFCOCEDSDATE between '{1}' and '{2}' and SABHSOURCE = 'BN1'
                and SFCOCEDSSABHCREATED = 1
                and SABHDATE != SFCOCEDSDATE
                order by 2,6", cmbKasaTarihMagaza.EditValue, Convert.ToDateTime(dteKasaTarihStart.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteKasaTarihEnd.EditValue).ToString("yyyy-MM-dd"));
                gridKasaTarih.DataSource = DataDonen(q);
            }
        }

        private void btnKasaTarihDuzelt_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = ViewKasaTarih.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;

            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var id = ViewKasaTarih.GetRowCellValue(selectedRows[i], "SABHID");
                string updateq = string.Format(@"update SAFEBEHAVE set SABHDATE = SFCOCEDSDATE 
                from SAFEBEHAVE
                inner join SAFECONTROLCENTERDAYSUM on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1
                where SABHID = '{0}'
                and SFCOCEDSSABHCREATED = 1", id);
                SqlCommand cmd = new SqlCommand(updateq, sql);
                if (sql.State == ConnectionState.Closed)
                {
                    sql.Open();
                }
                var sonuc = cmd.ExecuteNonQuery();
                if (sonuc > 0)
                {
                    ViewKasaTarih.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                    ViewKasaTarih.RefreshRow(selectedRows[i]);
                    success++;
                }
                else
                {
                    ViewKasaTarih.SetRowCellValue(selectedRows[i], "Sonuc", "Hata Oldu Kontrol Edin");
                    ViewKasaTarih.RefreshRow(selectedRows[i]);
                    error++;
                }
                sql.Close();
                progressForm.PerformStep(this);
            }
            this.Enabled = true;
            progressForm.Hide(this);
            XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private void ViewKasaTarih_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (ViewKasaTarih.DataRowCount > 0)
            {
                if (ViewKasaTarih.GetRowCellValue(e.RowHandle, "Sonuc") != null)
                {
                    var renkKodu = ViewKasaTarih.GetRowCellValue(e.RowHandle, "Sonuc").ToString();
                    if (renkKodu == "")
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ceff08");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#ceff08");
                    }
                    else if (renkKodu == "Başarılı")
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#00de7e");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#00de7e");
                    }
                    else if (renkKodu == "Hata Oldu Kontrol Edin")
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#d62c27");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#d62c27");
                    }
                }
            }
        }

        private void btnKasaTutar_Click(object sender, EventArgs e)
        {
            string magazalar = "";
            if (cmbKasaTutar.EditValue != null)
            {
                magazalar = MGZLIST;
                //if (cmbKasaTutar.EditValue.ToString() == "1")
                //{
                //    MGZLIST = "in ('43', '10', '12', '23', '44', '17', '18', '51', '14', '19','27','07','22','24','41','20')";
                //}
                //else if (cmbKasaTutar.EditValue.ToString() == "2")
                //{
                //    MGZLIST = "in ('26','32','37','06','13','47','29','38','46','31','45','09','36','05','21','02','01','49','04')";
                //}
                //else if (cmbKasaTutar.EditValue.ToString() == "3")
                //{
                //    MGZLIST = "in ('42','03','35','30','39','11','50','25','33','16','08','40','53','28','52','15','34')";
                //}
            }
            else
            {
                magazalar = "not in ('00')";
            }

            string q = string.Format(@"select isnull(SABHID,0),SFCOCEDSID,SFCOCEDSDATE,SFCOCEDSAMOUNT1,isnull(SABHDEEDNOTES,''),DIVNAME,
            case when SFCOCEDSSABHCREATED = 1 then 'BANKAYA YATIRLI İŞLEMİ YAPILMIŞ'
            when SFCOCEDSSENDCASH = 1 then 'MERKEZE GÖNDERİDİ İŞLEMİ YAPILMIŞ' 
            else 'KONTROL EDİLECEK' end as Durum
            from SAFECONTROLCENTERDAYSUM with (nolock)
            left outer join DIVISON on DIVVAL = SFCOCEDSDIVISON
            left outer join SAFEBEHAVE with (nolock) on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1 and SABHDATE = SFCOCEDSDATE
            where SFCOCEDSDATE = '{0}'", Convert.ToDateTime(dteKasaTutar.EditValue).ToString("yyyy-MM-dd"));
            if (cmbKasaTutar.EditValue != null)
            {
                q = q + string.Format(" and SFCOCEDSDIVISON {0}", magazalar);// cmbKasaTutar.EditValue);

            }
            q = q + " order by DIVVAL";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            gridKasaTutar.DataSource = dt;
            btnKasaTutarGuncelle.Enabled = true;
            txtKasaTutarYeniDeger.Enabled = true;
            btnKasaTutar.Enabled = false;
            btnKasaTutarYeni.Enabled = true;
            dteKasaTutarNewTarih.Enabled = true;
        }
        internal DataTable Sorgu(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        private void btnKasaTutarGuncelle_Click(object sender, EventArgs e)
        {
            sql.Open();
            var SABHID = ViewKasaTutar.GetRowCellValue(0, "SABHID").ToString();
            if (SABHID != "0")
            {
                var TTSID = conn.GetValue($"select TTSID from TOKENTIES where TTSSASHID = {SABHID}");
                string update1 = string.Format(@"update SAFEBEHAVE set SABHAMOUNT = '{1}' where SABHID = '{0}'", SABHID, txtKasaTutarYeniDeger.Text);
                string update2 = string.Format(@"update SAFEBEHAVECHILD set SABHCHEXCHAMOUNT = {1}, SABHCHAMOUNT = {1} where SABHCHSABHID = '{0}'", SABHID, txtKasaTutarYeniDeger.Text);
                string update3 = string.Format(@"uupdate ACCTOKEN set ACTOAMOUNT = {1} where ACTOTTSID = '{0}'", TTSID, txtKasaTutarYeniDeger.Text);
                string update4 = string.Format(@"update ACCBEHAVE set ACBHAMOUNT = {1},ACBHEXCHAMOUNT = {1},ACBHBHEXCHAMOUNT = {1} where ACBHDEEDNO = '{0}'", SABHID, txtKasaTutarYeniDeger.Text);
                SqlCommand cmd1 = new SqlCommand(update1, sql);
                SqlCommand cmd2 = new SqlCommand(update2, sql);
                SqlCommand cmd3 = new SqlCommand(update3, sql);
                SqlCommand cmd4 = new SqlCommand(update4, sql);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                cmd4.ExecuteNonQuery();

            }

            var kontrol = Sorgu($"select * from SAFEBEHAVE where SABHTIESABHID = {SABHID}");
            var SFCOCEDSID = ViewKasaTutar.GetRowCellValue(0, "SFCOCEDSID").ToString();
            string update5 = string.Format(@"update SAFECONTROLCENTERDAYSUM set SFCOCEDSAMOUNT1 = '{0}' where SFCOCEDSID = '{1}'", txtKasaTutarYeniDeger.Text, SFCOCEDSID);
            SqlCommand cmd5 = new SqlCommand(update5, sql);
            cmd5.ExecuteNonQuery();

            sql.Close();


            btnKasaTutarGuncelle.Enabled = false;
            txtKasaTutarYeniDeger.Enabled = false;
            btnKasaTutar.Enabled = true;
        }

        private void btnKasaTutarYeni_Click(object sender, EventArgs e)
        {
            btnKasaTutar.Enabled = true;
            txtKasaTutarYeniDeger.Enabled = false;
            btnKasaTutarGuncelle.Enabled = false;
            dteKasaTutarNewTarih.Enabled = false;
            txtKasaTutarYeniDeger.Text = "";
            dteKasaTutarNewTarih.EditValue = DateTime.Now.AddDays(-1);
        }

        private void btnKasaSil_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select SABHID,SFCOCEDSID,DIVVAL as [Mağaza kodu],DIVNAME as [Mağaza Adı],SABHDATE as [İşlenen Kasa Tarihi],SABHAMOUNT as [Kasa Toplam Tutarı],SFCOCEDSDATE as [Girilmesi Gereken Tarih], '' Sonuc
                from SAFEBEHAVE
                inner join DIVISON on DIVVAL = SABHDIVISON
                inner join SAFECONTROLCENTERDAYSUM on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1
                where SFCOCEDSDATE between '{0}' and '{1}' 
                and SABHSOURCE = 'BN1'
                and SFCOCEDSSABHCREATED = 1
                and SABHDATE = SFCOCEDSDATE
                and SABHDIVISON = '{2}'
                order by 2,6", Convert.ToDateTime(dteKasaSilStart.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteKasaSilEnd.EditValue).ToString("yyyy-MM-dd"), cmbKasaSilMagaza.EditValue);
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridKasaSil.DataSource = dt;
        }

        private void conSeciliSilID_Click(object sender, EventArgs e)
        {
            var selectedRows = viewKasaSil.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;

            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var SABHID = viewKasaSil.GetRowCellValue(selectedRows[i], "SABHID").ToString();
                var SFCOCEDSID = viewKasaSil.GetRowCellValue(selectedRows[i], "SFCOCEDSID").ToString();
                try
                {
                    string d1 = string.Format("DELETE BANKBEHAVE WHERE BABHSABHID = {0}", SABHID);
                    string d2 = string.Format("DELETE SAFECONTROLCENTERDAYSUM where SFCOCEDSID = {0}", SFCOCEDSID);
                    string d3 = string.Format("DELETE SAFEBEHAVE WHERE SABHID = {0}", SABHID);
                    SqlCommand cmd1 = new SqlCommand(d1, sql);
                    SqlCommand cmd2 = new SqlCommand(d2, sql);
                    SqlCommand cmd3 = new SqlCommand(d3, sql);
                    sql.Open();
                    var sili1 = cmd1.ExecuteNonQuery();
                    var sili2 = cmd2.ExecuteNonQuery();
                    var sili3 = cmd3.ExecuteNonQuery();
                    sql.Close();
                    viewKasaSil.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                    viewKasaSil.RefreshRow(selectedRows[i]);
                    success++;
                }
                catch (Exception ex)
                {
                    error++;
                    MessageBox.Show(ex.Message);
                }

                progressForm.PerformStep(this);
            }
            this.Enabled = true;
            progressForm.Hide(this);
            gridKasaSil.DataSource = null;
            dteKasaSilStart.EditValue = DateTime.Now.AddDays(-1);
            dteKasaSilEnd.EditValue = DateTime.Now.AddDays(-1);
            XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void btnKasaSilID_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select SABHID,SFCOCEDSID,DIVVAL as [Mağaza kodu],DIVNAME as [Mağaza Adı],SABHDATE as [İşlenen Kasa Tarihi],SABHAMOUNT as [Kasa Toplam Tutarı],SFCOCEDSDATE as [Girilmesi Gereken Tarih], '' Sonuc
                from SAFEBEHAVE
                inner join DIVISON on DIVVAL = SABHDIVISON
                inner join SAFECONTROLCENTERDAYSUM on SFCOCEDSDIVISON = SABHDIVISON and SABHAMOUNT = SFCOCEDSAMOUNT1
                where SABHSOURCE = 'BN1'
                and SFCOCEDSSABHCREATED = 1
                and SABHDATE = SFCOCEDSDATE
                and SFCOCEDSID = '{0}'
                order by 2,6", txtKasaSilID.Text);
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridKasaSil.DataSource = dt;
        }


        private void btnKasaMerkezDuzeltListe_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select SFCOCEDSID,DIVVAL,DIVNAME,SFCOCEDSDATE, SFCOCEDSAMOUNT1,SONAME + space(1) + SOSURNAME as Kasiyer,'' as Durum,'' as Sonuc
                    from SAFECONTROLCENTERDAYSUM 
                    left outer join SOCIAL on SOCODE = SFCOCEDSSOCODE
                    left outer join DIVISON on DIVVAL = SFCOCEDSDIVISON
                    where SFCOCEDSSENDCASH = 1
                    and SFCOCEDSDATE >= '2024-01-01'
                    and SFCOCEDSDATE between '{0}' and '{1}'", Convert.ToDateTime(dteKasaMerkezDuzeltStart.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteKasaMerkezDuzeltEnd.EditValue).ToString("yyyy-MM-dd"));
            if (cmbKasaMerkezDuzeltMagaza.EditValue != null)
            {
                q = q + string.Format(@" and SFCOCEDSDIVISON = '{0}'", cmbKasaMerkezDuzeltMagaza.EditValue);
            }
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridKasaMerkezDuzelt.DataSource = dt;

            for (int i = 0; i < viewKasaMerkezDuzelt.RowCount; i++)
            {
                string SFCOCEDSAMOUNT1 = viewKasaMerkezDuzelt.GetRowCellValue(i, "SFCOCEDSAMOUNT1").ToString();
                string SFCOCEDSDATE = Convert.ToDateTime(viewKasaMerkezDuzelt.GetRowCellValue(i, "SFCOCEDSDATE").ToString()).ToString("yyyy-MM-dd");
                string DIVVAL = viewKasaMerkezDuzelt.GetRowCellValue(i, "DIVVAL").ToString();
                string qq = string.Format(@"select * from SAFEBEHAVE where SABHSOURCE = 'VI1' and SABHDIVISON = '{0}' and SABHAMOUNT = '{1}' and SABHDATE = '{2}'", DIVVAL, SFCOCEDSAMOUNT1, SFCOCEDSDATE);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(qq, sql);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count != 0)
                {
                    viewKasaMerkezDuzelt.SetRowCellValue(i, "Durum", "İşlem olmuş");
                }
                else
                {
                    viewKasaMerkezDuzelt.SetRowCellValue(i, "Durum", "");
                }
            }

        }



        private void btnKasaMerkezDuzeltYeni_Click(object sender, EventArgs e)
        {
            cmbKasaMerkezDuzeltMagaza.EditValue = null;
            gridKasaMerkezDuzelt.DataSource = null;
        }

        public static string SOCODE;


        //public static string magazacikisID = "";
        //public static string merkezgirisID = "";
        //public static string merkezkasiyercikisID = "";
        //public static string merkezkasiyergirisID = "";

        string magazacikisID = "";
        string magazagirisID = "";
        string kasiyercikisID = "";
        string kasiyergirisID = "";
        string merkezgirisID = "";
        string merkezkasiyercikisID = "";
        string merkezkasiyergirisID = "";
        public static string CHSOCODE = "";
        public static string DSAFEID = "";
        public static string DSAFEVAL = "";
        public static string DSAFENAME = "";

        private void btnKasaMerkezGelenDuzelt_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = viewKasaMerkezDuzelt.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            KasiyerSec2 sec2 = new KasiyerSec2();
            sec2.ShowDialog();
            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                magazacikisID = "";
                magazagirisID = "";
                kasiyercikisID = "";
                kasiyergirisID = "";
                magazacikisID = "";
                merkezgirisID = "";
                merkezkasiyercikisID = "";
                merkezkasiyergirisID = "";



                var durum = viewKasaMerkezDuzelt.GetRowCellValue(selectedRows[i], "Durum").ToString();
                if (durum == "")
                {
                    try
                    {
                        var id = viewKasaMerkezDuzelt.GetRowCellValue(selectedRows[i], "SFCOCEDSID").ToString();
                        var SABHDATE = viewKasaMerkezDuzelt.GetRowCellValue(selectedRows[i], "SFCOCEDSDATE").ToString();
                        var DIVVAL = viewKasaMerkezDuzelt.GetRowCellValue(selectedRows[i], "DIVVAL").ToString();
                        var SABHAMOUNT = viewKasaMerkezDuzelt.GetRowCellValue(selectedRows[i], "SFCOCEDSAMOUNT1").ToString();

                        #region eski
                        //KasiyerSec sec = new KasiyerSec(id, SABHDATE, DIVVAL, SABHAMOUNT);
                        //sec.ShowDialog();
                        #endregion

                        #region yeni

                        var Islem = conn.GetData($"select * from SAFEMOVIE where SFCOCEDSID = {id}",MDE);
                        if (Islem == null)
                        {
                            try
                            {
                                string qq = String.Format(@"insert into SAFEMOVIE(SFCOCEDSID,SOCODE,DIVVAL,SABHDATE,SABHAMOUNT) values ('{0}','{1}','{2}','{3}','{4}')", id, CHSOCODE, DIVVAL, Convert.ToDateTime(SABHDATE).ToString("yyyy-MM-dd"), SABHAMOUNT);
                                var kayit = Logins(qq);

                                Dictionary<string, string> SAFE = new Dictionary<string, string>();
                                SAFE.Add("@id", id);
                                SAFE.Add("@SAFEID", "2155");
                                SAFE.Add("@SABHID", "");
                                SAFE.Add("@SABHDIVISON", "");
                                SAFE.Add("@SABHSOURCE", "VI1");
                                SAFE.Add("@SABHSOCODE", "");
                                SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Düzeltme");
                                SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                SAFE.Add("@ReturnDesc", "");
                                magazacikisID = conn.InsertBack("FK_KASAILK", SAFE);
                                //Form1.magazacikisID = magazacikisID;
                                string u1 = String.Format($"update SAFEMOVIE set VI1 = '{magazacikisID}' where SFCOCEDSID = '{id}'");
                                var sonuc1 = Logins(u1);

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", magazacikisID);
                                SAFECH.Add("@SAFEID", "2155");
                                SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                                string u2 = String.Format($"update SAFEMOVIE set VI1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                var sonuc2 = Logins(u2);


                                Dictionary<string, string> SAFE2 = new Dictionary<string, string>();
                                SAFE2.Add("@id", id);
                                SAFE2.Add("@SAFEID", "2155");
                                SAFE2.Add("@SABHID", magazacikisID);
                                SAFE2.Add("@SABHDIVISON", "00");
                                SAFE2.Add("@SABHSOURCE", "VI0");
                                SAFE2.Add("@SABHSOCODE", CHSOCODE);
                                SAFE2.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza Merkez Kasa Giriş");
                                SAFE2.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                SAFE2.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                SAFE2.Add("@ReturnDesc", "");
                                magazagirisID = conn.InsertBack("FK_KASAILK", SAFE2);
                                string u3 = String.Format($"update SAFEMOVIE set VI0 = '{magazagirisID}' where SFCOCEDSID = '{id}'");
                                var sonuc3 = Logins(u3);

                                Dictionary<string, string> SAFECH2 = new Dictionary<string, string>();
                                SAFECH2.Add("@SABHID", magazagirisID);
                                SAFECH2.Add("@SAFEID", "2155");
                                SAFECH2.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza Merkeze Kasa Giriş");
                                SAFECH2.Add("@ReturnDesc", "");
                                var SABHCHID2 = conn.InsertBack("FK_KASAMERKEZAL", SAFECH2);
                                string u4 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID2}' where SFCOCEDSID = '{id}'");
                                var sonuc4 = Logins(u4);



                                Dictionary<string, string> SAFE3 = new Dictionary<string, string>();
                                SAFE3.Add("@id", id);
                                SAFE3.Add("@SAFEID", "2155");
                                SAFE3.Add("@SABHID", "");
                                SAFE3.Add("@SABHDIVISON", "00");
                                SAFE3.Add("@SABHSOURCE", "VK1");
                                SAFE3.Add("@SABHSOCODE", CHSOCODE);
                                SAFE3.Add("@SABHDEEDNOTES", "Merkez Kasa " + CHSOCODE + " Kasiyer Çıkış");
                                SAFE3.Add("@SABHVERSEVAL", DSAFEVAL);
                                SAFE3.Add("@SABHVERSENAME", DSAFENAME);
                                SAFE3.Add("@ReturnDesc", "");
                                kasiyercikisID = conn.InsertBack("FK_KASAILK", SAFE3);
                                string u5 = String.Format($"update SAFEMOVIE set VK1 = '{kasiyercikisID}' where SFCOCEDSID = '{id}'");
                                var sonuc5 = Logins(u5);

                                Dictionary<string, string> SAFECH3 = new Dictionary<string, string>();
                                SAFECH3.Add("@SABHID", kasiyercikisID);
                                SAFECH3.Add("@SAFEID", DSAFEID);
                                SAFECH3.Add("@SABHDEEDNOTES", "Merkez Kasa " + CHSOCODE + " Kasiyer Çıkış");
                                SAFECH3.Add("@ReturnDesc", "");
                                var SABHCHID3 = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH3);
                                string u6 = String.Format($"update SAFEMOVIE set VK1CH = '{SABHCHID3}' where SFCOCEDSID = '{id}'");
                                var sonuc6 = Logins(u6);


                                Dictionary<string, string> SAFE4 = new Dictionary<string, string>();
                                SAFE4.Add("@id", id);
                                SAFE4.Add("@SAFEID", DSAFEID);
                                SAFE4.Add("@SABHID", kasiyercikisID);
                                SAFE4.Add("@SABHDIVISON", "00");
                                SAFE4.Add("@SABHSOURCE", "VK0");
                                SAFE4.Add("@SABHSOCODE", CHSOCODE);
                                SAFE4.Add("@SABHDEEDNOTES", DIVVAL + " MAağaza " + SABHDATE + " Tarihli Kasası");
                                SAFE4.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                SAFE4.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                SAFE4.Add("@ReturnDesc", "");
                                kasiyergirisID = conn.InsertBack("FK_KASAILK", SAFE4);
                                string u7 = String.Format($"update SAFEMOVIE set VK0 = '{kasiyergirisID}' where SFCOCEDSID = '{id}'");
                                var sonuc7 = Logins(u7);

                                Dictionary<string, string> SAFECH4 = new Dictionary<string, string>();
                                SAFECH4.Add("@SABHID", kasiyergirisID);
                                SAFECH4.Add("@SAFEID", "2155");
                                SAFECH4.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                                SAFECH4.Add("@ReturnDesc", "");
                                var SABHCHID4 = conn.InsertBack("FK_KASAMERKEZAL", SAFECH4);
                                string u8 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID4}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                                var sonuc8 = Logins(u8);

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        else
                        {

                            if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI1"].ToString()))
                            {
                                try
                                {
                                    Dictionary<string, string> SAFE = new Dictionary<string, string>();
                                    SAFE.Add("@id", id);
                                    SAFE.Add("@SAFEID", "2155");
                                    SAFE.Add("@SABHID", "");
                                    SAFE.Add("@SABHDIVISON", "");
                                    SAFE.Add("@SABHSOURCE", "VI1");
                                    SAFE.Add("@SABHSOCODE", "");
                                    SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Düzeltme");
                                    SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                    SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                    SAFE.Add("@ReturnDesc", "");
                                    magazacikisID = conn.InsertBack("FK_KASAILK", SAFE);
                                    //Form1.magazacikisID = magazacikisID;
                                    string u1 = String.Format($"update SAFEMOVIE set VI1 = '{magazacikisID}' where SFCOCEDSID = '{id}'");
                                    var sonuc1 = Logins(u1);

                                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                    SAFECH.Add("@SABHID", magazacikisID);
                                    SAFECH.Add("@SAFEID", "2155");
                                    SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                                    SAFECH.Add("@ReturnDesc", "");
                                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);


                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                            else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI1CH"].ToString()))
                            {
                                try
                                {
                                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                    SAFECH.Add("@SABHID", Islem.Rows[0]["VI1"].ToString());
                                    SAFECH.Add("@SAFEID", "2155");
                                    SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                                    SAFECH.Add("@ReturnDesc", "");
                                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                                    string u1 = String.Format($"update SAFEMOVIE set VI1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                    var sonuc1 = Logins(u1);
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }
                            if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI0"].ToString()))
                            {
                                Dictionary<string, string> SAFE = new Dictionary<string, string>();
                                SAFE.Add("@id", id);
                                SAFE.Add("@SAFEID", "2155");
                                SAFE.Add("@SABHID", Islem.Rows[0]["VI1"].ToString());
                                SAFE.Add("@SABHDIVISON", "00");
                                SAFE.Add("@SABHSOURCE", "VI0");
                                SAFE.Add("@SABHSOCODE", CHSOCODE);
                                SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Giriş");
                                SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                SAFE.Add("@ReturnDesc", "");
                                magazagirisID = conn.InsertBack("FK_KASAILK", SAFE);
                                string u1 = String.Format($"update SAFEMOVIE set VI0 = '{magazagirisID}' where SFCOCEDSID = '{id}'");
                                var sonuc1 = Logins(u1);

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", magazagirisID);
                                SAFECH.Add("@SAFEID", "2155");
                                SAFECH.Add("@SABHDEEDNOTES", "Merkeze Kasa Giriş");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                                string u2 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                var sonuc2 = Logins(u2);
                            }
                            else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI0"].ToString()))
                            {

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", magazagirisID);
                                SAFECH.Add("@SAFEID", "2155");
                                SAFECH.Add("@SABHDEEDNOTES", "Merkeze Kasa Giriş");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                                string u2 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                var sonuc2 = Logins(u2);
                            }
                            if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK1"].ToString()))
                            {
                                Dictionary<string, string> SAFE = new Dictionary<string, string>();
                                SAFE.Add("@id", id);
                                SAFE.Add("@SAFEID", "2155");
                                SAFE.Add("@SABHID", "");
                                SAFE.Add("@SABHDIVISON", "00");
                                SAFE.Add("@SABHSOURCE", "VK1");
                                SAFE.Add("@SABHSOCODE", CHSOCODE);
                                SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                                SAFE.Add("@SABHVERSEVAL", DSAFEVAL);
                                SAFE.Add("@SABHVERSENAME", DSAFENAME);
                                SAFE.Add("@ReturnDesc", "");
                                kasiyercikisID = conn.InsertBack("FK_KASAILK", SAFE);
                                string u3 = String.Format($"update SAFEMOVIE set VK1 = '{kasiyercikisID}' where SFCOCEDSID = '{id}'");
                                var sonuc3 = Logins(u3);

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", kasiyercikisID);
                                SAFECH.Add("@SAFEID", DSAFEID);
                                SAFECH.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                                string u4 = String.Format($"update SAFEMOVIE set VK1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                var sonuc4 = Logins(u4);

                            }
                            else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK0CH"].ToString()))
                            {
                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", Islem.Rows[0]["VK0"].ToString());
                                SAFECH.Add("@SAFEID", DSAFEID);
                                SAFECH.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                                string u4 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                                var sonuc4 = Logins(u4);
                            }
                            if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK0"].ToString()))
                            {
                                Dictionary<string, string> SAFE = new Dictionary<string, string>();
                                SAFE.Add("@id", id);
                                SAFE.Add("@SAFEID", DSAFEID);
                                SAFE.Add("@SABHID", kasiyercikisID);
                                SAFE.Add("@SABHDIVISON", "00");
                                SAFE.Add("@SABHSOURCE", "VK0");
                                SAFE.Add("@SABHSOCODE", CHSOCODE);
                                SAFE.Add("@SABHDEEDNOTES", DIVVAL + " MAağaza " + SABHDATE + " Tarihli Kasası");
                                SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                                SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                                SAFE.Add("@ReturnDesc", "");
                                kasiyergirisID = conn.InsertBack("FK_KASAILK", SAFE);
                                string u1 = String.Format($"update SAFEMOVIE set VK0 = '{kasiyergirisID}' where SFCOCEDSID = '{id}'");
                                var sonuc1 = Logins(u1);

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", kasiyergirisID);
                                SAFECH.Add("@SAFEID", "2155");
                                SAFECH.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                                string u2 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                                var sonuc2 = Logins(u2);
                            }
                            else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK1CH"].ToString()))
                            {

                                Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                                SAFECH.Add("@SABHID", Islem.Rows[0]["VK0"].ToString());
                                SAFECH.Add("@SAFEID", "2155");
                                SAFECH.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                                SAFECH.Add("@ReturnDesc", "");
                                var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                                string u2 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                                var sonuc2 = Logins(u2);
                            }
                        }
                        #endregion


                        viewKasaMerkezDuzelt.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                        viewKasaMerkezDuzelt.SetRowCellValue(selectedRows[i], "Durum", magazacikisID);
                        viewKasaMerkezDuzelt.RefreshRow(selectedRows[i]);

                        progressForm.PerformStep(this);
                        success++;
                    }
                    catch (Exception ex)
                    {
                        progressForm.PerformStep(this);
                        XtraMessageBox.Show(ex.Message);
                        error++;
                    }
                }
                else
                {
                    viewKasaMerkezDuzelt.SetRowCellValue(selectedRows[i], "Sonuc", "Tekrar İşlem Yapılamaz");
                    viewKasaMerkezDuzelt.RefreshRow(selectedRows[i]);
                    error += 1;
                }
            }
            this.Enabled = true;
            progressForm.Hide(this);
            XtraMessageBox.Show("İşlem tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "\r\n" + magazacikisID + "\r\n " + merkezgirisID + "\r\n " + merkezkasiyercikisID + "\r\n ");
        }
        internal int Logins(string query)
        {
            SqlCommand cmd = new SqlCommand(query, MDE);
            if (MDE.State == ConnectionState.Closed)
                MDE.Open();
            var sonuc = cmd.ExecuteNonQuery();
            return sonuc;
        }
        private void viewKasaMerkezDuzelt_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (viewKasaMerkezDuzelt.DataRowCount > 0)
            {
                if (viewKasaMerkezDuzelt.GetRowCellValue(e.RowHandle, "Durum") != null)
                {
                    var renkKodu = viewKasaMerkezDuzelt.GetRowCellValue(e.RowHandle, "Durum").ToString();
                    var renkKodu2 = viewKasaMerkezDuzelt.GetRowCellValue(e.RowHandle, "Sonuc").ToString();
                    if (renkKodu == "İşlem olmuş")
                    {
                        if (renkKodu2 == "")
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff2800");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#ff2800");
                            e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                        }
                        else if (renkKodu2 == "Başarılı")
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#6e8b3d");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#6e8b3d");
                            e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                        }
                        else
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff2800");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#ff2800");
                            e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                        }
                    }
                    else
                    {
                        if (renkKodu2 == "")
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fff");
                        }
                        else if (renkKodu2 == "Başarılı")
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#6e8b3d");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#6e8b3d");
                            e.Appearance.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                        }
                        else
                        {
                            e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#d62c27");
                            e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#d62c27");
                        }
                    }
                }
            }
        }

        private void ViewKasaTutar_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle > 0 && e.Clicks >= 2 && e.Button == MouseButtons.Left)
            {
                var tutar = ViewKasaTutar.GetRowCellValue(e.RowHandle, "SFCOCEDSAMOUNT1").ToString();
                var tarih = ViewKasaTutar.GetRowCellValue(e.RowHandle, "SFCOCEDSDATE").ToString();
                txtKasaTutarYeniDeger.Text = tutar;
                dteKasaTutarNewTarih.EditValue = Convert.ToDateTime(tarih).ToString("yyyy-MM-dd");
            }
        }

        private void ViewKasaTutar_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle > 0 && e.Clicks >= 2 && e.Button == MouseButtons.Left)
            {
                var tutar = ViewKasaTutar.GetRowCellValue(e.RowHandle, "SFCOCEDSAMOUNT1").ToString();
                var tarih = ViewKasaTutar.GetRowCellValue(e.RowHandle, "SFCOCEDSDATE").ToString();
                txtKasaTutarYeniDeger.Text = tutar;
                dteKasaTutarNewTarih.EditValue = Convert.ToDateTime(tarih).ToString("yyyy-MM-dd");
            }
        }

        private void btnBankFark_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select SABHID,SFCOCEDSID,DIVVAL as [Mağaza kodu],DIVNAME as [Mağaza Adı],SABHDATE as [İşlenen Kasa Tarihi],SABHAMOUNT as [Kasa Toplam Tutarı],SFCOCEDSDATE as [Girilmesi Gereken Tarih], '' Sonuc 
            from SAFEBEHAVE 
            inner join DIVISON on DIVVAL = SABHDIVISON
            left outer join SAFECONTROLCENTERDAYSUM on SFCOCEDSAMOUNT1 = SABHAMOUNT and SFCOCEDSDATE = SABHDATE and SFCOCEDSSABHCREATED = 1
            where SABHDATE between '{0}' and '{1}' and SABHSOURCE = 'BN1'
            and SFCOCEDSID is NULL", Convert.ToDateTime(dteBankFarkEnd.EditValue).ToString("yyyy-MM-yy"), Convert.ToDateTime(dteBankFarkEnd.EditValue).ToString("yyyy-MM-dd"));
            if (cmbBankFark.EditValue != null)
            {
                q = q + string.Format(" and SABHDIVISON = '{0}'", cmbBankFark.EditValue);
            }
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridBankFark.DataSource = dt;
        }

        private void btnBankFarkSil_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = ViewBankFark.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;

            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var SABHID = ViewBankFark.GetRowCellValue(selectedRows[i], "SABHID").ToString();
                try
                {
                    string d1 = string.Format("DELETE BANKBEHAVE WHERE BABHSABHID = {0}", SABHID);
                    string d3 = string.Format("DELETE SAFEBEHAVE WHERE SABHID = {0}", SABHID);
                    SqlCommand cmd1 = new SqlCommand(d1, sql);
                    SqlCommand cmd3 = new SqlCommand(d3, sql);
                    sql.Open();
                    var sili1 = cmd1.ExecuteNonQuery();
                    var sili3 = cmd3.ExecuteNonQuery();
                    sql.Close();
                    ViewBankFark.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                    ViewBankFark.RefreshRow(selectedRows[i]);
                    success++;
                }
                catch (Exception ex)
                {
                    error++;
                    MessageBox.Show(ex.Message);
                }

                progressForm.PerformStep(this);
            }
            this.Enabled = true;
            progressForm.Hide(this);
            //gridBankFark.DataSource = null;
            //dteBankFarkStart.EditValue = DateTime.Now.AddDays(-1);
            //dteBankFarkEnd.EditValue = DateTime.Now.AddDays(-1);
            XtraMessageBox.Show("Silindi. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnGunToplam_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"select SFCOCEDSID,DIVVAL,DIVNAME,SFCOCEDSDATE,SFCOCEDSAMOUNT1,SFCOCEDSSABHCREATED,SFCOCEDSSENDCASH,SONAME + space(1) + SOSURNAME as Kasiyer,'' as Sonuc, '' as Durum
            from SAFECONTROLCENTERDAYSUM with (nolock) 
            left outer join DIVISON with (nolock) on DIVVAL= SFCOCEDSDIVISON 
            left outer join SOCIAL with (nolock) on SOCODE = SFCOCEDSSOCODE
            where SFCOCEDSDIVISON = '{0}' and SFCOCEDSDATE between '{1}' and '{2}' 
            order by DIVVAL, SFCOCEDSDATE", cmbGunToplam.EditValue, Convert.ToDateTime(dteGunToplamStart.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteGunToplamEnd.EditValue).ToString("yyyy-MM-dd"));
            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridGunToplam.DataSource = dt;
            groupYeni.Enabled = true;
        }

        private void ViewGunToplam_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            var divval = ViewGunToplam.GetRowCellValue(ViewGunToplam.FocusedRowHandle, "DIVVAL").ToString();
            var date = ViewGunToplam.GetRowCellValue(ViewGunToplam.FocusedRowHandle, "SFCOCEDSDATE").ToString();
            string qq = string.Format(@"select SABHID,SABHSOURCE,DIVVAL,DIVNAME,SABHDATE,SABHDEEDNOTES,SABHAMOUNT,SONAME + space(1) + SOSURNAME  as Kasiyer,'' as Sonuc, '' as Durum
            from SAFEBEHAVE with (nolock) 
            left outer join DIVISON with (nolock) on DIVVAL= SABHDIVISON 
            left outer join SOCIAL with (nolock) on SOCODE = SABHSOCODE
            where SABHDIVISON = '{0}' and SABHDATE = '{1}' and SABHSOURCE = 'BN1' 
            union
            select SABHID,SABHSOURCE,DIVVAL,DIVNAME,SABHDATE,SABHDEEDNOTES,SABHAMOUNT,SONAME + space(1) + SOSURNAME  as Kasiyer,'' as Sonuc, '' as Durum
            from SAFEBEHAVE with (nolock) 
            left outer join DIVISON with (nolock) on DIVVAL= SABHDIVISON 
            left outer join SOCIAL with (nolock) on SOCODE = SABHSOCODE
            where SABHDIVISON = '{0}' and SABHDATE = '{1}' and (SABHSOURCE like 'VI%' and SABHVERSEVAL = '00.Kasa.TL')", divval, Convert.ToDateTime(date).ToString("yyyy-MM-dd"));
            SqlDataAdapter da = new SqlDataAdapter(qq, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridGunDetay.DataSource = dt;

        }

        private void seçileniSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SFCOCEDSID = ViewGunToplam.GetRowCellValue(ViewGunToplam.FocusedRowHandle, "SFCOCEDSID").ToString();
            string q = string.Format(@"delete SAFECONTROLCENTERDAYSUM  where SFCOCEDSID = '{0}'", SFCOCEDSID);
            SqlCommand cmd = new SqlCommand(q, sql);
            if (sql.State == ConnectionState.Closed)
            {
                sql.Open();
            }
            cmd.ExecuteNonQuery();
            sql.Close();
            ViewGunToplam.SetRowCellValue(ViewGunToplam.FocusedRowHandle, "Durum", "silindi");
            ViewGunToplam.RefreshRow(ViewGunToplam.FocusedRowHandle);
        }

        private void durumDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SFCOCEDSID = ViewGunToplam.GetRowCellValue(ViewGunToplam.FocusedRowHandle, "SFCOCEDSID").ToString();
            frmGunToplamDegistir degistir = new frmGunToplamDegistir(SFCOCEDSID);
            DialogResult result = degistir.ShowDialog();
            if (result == DialogResult.OK)
            {
                ViewGunToplam.SetRowCellValue(ViewGunToplam.FocusedRowHandle, "Durum", "Bankaya Gönderildi Yapıldı");
                ViewGunToplam.RefreshRow(ViewGunToplam.FocusedRowHandle);
            }
            else if (result == DialogResult.Cancel)
            {
                ViewGunToplam.SetRowCellValue(ViewGunToplam.FocusedRowHandle, "Durum", "Merkeze Gönderildi Yapıldı");
                ViewGunToplam.RefreshRow(ViewGunToplam.FocusedRowHandle);
            }
            else if (result == DialogResult.Abort)
            {
                ViewGunToplam.SetRowCellValue(ViewGunToplam.FocusedRowHandle, "Durum", "Bankaya Gönderildi Kaldırıldı");
                ViewGunToplam.RefreshRow(ViewGunToplam.FocusedRowHandle);
            }
            else if (result == DialogResult.Retry)
            {
                ViewGunToplam.SetRowCellValue(ViewGunToplam.FocusedRowHandle, "Durum", "Merkeze Gönderildi Kaldırıldı");
                ViewGunToplam.RefreshRow(ViewGunToplam.FocusedRowHandle);
            }
        }

        private void bankaGönderildiİşleminiYapToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var selectedRows = ViewGunToplam.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var durum = ViewGunToplam.GetRowCellValue(selectedRows[i], "Durum").ToString();
                if (durum == "")
                {
                    try
                    {
                        var id = ViewGunToplam.GetRowCellValue(selectedRows[i], "SFCOCEDSID").ToString();

                        Dictionary<string, string> keys = new Dictionary<string, string>();
                        keys.Add("@id", id);
                        keys.Add("@ReturnDesc", "");
                        var dt = conn.InsertBack("FK_KASABANKAGONDERKASADUS", keys);
                        ViewGunToplam.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                        ViewGunToplam.SetRowCellValue(selectedRows[i], "Durum", dt);
                        ViewGunToplam.RefreshRow(selectedRows[i]);
                        progressForm.PerformStep(this);
                        success++;
                    }
                    catch (Exception ex)
                    {
                        progressForm.PerformStep(this);
                        XtraMessageBox.Show(ex.Message);
                        error++;
                    }
                }
                else
                {
                    ViewGunToplam.SetRowCellValue(selectedRows[i], "Sonuc", "Tekrar İşlem Yapılamaz");
                    ViewGunToplam.RefreshRow(selectedRows[i]);
                    error += 1;
                }
            }

            this.Enabled = true;
            progressForm.Hide(this);
            XtraMessageBox.Show("İşlem tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnGunToplamYeni_Click(object sender, EventArgs e)
        {
            string q = string.Format(@"insert into SAFECONTROLCENTERDAYSUM values ((select max(SFCOCEDSID)+1 from SAFECONTROLCENTERDAYSUM ),'{0}','{1}','{2}',0,'00KS001','{0}',0,0)", Convert.ToDateTime(dteGunToplamYeni.EditValue).ToString("yyyy-MM-dd"), cmbGunToplamYeni.EditValue, txtGunToplamYeni.EditValue);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            txtGunToplamYeni.EditValue = "";
            dteGunToplamYeni.EditValue = DateTime.Now.AddDays(-1);
            groupYeni.Enabled = false;
            btnGunToplam_Click(null, null);
        }

        private void merkezeGönderildiİşleminiYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRows = ViewGunToplam.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var durum = ViewGunToplam.GetRowCellValue(selectedRows[i], "Durum").ToString();
                if (durum == "")
                {
                    try
                    {
                        var id = ViewGunToplam.GetRowCellValue(selectedRows[i], "SFCOCEDSID").ToString();

                        Dictionary<string, string> keys = new Dictionary<string, string>();
                        keys.Add("@id", id);
                        keys.Add("@ReturnDesc", "");
                        var dt = conn.InsertBack("FK_KASAMERKEZGONDERKASADUS", keys);
                        ViewGunToplam.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                        ViewGunToplam.SetRowCellValue(selectedRows[i], "Durum", dt);
                        ViewGunToplam.RefreshRow(selectedRows[i]);
                        progressForm.PerformStep(this);
                        success++;
                    }
                    catch (Exception ex)
                    {
                        progressForm.PerformStep(this);
                        XtraMessageBox.Show(ex.Message);
                        error++;
                    }
                }
                else
                {
                    ViewGunToplam.SetRowCellValue(selectedRows[i], "Sonuc", "Tekrar İşlem Yapılamaz");
                    ViewGunToplam.RefreshRow(selectedRows[i]);
                    error += 1;
                }
            }

            this.Enabled = true;
            progressForm.Hide(this);
            XtraMessageBox.Show("İşlem tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void seçileniSilToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string SABHID = ViewGunDetay.GetRowCellValue(ViewGunDetay.FocusedRowHandle, "SABHID").ToString();
                string d1 = string.Format("DELETE BANKBEHAVE WHERE BABHSABHID = {0}", SABHID);
                string d3 = string.Format("DELETE SAFEBEHAVE WHERE SABHID = {0}", SABHID);
                SqlCommand cmd1 = new SqlCommand(d1, sql);
                SqlCommand cmd3 = new SqlCommand(d3, sql);
                sql.Open();
                var sili1 = cmd1.ExecuteNonQuery();
                var sili3 = cmd3.ExecuteNonQuery();
                sql.Close();
                ViewGunDetay.SetRowCellValue(ViewGunDetay.FocusedRowHandle, "Durum", "silindi");
                ViewGunDetay.RefreshRow(ViewGunDetay.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void tileBarItem9_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = viewKasaSil.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;

            progressForm.Show(this);
            for (int i = 0; i < selectedRows.Length; i++)
            {
                var SABHID = viewKasaSil.GetRowCellValue(selectedRows[i], "SABHID").ToString();
                var SFCOCEDSID = viewKasaSil.GetRowCellValue(selectedRows[i], "SFCOCEDSID").ToString();
                try
                {
                    string d1 = string.Format("DELETE BANKBEHAVE WHERE BABHSABHID = {0}", SABHID);
                    string d2 = string.Format("DELETE SAFECONTROLCENTERDAYSUM where SFCOCEDSID = {0}", SFCOCEDSID);
                    string d3 = string.Format("DELETE SAFEBEHAVE WHERE SABHID = {0}", SABHID);
                    SqlCommand cmd1 = new SqlCommand(d1, sql);
                    SqlCommand cmd2 = new SqlCommand(d2, sql);
                    SqlCommand cmd3 = new SqlCommand(d3, sql);
                    sql.Open();
                    var sili1 = cmd1.ExecuteNonQuery();
                    var sili2 = cmd2.ExecuteNonQuery();
                    var sili3 = cmd3.ExecuteNonQuery();
                    sql.Close();
                    viewKasaSil.SetRowCellValue(selectedRows[i], "Sonuc", "Başarılı");
                    viewKasaSil.RefreshRow(selectedRows[i]);
                    success++;
                }
                catch (Exception ex)
                {
                    error++;
                    MessageBox.Show(ex.Message);
                }

                progressForm.PerformStep(this);
            }
            this.Enabled = true;
            progressForm.Hide(this);
            gridKasaSil.DataSource = null;
            dteKasaSilStart.EditValue = DateTime.Now.AddDays(-1);
            dteKasaSilEnd.EditValue = DateTime.Now.AddDays(-1);
            XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void cmbMagaza_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbMagaza.EditValue != null)
            {
                //if (cmbMagaza.EditValue.ToString() == "1")
                //{
                //    MGZLIST = "in ('43', '10', '12', '23', '44', '17', '18', '51', '14', '19','27','07','22','24','41','20')";
                //}
                //else if (cmbMagaza.EditValue.ToString() == "2")
                //{
                //    MGZLIST = "in ('26','32','37','06','13','47','29','38','46','31','45','09','36','05','21','02','01','49','04')";
                //}
                //else if (cmbMagaza.EditValue.ToString() == "3")
                //{
                //    MGZLIST = "in ('42','03','35','30','39','11','50','25','33','16','08','40','53','28','52','15','34')";
                //}

            }
            else
            {
                MGZLIST = "not in ('00')";
            }
        }

        private void cmbMagaza2_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbMagaza2.EditValue != null)
            {
                //if (cmbMagaza2.EditValue.ToString() == "1")
                //{
                //    MGZLIST = "in ('43', '10', '12', '23', '44', '17', '18', '51', '14', '19','27','07','22','24','41','20')";
                //}
                //else if (cmbMagaza2.EditValue.ToString() == "2")
                //{
                //    MGZLIST = "in ('26','32','37','06','13','47','29','38','46','31','45','09','36','05','21','02','01','49','04')";
                //}
                //else if (cmbMagaza2.EditValue.ToString() == "3")
                //{
                //    MGZLIST = "in ('42','03','35','30','39','11','50','25','33','16','08','40','53','28','52','15','34')";
                //}
            }
            else
            {
                MGZLIST = "not in ('00')";
            }

        }

        private void müşteriDeğiştriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var PCDSID = ViewOdemeListesi.GetRowCellValue(ViewOdemeListesi.FocusedRowHandle, "PCDSID").ToString();
            var CURID = ViewOdemeListesi.GetRowCellValue(ViewOdemeListesi.FocusedRowHandle, "PCDSCURID").ToString();
            frmOdemeDegistir odemeDegistir = new frmOdemeDegistir(PCDSID, CURID);
            odemeDegistir.ShowDialog();
            btnOdemeYenile_Click(null, null);
        }

        private void btnOdemeListele_Click(object sender, EventArgs e)
        {
            string q = String.Format(@"select PCDSID,PCDSCURID,DIVVAL,DIVNAME,PCDSDATE,'Taksit Ödemesi',PCDSAMOUNT,PCDSLATEINCOME,PCDSEARLYPAYDISC,PCDSDATETIME,isnull(SONAME + space(1) +SOSURNAME,CHVAL) as Kasiyer from PROCEEDS
            left outer join CURRENTS on CURID = PCDSCURID
            left outer join CASHIER on CHVAL = PCDSCASHIER
            left outer join SOCIAL on SOCODE = PCDSSOCODE
            left outer join DIVISON on DIVVAL = PCDSDIVISON
            where CURVAL = '{0}'
            and PCDSSALID is NULL
            order by PCDSDATE", txtOdemeMusteri.Text);
            gridOdemeListesi.DataSource = conn.GetData(q, sql);
            txtOdemeMusteri.Enabled = false;
            btnOdemeListele.Enabled = false;
        }

        private void btnOdemeYenile_Click(object sender, EventArgs e)
        {
            gridOdemeListesi.DataSource = null;
            txtOdemeMusteri.Text = null;
            txtOdemeMusteri.Enabled = true;
            btnOdemeListele.Enabled = true;
        }

        private void tileBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (frmLogin.userID == "00KS001")
            {
                frmYetkilendirme yetkilendirme = new frmYetkilendirme();
                yetkilendirme.ShowDialog();
            }
        }

        private void xtraTabControl2_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page.Name == "xtraTabPage8")
            {
                string q = "select DPYMID,DPYMNAME from DEFPAYMENTKIND where DPYMVAL = 'GSP'";
                srcWebOdemeOdemeTipi.Properties.DataSource = conn.GetData(q, sql);
                srcWebOdemeOdemeTipi.Properties.DisplayMember = "DPYMNAME";
                srcWebOdemeOdemeTipi.Properties.ValueMember = "DPYMID";

                string w = @"select CHVAL,SONAME+' '+SOSURNAME as CHNAME from CASHIER 
                left outer join SOCIAL on SOCODE = CHSOCODE
                where CHDIVISON = '00' and SOSTS = 1";
                srcWebOdemeKasiyer.Properties.DataSource = conn.GetData(w, sql);
                srcWebOdemeKasiyer.Properties.DisplayMember = "CHNAME";
                srcWebOdemeKasiyer.Properties.ValueMember = "CHVAL";

                dteWebOdemeBasTarih.EditValue = DateTime.Now.AddDays(-1);
                dteWebOdemeBitTarih.EditValue = DateTime.Now;
            }

        }

        private void btnWebOdemeListele_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (srcWebOdemeOdemeTipi.EditValue.ToString() != "Seçiniz....!")
            {
                if (srcWebOdemeKasiyer.EditValue.ToString() != "Seçiniz....!")
                {
                    ProgressBarFrm progressForm = new ProgressBarFrm()
                    {
                        Start = 0,
                        Finish = 1,
                        Position = 0,
                        ToplamAdet = 1.ToString(),
                    };
                    int success = 0;
                    int error = 0;
                    this.Enabled = false;
                    executeBackground(
               () =>
               {
                   progressForm.Show(this);
                   try
                    {
                        string q = String.Format(@"select CURID,PCDSUSEFIELDS1,CURVAL,CURNAME,PCDSDATE,count(*) as Adet,max(PCDSLATEINCOME) as vade,max(PCDSEARLYPAYDISC) as erken,max(PCDSAMOUNT) as OdemeTutari,sum(PCDSCHAMOUNT) as ToplamDsusum from PROCEEDS with (nolock)
                       left outer join PROCEEDSCHILD with (nolock) on PCDSCHPCDSID = PCDSID
                       left outer join CURRENTS with (nolock) on CURID = PCDSCURID
                       where PCDSDATE between '{0}' and '{1}'					   
                       ", Convert.ToDateTime(dteWebOdemeBasTarih.EditValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(dteWebOdemeBitTarih.EditValue).ToString("yyyy-MM-dd"));
                        if (txtWebOdemeMusteriNo.Text != "")
                        {
                            q = q + String.Format(@" and CURVAL = '{0}'", txtWebOdemeMusteriNo.Text);
                        }
                        if (srcWebOdemeOdemeTipi.EditValue.ToString() != "Seçiniz....!")
                        {
                            q = q + String.Format(@" and PCDSCHDPYMID = {0}", srcWebOdemeOdemeTipi.EditValue);
                        }
                        if (srcWebOdemeKasiyer.EditValue.ToString() != "Seçiniz....!")
                        {
                            q = q + String.Format(@" and PCDSCASHIER = '{0}'", srcWebOdemeKasiyer.EditValue);
                        }
                        q = q + @"
                       group by CURID,PCDSUSEFIELDS1,PCDSDATE,CURVAL,CURNAME
                       having COUNT(*) > 1
                       order by PCDSDATE ,adet desc,CURID";
                       dt = conn.GetData(q, sql);                       
                        success++;
                        progressForm.PerformStep(this);
                    }
                    catch (Exception exx)
                    {
                        error++;
                        progressForm.PerformStep(this);
                    }

               },
                     null,
                     () =>
                     {
                         gridWebOdeme.DataSource = dt;
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
                }
                else
                {
                    XtraMessageBox.Show("Kasiyer Seçilmedi");
                }
            }
            else
            {
                XtraMessageBox.Show("Ödeme Tipi Seçilmedi");
            }
        }
        ListtoDataTableConverter Converter = new ListtoDataTableConverter();
        private int desiredRowCount;

        private void btnWebOdemeDuzelt_ItemClick(object sender, TileItemEventArgs e)
        {
            int islemadet = 0;
            for (int i = 0; i < ViewWebOdeme.RowCount; i++)
            {
                islemadet = islemadet + int.Parse(ViewWebOdeme.GetRowCellValue(i, "Adet").ToString());
            }
            islemadet = islemadet - ViewWebOdeme.RowCount;
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = islemadet,
                Position = 0,
                ToplamAdet = islemadet.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            int silinenadet = 0;
            executeBackground(
       () =>
       {
           progressForm.Show(this);
           for (int i = 0; i < ViewWebOdeme.RowCount; i++)
           {
               List<Odeme> PCDSIDList = new List<Odeme>();
               try
               {
                   var minidcs = new Odeme();
                   var vadecs = new Odeme();
                   var erkencs = new Odeme();
                   var idsorgu = String.Format(@"select PCDSID from PROCEEDS with (nolock)
                    left outer join PROCEEDSCHILD with (nolock) on PCDSCHPCDSID = PCDSID
                    left outer join CURRENTS with (nolock) on CURID = PCDSCURID
                    where PCDSCHDPYMID = '83' --and PCDSDATE >= @tarih
                    and CURID = {0}
                    and PCDSUSEFIELDS1 = '{1}'
                    order by PCDSID desc", ViewWebOdeme.GetRowCellValue(i, "CURID").ToString(), ViewWebOdeme.GetRowCellValue(i, "PCDSUSEFIELDS1").ToString());
                   var dt = conn.GetData(idsorgu, sql).DataTableToList<Odeme>();

                   string minid = conn.GetValue(String.Format(@"select min(PCDSID) from PROCEEDS with (nolock) where PCDSCURID = {0} and PCDSUSEFIELDS1 = '{1}'",
                       ViewWebOdeme.GetRowCellValue(i, "CURID").ToString(), ViewWebOdeme.GetRowCellValue(i, "PCDSUSEFIELDS1").ToString()));

                   string vade = conn.GetValue(String.Format(@"select min(PCDSID) from PROCEEDS with (nolock) where PCDSCURID = {0} and PCDSUSEFIELDS1 = '{1}' and PCDSLATEINCOME = '{2}'", ViewWebOdeme.GetRowCellValue(i, "CURID").ToString(), ViewWebOdeme.GetRowCellValue(i, "PCDSUSEFIELDS1").ToString(), ViewWebOdeme.GetRowCellValue(i, "vade").ToString()));

                   string erken = conn.GetValue(String.Format(@"select min(PCDSID) from PROCEEDS with (nolock) where PCDSCURID = {0} and PCDSUSEFIELDS1 = '{1}' and PCDSEARLYPAYDISC = {2}", ViewWebOdeme.GetRowCellValue(i, "CURID").ToString(), ViewWebOdeme.GetRowCellValue(i, "PCDSUSEFIELDS1").ToString(), ViewWebOdeme.GetRowCellValue(i, "erken").ToString()));

                   minidcs.PCDSID = minid;
                   vadecs.PCDSID = vade;
                   erkencs.PCDSID = erken;

                   if (!dt.Any(f => f.ToString() != vadecs.PCDSID))
                   {
                       if (!dt.Any(f => f.ToString() != erkencs.PCDSID))
                       {
                           if (!dt.Any(f => f.ToString() == minidcs.PCDSID))
                           {
                               int index = dt.FindIndex(a => a.PCDSID == minidcs.PCDSID);
                               dt.RemoveAt(index);
                           }
                           else
                           {
                               XtraMessageBox.Show("Min id bulunmadı");
                           }

                       }
                       else
                       {
                           if (!dt.Any(f => f.ToString() == erkencs.PCDSID))
                           {
                               int index = dt.FindIndex(a => a.PCDSID == erkencs.PCDSID);
                               dt.RemoveAt(index);
                           }
                           else
                           {
                               XtraMessageBox.Show("Erken Ödeme id bulunmadı");
                           }

                       }

                   }
                   else
                   {

                       if (!dt.Any(f => f.ToString() == vadecs.PCDSID))
                       {
                           int index = dt.FindIndex(a => a.PCDSID == vadecs.PCDSID);
                           dt.RemoveAt(index);
                       }
                       else
                       {
                           XtraMessageBox.Show("Vadeli Ödeme id bulunmadı");
                       }
                   }

                   for (int ii = 0; ii < dt.Count; ii++)
                   {

                       var silinecekler = conn.GetData(String.Format(@"SELECT * FROM PROCEEDS WHERE PCDSID = {0}", dt[ii].PCDSID), sql);
                       string sbh = String.Format(@"DELETE SAFEBEHAVE WHERE SABHPCDSCHID IN(SELECT PCDSCHID FROM PROCEEDSCHILD with (nolock) WHERE PCDSCHPCDSID = {0})", dt[ii].PCDSID);
                       string bnh = String.Format(@"DELETE BANKBEHAVE WHERE BABHPCDSCHID IN(SELECT PCDSCHID FROM PROCEEDSCHILD with (nolock) WHERE PCDSCHPCDSID = {0})", dt[ii].PCDSID);
                       string pas = String.Format(@"DELETE PASSBEHAVE WHERE PABHPCDSCHID IN(SELECT PCDSCHID FROM PROCEEDSCHILD with (nolock) WHERE PCDSCHPCDSID = {0})", dt[ii].PCDSID);
                       string mir = String.Format(@"DELETE MIRPROCEEDS WHERE MRPCDSID = {0}", dt[ii].PCDSID);
                       string pcd = String.Format(@"DELETE PROCEEDS WHERE PCDSID = {0}", dt[ii].PCDSID);

                       var cbhsonuc = conn.InsertValue(sbh, sql);
                       var bnhsonuc = conn.InsertValue(bnh, sql);
                       var passonuc = conn.InsertValue(pas, sql);
                       var pcdsonuc = conn.InsertValue(pcd, sql);
                       if (pcdsonuc == 1)
                       {

                           silinenadet++;
                       }
                       else
                       {
                           string w1 = "update ACCFIREWALL set ACFWCLOSEANYACTION = DATEADD(YEAR,1,ACFWCLOSEANYACTION) where ACFWYEAR = DATENAME(YEAR,getdate())";
                           SqlCommand cmd3 = new SqlCommand(w1, sql);
                           cmd3.ExecuteNonQuery();
                           var Mirsonuc = conn.InsertValue(mir, sql);
                           var pcdsonuc2 = conn.InsertValue(pcd, sql);
                           if (pcdsonuc2 == 1)
                           {
                               string w2 = "update ACCFIREWALL set ACFWCLOSEANYACTION = DATEADD(YEAR,-1,ACFWCLOSEANYACTION) where ACFWYEAR = DATENAME(YEAR,getdate())";
                               SqlCommand cmd4 = new SqlCommand(w2, sql);
                               cmd4.ExecuteNonQuery();
                           }

                       }

                       #region historyBulkinsert
                       //DataTable HISTORYTable = new DataTable();
                       //HISTORYTable.Columns.Add("HISID", typeof(int));
                       //HISTORYTable.Columns.Add("HISDATETIME", typeof(DateTime));
                       //HISTORYTable.Columns.Add("HISSCODE", typeof(string));
                       //HISTORYTable.Columns.Add("HISKIND", typeof(string));
                       //HISTORYTable.Columns.Add("HISTITLE", typeof(string));
                       //HISTORYTable.Columns.Add("HISTITLEID", typeof(int));


                       //DataTable HISTORYCHTable = new DataTable();
                       //HISTORYCHTable.Columns.Add("HISCHHISID", typeof(int));
                       //HISTORYCHTable.Columns.Add("HISCHDETAIL", typeof(string));
                       //HISTORYCHTable.Columns.Add("HISBEFORE", typeof(string));
                       //HISTORYCHTable.Columns.Add("HISAFTER", typeof(string));


                       //var HISID = conn.GetValue(@"update REGISTER set RGID = RGID +1 where RGKIND = '118' select RGID from REGISTER where RGKIND = '118'");

                       //HISTORYTable.Rows.Add(HISID, DateTime.Now, frmLogin.userID, "D", "PROCEEDS", dt[ii].PCDSID);
                       //if (sql.State == ConnectionState.Closed)
                       //{
                       //    sql.Open();
                       //}
                       //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sql))
                       //{
                       //    bulkCopy.DestinationTableName = "HISTORY"; // Hedef tablo adını buraya yazın
                       //    bulkCopy.WriteToServer(HISTORYTable);
                       //}                        

                       //foreach (DataRow row in silinecekler.Rows)
                       //{
                       //    foreach (DataColumn col in silinecekler.Columns)
                       //    {
                       //        if (row[col] != null)
                       //        {
                       //            HISTORYCHTable.Rows.Add(HISID, col.ColumnName, row[col], "");
                       //        }
                       //        else
                       //        {

                       //        }
                       //    }
                       //}
                       //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sql))
                       //{
                       //    bulkCopy.DestinationTableName = "HISTORYCHILD"; // Hedef tablo adını buraya yazın
                       //    bulkCopy.WriteToServer(HISTORYCHTable);
                       //}
                       #endregion
                       progressForm.PerformStep(this);
                   }
                   success++;
                   conn.InsertValue($"update CUSTOMER set CUSCREDIT = 50000 CUSCURID = {ViewWebOdeme.GetRowCellValue(i, "CURID").ToString()}", sql); 
                   //CURID,PCDSUSEFIELDS1
               }
               catch (Exception exx)
               {
                   progressForm.PerformStep(this);
               }
           }
       },
                     null,
                     () =>
                     {
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "Toplam Silinen Satır : " + silinenadet, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
        }

        private void btnOdemeDakikaEkle_Click(object sender, EventArgs e)
        {
            var ID = txtOdemeID.Text;
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = 1,
                Position = 0,
                ToplamAdet = "1".ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            int silinenadet = 0;
            executeBackground(
       () =>
       {
           progressForm.Show(this);
           try
           {
               using (SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring))
               {
                   sql.Open();
                   SqlCommand cmd = sql.CreateCommand();
                   cmd.CommandType = CommandType.Text;
                   cmd.CommandText = String.Format(@"update PROCEEDS set PCDSDATETIME = DATEADD(MINUTE,1,PCDSDATETIME) where PCDSID = {0}", ID);
                   var sonuc = cmd.ExecuteNonQuery();
                   if (sonuc == 1)
                   {
                       success++;
                       this.Invoke((MethodInvoker)delegate
                       {
                           if (!string.IsNullOrEmpty(txtBakiyeMusteriNo.Text))
                           {
                               tileBarItem2_ItemClick(null, null);
                           }
                       });
                       progressForm.PerformStep(this);
                   }
                   else
                   {
                       progressForm.PerformStep(this);
                   }
               }
           }
           catch (Exception)
           {
               error++;
               progressForm.PerformStep(this);
           }

       },
                     null,
                     () =>
                     {
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "Toplam Silinen Satır : " + silinenadet, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
        }

        private void btnOdemeDakikaCikar_Click(object sender, EventArgs e)
        {
            var ID = txtOdemeID.Text;
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = 1,
                Position = 0,
                ToplamAdet = "1".ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            int silinenadet = 0;
            string CURVAL = txtBakiyeMusteriNo.Text;
            executeBackground(
       () =>
       {
           progressForm.Show(this);
           try
           {
               using (SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring))
               {
                   sql.Open();
                   SqlCommand cmd = sql.CreateCommand();
                   cmd.CommandType = CommandType.Text;
                   cmd.CommandText = String.Format(@"update PROCEEDS set PCDSDATETIME = DATEADD(MINUTE,-1,PCDSDATETIME) where PCDSID = {0}", ID);
                   var sonuc = cmd.ExecuteNonQuery();
                   if (sonuc == 1)
                   {
                       success++;
                       this.Invoke((MethodInvoker)delegate
                       {
                           if (!string.IsNullOrEmpty(txtBakiyeMusteriNo.Text))
                           {
                               Dictionary<string, string> keyValues = new Dictionary<string, string>();
                               keyValues.Add("@CURVAL", CURVAL);
                               conn.Insert("FK_INSTALMENTCANCELRECREATENEW", keyValues);
                               btnYeni_Click(null, null);
                           }
                       });
                       progressForm.PerformStep(this);
                   }
                   else
                   {
                       progressForm.PerformStep(this);
                   }
               }
           }
           catch (Exception)
           {
               error++;
               progressForm.PerformStep(this);
           }

       },
                     null,
                     () =>
                     {
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "Toplam Silinen Satır : " + silinenadet, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
        }

        private void btnOdemeTutarDegistir_Click(object sender, EventArgs e)
        {
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = 1,
                Position = 0,
                ToplamAdet = "1".ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            int silinenadet = 0;

            double PCDSAMOUNT = ParseToDouble(txtOdemeTutar.Text);
            double PCDSEXCHAMOUNT = ParseToDouble(txtOdemeTutar.Text);
            double PCDSEARLYPAYDISC = ParseToDouble(txtErkenOdeme.Text);
            double PCDSLATEINCOME = ParseToDouble(txtVadeFarki.Text);
            double PCDSCHAMOUNT = ParseToDouble(txtOdemeTutar.Text);
            string PCDSID = txtOdemeID.Text;
            string CURVAL = txtBakiyeMusteriNo.Text;
            executeBackground(
       () =>
       {
           progressForm.Show(this);
           try
           {
               var sonuc1 = conn.InsertValue($"update PROCEEDS set PCDSAMOUNT = {PCDSAMOUNT},PCDSEXCHAMOUNT = {PCDSAMOUNT},PCDSEARLYPAYDISC={PCDSEARLYPAYDISC},PCDSLATEINCOME={PCDSLATEINCOME} where PCDSID = {txtOdemeID.Text}", sql);
               if (sonuc1 != 0)
               {
                   double Fark = ((PCDSAMOUNT+ PCDSEARLYPAYDISC)- PCDSLATEINCOME) - LastPCDSAMOUNT;
                   if (toggleSwitch1.IsOn == true)
                   {
                       Fark = 0;
                   }
                   if (Fark > 0)
                   {
                       var minID = conn.GetValue($"select min(PCDSCHID) from PROCEEDSCHILD where PCDSCHPCDSID = {txtOdemeID.Text}");
                       var sonuc2 = conn.InsertValue($@"update SAFEBEHAVE set SABHAMOUNT = SABHAMOUNT + {Fark},SABHEXCHAMOUNT = SABHEXCHAMOUNT + {Fark} where SABHPCDSCHID = {minID}", sql);
                       var sonuc3 = conn.InsertValue($@"update PROCEEDSCHILD set PCDSCHAMOUNT = PCDSCHAMOUNT + {Fark} where PCDSCHID = {minID}", sql);
                   }
                   else
                   {
                       Fark = Fark * -1;
                       var minID = conn.GetValue($"select min(PCDSCHID) from PROCEEDSCHILD where PCDSCHPCDSID = {txtOdemeID.Text}");
                       var sonuc2 = conn.InsertValue($@"update SAFEBEHAVE set SABHAMOUNT = SABHAMOUNT - {Fark},SABHEXCHAMOUNT = SABHEXCHAMOUNT - {Fark} where SABHPCDSCHID = {minID}", sql);
                       var sonuc3 = conn.InsertValue($@"update PROCEEDSCHILD set PCDSCHAMOUNT = PCDSCHAMOUNT - {Fark} where PCDSCHID = {minID}", sql);
                   }
                   //var sonuc4 = conn.InsertValue($"update PROCEEDSCHILD set PCDSCHAMOUNT = {PCDSAMOUNT} where PCDSCHPCDSID = {txtOdemeID.Text}", sql);
                   XtraMessageBox.Show("İşlem Tamam");
               }
               progressForm.PerformStep(this);
               success++;
           }
           catch (Exception ex)
           {
               Console.Write(ex.Message);
               error++;
               progressForm.PerformStep(this);
           }

       },
                     null,
                     () =>
                     {
                         if (!string.IsNullOrEmpty(txtBakiyeMusteriNo.Text))
                         {
                             Dictionary<string, string> keyValues = new Dictionary<string, string>();
                             keyValues.Add("@CURVAL", CURVAL);
                             conn.Insert("FK_INSTALMENTCANCELRECREATENEW", keyValues);
                             btnYeni_Click(null, null);
                         }
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "Toplam Silinen Satır : " + silinenadet, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
        }
        static double ParseToDouble(string value)
        {
            // Eğer değer boş veya null ise 0 döndür
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0; // Varsayılan olarak 0 atanabilir
            }
            // Nokta yerine virgül olan durumlar için normalize et
            string normalizedValue = value.Replace(',', '.');

            // Normalize edilmiş değeri InvariantCulture ile çevir
            if (double.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            else
            {
                throw new FormatException("Geçersiz sayı formatı!");
            }
        }

        private void tileBarItem12_ItemClick(object sender, TileItemEventArgs e)
        {
            double control = 0;
            double toplam = double.Parse(ViewOdemeDuzeltme.GetRowCellValue(0, "PCDSAMOUNT").ToString());
            for (int ii = 0; ii < ViewOdemeDuzeltme.RowCount; ii++)
            {
                control += double.Parse(ViewOdemeDuzeltme.GetRowCellValue(ii, "PCDSAMOUNT").ToString());
            }
            if (control == toplam)
            {
                for (int i = 0; i < ViewOdemeDuzeltme.RowCount; i++)
                {
                    var PCDSCHID = ViewOdemeDuzeltme.GetRowCellValue(i, "PCDSAMOUNT").ToString();
                    double PCDSCHAMOUNT = double.Parse(ViewOdemeDuzeltme.GetRowCellValue(i, "PCDSAMOUNT").ToString());
                    var DPYMVAL = ViewOdemeDuzeltme.GetRowCellValue(i, "PCDSAMOUNT").ToString();
                    var DPYMNAME = ViewOdemeDuzeltme.GetRowCellValue(i, "PCDSAMOUNT").ToString();
                    conn.InsertValue($"update PROCEEDSCHILD set PCDSCHDPYMID = '{DPYMVAL}', PCDSCHAMOUNT = '{PCDSCHAMOUNT}' where PCDSCHID = {PCDSCHID}", sql);
                }
            }
            else
            {
                XtraMessageBox.Show("Toplam Tutar Eşit Değil");
            }
            btnYeni_Click(null, null);
        }

        private void btnIysListele_Click(object sender, EventArgs e)
        {
            string q = String.Format(@"select CURCHID as ID,CURCHTITLE as MUSTERİ, CURCHGSM1 as GSM1,
            replace(CURCHGSM2,CURCHGSM1,'') as GSM2,
            replace(replace(CURCHGSM3,CURCHGSM1,''),CURCHGSM2,'') as GSM3,
            CURCHCHECKGSM, '' Sonuc
            from CURRENTSCHILD 
            left outer join CURRENTS on CURID = CURCHID
            left outer join CUSTOMER on CUSCURID = CURCHID
            where (CURCHCHECKGSM is not NULL and CURCHCHECKGSM != '9999' and CURCHCHECKGSM != '') and (CURCHGSM1 like '5%'or CURCHGSM2 like '5%' or CURCHGSM3 like '5%') and (CURCHIYS = 0 or CURCHIYS is NULL)
            and isnull(CURUSEFIELD2,'') = ''
            and CUSDATETIME >= '{0}'", DateTime.Parse(dteIys.EditValue.ToString()).ToString("yyyy-MM-dd"));
            if (srcIys.EditValue != null && !string.IsNullOrEmpty(srcIys.EditValue.ToString()))
            {
                if (srcIys.EditValue.ToString() != "")
                {
                    q = q + String.Format(@"
                and CURDIVISON = '{0}'
                order by 3", srcIys.EditValue);
                }
            }
            gridIYS.DataSource = conn.GetData(q, sql);
        }
        private async Task executeBackgroundwait(Func<Task> work, Action onCompleted, Action onFail) { try { await work(); onCompleted?.Invoke(); } catch (Exception) { onFail?.Invoke(); } }
        private static readonly HttpClient client = new HttpClient();
        private async void btnIYSkaydet_ItemClick(object sender, TileItemEventArgs e)
        {
            var selectedRows = ViewIYS.GetSelectedRows();
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = selectedRows.Length,
                Position = 0,
                ToplamAdet = selectedRows.Length.ToString(),
            };
            int success = 0;
            int error = 0;
            this.Enabled = false;
            int silinenadet = 0;
            string GSM1 = "";
            string GSM2 = "";
            string GSM3 = "";
            string CURID = "";
            string SatirSonuc = "";
            await executeBackgroundwait(
       async () =>
       {
           // API URL'si
           string apiUrl = "https://restapi.ttmesaj.com/api/Iys/SendRecipient";

           progressForm.Show(this);
           for (int i = 0; i < selectedRows.Length; i++)
           {
               try
               {
                   GSM1 = ViewIYS.GetRowCellValue(selectedRows[i], "GSM1").ToString();
                   GSM2 = ViewIYS.GetRowCellValue(selectedRows[i], "GSM2").ToString();
                   GSM3 = ViewIYS.GetRowCellValue(selectedRows[i], "GSM3").ToString();
                   CURID = ViewIYS.GetRowCellValue(selectedRows[i], "ID").ToString();
                   if (GSM1 != "")
                   {
                       // Gönderilecek JSON verisi
                       var requestBody = new
                       {
                           userName = Properties.Settings.Default.SmsUser, // Kullanıcı adı
                           password = Properties.Settings.Default.SmsPassword, // Şifre
                           permissionType = "MESAJ", // İzin türü
                           brandCode = "611821", // Marka kodu
                           permissionStatus = "ONAY", // İzin durumu
                           permissionSource = 4, // Kaynak
                           isCheckBlackList = "1", // Kara liste kontrolü
                           iysDatas = new[]
                           {
                               new
                               {
                                   recipient = GSM1, // Alıcı numarası
                                   receiveType = "BIREYSEL", // Alıcı tipi
                                   consentDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy hh:mm")) // Onay tarihi
                               }
                           }
                       };

                       // JSON verisini oluştur
                       var jsonContent = JsonConvert.SerializeObject(requestBody);

                       // İçeriği oluştur
                       var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                       try
                       {
                           // API'ye POST isteği gönder
                           client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Properties.Settings.Default.SmsToken);
                           HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                           if (response.IsSuccessStatusCode)
                           {
                               // Başarılı ise yanıtı al ve işle
                               var responseBody = await response.Content.ReadAsStringAsync();
                               IYSResponse smsSonuc = JsonConvert.DeserializeObject<IYSResponse>(responseBody);
                               var update = conn.InsertValue($@"update CURRENTS set CURUSEFIELD2 = '{smsSonuc.PackageId}' where CURID = {CURID}", sql);
                               SatirSonuc += GSM1 + "-" + smsSonuc.ResponseDesc;
                               Invoke((MethodInvoker)delegate
                               {
                                   ViewIYS.SetRowCellValue(selectedRows[i], "Sonuc", smsSonuc.ResponseDesc);
                               });
                           }
                           else
                           {
                               // Hata durumunda
                               Console.WriteLine("Hata: " + response.StatusCode);
                               SatirSonuc += GSM1 + "-" + response;
                               Invoke((MethodInvoker)delegate
                               {
                                   ViewIYS.SetRowCellValue(selectedRows[i], "Sonuc", response);
                               });
                           }
                       }
                       catch (Exception ex)
                       {
                           // İstek sırasında oluşan hatayı yakala
                           Invoke((MethodInvoker)delegate
                           {
                               ViewIYS.SetRowCellValue(selectedRows[i], "Sonuc", ex.Message);
                           });
                           Console.WriteLine("Hata: " + ex.Message);
                           SatirSonuc += GSM1 + "-" + ex.Message;
                       }
                   }
                   if (GSM2 != "")
                   {
                       //SMS.SendRequest("ONAY", "MESAJ", GSM3, DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy hh:mm")));
                   }
                   if (GSM3 != "")
                   {
                       //SMS.SendRequest("ONAY", "4", GSM3, DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy hh:mm")));
                   }

                   progressForm.PerformStep(this);
                   success++;
               }
               catch (Exception)
               {
                   progressForm.PerformStep(this);
                   error++;
               }
           }
       },

                     null,
                     () =>
                     {
                         completeProgress();
                         progressForm.Hide(this);
                         XtraMessageBox.Show("Gönderim tamamlandı. Toplam Başarılı : ." + success + " Toplam Hatalı : " + error + "Sonuc: " + SatirSonuc.ToString(), "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         this.Focus();
                     });
            progressForm.Hide(this);
        }

        private void ViewTaksitDetayi_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            DataRow newRow = view.GetDataRow(e.RowHandle);
            newRow["PCDSCHID"] = 0; // Varsayılan değerler eklenebilir 
            newRow["PCDSCHAMOUNT"] = KalanPCDSAMOUNT; // Varsayılan değerler eklenebilir 
            newRow["DPYMVAL"] = 'N'; // Varsayılan değerler eklenebilir 
            newRow["DPYMNAME"] = ""; // Varsayılan değerler eklenebilir
            //desiredRowCount++;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            double Amount = 0;
            for (int i = 0; i < ViewTaksitDetayi.RowCount; i++)
            {
                double existingAmount;
                if (double.TryParse(ViewTaksitDetayi.GetRowCellValue(i,"PCDSCHAMOUNT").ToString(), out existingAmount))
                {
                    Amount += existingAmount;
                }
            }
            KalanPCDSAMOUNT = LastPCDSAMOUNT - Amount;
            ViewTaksitDetayi.BeginUpdate();
            try
            {
                ViewTaksitDetayi.AddNewRow();
                ViewTaksitDetayi.ShowEditForm();// Yeni eklenen satırın düzenleme modunda açılması 
            }
            finally
            {
                ViewTaksitDetayi.EndUpdate();
            }

            //ViewTaksitDetayi.AddNewRow();
            //ViewTaksitDetayi.ShowEditForm(); // Yeni eklenen satırın düzenleme modunda açılması 
        }

        private void ViewTaksitDetayi_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null)
            {
                e.Valid = false;
                e.ErrorText = "GridView nesnesi null.";
                return;
            }
            DataRow row = view.GetDataRow(e.RowHandle);
            if (row == null)
            {
                e.Valid = false;
                e.ErrorText = "DataRow nesnesi null.";
                return;
            }
            double newAmount = 0;
            if (!double.TryParse(row["PCDSCHAMOUNT"]?.ToString(), out newAmount))
            {
                e.Valid = false;
                e.ErrorText = "PCDSCHAMOUNT değeri geçersiz.";
                return;
            }
            double totalAmount = newAmount;
            for (int i = 0; i < view.RowCount; i++)
            {
                if (i != e.RowHandle)
                {
                    DataRow existingRow = view.GetDataRow(i);
                    if (existingRow != null && existingRow["PCDSCHAMOUNT"] != DBNull.Value)
                    {
                        double existingAmount;
                        if (double.TryParse(existingRow["PCDSCHAMOUNT"].ToString(), out existingAmount))
                        {
                            totalAmount += existingAmount;
                        }
                    }
                }
            }
            if (totalAmount > LastPCDSAMOUNT)
            {
                e.Valid = false;
                e.ErrorText = $"Toplam PCDSCHAMOUNT değeri {LastPCDSAMOUNT} değerini aşamaz.";
            }
        }

        private void ViewTaksitDetayi_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            // Hata mesajının varsayılan olarak gösterilmesini engelle 
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void btnOdemeDetayı_ItemClick(object sender, TileItemEventArgs e)
        {
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = ViewTaksitDetayi.RowCount,
                Position = 0,
                ToplamAdet = ViewTaksitDetayi.RowCount.ToString(),
            };
            int success = 0;
            int error = 0;
            executeBackground(
       () =>
       {
           progressForm.Show(this);
           for (int i = 0; i < ViewTaksitDetayi.RowCount; i++)
           {
               List<PASSBEHAVE> pASSBEHAVEs = new List<PASSBEHAVE>();
               var DEFPAYMENTS = conn.GetData($@"SELECT DPYMID,JPYMCURID,DPYMKIND
                FROM  DEFPAYMENTKIND  
                LEFT OUTER JOIN JOINPAYKINDDIV ON JPYMID=DPYMID AND JPYMDIVISON= '00' AND JPYMYEAR=2024 
                LEFT OUTER JOIN CURRENTS CURRENTS1 ON CURRENTS1.CURID=JPYMCURID 
                WHERE DPYMCOMPANY = '01' 
                --AND DPYMKIND = 'K' 
                AND DPYMSTS= 1
                AND DPYMVAL = '{ViewTaksitDetayi.GetRowCellValue(i, "DPYMVAL").ToString()}'", sql);
               var PCDSCHPCDSID = "0";
               var DIVISON = "";
               var CURID = "";
               var SOCODE = "";
               var ACCID = "0";
               var NOTES = "";
               DateTime DATETIME = DateTime.Now;
               string SOURCE = "";
               try
               {
                   var PCDSCHDPYMID = DEFPAYMENTS.Rows[0]["DPYMID"].ToString(); //conn.GetValue($"select DPYMID from DEFPAYMENTKIND where DPYMVAL = '{ViewTaksitDetayi.GetRowCellValue(i, "DPYMVAL").ToString()}'");
                   if (!String.IsNullOrEmpty(txtOdemeID.Text))
                   {
                       PCDSCHPCDSID = txtOdemeID.Text;
                   }
                   else
                   {
                       string q = string.Format(@"select PCDSID,SALDIVISON,SALCURID,SALSOCODE,SALDATETIME,PRMACCID,
                        case when SALSHIPKIND = 'H' then 'HEMEN TESLIM' else 'SONRA TESLIM' end+'/'+
                        case when SALSALEKIND = 'P' then 'PEŞİN' else CURNAME end as NOTE,
                        'TA.'+ case when SALSALEKIND = 'P' then ' PS' else 'TK' end as SOURCE
                        from SALES
                        left outer join CURRENTS on CURID = SALCURID
                        left outer join PROCEEDS on PCDSSALID = SALID
                        left outer join MDE_GENEL.dbo.MagazaYon on PRMDIVVAL = SALDIVISON
                        where SALID = {0}", txtSatısID.Text);
                       var DEGISGEN = conn.GetData(q, sql);

                       PCDSCHPCDSID = DEGISGEN.Rows[0]["PCDSID"].ToString();
                       DIVISON = DEGISGEN.Rows[0]["SALDIVISON"].ToString();
                       CURID = DEGISGEN.Rows[0]["SALCURID"].ToString();
                       SOCODE = DEGISGEN.Rows[0]["SALSOCODE"].ToString();
                       DATETIME = DateTime.Parse(DEGISGEN.Rows[0]["SALDATETIME"].ToString());
                       ACCID = DEGISGEN.Rows[0]["PRMACCID"].ToString();
                       NOTES = DEGISGEN.Rows[0]["NOTE"].ToString();
                       SOURCE = DEGISGEN.Rows[0]["SOURCE"].ToString();
                   }
                   var PCSDAMOUNT = ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHAMOUNT").ToString();
                   if (ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString() != "0")
                   {
                       if (ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHAMOUNT").ToString() != "0")
                       {
                           string q = String.Format("update PROCEEDSCHILD set PCDSCHAMOUNT = {0},PCDSCHDPYMID = {1} where PCDSCHID = {2}", PCSDAMOUNT, PCDSCHDPYMID, ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString());
                           var sonuc = conn.InsertValue(q, sql);
                           if (sonuc == 0)
                           {
                               error++;
                           }
                           else
                           {
                               if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "K")
                               {
                                   conn.InsertValue($@"update PASSBEHAVE set PABHAMOUNT = {PCSDAMOUNT}, PABHEXCHAMOUNT = {PCSDAMOUNT} where PABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}", sql);
                               }
                               else if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "N")
                               {
                                   conn.InsertValue($@"update SAFEBEHAVE set SABHAMOUNT = {PCSDAMOUNT} , SABHEXCHAMOUNT = {PCSDAMOUNT} where SABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}", sql);
                                   conn.InsertValue($@"update SAFEBEHAVECHILD set SABHCHAMOUNT = {PCSDAMOUNT}, SABHCHEXCHAMOUNT = {PCSDAMOUNT} from SAFEBEHAVE 
                                    left outer join SAFEBEHAVECHILD on SABHCHSABHID = SABHID
                                    where SABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}",sql);
                               }
                               success++;
                           }
                           progressForm.PerformStep(this);
                       }
                       else
                       {
                           if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "K")
                           {
                               var sili1 = conn.InsertValue($@"delete PASSBEHAVE where PABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}", sql);
                           }
                           else if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "N")
                           {
                               var sonuc1 = conn.InsertValue($@"delete SAFEBEHAVE where SABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}", sql);
                               var sonuc2 = conn.InsertValue($@"delete SAFEBEHAVECHILD where SABHCHSABHID in (select SABHID from SAFEBEHAVE where SABHPCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()})", sql);
                           }
                           var sonuc = conn.InsertValue($"delete PROCEEDSCHILD where PCDSCHID = {ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHID").ToString()}", sql);
                           if (sonuc == 0)
                           {
                               error++;
                           }
                           else
                           {
                               success++;
                           }
                           progressForm.PerformStep(this);
                       }
                   }
                   else
                   {

                       if (ViewTaksitDetayi.GetRowCellValue(i, "PCDSCHAMOUNT").ToString() != "0")
                       {
                           var PCSDCHID = conn.GetValue(@"update REGISTER set RGID = RGID +1 where RGKIND = '194'
                                                  select RGID from REGISTER where RGKIND = '194'");
                           string q = $"insert into PROCEEDSCHILD(PCDSCHID,PCDSCHPCDSID,PCDSCHDPYMID,PCDSCHAMOUNT) values ({PCSDCHID},{PCDSCHPCDSID},{PCDSCHDPYMID},{PCSDAMOUNT})";
                           var sonuc = conn.InsertValue(q, sql);
                           if (sonuc == 0)
                           {
                               error++;
                           }
                           else
                           {
                               try
                               {
                                   if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "K")
                                   {
                                       PASSBEHAVE pASSBEHAVE = new PASSBEHAVE();
                                       pASSBEHAVE.PABHID = long.Parse(conn.GetValue("update REGISTER set RGID = RGID +1 where RGKIND = '196' select RGID from REGISTER where RGKIND = '196'"));
                                       pASSBEHAVE.PABHCOMPANY = "01";
                                       pASSBEHAVE.PABHDIVISON = DIVISON;
                                       pASSBEHAVE.PABHDATE = Convert.ToDateTime(DATETIME.ToString("yyyy-MM-dd"));
                                       pASSBEHAVE.PABHCRCACURID = long.Parse(DEFPAYMENTS.Rows[0]["JPYMCURID"].ToString());
                                       pASSBEHAVE.PABHINSCOUNT = 1;
                                       pASSBEHAVE.PABHDC = "0";
                                       pASSBEHAVE.PABHEXCHAMOUNT = long.Parse(PCSDAMOUNT);
                                       pASSBEHAVE.PABHEXCH = "TL";
                                       pASSBEHAVE.PABHEXCHRATE = 1;
                                       pASSBEHAVE.PABHAMOUNT = long.Parse(PCSDAMOUNT);
                                       pASSBEHAVE.PABHCURID = long.Parse(CURID);
                                       pASSBEHAVE.PABHACCID = long.Parse(ACCID);
                                       pASSBEHAVE.PABHNOTES = NOTES;
                                       pASSBEHAVE.PABHPCDSCHID = long.Parse(PCSDCHID);
                                       pASSBEHAVE.PABHSOCODE = SOCODE;
                                       pASSBEHAVE.PABHDATETIME = DATETIME;
                                       pASSBEHAVEs.Add(pASSBEHAVE);
                                       DataTable dt = converter.ToDataTable(pASSBEHAVEs);
                                       string kayıt = conn.BulkInsertRetorn(dt, "PASSBEHAVE", sql);
                                   }
                                   else if (DEFPAYMENTS.Rows[0]["DPYMKIND"].ToString() == "N")
                                   {
                                       var SABHID = conn.GetValue(@"update REGISTER set RGID = RGID +1 where RGKIND = '130'
                                                  select RGID from REGISTER where RGKIND = '130'");
                                       var SABHCHID = conn.GetValue(@"update REGISTER set RGID = RGID +1 where RGKIND = '195'
                                                  select RGID from REGISTER where RGKIND = '195'");
                                       List<SAFEBHAVE> sAFEBHAVEs = new List<SAFEBHAVE>();
                                       List<SAFBHAVECHILD> sAFBHAVECHILDs = new List<SAFBHAVECHILD>();
                                       var SAFEID = conn.GetValue($"select DSAFEID from DEFSAFE where DSAFESTS = 1 and DSAFEKIND = 'K' and DSAFEDIVISON = '07' and substring(DSAFEVAL,4,{SOCODE.Length}) = '{SOCODE}'");
                                       if (SOURCE == "")
                                       {
                                           SOURCE = "TA.PS";
                                       }
                                       SAFEBHAVE sAFEBHAVE = new SAFEBHAVE();
                                       sAFEBHAVE.SABHID = SABHID;
                                       sAFEBHAVE.SABHDSAFEID = long.Parse(SAFEID);
                                       sAFEBHAVE.SABHCOMPANY = "01";
                                       sAFEBHAVE.SABHDIVISON = DIVISON;
                                       sAFEBHAVE.SABHSOURCE = SOURCE;
                                       sAFEBHAVE.SABHDATE = Convert.ToDateTime(DATETIME.ToString("yyyy-MM-dd")); 
                                       sAFEBHAVE.SABHDC = "0";
                                       sAFEBHAVE.SABHBT = false;
                                       sAFEBHAVE.SABHDEEDKIND = "K1";
                                       sAFEBHAVE.SABHDEEDDATE = Convert.ToDateTime(DATETIME.ToString("yyyy-MM-dd")); ;
                                       sAFEBHAVE.SABHDEEDNOTES = NOTES;
                                       sAFEBHAVE.SABHEXCHAMOUNT = double.Parse(PCSDAMOUNT);
                                       sAFEBHAVE.SABHEXCH = "TL";
                                       sAFEBHAVE.SABHRATE = "1";
                                       sAFEBHAVE.SABHAMOUNT = double.Parse(PCSDAMOUNT);
                                       sAFEBHAVE.SABHPCDSCHID = long.Parse(PCSDCHID);
                                       sAFEBHAVE.SABHACCKIND = "0";
                                       sAFEBHAVE.SABHACCSTS = "M";
                                       sAFEBHAVE.SABHSOCODE = SOCODE;
                                       sAFEBHAVE.SABHDATETIME = DATETIME;
                                       sAFEBHAVEs.Add(sAFEBHAVE);
                                       DataTable dt = converter.ToDataTable(sAFEBHAVEs);
                                       string kayıt = conn.BulkInsertRetorn(dt, "SAFEBEHAVE", sql);
                                       if (kayıt == "Aktarım Tamamlandı")
                                       {
                                           SAFBHAVECHILD sAFBHAVECHILD = new SAFBHAVECHILD();
                                           sAFBHAVECHILD.SABHCHID = long.Parse(SABHCHID);
                                           sAFBHAVECHILD.SABHCHSABHID = long.Parse(SABHID);
                                           sAFBHAVECHILD.SABHCHEXCHAMOUNT = double.Parse(PCSDAMOUNT);
                                           sAFBHAVECHILD.SABHCHAMOUNT = double.Parse(PCSDAMOUNT);
                                           sAFBHAVECHILD.SABHCHDC = 1.ToString();
                                           sAFBHAVECHILD.SABHCHCURID = long.Parse(CURID);
                                           sAFBHAVECHILD.SABHCHACCID = long.Parse(ACCID);
                                           sAFBHAVECHILD.SABHCHACCEXCH = "TL";
                                           sAFBHAVECHILD.SABHCHEXCHRATE = 1;
                                           sAFBHAVECHILD.SABHCHNOTES = NOTES;
                                           sAFBHAVECHILDs.Add(sAFBHAVECHILD);
                                           DataTable dt2 = converter.ToDataTable(sAFBHAVECHILDs);
                                           string kayıt2 = conn.BulkInsertRetorn(dt2, "SAFEBEHAVECHILD", sql);
                                       }
                                   }
                                   success++;
                               }
                               catch (Exception)
                               {
                                   error++;
                               }
                           }
                           progressForm.PerformStep(this);
                       }
                   }
               }
               catch (Exception)
               {
                   error++;
                   progressForm.PerformStep(this);
               }
           }

       },

            null,
            () =>
            {
                completeProgress();
                progressForm.Hide(this);
                XtraMessageBox.Show("", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnYeni_Click(null, null);
            });
        }

        private void btnKrediYeni_Click(object sender, EventArgs e)
        {
            gridKredi.DataSource = null;
        }
    }
}