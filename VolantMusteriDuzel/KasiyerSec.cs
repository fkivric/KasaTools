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
using VolantMusteriDuzel.Properties;

namespace VolantMusteriDuzel
{
    public partial class KasiyerSec : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection sql = new SqlConnection(Settings.Default.connectionstring);
        SqlConnection MDE = new SqlConnection(Settings.Default.connectionstring2);
        SqlConnectionObject conn = new SqlConnectionObject();
        string id;
        string SABHDATE;
        string DIVVAL;
        string SABHAMOUNT;
        public KasiyerSec(string _id, string _SABHDATE, string _DIVVAL, string _SABHAMOUNT)
        {
            InitializeComponent();
            id = _id;
            SABHDATE = _SABHDATE;
            DIVVAL = _DIVVAL;
            SABHAMOUNT = _SABHAMOUNT;
        }

        private void KasiyerSec_Load(object sender, EventArgs e)
        {
            string q = @"select CHSOCODE,DSAFEID,DSAFEVAL,DSAFENAME,UPPER(isnull(SONAME + ' ' + SOSURNAME,'Merkez Kasa')) as KasiyerAdı from DEFSAFE 
            left outer join CASHIER on DSAFEUNITE = CHSAFEUNI
            left outer join SOCIAL on SOCODE = CHSOCODE 
            where DSAFEDIVISON  ='00' and DSAFEID in (2608,2815,2155)
            order by 4";

            SqlDataAdapter da = new SqlDataAdapter(q, sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            srcKasiyerKasa.Properties.DataSource = dt;
            srcKasiyerKasa.Properties.ValueMember = "DSAFEID";
            srcKasiyerKasa.Properties.DisplayMember = "DSAFENAME";
        }

        internal int Logins(string query)
        {
            SqlCommand cmd = new SqlCommand(query, MDE);
            if (MDE.State == ConnectionState.Closed)
                MDE.Open();
            var sonuc = cmd.ExecuteNonQuery();
            return sonuc;
        }
        internal DataTable Sonuc(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, MDE);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        string magazacikisID;
        string magazagirisID;
        string kasiyercikisID;
        string kasiyergirisID;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var CHSOCODE = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "CHSOCODE").ToString();
            var DSAFEID = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEID").ToString();
            var DSAFEVAL = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEVAL").ToString();
            var DSAFENAME = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFENAME").ToString();

            Form1.CHSOCODE = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "CHSOCODE").ToString();
            Form1.DSAFEID = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEID").ToString();
            Form1.DSAFEVAL = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFEVAL").ToString();
            Form1.DSAFENAME = srcKasiyerKasaView.GetRowCellValue(srcKasiyerKasaView.FocusedRowHandle, "DSAFENAME").ToString();


            var Islem = Sonuc($"select * from SAFEMOVIE where SFCOCEDSID = {id}");
            if (Islem.Rows.Count == 0)
            {
                try
                {
                    string qq = String.Format(@"insert into SAFEMOVIE(SFCOCEDSID,SOCODE,DIVVAL,SABHDATE,SABHAMOUNT) values ('{0}','{1}','{2}','{3}','{4}')", id, CHSOCODE, DIVVAL, Convert.ToDateTime(SABHDATE).ToString("yyyy-MM-dd"), SABHAMOUNT);
                    var kayit = Logins(qq);

                    Dictionary<string, string> SAFE = new Dictionary<string, string>();
                    SAFE.Add("@id", id);
                    SAFE.Add("@SAFEID", "2155");
                    SAFE.Add("@SABHID", "");
                    SAFE.Add("@SABHDIVISON", "");
                    SAFE.Add("@SABHSOURCE", "VI1");
                    SAFE.Add("@SABHSOCODE", "");
                    SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Düzeltme");
                    SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                    SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                    SAFE.Add("@ReturnDesc", "");
                    magazacikisID = conn.InsertBack("FK_KASAILK", SAFE);
                    //Form1.magazacikisID = magazacikisID;
                    string u1 = String.Format($"update SAFEMOVIE set VI1 = '{magazacikisID}' where SFCOCEDSID = '{id}'");
                    var sonuc1 = Logins(u1);

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", magazacikisID);
                    SAFECH.Add("@SAFEID", "2155");
                    SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                    string u2 = String.Format($"update SAFEMOVIE set VI1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                    var sonuc2 = Logins(u2);


                    Dictionary<string, string> SAFE2 = new Dictionary<string, string>();
                    SAFE2.Add("@id", id);
                    SAFE2.Add("@SAFEID", "2155");
                    SAFE2.Add("@SABHID", magazacikisID);
                    SAFE2.Add("@SABHDIVISON", "00");
                    SAFE2.Add("@SABHSOURCE", "VI0");
                    SAFE2.Add("@SABHSOCODE", CHSOCODE);
                    SAFE2.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza Merkez Kasa Giriş");
                    SAFE2.Add("@SABHVERSEVAL", "00.Kasa.TL");
                    SAFE2.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                    SAFE2.Add("@ReturnDesc", "");
                    magazagirisID = conn.InsertBack("FK_KASAILK", SAFE2);
                    string u3 = String.Format($"update SAFEMOVIE set VI0 = '{magazagirisID}' where SFCOCEDSID = '{id}'");
                    var sonuc3 = Logins(u3);

                    Dictionary<string, string> SAFECH2 = new Dictionary<string, string>();
                    SAFECH2.Add("@SABHID", magazagirisID);
                    SAFECH2.Add("@SAFEID", "2155");
                    SAFECH2.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza Merkeze Kasa Giriş");
                    SAFECH2.Add("@ReturnDesc", "");
                    var SABHCHID2 = conn.InsertBack("FK_KASAMERKEZAL", SAFECH2);
                    string u4 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID2}' where SFCOCEDSID = '{id}'");
                    var sonuc4 = Logins(u4);



                    Dictionary<string, string> SAFE3 = new Dictionary<string, string>();
                    SAFE3.Add("@id", id);
                    SAFE3.Add("@SAFEID", "2155");
                    SAFE3.Add("@SABHID", "");
                    SAFE3.Add("@SABHDIVISON", "00");
                    SAFE3.Add("@SABHSOURCE", "VK1");
                    SAFE3.Add("@SABHSOCODE", CHSOCODE);
                    SAFE3.Add("@SABHDEEDNOTES", "Merkez Kasa "+CHSOCODE +" Kasiyer Çıkış");
                    SAFE3.Add("@SABHVERSEVAL", DSAFEVAL);
                    SAFE3.Add("@SABHVERSENAME", DSAFENAME);
                    SAFE3.Add("@ReturnDesc", "");
                    kasiyercikisID = conn.InsertBack("FK_KASAILK", SAFE3);
                    string u5 = String.Format($"update SAFEMOVIE set VK1 = '{kasiyercikisID}' where SFCOCEDSID = '{id}'");
                    var sonuc5 = Logins(u5);

                    Dictionary<string, string> SAFECH3 = new Dictionary<string, string>();
                    SAFECH3.Add("@SABHID", kasiyercikisID);
                    SAFECH3.Add("@SAFEID", DSAFEID);
                    SAFECH3.Add("@SABHDEEDNOTES", "Merkez Kasa " + CHSOCODE + " Kasiyer Çıkış");
                    SAFECH3.Add("@ReturnDesc", "");
                    var SABHCHID3 = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH3);
                    string u6 = String.Format($"update SAFEMOVIE set VK1CH = '{SABHCHID3}' where SFCOCEDSID = '{id}'");
                    var sonuc6 = Logins(u6);


                    Dictionary<string, string> SAFE4 = new Dictionary<string, string>();
                    SAFE4.Add("@id", id);
                    SAFE4.Add("@SAFEID", DSAFEID);
                    SAFE4.Add("@SABHID", kasiyercikisID);
                    SAFE4.Add("@SABHDIVISON", "00");
                    SAFE4.Add("@SABHSOURCE", "VK0");
                    SAFE4.Add("@SABHSOCODE", CHSOCODE);
                    SAFE4.Add("@SABHDEEDNOTES", DIVVAL + " MAağaza " + SABHDATE + " Tarihli Kasası");
                    SAFE4.Add("@SABHVERSEVAL", "00.Kasa.TL");
                    SAFE4.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                    SAFE4.Add("@ReturnDesc", "");
                    kasiyergirisID = conn.InsertBack("FK_KASAILK", SAFE4);
                    string u7 = String.Format($"update SAFEMOVIE set VK0 = '{kasiyergirisID}' where SFCOCEDSID = '{id}'");
                    var sonuc7 = Logins(u7);

                    Dictionary<string, string> SAFECH4 = new Dictionary<string, string>();
                    SAFECH4.Add("@SABHID", kasiyergirisID);
                    SAFECH4.Add("@SAFEID", "2155");
                    SAFECH4.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                    SAFECH4.Add("@ReturnDesc", "");
                    var SABHCHID4 = conn.InsertBack("FK_KASAMERKEZAL", SAFECH4);
                    string u8 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID4}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                    var sonuc8 = Logins(u8);

                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {


                if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI1"].ToString()))
                {
                    try
                    {
                        Dictionary<string, string> SAFE = new Dictionary<string, string>();
                        SAFE.Add("@id", id);
                        SAFE.Add("@SAFEID", "2155");
                        SAFE.Add("@SABHID", "");
                        SAFE.Add("@SABHDIVISON", "");
                        SAFE.Add("@SABHSOURCE", "VI1");
                        SAFE.Add("@SABHSOCODE", "");
                        SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Düzeltme");
                        SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                        SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                        SAFE.Add("@ReturnDesc", "");
                        magazacikisID = conn.InsertBack("FK_KASAILK", SAFE);
                        //Form1.magazacikisID = magazacikisID;
                        string u1 = String.Format($"update SAFEMOVIE set VI1 = '{magazacikisID}' where SFCOCEDSID = '{id}'");
                        var sonuc1 = Logins(u1);

                        Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                        SAFECH.Add("@SABHID", magazacikisID);
                        SAFECH.Add("@SAFEID", "2155");
                        SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                        SAFECH.Add("@ReturnDesc", "");
                        var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);


                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI1CH"].ToString()))
                {
                    try
                    {
                        Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                        SAFECH.Add("@SABHID", Islem.Rows[0]["VI1"].ToString());
                        SAFECH.Add("@SAFEID", "2155");
                        SAFECH.Add("@SABHDEEDNOTES", "Merkeze Para Gönderimi");
                        SAFECH.Add("@ReturnDesc", "");
                        var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                        string u1 = String.Format($"update SAFEMOVIE set VI1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                        var sonuc1 = Logins(u1);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI0"].ToString()))
                {
                    Dictionary<string, string> SAFE = new Dictionary<string, string>();
                    SAFE.Add("@id", id);
                    SAFE.Add("@SAFEID", "2155");
                    SAFE.Add("@SABHID", Islem.Rows[0]["VI1"].ToString());
                    SAFE.Add("@SABHDIVISON", "00");
                    SAFE.Add("@SABHSOURCE", "VI0");
                    SAFE.Add("@SABHSOCODE", CHSOCODE);
                    SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Giriş");
                    SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                    SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                    SAFE.Add("@ReturnDesc", "");
                    magazagirisID = conn.InsertBack("FK_KASAILK", SAFE);
                    string u1 = String.Format($"update SAFEMOVIE set VI0 = '{magazagirisID}' where SFCOCEDSID = '{id}'");
                    var sonuc1 = Logins(u1);

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", magazagirisID);
                    SAFECH.Add("@SAFEID", "2155");
                    SAFECH.Add("@SABHDEEDNOTES", "Merkeze Kasa Giriş");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                    string u2 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                    var sonuc2 = Logins(u2);
                }
                else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VI0"].ToString()))
                {

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", magazagirisID);
                    SAFECH.Add("@SAFEID", "2155");
                    SAFECH.Add("@SABHDEEDNOTES", "Merkeze Kasa Giriş");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                    string u2 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                    var sonuc2 = Logins(u2);
                }
                if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK1"].ToString()))
                {
                    Dictionary<string, string> SAFE = new Dictionary<string, string>();
                    SAFE.Add("@id", id);
                    SAFE.Add("@SAFEID", "2155");
                    SAFE.Add("@SABHID", "");
                    SAFE.Add("@SABHDIVISON", "00");
                    SAFE.Add("@SABHSOURCE", "VK1");
                    SAFE.Add("@SABHSOCODE", CHSOCODE);
                    SAFE.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                    SAFE.Add("@SABHVERSEVAL", DSAFEVAL);
                    SAFE.Add("@SABHVERSENAME", DSAFENAME);
                    SAFE.Add("@ReturnDesc", "");
                    kasiyercikisID = conn.InsertBack("FK_KASAILK", SAFE);
                    string u3 = String.Format($"update SAFEMOVIE set VK1 = '{kasiyercikisID}' where SFCOCEDSID = '{id}'");
                    var sonuc3 = Logins(u3);

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", kasiyercikisID);
                    SAFECH.Add("@SAFEID", DSAFEID);
                    SAFECH.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                    string u4 = String.Format($"update SAFEMOVIE set VK1CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                    var sonuc4 = Logins(u4);

                }
                else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK0CH"].ToString()))
                {
                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", Islem.Rows[0]["VK0"].ToString());
                    SAFECH.Add("@SAFEID", DSAFEID);
                    SAFECH.Add("@SABHDEEDNOTES", "Merkez Kasa Kasiyer Çıkış");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZGONDER", SAFECH);
                    string u4 = String.Format($"update SAFEMOVIE set VI0CH = '{SABHCHID}' where SFCOCEDSID = '{id}'");
                    var sonuc4 = Logins(u4);
                }
                if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK0"].ToString()))
                {
                    Dictionary<string, string> SAFE = new Dictionary<string, string>();
                    SAFE.Add("@id", id);
                    SAFE.Add("@SAFEID", DSAFEID);
                    SAFE.Add("@SABHID", kasiyercikisID);
                    SAFE.Add("@SABHDIVISON", "00");
                    SAFE.Add("@SABHSOURCE", "VK0");
                    SAFE.Add("@SABHSOCODE", CHSOCODE);
                    SAFE.Add("@SABHDEEDNOTES", DIVVAL + " MAağaza " + SABHDATE + " Tarihli Kasası");
                    SAFE.Add("@SABHVERSEVAL", "00.Kasa.TL");
                    SAFE.Add("@SABHVERSENAME", "Merkez Yönetim Ana Kasa TL");
                    SAFE.Add("@ReturnDesc", "");
                    kasiyergirisID = conn.InsertBack("FK_KASAILK", SAFE);
                    string u1 = String.Format($"update SAFEMOVIE set VK0 = '{kasiyergirisID}' where SFCOCEDSID = '{id}'");
                    var sonuc1 = Logins(u1);

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", kasiyergirisID);
                    SAFECH.Add("@SAFEID", "2155");
                    SAFECH.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                    string u2 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                    var sonuc2 = Logins(u2);
                }
                else if (string.IsNullOrWhiteSpace(Islem.Rows[0]["VK1CH"].ToString()))
                {

                    Dictionary<string, string> SAFECH = new Dictionary<string, string>();
                    SAFECH.Add("@SABHID", Islem.Rows[0]["VK0"].ToString());
                    SAFECH.Add("@SAFEID", "2155");
                    SAFECH.Add("@SABHDEEDNOTES", DIVVAL + " Mağaza " + SABHDATE + " Tarihli Kasası");
                    SAFECH.Add("@ReturnDesc", "");
                    var SABHCHID = conn.InsertBack("FK_KASAMERKEZAL", SAFECH);
                    string u2 = String.Format($"update SAFEMOVIE set VK0CH = '{SABHCHID}', DSAFEID = {DSAFEID} where SFCOCEDSID = '{id}'");
                    var sonuc2 = Logins(u2);
                }
            }
            this.Close();
            this.Dispose();
        }
    }
}