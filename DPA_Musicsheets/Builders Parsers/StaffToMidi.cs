using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders_Parsers
{
    class StaffToMidi
    {
        
        public Sequence load(Staff staff)
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);


            int _beatNote = staff.firstMeasure;
            int _beatsPerBar = staff.secondMeasure;

            // Calculate tempo
            int speed = (60000000 / staff.tempo);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            byte[] timeSignature = new byte[4];
            timeSignature[0] = (byte)_beatsPerBar;
            timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
            metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));    


            foreach (Bar b in staff.bars)
            {
                foreach (BaseNote n in b.notes)
                {
                    MusicNote m = (MusicNote)n;

                    // Calculate duration
                    double absoluteLength = m.Duration;

                    double relationToQuartNote = _beatNote / 4.0;
                    double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                    double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                    string pitch = m.Pitch.ToString();
                    if (m.PitchModifier == PitchModifier.Flat) pitch += "es";
                    if (m.PitchModifier == PitchModifier.Sharp) pitch += "is";

                    // Calculate height
                    int noteHeight = notesOrderWithCrosses.IndexOf(m.Pitch.ToString()) + ((m.Octave) * 12);
                    //noteHeight += note.Alter;
                    notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                    absoluteTicks += (int)deltaTicks;
                    notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);

            return sequence;
        }








    }
}
