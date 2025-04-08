using Syncfusion.Maui.GridCommon.ScrollAxis;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using BestNote_3951.ViewModels;

///
/// Will Otterbein
/// March 23 2025
///
namespace BestNote_3951.Models.FileSystem
{
    /// <summary>
    /// Best note readable base type.
    /// </summary>
    public interface IBNReadable
    {
        // StreamReader Reader { get; set; }
        string ReadFileContents();
    }

    /// <summary>
    /// Best note writable base type.
    /// </summary>
    public interface IBNWritable
    {
        // StreamWriter Writer { get; set; }
        void WriteToFile(string Content);
    }

    /// <summary>
    /// Best note tree view file base type.
    /// </summary>
    public interface IBNFile : IBNReadable, IBNWritable
    {
        FileInfo FileInfo { get; set; }
        void Rename(string NewFileName);
        void Move(FolderTreeItem NewParent);
        void Delete();
    }

    /// <summary>
    /// Best note tree view folder base type.
    /// </summary>
    public interface IBNFolder
    {
        DirectoryInfo DirectoryInfo { get; set; }
        ObservableCollection<BestFileTreeItemViewModel> Children { get; set; }
        void Rename(string NewFolderName);
        void Move(FolderTreeItem NewParent);
        void Delete();
        void DeleteAll();
        void AddChild(BestFileTreeItemViewModel item);
        void RemoveChild(BestFileTreeItemViewModel item);
        void LoadFileSystemObjects();
    }

    /// <summary>
    /// Tree view item base type.
    /// </summary>
    public interface ITreeViewItem : INotifyPropertyChanged
    {
        string      ItemName { get; set; }
        int         ItemLevel { get; set; }
        bool        HasChildren { get; }
        bool        CanHaveChildren { get; }
        bool        CanRename { get; set; }
        bool        ItemRename { get; set; }
        Thickness   IndentationPadding { get; set; }
        IBNFolder?  Parent { get; set; }
        ImageSource ImageIcon { get; }
        IEnumerable<BestFileTreeItemViewModel> SafeChildren { get; }
        void Rename(string NewItemName);
        void Move(FolderTreeItem NewParent);
        void Delete();
    }

    /// <summary>
    /// Base class for tree items, provides implementations that are common to most tree items.
    /// </summary>
    /// <p>
    /// It is still expected for specific types that further implementation for functionality
    /// such as reading or writing to files is provided via composition and delegation of functionality.
    /// </p>
    public abstract class TreeViewItemBase : ObservableObject, ITreeViewItem
    {
        private string itemName;
        private int itemLevel;
        private Thickness indentationPadding;
        private ImageSource imageIcon;
        private IBNFolder? parent;

        /// <summary>
        /// Integer representing the depth of this item in the tree.
        /// </summary>
        public int ItemLevel
        {
            get => itemLevel;
            set
            {
                itemLevel = value;
                RaisedOnPropertyChanged(nameof(ItemLevel));
            }
        }

        /// <summary>
        /// The indentation of this item in the tree view.
        /// </summary>
        public Thickness IndentationPadding
        {
            get => indentationPadding;
            set
            {
                indentationPadding = value;
                RaisedOnPropertyChanged(nameof(value));
            }
        }

        /// <summary>
        /// The name of the item as it would appear in the tree.
        /// </summary>
        public string ItemName
        {
            get => itemName;
            set
            {
                itemName = value;
                RaisedOnPropertyChanged(nameof(ItemName));
            }
        }

        /// <summary>
        /// Image icon displayed for the tree item.
        /// </summary>
        public ImageSource ImageIcon
        {
            get => imageIcon;
            set
            {
                imageIcon = value;
                RaisedOnPropertyChanged(nameof(ImageIcon));
            }
        }

        /// <summary>
        /// Reference to the parent tree item, may be null (top level).
        /// </summary>
        public IBNFolder? Parent
        {
            get => parent;
            set
            {
                parent = value;
                RaisedOnPropertyChanged(nameof(parent));
            }
        }

        /// <summary>
        /// Flag indicating whether the item has children.
        /// </summary>
        public virtual bool HasChildren => false;

        /// <summary>
        /// Flag indicating whether the item is capable of having children. False by default.
        /// </summary>
        public virtual bool CanHaveChildren => false;

        /// <summary>
        /// Flag indicating whether the item is renameable or not.
        /// </summary>
        public virtual bool CanRename { get; set; } = true;

        /// <summary>
        /// Flag indicating whether the item is being renamed or not.
        /// </summary>
        public virtual bool ItemRename { get; set; } = false;

        /// <summary>
        /// Safe access the contents of a nodes children. Used during data binding.
        /// </summary>
        public virtual IEnumerable<BestFileTreeItemViewModel> SafeChildren => Enumerable.Empty<BestFileTreeItemViewModel>();

        /// <summary>
        /// Rename a file system item.
        /// </summary>
        /// <param name="NewItemName"></param>
        public abstract void Rename(string NewItemName);

        /// <summary>
        /// Move a file system item to it's new parent.
        /// </summary>
        /// <param name="NewParent"></param>
        public abstract void Move(FolderTreeItem NewParent);

        /// <summary>
        /// Deletes file tree item.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invoke the event handler for property the on property changed event.
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisedOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
