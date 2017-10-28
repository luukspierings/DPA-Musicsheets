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

                    float d = 1.0f / duration;
                    float wDot = d / 1.5f;
                    return 1.0f / wDot;
                }

                return duration;
            }
            set { duration = value; }
        }

        public float BaseDuration
        {
            get { return duration; }
        }



    }
}
