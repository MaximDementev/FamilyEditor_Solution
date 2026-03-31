using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;

namespace MagicEntry.Plugins.FamilyEditor.Views
{
    public partial class BatchResaveWindow : Window
    {
        public BatchResaveViewModel ViewModel { get; }

        public List<OpenDocumentItem> Result { get; private set; }

        public BatchResaveWindow(List<Document> docs)
        {
            InitializeComponent();
            ViewModel = new BatchResaveViewModel(docs);
            DataContext = ViewModel;
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
            => ViewModel.SelectAll(true);

        private void DeselectAll_Click(object sender, RoutedEventArgs e)
            => ViewModel.SelectAll(false);

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Result = ViewModel.GetSelected();
            DialogResult = true;
            Close();
        }
    }
}