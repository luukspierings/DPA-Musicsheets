using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Managers;
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

        private MidiPlayer player;

        public MidiHandler(MidiPlayer player)
        {
            handleType = ContentType.MIDI;
            this.player = player;
        }

        public override void handle(Staff staff, ContentType contentType)
        {
            if (contentType != handleType)
            {
                base.handle(staff, contentType);
                return;
            }

            StaffToMidi staffToMidi = new StaffToMidi();
            player.load(staffToMidi.load(staff));


        }



    }
}
