using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using System.Windows.Controls;
using DPA_Musicsheets.Builders_Parsers;
using System.Diagnostics;
using DPA_Musicsheets.Memento;

namespace DPA_Musicsheets.Chain_of_responsibility
{
    class LilypondHandler : Handler
    {
        TextBox textBox;
        LilypondEditor lilypondEditor;

        public LilypondHandler(TextBox text, LilypondEditor lilypondEditor)
        {
            handleType = ContentType.LILYPOND;
            textBox = text;

            this.lilypondEditor = lilypondEditor;
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

            //lilypondEditor.setState();
        }


    }
}
