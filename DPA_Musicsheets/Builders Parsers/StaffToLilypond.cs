using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders_Parsers
{
    class StaffToLilypond
    {


        public String load(Staff staff)
        {
            string lilyContent = "";

            lilyContent += "\\relative c' {";
            lilyContent += "\n\\clef " + staff.sound;
            lilyContent += "\n\\time " + staff.firstMeasure + "/" + staff.secondMeasure;
            lilyContent += "\n\\tempo 4=" + staff.tempo;
            lilyContent += "\n";
            int lastOcave = ((MusicNote)staff.bars[0].notes[0]).Octave - 1;
            foreach (Bar b in staff.bars)
            {
                foreach (BaseNote n in b.notes)
                {
                    MusicNote note = (MusicNote)n;
                    string octaveString = "";
                    while (lastOcave < note.Octave)
                    {
                        octaveString += "'";
                        lastOcave++;
                    }
                    while (lastOcave > note.Octave)
                    {
                        octaveString += ",";
                        lastOcave--;
                    }
                    string pitchModifierString = "";
                    if (note.PitchModifier == PitchModifier.Flat) pitchModifierString = "es";
                    if (note.PitchModifier == PitchModifier.Sharp) pitchModifierString = "is";

                    int duration = (int)(1.0f / note.BaseDuration);

                    lilyContent +=
                        note.Pitch.ToString() +
                        pitchModifierString +
                        octaveString +
                        duration +
                        ((note.Dotted) ? "." : "") +
                        " ";
                }
                lilyContent += "|\n";
            }

            lilyContent += "32 }";

            return lilyContent;
        }


    }
}
