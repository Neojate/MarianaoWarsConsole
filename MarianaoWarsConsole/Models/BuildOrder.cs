using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWars.Models
{
    public class BuildOrder
    {

        public int Id { get; set; }

        public int ComputerId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int BuildId { get; set; }

    }
}
