using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NeedleworkStore.Classes
{
    public class ImagePathConverter : IValueConverter
    {
        private const string BaseResourcePath = "/ProdImages/";
        private const string DefaultImage = "/ResImages/NoPicture.png";
        private static readonly string ProjectRootPath = GetProjectRootPath();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string imgName) || string.IsNullOrWhiteSpace(imgName))
                return DefaultImage;
            string resourcePath = BaseResourcePath + imgName;
            if (ResourceExists(resourcePath))
                return resourcePath;
            string filePath = System.IO.Path.Combine(ProjectRootPath, "ProdImages", imgName);
            if (File.Exists(filePath))
                return filePath;
            return DefaultImage;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        private bool ResourceExists(string resourcePath)
        {
            try
            {
                var resource = Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));
                return resource != null;
            }
            catch
            {
                return false;
            }
        }
        private static string GetProjectRootPath()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(exePath);
            for (int i = 0; i < 2; i++)
            {
                if (directory.Parent != null)
                    directory = directory.Parent;
            }
            return directory.FullName;
        }
    }
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if (int.TryParse(value.ToString(), out int statusId))
                return statusId == 1;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
