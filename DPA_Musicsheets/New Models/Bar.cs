using DPA_Musicsheets.New_Models;
using DPA_Musicsheets.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Bar : NoteCollection
    {

        public List<BaseNote> notes;


        public Bar()
        {
            notes = new List<BaseNote>();
        }

        public Bar(List<BaseNote> n)
        {
            notes = n;
        }

        public void accept(IVisitor v)
        {
            v.visit(this);
        }

        public void addNote(BaseNote note, float duration)
        {
            notes.Add(note);
        }

        public float getDuration()
        {
            float d = 0.0f;
            foreach(BaseNote n in notes){
                d += n.Duration;
            }
            return d;
        }

        public List<Bar> getBars()
        {
            return new List<Bar>() { new Bar(notes) };
        }

        public bool newBar()
        {
            return false;
        }

        
    }
}
