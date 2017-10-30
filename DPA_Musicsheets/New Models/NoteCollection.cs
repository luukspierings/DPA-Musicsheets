using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_Models
{
    public interface NoteCollection
    {

        List<Bar> getBars();

        void addNote(BaseNote note, float duration);

        float getDuration();

        bool newBar();


    }
}
