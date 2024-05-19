using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public enum TokenType
    {
        INTEGER, BOOLEAN, DOUBLE, LITERAL, ID, DELIMETER, DIM,
        AS, FOR, TO, NEXT, EQUAL, PLUS,
        MINUS, STAR, CHERTA, COMMA, LPAR, RPAR,
        MENYSHE, BOLYSHE, AND, OR, ENDOFLINE
    }
}