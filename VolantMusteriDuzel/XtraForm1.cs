using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using VolantMusteriDuzel.Properties;
using static VolantMusteriDuzel.Class.Volant;

namespace VolantMusteriDuzel
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
        }
        public static bool Emre=false;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true) 
            {
                Emre = true;
            }
            else
            {
                Emre = false;
            };
            if (toggleSwitch1.IsOn == false)
            {
                Properties.Settings.Default.connectionstring = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
                Properties.Settings.Default.connectionstring2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
                Properties.Settings.Default.Save();
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            else
            {
                VolXml();
                Properties.Settings.Default.Save();
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();

            }
        }
        private void VolXml()
        {
            string ConStrg = "";
            string MDE = "";
            try
            {
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
                        MDE = string.Format("Server={0};Database={1};User Id={2};Password={3};",
                        reader.GetAttribute("SERVERNAME").ToString(),
                        "MDE_GENEL",
                        reader.GetAttribute("LOGIN").ToString(),
                        reader.GetAttribute("PASSWORD").ToString());

                    }
                }
                reader.Close();
                Settings.Default.connectionstring = ConStrg;
                Settings.Default.connectionstring2 = MDE;
                Settings.Default.Save();
            }
            catch (Exception exp)
            {
                XtraMessageBox.Show(exp.Message);
            }
        }
    }
}