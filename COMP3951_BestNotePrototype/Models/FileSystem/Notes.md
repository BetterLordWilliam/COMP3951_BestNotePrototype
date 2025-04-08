# FileSystem Model Design
20250320, Will Otterbein

This document will outline the design of the file system.

## Interfaces
In order that the dependencies of the file system be extendable, there are -- at present -- a set of 4 interfaces which define the functionalities of the system.

```csharp
namespace BestNote_3951.Models.FileSystem

public interface IBNWritable;
public interface IBNReadable;

public interface IBNFile;
public interface IBNFolder;

public interface ITreeViewItem;
```

- `IBNWritable` and `IBNReadable` define the contract that types must fulfil such that their contents can be modifier by BestNote.
- `INBFile` defines the contract(s) that files in the tree view must fulfill
- `IBNFolder` defines the contract(s) that folders/ perhaps more generically parents, must fulfill.
- `ITreeViewItem` is the generic item as it would appear in the treeview
	- Defines that types need to have the following properties

### `IBNWritable` and `IBNReadable`
Define that a type must have a StreamWriter/StreamReader property which other subprocesses of the application
can use to write contents to the actual file object

### `IBNFile`
Defines that types need to have a FileInfo object, which other subprocesses of the application will use.

### `IBNFolder`
Definese that types need to have a DirectoryInfo object, which other subprocesses of the application will use.

### `ITreeViewItem` 
The main type which the front end will use, though for specific tree view item contents, different data templates are defined (more on this later).

| Visibility | Type | Property Name | Description |
| ---------- | ---- | ------------- | ----------- |
| public | string | ItemName | Name that should be displayed for the item |
| public | int | ItemLevel | UI property, indicates the level of the item, modifiable property |
| public | bool | HasChildren | UI property, used to conditionally render certain parts of the UI |
| public | bool | CanHaveChildren | UI property, used to conditionally render certain parts of the UI |
| public | Thickness | IndentationPadding | UI property, used to define the UI indendation for treeview items |
| public | IEnumerable<ITreeViewItem> | SafeChildren | The property used to access the contents of a treeview item in a safe way **1** |
| public | ImageSource | ImageIcon | Icon image to render beside the filename |

**1** -- This is done to access the contents of a node in a safe way. Actual binding to children is only defined for IBNFolder obejcts,
which have the Children property. More on this in the XAML.

## Implementations for the tree view

### `TreeViewItemBase`
There is a base type defined for treeview items, an abstract class called TreeViewItemBase,
which implements ITreeViewItem. This class defines properties that will be common to all
tree view items.
- ItemName
- IndentationPadding
- ImageIcon
- HasChildren
- CanHaveChildren
- SafeChildren
- PropertyChanged (base type event)
- Raised on property changed

### `FileTreeItem`
Extends `TreeViewItemBase` class, implements `IBNFolder` implemented via composition -- delegation pattern

### `FolderTreeItem`
Extends `TreeViewItemBase` class, implements `IBNFile` implemented via composition -- delegation pattern.

### MarkdownFile
Concrete implementation of `INBFile`, also implements `IBNReadable` and `IBNWritable` interfaces, to be used by fileio
operations in the future.

### WindowsFolder
Concrete implementation of `IBNFolder`.


# Random Notes

- It is necessary to refactor the FileStructureViewModel code to extract the item instantiaiton.
- Put this into another class, file system utils or the like.
- Classes to help instantiation BestNote objects from file system objects (contain mappings, etc).

It would possibly be worth looking into the following class, `FileSystemWatcher`

[File system watcher docs](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-9.0)

Could avoid querying the file system and instead updating items in real time

This would *potentially* require some changes perhaps to the underlying data structures, for example may need to consider using a map
as the underlying type for `FileSystem` in the view model and `Children` in `IBNFolder` types -- could map FileSystemInfo objects to `ITreeViewItem` instnaces for example.

> This is an idea that I am liking for several reasons, but I don't want to refactor too much
> Good news is that I would need to change any of the models for this