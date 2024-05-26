using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate {
            get
            {
                return StartDate.AddMonths(3);
            }
        }
    }
}
