using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
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

        public void addNote(int duration, char pitch = ' ')
        {
            BaseNote note;
            if(pitch == ' ')
            {
                note = noteFactory.getRest(duration);
            }
            else
            {
                note = noteFactory.getNote(duration, pitch);
            }
            staff.addNote(note);

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
            return "";
        }




    }
}
