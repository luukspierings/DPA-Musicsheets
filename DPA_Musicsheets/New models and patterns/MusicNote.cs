using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class MusicNote : BaseNote
    {

        public char Pitch { get; set; }
        public int Octave { get; set; }

        public bool Dotted { get; set; }
        public PitchModifier PitchModifier { get; set; }

        public override float Duration
        {
            get
            {
                if (Dotted)
                {
                    return duration * 1.5f;
                }

                return duration;
            }
            set { duration = value; }
        }



    }
}
