using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarianaoWarsConsole.Models
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        
        public int ComputerId{ get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int GroupId{ get; set; }

        public int RowId { get; set; }
    }
}
