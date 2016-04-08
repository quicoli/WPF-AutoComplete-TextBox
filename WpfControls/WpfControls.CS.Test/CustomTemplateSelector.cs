using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.CS.Test
{
    public class CustomTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }
        public DataTemplate DirectoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FileInfo)
                return FileTemplate;
            else if (item is DirectoryInfo)
                return DirectoryTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
