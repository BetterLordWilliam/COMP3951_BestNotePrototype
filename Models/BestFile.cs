using Syncfusion.Maui.GridCommon.ScrollAxis;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
        private DirectoryInfo parentItemDirectory;
        
        public ObservableCollection<BestFile> SubFiles { get; set; } = new ObservableCollection<BestFile>();
        public int Level { get; set; }
        public Thickness IndentationPadding => new Thickness(Level, 0, 0, 0);
        public bool Expandable { get; private set; } = true;

        /// <summary>
        /// Best file model constructor.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="imageIcon"></param>
        /// <param name="itemDirectory"></param>
        /// <param name="parentItemDirectory"></param>
        private BestFile(
            string              itemName,
            ImageSource         imageIcon,
            DirectoryInfo       itemDirectory,
            DirectoryInfo       parentItemDirectory
        ) {
            this.itemName = itemName;
            this.imageIcon = imageIcon;
            this.itemDirectory = itemDirectory;
            this.parentItemDirectory = parentItemDirectory;
        }

        public static BestFile BestFileMarkdown(
            string itemName,
            ImageSource imageIcon,
            DirectoryInfo itemDirectory,
            DirectoryInfo parentItemDirectory
        )
        {
            BestFile bf = new BestFile(itemName, imageIcon, itemDirectory, parentItemDirectory);
            bf.Expandable = false;

            return bf;
        }

        public static BestFile BestFileFolder(
            string itemName,
            ImageSource imageIcon,
            DirectoryInfo itemDirectory,
            DirectoryInfo parentItemDirectory
        )
        {
            BestFile bf = new BestFile(itemName, imageIcon, itemDirectory, parentItemDirectory);
            bf.Expandable = true;

            return bf;
        }

        public DirectoryInfo ItemDirectory
        {
            get { return itemDirectory; }
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
