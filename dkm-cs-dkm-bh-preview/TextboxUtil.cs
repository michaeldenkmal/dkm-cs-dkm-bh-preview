using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{
    public static class TextboxUtil
    {

        public delegate bool TFN_ValidateTextNumIntRule_validate(int value);
        public delegate string TFN_FormatErrorMsg(string szText);
        public delegate void TFN_setErrorListInfo(List<string> errors);
        
        public class ValidateTextNumIntRule
        {
            public TFN_ValidateTextNumIntRule_validate FnValidate { get; private set; }
            public TFN_FormatErrorMsg FnFormatErrorMsg { get; private set; }

            private ValidateTextNumIntRule(TFN_ValidateTextNumIntRule_validate validate, TFN_FormatErrorMsg formatErrorMsg)
            {
                FnValidate = validate;
                FnFormatErrorMsg = formatErrorMsg;
            }
            public static ValidateTextNumIntRule Create(TFN_ValidateTextNumIntRule_validate validate, TFN_FormatErrorMsg formatErrorMsg)
            {
                return new ValidateTextNumIntRule(validate, formatErrorMsg);
            }
        }

        public delegate string TFN_getStrVal();
        

        public static string formatErrList(List<string> errs)
        {
            return string.Join(";", errs);
        }

        public static int? ValidateNumTextBox(TFN_getStrVal fnGetVal, 
            ValidateTextNumIntRule[] validationRules, TFN_FormatErrorMsg fnNumFormatErr,
                TFN_setErrorListInfo fnSetErrInfo)
        {
            int mon;
            List<string> errors = new List<string>();
            string szTextboxValue = fnGetVal();
            if (int.TryParse(szTextboxValue, out mon))
            {
                foreach (var valRule in validationRules)
                {
                    if (!valRule.FnValidate(mon))
                    {
                        string errmsg = valRule.FnFormatErrorMsg(szTextboxValue);
                        errors.Add(errmsg);
                    }
                }
                if (errors.Count==0)
                {
                    return mon;
                }
                else
                {
                    fnSetErrInfo(errors);
                    return null;
                }
            }
            else
            {
                errors.Add(fnNumFormatErr(szTextboxValue));
                fnSetErrInfo(errors);
                return null;
            }
        }

        public delegate bool TFN_validateTextBox(string v);

        public class ValidateTextBoxRule
        {
            public TFN_validateTextBox FnValidate { get; set; }
            public TFN_FormatErrorMsg FnFormatErrorMsg { get; private set; }

            public static ValidateTextBoxRule Create(TFN_validateTextBox fnValidate, TFN_FormatErrorMsg fnFormatErrorMsg)
            {
                var ret = new ValidateTextBoxRule();
                ret.FnValidate = fnValidate;
                ret.FnFormatErrorMsg = fnFormatErrorMsg;
                return ret;
            }
        }
        
        
        public static string  ValidateTextBox(TFN_getStrVal fnGetStrVal,
                TFN_setErrorListInfo fnSetErrInfo,
                string valueOnErr,
                ValidateTextBoxRule[] validationRules)
        {
            string textboxvalue = fnGetStrVal();
            List<string> errors = new List<string>();
            foreach (var valRule in validationRules)
            {
                if (!valRule.FnValidate(textboxvalue))
                {
                    string errmsg = valRule.FnFormatErrorMsg(textboxvalue);
                    errors.Add(errmsg);
                }
            }
            if (errors.Count >0)
            {
                fnSetErrInfo(errors);
                return valueOnErr;
            } else
            {
                return textboxvalue;
            }
        }
    }


}

