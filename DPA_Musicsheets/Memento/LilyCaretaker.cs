using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class LilyCaretaker
    {
        private Stack<LilyMemento> UndoStack = new Stack<LilyMemento>();
        private Stack<LilyMemento> RedoStack = new Stack<LilyMemento>();

        public LilyMemento undoMemento()
        {
            if (UndoStack.Count > 1)
            {
                RedoStack.Push(UndoStack.Pop());
                return UndoStack.Peek();
            }
            else return null;
        }
        public LilyMemento redoMemento()
        {
            if (RedoStack.Count > 0)
            {
                UndoStack.Push(RedoStack.Pop());
                return UndoStack.Peek();
            }
            else return null;
        }
        public LilyMemento getLast()
        {
            if (UndoStack.Count > 0)
            {
                return UndoStack.Peek();
            }
            else return null;
        }
        public void insertMemento(LilyMemento memento)
        {
            UndoStack.Push(memento);
            RedoStack.Clear();
        }
        public void resetMomento()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }

    }
}
