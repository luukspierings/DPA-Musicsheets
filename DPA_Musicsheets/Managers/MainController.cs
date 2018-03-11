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

        private Dictionary<string, Command> commands;
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
            Command command = commands[commandKey];
            if (command != null)
            {
                command.execute();
            }
        }

        public void mapKeys()
        {
            commands = new Dictionary<string, Command>
            {
                [CKey.Open] = new OpenCommand(CKey.Open, this, _fileHandler),
                [CKey.InsertClef] = new InsertClefCommand(CKey.InsertClef),
                [CKey.InsertTempo] = new InsertTempoCommand(CKey.InsertTempo),

                [CKey.Save] = new SaveCommand(CKey.Save),
                [CKey.SavePdf] = new SaveCommand(CKey.Save, ".pdf"),

                [CKey.InsertTime] = new InsertTimeCommand(CKey.InsertTime),
                [CKey.InsertTime44] = new InsertTimeCommand(CKey.InsertTime, "4/4"),
                [CKey.InsertTime34] = new InsertTimeCommand(CKey.InsertTime, "3/4"),
                [CKey.InsertTime68] = new InsertTimeCommand(CKey.InsertTime, "6/8")
            };
        }
    }
}
