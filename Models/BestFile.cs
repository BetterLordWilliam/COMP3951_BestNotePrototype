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
        public DirectoryInfo itemDirectory { get; set; }

        private ObservableCollection<BestFile> subFiles { get; set; }

        public BestFile(string itemName, ImageSource imageIcon, DirectoryInfo itemDirectory)
        {
            this.itemName = itemName;
            this.imageIcon = imageIcon;
            this.itemDirectory = itemDirectory;
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
