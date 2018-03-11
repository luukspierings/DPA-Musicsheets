
using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_models_and_patterns;
using Microsoft.Win32;
using Sanford.Multimedia.Midi;
using System;
using System.IO;
using System.Text;


namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {
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

            return null;
        }
    }
}
