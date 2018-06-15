using System.IO;
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
            return item is DirectoryInfo ? DirectoryTemplate : base.SelectTemplate(item, container);
        }
    }
}