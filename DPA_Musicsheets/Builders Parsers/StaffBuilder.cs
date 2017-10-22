using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_models_and_patterns
{
    class StaffBuilder
    {

        public Staff staff;

        private NoteFactory noteFactory = new NoteFactory();



        public StaffBuilder(int firstMeasure = 4, int secondMeasure = 4, String sound = "treble")
        {
            staff = new Staff(firstMeasure, secondMeasure, sound);

        }

        public void addNote(int duration, string pitch = "", int deltaOctave = 0, bool isDotted = false)
        {
            BaseNote note;

            while(deltaOctave!= 0)
            {
                if (deltaOctave > 0)
                {
                    noteFactory.increaseOctave();
                    deltaOctave--;
                }
                if (deltaOctave < 0)
                {
                    noteFactory.decreaseOctave();
                    deltaOctave++;
                }
            }

            if(pitch == "")
            {
                note = noteFactory.getRest(duration);
            }
            else
            {
                note = noteFactory.getNote(duration, pitch, isDotted);
            }
            staff.addNote(note);
        }

        public void addBarLine()
        {
            staff.newBar();
        }

        public Staff getStaffObject()
        {
            return staff;
        }

        public Sequence getStaffSequence()
        {
            return new Sequence();
        }
   
        public String getStaffLily()
        {

            foreach(Bar b in staff.bars)
            {
                foreach(BaseNote n in b.notes)
                {

                    MusicNote note = (MusicNote)n;

                    Debug.Write(note.Pitch.ToString() + 1.0f/note.Duration + " ");
                }
            }


            return "";
        }




    }
}
