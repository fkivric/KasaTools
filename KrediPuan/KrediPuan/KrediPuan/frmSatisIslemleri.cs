using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrediPuan
{
    public partial class frmSatisIslemleri : DevExpress.XtraEditors.XtraForm
    {
        string CURNAME;
        string CURID;
        string SALID;
        public frmSatisIslemleri(string cURNAME, string cURID, string cSALID)
        {
            InitializeComponent();
            CURNAME = cURNAME;
            CURID = cURID;  
            SALID = cSALID;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            XtraUserControl userControl;
            Control_Satis satis = new Control_Satis(CURID, SALID);
            userControl = CreateUserControlFull(satis, CURNAME);
            tabbedView1.AddDocument(userControl);
            tabbedView1.ActivateDocument(userControl);

        }
        public XtraUserControl CreateUserControlFull(XtraUserControl control, string text)
        {
            control.Name = text.ToLower() + "UserControl";
            control.Text = text;
            LabelControl label = new LabelControl();
            label.Parent = control;
            label.Appearance.Font = new Font("Tahoma", 25.25F);
            label.Appearance.ForeColor = Color.Gray;
            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.AutoSizeMode = LabelAutoSizeMode.None;
            label.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            label.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            label.Text = text;
            return control;
        }

        private void tabbedView1_DocumentClosing(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentCancelEventArgs e)
        {

        }
    }
}