using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public static class CKey
    {

        public static string Open         { get { return "ctrl+o"; } }
        public static string InsertClef   { get { return "alt+c"; } }
        public static string InsertTempo  { get { return "alt+s"; } }


        public static string Save         { get { return "ctrl+s"; } }
        public static char SavePdf        { get { return 'p'; } }

        public static string InsertTime   { get { return "alt+t"; } }
        public static char InsertTime44   { get { return '4'; } }
        public static char InsertTime34   { get { return '3'; } }
        public static char InsertTime68   { get { return '6'; } }

    }
}
