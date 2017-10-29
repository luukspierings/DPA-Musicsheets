
using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_models_and_patterns;
using Microsoft.Win32;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {

        public Lillypond lillypondClass = new Lillypond();
        public Midi midiClass = new Midi();

        LilypondToStaff lilypondToStaff = new LilypondToStaff();
        MidiToStaff midiToStaff = new MidiToStaff();


        public String selectFile()
        {
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            return fileName;
        }

        public Staff OpenFile(string fileName)
        {
            
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {
                Sequence MidiSequence = new Sequence();
                MidiSequence.Load(fileName);

                return midiToStaff.load(MidiSequence);

            }
            else if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    sb.AppendLine(line);
                }
                
                return lilypondToStaff.load(sb.ToString());
            }
            else
            {
                throw new NotSupportedException($"File extension {Path.GetExtension(fileName)} is not supported.");
            }
        }

        public void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    lillypondClass.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        }



        internal void SaveToMidi(string fileName)
        {
            Sequence sequence = midiClass.GetSequenceFromWPFStaffs(lillypondClass.WPFStaffs);

            sequence.Save(fileName);
        }

        

        internal void SaveToPDF(string fileName)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";
            lillypondClass.SaveToLilypond(tmpFileName);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

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
            while (!process.HasExited) { /* Wait for exit */
                }
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }

        
    }
}
