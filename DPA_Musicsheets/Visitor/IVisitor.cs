using DPA_Musicsheets.Models;
using DPA_Musicsheets.New_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public interface IVisitor
    {

        void visit(Bar bar);
        void visit(Repeat repeat);
        void visit(MusicNote musicnote);
        void visit(RestNote restNote);


    }
}
