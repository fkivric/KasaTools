using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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

namespace KrediPuan
{
    public partial class Control_Satis : DevExpress.XtraEditors.XtraUserControl
    {
        string CURID;
        string SALID;
        public Control_Satis(string cURID, string sALID)
        {
            InitializeComponent();
            CURID = cURID;
            SALID = sALID;
        }
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        private void Control_Satis_Load(object sender, EventArgs e)
        {
            string SToplamSatis = string.Format("select * from SALES where SALCURID = {0}",CURID);
            string STutar = string.Format("select * from SALES where SALID = {0}", SALID);
            var Tutar = DataDonen(STutar);
            lblTutar.Text = Tutar.Rows[0]["SALAMOUNT"].ToString();
            gridControl1.DataSource = ExecuteQuery(Properties.Settings.Default.connectionstring,string.Format(@"select PROVAL,PRONAME,ORDCHQUAN,ORDCHBALANCE,
            case when (select SALAMOUNT from SALES where SALID = ORDSALID) <= Risk_TutarMax then Risk_id else Risk_id+1 end as Risktutar,
            case when ORDCHBALANCE >= Risk_TutarMax then Risk_id+1 else Risk_id end as Riskid 
			from ORDERS 
            left outer join ORDERSCHILD on ORDCHORDID = ORDID
            left outer join PRODUCTS on PROID = ORDCHPROID
            left outer join KrediPuan.dbo.KrediPuan_RiskUrunGurup on VOLUID = PROPROUID
            left outer join KrediPuan.dbo.KrediPuan_RiskUrunGurupPuan on id = Risk_id
            where ORDSALID = {0}", SALID));
            gridView1.Columns[4].Visible = false;
            gridView1.Columns[5].Visible = false;
            gridView1.FocusedRowHandle = GridControl.InvalidRowHandle;
            gridView1.OptionsSelection.MultiSelect = false;

        }
        public DataTable DataDonen(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }
        public DataTable ExecuteQuery(string connectionString, string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }

                }
            }
            catch (Exception ex)
            {
                // Hata durumunda burada hata işleme kodları ekleyebilirsiniz.
                // Örneğin, hata mesajını kaydedebilir veya kullanıcıya gösterebilirsiniz.
                Console.WriteLine("Hata oluştu: " + ex.Message);
            }

            return dataTable;
        }
        private void btnEDevletPuanla_Click(object sender, EventArgs e)
        {
            frmEDevletKrediPuan frmEDevlet = new frmEDevletKrediPuan("765252");            
            frmEDevlet.ShowDialog();
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (gridView1.DataRowCount > 0)
            {
                if (gridView1.GetRowCellValue(e.RowHandle, "Riskid") != null)
                {
                    var riskid = gridView1.GetRowCellValue(e.RowHandle, "Riskid").ToString();
                    var risktutar = gridView1.GetRowCellValue(e.RowHandle, "Risktutar").ToString();
                    if (riskid == "3")
                    {
                        e.Appearance.BackColor = Color.Red;//System.Drawing.ColorTranslator.FromHtml("#ff0000");
                        e.Appearance.ForeColor = Color.White;//System.Drawing.ColorTranslator.FromHtml("#ff0000");
                    }
                    else if (riskid == "2")
                    {
                        if (int.Parse(risktutar) > int.Parse(riskid))
                        {
                            e.Appearance.BackColor = Color.Yellow;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                            e.Appearance.BackColor2 = Color.Red;
                            e.Appearance.ForeColor = Color.Black;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                        }
                        else if (risktutar == riskid)
                        {
                            e.Appearance.BackColor = Color.Yellow;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                            e.Appearance.ForeColor = Color.Black;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                        }
                    }
                    else if (riskid == "1")
                    {
                        if (int.Parse(risktutar) > int.Parse(riskid))
                        {
                            e.Appearance.BackColor = Color.Green;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                            e.Appearance.BackColor2 = Color.Yellow;
                            e.Appearance.ForeColor = Color.Black;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                        }
                        else if (risktutar == riskid)
                        {
                            e.Appearance.BackColor = Color.Green;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                            e.Appearance.ForeColor = Color.White;// System.Drawing.ColorTranslator.FromHtml("#ccf097");
                        }
                    }
                    else
                    {
                        e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#fff ");
                        e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#fff ");
                    }
                }


            }

        }
    }
}
