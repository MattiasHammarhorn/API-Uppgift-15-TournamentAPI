using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class GameDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int TournamentId { get; set; }
    }
}
