using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }
    }
}
