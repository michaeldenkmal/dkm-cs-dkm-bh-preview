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

        public class ErrProvInfoRec
        {
            public Func<string> FnGetErr { get; set; }
            public Control TheControl { get; set; }
            public ErrorProvider ThErrorProvider { get; set; }

            public static ErrProvInfoRec Create(Func<string> fnGetErr, Control theControl, ErrorProvider theErrorProvider)
            {
                return new ErrProvInfoRec()
                {
                    FnGetErr = fnGetErr,
                    TheControl = theControl,
                    ThErrorProvider = theErrorProvider
                };
            }
        }
        /// <summary>
        /// setz Error Provider
        /// </summary>
        /// <param name="epirs"></param>
        /// <returns>true wenn keine Fehler ansonst false</returns>
        public static bool setErrProvByRec(ErrProvInfoRec[] epirs)
        {
            bool noErrorFound = true;
            foreach (var epir in epirs)
            {
                if (!setErrProvider(epir.FnGetErr(), epir.TheControl, epir.ThErrorProvider))
                {
                    noErrorFound = false;
                }
            }
            return noErrorFound;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="err"></param>
        /// <param name="control"></param>
        /// <param name="errprov"></param>
        /// <returns>true wenn kein Fehler ansonsten false</returns>
        public static bool setErrProvider(string err, Control control, ErrorProvider errprov)
        {
            if (string.IsNullOrEmpty(err))
            {
                errprov.Clear();
                return true;
            } else
            {
                errprov.SetError(control, err);
                return false;
            }
        }
    }
}
