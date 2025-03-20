using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 19 2025
/// 
namespace BestNote_3951.Views.TemplateSelectors
{
    /// <summary>
    /// Template selector for the FileStructureView.
    /// </summary>
    public class TreeViewItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate TemporaryTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Models.FileSystem.FileTreeItem)
            {
                return FileTemplate;
            }
            else if (item is Models.FileSystem.FolderTreeItem)
            {
                return FolderTemplate;
            }
            else
            {
                return TemporaryTemplate;
            }
        }
    }
}
