using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public abstract class BaseNote
    {
        protected float duration;
        public virtual float Duration { get { return duration; } set { duration = value; } }


    }
}
