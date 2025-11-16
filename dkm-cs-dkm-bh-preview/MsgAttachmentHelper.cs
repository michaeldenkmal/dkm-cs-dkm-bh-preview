using dkm_cs_dkm_bh_preview;
using MsgReader.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public sealed class AttachmentInfo
{
    public string FileName { get; private set; }
    public string FullPath { get; private set; }

    public AttachmentInfo(string fileName, string fullPath)
    {
        FileName = fileName;
        FullPath = fullPath;
    }
}

public sealed class MailInfo
{
    public string Subject { get; private set; }
    public string PlainBody { get; private set; }
    public string HtmlBody { get; private set; }

    public string AttachmendRootDir { get; set; }
    public IReadOnlyList<AttachmentInfo> Attachments { get; private set; }

    public MailInfo(
        string subject,
        string plainBody,
        string htmlBody,
        IReadOnlyList<AttachmentInfo> attachments)
    {
        Subject = subject;
        PlainBody = plainBody;
        HtmlBody = htmlBody;
        Attachments = attachments ?? new List<AttachmentInfo>();
    }

    public void cleanUp()
    {
        // Zuerst die Ordner der Attachments files heraussuchen
        var folders = this.Attachments.Select(a => Path.GetDirectoryName(a.FullPath)).ToList();
        // die Attachments files löschen
        foreach (var attach  in this.Attachments)
        {
            // Wenn löschen fehlschlägt dann Ignorieren wenn die Datei in Verwendung ist
            try
            {
                File.Delete(attach.FullPath);
            } catch (System.IO.IOException)
            {
                FileCleanUpHolder.GetInst().addFileFp(attach.FullPath);
            } catch(Exception ex)
            {
                throw new Exception($"{ex.Message}:beim löschen von @@{attach.FullPath}@@:{ex}");
            }

        }
        // Die Ordner löschen, wenn sie leer sind
        foreach(var folder in folders)
        {
            try
            {
                Directory.Delete(folder);
            } catch (System.IO.IOException)
            {
                FileCleanUpHolder.GetInst().addFolderFp(folder);
            } catch (Exception ex)
            {
                throw new Exception($"{ex.Message}:beim löschen von @@{folder}@@:{ex}");
            }
        }

    }
}

public static class MsgAttachmentHelper
{
    public static void SaveAttachment (Storage.Attachment fileAttachment, string filePath)
    {
        if (fileAttachment == null)
            throw new ArgumentNullException(nameof(fileAttachment));

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // MSGReader: Data enthält die Datei als byte[]
        byte[] data = fileAttachment.Data;

        if (data == null || data.Length == 0)
        {
            // Falls Attachment leer ist, trotzdem Datei erzeugen
            File.WriteAllText(filePath + ".empty", "Attachment contained no data.");
            return;
        }

        File.WriteAllBytes(filePath, data);
    }
    public static List<AttachmentInfo> SaveMsgAttachments(Storage.Message message, string targetDir)
    {
        List<AttachmentInfo> savedFiles = new List<AttachmentInfo>();

        if (message == null)
            throw new ArgumentNullException(nameof(message));

        if (string.IsNullOrWhiteSpace(targetDir))
            throw new ArgumentNullException(nameof(targetDir));

        Directory.CreateDirectory(targetDir);

        int fileCnt = 0;

        foreach (var attObj in message.Attachments)
        {
            fileCnt++;
            // 1) Normale Datei-Anhänge
            if (attObj is Storage.Attachment fileAttachment)
            {
                string fileName = fileAttachment.FileName;
                string ext = Path.GetExtension(fileName);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"attach_{fileCnt}.attach";
                } else
                {
                    fileName = $"attach_{fileCnt}{ext}";
                }
                    

                string savePath = GetUniqueFilePath(Path.Combine(targetDir, fileName));

                // In MSGReader → Daten kommen über fileAttachment.Data
                SaveAttachment(fileAttachment, savePath );
                savedFiles.Add(new AttachmentInfo(fileName, savePath)); 
            }
            // 2) Eingebettete Mails (.msg)
            else if (attObj is Storage.Message embeddedMsg)
            {
                //string subject = embeddedMsg.Subject ?? "embedded_message";
                //subject = MakeSafeFilename(subject) + ".msg";
                string fileName = $"attach_{fileCnt}.msg";
                string savePath = GetUniqueFilePath(Path.Combine(targetDir, fileName));

                embeddedMsg.Save(savePath);
                savedFiles.Add(new AttachmentInfo(fileName, savePath));
            }
        }

        return savedFiles;
    }

    // Hilfsfunktion: Dateiname darf keine ungültigen Zeichen enthalten
    private static string MakeSafeFilename(string s)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            s = s.Replace(c, '_');

        return s.Replace(' ','_');
    }

    // Hilfsfunktion: unique filename erzeugen
    private static string GetUniqueFilePath(string path)
    {
        if (!File.Exists(path))
            return path;

        string dir = Path.GetDirectoryName(path);
        string name = Path.GetFileNameWithoutExtension(path);
        string ext = Path.GetExtension(path);

        int i = 1;
        string newPath;

        do
        {
            newPath = Path.Combine(dir, $"{name}_{i}{ext}");
            i++;
        }
        while (File.Exists(newPath));

        return newPath;
    }
    public static MailInfo ReadMsgAndSaveAttachments(string msgPath)
    {
        if (!File.Exists(msgPath))
            throw new FileNotFoundException("MSG-Datei nicht gefunden", msgPath);

        // Zielordner für Attachments
        //string attachmentsBaseDir = Path.Combine(
        //    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        //    "MsgAttachments");

        string attachmentsBaseDir = Path.Combine(
            Environment.GetEnvironmentVariable("TEMP"),
            "MsgAttachments");
        string msgFolderName = MakeSafeFilename(Path.GetFileNameWithoutExtension(msgPath));
        string attachmentDir = Path.Combine(attachmentsBaseDir, msgFolderName);
        Directory.CreateDirectory(attachmentDir);

        using (var message = new Storage.Message(msgPath))
        {
            // Betreff + Body
            string subject = message.Subject ?? "(kein Betreff)";
            string bodyText = message.BodyText ?? "";
            string bodyHtml = message.BodyHtml ?? "";
            var attachInfos = SaveMsgAttachments(message, attachmentDir);
            return new MailInfo(subject, plainBody: bodyText, htmlBody: bodyHtml, attachments: attachInfos);
        }
    }

}
