using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfControls.CS.Test.Properties;

namespace WpfControls.CS.Test
{
    public class FileIconConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Icon iconFile = SystemIcons.WinLogo;

            if (value is FileSystemInfo fsInfo)
            {
                try
                {
                    if (fsInfo is DirectoryInfo)
                    {
                        iconFile = Resources.folder_2_32;
                    }
                    else
                    {
                        string filePath = fsInfo.FullName;

                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                            iconFile = Icon.ExtractAssociatedIcon(filePath);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return ConvertToImageSource(iconFile);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private static ImageSource ConvertToImageSource(Icon icon)
        {
            using (Icon i = Icon.FromHandle(icon.Handle))
            {

                ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                                        i.Handle,
                                        new Int32Rect(0, 0, i.Width, i.Height),
                                        BitmapSizeOptions.FromEmptyOptions());
                return img;
            }
        }

        #endregion Private Methods
    }
}