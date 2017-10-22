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


        public BaseNote getNote(int duration, string pitch, bool dotted = false)
        {
            MusicNote note = new MusicNote();

            char[] pitchArray = pitch.ToCharArray();

            if (pitchArray.Length > 0) note.Pitch = pitchArray[0];

            if (pitchArray.Length > 3 && pitchArray[1] == 'i') note.PitchModifier = PitchModifier.Sharp;
            else if (pitchArray.Length > 3 && pitchArray[1] == 'e') note.PitchModifier = PitchModifier.Flat;
            else note.PitchModifier = PitchModifier.None;

            note.Duration = 1.0f / duration;
            note.Dotted = dotted;
            note.Octave = currentOctave;

            return note;
        }

        public BaseNote getRest(int duration)
        {
            RestNote note = new RestNote();
            note.Duration = 1.0f/duration;

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
