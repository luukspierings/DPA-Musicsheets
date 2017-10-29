using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Memento;
using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers
{
    class MainController
    {

        private TextBox fileTextBox;
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                fileTextBox.Text = _fileName;
            }
        }

        public MidiPlayer player;
        private FileHandler _fileHandler;
        public ObservableCollection<MusicalSymbol> DrawableStaff { get; set; }

        private Dictionary<string, Dictionary<char, Command>> commands;
        public Handler ViewCOR;
        public LilypondEditor lilypondEditor;


        public MainController(TextBox fileTextBox, TextBox LilyTextBox)
        {
            this.fileTextBox = fileTextBox;
            _fileHandler = new FileHandler();
            DrawableStaff = new ObservableCollection<MusicalSymbol>();
            player = new MidiPlayer();
            lilypondEditor = new LilypondEditor(LilyTextBox, this);
            
            ViewCOR = new LilypondHandler(LilyTextBox, lilypondEditor);
            ViewCOR.addHandler(new MidiHandler(player._sequencer));
            ViewCOR.addHandler(new WPFHandler(DrawableStaff));

            mapKeys();
        }

        public void executeCommand(string commandKey)
        {
            string[] splitKeys = commandKey.Split('+');

            Dictionary<char, Command> command = commands[splitKeys[0] + "+" + splitKeys[1]];
            if(splitKeys.Length > 2 && command.ContainsKey(splitKeys[2].ToCharArray()[0]))
            {
                command[splitKeys[2].ToCharArray()[0]].execute();
            }
            else
            {
                command[' '].execute();
            }
        }

        public void mapKeys()
        {
            commands = new Dictionary<string, Dictionary<char, Command>>();
            
            commands[CKey.Open] = new Dictionary<char, Command>() { { ' ', new OpenCommand(CKey.Open, this, _fileHandler) } };
            commands[CKey.InsertClef] = new Dictionary<char, Command>() { { ' ', new InsertClefCommand(CKey.InsertClef) } };
            commands[CKey.InsertTempo] = new Dictionary<char, Command>() { { ' ', new InsertTempoCommand(CKey.InsertTempo) } };

            commands[CKey.Save] = new Dictionary<char, Command>() {
                { ' ',          new SaveCommand(CKey.Save) },
                { CKey.SavePdf, new InsertTimeCommand(CKey.Save, ".pdf") },
            };
            commands[CKey.InsertTime] = new Dictionary<char, Command>() {
                { ' ',               new InsertTimeCommand(CKey.InsertTime) },
                { CKey.InsertTime44, new InsertTimeCommand(CKey.InsertTime, "4/4") },
                { CKey.InsertTime34, new InsertTimeCommand(CKey.InsertTime, "3/4") },
                { CKey.InsertTime68, new InsertTimeCommand(CKey.InsertTime, "6/8") },
            };

        }


    }
}
