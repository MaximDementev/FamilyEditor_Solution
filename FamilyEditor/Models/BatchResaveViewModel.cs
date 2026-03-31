using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class BatchResaveViewModel
{
    public ObservableCollection<OpenDocumentItem> Documents { get; }

    public BatchResaveViewModel(IEnumerable<Document> docs)
    {
        Documents = new ObservableCollection<OpenDocumentItem>(
            docs.Select(d => new OpenDocumentItem
            {
                Document = d,
                IsSelected = true
            }));
    }

    public void SelectAll(bool value)
    {
        foreach (var doc in Documents)
            doc.IsSelected = value;
    }

    public List<OpenDocumentItem> GetSelected()
    {
        return Documents.Where(d => d.IsSelected).ToList();
    }
}