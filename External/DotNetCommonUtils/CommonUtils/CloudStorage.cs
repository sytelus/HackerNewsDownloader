using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public static class CloudStorage
    {
        private const string DropBoxHostFileName = "Dropbox\\host.db";

        private static Lazy<string> dropBoxBasePathLazy = new Lazy<string>(() => {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dbPath = Path.Combine(appDataPath, DropBoxHostFileName);

            if (!File.Exists(dbPath))
                return null;

            var lines = File.ReadAllLines(dbPath);
            var dbBase64Text = Convert.FromBase64String(lines[1]);
            return Encoding.UTF8.GetString(dbBase64Text);
        });

        public static string GetDropBoxPath(string relativePathWithinDropBox = null)
        {
            if (dropBoxBasePathLazy.Value != null && relativePathWithinDropBox != null)
                return Path.Combine(dropBoxBasePathLazy.Value, relativePathWithinDropBox);
            else
                return dropBoxBasePathLazy.Value;
        }

    }
}
