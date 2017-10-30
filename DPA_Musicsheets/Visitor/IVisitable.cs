using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Visitor
{
    public interface IVisitable
    {

        void accept(IVisitor visitor);

    }
}
