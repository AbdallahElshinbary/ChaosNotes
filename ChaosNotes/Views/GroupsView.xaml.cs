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
    public partial class GroupsView : UserControl
    {
        ObservableCollection<GroupModel> groups;
        public GroupsView()
        {
            InitializeComponent();

            groups = new ObservableCollection<GroupModel>(NotesDB.db_queryGroups());
            this.DataContext = groups;
        }
        private void showGroupNotes(object sender, MouseButtonEventArgs e)
        {
            NotesDB.currentGroupId = int.Parse(((StackPanel)e.Source).Tag.ToString());
            
            MainWindow w = Window.GetWindow(this) as MainWindow;
            w.DataContext = new NoteModel();
        }

        private void addGroup(object sender, RoutedEventArgs e)
        {
            GroupModel g = new GroupModel { Title = "New Group" };
            NotesDB.db_insertGroup(g);
            groups.Add(NotesDB.db_queryGroups().Last());
        }

        private void editGroupTitle(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                int Id = int.Parse(((TextBox)e.Source).Tag.ToString());
                string newTitle = ((TextBox)e.Source).Text;
                
                groups.Where(o => o.Id == Id).ToList().ForEach(o => o.Title = newTitle);
                NotesDB.db_updateGroup(new GroupModel { Id = Id, Title = newTitle });
            }
        }

        private void deleteGroup(object sender, RoutedEventArgs e)
        {
            int Id = int.Parse(((Button)e.Source).Tag.ToString());
            groups.Remove(groups.FirstOrDefault(o => o.Id == Id));
            NotesDB.db_deleteGroup(Id);
        }
    }
}
