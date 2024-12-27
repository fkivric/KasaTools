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
    public partial class Control_RiskGurup : DevExpress.XtraEditors.XtraUserControl
    {
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        public Control_RiskGurup()
        {
            InitializeComponent();
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            if (navigationFrame.SelectedPage == navigationPage1)
            {
                gridRiskGroup1.DataSource = DataDonen(string.Format(@"select r.id,r.Tanım,r.Zorunlu as Risk,p.Puan,p.Ceza from {0}.dbo.EDevlet_RiskGrubu_1 r
                left outer join {0}.dbo.EDevlet_RiskGrubu_Puan_1 p on r.id = p.Risk_id", Properties.Settings.Default.DbName));
            }
            if (navigationFrame.SelectedPage == navigationPage2)
            {
                cmbAnaRisk1.Properties.DataSource = DataDonen(string.Format(@"select * from {0}.dbo.EDevlet_RiskGrubu_1", Properties.Settings.Default.DbName));
                cmbAnaRisk1.Properties.DisplayMember = "Tanım";
                cmbAnaRisk1.Properties.ValueMember = "id";
            }
            if (navigationFrame.SelectedPage == navigationPage3)
            {
                cmbAnaRisk2.Properties.DataSource = DataDonen(string.Format(@"select * from {0}.dbo.EDevlet_RiskGrubu_1", Properties.Settings.Default.DbName));
                cmbAnaRisk2.Properties.DisplayMember = "Tanım";
                cmbAnaRisk2.Properties.ValueMember = "id";
            }
        }

        private void Control_RiskGurup_Load(object sender, EventArgs e)
        {
            gridRiskGroup1.DataSource = DataDonen(string.Format(@"select r.id,r.Tanım,r.Zorunlu as Risk,p.Puan,p.Ceza from {0}.dbo.EDevlet_RiskGrubu_1 r
            left outer join {0}.dbo.EDevlet_RiskGrubu_Puan_1 p on r.id = p.Risk_id", Properties.Settings.Default.DbName));
        }
        public DataTable DataDonen(string query)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            gridRiskGroup2.DataSource = null;
            gridRiskGroup2.DataSource = DataDonen(string.Format(@"select r.id,r.Tanım,r.Risk,p.Puan,p.Ceza from {0}.dbo.EDevlet_RiskGrubu_2 r
            left outer join {0}.dbo.EDevlet_RiskGrubu_Puan_2 p on r.id = p.Risk_id
            where r.Parent_id = {1}", Properties.Settings.Default.DbName, cmbAnaRisk1.EditValue));
        }

        private void cmbAnaRisk2_EditValueChanged(object sender, EventArgs e)
        {
            cmbAltRisk.Properties.DataSource = null;
            cmbAltRisk.Properties.DataSource = DataDonen(string.Format(@"select id,Tanım from {0}.dbo.EDevlet_RiskGrubu_2 where Parent_id = {1}", Properties.Settings.Default.DbName, cmbAnaRisk2.EditValue));
            cmbAltRisk.Properties.DisplayMember = "Tanım";
            cmbAltRisk.Properties.ValueMember = "id";
        }

        private void cmbAltRisk_EditValueChanged(object sender, EventArgs e)
        {
            gridRiskGroup3.DataSource = null;
            gridRiskGroup3.DataSource = DataDonen(string.Format(@"select r.id,r.Tanım,r.Risk,p.Puan,p.Ceza from {0}.dbo.EDevlet_RiskGrubu_3 r
            left outer join {0}.dbo.EDevlet_RiskGrubu_Puan_3 p on r.id = p.Risk_id
            where r.Parent_id = {1}", Properties.Settings.Default.DbName, cmbAltRisk.EditValue));
        }
    }
}
