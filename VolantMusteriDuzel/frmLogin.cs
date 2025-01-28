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
using static VolantMusteriDuzel.Class.Volant;
using VolantMusteriDuzel.Properties;
using System.Data.SqlClient;
using System.Reflection.Emit;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraEditors.Design;
using Microsoft.Win32;
using System.Diagnostics;
using VolantMusteriDuzel.Class;
using static VolantMusteriDuzel.Class.Entegref;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Globalization;

namespace VolantMusteriDuzel
{

    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        private string clientName;
        public static string userID;

        public frmLogin()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://lisans.entegref.com/");
            if (VKN == null)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EntegrefWeb");
                if (!string.IsNullOrEmpty(key.GetValue("ApplicationVKN").ToString()))
                {
                    VKN = key.GetValue("ApplicationVKN").ToString();
                }
                else
                {
                    VKN = Properties.Settings.Default.VKN;
                }
            }
        }
        private List<eDatabase> lDatabase;
        List<Firma> firmas = new List<Firma>();
        string version;
        string ProductName = "";
        public static string VKN = null;
        private readonly HttpClient httpClient;

        SqlConnection Entgref = new SqlConnection("Server=31.145.19.56;Database=Netbil_Connector; User ID=fatih;Password=05101981;");
        SqlConnectionObject conn = new SqlConnectionObject();
        private async void frmLogin_LoadAsync(object sender, EventArgs e)
        {
            ProductName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name.ToString(); // proje adı
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
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                lblversion.Text = "Version : " + ad.CurrentVersion.Major + "." + ad.CurrentVersion.Minor + "." + ad.CurrentVersion.Build + "." + ad.CurrentVersion.Revision;
                version = ad.CurrentVersion.Revision.ToString();
            }
            else
            {
                string _s1 = Application.ProductVersion; // versiyon
                lblversion.Text = "Version : " + _s1;
                version = _s1;
            }
            RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EntegrefTools");
            if (key2.GetValue("ComputerLisansingID") == null)
            {
                //CustomMessageBox.ShowMessage("Lütfen Bekleyin: ", "Propgram Lisanslanıyor", this, "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var pcİsmi = key2.GetValue("ComputerName");
                var pcModeli = key2.GetValue("ComputerID");
                var Cpuid = key2.GetValue("CPU");
                var Motherboardid = key2.GetValue("motherboardid");
                var vknvar = key2.GetValue("ApplicationVKN");
                Entegref client = new Entegref();
                string response = await client.UpdateLicensingUser(vknvar.ToString(), pcİsmi.ToString(), pcModeli.ToString(), version, ProductName);
                //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseData);
                List<Sonuc> myDeserializedClass = JsonConvert.DeserializeObject<List<Sonuc>>(response);
                var ConnectionLisansingID = myDeserializedClass[0].message;
                string response2 = await client.UpdateLicensing(vknvar.ToString(), ConnectionLisansingID.ToString(), Cpuid.ToString(), Motherboardid.ToString(), ProductName);
                List<Sonuc> myDeserializedClass2 = JsonConvert.DeserializeObject<List<Sonuc>>(response2);
                string[] valueNames = key2.GetValueNames();
                if (valueNames.Contains("ApplicationSecretPhase"))
                {
                    string SecretPhase = key2.GetValue("ApplicationSecretPhase").ToString();

                    if (string.IsNullOrEmpty(SecretPhase))
                    {
                        Lisansing(key2.GetValue("ApplicationVKN").ToString());
                    }
                }
                else
                {
                    Lisansing(key2.GetValue("ApplicationVKN").ToString());
                }
                key2.SetValue("ComputerLisansingID", myDeserializedClass2[0].message);
                txtVolantUser.Focus();
            }
            else
            {
                int new_version = int.Parse(conn.QueryEntegref($"select Version from Entegref_Main.dbo.Versiyon_Other where AppName = '{ProductName}'", Entgref).Replace(".", ""));
                int last_version = int.Parse(version.Replace(".", ""));
                if (new_version > last_version)
                {
                    this.Enabled = false;
                    try
                    {
                        string pathToUpdater = @"Kasa Update.exe"; // updater.exe dosyasının adını belirtin

                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = pathToUpdater,
                            WindowStyle = ProcessWindowStyle.Normal // İsteğe bağlı: Pencere stili
                        };

                        Process.Start(startInfo);
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.ShowMessage("Güncelleme Var Hata = " + ex.Message, "Güncelleme var Oto güncelleme Çalışmadı! Elle güncelleyiniz.\r\n Seçenekler = \r\n 1-) C:\\Program Files (x86)\\Entegref Yazılım Tic. Ltd.Şti\\EntegreF Connector\\updater.exe dosya yoluna gidip güncelleme programını çalıştırınız.\r\n 2-) Başlat butonundan Updater aratarak çıkanı çalıştırınız", this, "Güncelleme Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Enabled = true;
                    }
                }
                txtVolantUser.Focus();
            }
            CultureInfo culture = new CultureInfo("tr-TR");
            bugun = DateTime.Now.ToString("d", culture);
            var secretPhaseValue = key2.GetValue("ApplicationSecretPhase");
            if (secretPhaseValue != null)
            {
                SKGL.Validate validate = new SKGL.Validate();
                validate.secretPhase = VKN;
                validate.Key = key2.GetValue("ApplicationSecretPhase").ToString();
                txtLisansing2.Text = "Başlangıç Tarihi : \r\n " + validate.CreationDate.ToShortDateString();
                txtLisansing3.Text = "Sona Erme Tarihi : \r\n " + validate.ExpireDate.ToShortDateString();
                txtLisansing1.Text = "Kalan Gün : \r\n" + validate.DaysLeft;
                if (validate.DaysLeft > 2)
                {
                    pnlLisans.Visible = false;
                    this.Size = new Size(718, 325);
                }
            }
            else
            {
                await Task.Delay(10000);
                Lisansing(key2.GetValue("ApplicationVKN").ToString());
                RegistryKey lisans = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EntegreFYonAvmTools");
                SKGL.Validate validate = new SKGL.Validate();
                validate.secretPhase = VKN;
                validate.Key = lisans.GetValue("ApplicationSecretPhase").ToString();
                txtLisansing2.Text = "Başlangıç Tarihi : \r\n " + validate.CreationDate.ToShortDateString();
                txtLisansing3.Text = "Sona Erme Tarihi : \r\n " + validate.ExpireDate.ToShortDateString();
                txtLisansing1.Text = "Kalan Gün : \r\n " + validate.DaysLeft;
                if (validate.DaysLeft > 2)
                {
                    pnlLisans.Visible = false;
                    this.Size = new Size(718, 325);
                }
            }
        }

        DateTime now = DateTime.Now;
        string bugun = "";
        bool yenigun = false;
        private async void Lisansing(string vknid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://lisans.entegref.com/token");
                    //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44371/token");
                    request.Content = new StringContent("grant_type=password&username=admin&password=Madam3169");

                    // İstek başlığına gerekli kimlik doğrulama bilgilerini ekleyin
                    //string clientId = "your_client_id";
                    //string clientSecret = "your_client_secret";
                    //string credentials = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(clientId + ":" + clientSecret));
                    //request.Headers.Add("Authorization", "Basic " + credentials);

                    // İsteği gönderin ve yanıtı alın
                    HttpResponseMessage responses = await client.SendAsync(request);

                    // Yanıtın başarılı olup olmadığını kontrol edin
                    if (responses.IsSuccessStatusCode)
                    {
                        string responseData = await responses.Content.ReadAsStringAsync();
                        Token myDeserializedClass = JsonConvert.DeserializeObject<Token>(responseData);
                        Properties.Settings.Default.EntegrefAIPToken = myDeserializedClass.access_token;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show("API isteği başarısız: " + responses.StatusCode, "Servis Uyarısı");
                    }
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.EntegrefAIPToken);
                HttpResponseMessage response = await httpClient.GetAsync($"api/data/Lisans?VKN={vknid}&AppName={ProductName}");
                string Timer;
                HttpResponseMessage time = await httpClient.GetAsync("api/data/forall");
                Timer = await time.Content.ReadAsStringAsync();
                ReturnModel Zaman = JsonConvert.DeserializeObject<ReturnModel>(Timer);
                string originalDateTimeString = Zaman.message;
                //if (bugun == originalDateTimeString)
                //{
                yenigun = true;
                if (Properties.Settings.Default.YeniGun != bugun)
                {
                    yenigun = false;
                }
                else
                {
                    yenigun = true;
                }


                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseData);
                    List<Sonuc> myDeserializedClass = JsonConvert.DeserializeObject<List<Sonuc>>(responseData);
                    foreach (var item in myDeserializedClass)
                    {
                        if (item.status)
                        {
                            SKGL.Validate validate = new SKGL.Validate();
                            validate.secretPhase = VKN;
                            validate.Key = item.message;
                            Properties.Settings.Default.EntegrefSecretPhase = item.message;
                            Properties.Settings.Default.Save();

                            txtLisansing2.Text = "Başlangıç Tarihi : \r\n " + validate.CreationDate.ToShortDateString();
                            txtLisansing3.Text = "Sona Erme Tarihi : \r\n " + validate.ExpireDate.ToShortDateString();
                            txtLisansing1.Text = "Kalan Gün : \r\n " + validate.DaysLeft;
                            //txtLisansing2.Text = "Lisans Başlanğıç Tarihi:" + validate.CreationDate.ToShortDateString() + "\r\n" + "Lisans Sona Erme Tarihi:" + validate.ExpireDate.ToShortDateString() + "\r\n" + "Lisans Kullanım Kalan Gün:" + validate.DaysLeft;

                            var assembly = typeof(Program).Assembly;
                            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                            var id = attribute.Value;
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EntegrefTools");
                            key.SetValue("ApplicationSetupComplate", "true");
                            key.SetValue("ApplicationGUID", id);
                            key.SetValue("ApplicationSecretPhase", item.message);// Properties.Settings.Default.EntegrefSecretPhase);
                            key.Close();
                        }
                        else
                        {
                            MessageBox.Show(item.message, "Dikkat", MessageBoxButtons.YesNo);
                        }
                    }
                }
                else
                {
                    CustomMessageBox.ShowMessage("API isteği başarısız: ", response.Content.ToString(), this, "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //}
                //else
                //{
                //    CustomMessageBox.ShowMessage("Sistem Saati dogru değil kontrol ediniz.", "", this, "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
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
            string company = cmbVolantSirket.EditValue.ToString();
            if (company.Contains("YON"))
            {
                pictureEdit4.Visible = true;
                pictureEdit4.Image = Properties.Resources.YON_AVM_400;
            }
            else if (company.Contains("KAMALAR"))
            {
                pictureEdit4.Visible = true;
                pictureEdit4.Image = Properties.Resources.Kamalar_logo;
            }
            else
            {
                pictureEdit4.Visible = false;
            }
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
            var yetki = Sorgu(string.Format(@"select * from SOCIAL where SOCODE = '{0}' and SOENTERKEY = '{1}' and (SODEPART = '019' or SODEPART = 'ADMIN')", txtVolantUser.Text, txtVolantPassword.Text), Settings.Default.connectionstring);

            if (yetki.Rows.Count > 0)
            {
                userID = yetki.Rows[0][0].ToString();
                this.Hide();
                Form1 main = new Form1();
                main.ShowDialog();
                this.Close();
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