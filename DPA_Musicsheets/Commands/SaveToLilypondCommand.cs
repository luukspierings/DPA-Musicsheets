using DPA_Musicsheets.Memento;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class SaveToLilypondCommand: Command
    {
        private LilypondEditor _lilypondEditor;
        private string _fileName;

        public SaveToLilypondCommand(LilypondEditor lilypondEditor, string fileName = "")
        {
            _lilypondEditor = lilypondEditor;
            _fileName = fileName;
        }

        public override void execute()
        {
            if (_fileName == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond | *.ly" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    _fileName = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            using (StreamWriter outputFile = new StreamWriter(_fileName))
            {
                outputFile.Write(_lilypondEditor.GetText());
                outputFile.Close();
            }
        }
    }
}
