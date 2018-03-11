using DPA_Musicsheets.Managers;
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
        FileHandler _fileHandler;
        MainController _controller;

        public SaveCommand(MainController controller, FileHandler fileHandler, string extension = "")
        {
            _fileHandler = fileHandler;
            _controller = controller;
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
