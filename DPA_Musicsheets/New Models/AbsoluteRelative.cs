using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_Models
{
    public class AbsoluteRelative
    {
        protected int lastOctave;
        protected int startOctave;

        public AbsoluteRelative(int deltaOctave = 0)
        {
            lastOctave = 3 + deltaOctave;
            startOctave = lastOctave;
        }

        public virtual int getOctave(char pitch) {
            return lastOctave;
        }
        public int getLastOctave()
        {
            return lastOctave;
        }

        public void increaseOctave()
        {
            lastOctave++;
        }
        public void decreaseOctave()
        {
            lastOctave--;
        }
        public virtual void reset()
        {
            lastOctave = startOctave;
        }


    }
}
