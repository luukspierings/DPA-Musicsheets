using DPA_Musicsheets.Memento;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Commands
{
    class SaveToPDFCommand: Command
    {
        private LilypondEditor _lilypondEditor;
        private string _fileName;

        public SaveToPDFCommand(LilypondEditor lilypondEditor, string fileName = "")
        {
            _lilypondEditor = lilypondEditor;
            _fileName = fileName;
        }

        public override void execute()
        {
            if(_fileName == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF|*.pdf" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    _fileName = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            string withoutExtension = Path.GetFileNameWithoutExtension(_fileName);
            string tmpFileName = $"{_fileName}-tmp.ly";

            using (StreamWriter outputFile = new StreamWriter(tmpFileName))
            {
                outputFile.Write(_lilypondEditor.GetText());
                outputFile.Close();
            }

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(_fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(_fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }


            try
            {
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
                {
                    File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                    File.Delete(tmpFileName);
                }

                MessageBox.Show("Successful saved to PDF");
            }
            catch
            {
                MessageBox.Show("Could not convert to PDF");
            }
           
            _fileName = string.Empty;
        }
    }
}
