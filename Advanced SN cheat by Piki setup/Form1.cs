using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advanced_SN_cheat_by_Piki_setup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = false;
            Install();
        }

        private void Install()
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Title = "Open Secret Neighbor exe";
            dia.Filter = "Secret Neighbor | Secret Neighbour.exe";
            if (dia.ShowDialog() != DialogResult.OK) Application.Exit();

            dir = Path.GetDirectoryName(dia.FileName);
            CleanML();
            filename = Path.Combine(dir, "Temp");
            wc = new WebClient();
            wc.DownloadFileAsync(new Uri(downloadUrls[0]), filename + "0");
            wc.DownloadProgressChanged += DownloadProgress;
            wc.DownloadFileCompleted += DownloadComplete;
        }

        private void CleanML()
        {
            string[] files = new string[]
            {
                "version.dll",
                "NOTICE.txt"
            };
            string[] dirs = new string[]
            {
                "Logs",
                "MelonLoader",
                "Mods",
                "Plugins",
                "UserData"
            };
            foreach (string f in files)
            {
                string path = Path.Combine(dir, f);
                if (File.Exists(path)) File.Delete(path);
            }
            foreach (string f in dirs)
            {
                string path = Path.Combine(dir, f);
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("The download failed!\nPlease check your internet connection, otherwise contact the developer.", "Download failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            InstallFromZip();
            installed++;
            if (installed >= downloadUrls.Length)
            {
                button1.Enabled = true;
                button2.Text = "Finished!";
                return;
            }
            wc.DownloadFileAsync(new Uri(downloadUrls[installed]), filename + installed.ToString());
        }

        private void InstallFromZip()
        {
            string f = filename + installed.ToString();
            ZipFile.ExtractToDirectory(f, dir);
            File.Delete(f);
        }

        WebClient wc;
        int installed = 0;
        static string dir;
        static string filename;
        static readonly string[] downloadUrls = new string[]
        {
            "https://github.com/PikiGames/Advanced-SN-cheat/raw/main/Cheat1.zip",
            "https://github.com/PikiGames/Advanced-SN-cheat/raw/main/Cheat2.zip",
            "https://github.com/PikiGames/Advanced-SN-cheat/raw/main/Cheat3.zip"
        };
    }
}
