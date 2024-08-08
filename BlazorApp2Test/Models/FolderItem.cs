namespace BlazorApp2Test.Models;

public class FolderItem
{
    public string Name { get; set; }
    public List<FolderItem> SubFolders { get; set; } = new List<FolderItem>();
    public List<FileItem> Files { get; set; } = new List<FileItem>();
}