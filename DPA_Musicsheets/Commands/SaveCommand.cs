using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class SaveCommand : Command
    {
        string _extension;

        public SaveCommand(string pattern, string extension = "")
        {
            _pattern = pattern;
            _extension = extension;
        }

        public override void execute()
        {
            if(_extension != "")
            {

            }
        }
    }
}
