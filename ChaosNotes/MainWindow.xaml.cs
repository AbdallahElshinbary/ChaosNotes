using System;
using System.Collections.Generic;
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

namespace ChaosNotes
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new GroupModel();
        }

        private void showGroups(object sender, RoutedEventArgs e)
        {
            this.DataContext = new GroupModel();
        }

        private void showEvents(object sender, RoutedEventArgs e)
        {
            this.DataContext = new EventModel();
        }

        private void clearNotes(object sender, RoutedEventArgs e)
        {
            NotesDB.db_deleteAllGroup();
            this.DataContext = new EventModel();
        }

        private void clearEvents(object sender, RoutedEventArgs e)
        {
            NotesDB.db_deleteAllEvent();
            this.DataContext = new GroupModel();
        }

        private void aboutUs(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AboutUsModel();
        }
    }
}
