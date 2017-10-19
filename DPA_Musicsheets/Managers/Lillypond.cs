using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class Lillypond
    {

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;

        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();

        public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;


        private static List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };


        private string _lilypondText;
        public string LilypondText
        {
            get { return _lilypondText; }
            set
            {
                _lilypondText = value;
                LilypondTextChanged?.Invoke(this, new LilypondEventArgs() { LilypondText = value });
            }
        }

        public void LoadLilypond(string content)
        {
            LilypondText = content;

            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
            LinkedList<LilypondToken> tokens = GetTokensFromLilypond(content);
            WPFStaffs.Clear();
            string message;
            WPFStaffs.AddRange(GetStaffsFromTokens(tokens, out message));
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs, Message = message });

            
        }


        private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' '))
            {
                LilypondToken token = new LilypondToken()
                {
                    Value = s
                };

                switch (s)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                token.Value = s;

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Note;
                }
                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Rest;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
        }

        private static IEnumerable<MusicalSymbol> GetStaffsFromTokens(LinkedList<LilypondToken> tokens, out string message)
        {
            List<MusicalSymbol> symbols = new List<MusicalSymbol>();
            message = "";

            try
            {
                Clef currentClef = null;
                int previousOctave = 4;
                char previousNote = 'c';

                LilypondToken currentToken = tokens.First();
                while (currentToken != null)
                {
                    switch (currentToken.TokenKind)
                    {
                        case LilypondTokenKind.Unknown:
                            break;
                        case LilypondTokenKind.Note:
                            // Length
                            int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
                            // Crosses and Moles
                            int alter = 0;
                            alter += Regex.Matches(currentToken.Value, "is").Count;
                            alter -= Regex.Matches(currentToken.Value, "es|as").Count;
                            // Octaves
                            int distanceWithPreviousNote = notesorder.IndexOf(currentToken.Value[0]) - notesorder.IndexOf(previousNote);
                            if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
                            {
                                distanceWithPreviousNote -= 7; // The number of notes in an octave
                            }
                            else if (distanceWithPreviousNote < -3)
                            {
                                distanceWithPreviousNote += 7; // The number of notes in an octave
                            }

                            if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) >= 7)
                            {
                                previousOctave++;
                            }
                            else if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) < 0)
                            {
                                previousOctave--;
                            }

                            // Force up or down.
                            previousOctave += currentToken.Value.Count(c => c == '\'');
                            previousOctave -= currentToken.Value.Count(c => c == ',');

                            previousNote = currentToken.Value[0];

                            var note = new Note(currentToken.Value[0].ToString().ToUpper(), alter, previousOctave, (MusicalSymbolDuration)noteLength, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
                            note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));

                            symbols.Add(note);
                            break;
                        case LilypondTokenKind.Rest:
                            var restLength = Int32.Parse(currentToken.Value[1].ToString());
                            symbols.Add(new Rest((MusicalSymbolDuration)restLength));
                            break;
                        case LilypondTokenKind.Bar:
                            symbols.Add(new Barline());
                            break;
                        case LilypondTokenKind.Clef:
                            currentToken = currentToken.NextToken;
                            if (currentToken.Value == "treble")
                                currentClef = new Clef(ClefType.GClef, 2);
                            else if (currentToken.Value == "bass")
                                currentClef = new Clef(ClefType.FClef, 4);
                            else
                                throw new NotSupportedException($"Clef {currentToken.Value} is not supported.");

                            symbols.Add(currentClef);
                            break;
                        case LilypondTokenKind.Time:
                            currentToken = currentToken.NextToken;
                            var times = currentToken.Value.Split('/');
                            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));
                            break;
                        case LilypondTokenKind.Tempo:
                            // Tempo not supported
                            break;
                        default:
                            break;
                    }
                    currentToken = currentToken.NextToken;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return symbols;
        }



        internal void SaveToLilypond(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(LilypondText);
                outputFile.Close();
            }
        }

    }
}
