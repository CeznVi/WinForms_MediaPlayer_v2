using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPlayer.Playlist
{
    public class MediaRecord
    {
        private string _path;
        public MediaRecord(string pathToMedia)
        {
            Path = pathToMedia;
        }
        public string Path
        {
            get { return _path; }
            set
            {
                //файл существует
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"Файл по указанному пути {value} Не существует");
                }
                if (MediaFilter.IsMediaFile(value))
                {
                    _path = value;
                } else
                {
                    throw new ArgumentException($"Не корректный формат файла {value}");
                }
            }
        }
        public override string ToString()
        {
            return _path.Split('\\').Last();        
        }
    }
}
