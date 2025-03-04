using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BestNote_3951.Models;

namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// Handles the logic for the file structure view.
    /// 
    /// Observable object just simplifies a buttload of boilerplater code so we can use the observer pattern out of the box easily.
    /// </summary>
    public partial class FileStructureViewModel : ObservableObject
    {
        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>
        public ObservableCollection<BestFile> Files { get; } = new ObservableCollection<BestFile>
        {
            new BestFile { Name = "comp3951notes.md", Path = "/docs/document1.md", FileType = "md" },
            new BestFile { Name = "comp3721notes.md", Path = "/docs/document2.md", FileType = "md" },
            new BestFile { Name = "textbook.pdf", Path = "/docs/presentation.pdf", FileType = "pdf" },
            new BestFile { Name = "quakelogo.png", Path = "/images/image1.png", FileType = "png" }
        };

        /// <summary>
        /// Handles the open file logic (eventually)
        /// 
        /// The relay command attribute automatically generates a command property that can be invoked.
        /// It basically just implements a Command  design pattern with a decorator instead of having to write that crap ourselves.
        /// </summary>
        /// <param name="file"></param>
        [RelayCommand]
        public void OpenFile(BestFile file)
        {
            // TODO: open file logic
        }
    }
}
