using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPlayer.Playlist
{
    static class GetFileName
    {
        public static string From(string path) 
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Name;
        }
    }
}
