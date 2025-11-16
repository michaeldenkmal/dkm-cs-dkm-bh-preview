using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{
    public static class  TextboxUtil
    {
        public interface ValidateTextNumIntRule
        {
            bool validate(int value);
            string formatErrorMsg(string szText);

        }

        public interface ValidateNumTextBoxOpts
        {
            string getVal();
            ValidateTextNumIntRule[] getValidateRules();

            string getNumFormatErr(string strValue);

            void setErrInfo(string errmsg);

            void setIntVal(int? v);
        }

        public class ValidateNumTextBoxRes
        {
            public int? newValue { get; set; }
            public string Error { get; set; }
        }

        public  static void ValidateNumTextBox(ValidateNumTextBoxOpts opts)
        {
            int mon;
            if (int.TryParse(opts.getVal(), out mon))
            {
                bool errFound = false;
                foreach (var valRule in opts.getValidateRules())
                {
                    if (!valRule.validate(mon))
                    {
                        errFound = true;
                        string errmsg = valRule.formatErrorMsg(opts.getVal());
                        opts.setErrInfo(errmsg);
                    }
                }
                if (!errFound)
                {
                    opts.setIntVal(mon);
                } else
                {
                    opts.setIntVal(null);
                }
            }
            else
            {
                opts.setIntVal(null);
                var errmsg = opts.getNumFormatErr(opts.getVal());
                opts.setErrInfo(errmsg);
            }

        }

    }
}
