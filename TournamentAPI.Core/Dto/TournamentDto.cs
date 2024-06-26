﻿using System;
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
        [MaxLength(25, ErrorMessage = "Title cannot contain more than 25 characters.")]
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
