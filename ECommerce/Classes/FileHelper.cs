using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class FileHelper
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string fileName)
        {
            if(file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            
            try
            {
                string path = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), fileName);
                    file.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool UpdatePhoto(string name)
        {
            try
            {
                var file = new FileInfo(HttpContext.Current.Server.MapPath(name));
                if (!file.Exists)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeletePhoto(string name)
        {
            try
            {
                var file = new FileInfo(HttpContext.Current.Server.MapPath(name));
                if (!file.Exists)
                {
                    return false;
                }
                file.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}