using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.New_Models;

namespace DPA_Musicsheets.Builders_Parsers
{
    class StaffToWPF : IVisitor
    {
        MusicNote previousNote;

        MusicalSymbol symbolBuffer;

        List<MusicalSymbol> symbols;

        public List<MusicalSymbol> load(Staff staff)
        {
            symbols = new List<MusicalSymbol>();
            //message = "";

            try
            {
                Clef currentClef = null;
                previousNote = (MusicNote)staff.getBars().First().notes.First();
                int previousOctave = previousNote.Octave;

                //LilypondToken currentToken = tokens.First();


                //if (currentToken.Value == "treble")
                if (staff.sound == Sound.TREBLE) currentClef = new Clef(ClefType.GClef, 2);
                else if (staff.sound == Sound.BASS) currentClef = new Clef(ClefType.FClef, 2);
                else throw new NotSupportedException($"Clef {currentClef.ToString()} is not supported.");
                //else if (currentToken.Value == "bass")
                //    currentClef = new Clef(ClefType.FClef, 4);
                //else
                //    throw new NotSupportedException($"Clef {currentToken.Value} is not supported.");

                symbols.Add(currentClef);

                symbols.Add(new TimeSignature(TimeSignatureType.Numbers, (UInt32)staff.firstMeasure, (UInt32)staff.secondMeasure));


                foreach (Bar b in staff.getBars())
                {
                    b.accept(this);
                }



                //while (currentToken != null)
                //{
                //    switch (currentToken.TokenKind)
                //    {
                //        case LilypondTokenKind.Unknown:
                //            break;
                //        case LilypondTokenKind.Note:
                            
                //            break;
                //        case LilypondTokenKind.Rest:
                //            var restLength = Int32.Parse(currentToken.Value[1].ToString());
                //            symbols.Add(new Rest((MusicalSymbolDuration)restLength));
                //            break;
                //        case LilypondTokenKind.Bar:
                //            break;
                //        case LilypondTokenKind.Clef:
                            
                //            break;
                //        case LilypondTokenKind.Time:
                //            currentToken = currentToken.NextToken;
                //            var times = currentToken.Value.Split('/');
                //            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));
                //            break;
                //        case LilypondTokenKind.Tempo:
                //            // Tempo not supported
                //            break;
                //        default:
                //            break;
                //    }
                //    currentToken = currentToken.NextToken;
                //}
            }
            catch (Exception ex)
            {
                //message = ex.Message;
            }

            return symbols;
        }

        public void visit(Bar bar)
        {
            foreach (BaseNote n in bar.notes)
            {
                n.accept(this);
                symbols.Add(symbolBuffer);
            }
            symbols.Add(new Barline());
        }

        public void visit(Repeat repeat)
        {
        }

        public void visit(MusicNote n)
        {
            // Length
            //int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
            float noteLength = 1 / n.Duration;

            // Crosses and Moles
            int alter = 0;
            //alter += Regex.Matches(currentToken.Value, "is").Count;
            //alter -= Regex.Matches(currentToken.Value, "es|as").Count;

            if (n.PitchModifier == PitchModifier.Sharp) alter++;
            if (n.PitchModifier == PitchModifier.Flat) alter--;


            // Octaves
            //int distanceWithPreviousNote = notesorder.IndexOf(currentToken.Value[0]) - notesorder.IndexOf(previousNote);
            //if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
            //{
            //    distanceWithPreviousNote -= 7; // The number of notes in an octave
            //}
            //else if (distanceWithPreviousNote < -3)
            //{
            //    distanceWithPreviousNote += 7; // The number of notes in an octave
            //}

            //if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) >= 7)
            //{
            //    previousOctave++;
            //}
            //else if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) < 0)
            //{
            //    previousOctave--;
            //}

            //// Force up or down.
            //previousOctave += currentToken.Value.Count(c => c == '\'');
            //previousOctave -= currentToken.Value.Count(c => c == ',');

            previousNote = n;

            var note = new Note(n.Pitch.ToString().ToUpper(), alter, n.Octave, (MusicalSymbolDuration)noteLength, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
            //note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));
            note.NumberOfDots += (n.Dotted) ? 1 : 0;

            symbolBuffer = note;
        }

        public void visit(RestNote n)
        {
            float noteLength = 1 / n.Duration;
            symbolBuffer = new Rest((MusicalSymbolDuration)noteLength);
        }
    }
}
