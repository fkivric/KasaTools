using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using KrediPuan.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static KrediPuan.Class.Volant;
using KrediPuan.Properties;
using System.Data.SqlClient;
using System.Reflection.Emit;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraEditors.Design;

namespace KrediPuan
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        private string clientName;

        public frmLogin()
        {
            InitializeComponent();
        }
        private List<eDatabase> lDatabase;
        List<Firma> firmas = new List<Firma>();
        private void frmLogin_Load(object sender, EventArgs e)
        {
            VolXml();
            DataCek();
            if (btnNewDatabase.Enabled)
            {
                txtKreidPuanUser.Enabled = false;
                txtKreidPuanPass.Enabled = false;
                simpleButton2.Enabled = false;
                navigationPage1.Enabled = false;
                navigationFrame.SelectedPage = navigationPage2;
            }
            DataTable Compny = Sorgu("select distinct CATALOG_NAME from INFORMATION_SCHEMA.SCHEMATA", Settings.Default.connectionstring);
            cmbVolantSirket.Properties.DataSource = firmas;// Compny;
            cmbVolantSirket.Properties.DisplayMember = "COMPANYNAME";
            cmbVolantSirket.Properties.ValueMember = "COMPANYDB";
            cmbVolantSirket.EditValue = Compny.Rows[0]["CATALOG_NAME"];            
        }
        private void VolXml()
        {
            string ConStrg = "";
            try
            {
                clientName = SystemInformation.ComputerName;
                if (!File.Exists("C:\\Program Files (x86)\\Volant Yazılım\\Volant Erp Setup\\VolErpConnection.xml"))
                {
                    throw new Exception("VolErpConnection Dosyası Eksik!");
                }
                XmlTextReader reader = new XmlTextReader("C:\\Program Files (x86)\\Volant Yazılım\\Volant Erp Setup\\VolErpConnection.xml");
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element && reader.Name == "PARAMS") || reader.NodeType != XmlNodeType.Element || !(reader.Name == "DB"))
                    {
                        continue;
                    }
                    eDatabase rDatabase = new eDatabase();
                    try
                    {
                        ConStrg = string.Format("Server={0};Database={1};User Id={2};Password={3};",
                        reader.GetAttribute("SERVERNAME").TextSifreCoz(),
                        reader.GetAttribute("DATABASE").TextSifreCoz(),
                        reader.GetAttribute("LOGIN").TextSifreCoz(),
                        reader.GetAttribute("PASSWORD").TextSifreCoz());
                    }
                    catch
                    {
                        ConStrg = string.Format("Server={0};Database={1};User Id={2};Password={3};",
                        reader.GetAttribute("SERVERNAME").ToString(),
                        reader.GetAttribute("DATABASE").ToString(),
                        reader.GetAttribute("LOGIN").ToString(),
                        reader.GetAttribute("PASSWORD").ToString());
                    }

                    Settings.Default.Company = reader.GetAttribute("DATABASE").ToString();
                }
                Settings.Default.connectionstring = ConStrg;
                Settings.Default.Save();

                reader.Close();
            }
            catch (Exception exp)
            {
                XtraMessageBox.Show(exp.Message);
            }
        }
        void DataCek()
        {
            List<AllDataBase> allDatas = new List<AllDataBase>() ;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                connection.Open();

                DataTable databases = connection.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    AllDataBase dts = new AllDataBase { DbNAme = database["database_name"].ToString() };
                    allDatas.Add(dts);
                    string databaseName = database["database_name"].ToString();
                    if (databaseName == cmbDatabase.Text || databaseName == txtKrediPuanDbName.Text)
                    {
                        cmbDatabase.Properties.DataSource = databases;
                        cmbDatabase.Properties.DisplayMember = "database_name";
                        cmbDatabase.Properties.ValueMember = "dbid";
                        cmbDatabase.EditValue = database["dbid"];
                        btnNewDatabase.Enabled = false;
                        txtKreidPuanUser.Enabled = true;
                        txtKreidPuanPass.Enabled = true;
                        simpleButton2.Enabled = true;
                        navigationPage1.Enabled = true;
                        tablePanel4.Rows[0].Visible = true;
                        tablePanel4.Rows[1].Visible = false;
                        tablePanel4.Rows[2].Visible = false;
                        tablePanel4.Rows[3].Visible = true;
                        tablePanel4.Rows[4].Visible = true;
                        Settings.Default.DbName = cmbDatabase.Text;
                        Settings.Default.Save();
                    }
                    else
                    {
                        tablePanel4.Rows[0].Visible = false;
                        tablePanel4.Rows[1].Visible = true;
                        tablePanel4.Rows[2].Visible = true;
                        tablePanel4.Rows[3].Visible = false;
                        tablePanel4.Rows[4].Visible = false;
                    }
                    var dd = Sorgu(string.Format("select COMPANYVAL,COMPANYNAME from {0}.dbo.COMPANY", database["database_name"]), Settings.Default.connectionstring);
                    if (dd != null)
                    {
                        var ff = new Firma();
                        ff.COMPANYNAME = dd.Rows[0]["COMPANYNAME"].ToString();
                        ff.COMPANYDB = database["database_name"].ToString();
                        if (!firmas.Any(f => f.COMPANYNAME == ff.COMPANYNAME))
                        {
                            firmas.Add(ff);
                        }
                    }
                }
                connection.Close();
            }
        }
        void FirmaBilgileri()
        {
            cmbVolantMagaza.Properties.DataSource = null;
            DataTable Divison = Sorgu("select DIVVAL,DIVNAME from DIVISON where DIVSTS = 1 and DIVSALESTS = 1", Settings.Default.connectionstring);
            cmbVolantMagaza.Properties.DataSource = Divison;
            cmbVolantMagaza.Properties.DisplayMember = "DIVNAME";
            cmbVolantMagaza.Properties.ValueMember = "DIVVAL";
            cmbVolantMagaza.EditValue = Divison.Rows[0]["DIVVAL"];
            var ftp = Sorgu("select MTFTPIP,MTFTPUSER,MTFTPPASSWORD from MANAGEMENT", Settings.Default.connectionstring);
            Properties.Settings.Default.VolFtpHost = ftp.Rows[0]["MTFTPIP"].ToString();
            Properties.Settings.Default.VolFtpUser = ftp.Rows[0]["MTFTPUSER"].ToString();
            Properties.Settings.Default.VolFtpPass = ftp.Rows[0]["MTFTPPASSWORD"].ToString();
            Properties.Settings.Default.Save();
        }
        private void cmbVolantSirket_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Default.connectionstring = Settings.Default.connectionstring.Replace(Settings.Default.Company.ToString(), cmbVolantSirket.EditValue.ToString());
            Settings.Default.Company = cmbVolantSirket.EditValue.ToString();
            Settings.Default.Save();
            FirmaBilgileri();
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            var ss = tileBarGroupTables.Items.IndexOf(e.Item);
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
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
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var yetki = Sorgu(string.Format(@"select SONAME +SPACE(1)+SOSURNAME as SOCNAME, SOADMIN,
                case 
	                when POTVAL = 'hasPotency_ToSeeSatisOnayMerkezi' then  'Kredi İstihbarat Modül - Satış Onay Merkezi'
	                when POTVAL = 'hasPotency_ToConfirmSalesConfirm' then  'Satış Onay Merkezi - Satış Onaylama' 
	                when POTVAL = 'hasPotency_ToNotConfirmSalesConfirm' then  'Satış Onay Merkezi - Satış Red Etme' 
	                when POTVAL = 'hasPotency_ToInsertSalesConfirm ' then 'Satış Onay Merkezi - Satış Onay Notu Oluşturma' 
	                when POTVAL = 'hasPotency_ToPrintSalesConfirm' then  'Satış Onay Merkezi - Satış Onay Yazdırma' 
	                when POTVAL = 'hasPotency_ToInsertInvestigationSalesConfirm' then  'Satış Onay Merkezi - Araştırma Talebi Oluşturma' 
	                when POTVAL = 'hasPotency_ToIstisareOlusturSalesConfirm' then  'Satış Onay Merkezi - İstişare Oluşturma' end as POTVAL
                ,POTSTS
                from SOCIAL 
                left outer join POTENCY ON SODEPART=POTDEPART
                where SOCODE = '{0}' and SOENTERKEY = '{1}' and SOSTS = 1
                AND (POTVAL in ('hasPotency_ToSeeSatisOnayMerkezi') or POTVAL like '%SalesConfirm')", txtVolantUser.Text,txtVolantPassword.Text), Settings.Default.connectionstring);

            if (yetki.Rows.Count > 0)
            {
                if (bool.Parse(yetki.Rows[0]["SOADMIN"].ToString()))
                {
                    Settings.Default.SatışOnayGiris = true;
                    Settings.Default.SatışOnaylama = true;
                    Settings.Default.SatışRedEtme = true;
                    Settings.Default.SatışOnayNotu = true;
                    Settings.Default.SatışOnayYazdırma = true;
                    Settings.Default.SatışAraştırmaTalebiOluşturma = true;
                    Settings.Default.SatışİstişareOluşturma = true;
                    Settings.Default.Save();
                    this.Hide();
                    frmMain main = new frmMain();
                    main.ShowDialog();
                    this.Close();
                }
                else if (Settings.Default.SatışOnayGiris)
                {
                    Settings.Default.SatışOnayGiris = bool.Parse(yetki.Rows[0]["POTSTS"].ToString());
                    Settings.Default.SatışOnaylama = bool.Parse(yetki.Rows[1]["POTSTS"].ToString());
                    Settings.Default.SatışRedEtme = bool.Parse(yetki.Rows[2]["POTSTS"].ToString());
                    Settings.Default.SatışOnayNotu = bool.Parse(yetki.Rows[3]["POTSTS"].ToString());
                    Settings.Default.SatışOnayYazdırma = bool.Parse(yetki.Rows[4]["POTSTS"].ToString());
                    Settings.Default.SatışAraştırmaTalebiOluşturma = bool.Parse(yetki.Rows[5]["POTSTS"].ToString());
                    Settings.Default.SatışİstişareOluşturma = bool.Parse(yetki.Rows[6]["POTSTS"].ToString());
                    Settings.Default.Save();
                    this.Hide();
                    frmMain main = new frmMain();
                    main.ShowDialog();
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show("Giriş Yetkiniz Yok");
                }
            }
            else
            {
                XtraMessageBox.Show("Giriş Bilgilerinizi Kontrol Ediniz");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtVolantPassword_Enter(object sender, EventArgs e)
        {
            if (txtVolantPassword.Text == "Parola")
            {
                txtVolantPassword.Properties.PasswordChar = '*';
                txtVolantPassword.ForeColor = SystemColors.WindowText;
                txtVolantPassword.Text = "";
            }
        }

        private void txtVolantPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(null, null);
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
                Application.Exit();
            }

        }

        private void txtVolantPassword_Leave(object sender, EventArgs e)
        {
            if (txtVolantPassword.Text.Length == 0)
            {
                txtVolantPassword.Properties.PasswordChar = '\0';
                txtVolantPassword.Text = "Parola";
                txtVolantPassword.ForeColor = SystemColors.GrayText;
            }
        }

        private void btnNewDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Path.GetDirectoryName(Application.ExecutablePath);
                // SQL.script dosyasını okuyun
                string scriptContent = File.ReadAllText("NewKrediPuan.sql");
                using (SqlConnection con = new SqlConnection(Settings.Default.connectionstring))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USE master\r\nCreate Database KrediPuan\r\n",con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                using (SqlConnection connection = new SqlConnection(Settings.Default.connectionstring))
                {
                    connection.Open();

                    // SQL sorgusunu çalıştırın
                    using (SqlCommand command = new SqlCommand(scriptContent, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show(txtKrediPuanDbName.Text + " başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Settings.Default.DbName = txtKrediPuanDbName.Text;
                    Settings.Default.Save();
                    connection.Close(); 
                }

                DataCek();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settings.Default.DbName = "";
                Settings.Default.Save();
            }
            //txtKrediPuanDbName.Text;
        }

        private void txtVolantUser_TextChanged(object sender, EventArgs e)
        {
            var sonuc = Sorgu("select SONAME +SPACE(1)+SOSURNAME from SOCIAL left outer join DEPARTMENT on DEPVAL = SODEPART where SOCODE = '" + txtVolantUser.Text + "'", Settings.Default.connectionstring);
            if (sonuc.Rows.Count > 0)
            {
                togsKullanici.IsOn = true; 
                togsKullanici.Properties.OnText = sonuc.Rows[0][0].ToString();
            }
            else
            {
                togsKullanici.IsOn = false;
            }
        }
    }
}