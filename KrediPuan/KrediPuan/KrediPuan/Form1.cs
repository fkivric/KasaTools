using DevExpress.XtraExport.Xls;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KrediPuan.Class;

namespace KrediPuan
{
    public partial class Form1 : Form
    {        
        string kimlikresmi = "";
        string Portreresmi = "";
        string CURID;
        public Form1(string _id)
        {
            InitializeComponent();

            var qs = string.Format("select CURID,CURVAL,CURNAME,CUPIDENTITY,CUPPORTRAIT from CURRENTS " +
                "left outer join CUSTOMERPICTURE on CUPCURID = CURID " +
                "where CURID = '{0}'", _id);
            var kimlik = ExecuteQuery(Properties.Settings.Default.connectionstring, qs);
            if (kimlik.Rows.Count > 0)
            {
                kimlikresmi = kimlik.Rows[0]["CUPIDENTITY"].ToString();
                Portreresmi = kimlik.Rows[0]["CUPPORTRAIT"].ToString();
                pictureEdit1.Image = Convert.FromBase64String(kimlikresmi).byteArrayToImage();
                pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                pictureEdit1.MouseEnter += PictureEdit1_MouseEnter;
                pictureEdit1.MouseMove += PictureEdit1_MouseMove;
                pictureEdit1.MouseLeave += PictureEdit1_MouseLeave;
                pictureEdit1.Size = new Size(300, 300);

                pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;

                flyoutPanel1.OwnerControl = this;
                flyoutPanel1.Size = new Size(700, 700);
                flyoutPanel1.Options.Location = new Point(350, 22);
                flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Manual;
                flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            }
        }
        int zoomFacet = 400;
        private void PictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!flyoutPanel1.IsPopupOpen)
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
        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics g = Graphics.FromImage(bmpImage))
            {
                g.DrawImage(img, new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), cropArea, GraphicsUnit.Pixel);
            }
            return bmpImage;
        }
        //private static Image cropImage(Image img, Rectangle cropArea)
        //{
        //    Bitmap bmpImage = new Bitmap(img);
        //    return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        //}

        private void PictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            flyoutPanel1.HidePopup();
        }

        private void PictureEdit1_MouseEnter(object sender, EventArgs e)
        {
            flyoutPanel1.ShowPopup();
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
        }
}
