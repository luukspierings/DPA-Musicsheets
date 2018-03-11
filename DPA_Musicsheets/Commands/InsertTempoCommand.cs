using System.Windows.Controls;

namespace DPA_Musicsheets.Commands
{
    class InsertTempoCommand : Command
    {
        private const string TEMPO = "\\tempo 4=120";
        private TextBox _textBox;

        public InsertTempoCommand(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void execute()
        {
            var selectionIndex = _textBox.SelectionStart;
            _textBox.Text = _textBox.Text.Insert(selectionIndex, TEMPO);
            _textBox.SelectionStart = selectionIndex + TEMPO.Length;
        }
    }
}
