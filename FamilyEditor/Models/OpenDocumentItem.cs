using Autodesk.Revit.DB;
using System.ComponentModel;
using System.IO;

public class OpenDocumentItem : INotifyPropertyChanged
{
    public string Name => Path.GetFileName(Document.PathName);
    public Document Document { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        { 
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}