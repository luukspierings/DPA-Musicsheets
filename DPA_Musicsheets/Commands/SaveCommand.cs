using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Memento;
using System;

namespace DPA_Musicsheets.Commands
{
    class SaveCommand : Command
    {
        string _extension;
        FileHandler _fileHandler;
        LilypondEditor _lilypondEditor;

        public SaveCommand(LilypondEditor lilypondEditor, FileHandler fileHandler, string extension = "")
        {
            _fileHandler = fileHandler;
            _lilypondEditor = lilypondEditor;
            _extension = extension;
        }

        public override void execute()
        {
            _fileHandler.SaveFile();


            if (_extension != "")
            {
               
            }
        }
    }
}
