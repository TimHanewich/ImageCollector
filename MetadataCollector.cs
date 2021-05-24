using System;
using System.IO;
using TimHanewich.ImageMetadata;
using System.Collections.Generic;

namespace ImageCollector
{
    public class MetadataCollector
    {
        public event StatusChange StatusChanged;

        public FileImageMetadata[] DeepCollectMetadata(string root_dir_path)
        {
            //Search this folder
            List<FileImageMetadata> ToReturn = new List<FileImageMetadata>();
            FileImageMetadata[] FromThisFolder = CollectMetadata(root_dir_path);
            ToReturn.AddRange(FromThisFolder);

            //Deep search
            string[] child_dirs = System.IO.Directory.GetDirectories(root_dir_path);
            foreach (string s in child_dirs)
            {
                FileImageMetadata[] fims = DeepCollectMetadata(s);
                ToReturn.AddRange(fims);
            }

            return ToReturn.ToArray();
        }

        public FileImageMetadata[] CollectMetadata(string dir_path)
        {
            TryUpdateStatus("Getting files for directory '" + dir_path + "'");
            string[] files = System.IO.Directory.GetFiles(dir_path);
            TryUpdateStatus(files.Length.ToString("#,##0") + " files found.");
            List<FileImageMetadata> ToReturn = new List<FileImageMetadata>();
            foreach (string s in files)
            {
                if (s.ToLower().Contains(".jpg") || s.ToLower().Contains(".jpeg") || s.ToLower().Contains(".png"))
                {
                    TryUpdateStatus("Getting metadata for file '" + s + "'");
                    FileImageMetadata fim = new FileImageMetadata();
                    fim.Path = s;
                    try
                    {
                        fim.Metadata = ImageMetadata.Read(System.IO.File.OpenRead(s));
                        ToReturn.Add(fim);
                    }
                    catch
                    {

                    }
                }
            }
            return ToReturn.ToArray();
        }

        private void TryUpdateStatus(string status)
        {
            if (StatusChanged != null)
            {
                StatusChanged.Invoke(status);
            }
        }
    }

    public delegate void StatusChange(string status);
}