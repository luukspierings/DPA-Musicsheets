using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_models_and_patterns;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders_Parsers
{
    class MidiToStaff
    {


        StaffBuilder builder = new StaffBuilder();


        public Staff load(Sequence sequence)
        {

            int _beatNote = 4;        // De waarde van een beatnote.
            int _bpm = 120;             // Aantal beatnotes per minute.
            int _beatsPerBar = 4;     // Aantal beatnotes per maat.

            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            string lastNotePitch = "";
            int deltaOctave = 0;

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                                    _beatNote = timeSignatureBytes[0];
                                    _beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));

                                    builder.setFirstMeasure(_beatNote);
                                    builder.setSecondMeasure(_beatsPerBar);
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    _bpm = 60000000 / tempo;

                                    builder.setTempo(_bpm);
                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        double percentageOfBar = getPercentageOfBar(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatsPerBar);
                                        int noteLength = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar);

                                        bool dotted = isDotted(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar);
                                        builder.addNote(noteLength, lastNotePitch, deltaOctave, dotted);


                                        percentageOfBarReached += percentageOfBar;
                                        if (percentageOfBarReached >= 1)
                                        {
                                            builder.addBarLine();
                                            percentageOfBar = percentageOfBar - 1;
                                        }
                                    }
                                    break;
                                default: break;
                            }
                            break;
                        case MessageType.Channel:
                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                if (channelMessage.Data2 > 0) // Data2 = loudness
                                {
                                    // Append the new note.
                                    lastNotePitch = GetNoteName(channelMessage.Data1);
                                    deltaOctave = getDeltaOctave(previousMidiKey, channelMessage.Data1);

                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    double percentageOfBar = getPercentageOfBar(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatsPerBar);

                                    int noteLength = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar);

                                    if(lastNotePitch == "r")
                                    {
                                        builder.addNote(noteLength);
                                    }
                                    else
                                    {
                                        bool dotted = isDotted(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar);
                                        builder.addNote(noteLength, lastNotePitch, deltaOctave, dotted);
                                    }

                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;

                                    percentageOfBarReached += percentageOfBar;
                                    if (percentageOfBarReached >= 1)
                                    {
                                        builder.addBarLine();
                                        percentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                                    lastNotePitch = "r";
                                }
                            }
                            break;
                    }
                }


            }

            return builder.getStaffObject();
        }


        private static int getDeltaOctave(int previousMidiKey, int midiKey)
        {
            int distance = midiKey - previousMidiKey;
            int deltaOctave = 0;
            while (distance < -6)
            {
                deltaOctave--;
                distance += 8;
            }
            while (distance > 6)
            {
                deltaOctave++;
                distance -= 8;
            }

            return deltaOctave;
        }

        private static string GetNoteName(int midiKey)
        {
            string[] notes = { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            return notes[midiKey % 12];
        }


        private int GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar)
        {
            if (nextNoteAbsoluteTicks - absoluteTicks <= 0) return 0;

            int duration = 0;
            double percentageOfBar = getPercentageOfBar(absoluteTicks, nextNoteAbsoluteTicks, division, beatsPerBar);

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2) noteLength = 2;

                    if (noteLength >= 17)     duration = 32;
                    else if (noteLength >= 9) duration = 16;
                    else if (noteLength >= 5) duration = 8;
                    else if (noteLength >= 3) duration = 4;
                    else                      duration = 2;

                    break;
                }
            }

            return duration;
        }

        private bool isDotted(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar)
        {
            if (nextNoteAbsoluteTicks - absoluteTicks <= 0) return false;

            int dots = 0;
            double percentageOfBar = getPercentageOfBar(absoluteTicks, nextNoteAbsoluteTicks, division, beatsPerBar);
    
            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2) noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32) subtractDuration = 32;
                    else if (noteLength >= 16) subtractDuration = 16;
                    else if (noteLength >= 8) subtractDuration = 8;
                    else if (noteLength >= 4) subtractDuration = 4;
                    else subtractDuration = 2;

                    double currentTime = 0;

                    while (currentTime < (noteLength - subtractDuration))
                    {
                        var addtime = 1 / ((subtractDuration / beatNote) * Math.Pow(2, dots));
                        if (addtime <= 0) break;
                        currentTime += addtime;
                        if (currentTime <= (noteLength - subtractDuration))
                        {
                            dots++;
                        }
                        if (dots >= 4) break;
                    }

                    break;
                }
            }

            return (dots >= 1);
        }



        private double getPercentageOfBar(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatsPerBar)
        {
            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;
            if (deltaTicks <= 0) return 0;

            double percentageOfBeatNote = deltaTicks / division;
            return ((1.0 / beatsPerBar) * percentageOfBeatNote);
        }






    }
}
