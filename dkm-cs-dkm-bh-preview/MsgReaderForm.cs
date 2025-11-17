using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using MsgReader.Outlook;
using static dkm_cs_dkm_bh_preview.FormUtil;
using static dkm_cs_dkm_bh_preview.TextboxUtil;

namespace dkm_cs_dkm_bh_preview
{
    public partial class MsgReaderForm : Form
    {

        public class Opts
        {
            public BhBaseData bhbaseData;
        }

        //private Opts _opts;

        public MsgReaderForm(Opts opts)
        {
            //this._opts = opts;
            InitializeComponent();
            InitializeControls();
            applyOpts(opts);
        }

        private void applyOpts(Opts opts)
        {
            var guiData = new GuiData();
            // Jahr
            guiData.BelegYear = opts.bhbaseData.BelegYear;
            // Monat
            guiData.BelegMon = opts.bhbaseData.BelegMon;
            // bhRootFolder
            guiData.BhRootFolder = opts.bhbaseData.BhRootFolder;
            // cbxLastRootFolders
            guiData.LastBhRootFolders = opts.bhbaseData.LastBhFolders;
            applyDataToGui(guiData);
        }

        public class GuiErrors
        {
            public string ErrBelegMon { get; set; }
            public string ErrBelegYear { get; set; }
            public string ErrBelegDate { get; set; }
            public string ErrBhRootFolder { get; set; }
            public string ErrBetrag { get; set; }
            public string ErrInfo { get; set; }

            public bool anyErrs()
            {
                string[] test = new string[] { ErrBelegDate, ErrBelegMon, ErrBelegYear, ErrBhRootFolder ,ErrBetrag, ErrInfo};
                return test.ToList().Exists(p => !string.IsNullOrEmpty(p));
            }

        }

        public class GuiData:GuiErrors
        {

            public string Info { get; set; }
            public DateTime? BelegDate { get; set; }
            public Decimal? Betrag { get; set; }
            public int? BelegYear { get; set; }
            public int? BelegMon { get; set; }
            public string BhRootFolder { get; set; }
            public string[] LastBhRootFolders { get; set; }

            public ErrProvInfoRec[] buildErrProvInfoRecs(MsgReaderForm form)
            {
                //   ErrInfo
                return new ErrProvInfoRec[]
                {
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrBelegDate, theControl:form.dtpBelegDate, theErrorProvider: form.errProvBelegDate),
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrBelegMon, theControl:form.edtMonat.TextBox, theErrorProvider: form.errProvMon),
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrBelegYear, theControl:form.edtJahr.TextBox, theErrorProvider: form.errProvYear),
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrBhRootFolder, theControl:form.edtBhRootFolder.ComboBox, theErrorProvider: form.errProvBhRootFolder),
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrBetrag, theControl:form.edtBetrag, theErrorProvider: form.errProvBetrag),
                    ErrProvInfoRec.Create(fnGetErr: ()=> this.ErrInfo, theControl:form.edtInfo, theErrorProvider: form.errProvInfo)
                };
            }
        }


        private GuiData guiToData()
        {
            var ret = new GuiData();
            ret.BelegDate = dtpBelegDate.Value;
            if (!ret.BelegDate.HasValue)
            {
                ret.ErrBelegDate = $"Belegdatum ist leer";
            }
            ret.Info = edtInfo.Text;
            if (string.IsNullOrEmpty(ret.Info))
            {
                ret.ErrInfo = "Info fehlt";
            }
            ret.Betrag = edtBetrag.Value;
            if (!ret.Betrag.HasValue)
            {
                ret.ErrBetrag = "Belegbetrag ist leer";
            }
            // Monat
            ret.BelegMon= TextboxUtil.ValidateNumTextBox(
                fnGetVal: () => edtMonat.Text,
                //ValidationRules = validationRules;
                validationRules:  new ValidateTextNumIntRule[]{
                    ValidateTextNumIntRule.Create
                    (
                    //TFN_ValidateTextNumIntRule_validate validate,  bool TFN_ValidateTextNumIntRule_validate(int value) 
                    validate:(int value) => (value > 0) && (value < 13),
                    //TFN_FormatErrorMsg formatErrorMsg) 
                    formatErrorMsg: szText => $"{szText}:Monat muss zwischen 1-12 liegen"
                    ) },
                fnNumFormatErr: (strValue) => $"{strValue} ist keine Zahl, kein gültiger Monat",
                fnSetErrInfo:(errors) => { ret.ErrBelegMon = string.Join(";", errors); }
                );
            //Jahr
            ret.BelegYear= TextboxUtil.ValidateNumTextBox(
                fnGetVal: () => edtJahr.Text,
                //ValidationRules = validationRules;
                validationRules: new ValidateTextNumIntRule[] { },
                fnNumFormatErr: (strValue) => $"{strValue} ist keine Zahl",
                fnSetErrInfo: (errmsgs) => { ret.ErrBelegYear = TextboxUtil.formatErrList(errmsgs); }                
                );
            // bhRootFolder
            ret.BhRootFolder = TextboxUtil.ValidateTextBox(
                fnGetStrVal: () => edtBhRootFolder.Text,
                validationRules: new ValidateTextBoxRule[]
                {
                    ValidateTextBoxRule.Create(
                        fnValidate:text=> Directory.Exists(text),
                        fnFormatErrorMsg:text=> $"Ordner @@{text}@@ existiert nicht")
                },
                fnSetErrInfo:errs=> { ret.ErrBhRootFolder = TextboxUtil.formatErrList(errs); },
                valueOnErr: ""
                ) ;

            var allOk =FormUtil.setErrProvByRec(ret.buildErrProvInfoRecs(this));

            return ret;
        }


        // Controls
        private CurrencyTextBox edtBetrag;
        private NullableDateTimePicker dtpBelegDate;
        private WebView2 webView;
        // ende Controls
        private bool inGuiDataToForm = false;


        private void updateErrProvider(ErrorProvider errprov, Control ctrl, string errinfo)
        {
            if (string.IsNullOrEmpty(errinfo))
            {
                errprov.Clear();
            }
            else
            {
                errprov.SetError(ctrl, errinfo);
            }
        }



        private void applyDataToGui(GuiData guidata)
        {
            inGuiDataToForm = true;
            try
            {
                dtpBelegDate.Value = guidata.BelegDate.GetValueOrDefault(DateTime.Now);
                edtInfo.Text = guidata.Info;
                edtBetrag.Value = guidata.Betrag.GetValueOrDefault(0);
                edtMonat.Text = guidata.BelegMon.GetValueOrDefault(0).ToString();
                edtJahr.Text = guidata.BelegYear.GetValueOrDefault(0).ToString();
                edtBhRootFolder.Text = guidata.BhRootFolder;
                edtBhRootFolder.Items.Clear();
                foreach (var folder in guidata.LastBhRootFolders)
                {
                    edtBhRootFolder.Items.Add(folder);
                }

            }
            finally
            {
                inGuiDataToForm = false;
            }

        }


        public void updateGuiState(GuiData newGuiState)
        {
            applyDataToGui(newGuiState);
        }

        private void MsgReaderForm_Load(object sender, EventArgs e)
        {

        }


        private void initBetrag()
        {
            this.edtBetrag = new CurrencyTextBox();
            edtBetrag.Culture = new CultureInfo("de-AT");
            edtBetrag.Left = lblBetrag.Left;
            edtBetrag.Top = edtInfo.Top;
            this.pnlInputData.Controls.Add(edtBetrag);
        }

        private void InitializeControls()
        {
            initBetrag();
            initBelegDate();
            initWebView();
            initTabOrder();
            InitializeWebView2Async();
        }


        private void initTabOrder()
        {
            FormUtil.setTabOrder(new Control[]{
                dtpBelegDate,edtInfo,edtBetrag, btnExecute
            });
        }

        private void initRootFolder(string rootFolder)
        {
            if (Directory.Exists(rootFolder))
            {
                edtBhRootFolder.Text = rootFolder;
            }
        }


        private void initWebView()
        {
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(webView);
        }

        private async void InitializeWebView2Async()
        {
            try
            {
                // Initialisierung des WebView2
                // Optional: eigenen User-Daten-Folder nutzen:
                // var env = await CoreWebView2Environment.CreateAsync(userDataFolder: @"C:\temp\WebView2Cache");
                // await webView.EnsureCoreWebView2Async(env);

                await webView.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Initialisierung von WebView2:\r\n" + ex.Message,
                    "WebView2 Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void initBelegDate()
        {
            this.dtpBelegDate = new NullableDateTimePicker();
            this.dtpBelegDate.Left = lblBelegDate.Left;
            this.dtpBelegDate.Top = edtInfo.Top;
            this.pnlInputData.Controls.Add(dtpBelegDate);
        }

        private void initYearMon(int year, int mon)
        {
            edtJahr.Text = year.ToString();
            edtMonat.Text = mon.ToString();
        }




        private void log(string msg)
        {
            edtLog.AppendText(msg + Environment.NewLine);
        }

        private void cleanInput()
        {
            this.edtBetrag.Value = 0;
            this.edtInfo.Text = "";
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            saveBhBaseData();
            var guidata = guiToData();
            if (guidata.anyErrs())
            {
                return;
            }
            try
            {
                DateTime belegDate;
                if (guidata.BelegDate.HasValue)
                {
                    belegDate = guidata.BelegDate.Value;
                }
                else
                {
                    throw new Exception($"belegDat ist null");
                }
                decimal betrag;
                if (guidata.Betrag.HasValue)
                {
                    betrag = guidata.Betrag.Value;
                }
                else
                {
                    throw new Exception($"betrag is null");
                }
                CreMoveFileUtil.copyFileToErFolder(pdfFullPath: pdfFileList.Elem.PdfFp, belegDate: belegDate,
                    info: guidata.Info, betrag: betrag, 
                    outputErFolder: BhBaseData.getOutputFolder(guidata.BhRootFolder),
                    origFileHandledFolder:  BhBaseData.getOrigFileHandledFolder(guidata.BhRootFolder),
                    origFileFullName: pdfFileList.Elem.BhFileBaseInst.OrigFileName
                    );
                pdfFileList.Elem.BhFileBaseInst.cleanUp();
                handleBtnNext();
                cleanInput();
                dtpBelegDate.Focus();

            }
            catch (Exception ex)
            {
                log($"{ex.Message}: handling of file: {pdfFileList.Elem.PdfFp}:{ex}");
            }

        }

        private ArrToCursor<BhFileHandler.PdfFileAndCleanUp> pdfFileList;

        private bool TryGetPdfFileList(out ArrToCursor<BhFileHandler.PdfFileAndCleanUp> pdfFileList)
        {
            pdfFileList = this.pdfFileList;
            return pdfFileList != null;
        }


        private void showPdfFile(string pdfFilePath)
        {
            log("Zeige Pdf Datei: " + pdfFilePath);
            try
            {
                // Lokale Datei als URI
                var uri = new Uri(pdfFilePath);
                if (webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.Navigate(uri.AbsoluteUri);
                }
                else
                {
                    // Falls CoreWebView2 noch nicht initialisiert ist, nachinitialisieren und dann laden
                    webView.CoreWebView2InitializationCompleted += (s, args) =>
                    {
                        if (args.IsSuccess)
                        {
                            webView.CoreWebView2.Navigate(uri.AbsoluteUri);
                        }
                        else
                        {
                            MessageBox.Show("WebView2 konnte nicht initialisiert werden:\r\n" + args.InitializationException,
                                "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    InitializeWebView2Async();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der PDF-Datei:\r\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void btnReadBhFolder_Click(object sender, EventArgs e)
        {
            edtBhRootFolder_Validating();
            saveBhBaseData();
            if (!Directory.Exists(edtBhRootFolder.Text))
            {
                return;
            }

            CreMoveFileUtil.SetupSubDirsInBhFolder(edtBhRootFolder.Text);

            List<BhFileBase> bhFileBases = BhFileHandler.readBhRootFolder(edtBhRootFolder.Text);
            var itertator = BhFileHandler.buildPdfFileIterator(bhFileBases);
            List<BhFileHandler.PdfFileAndCleanUp> lstpdffiles = new List<BhFileHandler.PdfFileAndCleanUp>();
            foreach (var it in itertator)
            {
                lstpdffiles.Add(it);
            }
            this.pdfFileList = new ArrToCursor<BhFileHandler.PdfFileAndCleanUp>(lstpdffiles.ToArray());
            if (!pdfFileList.empty())
            {
                showPdfFile(this.pdfFileList.Elem.PdfFp);
            }
        }
        void saveBhBaseData()
        {
            var guiData = guiToData();
            BhBaseData bhbase = BhBaseData.Create(
                bhRootFolder: edtBhRootFolder.Text,
                belegYear: guiData.BelegYear.GetValueOrDefault(),
                belegMon: guiData.BelegMon.GetValueOrDefault()
                );
            foreach (var itm in edtBhRootFolder.Items)
            {
                string folder = itm as string;
                bhbase.addBhFolder(folder);
            }
            BhBaseData.saveToYaml(bhbase);
        }

        

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (!this.pdfFileList.bof())
            {
                this.pdfFileList.prev();
                showPdfFile(this.pdfFileList.Elem.PdfFp);
            }
        }

        private void handleBtnNext()
        {
            if (!this.pdfFileList.eof())
            {
                this.pdfFileList.next();
                showPdfFile(this.pdfFileList.Elem.PdfFp);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            handleBtnNext();
        }

        private bool folderExistsInCbxItems(ComboBox cbx,string folder)
        {
            if (folder == null)
            {
                return false;
            }
            foreach (var item in cbx.Items)
            {
                var szItm = item as string;
                if (string.Compare(folder, szItm,ignoreCase:true)==0)
                {
                    return true;
                }
            }
            return false;
        }


        private void edtBhRootFolder_Validating()
        {
            var cbx = edtBhRootFolder.ComboBox;
            var newRootFolder = cbx.Text;
            var exists= Directory.Exists(newRootFolder);
            if (exists)
            {
                if (!folderExistsInCbxItems(cbx, newRootFolder))
                {
                    cbx.Items.Add(newRootFolder);
                }
            }
        }
    }
}
