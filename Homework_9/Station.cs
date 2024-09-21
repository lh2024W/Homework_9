using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_9
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Train> Trains { get; set; }
    }
}
