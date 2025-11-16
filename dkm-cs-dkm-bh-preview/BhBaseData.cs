using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{
    public class BhBaseData
    {
        public string RootFolder { get; private set; }
        public string OutputFolder
        {
            get
            {
                return Path.Combine(RootFolder, "upload");
            }
        }
        public string OrigFileHandledFolder
        {
            get
            {
                return Path.Combine(RootFolder, "erledigt");
            }
        }

        public int BelegYear { get; private set; }
        public int BelegMon { get; private set; }


        public static BhBaseData Create(string rootFolder, int belegYear, int belegMon)
        {
            var ret = new BhBaseData();
            ret.RootFolder = rootFolder;
            ret.BelegYear = belegYear;
            ret.BelegMon = belegMon;
            return ret;
        }

    }
}
