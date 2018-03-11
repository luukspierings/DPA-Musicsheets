using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public static class CKey
    {

        public static string Open         { get { return "LeftCtrl+O"; } }
        public static string InsertClef   { get { return "LeftShift+C"; } }
        public static string InsertTempo  { get { return "LeftShift+S"; } }


        public static string Save         { get { return "LeftCtrl+S"; } }
        public static string SavePdf        { get { return "LeftCtrl+S+P"; } }

        public static string InsertTime   { get { return "LeftShift+T"; } }
        public static string InsertTime44   { get { return "LeftShift+T+4"; } }
        public static string InsertTime34   { get { return "LeftShift+T+3"; } }
        public static string InsertTime68   { get { return "LeftShift+T+6"; } }

    }
}
