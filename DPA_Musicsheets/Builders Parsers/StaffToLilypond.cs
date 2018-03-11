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

        private string lilyContent;

        public String load(Staff staff)
        {
            lilyContent = "";

            relative = staff.relative;
            relative.reset();

            lilyContent += "\\relative c' {";
            lilyContent += "\n\\clef " + staff.sound;
            lilyContent += "\n\\time " + staff.firstMeasure + "/" + staff.secondMeasure;
            lilyContent += "\n\\tempo 4=" + staff.tempo;
            lilyContent += "\n";

            //int lastOcave = relative.getLastOctave();
            
            foreach(NoteCollection nc in staff.bars)
            {
                nc.accept(this);
            }

            lilyContent += "32 }";

            return lilyContent;
        }



        public void visit(Bar bar)
        {
            for (int x = 0; x < bar.notes.Count; x++)
            { 
                bar.notes[x].accept(this);
            }
            lilyContent += "|\n";
        }

        public void visit(Repeat repeat)
        {
            lilyContent += "\\repeat volta " + repeat.repeating + " {\n";
            foreach(Bar b in repeat.bars)
            {
                b.accept(this);
            }
            lilyContent += "}\n";
            if(repeat.alternatives.Count > 0)
            {
                lilyContent += "\\alternative {\n";
                foreach(List<Bar> alt in repeat.alternatives)
                {
                    lilyContent += "{\n";
                    foreach(Bar altb in alt)
                    {
                        altb.accept(this);
                    }
                    lilyContent += "}\n";
                }
                lilyContent += "}\n";
            }

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

            lilyContent +=
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

            lilyContent +=
                        "r" +
                        duration +
                        " ";
        }

        public void visit(BaseNote baseNote)
        {
        }
    }
}
