using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Chain_of_responsibility
{
    class WPFHandler : Handler
    {

        ObservableCollection<MusicalSymbol> wpfStaff;

        public WPFHandler(ObservableCollection<MusicalSymbol> wpf)
        {
            handleType = ContentType.WPF;
            wpfStaff = wpf;
        }

        public override void handle(Staff staff, ContentType contentType)
        {
            if (contentType != handleType)
            {
                nextHandler.handle(staff, contentType);
                return;
            }

            StaffToWPF parser = new StaffToWPF();
            List<MusicalSymbol> drawableStaff = parser.load(staff);

            wpfStaff.Clear();
            foreach (var symbol in drawableStaff)
            {
                wpfStaff.Add(symbol);
            }
        }


    }
}
