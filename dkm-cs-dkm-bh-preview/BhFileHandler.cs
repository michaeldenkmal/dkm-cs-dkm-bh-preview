using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{

    // Welche Files gibts?
    // pdf 
    // msg
    public enum BhFileType
    {
        pdf = 1,
        msg = 2
    }

    
    public abstract class BhFileBase
    {
        protected string _rootFullFilePath;

        public BhFileBase(string rootFullFilePath)
        {
            this._rootFullFilePath = rootFullFilePath;
        }

        public abstract BhFileType getFileType();

        public abstract IEnumerable<string> GetPdfilePaths();
        public abstract void cleanUp();

        public string OrigFileName
        {
            get
            {
                return _rootFullFilePath;
            }
        }
    }

    public class BhFilePdf : BhFileBase
    {
        public BhFilePdf(string rootFullFilePath) : base(rootFullFilePath)
        {
        }

        public override void cleanUp()
        {
            // Nur die Pdf Datei löschen, nicht notwendig
        }

        public override BhFileType getFileType()
        {
            return BhFileType.pdf;
        }


        public override IEnumerable<string> GetPdfilePaths()
        {
            yield return this._rootFullFilePath;
        }
    }

    

    public class BhFileMsg : BhFileBase
    {

        private MailInfo _mailInfo;
        public BhFileMsg(string rootFullFilePath, MailInfo mailInfo) : base(rootFullFilePath)
        {
            this._mailInfo = mailInfo;
        }

        public override void cleanUp()
        {
            this._mailInfo.cleanUp();
        }

        public override BhFileType getFileType()
        {
            return BhFileType.msg;
        }

        public override IEnumerable<string> GetPdfilePaths()
        {
            foreach (var attach in this._mailInfo.Attachments)
            {
                var ext = Path.GetExtension(attach.FullPath);
                if (string.Compare(ext, ".PDF",true)==0)
                {
                    yield return attach.FullPath;
                }
            }
        }
    }




    public static class BhFileHandler
    {
        public class PdfFileAndCleanUp
        {
            public string PdfFp { get; private set; }
            public BhFileBase BhFileBaseInst { get; private set; }

            public PdfFileAndCleanUp(string pdfFp, BhFileBase bhFileBaseInst)
            {
                PdfFp = pdfFp;
                BhFileBaseInst = bhFileBaseInst;
            }
        }

        public static IEnumerable<PdfFileAndCleanUp> buildPdfFileIterator(List<BhFileBase> lstBhFileBase)
        {
            foreach (var fb in lstBhFileBase)
            {
                foreach (var pdffp in fb.GetPdfilePaths())
                {
                    PdfFileAndCleanUp retv = new PdfFileAndCleanUp(pdffp, fb);
                    yield return retv;
                }
            }
        }

        public static List<BhFileBase> readBhRootFolder(string rootFolderPath)
        {

            List<BhFileBase> ret = new List<BhFileBase>();
            string[] files;
            try
            {
                files = Directory.GetFiles(rootFolderPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}:readBhRootFolder:rootFolderPath={rootFolderPath}:{ex.ToString()}");
            }

            foreach (string fpfile in files)
            {
                BhFileBase fb;
                if (TryfileToBhBase(fpfile, out fb))
                {
                    ret.Add(fb);
                }
            }
            return ret;
        }

        public static bool TryfileToBhBase(string fp, out BhFileBase bhFileBase)
        {
            string ext = Path.GetExtension(fp).ToUpper();
            bhFileBase = null;
            if (ext == ".PDF")
            {
                 bhFileBase = new BhFilePdf(fp);
                return true;                    
            } else if (ext ==".MSG")
            {
                MailInfo mailInfo = MsgAttachmentHelper.ReadMsgAndSaveAttachments(fp);
                bhFileBase= new BhFileMsg(fp, mailInfo);
                return true;
            }
            return false;
        }
    }
}
