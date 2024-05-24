using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Entities
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }

        // Navigation property
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
