using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_C_sharp_45_wpf.Model
{
   public class Element
    {
       public string NameFile { get; set; }
       public string PathFile { get; set; }

       public Element() { }

       public Element(string NameFile, string PathFile)
        {
            this.NameFile = NameFile;
            this.PathFile = PathFile;
        }

        public override string ToString()
        {
            return NameFile;
        }
    }
}
