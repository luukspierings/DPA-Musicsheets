using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_Models;
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
        public void setRelative(char pitch = 'c', int deltaOctave = 0)
        {
            staff.relative = new RelativeRelative(pitch, deltaOctave);
        }

        public void newStaff(int firstMeasure = 4, int secondMeasure = 4, String sound = "treble", int tempo = 120)
        {
            staff = new Staff(firstMeasure, secondMeasure, sound, tempo);
            noteFactory = new NoteFactory();

        }

        public StaffBuilder()
        {
            newStaff();
        }

        public void addNote(int duration, string pitch = "", int deltaOctave = 0, bool isDotted = false)
        {
            BaseNote note;

            while(deltaOctave!= 0)
            {
                if (deltaOctave > 0)
                {
                    staff.relative.increaseOctave();
                    deltaOctave--;
                }
                if (deltaOctave < 0)
                {
                    staff.relative.decreaseOctave();
                    deltaOctave++;
                }
            }

            if(pitch == "r")
            {
                note = noteFactory.getRest(duration);
            }
            else
            {
                char cPitch = pitch.ToCharArray()[0];
                note = noteFactory.getNote(duration, pitch, staff.relative.getOctave(cPitch), isDotted);
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

        public void startRepeat()               => staff.startRepeat();
        public void setRepeatAmount(int amount) => staff.setRepeatAmount(amount);
        public void endRepeat()                 => staff.endRepeat();
        public void addAlternative()            => staff.addAlternative();


    }
}
