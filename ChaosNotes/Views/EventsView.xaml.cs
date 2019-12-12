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
    public partial class EventsView : UserControl
    {
        ObservableCollection<EventModel> events;

        public EventsView()
        {
            InitializeComponent();

            events = new ObservableCollection<EventModel>(NotesDB.db_queryEvents());
            this.DataContext = events;
        }

        private void addEvent(object sender, RoutedEventArgs e)
        {
            Random rn = new Random();
            string red = rn.Next(0, 256).ToString("X");
            string green = rn.Next(0, 256).ToString("X");
            string blue = rn.Next(0, 256).ToString("X");
            string color = $"#{red}{green}{blue}";

            EventModel eve = new EventModel { Content = "", Color = color, Date = "DD/MM/YYYY" };
            NotesDB.db_insertEvent(eve);
            events.Add(NotesDB.db_queryEvents().Last());
        }

        private void editEventContent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int Id = int.Parse(((TextBox)e.Source).Tag.ToString());
                string newContent = ((TextBox)e.Source).Text;

                events.Where(o => o.Id == Id).ToList().ForEach(o => o.Content = newContent);
                NotesDB.db_updateEventContent(new EventModel { Id = Id, Content = newContent });
            }
        }

        private void editEventDate(object sender, SelectionChangedEventArgs e)
        {
            int Id = int.Parse(((DatePicker)e.Source).Tag.ToString());
            string newDate = ((DatePicker)e.Source).SelectedDate.Value.Date.ToShortDateString();

            List<string> date = newDate.Split('/').ToList();
            newDate = $"{date[1].PadLeft(2, '0')}/{date[0].PadLeft(2, '0')}/{date[2]}";

            int idx = events.IndexOf(events.Where(o => o.Id == Id).FirstOrDefault());
            EventModel newEvent = new EventModel
            {
                Id = events[idx].Id,
                Content = events[idx].Content,
                Color = events[idx].Color,
                Date = newDate
            };
            events[idx] = newEvent;

            NotesDB.db_updateEventDate(new EventModel { Id = Id, Date = newDate });
        }

        private void deleteEvent(object sender, RoutedEventArgs e)
        {
            int Id = int.Parse(((Button)e.Source).Tag.ToString());
            events.Remove(events.FirstOrDefault(o => o.Id == Id));
            NotesDB.db_deleteEvent(Id);
        }
    }
}
