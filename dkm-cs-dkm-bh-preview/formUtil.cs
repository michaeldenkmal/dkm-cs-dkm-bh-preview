using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dkm_cs_dkm_bh_preview
{
    public static class FormUtil
    {
        public static void setTabOrder (Control[] controls)
        {
            for (int i=0; i<controls.Length;i++)
            {
                controls[i].TabIndex = i;
            }
        }
    }
}
