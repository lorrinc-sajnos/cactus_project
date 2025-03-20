using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDemo.Code.CodeLine
{
    internal class CodeLine
    {
        string code;
        public string GetCode() => code;
        public CodeLine(string code) {
            this.code = code;
        }

    }
}
