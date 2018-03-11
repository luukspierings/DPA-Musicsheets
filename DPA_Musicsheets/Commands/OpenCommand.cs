using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class OpenCommand : Command
    {
        FileHandler fileHandler;
        MainController controller;

        public OpenCommand(string pattern, MainController controller, FileHandler fileHandler)
        {
            _pattern = pattern;
            this.fileHandler = fileHandler;
            this.controller = controller;
        }

        public override void execute()
        {
            string fileName = fileHandler.selectFile();
            controller.FileName = fileName;

            Staff staff = fileHandler.OpenFile(fileName);

            controller.ViewCOR.handle(staff, ContentType.MIDI);
            controller.ViewCOR.handle(staff, ContentType.WPF);
            controller.ViewCOR.handle(staff, ContentType.LILYPOND);

        }
    }
}
