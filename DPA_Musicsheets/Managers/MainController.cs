using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Memento;
using PSAMControlLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public FileHandler FileHandler { get; set; }
        public ObservableCollection<MusicalSymbol> DrawableStaff { get; set; }

        private Dictionary<string, Command> commands;
        public Handler ViewCOR;
        public LilypondEditor lilypondEditor;

        public MainController(TextBox fileTextBox, TextBox LilyTextBox)
        {
            this.fileTextBox = fileTextBox;
            FileHandler = new FileHandler();
            DrawableStaff = new ObservableCollection<MusicalSymbol>();
            player = new MidiPlayer();
            lilypondEditor = new LilypondEditor(LilyTextBox, this);

            ViewCOR = new LilypondHandler(LilyTextBox, lilypondEditor);
            ViewCOR.addHandler(new MidiHandler(player));
            ViewCOR.addHandler(new WPFHandler(DrawableStaff));

            commands = new Dictionary<string, Command>
            {
                [CKey.InsertClef] = new InsertClefCommand(LilyTextBox),
                [CKey.InsertTempo] = new InsertTempoCommand(LilyTextBox),

                [CKey.InsertTime] = new InsertTimeCommand(LilyTextBox),
                [CKey.InsertTime44] = new InsertTimeCommand(LilyTextBox, "4/4"),
                [CKey.InsertTime34] = new InsertTimeCommand(LilyTextBox, "3/4"),
                [CKey.InsertTime68] = new InsertTimeCommand(LilyTextBox, "6/8"),

                [CKey.Open] = new OpenCommand(this),
                [CKey.Save] = new SaveCommand(lilypondEditor),
                [CKey.SavePdf] = new SaveToPDFCommand(lilypondEditor)
            };
        }

        public void executeCommand(string commandKey)
        {
            if (commands.ContainsKey(commandKey))
            {
                commands[commandKey].execute();
            }
        }
    }
}
