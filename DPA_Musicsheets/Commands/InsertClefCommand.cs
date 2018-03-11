using System.Windows.Controls;

namespace DPA_Musicsheets.Commands
{
    class InsertClefCommand : Command
    {
        private const string CLEF = "\\clef t";
        private TextBox _textBox;
      
        public InsertClefCommand(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void execute()
        {
            var selectionIndex = _textBox.SelectionStart;
            _textBox.Text = _textBox.Text.Insert(selectionIndex, CLEF);
            _textBox.SelectionStart = selectionIndex + CLEF.Length;
        }
    }
}
