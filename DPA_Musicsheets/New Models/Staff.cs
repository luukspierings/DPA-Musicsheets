using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Staff
    {

        public List<Bar> bars = new List<Bar>();
        public Bar currentBar = new Bar();

        public int firstMeasure { get; set; }    // bovenste maat 
        public int secondMeasure { get; set; }   // onderste maat
        public String sound { get; set; }        // treble of bass
        public int tempo { get; set; }           // tempo


        public Staff(int firstMeasure, int secondMeasure, String sound)
        {
            this.firstMeasure = firstMeasure;
            this.secondMeasure = secondMeasure;
            this.sound = sound;
        }

        public void addNote(BaseNote note)
        {
            currentBar.addNote(note);

            if (currentBar.getDuration() >= getMaxBarDuration())
            {
                newBar();
            }
        }

        public void newBar()
        {
            if(currentBar.notes.Count > 0)
            {
                bars.Add(currentBar);
                currentBar = new Bar();
            }
        }

        public float getMaxBarDuration()
        {
            return firstMeasure * (1.0f/secondMeasure);
            // 2 * (1/2) = 1;
            // 2 * (1/4) = 0.5;
            // 3 * (1/4) = 0.75;
            // 4 * (1/4) = 1;
            // 3 * (1/8) = 0.375;
            // 6 * (1/8) = 0.75;
        }

        

    }
}
