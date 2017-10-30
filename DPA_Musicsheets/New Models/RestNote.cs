using DPA_Musicsheets.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class RestNote : BaseNote, IVisitable
    {
        public override void accept(IVisitor v)
        {
            v.visit(this);
        }
    }
}
