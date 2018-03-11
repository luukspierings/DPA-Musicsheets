using System;
using System.Windows.Controls;

namespace DPA_Musicsheets.Commands
{
    class InsertTimeCommand : Command
    {
        private const string TIME = "\\time ";
        private TextBox _textBox;
        private string _time;

        public InsertTimeCommand(TextBox textbox, string time = "4/4")
        {
            _textBox = textbox;
            _time = time;
        }

        public override void execute()
        {
            Console.WriteLine((TIME + _time));

            var selectionIndex = _textBox.SelectionStart;
            _textBox.Text = _textBox.Text.Insert(selectionIndex, TIME + _time);
            _textBox.SelectionStart = selectionIndex + (TIME + _time).Length;
        }
    }
}
