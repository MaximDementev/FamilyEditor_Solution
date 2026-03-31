using Autodesk.Revit.DB;
using System.IO;

public class OpenDocumentItem
{
    public Document Document { get; set; }
    public string Name => Path.GetFileName(Document.PathName);
    public bool IsSelected { get; set; }
}