using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_models_and_patterns
{
    class NoteFactory
    {


        public int currentOctave = 4;


        public BaseNote getNote(int duration, char pitch, bool dotted = false, PitchModifier modifier = PitchModifier.None)
        {
            MusicNote note = new MusicNote();
            note.Duration = 1 / duration;
            note.Pitch = pitch;
            note.Dotted = dotted;
            note.PitchModifier = modifier;
            note.Octave = currentOctave;

            return note;
        }

        public BaseNote getRest(int duration)
        {
            RestNote note = new RestNote();
            note.Duration = 1/duration;

            return note;
        }

        public void increaseOctave()
        {
            currentOctave++;
        }

        public void decreaseOctave()
        {
            currentOctave--;
        }



    }
}
