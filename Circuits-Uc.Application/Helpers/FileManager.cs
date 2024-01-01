using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CircuitsUc.Application.Helpers
{
  

    public static class FileManager
    {
        public static bool CheckFilePath(string physicalPath)
        {
            try
            {

                if (Directory.Exists(physicalPath))
                    return true;

                Directory.CreateDirectory(physicalPath);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool DeleteFolder(string folder)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    var files = Directory.GetFiles(folder);
                    if (files.Length > 0)
                    {
                        foreach (var f in files)
                        {
                            File.Delete(f);
                        }
                    }

                    Directory.Delete(folder);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool DeleteSpecificFile(string specificFile)
        {
            try
            {
                if (File.Exists(specificFile))
                {
                    File.Delete(specificFile);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool IsSpecificFileExist(string specificFile)
        {
            try
            {
                if (File.Exists(specificFile))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool IsVideo(string fileName)
        {
            var postedFileExtension = Path.GetExtension(fileName);

            if (string.Equals(postedFileExtension, ".mp4", StringComparison.OrdinalIgnoreCase)
                || string.Equals(postedFileExtension, ".AVI", StringComparison.OrdinalIgnoreCase)
                || string.Equals(postedFileExtension, ".wmv", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
        public static bool IsImage(string fileName)
        {
            var postedFileExtension = Path.GetExtension(fileName);

            if (string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                || string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                || string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                || string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static void AddFileBase64(Guid RefEntityID, Guid docCode, string enumVal,string FileName ,string base64String)
        {
            string path = DocumentController.GetLocatedImagePath(Directory.GetCurrentDirectory(), enumVal, RefEntityID);
            string savingPath = Path.Combine(path, $"{docCode}{Path.GetExtension(FileName)}");

            // Convert Base64 string to byte array
            byte[] fileBytes = Convert.FromBase64String(base64String);
            // Save byte array as a file
            File.WriteAllBytes(savingPath, fileBytes);
        }

    }
}
