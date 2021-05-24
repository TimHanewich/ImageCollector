using System;
using TimHanewich.ImageMetadata;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ImageCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            MetadataCollector mc = new MetadataCollector();
            mc.StatusChanged += PrintStatus;
            FileImageMetadata[] fims = mc.DeepCollectMetadata(@"C:\Users\tahan\Downloads\MyPhotos");
            System.IO.File.WriteAllText(@"C:\Users\tahan\Downloads\meta.json", JsonConvert.SerializeObject(fims));
        }

        public static void PrintStatus(string s)
        {
            Console.WriteLine(s);
        }

        
    }
}
