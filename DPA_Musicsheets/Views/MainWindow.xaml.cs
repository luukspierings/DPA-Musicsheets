using Microsoft.Win32;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPA_Musicsheets.Managers;
using System.ComponentModel;
using PSAMWPFControlLibrary;
using PSAMControlLibrary;
using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.New_models_and_patterns;
using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using System.Collections.Generic;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // FILES
        private FileHandler filehandler;
        private string _currentState;
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
            }
        }
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        // LILYPOND
        private string _text;
        private string _previousText;
        private string _nextText;
        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;

        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    _previousText = _text;
                }
                _text = value;
            }
        }

        // MIDI
        private OutputDevice _outputDevice;
        private Sequencer _sequencer; // De sequencer maakt het mogelijk om een sequence af te spelen. Deze heeft een timer en geeft events op de juiste momenten.
        private bool _running;


        // STAFFS
        public ObservableCollection<MusicalSymbol> DrawableStaff { get; set; }


        // NEW CODE
        public Handler ViewCOR;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            // NEW CODE
            DrawableStaff = new ObservableCollection<MusicalSymbol>();

            _outputDevice = new OutputDevice(0);
            _sequencer = new Sequencer();

            ViewCOR = new LilypondHandler(LilyTextBox);
            ViewCOR.addHandler(new MidiHandler(_sequencer));
            ViewCOR.addHandler(new WPFHandler(DrawableStaff));




            // FILES
            filehandler = new FileHandler();
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            var textbox = fileNameText as TextBox;
            textbox.Text = FileName;


            // LILYPOND


            // MIDI
            _sequencer.ChannelMessagePlayed += ChannelMessagePlayed;
            _sequencer.PlayingCompleted += (playingSender, playingEvent) =>
            {
                _sequencer.Stop();
                _running = false;
            };

        }



        private void LoadFile(object sender, RoutedEventArgs e)
        {
            Staff staff = filehandler.OpenFile(FileName);

            ViewCOR.handle(staff, ContentType.LILYPOND);
            ViewCOR.handle(staff, ContentType.MIDI);
            ViewCOR.handle(staff, ContentType.WPF);

        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        }

        private void PlayContent(object sender, RoutedEventArgs e)
        {
            if (!_running && _sequencer.Sequence != null)
            {
                _running = true;
                _sequencer.Continue();
            }
        }
        private void PauseContent(object sender, RoutedEventArgs e)
        {
            if (_running)
            {
                _running = false;
                _sequencer.Stop();
            }
        }
        private void StopContent(object sender, RoutedEventArgs e)
        {
            if (_running)
            {
                _running = false;
                _sequencer.Stop();
                _sequencer.Position = 0;
            }
            
        }
        private void UndoContent(object sender, RoutedEventArgs e)
        {
            if (_previousText != LilypondText) { };

            _nextText = LilypondText;
            LilypondText = _previousText;
            _previousText = null;
        }
        private void RedoContent(object sender, RoutedEventArgs e)
        {
            if (_nextText != LilypondText) { };

            _previousText = LilypondText;
            LilypondText = _nextText;
            _nextText = null;
        }
        private void SaveContent(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    filehandler.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    filehandler.lillypondClass.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    filehandler.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        }
        private void ChangedContent(object sender, RoutedEventArgs e)
        {
            //var textBox = sender as TextBox;

            //_textChangedByLoad = true;
            //LilypondText = _previousText = textBox.Text;
            //_textChangedByLoad = false;


            //if (!_textChangedByLoad)
            //{
            //    _waitingForRender = true;
            //    _lastChange = DateTime.Now;
            //    //MessengerInstance.Send<CurrentStateMessage>(new CurrentStateMessage() { State = "Rendering..." });

            //    Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
            //    {
            //        if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
            //        {
            //            _waitingForRender = false;
            //            //UndoCommand.RaiseCanExecuteChanged();

            //            //filehandler.loadLilypond(LilypondText);

            //            LilypondToStaff lilypondToStaff = new LilypondToStaff();
            //            StaffToMidi staffToMidi = new StaffToMidi();

            //            Staff staff = lilypondToStaff.load(LilypondText);
            //            ViewCOR.handle(staff, ContentType.WPF);

            //        }
            //    }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            //}

        }

        private void ExitWindow(object sender, CancelEventArgs e)
        {
            _sequencer.Stop();
            _sequencer.Dispose();
            _outputDevice.Dispose();
        }


        private void ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            try
            {
                _outputDevice.Send(e.Message);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is OutputDeviceException)
            {
                // Don't crash when we can't play
                // We have to do it this way because IsDisposed on
                // _outDevice may be false when it is being disposed
                // so this is the only safe way to prevent race conditions
            }
        }

    }
}
