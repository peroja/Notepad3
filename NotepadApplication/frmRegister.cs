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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Be Aware: Some Strings are having German Text in it. Please use Google Trandlator


namespace NotepadApplication
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {

        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string programm = "Editor";
            string registerdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int notiz = chkNote.Checked ? 1 : 0;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please Provide a Name and valid Email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 📦 Registry speichern
            string regPath = @"Software\Randy Tomlinson\Editor";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath);
            key.SetValue("Registered", name);
            key.SetValue("Email", email); // optional
            key.Close();

            // 🌐 Daten an Webserver senden
            try
            {
                // 🔑 regkey aus Registry lesen
                string hiddenKeyPath = @"Software\Microsoft\Windows\Rjte";
                RegistryKey hiddenKey = Registry.CurrentUser.OpenSubKey(hiddenKeyPath);
                string regkey = hiddenKey?.GetValue("SysId", "").ToString();

                if (string.IsNullOrWhiteSpace(regkey))
                {
                    MessageBox.Show("No Registerkey found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
        {
            { "regkey", regkey },
            { "name", name },
            { "email", email },
            { "programm", programm },
            { "registerdate", registerdate },
            { "notiz", notiz.ToString() }
        };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("https://www.randytomlinson.com/register.php", content);

                    string responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.Trim() == "OK")
                    {
                        MessageBox.Show("Registering Successfulh!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Registering at the Server Failed:\n" + responseString, "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection to the Registering Server failed.:\n" + ex.Message, "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
