using DPA_Musicsheets.Models;
using DPA_Musicsheets.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_Models
{
    public class Repeat : NoteCollection
    {

        public List<Bar> bars = new List<Bar>();
        public Bar currentBar = new Bar();
        public int repeating = 1;
        public List<List<Bar>> alternatives = new List<List<Bar>>();
        public List<Bar> currentAlternative = new List<Bar>();

        private bool addingAlternative = false;

        public void setAmount(int amount)
        {
            repeating = amount;
        }

        public void accept(IVisitor v)
        {
            v.visit(this);
        }


        public void addNote(BaseNote note, float duration)
        {
            currentBar.addNote(note, duration);

            if (currentBar.getDuration() >= duration)
            {
                newBar();
            }
        }
        
        public void newAlternative()
        {
            addingAlternative = true;
            if(currentAlternative.Count > 0) alternatives.Add(currentAlternative);
            currentAlternative = new List<Bar>();
        }

        public bool newBar()
        {
            if (currentBar.getBars().Last().notes.Count > 0)
            {
                if(addingAlternative) currentAlternative.Add(currentBar);
                else bars.Add(currentBar);
                currentBar = new Bar();
            }
            return true;
        }


        public List<Bar> getBars()
        {
            List<Bar> returnBars = new List<Bar>();

            for (int x = 0; x < repeating; x++)
            {
                foreach (Bar b in bars)
                {
                    returnBars.Add(b);
                }

                if (alternatives.Count > x)
                {
                    foreach (Bar b in alternatives[x])
                    {
                        returnBars.Add(b);
                    }
                }
            }

            return returnBars;
        }

        public float getDuration()
        {
            return 0;
        }

       
    }
}
