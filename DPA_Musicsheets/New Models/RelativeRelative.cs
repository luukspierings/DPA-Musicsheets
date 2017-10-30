using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_Models
{
    class RelativeRelative : AbsoluteRelative
    {

        protected List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
        protected char startingPitch;
        protected char lastPitch;

        public RelativeRelative(char pitch = 'c', int deltaOctave = 1): base(deltaOctave)
        {
            startingPitch = pitch;
            lastPitch = pitch;
        }

        public override int getOctave(char pitch)
        {
            int pitchO = notesorder.IndexOf(pitch);
            int lPitchO = notesorder.IndexOf(lastPitch);

            int deltaPitch = pitchO - lPitchO;


            if (deltaPitch > 3) lastOctave--; 
            else if (deltaPitch < -3) lastOctave++;

            lastPitch = pitch;

            return lastOctave;
        }
        public override void reset()
        {
            base.reset();
            lastPitch = startingPitch;
        }
    }
}
