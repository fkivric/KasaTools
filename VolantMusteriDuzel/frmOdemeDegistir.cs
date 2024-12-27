using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolantMusteriDuzel
{
    public partial class frmOdemeDegistir : DevExpress.XtraEditors.XtraForm
    {
        SqlConnectionObject conn = new SqlConnectionObject();
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        string PCDSID;
        string trueCURID;
        string falseCURID;
        public frmOdemeDegistir(string _pcdsid, string _curid)
        {
            InitializeComponent();
            PCDSID = _pcdsid;
            falseCURID = _curid;
        }
        private void frmOdemeDegistir_Load(object sender, EventArgs e)
        {
            var odeme = conn.GetData($"select PCDSAMOUNT,PCDSLATEINCOME,PCDSEARLYPAYDISC from PROCEEDS where PCDSID = {PCDSID}", sql);
            double amount = double.Parse(odeme.Rows[0]["PCDSAMOUNT"].ToString());
            double erken = double.Parse(odeme.Rows[0]["PCDSEARLYPAYDISC"].ToString());
            double vade = double.Parse(odeme.Rows[0]["PCDSLATEINCOME"].ToString());
            double net = (amount - erken) + vade;
            lblTutar.Text = net.ToString(); // odeme.Rows[0]["PCDSAMOUNT"].ToString();
            lblVade.Text = vade.ToString(); //odeme.Rows[0]["PCDSLATEINCOME"].ToString();
            lblIndirim.Text = erken.ToString(); //odeme.Rows[0]["PCDSEARLYPAYDISC"].ToString();
            gridControl1.DataSource = conn.GetData($"select INSID,INSFIXDATE,INSAMOUNT,INSPCDAMOUNT,INSBALANCE from INSTALMENTPROCEEDS left outer join INSTALMENT on INSID = INSPCDINSID where INSPCDPCDID = {PCDSID}", sql);
        }
        //
        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                trueCURID = conn.GetValue($"select CURID from CURRENTS where  CURVAL = '{textEdit1.Text}'");
                SqlCommand taksitsil = new SqlCommand($"delete INSTALMENTPROCEEDS where INSPCDPCDID = {PCDSID}", sql);
                sql.Open();
                taksitsil.ExecuteNonQuery();
                var nettutar = double.Parse(lblTutar.Text.ToString()) - double.Parse(textEdit2.EditValue.ToString());
                string q = String.Format(@"update PROCEEDS set PCDSCURID ={1} ,PCDSAMOUNT = {2},PCDSLATEINCOME = {3},PCDSEARLYPAYDISC = {4} where PCDSID = {0}", PCDSID, trueCURID, nettutar.ToString(), textEdit2.Text, "0");
                SqlCommand guncelle = new SqlCommand(q, sql);
                guncelle.ExecuteNonQuery();
                SqlCommand sp1 = new SqlCommand($"exec FK_INSTALMENTRECREATENEW {falseCURID}", sql);
                SqlCommand sp2 = new SqlCommand($"exec FK_INSTALMENTRECREATENEW {trueCURID}", sql);
                sp1.ExecuteNonQuery();
                sp2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            this.Close();
            this.Dispose();
        }
    }
}