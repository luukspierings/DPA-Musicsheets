using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using System.Windows.Controls;
using DPA_Musicsheets.Builders_Parsers;
using System.Diagnostics;

namespace DPA_Musicsheets.Chain_of_responsibility
{
    class LilypondHandler : Handler
    {
        TextBox textBox;

        public LilypondHandler(TextBox text)
        {
            handleType = ContentType.LILYPOND;
            textBox = text;
            textBox.Text = "Your lilypond text will appear here.";
        }

        public override void handle(Staff staff, ContentType contentType)
        {
            if (contentType != handleType)
            {
                base.handle(staff, contentType);
                return;
            }

            StaffToLilypond parser = new StaffToLilypond();

            textBox.Clear();
            textBox.Text = parser.load(staff);
            Debug.Write(textBox.Text);

        }


    }
}
