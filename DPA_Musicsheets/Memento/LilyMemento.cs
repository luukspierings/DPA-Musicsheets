using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class LilyMemento
    {
        private String state;

        public LilyMemento(String state)
        {
            this.state = state;
        }

        public String getState()
        {
            return state;
        }
    }
}
