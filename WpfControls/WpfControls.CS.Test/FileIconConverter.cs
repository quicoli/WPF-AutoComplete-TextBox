using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControls.CS.Test
{
    public class FileIconConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Icon iconFile = SystemIcons.WinLogo;
            FileSystemInfo fsInfo = value as FileSystemInfo;

            if (fsInfo != null)
            {
                try
                {
                    if (fsInfo is DirectoryInfo)
                    {
                        iconFile = Properties.Resources.folder_2_32;
                    }
                    else
                    {
                        string filePath = fsInfo.FullName;

                        if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                            iconFile = Icon.ExtractAssociatedIcon(filePath);
                    }
                }
                catch (Exception)
                {
                    
                }
                
            }
            return ConvertToImageSource(iconFile);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
