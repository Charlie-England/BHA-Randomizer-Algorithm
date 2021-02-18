using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHA_Randomizer_NET
{
    public class ProviderModel
    {
        public string Name { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }


        public double Fte { get; set; }
        public string clinic { get; set; }

        public int assignedValue { get; set; }

        public ProviderModel(string _name, bool _monday, bool _tuesday, bool _wednesday, bool _thursday, bool _friday, double _fte, string _clinic)
        {
            Name = _name;
            Monday = _monday;
            Tuesday = _tuesday;
            Wednesday = _wednesday;
            Thursday = _thursday;
            Friday = _friday;
            Fte = _fte;
            assignedValue = 0;
            clinic = _clinic;
        }

        public void resetAssignedValue()
        {
            assignedValue = 0;
        }

    }
}