using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDemo.Code.CodeLine {
    internal class IncludeLine : CodeLine {
        public IncludeLine(string line) : base($"#include {line}") { }
    }
}
