using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.ViewModels
{
    public class MentorDetailsViewModel
    {
        public Mentor Mentor { get; set; }
        public List<Classroom> Classrooms { get; set; }
        public List<Group> Groups { get; set; }
        public List<Student> Students { get; set; }
    }
}
