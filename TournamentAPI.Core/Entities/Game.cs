using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Title cannot contain more than 25 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int TournamentId { get; set; }

        // Navigation property
        [ForeignKey("TournamentId")]
        public Tournament? Tournament { get; set; }
    }
}
