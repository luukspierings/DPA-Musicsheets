using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Chain_of_responsibility
{
    public class Handler
    {
        protected Handler super;
        protected Handler nextHandler;
        protected ContentType handleType;

        public virtual void handle(Staff staff, ContentType contentType)
        {
            if (nextHandler != null) nextHandler.handle(staff, contentType);
        }

        public Handler addHandler(Handler handler)
        {
            if (nextHandler == null) nextHandler = handler;
            else nextHandler.addHandler(handler);
            return handler; // Usefull for chaining adding handlers.
        }

    }
}
