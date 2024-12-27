using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolantMusteriDuzel.Class
{
    public class TableClass
    {
        public class Islemler
        {
            public int INSCINSID { get; set; }

            public string Musteri_Kodu { get; set; }

            public string Muster_Adi { get; set; }

            public DateTime? Iade_Tarihi { get; set; }

            public int? Iade_ID { get; set; }

            public DateTime? Hatali_Satis_Tarihi { get; set; }

            public int? Hatali_Satis_ID { get; set; }

            public int? Dogru_Satis_ID { get; set; }

            public decimal? Taksit_Tutari { get; set; }

            public decimal? Iade_Tutarı { get; set; }

            public decimal? Bakiye { get; set; }

        }
        public class IadeBakiyeDuzeltilenlerBakiye
        {
            public string Musteri_Kodu { get; set; }

            public decimal? SATISTOPLAM { get; set; }

            public decimal? PESINATTAHSILAT { get; set; }

            public decimal? TAKSITTAHSILAT { get; set; }

            public decimal? TAKSITBAKIYE { get; set; }

            public int? TAKSITSAYISI { get; set; }

            public DateTime? MINVADETARIHI { get; set; }

            public DateTime? SONODEMEMTARIHI { get; set; }

            public DateTime? SONALISVERISTARIHI { get; set; }

        }
        public class Odeme
        {
            public string PCDSID { get; set; }
        }
        public class HISTORYCHILD
        {
            public string HISCHHISID { get; set; }
            public string HISCHDETAIL { get; set; }
            public string HISBEFORE { get; set; }
            public string HISAFTER { get; set; }
        }
        public class HISTORY
        {
            public string HISID { get; set; }
            public string HISDATETIME { get; set; }
            public string HISSCODE { get; set; }
            public string HISKIND { get; set; }
            public string HISTITLE { get; set; }
            public string HISTITLEID { get; set; }
        }
    }
}
