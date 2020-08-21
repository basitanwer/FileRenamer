using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderName;
        List<FileName> map = new List<FileName>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = folderName;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderName.Content = Path.GetFileName(dialog.FileName);
                folderName = dialog.FileName;
                FillData();
            }
        }



        private void FillData()
        {
            if (folderName == null || folderName.Length == 0)
                return;

            map.Clear();
            map = new List<FileName>();
            var sorted = Directory.GetFiles(folderName, Search.Text.Length == 0 ? "*" : Search.Text).OrderBy(f => f, new NaturalSorter());
            int count = 0;
            int.TryParse(NumberingFrom.Text,out count);
            foreach (var item in sorted)
            {
                string pre = count <= 9 ? "0" : "";
                map.Add(new FileName() 
                { 
                    OldName = item, 
                    NewName = pre + count++ + BaseName.Text + Path.GetExtension(item) 
                }); ;
            }
            Data.ItemsSource = map;

        }

        public class FileName
        {
            private string oldName;
            private string newName;

            public string OldName { get => Path.GetFileName(oldName); set => oldName = value; }
            public string NewName { get => newName; set => newName = value; }
            public string Fullpath { get => oldName;}
        }

        private void Search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FillData();
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in map)
            {
                System.IO.File.Move(item.Fullpath, Path.GetDirectoryName(item.Fullpath) + "\\" + item.NewName);
            }
            FillData();
        }
    }
}
