using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.UWP
{
    public static class FileAccessHelper
    {
        public static string GetLocalFilePath(string fileName)
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            return System.IO.Path.Combine(folder, fileName);
        }
    }
}
