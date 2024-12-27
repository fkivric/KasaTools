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

namespace KrediPuan
{
    public partial class Control_PuanTanımlama : DevExpress.XtraEditors.XtraUserControl
    {
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        public Control_PuanTanımlama()
        {
            InitializeComponent();
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            if (navigationFrame.SelectedPage == navigationPage2)
            {
                string q = "select PROUID,PROUVAL,PROUNAME,VOLUID,Risk_id, Risk_Adi,p.Risk_TutarMin,p.Risk_TutarMax " +
                            "from PRODUCTSUNITED v " +
                            "left outer join "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurup k on k.VOLUID = v.PROUID "+
                            "left outer join "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurupPuan p on p.id = k.Risk_id";
                var dt = DataDonen(q);
                gridUnited.DataSource = dt;
                viewUnited.OptionsView.BestFitMaxRowCount = -1;
                viewUnited.BestFitColumns(true);
                var dd = DataDonen("select id as Risk_id, Risk_Adi from "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurupPuan");
                cmbRiskUrunGurup.DataSource = dd;
                cmbRiskUrunGurup.ValueMember = "Risk_id";
                cmbRiskUrunGurup.DisplayMember = "Risk_Adi";
            }
            if (navigationFrame.SelectedPage == navigationPage3)
            {
                cmbRiskGurbu.Properties.DataSource = DataDonen("select id, Risk_Adi from "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurupPuan");
                cmbRiskGurbu.Properties.DisplayMember = "Risk_Adi";
                cmbRiskGurbu.Properties.ValueMember = "id";
            }
        }
        public DataTable DataDonen(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }        
        private void Control_PuanTanımlama_Load(object sender, EventArgs e)
        {
            var dt = DataDonen("select id,Risk_Adi,Risk_TutarMin,Risk_TutarMax from "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurupPuan");
            gridUrunGurupPuan.DataSource = dt;            
        }

        private void cmbRiskGurbu_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbRiskGurbu.EditValue != null)
            {
                gridYetkilendirme.DataSource = DataDonen(string.Format("select DEPVAL,DEPNAME,isnull(RiskUrunGurup{0},0) as RiskUrunGurup from DEPARTMENT\r\n" +
                    "left outer join "+Properties.Settings.Default.DbName+".[dbo].[EDevlet_RiskGrubu_Yetkilendirme]  on DEPVAL = YetkiDepval", cmbRiskGurbu.EditValue));
            }
            else
            {
                
            }
            
        }

        public int Update(string query)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            sql.Open();
            var sonuc = cmd.ExecuteNonQuery(); 
            sql.Close();
            return sonuc;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int adet = 0;
            for (int i = 0; i < viewUrunGurupPuan.RowCount; i++)
            {
                var id = viewUrunGurupPuan.GetRowCellValue(i, "id");
                var Risk_Adi = viewUrunGurupPuan.GetRowCellValue(i, "Risk_Adi");
                var Risk_TutarMin = viewUrunGurupPuan.GetRowCellValue(i, "Risk_TutarMin");
                var Risk_TutarMax = viewUrunGurupPuan.GetRowCellValue(i, "Risk_TutarMax");
                string q = string.Format("Update "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurupPuan set Risk_Adi= '{1}',Risk_TutarMin='{2}',Risk_TutarMax='{3}' where id = {0}", id, Risk_Adi, Risk_TutarMin, Risk_TutarMax);
                adet = adet + (Update(q));
            }
            if (adet == 4)
            {
                XtraMessageBox.Show("Güncellendi");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < viewUnited.RowCount; i++)
            {
                var PROUID = viewUnited.GetRowCellValue(i, "PROUID");
                var Risk_id = viewUnited.GetRowCellValue (i, "Risk_id");
                var dd = DataDonen(string.Format("select * from "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurup where VOLUID = '{0}' and Risk_id = '{1}'", PROUID, Risk_id));
                if (dd.Rows.Count != 0)
                {
                    Update(string.Format("update "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurup set Risk_id = '{1}' where VOLUID = '{0}' ",PROUID, Risk_id));
                }
                else if (Risk_id != null)
                {
                    Update(string.Format("insert into  "+Properties.Settings.Default.DbName+".[dbo].KrediPuan_RiskUrunGurup values('{1}' ,'{0}') ", PROUID, Risk_id));
                }
            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
