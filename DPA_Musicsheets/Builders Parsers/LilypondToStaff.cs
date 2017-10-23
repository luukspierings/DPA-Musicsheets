using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_models_and_patterns
{
    class LilypondToStaff
    {

        private string[] lilyArray;

        StaffBuilder builder = new StaffBuilder();

        Regex notes = new Regex("[a-g](is|es)?[,']*[0-9]+[.]?");
        Regex pitch = new Regex("[a-g](is|es)?");
        Regex octaveUp = new Regex("'");
        Regex octaveDown = new Regex(",");
        Regex duration = new Regex("[0-9]+");
        Regex dotted = new Regex("\\.");

        public Staff load(string content)
        {
            content = content.Trim().ToLower()
                .Replace("\\r\\n", " ")
                .Replace("\r\n", " ")
                .Replace("\\n", " ")
                .Replace("\n", " ")
                .Replace("  ", " ");

            lilyArray = content.Split(' ').ToArray();

            for (int i = 0; i < lilyArray.Length; i++)
            {

                if (notes.IsMatch(lilyArray[i])){

                    Match pitchM = pitch.Match(lilyArray[i]);
                    Match octaveUpM = octaveUp.Match(lilyArray[i]);
                    Match octaveDownM = octaveDown.Match(lilyArray[i]);
                    Match durationM = duration.Match(lilyArray[i]);
                    Match dottedM = dotted.Match(lilyArray[i]);

                    string pitchV = "";
                    int octaveUpV = 0;
                    int octaveDownV = 0;
                    int durationV = 0;
                    bool dottedV = false;

                    if (pitchM.Success) pitchV = pitchM.Value;

                    if (octaveUpM.Success) octaveUpV = 1;
                    if (octaveDownM.Success) octaveDownV = -1;

                    if (durationM.Success) durationV = Int32.Parse(durationM.Value);
                    if (dottedM.Success) dottedV = true;

                    if(durationV != 0)
                    {
                        builder.addNote(durationV, pitchV, octaveUpV - octaveDownV, dottedV);

                    }

                }

                if(lilyArray[i] == "|")
                {
                    builder.addBarLine();
                }

            }
            //builder.getStaffLily();
            return builder.getStaffObject();
        }


    }
}
