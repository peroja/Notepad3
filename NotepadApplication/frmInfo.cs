using System;
using System.Collections.Generic;
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



using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Be Aware: Some Strings are having German Text in it. Please use Google Trandlator


namespace NotepadApplication
{
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            lblVersion.Text = $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
            string authorKey = @"Software\Randy Tomlinson\Editor";
            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(authorKey))
            {
                if (key != null)
                {
                    string registeredTo = key.GetValue("Registered", "").ToString();

                    if (!string.IsNullOrWhiteSpace(registeredTo))
                    {
                        // Wenn registriert → Button ausblenden
                        button1.Visible = true;
                        label2.Visible = false;
                        label4.Visible = false;


                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.randytomlinson.com",
                UseShellExecute = true
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "mailto:randy@randytomlinson.com?subject=Supportanfrage&body=Hallo%20Support-Team%2C",
                UseShellExecute = true
            });
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegister frm2 = new frmRegister();
            frm2.Show();
        }
    }
}
