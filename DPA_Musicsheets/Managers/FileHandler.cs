
using DPA_Musicsheets.Models;
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

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {

        public Lillypond lillypondClass = new Lillypond();
        public Midi midiClass = new Midi();


        
        public Sequence MidiSequence { get; set; }

        public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;



        public void updateMidiSequence()
        {

            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });

        }



        public void OpenFile(string fileName)
        {
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {
                MidiSequence = new Sequence();
                MidiSequence.Load(fileName);
                updateMidiSequence();
                LoadMidi(MidiSequence);
            }
            else if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    sb.AppendLine(line);
                }
                
                lillypondClass.LoadLilypond(sb.ToString());

                MidiSequence = midiClass.GetSequenceFromWPFStaffs(lillypondClass.WPFStaffs);
                updateMidiSequence();
            }
            else
            {
                throw new NotSupportedException($"File extension {Path.GetExtension(fileName)} is not supported.");
            }
        }

        

        public void LoadMidi(Sequence sequence)
        {
            String lilypondContent = midiClass.LoadMidi(sequence);

            lillypondClass.LoadLilypond(lilypondContent);

            MidiSequence = midiClass.GetSequenceFromWPFStaffs(lillypondClass.WPFStaffs);
            updateMidiSequence();
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
