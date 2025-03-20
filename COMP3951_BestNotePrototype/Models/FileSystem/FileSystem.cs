using Syncfusion.Maui.GridCommon.ScrollAxis;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

///
/// Will Otterbein
/// March 19 2025
///
namespace BestNote_3951.Models.FileSystem
{
    public interface IBNReadable
    {
        // StreamReader Reader { get; set; }
        string ReadFileContents();
    }

    public interface IBNWritable
    {
        // StreamWriter Writer { get; set; }
        void WriteToFile(string Content);
    }

    public interface IBNFile
    {
        FileInfo FileInfo { get; set; }
    }

    public interface IBNFolder
    {
        DirectoryInfo DirectoryInfo { get; set; }
        ObservableCollection<ITreeViewItem> Children { get; set; }
        // interface method for adding a child item.
        // define contract that these items need to define for UI purposes.
    }

    public interface ITreeViewItem : INotifyPropertyChanged
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public bool HasChildren { get; }
        public bool CanHaveChildren { get; }
        public Thickness IndentationPadding { get; set; }
        public IEnumerable<ITreeViewItem> SafeChildren { get; }
        public ImageSource ImageIcon { get; }
    }

    /// <summary>
    /// Base class for tree items.
    /// </summary>
    public abstract class TreeViewItemBase : ITreeViewItem
    {
        private string itemName;
        private int itemLevel;
        private Thickness indentationPadding;
        private ImageSource imageIcon;

        public int ItemLevel
        {
            get => itemLevel;
            set
            {
                itemLevel = value;
                RaisedOnPropertyChanged(nameof(ItemLevel));
            }
        }

        public Thickness IndentationPadding
        {
            get => indentationPadding;
            set
            {
                indentationPadding = value;
                RaisedOnPropertyChanged(nameof(value));
            }
        }

        public string ItemName
        {
            get => itemName;
            set
            {
                itemName = value;
                RaisedOnPropertyChanged(nameof(ItemName));
            }
        }

        public ImageSource ImageIcon
        {
            get => imageIcon;
            set
            {
                imageIcon = value;
                RaisedOnPropertyChanged(nameof(ImageIcon));
            }
        }

        public virtual bool HasChildren => false;

        public virtual bool CanHaveChildren => false;

        public virtual IEnumerable<ITreeViewItem> SafeChildren => Enumerable.Empty<ITreeViewItem>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisedOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Temporary tree item.
    /// </summary>
    public partial class TempTreeItem : TreeViewItemBase
    {
        private ITreeViewItem _parent;

        /// <summary>
        /// Constructor for the temporary tree view item, defines the parent of the item.
        /// </summary>
        /// <param name="indendationPadding"></param>
        /// <param name="itemLevel"></param>
        /// <param name="parent"></param>
        public TempTreeItem(
            int             itemLevel,
            Thickness       indendationPadding,
            ITreeViewItem   parent
        ) {
            _parent = parent;
            ImageIcon = "";
            ItemLevel = itemLevel;
            IndentationPadding = indendationPadding;
        }
    }

    /// <summary>
    /// Tree File Items.
    /// </summary>
    public partial class FileTreeItem : TreeViewItemBase, IBNFile
    {
        private readonly IBNFile _sourceFile;

        /// <summary>
        /// File tree file item class.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name=""></param>
        public FileTreeItem(
            int         itemLevel,
            Thickness   indentationPadding,
            IBNFile     sourceFile
        )
        {
            _sourceFile = sourceFile;
            ItemName = sourceFile.FileInfo.Name;
            ImageIcon = "md_file.png";
            ItemLevel = itemLevel;
            IndentationPadding = indentationPadding;
        }

        // Source file delegated implementations
        public FileInfo FileInfo
        {
            get => _sourceFile.FileInfo;
            set => _sourceFile.FileInfo = value;
        }
    }

    /// <summary>
    /// Tree Folder Items.
    /// </summary>
    public partial class FolderTreeItem : TreeViewItemBase, IBNFolder
    {
        private readonly IBNFolder _sourceFolder;

        /// <summary>
        /// File tree folder constructor.
        /// </summary>
        /// <param name="itemLevel"></param>
        /// <param name="indentationPadding"></param>
        /// <param name="sourceFolder"></param>
        public FolderTreeItem(
            int         itemLevel,
            Thickness   indentationPadding,
            IBNFolder   sourceFolder
        )
        {
            _sourceFolder = sourceFolder;
            ItemName = sourceFolder.DirectoryInfo.Name;
            ImageIcon = "folder_icon.png";
            ItemLevel = itemLevel;
            IndentationPadding = indentationPadding;
        }

        // Define folder specific tree view item properties
        public override bool CanHaveChildren => true;
        public override bool HasChildren => _sourceFolder.Children.Count > 0;

        // Source folder delegated implementation
        public DirectoryInfo DirectoryInfo
        {
            get =>  _sourceFolder.DirectoryInfo;
            set => _sourceFolder.DirectoryInfo = value;
        }

        public ObservableCollection<ITreeViewItem> Children
        {
            get => _sourceFolder.Children;
            set => _sourceFolder.Children = value;
        }

        public override IEnumerable<ITreeViewItem> SafeChildren
        {
            get => _sourceFolder.Children;
        }
    }
}
