using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders_Parsers
{
    class StaffToLilypond : IVisitor
    {
        private AbsoluteRelative relative;
        private string contentBuffer;

        public String load(Staff staff)
        {
            string lilyContent = "";

            relative = staff.relative;
            relative.reset();

            lilyContent += "\\relative c' {";
            lilyContent += "\n\\clef " + staff.sound;
            lilyContent += "\n\\time " + staff.firstMeasure + "/" + staff.secondMeasure;
            lilyContent += "\n\\tempo 4=" + staff.tempo;
            lilyContent += "\n";

            //int lastOcave = relative.getLastOctave();

            foreach (Bar b in staff.getBars())
            {

                for(int x = 0; x < b.notes.Count; x++)
                {
                    b.notes[x].accept(this);

                    lilyContent += contentBuffer;
                }
                
                lilyContent += "|\n";
            }

            return lilyContent;
        }



        public void visit(Bar bar)
        {
        }

        public void visit(Repeat repeat)
        {
        }

        public void visit(MusicNote note)
        {
            string octaveString = "";

            int newOctave = relative.getOctave(note.Pitch);

            while (newOctave < note.Octave)
            {
                octaveString += "'";
                relative.increaseOctave();
                newOctave++;
            }
            while (newOctave > note.Octave)
            {
                octaveString += ",";
                relative.decreaseOctave();
                newOctave--;
            }
            string pitchModifierString = "";
            if (note.PitchModifier == PitchModifier.Flat) pitchModifierString = "es";
            if (note.PitchModifier == PitchModifier.Sharp) pitchModifierString = "is";

            int duration = (int)(1.0f / note.BaseDuration);

            contentBuffer =
                        note.Pitch.ToString() +
                        pitchModifierString +
                        octaveString +
                        duration +
                        ((note.Dotted) ? "." : "") +
                        " ";


        }

        public void visit(RestNote note)
        {
            int duration = (int)(1.0f / note.Duration);

            contentBuffer =
                        "r" +
                        duration +
                        " ";
        }

        public void visit(BaseNote baseNote)
        {
        }
    }
}
