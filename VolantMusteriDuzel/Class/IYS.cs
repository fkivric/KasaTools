using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolantMusteriDuzel.Class
{
    public class IYS
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string permissionType { get; set; }
        public string brandCode { get; set; }
        public string permissionStatus { get; set; }
        public int permissionSource { get; set; }
        public string isCheckBlackList { get; set; }
        public iysData iysDatas { get; set; }
    }
    public class iysData
    {
        public string recipient { get; set; }
        public string receiveType { get; set; }
        public DateTime consentDate { get; set; }
    }
}
