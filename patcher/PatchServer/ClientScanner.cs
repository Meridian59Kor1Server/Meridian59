﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace PatchListGenerator
{
    public class ClientScanner
    {
        private List<string> ScanFolder { get; set; }
        private List<string> ScanExtensions { get; set; }
        public List<ManagedFile> Files { get; set; }
        public string BasePath { get; set; }
        
        public ClientScanner()
        {
            ScanFolder = new List<string>
            {
                "C:\\Meridian59-master\\run\\localclient\\",
                "C:\\Meridian59-master\\run\\localclient\\resource",
                "C:\\Meridian59-master\\run\\localclient\\d3dfonts"
            };
            AddExensions();
        }

        public ClientScanner(string basepath)
        {
            ScanFolder = new List<string> { basepath, basepath + "\\resource", basepath + "\\d3dfonts" };
            AddExensions();
        }

        private void AddExensions()
        {
            ScanExtensions = new List<string> {".roo", ".dll", ".rsb", ".exe", ".bgf", ".wav", ".mp3", ".ttf", ".bsf", ".dfd"};
        }

        public void ScanSource()
        {
            Files = new List<ManagedFile>();
            foreach (string folder in ScanFolder) //
            {
                if (!Directory.Exists(folder))
                {
                    //Folder doesn't exist =(
                    throw new Exception();
                }
                // Process the list of files found in the directory. 
                string[] fileEntries = Directory.GetFiles(folder);
                foreach (string fileName in fileEntries)
                {
                    string ext = fileName.Substring(fileName.Length - 4).ToLower();
                    if (ScanExtensions.Contains(ext))
                    {
                        var file = new ManagedFile(fileName);
                        file.ParseFilePath();
                        file.ComputeHash();
                        file.FillLength();
                        Files.Add(file);
                    }
                }
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Files, Formatting.Indented);
        }

        public void Report()
        {
            foreach (ManagedFile file in Files)
                Console.WriteLine(file.ToString());
        }

    }
}
