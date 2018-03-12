using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Memento;
using Microsoft.Win32;
using System;
using System.Windows;

namespace DPA_Musicsheets.Commands
{
    class SaveCommand: Command
    {
        LilypondEditor _lilypondEditor;

        public SaveCommand(LilypondEditor lilypondEditor)
        {
            _lilypondEditor = lilypondEditor;
        }

        public override void execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".ly"))
                {
                    SaveToLilypondCommand saveToLilypondCommand = new SaveToLilypondCommand(_lilypondEditor, saveFileDialog.FileName);
                    saveToLilypondCommand.execute();
                }
                else if (extension.EndsWith(".pdf"))
                {
                    SaveToPDFCommand saveToLilypondCommand = new SaveToPDFCommand(_lilypondEditor, saveFileDialog.FileName);
                    saveToLilypondCommand.execute();
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        }
    }
}
