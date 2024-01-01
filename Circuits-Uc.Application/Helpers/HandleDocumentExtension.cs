using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Helpers
{


    public static class HandleDocumentExtension
    {
        public static string GetExtension(string[] exts)
        {
            var current = exts;

            int lenght = exts.Length;

            string ext = exts[lenght - 1];
            return ext;
        }
    }
}
