﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WinPlayer.Playlist
{
    public class PlayListController
    {
        private string _pathToXMLFile;
        public List<PlayList> PlayLists;
        private XDocument _xDocument;
        private XElement _rootElement;
        
        public string PathToXml
        {
            get { return _pathToXMLFile; }
            set
            {
                if (!File.Exists(value))
                {
                   throw new FileNotFoundException();
                }
                if (Path.GetExtension(value).ToLower() != ".xml")
                {
                    throw new ArgumentException("Не корректный формат файла");
                }
                _pathToXMLFile = value;
            }
        }

        public PlayListController(string pathToXml)
        {
            PlayLists = new List<PlayList>();
            PathToXml = pathToXml;
            LoadXmlData();
        }

        private void LoadXmlData()
        {
            _xDocument = XDocument.Load(PathToXml);

            _rootElement = _xDocument.Element(PlayListXMLMap.Root.ElementName);

            try
            {
                IEnumerable<XElement> playlistElements = _rootElement.Elements(PlayListXMLMap.Root.PlayList.ElementName);

                foreach (var onePlayList in playlistElements)
                {
                    PlayList tmp = new PlayList();
                    tmp.MediaRecords = new List<MediaRecord>();
                    tmp.Name = onePlayList.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value;

                    IEnumerable<XElement> mediaRecords = onePlayList.Elements(PlayListXMLMap.Root.PlayList.MediaRecord.ElementName);
                    foreach (var oneRecord in mediaRecords)
                    {
                        tmp.MediaRecords.Add(new MediaRecord(oneRecord.Attribute(PlayListXMLMap.Root.PlayList.MediaRecord.Attributes.Path).Value));
                    }
                    PlayLists.Add(tmp);
                }
            }
            catch (Exception e)
            {
                var error = MessageBox.Show($"Ошибка при загрузке данных из ХМЛ:\n\n{e.Message.ToUpper()}\n\nЧтобы избавиться от" +
                    $" ошибки нажмите ОК (хмл файл будет очищен)\n" +
                    $"Если нажать отмена - приложение закроется", "Ошибка при загрузке", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (error == DialogResult.OK)
                {
                    string baseFile = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Playlists/>\r";  
                    File.Delete(_pathToXMLFile);
                    File.Create(_pathToXMLFile).Close();
                    File.WriteAllText(_pathToXMLFile, baseFile);
                    LoadXmlData();
                }

            }
        }

        public void AddNewPlayList(PlayList playlist)
        {
            PlayLists.Add(playlist);

            _rootElement.Add(
                new XElement(PlayListXMLMap.Root.PlayList.ElementName, new XAttribute(PlayListXMLMap.Root.PlayList.Attributes.Name, playlist.Name))
            );
            _xDocument.Save(PathToXml);
        }

        public void RemovePlayList(string playListName)
        {
            IEnumerable<XElement> playlists = _rootElement.Elements(PlayListXMLMap.Root.PlayList.ElementName);
            foreach (XElement item in playlists)
            {
                if (item.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value == playListName)
                {
                    item.Remove();
                }
            }

            _xDocument.Save(PathToXml);
        }

        public void AddNewMediaRecord(MediaRecord mediaRecord, PlayList playlist)
        {
            IEnumerable<XElement> playlists = _rootElement.Elements(PlayListXMLMap.Root.PlayList.ElementName);
            foreach (XElement item in playlists)
            {
                if (item.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value == playlist.Name)
                {
                    item.Add(new XElement(
                            PlayListXMLMap.Root.PlayList.MediaRecord.ElementName,
                            new XAttribute(PlayListXMLMap.Root.PlayList.MediaRecord.Attributes.Path, mediaRecord.Path)
                    ));
                    _xDocument.Save(PathToXml);
                    return;
                }
            }
        }

        public void RemoveMediaRecord(MediaRecord mediaRecord, PlayList playlist)
        {
            IEnumerable<XElement> playlists = _rootElement.Elements(PlayListXMLMap.Root.PlayList.ElementName);
            
            foreach (XElement item in playlists)
            {
                if (item.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value == playlist.Name)
                {
                    IEnumerable<XElement> mediaRecords = item.Elements(PlayListXMLMap.Root.PlayList.MediaRecord.ElementName);
                    foreach (XElement mediaRec in mediaRecords)
                    {
                        if (mediaRec.Attribute(PlayListXMLMap.Root.PlayList.MediaRecord.Attributes.Path).Value == mediaRecord.Path)
                        {
                            mediaRec.Remove();
                            _xDocument.Save(PathToXml);
                            return;
                        }
                    }
                }
            }
        }

        public void RenamePlayList(string oldName, string newName)
        {
            IEnumerable<XElement> playlists = _rootElement.Elements(PlayListXMLMap.Root.PlayList.ElementName);
            foreach (XElement item in playlists)
            {
                if (item.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value == oldName)
                {
                    item.Attribute(PlayListXMLMap.Root.PlayList.Attributes.Name).Value = newName;
                    _xDocument.Save(PathToXml);
                    return;
                }
            }
        }
    }
}