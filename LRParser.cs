using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translator;

namespace Lexer
{
    internal class LRParser
    {
        private List<TokenType> LexemeType;
        private Stack<string> LexemeStack;
        private Stack<int> StateStack;
        private int LexIndex;
        private int State;

        public LRParser(List<TokenType> lexemeType)
        {
            LexemeType = lexemeType;
            LexemeStack = new Stack<string>();
            StateStack = new Stack<int>();
            LexIndex = 0;
            GoToState(0);
        }

        private bool IsEnd()
        {
            return LexIndex > LexemeType.Count ? true : false;
        }

        private void Shift()
        {
            LexemeStack.Push(LexemeType[LexIndex].ToString());
            LexIndex++;
        }

        private void GoToState(int state)
        {
            StateStack.Push(state);
            State = state;
        }

        private void Reduce(int num, string neterminal)
        {
            for ( ; num > 0; num--)
            {
                LexemeStack.Pop();
                StateStack.Pop();
            }
            LexemeStack.Push(neterminal);
            State = StateStack.Peek();
        }

        private void Expression()
        {
            for ( ; LexemeType[LexIndex] != TokenType.ENDOFLINE; LexIndex++)
                ;
        }

        private void State0()
        {
            if (LexemeStack.Count == 0)
            {
                Shift();
            }
            switch (LexemeStack.Peek())
            {
                case "<program>":
                    MessageBox.Show("Успешное завершение разбора");
                    LexIndex = LexemeType.Count + 1;
                    break;
                case "<spis. opis.>":
                    GoToState(1);
                    break;
                case "<opis.>":
                    GoToState(2);
                    break;
                case "DIM":
                    GoToState(4);
                    break;
                default:
                    throw new Exception($"Ожидался Dim, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State1()
        {
            switch (LexemeStack.Peek())
            {
                case "<spis. opis.>":
                    Shift();
                    break;
                case "<spis. oper.>":
                    if (LexIndex >= LexemeType.Count)
                    {
                        GoToState(5);
                    }
                    else
                    {
                        GoToState(35);
                    }
                    break;
                case "<oper.>":
                    GoToState(6);
                    break;
                case "<oper. loop>":
                    GoToState(8);
                    break;
                case "<prisv.>":
                    GoToState(9);
                    break;
                case "FOR":
                    GoToState(10);
                    break;
                case "ID":
                    GoToState(11);
                    break;
                default:
                    throw new Exception($"Ожидался оператор for или id, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State2()
        {
            switch (LexemeStack.Peek())
            {
                case "<opis.>":
                    if (LexemeType[LexIndex] == TokenType.FOR || LexemeType[LexIndex] == TokenType.ID)
                    {
                        Reduce(1, "<spis. opis.>");
                    }
                    else if (LexemeType[LexIndex] == TokenType.DIM)
                    {
                        Shift();
                        GoToState(4);
                    }
                    break;
                case "<spis. opis.>":
                    GoToState(12);
                    break;
                default:
                    throw new Exception($"Ожидался оператор for или id, либо Dim, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State4()
        {
            switch (LexemeStack.Peek())
            {
                case "DIM":
                    Shift();
                    break;
                case "<spis. perem.>":
                    GoToState(13);
                    break;
                case "ID":
                    GoToState(14);
                    break;
                default:
                    throw new Exception($"Ожидался id, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State5()
        {
            Reduce(2, "<program>");
            GoToState(0);
        }

        private void State6()
        {
            switch (LexemeStack.Peek())
            {
                case "<oper.>":
                    if (LexIndex >= LexemeType.Count || LexemeType[LexIndex] == TokenType.NEXT)
                    {
                        Reduce(1, "<spis. oper.>");
                    }
                    else if (LexemeType[LexIndex] == TokenType.FOR || LexemeType[LexIndex] == TokenType.ID)
                    {
                        Shift();
                        GoToState(1);
                    }
                    break;
                case "<spis. oper.>":
                    GoToState(16);
                    break;
                default:
                    throw new Exception($"Ожидался оператор for или id, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State8()
        {
            if (string.Equals(LexemeStack.Peek(), "<oper. loop>"))
            {
                Reduce(1, "<oper.>");
            }
        }

        private void State9()
        {
            if (string.Equals(LexemeStack.Peek(), "<prisv.>"))
            {
                Reduce(1, "<oper.>");
            }
        }

        private void State10()
        {
            switch (LexemeStack.Peek())
            {
                case "FOR":
                    Shift();
                    break;
                case "ID":
                    GoToState(17);
                    break;
                default:
                    throw new Exception($"Ожидался id, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State11()
        {
            switch (LexemeStack.Peek())
            {
                case "ID":
                    Shift();
                    break;
                case "EQUAL":
                    GoToState(18);
                    break;
                default:
                    throw new Exception($"Ожидался =, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State12()
        {
            Reduce(2, "<spis. opis.>");
        }

        private void State13()
        {
            switch (LexemeStack.Peek())
            {
                case "<spis. perem.>":
                    Shift();
                    break;
                case "AS":
                    GoToState(19);
                    break;
                case "COMMA":
                    GoToState(20);
                    break;
                default:
                    throw new Exception($"Ожидался as или ',', а встретился {LexemeStack.Peek()}");
            }
        }

        private void State14()
        {
            if (string.Equals(LexemeStack.Peek(), "ID"))
            {
                Reduce(1, "<spis. perem.>");
            }
        }

        private void State16()
        {
            Reduce(2, "<spis. oper.>");
        }

        private void State17()
        {
            switch (LexemeStack.Peek())
            {
                case "ID":
                    Shift();
                    break;
                case "EQUAL":
                    GoToState(21);
                    break;
                default:
                    throw new Exception($"Ожидался =, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State18()
        {
            switch (LexemeStack.Peek())
            {
                case "EQUAL":
                    Shift();
                    Expression();
                    Shift();
                    GoToState(22);
                    break;
                default:
                    throw new Exception($"Ожидался =, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State19()
        {
            switch (LexemeStack.Peek())
            {
                case "AS":
                    Shift();
                    break;
                case "<type>":
                    GoToState(23);
                    break;
                case "INTEGER":
                    GoToState(24);
                    break;
                case "BOOLEAN":
                    GoToState(25);
                    break;
                case "DOUBLE":
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"Ожидался тип данных(integer, boolean, double), а встретился {LexemeStack.Peek()}");
            }
        }

        private void State20()
        {
            switch (LexemeStack.Peek())
            {
                case "COMMA":
                    Shift();
                    break;
                case "ID":
                    GoToState(27);
                    break;
                default:
                    throw new Exception($"Ожидался идентификатор, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State21()
        {
            switch (LexemeStack.Peek())
            {
                case "EQUAL":
                    Shift();
                    break;
                case "<operand>":
                    GoToState(28);
                    break;
                case "ID":
                    GoToState(29);
                    break;
                case "LITERAL":
                    GoToState(30);
                    break;
                default:
                    throw new Exception($"Ожидался идентификатор или литерал, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State22()
        {
            if (string.Equals(LexemeStack.Peek(), "ENDOFLINE"))
            {
                GoToState(31);
            }
            else
            {
                throw new Exception($"Ожидался \\n, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State23()
        {
            switch (LexemeStack.Peek())
            {
                case "<type>":
                    Shift();
                    break;
                case "ENDOFLINE":
                    GoToState(32);
                    break;
                default:
                    throw new Exception($"Ожидался \\n, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State24()
        {
            if (string.Equals(LexemeStack.Peek(), "INTEGER"))
            {
                Reduce(1, "<type>");
            }
        }

        private void State25()
        {
            if (string.Equals(LexemeStack.Peek(), "BOOLEAN"))
            {
                Reduce(1, "<type>");
            }
        }

        private void State26()
        {
            if (string.Equals(LexemeStack.Peek(), "DOUBLE"))
            {
                Reduce(1, "<type>");
            }
        }

        private void State27()
        {
            Reduce(3, "<spis. perem.>");
        }

        private void State28()
        {
            switch (LexemeStack.Peek())
            {
                case "<operand>":
                    Shift();
                    break;
                case "TO":
                    GoToState(33);
                    break;
                default:
                    throw new Exception($"Ожидался to, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State29()
        {
            if (string.Equals(LexemeStack.Peek(), "ID"))
            {
                Reduce(1, "<operand>");
            }
        }

        private void State30()
        {
            if (string.Equals(LexemeStack.Peek(), "LITERAL"))
            {
                Reduce(1, "<operand>");
            }
        }

        private void State31()
        {
            Reduce(4, "<prisv.>");
        }

        private void State32()
        {
            Reduce(5, "<opis.>");
        }

        private void State33()
        {
            switch (LexemeStack.Peek())
            {
                case "TO":
                    Shift();
                    break;
                case "<operand>":
                    GoToState(34);
                    break;
                case "ID":
                    GoToState(29);
                    break;
                case "LITERAL":
                    GoToState(30);
                    break;
                default:
                    throw new Exception($"Ожидался идентификатор или литерал, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State34()
        {
            switch (LexemeStack.Peek())
            {
                case "<operand>":
                    Shift();
                    break;
                case "ENDOFLINE":
                    GoToState(35);
                    break;
                default:
                    throw new Exception($"Ожидался \\n, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State35()
        {
            switch (LexemeStack.Peek())
            {
                case "ENDOFLINE":
                    Shift();
                    break;
                case "<spis. oper.>":
                    GoToState(36);
                    break;
                case "<oper.>":
                    GoToState(6);
                    break;
                case "FOR":
                    GoToState(1);
                    break;
                case "ID":
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"Ожидался for или идентификатор, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State36()
        {
            switch (LexemeStack.Peek())
            {
                case "<spis. oper.>":
                    Shift();
                    break;
                case "NEXT":
                    GoToState(37);
                    break;
                default:
                    throw new Exception($"Ожидался next, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State37()
        {
            switch (LexemeStack.Peek())
            {
                case "NEXT":
                    Shift();
                    break;
                case "FOR":
                    GoToState(38);
                    break;
                default:
                    throw new Exception($"Ожидался for, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State38()
        {
            switch (LexemeStack.Peek())
            {
                case "FOR":
                    Shift();
                    break;
                case "ENDOFLINE":
                    GoToState(39);
                    break;
                default:
                    throw new Exception($"Ожидался \\n, а встретился {LexemeStack.Peek()}");
            }
        }

        private void State39()
        {
            Reduce(11, "<oper. loop>");
            GoToState(1);
        }

        public void Analyze()
        {
            while (!IsEnd())
            {
                switch (State)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                }
            }
        }
    }
}