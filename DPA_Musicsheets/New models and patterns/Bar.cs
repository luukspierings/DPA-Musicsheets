using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Bar
    {

        public List<BaseNote> notes = new List<BaseNote>();

        public void addNote(BaseNote note)
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

    }
}
