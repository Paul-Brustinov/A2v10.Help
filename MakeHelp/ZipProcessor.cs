﻿// Copyright © 2012-2017 Alex Kukhtin. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeHelp
{
    class ZipProcessor
    {
        String _fileName;
        String _dirName;

        public String FileName => _fileName;

        public void Process(String dir)
        {
            _dirName = dir;
            _fileName = Path.Combine(dir, "..\\..\\apache.zip");
            _fileName = Path.GetFullPath(_fileName);
            WriteFile();
        }

        void WriteFile()
        {
            if (File.Exists(_fileName))
                File.Delete(_fileName);

            String[] rootFiles = { "index.html", "404.html", "robots.txt", ".htaccess" };

            using (var za = ZipFile.Open(_fileName, ZipArchiveMode.Create))
            {
                foreach (var rf in rootFiles) {
                    String fn = Path.Combine(_dirName, rf);
                    za.CreateEntryFromFile(fn, rf);
                }
                AddFilesFromDirectory(za, "css");
                AddFilesFromDirectory(za, "scripts");
                AddFilesFromDirectory(za, "html");
            }
        }

        void AddFilesFromDirectory(ZipArchive za, String dir)
        {
            String srcDir = Path.Combine(_dirName, dir);
            foreach(var f in Directory.GetFiles(srcDir))
            {
                String fn = Path.GetFileName(f);
                za.CreateEntryFromFile(f, Path.Combine(dir, fn).Replace("\\", "/"));
            }
            foreach (var d in Directory.GetDirectories(srcDir))
            {
                String subDir = Path.Combine(dir, Path.GetFileName(d)).Replace("\\", "/");
                AddFilesFromDirectory(za, subDir);
            }
        }
    }
}
