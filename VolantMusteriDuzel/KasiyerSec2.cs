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
using VolantMusteriDuzel.Properties;

namespace VolantMusteriDuzel
{
    public partial class KasiyerSec2 : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection sql = new SqlConnection(Settings.Default.connectionstring);
        SqlConnection MDE = new SqlConnection(Settings.Default.connectionstring2);
        SqlConnectionObject conn = new SqlConnectionObject();
        public KasiyerSec2()
        {
            InitializeComponent();
        }

        private void KasiyerSec_Load(object sender, EventArgs e)
        {
            string q = @"select CHSOCODE,DSAFEID,DSAFEVAL,DSAFENAME,UPPER(isnull(SONAME + ' ' + SOSURNAME,'Merkez Kasa')) as KasiyerAdı from DEFSAFE 
            left outer join CASHIER on DSAFEUNITE = CHSAFEUNI
            left outer join SOCIAL on SOCODE = CHSOCODE 
            where DSAFEDIVISON  ='00' and DSAFEID in (2608,2815,2155)
            order by 4";

            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            srcKasiyerKasa.Properties.DataSource = dt;
            srcKasiyerKasa.Properties.ValueMember = "DSAFEID";
            srcKasiyerKasa.Properties.DisplayMember = "DSAFENAME";
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form1.CHSOCODE = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "CHSOCODE").ToString();
            Form1.DSAFEID = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEID").ToString();
            Form1.DSAFEVAL = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEVAL").ToString();
            Form1.DSAFENAME = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFENAME").ToString();
            this.Close();
            this.Dispose();
        }
    }
}