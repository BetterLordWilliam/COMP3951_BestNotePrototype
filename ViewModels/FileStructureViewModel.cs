using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BestNote_3951.ViewModels
{
    public partial class FileStructureViewModel : ObservableObject
    {
        public ObservableCollection<string> Files { get; } =
        [
            "File1.md",
            "File2.md",
            "File3.txt"
        ];
    }
}
