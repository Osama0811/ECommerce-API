
using CircuitsUc.Application.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Helpers
{

    public static class DocumentController
    {
        const string _prePath = "Attachments";

        public static string GetImage(Guid RefEntityID, Guid docCode,string enumVal, string fileName)
        {
            return $"{_prePath}/{enumVal}/{RefEntityID}/{docCode}{Path.GetExtension(fileName)}";
        }
        public static string GetLocatedImagePath(string serverPath,string enumVal, Guid RefEntityID)
        {
            string path = $"{serverPath}/wwwroot/{_prePath}/{enumVal}/{RefEntityID}";
            FileManager.CheckFilePath(path);
            return path;
        }
    }
}
