using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChaosNotes.ViewModels;
using ChaosNotes.Data;

namespace ChaosNotes.Views
{
    public partial class NotesView : UserControl
    {
        ObservableCollection<NoteModel> notes;
        GroupModel parentGroup;
        public NotesView()
        {
            InitializeComponent();

            parentGroup = NotesDB.db_queryGroup(NotesDB.currentGroupId);
            notes = new ObservableCollection<NoteModel>(NotesDB.db_queryNotes(parentGroup.Id));

            this.DataContext = notes;
            group_title.Content = parentGroup.Title;
        }

        private void addNote(object sender, RoutedEventArgs e)
        {
            Random rn = new Random();
            string red = rn.Next(0, 256).ToString("X");
            string green = rn.Next(0, 256).ToString("X");
            string blue = rn.Next(0, 256).ToString("X");
            string color = $"#{red}{green}{blue}";

            NoteModel n = new NoteModel { Content = "", Color = color, GroupID = parentGroup.Id };
            NotesDB.db_insertNote(n);
            notes.Add(NotesDB.db_queryNotes(parentGroup.Id).Last());
        }

        private void editNote(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int Id = int.Parse(((TextBox)e.Source).Tag.ToString());
                string newContent = ((TextBox)e.Source).Text;

                notes.Where(o => o.Id == Id).ToList().ForEach(o => o.Content = newContent);
                NotesDB.db_updateNote(new NoteModel { Id = Id, Content = newContent });
            }
        }

        private void deleteNote(object sender, RoutedEventArgs e)
        {
            int Id = int.Parse(((Button)e.Source).Tag.ToString());
            notes.Remove(notes.FirstOrDefault(o => o.Id == Id));
            NotesDB.db_deleteNote(Id);
        }
    }
}
