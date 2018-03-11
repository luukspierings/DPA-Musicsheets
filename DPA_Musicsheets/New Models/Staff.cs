using DPA_Musicsheets.New_Models;
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
        public AbsoluteRelative relative;

        public List<NoteCollection> bars = new List<NoteCollection>();
        public NoteCollection currentBar;

        public int firstMeasure { get; set; }    // bovenste maat 
        public int secondMeasure { get; set; }   // onderste maat
        public String sound { get; set; }        // treble of bass
        public int tempo { get; set; }           // tempo


        public Staff(int firstMeasure, int secondMeasure, String sound, int tempo)
        {
            this.firstMeasure = firstMeasure;
            this.secondMeasure = secondMeasure;
            this.sound = sound;
            this.tempo = tempo;
            relative = new AbsoluteRelative();
            currentBar = new Bar();
        }

        public void addNote(BaseNote note)
        {
            if (currentBar == null) currentBar = new Bar();

            currentBar.addNote(note, getMaxBarDuration());

            if (currentBar.getDuration() >= getMaxBarDuration())
            {
                newBar();
            }
        }

        public void newBar()
        {
            if (!currentBar.newBar() && currentBar.getBars().Last().notes.Count > 0)
            {
                bars.Add(currentBar);
                currentBar = new Bar();
            }
        }

        public List<Bar> getBars()
        {
            List<Bar> returnBars = new List<Bar>();

            foreach (NoteCollection nc in bars)
            {
                foreach (Bar b in nc.getBars())
                {
                    returnBars.Add(b);
                }
            }
            return returnBars;
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


        public void startRepeat()
        {
            newBar();
            currentBar = new Repeat();
        }
        public void setRepeatAmount(int amount)
        {
            ((Repeat)currentBar)?.setAmount(amount);
        }
        public void addAlternative()
        {
            ((Repeat)currentBar)?.newAlternative();
        }
        public void endRepeat()
        {
            currentBar.newBar();
            bars.Add(currentBar);
            currentBar = new Bar();
        }


    }
}
