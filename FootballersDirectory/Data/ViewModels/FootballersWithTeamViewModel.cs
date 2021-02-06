using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersDirectory.Data.ViewModels
{
    public class FootballersWithTeamViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public string TeamName { get; set; }

        public string Country { get; set; }
    }
}
