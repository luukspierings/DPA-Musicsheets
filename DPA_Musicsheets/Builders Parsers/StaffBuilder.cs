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

        public void setFirstMeasure(int fMeasure)
        {
            staff.firstMeasure = fMeasure;
        }
        public void setSecondMeasure(int sMeasure)
        {
            staff.secondMeasure = sMeasure;
        }
        public void setSound(String sound)
        {
            staff.sound = sound;
        }
        public void setTempo(int tempo)
        {
            staff.tempo = tempo;
        }


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
   
        




    }
}
