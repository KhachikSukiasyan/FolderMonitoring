using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FolderMonitoring
{
   public static class Monitor
    {
        static FileSystemWatcher watcher;

        public static void follow(string input)
        {
            try
            {
                if (!Directory.Exists(input))
                {
                    throw new DirectoryNotFoundException();
                }
                else
                {
                    watcher.Path = input;
                    watcher.EnableRaisingEvents = true;

                    watcher.Renamed += new RenamedEventHandler(OnRenamed);
                    watcher.Created += new FileSystemEventHandler(OnCreatedOrDeletedChanged);
                    watcher.Deleted += new FileSystemEventHandler(OnCreatedOrDeletedChanged);
                    watcher.Changed += new FileSystemEventHandler(OnCreatedOrDeletedChanged);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Wrong input directory");
            }
        }


        static Monitor()
        {
            watcher = new FileSystemWatcher();
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            FileAttributes attr = File.GetAttributes(e.FullPath);
            if (attr.HasFlag(FileAttributes.Directory))
                Console.WriteLine("Folder: {0} renamed to {1}", Path.GetFileName(e.OldFullPath), Path.GetFileName(e.FullPath));
            else
                Console.WriteLine("File: {0} renamed to {1}", Path.GetFileName(e.OldFullPath), Path.GetFileName(e.FullPath));
        }

        private static void OnCreatedOrDeletedChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                FileAttributes attr = File.GetAttributes(e.FullPath);

                if (attr.HasFlag(FileAttributes.Directory))
                    Console.WriteLine(e.ChangeType + " Folder: {0}", Path.GetFileName(e.FullPath));
                else
                    Console.WriteLine(e.ChangeType + " File: {0}", Path.GetFileName(e.FullPath));
            }
            else
                Console.WriteLine("Deleted {0}", Path.GetFileName(e.FullPath));
        }

    }
}
