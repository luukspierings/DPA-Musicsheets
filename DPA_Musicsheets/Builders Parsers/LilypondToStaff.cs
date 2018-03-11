using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_models_and_patterns
{
    class LilypondToStaff
    {

        private string[] lilyArray;

        StaffBuilder builder;

        Regex notes = new Regex("[a-gr](is|es)?[,']*[0-9]+[.]?");
        Regex pitch = new Regex("[a-gr](is|es)?");
        Regex octaveUp = new Regex("'");
        Regex octaveDown = new Regex(",");
        Regex duration = new Regex("[0-9]+");
        Regex dotted = new Regex("\\.");

        Regex relative = new Regex("\\\\relative");
        Regex relativeNote = new Regex("[a-g](is|es)?[,']*");

        Regex alternative = new Regex("\\\\alternative");

        Regex repeat = new Regex("\\\\repeat");
        Regex repeatVolta = new Regex("volta");
        Regex repeatAmount = new Regex("[0-9]+");

        Regex newBlock = new Regex("{");
        Regex endBlock = new Regex("}");

        Regex time = new Regex("\\\\time");
        Regex timeMeasure = new Regex("[346]//[48]");

        Regex tempo = new Regex("\\\\tempo");
        Regex tempoValue = new Regex("4=[0-9]+");

        Regex clef = new Regex("\\\\clef");
        Regex clefValue = new Regex("(treble|bass)");

        bool inRepeat = false;
        bool inAlternative = false;
        bool inSubAlternative = false;

        public Staff load(string content)
        {
            builder = new StaffBuilder();


            content = content.Trim().ToLower()
                .Replace("\\r\\n", " ")
                .Replace("\r\n", " ")
                .Replace("\\n", " ")
                .Replace("\n", " ")
                .Replace("  ", " ");

            Debug.WriteLine(content);

            List<string> buffer = new List<string>();
            foreach(string s in content.Split(' '))
            {
                if (s != "") buffer.Add(s);
            }
            lilyArray = buffer.ToArray();


            for (int i = 0; i < lilyArray.Length; i++)
            {
                if (relative.IsMatch(lilyArray[i]))
                {

                    if (relativeNote.IsMatch(lilyArray[i + 1]))
                    {
                        Match pitchM = pitch.Match(lilyArray[i + 1]);
                        MatchCollection octaveUpM = octaveUp.Matches(lilyArray[i + 1]);
                        MatchCollection octaveDownM = octaveDown.Matches(lilyArray[i + 1]);

                        string pitchV = "";
                        int dOctave = 0;

                        if (pitchM.Success) pitchV = pitchM.Value;

                        foreach (Match upM in octaveUpM)
                        {
                            if (upM.Success) dOctave++;
                        }
                        foreach (Match downM in octaveDownM)
                        {
                            if (downM.Success) dOctave--;
                        }

                        builder.setRelative(pitchV.ToCharArray()[0], dOctave);
                    }
                    else
                    {
                        builder.setRelative();
                    }

                }

                if (clef.IsMatch(lilyArray[i]))
                {
                    Match clefM = clefValue.Match(lilyArray[i + 1]);
                    if (clefM.Success)
                    {
                        if(clefM.Value == "treble") builder.setSound(Sound.TREBLE);
                        if(clefM.Value == "bass") builder.setSound(Sound.BASS);
                    }
                }

                if (time.IsMatch(lilyArray[i]))
                {
                    Match timeM = timeMeasure.Match(lilyArray[i + 1]);

                    if (timeM.Success)
                    {
                        string[] values = timeM.Value.Split('/');
                        builder.setFirstMeasure(Int32.Parse(values[0]));
                        builder.setSecondMeasure(Int32.Parse(values[1]));
                    }

                }

                if (tempo.IsMatch(lilyArray[i]))
                {
                    Match timeM = tempoValue.Match(lilyArray[i + 1]);
                    if (timeM.Success)
                    {
                        string[] values = timeM.Value.Split('=');
                        builder.setTempo(Int32.Parse(values[1]));
                    }
                }

                if (notes.IsMatch(lilyArray[i])) {

                    Match pitchM = pitch.Match(lilyArray[i]);
                    MatchCollection octaveUpM = octaveUp.Matches(lilyArray[i]);
                    MatchCollection octaveDownM = octaveDown.Matches(lilyArray[i]);
                    Match durationM = duration.Match(lilyArray[i]);
                    Match dottedM = dotted.Match(lilyArray[i]);

                    string pitchV = "";
                    int dOctave = 0;
                    int durationV = 0;
                    bool dottedV = false;

                    if (pitchM.Success) pitchV = pitchM.Value;

                    foreach (Match upM in octaveUpM)
                    {
                        if (upM.Success) dOctave++;
                    }
                    foreach (Match downM in octaveDownM)
                    {
                        if (downM.Success) dOctave--;
                    }

                    if (durationM.Success) durationV = Int32.Parse(durationM.Value);
                    if (dottedM.Success) dottedV = true;

                    if (durationV != 0)
                    {
                        builder.addNote(durationV, pitchV, dOctave, dottedV);
                    }
                }

                if (lilyArray[i] == "|")
                {
                    builder.addBarLine();
                }

                if (repeat.IsMatch(lilyArray[i]))
                {
                    builder.startRepeat();
                    inRepeat = true;
                }
                if (repeatVolta.IsMatch(lilyArray[i]))
                {
                    Match repeatAmountM = repeatAmount.Match(lilyArray[i + 1]);
                    if (repeatAmountM.Success)
                    {
                        builder.setRepeatAmount(Int32.Parse(repeatAmountM.Value));
                        i++;
                    }
                }
                if (newBlock.IsMatch(lilyArray[i]))
                {
                    if (inRepeat && inAlternative)
                    {
                        builder.addAlternative();
                        inSubAlternative = true;
                    }
                }
                if(alternative.IsMatch(lilyArray[i]))
                {
                    inAlternative = true;
                }
                if (endBlock.IsMatch(lilyArray[i]))
                {
                    bool repeatWithAlternative = i+1 < lilyArray.Length && alternative.IsMatch(lilyArray[i + 1]);

                    if (inRepeat && !repeatWithAlternative && !inAlternative && !inSubAlternative)
                    {
                        builder.endRepeat();
                        inRepeat = false;
                    }
                    else if (inRepeat && inAlternative && !inSubAlternative)
                    {
                        inAlternative = false;
                        inRepeat = false;
                        builder.addAlternative();
                        builder.endRepeat();
                    }
                    else if(inRepeat && inAlternative && inSubAlternative)
                    {
                        inSubAlternative = false;
                    }
                }
                


            }

            return builder.getStaffObject();
        }


    }
}
