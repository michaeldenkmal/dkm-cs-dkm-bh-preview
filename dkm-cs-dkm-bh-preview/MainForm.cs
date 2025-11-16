using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace dkm_cs_dkm_bh_preview
{
    public partial class MainForm : Form
    {
        private WebView2 webView;
        private Button btnOpen;

        public MainForm()
        {
            this.Text = "PDF Vorschau mit WebView2 (.NET Framework 4.8)";
            this.Width = 1000;
            this.Height = 700;

            InitializeControls();
            InitializeWebView2Async();
        }

        private void InitializeControls()
        {
            // Button "PDF öffnen..."
            btnOpen = new Button();
            btnOpen.Text = "PDF öffnen...";
            btnOpen.Dock = DockStyle.Top;
            btnOpen.Height = 35;
            btnOpen.Click += BtnOpen_Click;

            // WebView2 Control
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;

            // Reihenfolge ist wichtig: erst Button oben, dann WebView2 füllt den Rest
            this.Controls.Add(webView);
            this.Controls.Add(btnOpen);
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

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Dateien (*.pdf)|*.pdf|Alle Dateien (*.*)|*.*";
                ofd.Title = "PDF-Datei auswählen";

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    var filePath = ofd.FileName;

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("Datei nicht gefunden:\r\n" + filePath,
                            "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        // Lokale Datei als URI
                        var uri = new Uri(filePath);
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
            }
        }
    }
}
