using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.Commands
{
    class OpenCommand : Command
    {
        MainController controller;

        public OpenCommand(MainController controller)
        {
            this.controller = controller;
        }

        public override void execute()
        {
            string fileName = controller.FileHandler.selectFile();
            controller.FileName = fileName;

            Staff staff = controller.FileHandler.OpenFile(fileName);

            if(staff != null)
            {
                controller.ViewCOR.handle(staff, ContentType.MIDI);
                controller.ViewCOR.handle(staff, ContentType.WPF);
                controller.ViewCOR.handle(staff, ContentType.LILYPOND);
            }
        }
    }
}
