using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dkm_cs_dkm_bh_preview
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BhBaseData bhbase;

            if (!BhBaseData.tryLoadYamlFile(out bhbase))
            {
                bhbase = BhBaseData.Create(
                rootFolder: @"X:\bmd\ER\2025\09",
                    belegYear: 2025, belegMon: 9);
            }
            
            MsgReaderForm.Opts opts = new MsgReaderForm.Opts();
            opts.bhbaseData = bhbase;

            Application.Run(new MsgReaderForm(opts));
            FileCleanUpHolder.GetInst().cleanUp();
        }
    }
}
