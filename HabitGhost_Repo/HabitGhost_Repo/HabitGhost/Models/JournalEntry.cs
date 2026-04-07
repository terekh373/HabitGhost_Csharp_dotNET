using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitGhost.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Content { get; set; }
        public List<int> LinkedHabitIds { get; set; } = new List<int>();
        public List<int> LinkedGoalIds { get; set; } = new List<int>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
