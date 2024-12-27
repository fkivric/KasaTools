using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraBars.Navigation;
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
using static KrediPuan.Class.Volant;

namespace KrediPuan
{
    public partial class Control_Satislar : DevExpress.XtraEditors.XtraUserControl
    {
        public Control_Satislar()
        {
            InitializeComponent();
        }
        SqlConnection sql2 = new SqlConnection(Properties.Settings.Default.connectionstring);
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            var ss = tileBarGroupTables.Items.IndexOf(e.Item);
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
        }
        private void Control_Satislar_Load(object sender, EventArgs e)
        {
            DataTable Mgztbl = new DataTable();
            List<Magazalar> magazalars = new List<Magazalar>();
            Mgztbl = DataDonen("select DIVVAL, DIVNAME from DIVISON " +
                                                           "where DIVSTS = 1 and DIVSALESTS = 1");
            for (int i = 0; i < Mgztbl.Rows.Count; i++)
            {
                Magazalar mgz = new Magazalar();
                mgz.DIVVAL = Mgztbl.Rows[i]["DIVVAL"].ToString();
                mgz.DIVNAME = Mgztbl.Rows[i]["DIVNAME"].ToString();
                magazalars.Add(mgz);
            }

            cmbMagazalar.Properties.DataSource = DataDonen("select DIVVAL as [Mağaza Kodu], DIVNAME as [Mağaza Adı] from DIVISON " +
                                                           "where DIVSTS = 1 and DIVSALESTS = 1");
            cmbMagazalar.Properties.DisplayMember = "Mağaza Adı";
            cmbMagazalar.Properties.ValueMember= "Mağaza Kodu";
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            İslemler();
        }

        public DataTable DataDonen(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql2);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }
        private void İslemler()
        {
            gridSatisOnayi.DataSource = null;
            string Bugun = DateTime.Now.ToString("yyyy-MM-dd");
            string q = string.Format("SELECT CURID,CURVAL,CURNAME,CUSCREDIT, ORDID, ORDINVCSAID,YEAR(GETDATE())-YEAR(CUSIDBIRTHDAY) YAS,CUSIDBIRTHDAY,CUSIDTCNO,SALCONSULTATOR, \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN 'Hemen Teslimat' ELSE 'Sonra Teslimat' END TESLIMSEKLI,  \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN INVBALANCEEXCH ELSE ORDBALANCE END NETTUTAR,  \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN INVDATE ELSE ORDDATE END SATISTARIHI,  \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN INVDATETIME ELSE ORDDATETIME END KAYITZAMANI,  \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN INVDIVISON ELSE ORDDIVISON END SUBENO,  \r\n " +
                "CASE WHEN INVID IS NOT NULL THEN INVSOCODE ELSE ORDSOCODE END KASIYER,  \r\n " +
                "DIVISON.DIVNAME, \r\n " +
                "(SELECT SUM(PCDSAMOUNT*PCDSRATE) FROM PROCEEDS WHERE PCDSSALID=s.SALID) PESINAT, \r\n " +
                "(SELECT SUM(CASE WHEN INSFIXDATE< {0} THEN INSBALANCE END) FROM INSTALMENT WHERE INSSALID=s.SALID) GECIKEN, \r\n " +
                "(SELECT SUM(CASE WHEN INSFIXDATE< {0} THEN INSBALANCE END) FROM INSTALMENT WHERE INSCURID=s.SALCURID) TOPLAMGECIKEN, \r\n " +
                "(SELECT SUM(INSPCDAMOUNT) FROM INSTALMENTPROCEEDS LEFT OUTER JOIN INSTALMENT ON INSID=INSPCDINSID WHERE INSSALID=s.SALID) INSTALMENTPROCEEDS, \r\n " +
                "(SELECT SUM (A.SALAMOUNT)FROM SALES A WHERE A.SALCANSALID = s.SALID ) IPTALTUTAR,\r\n " +
                "(SELECT SUM(A.SALAMOUNT)FROM SALES A WHERE EXISTS(SELECT * FROM SALES A2 WHERE A2.SALCHANGEID = A.SALCHANGEID AND A2.SALCANSALID = s.SALID AND A.SALID > 0)) DEGISIMTUTAR, \r\n " +
                "(select sum (PCDSAMOUNT*PCDSRATE) FROM PROCEEDS IPT LEFT OUTER JOIN SALES B  ON B.SALID = IPT.PCDSSALID WHERE B.SALCANSALID = s.SALID AND IPT.PCDSKIND=-2) IPTALPESINAT ,\r\n " +
                "(SELECT ISNULL(SUM(CASE WHEN PCDSDC = '0' THEN(PCDSAMOUNT + PCDSEXTRA) ELSE - 1 * (PCDSAMOUNT + PCDSEXTRA) END), 0) TAHSILATTUTAR\r\n " +
                "FROM PROCEEDS  \r\n " +
                "WHERE PCDSCOMPANY = DIVCOMPANY AND PCDSCURID = CURID) TAHSILATTUTAR,\r\n " +
                "s.*, SALESINVESTIGATION.*  \r\n " +
                "FROM SALES s \r\n " +
                "LEFT OUTER JOIN INVOICE ON INVSALID=SALID \r\n " +
                "LEFT OUTER JOIN ORDERS ON ORDSALID=SALID \r\n " +
                "LEFT OUTER JOIN CURRENTS ON CURID=SALCURID \r\n " +
                "LEFT OUTER JOIN CUSTOMER ON CUSCURID=SALCURID\r\n " +
                "LEFT OUTER JOIN CUSIDENTITY ON CUSIDCURID=SALCURID \r\n " +
                "LEFT OUTER JOIN DIVISON ON DIVVAL=INVDIVISON OR DIVVAL = ORDDIVISON \r\n " +
                "LEFT OUTER JOIN SALESINVESTIGATION ON SAINGTSALID = SALID AND SAINGTCURORWRTRID = SALCURID \r\n " +
                "WHERE SALINSSTS=1 AND SALCREDITCHECK=0  AND SALSHIPKIND='S' AND SALSALEKIND='T' AND ORDSALECONFIRM<>1  \r\n " +
                "AND NOT EXISTS (select * from SALES i where i.SALCANSALID = s.SALID)  \r\n ", Bugun);
            if (cmbMagazalar.EditValue != null)
            {
                q = q + "AND SALDIVISON = '" + cmbMagazalar.EditValue + "'  \r\n ";
            }
            q = q + "ORDER BY CASE WHEN INVID IS NOT NULL THEN INVDATE ELSE ORDDATE END,CASE WHEN INVID IS NOT NULL THEN INVDATETIME ELSE ORDDATETIME END ";
            SqlDataAdapter da = new SqlDataAdapter(q, sql2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridSatisOnayi.DataSource = dt; 
            ViewSatisOnayi.OptionsView.BestFitMaxRowCount = -1;
            ViewSatisOnayi.BestFitColumns(true);
        }

        private void ViewSatisOnayi_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                var CURID = ViewSatisOnayi.GetRowCellValue(ViewSatisOnayi.FocusedRowHandle, "CURID").ToString();
                var CURNAME = ViewSatisOnayi.GetRowCellValue(ViewSatisOnayi.FocusedRowHandle, "CURNAME").ToString();
                var SALID = ViewSatisOnayi.GetRowCellValue(ViewSatisOnayi.FocusedRowHandle, "SALID").ToString();
                frmSatisIslemleri frm = new frmSatisIslemleri(CURNAME,CURID, SALID);
                frm.ShowDialog();
            }
        }
    }
}
