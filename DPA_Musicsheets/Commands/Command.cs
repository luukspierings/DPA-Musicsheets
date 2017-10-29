using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    abstract class Command
    {
        protected string _pattern;

        public string Pattern
        {
            get
            {
                return _pattern;
            }
        }
        public abstract void execute();

    }
}
