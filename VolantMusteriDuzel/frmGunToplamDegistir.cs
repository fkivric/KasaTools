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
    public partial class frmGunToplamDegistir : Form
    {
        string id;
        SqlConnectionObject conn = new SqlConnectionObject();
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        public string donecekDeger1 { get; set; }
        public frmGunToplamDegistir(string _id)
        {
            InitializeComponent();
            id = _id;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string q = string.Format("update SAFECONTROLCENTERDAYSUM set SFCOCEDSSABHCREATED = 1 where SFCOCEDSID = '{0}'",id);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
            this.Dispose();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string q = string.Format("update SAFECONTROLCENTERDAYSUM set SFCOCEDSSABHCREATED = 0 where SFCOCEDSID = '{0}'", id);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            this.DialogResult = DialogResult.Abort;
            this.Close();
            this.Dispose();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string q = string.Format("update SAFECONTROLCENTERDAYSUM set SFCOCEDSSENDCASH = 1 where SFCOCEDSID = '{0}'", id);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string q = string.Format("update SAFECONTROLCENTERDAYSUM set SFCOCEDSSENDCASH = 0 where SFCOCEDSID = '{0}'", id);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            this.DialogResult = DialogResult.Retry;
            this.Close();
            this.Dispose();
        }
    }
}
