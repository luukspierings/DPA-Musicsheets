using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Staff
    {

        public List<Bar> bars = new List<Bar>();
        public Bar currentBar = new Bar();

        public int firstMeasure;    // bovenste maat 
        public int secondMeasure;   // onderste maat
        public String sound;        // treble of bass
        public int tempo;           // tempo


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
                bars.Add(currentBar);
                currentBar = new Bar();
            }
        }

        public float getMaxBarDuration()
        {
            return firstMeasure * (1/secondMeasure);
            // 2 * (1/2) = 1;
            // 2 * (1/4) = 0.5;
            // 3 * (1/4) = 0.75;
            // 4 * (1/4) = 1;
            // 3 * (1/8) = 0.375;
            // 6 * (1/8) = 0.75;
        }


    }
}
