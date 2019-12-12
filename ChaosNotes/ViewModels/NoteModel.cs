using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosNotes.ViewModels
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Color { get; set; }
        public int GroupID { get; set; }
    }
}
