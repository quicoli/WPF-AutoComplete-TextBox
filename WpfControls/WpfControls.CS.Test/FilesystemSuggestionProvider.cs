namespace WpfControls.CS.Test
{

    using System.Collections.Generic;
    using System.IO;
    using WpfControls.Editors;
    using System.Linq;
    using System.Threading;

    public class FilesystemSuggestionProvider : ISuggestionProvider
    {

        public System.Collections.IEnumerable GetSuggestions(string filter)
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

            List<System.IO.FileSystemInfo> lst = new List<System.IO.FileSystemInfo>();
            string dirFilter = "*";
            string dirPath = filter;
            if (!filter.EndsWith("\\"))
            {
                int index = filter.LastIndexOf("\\");
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
