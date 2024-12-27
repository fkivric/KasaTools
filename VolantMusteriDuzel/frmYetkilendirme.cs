using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YONAVMSiparis;

namespace VolantMusteriDuzel
{
    public partial class frmYetkilendirme : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        public frmYetkilendirme()
        {
            InitializeComponent();
        }

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
        private void frmYetkilendirme_Load(object sender, EventArgs e)
        {
            srcUsers.Properties.DataSource = Sorgu("select SOCODE,SONAME + ' ' + SOSURNAME as Name from SOCIAL where (SODEPART = '019') and SOSTS = 1", Properties.Settings.Default.connectionstring);
            srcUsers.Properties.ValueMember = "SOCODE";
            srcUsers.Properties.DisplayMember = "Name";
        }
        public DataTable Sorgu(string sorgu, string connection)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sorgu, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void srcUsers_EditValueChanged(object sender, EventArgs e)
        {
            string q = String.Format(@" select DIVVAL,DIVNAME,DIVSTS,POTSTS from POTENCY 
             left outer join DIVISON on DIVVAL = POTNOTES1
             where POTSOURCE = 'DIVISON' and POTDEPART = '{0}'
             and DIVSTS = 1  and DIVSALESTS = 1
            union
            select 'K' + DIVVAL,'KAMALAR ' + DIVNAME,DIVSTS,POTSTS from VDB_KAMALAR01.dbo.POTENCY 
                         left outer join VDB_KAMALAR01.dbo.DIVISON on DIVVAL = POTNOTES1
                         where POTSOURCE = 'DIVISON' and POTDEPART = '{0}'
                         and DIVSTS = 1  and DIVSALESTS = 1", srcUsers.EditValue.ToString());
            gridMagaza.DataSource = Sorgu(q, Properties.Settings.Default.connectionstring);
        }

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            ProgressBarFrm progressForm = new ProgressBarFrm()
            {
                Start = 0,
                Finish = ViewMagaza.RowCount,
                Position = 0,
                ToplamAdet = ViewMagaza.RowCount.ToString(),
            };
            int success = 0;
            int error = 0;
            int yetkili =0;
            int yetkisiz = 0;
            this.Enabled = false;
            executeBackground(
        () =>
        {
            progressForm.Show(this);
            for (int i = 0; i < ViewMagaza.RowCount; i++)
            {
                try
                {
                    string DIVVAL = ViewMagaza.GetRowCellValue(i, "DIVVAL").ToString();
                    string SODEPOART = srcUsers.EditValue.ToString();
                    string POSTS = ViewMagaza.GetRowCellValue(i, "POTSTS").ToString();
                    if (POSTS == "True")
                    {
                        POSTS = "1";
                        yetkili++;
                    }
                    else
                    {
                        POSTS = "0";
                        yetkisiz++;
                    }
                    if (DIVVAL.StartsWith("K"))
                    {
                        string q = String.Format(@"update VDB_KAMALAR01.dbo.POTENCY set POTSTS = '{1}' where POTSOURCE = 'DIVISON' and POTNOTES1 = '{2}' and POTDEPART = '{0}'", SODEPOART, POSTS, DIVVAL.Replace("K",""));
                        SqlCommand cmd = new SqlCommand(q, sql);
                        if (sql.State == ConnectionState.Closed)
                        {
                            sql.Open();
                        }
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string q = String.Format(@"update POTENCY set POTSTS = '{1}' where POTSOURCE = 'DIVISON' and POTNOTES1 = '{2}' and POTDEPART = '{0}'", SODEPOART, POSTS, DIVVAL);
                        SqlCommand cmd = new SqlCommand(q, sql);
                        if (sql.State == ConnectionState.Closed)
                        {
                            sql.Open();
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    error++;
                }
                progressForm.PerformStep(this);
            }
        },
                        null,
                        () =>
                        {
                            completeProgress();
                            progressForm.Hide(this);
                            sql.Close();
                            XtraMessageBox.Show("İşlem tamamlandı. Yetki Verilen Mağaza Sayısı : ." + yetkili + " Yetkisiz Mağaza sayısı : " + yetkisiz, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });
        }
    }
}