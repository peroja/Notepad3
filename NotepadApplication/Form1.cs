//*****************************************************************************************************
//
//  Project: Notepad 3
//  Open Source under the MIT License
//
//  Copyright (c) 2025 Randy J. Tomlinson
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
//
//*****************************************************************************************************


using Microsoft.Win32;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

// Be Aware: Some Strings are having German Text in it. Please use Google Trandlator

namespace NotepadApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load); // 🔧 Wichtig: Load-Event binden
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        private async Task SendSysIdToServerAsync(string sysId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "regkey", sysId }
                    };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("https://www.randytomlinson.com/register.php", content);
                    string responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Senden der SysId: " + ex.Message);
            }
        }

        private async Task CheckAndInitializeRegistry()
        {
            try
            {
                string authorKey = @"Software\Randy Tomlinson\Editor";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(authorKey, true);

                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(authorKey);
                    key.SetValue("Author", "Randy Tomlinson");
                    key.SetValue("Software", "Editor");
                    key.SetValue("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    key.SetValue("Registered", "");
                    key.SetValue("Install", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                string registeredTo = key.GetValue("Registered", "").ToString();
                lblRegistered.Text = !string.IsNullOrWhiteSpace(registeredTo)
                    ? $"Registered to \"{registeredTo}\""
                    : "Unregistered Version";

                string hiddenKeyPath = @"Software\Microsoft\Windows\Rjte";
                RegistryKey hiddenKey = Registry.CurrentUser.OpenSubKey(hiddenKeyPath, true);

                if (hiddenKey == null)
                {
                    hiddenKey = Registry.CurrentUser.CreateSubKey(hiddenKeyPath);
                    string randomId = GenerateRandomString(12);
                    hiddenKey.SetValue("SysId", randomId);
                    await SendSysIdToServerAsync(randomId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Zugriff auf die Registry: " + ex.Message);
            }
        }

        private async Task<string> GetOnlineVersionAsync()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync("https://www.yourdomain.com/version.txt");
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await CheckAndInitializeRegistry();

            try
            {
                string localVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string onlineVersionRaw = await GetOnlineVersionAsync();
                string onlineVersion = onlineVersionRaw.Trim();

                if (new Version(onlineVersion) > new Version(localVersion))
                {
                    var result = MessageBox.Show(
                        $"A new Update is available ({onlineVersion}). Do you want to update?",
                        "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "https://www.yourdomain.com/download.php",
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Versionsprüfung:\n" + ex.Message);
            }

            lblRegistered.Text = $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Text = "";
        }

        private void OpenDlg()
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Any File|*.*";
            if (of.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.Text = File.ReadAllText(of.FileName);
                this.Text = of.FileName;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDlg();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(this.Text, fastColoredTextBox1.Text);
            }
            catch
            {
                SaveDlg();
            }
        }

        private void SaveDlg()
        {
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "Any File|*.*";
            if (of.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(of.FileName, fastColoredTextBox1.Text);
                this.Text = of.FileName;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDlg();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.BackColor = cd.Color;
            }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.ForeColor = cd.Color;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog cd = new FontDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.Font = cd.Font;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Redo();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.SelectAll();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }

        private void pastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowFindDialog();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowGoToDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowReplaceDialog();
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.CSharp;
        }

        private void vBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.VB;
        }

        private void hTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.HTML;
        }

        private void pHPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.PHP;
        }

        private void jSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.JS;
        }

        private void sQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.SQL;
        }

        private void lUAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Lua;
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.XML;
        }

        private void infoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmInfo frm2 = new frmInfo();
            frm2.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF file (*.pdf)|*.pdf";
            sfd.Title = "Save PDF File";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;

                PdfDocument document = new PdfDocument();
                document.Info.Title = "Exported Text";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Consolas", 10);

                string text = fastColoredTextBox1.Text;
                double lineHeight = font.GetHeight();

                double x = XUnit.FromPoint(40);
                double y = XUnit.FromPoint(40);
                double maxHeight = page.Height - XUnit.FromPoint(40);

                string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (y + lineHeight > maxHeight)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = XUnit.FromPoint(40);
                    }

                    gfx.DrawString(line, font, XBrushes.Black,
                        new XRect(x, y, page.Width - XUnit.FromPoint(80), page.Height),
                        XStringFormats.TopLeft);

                    y += lineHeight;
                }

                document.Save(filename);
                document.Close();

                MessageBox.Show("PDF erfolgreich gespeichert!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            // Keine Funktion vorhanden
        }
    }
}
