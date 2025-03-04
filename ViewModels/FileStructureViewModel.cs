using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BestNote_3951.Models;

namespace BestNote_3951.ViewModels
{
    public partial class FileStructureViewModel : ObservableObject
    {
        public ObservableCollection<BestFile> Files { get; } = new ObservableCollection<BestFile>
        {
            new BestFile { Name = "Document1.md", Path = "/docs/document1.md", FileType = "md" },
            new BestFile { Name = "Document2.md", Path = "/docs/document2.md", FileType = "md" },
            new BestFile { Name = "Presentation.pdf", Path = "/docs/presentation.pdf", FileType = "pdf" },
            new BestFile { Name = "Image1.png", Path = "/images/image1.png", FileType = "png" }
        };

        [RelayCommand]
        public void OpenFile(BestFile file)
        {
            // TODO: Add logic to open the file.
        }
    }
}
