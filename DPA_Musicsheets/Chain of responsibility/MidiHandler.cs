using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Chain_of_responsibility
{
    class MidiHandler : Handler
    {

        Sequencer sequencer;

        public MidiHandler(Sequencer midisequencer)
        {
            handleType = ContentType.MIDI;
            sequencer = midisequencer;
        }

        public override void handle(Staff staff, ContentType contentType)
        {
            if (contentType != handleType)
            {
                base.handle(staff, contentType);
                return;
            }

            StaffToMidi staffToMidi = new StaffToMidi();
            sequencer.Sequence = staffToMidi.load(staff);


        }



    }
}
