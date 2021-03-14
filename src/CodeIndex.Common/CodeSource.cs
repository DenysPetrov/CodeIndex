﻿using System;
using System.IO;

namespace CodeIndex.Common
{
    public class CodeSource
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public string Content { get; set; }
        public DateTime IndexDate { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public string CodePK { get; set; } = Guid.NewGuid().ToString("N");
        public string Info => $"FileName: {FileName}{Environment.NewLine}FilePath: {FilePath}{Environment.NewLine}Index Date: {IndexDate.ToLocalTime()}{Environment.NewLine}Last Modify Date:{LastWriteTimeUtc.ToLocalTime()}";

        public static CodeSource GetCodeSource(FileInfo fileInfo, string content)
        {
            return new CodeSource
            {
                FileExtension = fileInfo.Extension.Replace(".", string.Empty),
                FileName = fileInfo.Name,
                FilePath = fileInfo.FullName,
                IndexDate = DateTime.UtcNow,
                Content = content,
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc
            };
        }
    }
}
