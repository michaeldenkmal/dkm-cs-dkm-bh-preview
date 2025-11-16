using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dkm_cs_dkm_bh_preview
{
    public class FileCleanUpHolder
    {
        private List<string> filesToCleanUp;
        private List<string> folderToCleanUp;

        private FileCleanUpHolder()
        {
            this.filesToCleanUp = new List<string>();
            this.folderToCleanUp = new List<string>();
        }

        private static FileCleanUpHolder _inst;
        public static FileCleanUpHolder GetInst()
        {
            if (_inst == null)
            {
                _inst = new FileCleanUpHolder();
            }
            return _inst;
        }

        private void addToFpList(List<string> lst, string fp)
        {
            if (fp == null)
            {
                return;
            }
            string uc_fp = fp.ToLower();
            if (lst.IndexOf(uc_fp) == -1)
            {
                lst.Add(uc_fp);
            }
        }

        public void addFileFp(string fp)
        {
            addToFpList(filesToCleanUp, fp);
        }
        public void addFolderFp(string fp)
        {
            addToFpList(folderToCleanUp, fp);
        }


        private void cleanUpFiles()
        {
            foreach (var fp in filesToCleanUp)
            {
                try
                {
                    File.Delete(fp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}: bei löschen von Datei @@{fp}@@: {ex}");
                }
            }
        }

        private void cleanUpFolder()
        {
            foreach (var fp in folderToCleanUp)
            {
                try
                {
                    Directory.Delete(fp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}: bei löschen von Ordner @@{fp}@@: {ex}");
                }
            }

        }

        public void cleanUp()
        {
            cleanUpFiles();
            cleanUpFolder();
        }
    }


}
