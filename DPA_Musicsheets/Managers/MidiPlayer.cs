using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    class MidiPlayer
    {

        private OutputDevice _outputDevice;
        public Sequencer _sequencer;
        private bool _running;


        public MidiPlayer()
        {
            _outputDevice = new OutputDevice(0);
            _sequencer = new Sequencer();

            _sequencer.ChannelMessagePlayed += ChannelMessagePlayed;
            _sequencer.PlayingCompleted += (playingSender, playingEvent) =>
            {
                _sequencer.Stop();
                _running = false;
            };
        }

        public void play()
        {
            if (!_running && _sequencer.Sequence != null)
            {
                _running = true;
                _sequencer.Continue();
            }
        }

        public void pause()
        {
            if (_running)
            {
                _running = false;
                _sequencer.Stop();
            }
        }

        public void stop()
        {
            if (_running)
            {
                _running = false;
                _sequencer.Stop();
                _sequencer.Position = 0;
            }
        }

        public void destroy()
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
