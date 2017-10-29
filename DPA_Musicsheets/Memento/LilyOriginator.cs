using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Memento
{
    class LilyOriginator
    {

        private TextBox _textbox;

        public LilyOriginator(TextBox textBox)
        {
            _textbox = textBox;
        }

        public LilyMemento getMemento()
        {
            return new LilyMemento(_textbox.Text);
        }
        public void setMemento(LilyMemento memento)
        {
            _textbox.Text = memento.getState();
        }


    }
}
