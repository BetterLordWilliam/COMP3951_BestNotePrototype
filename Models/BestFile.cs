namespace BestNote_3951.Models
{
    /// <summary>
    /// Model for a BestFile.
    /// 
    /// Has the data for a file in our file structure, name, path, file type and whatever other dope stuff we add.
    /// </summary>
    public class BestFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; } 
    }
}
