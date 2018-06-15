using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WpfControls.Editors;

namespace WpfControls.CS.Test
{
    public class FilesystemSuggestionProvider : ISuggestionProvider
    {

        public IEnumerable GetSuggestions(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }
            if (filter.Length < 3)
            {
                return null;
            }

            if (filter[1] != ':')
            {
                return null;
            }

            List<FileSystemInfo> lst = new List<FileSystemInfo>();
            string dirFilter = "*";
            string dirPath = filter;
            if (!filter.EndsWith("\\"))
            {
                int index = filter.LastIndexOf("\\", StringComparison.Ordinal);
                dirPath = filter.Substring(0, index + 1);
                dirFilter = filter.Substring(index + 1) + "*";
            }
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            lst.AddRange(dirInfo.GetDirectories(dirFilter));
            lst.AddRange(dirInfo.GetFiles(dirFilter));
            return lst;
        }
    }
}