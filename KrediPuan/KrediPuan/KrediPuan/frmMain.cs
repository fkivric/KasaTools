using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab;
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
    public partial class frmMain : XtraForm
    {
        public frmMain()
        {
            try
            {
                SplashScreen();
                InitializeComponent();
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
                DefaultLookAndFeel defaultLookAndFeel = new DefaultLookAndFeel();
                defaultLookAndFeel.LookAndFeel.SkinName = "McSkin";
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 200, this);
            }
        }

        void SplashScreen()
        {
            FluentSplashScreenOptions splashScreen = new FluentSplashScreenOptions();
            splashScreen.Title = "EntegreF";
            splashScreen.Subtitle = "EntegreF® Volant Kredi Puan Sistemi";
            splashScreen.RightFooter = "Başlıyor...";
            splashScreen.LeftFooter = $"CopyRight ® 2023 {Environment.NewLine} Tüm Hahkları Saklıdır.";
            splashScreen.LoadingIndicatorType = FluentLoadingIndicatorType.Dots;
            splashScreen.OpacityColor = System.Drawing.Color.FromArgb(16, 110, 190);
            splashScreen.Opacity = 90;
            splashScreen.AppearanceLeftFooter.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowFluentSplashScreen(splashScreen, parentForm: this, useFadeIn: true, useFadeOut: true);
        }
        public XtraUserControl CreateUserControlFull(XtraUserControl control,string text)
        {
            control.Name = text.ToLower();
            control.Text = text;
            //LabelControl label = new LabelControl();
            //label.Parent = control;
            //label.Appearance.Font = new Font("Tahoma", 25.25F);
            //label.Appearance.ForeColor = Color.Gray;
            //label.Dock = System.Windows.Forms.DockStyle.Fill;
            //label.AutoSizeMode = LabelAutoSizeMode.None;
            //label.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //label.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            //label.Text = text;
            return control;
        }
        void accordionControl_SelectedElementChanged(object sender, SelectedElementChangedEventArgs e)
        {
            XtraUserControl userControl = new XtraUserControl();
            if (e.Element == null)
            {
                return;
            }
            else
            {
                    if (e.Element.Text == "Mağaza Bazlı Satışlar")
                    {
                        Control_Satislar sts = new Control_Satislar();
                        userControl = CreateUserControlFull(sts, e.Element.Text);
                        //tabbedView.AddDocument(userControl);
                        //tabbedView.ActivateDocument(userControl);
                    }
                    else if (e.Element.Text == "E-Devlet Parametreleri")
                    {
                        Control_RiskGurup ss = new Control_RiskGurup();
                        userControl = CreateUserControlFull(ss, e.Element.Text);
                        //tabbedView.AddDocument(userControl);
                        //tabbedView.ActivateDocument(userControl);
                    }
                    else if (e.Element.Text == "Baraj Puan Tanımlama")
                    {
                        Control_PuanTanımlama _PuanTanımlama = new Control_PuanTanımlama();
                        userControl = CreateUserControlFull(_PuanTanımlama, e.Element.Text);
                        //tabbedView.AddDocument(userControl);
                        //tabbedView.ActivateDocument(userControl);
                    }

                bool var = false;
                for (int i = 0; i < tabbedView.Documents.Count; i++)
                {
                    if (tabbedView.Documents[i].Caption == e.Element.Text)
                    {
                        var = true;
                    }

                }
                if (!var)
                {
                    tabbedView.AddDocument(userControl);
                    tabbedView.ActivateDocument(userControl);
                }
                else
                {
                    //tabbedView.AddDocument(userControl);
                    tabbedView.ActivateDocument(userControl);
                }
            }
        }
        void tabbedView_DocumentClosed(object sender, DocumentEventArgs e)
        {
            RecreateUserControls(e);
            SetAccordionSelectedElement(e);
        }
        void SetAccordionSelectedElement(DocumentEventArgs e)
        {
            if (tabbedView.Documents.Count != 0)
            {
                if (e.Document.Caption == "Employees")
                {
                    accordionControl.SelectedElement = customersAccordionControlElement;
                }
                else
                {
                    accordionControl.SelectedElement = employeesAccordionControlElement;
                }
            }
            else
            {
                accordionControl.SelectedElement = null;
            }
        }
        void RecreateUserControls(DocumentEventArgs e)
        {
            //if (e.Document.Caption == "Employees")
            //{
            //    employeesUserControl = CreateUserControl("Mağaza Bazlı Satışlar");
            //}
            //else
            //{
            //    customersUserControl = CreateUserControl("Customers");
            //}
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz?", "Kapatma Onayı", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Kapatma işlemini iptal et
            }
            else
            {
                System.Environment.Exit(0);
                // Kapatma işlemini devam ettir
                // Gerekli kaynakları temizleyebilir ve diğer işlemleri gerçekleştirebilirsiniz
                Application.Exit(); // Programı kapat
            }
        }
    }
}