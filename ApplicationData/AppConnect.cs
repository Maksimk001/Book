using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.ApplicationData
{
    internal class AppConnect
    {
        public static BookEntities model0db = new BookEntities();

        public static Users CurrentUser {  get; set; }
    }
}
