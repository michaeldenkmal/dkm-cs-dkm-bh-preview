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
using static dkm_cs_dkm_bh_preview.TextboxUtil;

namespace dkm_cs_dkm_bh_preview
{
    public partial class MsgReaderForm : Form
    {

        public class Opts
        {
            public BhBaseData bhbaseData;
        }

        private Opts _opts;

        public MsgReaderForm(Opts opts)
        {
            this._opts = opts;
            InitializeComponent();
            InitializeControls();
        }
        public class GuiData
        {

            public string Info { get; set; }
            public DateTime? BelegDate { get; set; }
            public Decimal? Betrag { get; set; }
            public int? BelegYear { get; set; }
            public int? BelegMon { get; set; }

            public string ErrBelegMon { get; set; }
            public string ErrBelegYear { get; set; }
            public string ErrBelegDate { get; set; }

            public bool anyErrs()
            {
                if (!string.IsNullOrEmpty(this.ErrBelegMon))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(this.ErrBelegYear))
                {
                    return true;
                }
                return false;
            }

        }

        private class BelegmonValRule : ValidateTextNumIntRule
        {

            public string formatErrorMsg(string szText)
            {
                return $"{szText}:Monat muss zwischen 1-12 liegen";
            }


            public bool validate(int value)
            {
                return (value > 0) && (value < 13);
            }
        }

        private class BelegMonValNumOpts : ValidateNumTextBoxOpts
        {

            private MsgReaderForm _form;
            private GuiData _guiData;

            public BelegMonValNumOpts(MsgReaderForm form, GuiData guiData)
            {
                this._form = form;
                this._guiData = guiData;
            }



            public string getNumFormatErr(string strValue)
            {
                return $"{strValue} ist keine Zahl kein gültiger Monat";
            }

            public string getVal()
            {
                return _form.edtMonat.Text;
            }

            public ValidateTextNumIntRule[] getValidateRules()
            {
                return new ValidateTextNumIntRule[]
                {
                    new BelegmonValRule()
                };
            }

            public void setErrInfo(string errmsg)
            {
                _guiData.ErrBelegMon = errmsg;
            }

            public void setIntVal(int? v)
            {
                _guiData.BelegMon = v;
            }
        }


        private class BelegJahrValNumOpts : ValidateNumTextBoxOpts
        {

            private MsgReaderForm _form;
            private GuiData _guiData;

            public BelegJahrValNumOpts(MsgReaderForm form, GuiData guiData)
            {
                this._form = form;
                this._guiData = guiData;
            }



            public string getNumFormatErr(string strValue)
            {
                return $"{strValue} ist keine Zahl kein gültiger Monat";
            }

            public string getVal()
            {
                return _form.edtJahr.Text;
            }

            public ValidateTextNumIntRule[] getValidateRules()
            {
                return new ValidateTextNumIntRule[]
                {
                };
            }

            public void setErrInfo(string errmsg)
            {
                _guiData.ErrBelegYear = errmsg;
            }

            public void setIntVal(int? v)
            {
                _guiData.BelegYear = v;
            }
        }




        private GuiData guiToData()
        {
            var ret = new GuiData();
            ret.BelegDate = dtpBelegDate.Value;
            ret.Info = edtInfo.Text;
            ret.Betrag = edtBetrag.Value;
            TextboxUtil.ValidateNumTextBox(new BelegMonValNumOpts(this, ret));
            TextboxUtil.ValidateNumTextBox(new BelegJahrValNumOpts(this, ret));
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
                dtpBelegDate.Value = guidata.BelegDate;
                edtInfo.Text = guidata.Info;
                edtBetrag.Value = guidata.Betrag;
                updateErrProvider(errProvMon, edtMonat.TextBox, guidata.ErrBelegMon);
                updateErrProvider(errProvYear, edtJahr.TextBox, guidata.ErrBelegMon);

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
            initYearMon();
            initWebView();
            initRootFolder();
            initTabOrder();
            InitializeWebView2Async();
        }

        private void initTabOrder()
        {
            FormUtil.setTabOrder(new Control[]{
                dtpBelegDate,edtInfo,edtBetrag, btnExecute
            });
        }

        private void initRootFolder()
        {
            if (Directory.Exists(_opts.bhbaseData.RootFolder))
            {
                edtBhRootFolder.Text = _opts.bhbaseData.RootFolder;
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

        private void initYearMon()
        {
            edtJahr.Text = _opts.bhbaseData.BelegYear.ToString();
            edtMonat.Text = _opts.bhbaseData.BelegMon.ToString();
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
                } else
                {
                    throw new Exception($"belegDat ist null");
                }
                decimal betrag;
                if (guidata.Betrag.HasValue)
                {
                    betrag = guidata.Betrag.Value;
                } else
                {
                    throw new Exception($"betrag is null");
                }
                CreMoveFileUtil.copyFileToErFolder(pdfFullPath: pdfFileList.Elem.PdfFp, belegDate: belegDate,
                    info: guidata.Info, betrag: betrag, outputErFolder: _opts.bhbaseData.GetOutputFolder(), 
                    origFileHandledFolder: _opts.bhbaseData.GetOrigFileHandledFolder(),
                    origFileFullName: pdfFileList.Elem.BhFileBaseInst.OrigFileName
                    );                
                pdfFileList.Elem.BhFileBaseInst.cleanUp();
                handleBtnNext();
                cleanInput();
                dtpBelegDate.Focus();
                
            } catch (Exception ex) {
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
            showPdfFile(this.pdfFileList.Elem.PdfFp);
        }
        void saveBhBaseData()
        {
            var guiData = guiToData();
            BhBaseData bhbase = BhBaseData.Create(
                rootFolder: edtBhRootFolder.Text,
                belegYear: guiData.BelegYear.GetValueOrDefault(),
                belegMon: guiData.BelegMon.GetValueOrDefault()
                );
            BhBaseData.saveToYaml(bhbase);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            var dlgres = folderBrowserDialog1.ShowDialog();
            if (dlgres == DialogResult.OK)
            {
                edtBhRootFolder.Text = folderBrowserDialog1.SelectedPath;
            }

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
    }
}
