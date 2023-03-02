using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinPlayer.Playlist;
using WMPLib;

namespace WinPlayer
{
    public partial class MainForm : Form
    {
        private PlayListsForm _playListsForm;
        public PlayListController PlayListsController;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PlayListsController = new PlayListController(PlayListXMLMap.Path);
        }
        public AxWindowsMediaPlayer MediaPlayer
        {
            get
            {
                return WindowsMediaPlayer;
            }
        }
        private void плейлистToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_playListsForm == null)
            {
                _playListsForm = new PlayListsForm(this);
            }
            _playListsForm.Location = new Point(this.Location.X + this.Width - 13, this.Location.Y);
            _playListsForm.Show();
        }


        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = MediaFilter.GetOpenFileDialogFilter();

            //Директория которая откроется по умолчанию (стоит папка медиа проекта)
            openFileDialog.InitialDirectory = GetPath.MediaDir();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MediaPlayer.URL = openFileDialog.FileName;

                this.toolStripStatusLabelInfo.Text =
                    $"Сейчас воспроизводится: {GetFileName.From(openFileDialog.FileName)}";
            }
        }

        /// <summary>
        /// Публичный метод для изменения строки состояния 
        /// </summary>
        /// <param name="path"></param>
        public void setPlayStatus(string path)
        {
            this.toolStripStatusLabelInfo.Text =
                  $"Сейчас воспроизводится: {GetFileName.From(path)}";
        }

        private void WindowsMediaPlayer_Enter(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            плейлистToolStripMenuItem_Click(sender, e);
            _playListsForm.openAllMediaInFolder();
        }
    }
}
