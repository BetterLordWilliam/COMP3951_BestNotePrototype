using Syncfusion.Maui.GridCommon.ScrollAxis;
using System.Collections.ObjectModel;
using System.ComponentModel;

///
/// Will Otterbein
/// March 12 2025
///
namespace BestNote_3951.Models
{
    /// <summary>
    /// Model for a BestFile.
    /// 
    /// Has the data for a file in our file structure, name, path, file type and whatever other dope stuff we add.
    /// </summary>
    public class BestFile : INotifyPropertyChanged
    {
        private string itemName;
        private ImageSource imageIcon;
        private DirectoryInfo itemDirectory;
        private FileInfo fileItemDirectory;
        private DirectoryInfo parentItemDirectory;
        private ObservableCollection<BestFile> subFiles;

        public int Level { get; set; }
        public Thickness IndentationPadding => new Thickness(Level, 0, 0, 0);
        public bool Parent { get; private set; } = true;

        /// <summary>
        /// Best file model constructor.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="imageIcon"></param>
        /// <param name="parentItemDirectory"></param>
        private BestFile(
            string              itemName,
            ImageSource         imageIcon,
            DirectoryInfo       parentItemDirectory
        ) {
            this.itemName = itemName;
            this.imageIcon = imageIcon;
            this.parentItemDirectory = parentItemDirectory;
        }

        /// <summary>
        /// Create a markdown BestFile.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="imageIcon"></param>
        /// <param name="fileItemDirectory"></param>
        /// <param name="parentItemDirectory"></param>
        /// <returns></returns>
        public static BestFile BestFileMarkdown(
            string itemName,
            ImageSource imageIcon,
            FileInfo fileItemDirectory,
            DirectoryInfo parentItemDirectory
        )
        {
            BestFile bf = new BestFile(itemName, imageIcon, parentItemDirectory);
            bf.fileItemDirectory = fileItemDirectory;
            bf.Parent = false;

            return bf;
        }

        /// <summary>
        /// Create a folder BestFile.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="imageIcon"></param>
        /// <param name="itemDirectory"></param>
        /// <param name="parentItemDirectory"></param>
        /// <returns></returns>
        public static BestFile BestFileFolder(
            string itemName,
            ImageSource imageIcon,
            DirectoryInfo itemDirectory,
            DirectoryInfo parentItemDirectory
        )
        {
            BestFile bf = new BestFile(itemName, imageIcon, parentItemDirectory);
            bf.itemDirectory = itemDirectory;
            bf.Parent = true;
            bf.subFiles = new ObservableCollection<BestFile>();

            return bf;
        }

        public DirectoryInfo ItemDirectory
        {
            get { return itemDirectory; }
        }

        public ObservableCollection<BestFile> SubFiles
        {
            get { return subFiles; }
            set
            {
                subFiles = value;
                RaisedOnPropertyChanged("SubFiles");
            }
        }

        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                RaisedOnPropertyChanged("ItemName");
            }
        }

        public ImageSource ImageIcon
        {
            get { return imageIcon; }
            set
            {
                imageIcon = value;
                RaisedOnPropertyChanged("ImageIcon");
            }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return itemDirectory; }
            set
            {
                itemDirectory = value;
                RaisedOnPropertyChanged("DirectoryInfo");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisedOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
