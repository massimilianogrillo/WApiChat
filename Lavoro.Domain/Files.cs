using Extensions;
using System;
using System.Collections.Generic;

namespace Lavoro.Domain
{
    public enum FileType
    {
        Folder,
        Document,
        Spreadsheet,
        Pdf,
        Image,
        Other
    }
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FileType FileType { get; set; }
        public string Type
        {
            get {
                return FileType.ToString().ToLower();
            }
            set {
                if (value == "folder")
                    FileType = FileType.Folder;
                else if (value == "document")
                    FileType = FileType.Document;
                else if (value == "spreadsheet")
                    FileType = FileType.Spreadsheet;
                else if (value.ToLower().Contains("pdf"))
                    FileType = FileType.Pdf;
                else if (
                    value.ToLower().ContainsAny(
                        new List<string> { "img", "image", "png", "gif", "jpg" }
                        ))
                    FileType = FileType.Image;
                else
                    FileType = FileType.Other;
            }

        }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public string Size { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Opened { get; set; }
        public DateTime Created { get; set; }
        public string Extention { get; set; }
        public string Location { get; set; }
        public bool Offline { get; set; }
        public string Preview { get; set; }

        public static string Owner(User user)
        {
            if (user == null)
                return "public";
            return user.Name;
        }
    }
}
