using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{
    public static class CreMoveFileUtil
    {

        public static void SetupSubDirsInBhFolder(string bhFolder)
        {
            string[] subdirs = new string[]
            {
                "upload","uploaded","erledigt"
            };

            foreach (var subdir in subdirs)
            {
                string fpfld = Path.Combine(bhFolder, subdir);
                if (!Directory.Exists(fpfld))
                {
                    Directory.CreateDirectory(fpfld);
                }
            }

        }

        public static string buildErFileName(string pdfFullPath, DateTime belegDate, string info, decimal betrag)
        {
            string ext = Path.GetExtension(pdfFullPath);
            string fnameonly = Path.GetFileNameWithoutExtension(pdfFullPath);
            int year2Digit = belegDate.Year - 2000;
            int mon = belegDate.Month;
            int tag = belegDate.Day;
            return $"{year2Digit.ToString("00")}{mon.ToString("00")}{tag.ToString("00")}-{info}-{betrag.ToString("N2")}.pdf";
        }

        public static void copyFileToErFolder(string pdfFullPath, DateTime belegDate, string info, decimal betrag, string outputErFolder, string origFileHandledFolder,
            string origFileFullName)
        {
            // zuerst neuen Dateinamen erzeugen
            string erFileName = buildErFileName(pdfFullPath, belegDate, info, betrag);
            // pdfFullPath zu neuem Dateiname  kopieren
            string erDstFp = Path.Combine(outputErFolder, erFileName);
            try
            {
                File.Copy(pdfFullPath, erDstFp,overwrite:true);
            } catch (Exception ex)
            {
                throw new Exception($"{ex.Message}: beim kopieren von @@{pdfFullPath}@@ nach @@{erDstFp}@@:{ex}");
            }
            // Originale Datei verschiebn
            string fileName = Path.GetFileName(origFileFullName);
            string dstFileName = Path.Combine(origFileHandledFolder, fileName);
            if (!Directory.Exists(origFileHandledFolder))
            {
                Directory.CreateDirectory(origFileHandledFolder);
            }
            if (File.Exists(dstFileName))
            {
                File.Delete(dstFileName);
            }
            moveFile(origFileFullName, dstFileName);
        }

        public static void moveOrigFile(string fpOrig, string dstFolderPath)
        {
            string dstFp = Path.Combine(dstFolderPath, fpOrig);
            if (File.Exists(dstFp))
            {
                File.Delete(dstFp);
            }
            moveFile(fpOrig, dstFp);
        }

        public static void moveFile(string srcFp, string  dstFp)
        {
            try
            {
                File.Move(srcFp, dstFp);
            } catch (Exception ex)
            {
                throw new IOException($"{ex.Message}: beim verschieben von @@{srcFp}@@ nach @@{dstFp}@@:{ex}");
            }
        }
    }
}
