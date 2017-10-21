using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Interpreters
{
    abstract class AbstractExpression
    {
        public abstract void Interpret(String s);
    }
}
