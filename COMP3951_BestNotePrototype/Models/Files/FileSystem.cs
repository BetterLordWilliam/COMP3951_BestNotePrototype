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

    public interface IBNFile : IBNWritable, IBNReadable
    {
        FileInfo FileInfo { get; set; }
    }

    public interface IBNFolder
    {
        DirectoryInfo DirectoryInfo { get; set; }
        ObservableCollection<ITreeViewItem> Children { get; }
    }

    public interface ITreeViewItem : INotifyPropertyChanged
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public bool HasChildren { get; set; }
        public bool CanHaveChildren { get; }
        public Thickness IndentationPadding { get; set; }
        public ImageSource ImageIcon { get; }
    }

    public class FileTreeItem : ITreeViewItem, IBNFile
    {
        /// <summary>
        /// File tree file item class.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name=""></param>
        public FileTreeItem(
            int itemLevel,
            Thickness indentationPadding,
            IBNFile sourceFile
        )
        {
            _sourceFile = sourceFile;
            itemName = sourceFile.FileInfo.Name;
            imageIcon = "md_file.png";

            this.itemLevel = itemLevel;
            this.indentationPadding = indentationPadding;
        }

        private readonly IBNFile _sourceFile;

        private string itemName;
        private int itemLevel;
        private Thickness indentationPadding;
        private ImageSource imageIcon;

        public bool CanHaveChildren { get; } = false;

        public bool HasChildren { get; set; } = false;

        public FileInfo FileInfo
        {
            get => _sourceFile.FileInfo;
            set => _sourceFile.FileInfo = value;
        }

        public void WriteToFile(string Content) => _sourceFile.WriteToFile(Content);

        public string ReadFileContents() => _sourceFile.ReadFileContents();

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisedOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FolderTreeItem : ITreeViewItem, IBNFolder
    {
        /// <summary>
        /// File tree folder constructor.
        /// </summary>
        /// <param name="itemLevel"></param>
        /// <param name="indentationPadding"></param>
        /// <param name="sourceFolder"></param>
        public FolderTreeItem(
            int itemLevel,
            Thickness indentationPadding,
            IBNFolder sourceFolder
        )
        {
            _sourceFolder = sourceFolder;
            itemName = sourceFolder.DirectoryInfo.Name;
            imageIcon = "folder_icon.png";

            this.itemLevel = itemLevel;
            this.indentationPadding = indentationPadding;
        }

        private readonly IBNFolder _sourceFolder;

        private string itemName;
        private int itemLevel;
        private Thickness indentationPadding;
        private ImageSource imageIcon;

        public bool CanHaveChildren { get; } = false;

        public bool HasChildren { get; set; } = false;

        public DirectoryInfo DirectoryInfo
        {
            get =>  _sourceFolder.DirectoryInfo;
            set => _sourceFolder.DirectoryInfo = value;
        }

        public ObservableCollection<ITreeViewItem> Children { get => _sourceFolder.Children; }

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisedOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
