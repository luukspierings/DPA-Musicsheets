using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_models_and_patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Memento
{
    class LilypondEditor
    {

        //private string _text;
        //private string _previousText;
        //private string _nextText;
        //private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;


        private LilyCaretaker caretaker;
        private LilyOriginator originator;
        private MainController controller;
        private LilypondToStaff lilypondToStaff;


        public LilypondEditor(TextBox textbox, MainController controller)
        {
            caretaker = new LilyCaretaker();
            originator = new LilyOriginator(textbox);
            this.controller = controller;
            lilypondToStaff = new LilypondToStaff();
            setState();
        }

        public void undo()
        {
            LilyMemento memento = caretaker.undoMemento();
            if (memento != null)
            {
                originator.setMemento(memento);
                Debug.WriteLine("Did undo.");
            }
        }
        public void redo()
        {
            LilyMemento memento = caretaker.redoMemento();
            if (memento != null)
            {
                originator.setMemento(memento);
                Debug.WriteLine("Did redo.");
            }
        }

        public void setState()
        {
            caretaker.insertMemento(originator.getMemento());
        }

        public void clearState()
        {
            caretaker.resetMomento();
        }

        public void stateChanged()
        {
            _lastChange = DateTime.Now;

            Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
            {
                if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                {
                    LilyMemento currentMemento = originator.getMemento();
                    LilyMemento lastMemento = caretaker.getLast();
                    if (lastMemento != null && lastMemento.getState() != currentMemento.getState())
                    {
                        caretaker.insertMemento(currentMemento);
                    }

                    Staff staff = lilypondToStaff.load(currentMemento.getState());
                    controller.ViewCOR.handle(staff, ContentType.WPF);
                    Debug.WriteLine("New rendering set.");

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }

    }
}
