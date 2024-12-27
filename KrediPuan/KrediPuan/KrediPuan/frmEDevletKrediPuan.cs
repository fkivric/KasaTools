using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using DevExpress.XtraEditors;
using KrediPuan.Class;
using KrediPuan.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrediPuan
{
    public partial class frmEDevletKrediPuan : DevExpress.XtraEditors.XtraForm
    {
        string CURID;
        string Uyeid;
        string SipID;
        string kimlikresmi = "";
        string Portreresmi = "";
        public frmEDevletKrediPuan(string _id)
        {
            InitializeComponent();
            CURID = _id;
            dteSonAy.Value = DateTime.Now.AddMonths(-2);
            dteSonAy.Format = DateTimePickerFormat.Custom;
            dteSonAy.CustomFormat = "MMMM";
            dteSonAy.ShowUpDown = true;
            Kontrol(CURID);
        }

        private string ftpHost = Properties.Settings.Default.VolFtpHost; //"ftp://212.174.235.106:1025";// "ftp://212.174.235.106:1025";
        private string ftpUsername = Properties.Settings.Default.VolFtpUser;
        private string ftpPassword = Properties.Settings.Default.VolFtpPass;
        SqlConnection sql = new SqlConnection(Properties.Settings.Default.connectionstring);
        SqlConnection sql2 = new SqlConnection(Properties.Settings.Default.connectionstring2);
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        List<string> sorgu = new List<string> { "Var", "Yok" };
        DataTable kayitli = new DataTable();
        private void frmEDevletKrediPuan_Load(object sender, EventArgs e)
        {

            //tablePanel1.SetCell(btnKaydet, 7, 2);
            string folderPath = CURID;
            var qs = string.Format("select CURID,CURVAL,CURNAME,CUPIDENTITY,CUPPORTRAIT from CURRENTS " +
                "left outer join CUSTOMERPICTURE on CUPCURID = CURID " +
                "where CURID = '{0}'", folderPath);
            var kimlik = ExecuteQuery(Properties.Settings.Default.connectionstring, qs);
            if (kimlik.Rows.Count > 0)
            {
                kimlikresmi = kimlik.Rows[0]["CUPIDENTITY"].ToString();
                Portreresmi = kimlik.Rows[0]["CUPPORTRAIT"].ToString();
                pictureEdit1.Image = Convert.FromBase64String(kimlikresmi).byteArrayToImage();
                pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                // pictureEdit1.Size = new Size(300, 300);

                pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;

                flyoutPanel1.OwnerControl = this;
                flyoutPanel1.Size = new Size(500, 500);
                flyoutPanel1.Options.Location = new Point(238, 238);
                flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Manual;
                flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            }

        }

        int zoomFacet = 400;
        private void PictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!flyoutPanel1.IsPopupOpen)
                flyoutPanel1.ShowPopup();
            Point offsetLocation = pictureEdit1.ViewportToImage(e.Location);

            offsetLocation.X -= zoomFacet / 2;
            offsetLocation.Y -= zoomFacet / 2;

            offsetLocation.X = offsetLocation.X + zoomFacet > pictureEdit1.Image.Width
                ? pictureEdit1.Image.Width - zoomFacet : offsetLocation.X;
            offsetLocation.X = offsetLocation.X < 0 ? 0 : offsetLocation.X;
            offsetLocation.Y = offsetLocation.Y + zoomFacet > pictureEdit1.Image.Height
                ? pictureEdit1.Image.Height - zoomFacet : offsetLocation.Y;
            offsetLocation.Y = offsetLocation.Y < 0 ? 0 : offsetLocation.Y;

            pictureEdit2.Image = cropImage(pictureEdit1.Image,
                new Rectangle(offsetLocation, new Size(zoomFacet, zoomFacet)));
        }

        private void PictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            flyoutPanel1.HidePopup();
        }

        private void PictureEdit1_MouseEnter(object sender, EventArgs e)
        {
            flyoutPanel1.ShowPopup();
        }
        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics g = Graphics.FromImage(bmpImage))
            {
                g.DrawImage(img, new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), cropArea, GraphicsUnit.Pixel);
            }
            return bmpImage;
        }

        public void Kontrol(string folderPath)
        {
            try
            {
                // Klasörün var olup olmadığını kontrol et
                if (CheckFtpFolderExists(folderPath))
                {
                    var q = string.Format(@"select CURNAME,CURCHADR2,DREGIONVAL,CURCHADR1,DCOUNTYVAL,CURCHCOUNTY,DCITYVAL,CURCHCITY,CUSIDTCNO,CUSIDBIRTHDAY 
                    from CURRENTS
                    LEFT OUTER JOIN CURRENTSCHILD ON CURCHID=CURID
                    LEFT OUTER JOIN CUSIDENTITY ON CUSIDCURID=CURID
                    LEFT OUTER JOIN DEFCITY on DCITYNAME = CURCHCITY
                    LEFT OUTER JOIN DEFCOUNTY on DCITYVAL=DCOUNTYCITYVAL and DCOUNTYNAME = CURCHCOUNTY
                    LEFT OUTER JOIN DEFREGION on DREGIONCOUNTYVAL = DCOUNTYVAL and DREGIONNAME = CURCHADR1
                    WHERE CURRENTS.CURID = {0}", CURID);

                    kayitli = ExecuteQuery(Settings.Default.connectionstring, q);
                    cmbIL.Properties.DisplayMember = "DCITYNAME";
                    cmbIL.Properties.ValueMember = "DCITYVAL";
                    cmbIL.Properties.DataSource = ExecuteQuery(Settings.Default.connectionstring, "select DCITYVAL,DCITYNAME from DEFCITY");
                    cmbIL.EditValue = kayitli.Rows[0]["DCITYVAL"];
                    dteDogumTarihi.EditValue = kayitli.Rows[0]["CUSIDBIRTHDAY"].ToString();
                    txtAdres.Text = kayitli.Rows[0]["CURCHADR2"].ToString();
                    txtTC.Text = kayitli.Rows[0]["CUSIDTCNO"].ToString();
                }
                else
                {
                    MessageBox.Show("Klasör bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private void cmbIL_EditValueChanged(object sender, EventArgs e)
        {
            string ILCEsor = string.Format("select DCOUNTYVAL,DCOUNTYNAME from DEFCOUNTY where DCOUNTYCITYVAL = '{0}'", cmbIL.EditValue);            
            cmbILCE.Properties.DisplayMember = "DCOUNTYNAME";
            cmbILCE.Properties.ValueMember = "DCOUNTYVAL";
            cmbILCE.Properties.DataSource = ExecuteQuery(Settings.Default.connectionstring, ILCEsor);
            cmbILCE.EditValue = kayitli.Rows[0]["DCOUNTYVAL"];
        }
        private void cmbILCE_EditValueChanged(object sender, EventArgs e)
        {
            string MAHsor = string.Format("select DREGIONVAL,DREGIONNAME from DEFREGION where DREGIONCOUNTYVAL = '{0}' order by DREGIONNAME", cmbILCE.EditValue);           
            cmbMAHALLE.Properties.DisplayMember = "DREGIONNAME";
            cmbMAHALLE.Properties.ValueMember = "DREGIONVAL";
            cmbMAHALLE.Properties.DataSource = ExecuteQuery(Settings.Default.connectionstring, MAHsor);
            if (kayitli.Rows[0]["DREGIONVAL"].ToString() != "")
            {
                cmbMAHALLE.EditValue = kayitli.Rows[0]["DREGIONVAL"].ToString();
            }
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
        private bool CheckFtpFolderExists(string folderPath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpHost + "/" + folderPath));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return true; // Klasör var
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false; // Klasör bulunamadı
                }
                else
                {
                    throw; // Diğer hataları fırlat
                }
            }
        }

        private void ListFilesInFtpFolder(string folderPath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpHost + "/" + folderPath));
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                string[] files = reader.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Dosyaları TextListBox içine listele
                Klasörler.Items.Clear();
                Klasörler.Columns.Add("Mevcut Dosyalar");
                foreach (string fileDetails in files)
                {
                    #region Önceki ftp sorgusu
                    // Dosya adını elde etmek için dosya yolunu parçala
                    //string[] fileInfo = fileDetails.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    //string fileName = fileInfo[fileInfo.Length - 1];
                    //Klasörler.Items.Add(fileName);
                    #endregion

                    string[] fileInfo = fileDetails.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string fileName = fileInfo[fileInfo.Length - 1];

                    string[] fileNameParts = fileName.Split('_');
                    string desiredPart = fileNameParts[0];

                    // Özel sınıf kullanarak ListViewItem'ı oluşturun ve özel verileri ekleyin
                    //CustomListItem customItem = new CustomListItem
                    //{
                    //    FileName = desiredPart,
                    //    AdditionalData = fileName // İstediğiniz ek veriyi buraya ekleyebilirsiniz
                    //};

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = desiredPart;
                    listViewItem.Tag = fileName; // Özel veriyi ListViewItem'ın Tag özelliğine ekleyin
                    listViewItem.Name = fileName;

                    Klasörler.Items.Add(listViewItem);
                    foreach (ColumnHeader column in Klasörler.Columns)
                    {
                        Klasörler.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        Klasörler.Width = column.Width;
                    }

                }
                reader.Close();
                response.Close();
            }
        }
        public class CustomListItem
        {
            public string FileName { get; set; }
            public string AdditionalData { get; set; }
        }
        public void PDF(String uri, String username, String password)
        {
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential(username, password);
            byte[] imageByte = ftpClient.DownloadData(uri);


            var tempFileName = Path.GetTempFileName().Replace("tmp", "pdf");
            System.IO.File.WriteAllBytes(tempFileName, imageByte);

            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles);
            pdfViewer.LoadDocument(tempFileName);
        }

        private byte[] DownloadFileFromFtp(string fileName)
        {
            // FTP sunucusundan dosyayı indirin
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpHost + "/" + fileName));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        ftpStream.CopyTo(stream);
                    }
                    return stream.ToArray();
                }
            }
        }

        private void Klasörler_DoubleClick(object sender, EventArgs e)
        {
            string selectedFileName = Klasörler.SelectedItems[0].Name.ToString();// SubItems[0].Text.ToString();

            // Tam FTP yolu oluşturun
            string fullFtpPath = ftpHost + "/" + CURID + "/" + selectedFileName;
            PDF(fullFtpPath, ftpUsername, ftpPassword);
        }
        private void checkEdit9_CheckedChanged(object sender, EventArgs e)
        {
            if (chkicraAcik.Checked)
            {
                cmbicraAcik1.Properties.DataSource = numbers;
                cmbicraAcik2.Properties.DataSource = numbers;
                chkicraAcik.ForeColor = Color.Black;
                tablePanel13.Visible = true;
                chkicraAcik.Text = "Açık İcra VAR";
            }
            else
            {
                cmbicraAcik1.Properties.DataSource = null;
                cmbicraAcik2.Properties.DataSource = null;
                chkicraAcik.ForeColor = Color.Green;
                tablePanel13.Visible = false;
                chkicraAcik.Text = "Açık İcra Yok";
            }
        }

        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkicraKapali.Checked)
            {
                cmbicraKapali1.Properties.DataSource = numbers;
                cmbicraKapali2.Properties.DataSource = numbers;
                chkicraKapali.ForeColor = Color.Black;
                tablePanel9.Visible = true;
                chkicraKapali.Text = "Kapalı İcra VAR";
            }
            else
            {
                cmbicraKapali1.Properties.DataSource = null;
                cmbicraKapali2.Properties.DataSource = null;
                chkicraKapali.ForeColor = Color.Green;
                tablePanel9.Visible = false;
                chkicraKapali.Text = "Kapalı İcra Yok";
            }
        }

        private void checkEdit8_CheckedChanged(object sender, EventArgs e)
        {
            if (chkicraGizli.Checked)
            {
                cmbicraGizli1.Properties.DataSource = numbers;
                cmbicraGizli2.Properties.DataSource = numbers;
                chkicraGizli.ForeColor = Color.Black;
                tablePanel11.Visible = true;
                chkicraGizli.Text = "Gizli İcra VAR";
            }
            else
            {
                cmbicraGizli1.Properties.DataSource = null;
                cmbicraGizli2.Properties.DataSource = null;
                chkicraGizli.ForeColor = Color.Green;
                tablePanel11.Visible = false;
                chkicraGizli.Text = "Gizli İcra Yok";
            }
        }
        private void chkAdres_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdres.Checked)
            {
                chkAdres.ForeColor = Color.Green;
                chkAdres.Text = "Adres Bilgisi UYUŞUYOR....!!";
            }
            else
            {
                chkAdres.ForeColor = Color.Black;
                chkAdres.Text = "E-Devlet İle Adres Farklı";
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkkSGK.Checked)
            {
                chkkSGK.ForeColor = Color.Green;
                chkkSGK.Text = "SGK Var";
                tablePanel15.Visible = true;
                lblSGKMeslekAdı.Visible = true;
                lblSGKMeslekAdı.Text = "Meslek Adı";
            }
            else
            {
                txtSGKTopSure.Text = "";
                txtSGKsongiris.Text = "";
                txtSGKsonPrim.Text = "";
                dteSonAy.Text = "";
                txtSGKYılGun.Text = "";
                txtSGKYılPrim.Text = "";
                txtSGKMeslekKodu.Text = "";
                lblSGKMeslekAdı.Text = "";
                chkkSGK.ForeColor = Color.Black;
                chkkSGK.Text = "SGK Bilgisi Yok";
                tablePanel15.Visible = false;
                lblSGKMeslekAdı.Visible = false;
            }
        }

        private void chkTapu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTapu.Checked)
            {
                cmbMesken.Properties.DataSource = numbers;
                cmbTasinmaz.Properties.DataSource = numbers;
                cmbMulk.Properties.DataSource = numbers;
                chkTapu.ForeColor = Color.Green;
                chkTapu.Text = "Tapu Bilgisi Var";
                cmbMesken.Visible = true;
                cmbTasinmaz.Visible = true;
                labelControl15.Visible = true;
                labelControl28.Visible = true;
            }
            else
            {
                cmbMesken.Properties.DataSource = null;
                cmbTasinmaz.Properties.DataSource = null;
                cmbMulk.Properties.DataSource = null;
                chkTapu.ForeColor = Color.Black;
                chkTapu.Text = "Tapu Bilgisi Yok";
                cmbMesken.Visible = false;
                cmbTasinmaz.Visible = false;
                labelControl15.Visible = false;
                labelControl28.Visible = false;
            }
        }


        private void chkArac_CheckedChanged(object sender, EventArgs e)
        {

            if (chkArac.Checked)
            {
                cmbArac.Properties.DataSource = numbers;
                chkArac.ForeColor = Color.Green;
                chkArac.Text = "Araç Bilgisi Var";
                cmbArac.Visible = true;
                labelControl13.Visible = true;
            }
            else
            {
                cmbArac.Properties.DataSource = null;
                chkArac.ForeColor = Color.Black;
                chkArac.Text = "Araç Bilgisi Yok";
                cmbArac.Visible = false;
                labelControl13.Visible = false;
            }
        }
        private void checkEmekli_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmekli.Checked)
            {
                chkEmekli.ForeColor = Color.Green;
                chkEmekli.Text = "Emekli Aylığı Var";
                txtEmekliAylik.Enabled = true;
                txtEmekliAylik.Visible = true;
                lblEmekliMaas.Visible = true;
            }
            else
            {
                txtEmekliAylik.Text = "";
                chkEmekli.ForeColor = Color.Black;
                chkEmekli.Text = "Emekli Değil";
                txtEmekliAylik.Visible = false;
                lblEmekliMaas.Visible = false;
            }
        }

        private void checkDava_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDava.Checked)
            {
                cmbDava1.Properties.DataSource = numbers;
                cmbDava1.Properties.DataSource = numbers;
                chkDava.ForeColor = Color.Black;
                chkDava.Text = "Adına Dava Var";
                tablePanel14.Visible = true;
            }
            else
            {
                cmbDava1.Properties.DataSource = null;
                cmbDava1.Properties.DataSource = null;
                chkDava.ForeColor = Color.Green;
                chkDava.Text = "Dava Yok";
                tablePanel14.Visible = false;
            }
        }

        private void txtSGKTopSure_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtSGKsongiris_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtSGKsonPrim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtSGKYılGun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtSGKYılPrim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtEmekliAylik_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            bool icraAcik = false;
            bool icraKapali = false;
            bool icraGizli = false;
            bool SSK = false;
            bool Tapu = false;
            bool Dava = false;
            bool Emekli = false;
            bool Adress = false;
            bool Arac = false;
            DateTime DogumTarihi = DateTime.Now;
            if (chkicraAcik.Checked == true)
            {
                icraAcik = true;
            }
            if (chkicraKapali.Checked == true)
            {
                icraKapali = true;
            }
            if (chkicraGizli.Checked == true)
            {
                icraGizli = true;
            }
            if (chkkSGK.Checked == true)
            {
                SSK = true;
            }
            if (chkTapu.Checked == true)
            {
                Tapu = true;
            }
            if (chkArac.Checked == true)
            {
                Arac = true;
            }
            if (chkDava.Checked == true)
            {
                Dava = true;
            }
            if (chkEmekli.Checked == true)
            {
                Emekli = true;
            }
            if (chkAdres.Checked == true)
            {
                Adress = true;
            }
            if (txtDogumTarihi.Text != "")
            {
                DogumTarihi = Convert.ToDateTime(txtDogumTarihi.Text);
            }
            else
            {
                DogumTarihi = Convert.ToDateTime(dteDogumTarihi.EditValue);
            }
            string q = string.Format("insert into EDevlet_MusteriDurum values ('{0}',	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	'{9}',	'{10}',	'{11}',	'{12}',	'{13}',	'{14}',	'{15}',	'{16}',	'{17}',	'{18}',	'{19}',	'{20}',	'{21}',	'{22}',	'{23}',	'{24}',	'{25}',	'{26}',	'{27}',	'{28}',	'{29}',	'{30}',	'{31}','{32}','{33}'", CURID, Uyeid, SipID, DateTime.Now, Adress, DogumTarihi, icraAcik, cmbicraAcik1.EditValue, cmbicraAcik2.EditValue, icraKapali, cmbicraKapali1.EditValue, cmbicraKapali2.EditValue, icraGizli, cmbicraGizli1.EditValue, cmbicraGizli2.EditValue, SSK, txtSGKTopSure.Text, txtSGKsongiris.Text, txtSGKsonPrim.Text, dteSonAy.Text, txtSGKYılGun.Text, txtSGKYılPrim.Text, txtSGKMeslekKodu.Text, Tapu, cmbMesken.EditValue, cmbTasinmaz.EditValue, cmbMulk.EditValue, Arac, cmbArac.EditValue, Dava, cmbDava1.EditValue, cmbDava2.EditValue, Emekli, txtEmekliAylik.Text);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
        }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            bool icraAcik = false;
            bool icraKapali = false;
            bool icraGizli = false;
            bool SSK = false;
            bool Tapu = false;
            bool Dava = false;
            bool Emekli = false;
            bool Adress = false;
            bool Arac = false;
            DateTime DogumTarihi = DateTime.Now;
            if (chkicraAcik.Checked == true)
            {
                icraAcik = true;
            }
            if (chkicraKapali.Checked == true)
            {
                icraKapali = true;
            }
            if (chkicraGizli.Checked == true)
            {
                icraGizli = true;
            }
            if (chkkSGK.Checked == true)
            {
                SSK = true;
            }
            if (chkTapu.Checked == true)
            {
                Tapu = true;
            }
            if (chkArac.Checked == true)
            {
                Arac = true;
            }
            if (chkDava.Checked == true)
            {
                Dava = true;
            }
            if (chkEmekli.Checked == true)
            {
                Emekli = true;
            }
            if (chkAdres.Checked == true)
            {
                Adress = true;
            }
            if (txtDogumTarihi.Text != "")
            {
                DogumTarihi = Convert.ToDateTime(txtDogumTarihi.Text);
            }
            else
            {
                DogumTarihi = Convert.ToDateTime(dteDogumTarihi.EditValue);
            }
            string q = string.Format("update EDevlet_MusteriDurum set Adress = '{0}',DogumTarihi = '{1}',icraAcik = '{2}',icraAcikOnemli = '{3}',icraAcikOnemsiz = '{4}',icraKapali = '{5}',icraKapaliOnemli = '{6}',icraKapaliOnemsiz = '{7}',icraGizli = '{8}',icraGizliOnemli = '{9}',icraGizliOnemsiz = '{10}',SSK = '{11}',SGKTopSure = '{12}',SGKsongiris = '{13}',SGKsonPrim = '{14}',dteSonAy = '{15}',SGKYılGun = '{16}',SGKYılPrim = '{17}',Tapu = '{18}',Mesken = '{19}',Tasinmaz = '{20}',Mulk = '{21}',Arac = '{22}',AracSayi = '{23}',Dava = '{24}',DavaOnemli = '{25}',DavaOnemsiz = '{26}',Emekli = '{27}',EmekliAylik = '{28}' where Musteri_VolID = '{29}',Musteri_TicID = '{30}',Musteri_SipID = '{31}'", CURID, Uyeid, SipID, Adress, DogumTarihi, icraAcik, cmbicraAcik1.EditValue, cmbicraAcik2.EditValue, icraKapali, cmbicraKapali1.EditValue, cmbicraKapali2.EditValue, icraGizli, cmbicraGizli1.EditValue, cmbicraGizli2.EditValue, SSK, txtSGKTopSure.Text, txtSGKsongiris.Text, txtSGKsonPrim.Text, dteSonAy.Text, txtSGKYılGun.Text, txtSGKYılPrim.Text, Tapu, cmbMesken.EditValue, cmbTasinmaz.EditValue, cmbMulk.EditValue, Arac, cmbArac.EditValue, Dava, cmbDava1.EditValue, cmbDava2.EditValue, Emekli, txtEmekliAylik.Text);
            SqlCommand cmd = new SqlCommand(q, sql);
            sql.Open();
            var sonuc = cmd.ExecuteNonQuery();
            sql.Close();
        }
        private void btnYeni_Click(object sender, EventArgs e)
        {
            dteSonAy.Value = DateTime.Now.AddMonths(-2);
            dteSonAy.Format = DateTimePickerFormat.Custom;
            dteSonAy.CustomFormat = "MMMM";
            dteSonAy.ShowUpDown = true;
            Kontrol(CURID);
        }

        private void txtMeslekKodu_EditValueChanged(object sender, EventArgs e)
        {
            if (txtSGKMeslekKodu.Text != "")
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from KrediPuan.dbo.MeslekKodu where KODU like '%" + txtSGKMeslekKodu.Text + "%'", sql);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lblSGKMeslekAdı.Text = dt.Rows[0][1].ToString();
                }
            }
        }

        private void cmbicraAcik1_EditValueChanged(object sender, EventArgs e)
        {

        }

    }
}