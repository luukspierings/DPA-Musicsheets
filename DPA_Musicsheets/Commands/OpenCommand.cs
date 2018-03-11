using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.Commands
{
    class OpenCommand : Command
    {
        FileHandler fileHandler;
        MainController controller;

        public OpenCommand(MainController controller, FileHandler fileHandler)
        {
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
