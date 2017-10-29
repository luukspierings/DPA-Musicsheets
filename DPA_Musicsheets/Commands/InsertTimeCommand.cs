using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class InsertTimeCommand : Command
    {
        string _time;

        public InsertTimeCommand(string pattern, string time = "4/4")
        {
            _pattern = pattern;
            _time = time;
            
        }

        public override void execute()
        {

            

        }
    }
}
