using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    internal class Parser
    {
        private List<TokenType> LexemeType;
        private List<string> LexResult;
        private TokenType CurrentLexeme;
        private int Index;

        public Parser(List<TokenType> lexemeType, List<string> lexResult)
        {
            LexemeType = lexemeType;
            LexResult = lexResult;
            Index = 0;
            CurrentLexeme = LexemeType[Index];
        }

        // Переход на следующий тип лексемы
        private void Next()
        {
            if (Index < LexemeType.Count - 1)
            {
                CurrentLexeme = LexemeType[++Index];
            }
        }

        // Программа
        public void Program()
        {
            ListOfDescriptions();
            OperatorList();
            MessageBox.Show("Синтаксический разбор успешно выполнен");
        }

        #region ListOfDescriptions

        // Список описаний
        private void ListOfDescriptions()
        {
            Description();
            Next();
            Y();
        }

        // Описание
        private void Description()
        {
            if (CurrentLexeme != TokenType.DIM)
            {
                throw new Exception($"Ожидался Dim, а встретился {CurrentLexeme}");
            }
            Next();
            VariablesList();
            Next();
            VariableType();
            Next();
            if (CurrentLexeme != TokenType.ENDOFLINE)
            {
                throw new Exception($"Ожидался конец строки, а встретился {CurrentLexeme}");
            }
        }

        private void Y()
        {
            if (CurrentLexeme == TokenType.FOR || CurrentLexeme == TokenType.ID)
            {
                return;
            }
            Z();
        }

        private void Z()
        {
            Description();
            Next();
            Y();
        }

        #endregion

        #region VariableList

        // Список переменных
        private void VariablesList()
        {
            if (CurrentLexeme != TokenType.ID)
            {
                throw new Exception($"Ожидался идентификатор, а встретился {CurrentLexeme}");
            }
            Next();
            X();
        }

        private void X()
        {
            if (CurrentLexeme == TokenType.AS)
            {
                return;
            }
            U();
        }

        private void U()
        {
            if (CurrentLexeme != TokenType.COMMA)
            {
                throw new Exception($"Ожидалась \",\", а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.ID)
            {
                throw new Exception($"Ожидался идентификатор, а встретился {CurrentLexeme}");
            }
            Next();
            X();
        }

        #endregion

        // Тип данных переменной
        private void VariableType()
        {
            if (CurrentLexeme == TokenType.INTEGER || CurrentLexeme == TokenType.BOOLEAN || CurrentLexeme == TokenType.DOUBLE)
            {
                return;
            }
            throw new Exception($"Ожидался тип данных(integer, boolean, double), а встретился {CurrentLexeme}");
        }

        #region OperatorList

        // Список операторов
        private void OperatorList()
        {
            Operator();
            Next();
            R();
        }

        // Оператор
        private void Operator()
        {
            if (CurrentLexeme == TokenType.FOR)
            {
                LoopOperator();
            }
            else if (CurrentLexeme == TokenType.ID)
            {
                Assigment();
            }
            else
            {
                throw new Exception($"Ожидался оператор for или идентификатор, а встретился {CurrentLexeme}");
            }
        }

        // Оператор цикла
        private void LoopOperator()
        {
            if (CurrentLexeme != TokenType.FOR)
            {
                throw new Exception($"Ожидался оператор for, а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.ID)
            {
                throw new Exception($"Ожидался идентификатоор, а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.EQUAL)
            {
                throw new Exception($"Ожидался знак '=', а встретился {CurrentLexeme}");
            }
            Next();
            // Проверка первого операнда
            Operand();
            Next();
            if (CurrentLexeme != TokenType.TO)
            {
                throw new Exception($"Ожидался to, а встретился {CurrentLexeme}");
            }
            Next();
            // Проверка второго операнда
            Operand();
            Next();
            if (CurrentLexeme != TokenType.ENDOFLINE)
            {
                throw new Exception($"Ожидался \\n, а встретился {CurrentLexeme}");
            }
            Next();
            OperatorList();
            if (CurrentLexeme != TokenType.NEXT)
            {
                throw new Exception($"Ожидался next, а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.FOR)
            {
                throw new Exception($"Ожидался оператор for, а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.ENDOFLINE)
            {
                throw new Exception($"Ожидался \\n, а встретился {CurrentLexeme}");
            }
        }

        // Операнд
        private void Operand()
        {
            if (CurrentLexeme != TokenType.ID && CurrentLexeme != TokenType.LITERAL)
            {
                throw new Exception($"Ожидался идентификатор или литерал, а встретился {CurrentLexeme}");
            }
        }

        // Присваивание
        private void Assigment()
        {
            if (CurrentLexeme != TokenType.ID)
            {
                throw new Exception($"Ожидался идентификатор, а встретился {CurrentLexeme}");
            }
            Next();
            if (CurrentLexeme != TokenType.EQUAL)
            {
                throw new Exception($"Ожидался знак '=', а встретился {CurrentLexeme}");
            }
            Next();
            Expression();
        }

        #region Expression

        // Матрица арифметического оператора
        public List<string> Matrix = new List<string>();
        private int ExprCounter = 0;

        // Метод возвращает значение лексемы
        private string ToValue(string valueAndType)
        {
            string res = string.Empty;
            for (int i = 0; i < valueAndType.Length; i++)
            {
                if (valueAndType[i] != ' ')
                {
                    res += valueAndType[i];
                }
                else
                {
                    return res;
                }
            }
            return null;
        }

        // Метод записывает в матрицу тройку [OP, операнд, операнд]
        private void K(Stack<string> operands, TokenType op)
        {
            char sign;
            switch (op)
            {
                case TokenType.PLUS:
                    sign = '+';
                    break;
                case TokenType.MINUS:
                    sign = '-';
                    break;
                case TokenType.STAR:
                    sign = '*';
                    break;
                case TokenType.CHERTA:
                    sign = '/';
                    break;
                case TokenType.MENYSHE:
                    sign = '<';
                    break;
                case TokenType.BOLYSHE:
                    sign = '>';
                    break;
                case TokenType.AND:
                    sign = '&';
                    break;
                case TokenType.OR:
                    sign = '|';
                    break;
                case TokenType.EQUAL:
                    sign = '=';
                    break;
                default:
                    sign = ' ';
                    break;
            }
            string second = operands.Pop();
            string first = operands.Pop();
            Matrix.Add($"M{ExprCounter}   {sign} {first} {second}\n");
            operands.Push($"M{ExprCounter++}");
        }

        // Разбор выражения методом Бауэра-Замельзона
        private void Expression()
        {
            Stack<string> operands = new Stack<string>();
            Stack<TokenType> operators = new Stack<TokenType>();
            List<TokenType> listOfOperators = new List<TokenType>()
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.STAR,
                TokenType.CHERTA,
                TokenType.LPAR,
                TokenType.RPAR,
                TokenType.MENYSHE,
                TokenType.BOLYSHE,
                TokenType.AND,
                TokenType.OR,
                TokenType.EQUAL,
                TokenType.ENDOFLINE
            };

            while (true)
            {
                if (CurrentLexeme == TokenType.ID || CurrentLexeme == TokenType.LITERAL)
                {
                    operands.Push(ToValue(LexResult[Index]));
                    Next();
                    if (CurrentLexeme == TokenType.ID || CurrentLexeme == TokenType.LITERAL)
                    {
                        throw new Exception("Ошибка в выражении. Конец разбора");
                    }
                    continue;
                }
                if (listOfOperators.Contains(CurrentLexeme))
                {
                    // Вершина стека
                    if (operators.Count == 0)
                    {
                        // Входной поток
                        if (CurrentLexeme == TokenType.ENDOFLINE)
                        {
                            Matrix.Add("\n");
                            ExprCounter = 0;
                            MessageBox.Show("Успешное завершение разбора выражения");
                            return;
                        }
                        else if (CurrentLexeme == TokenType.LPAR || CurrentLexeme == TokenType.PLUS || CurrentLexeme == TokenType.MINUS
                           || CurrentLexeme == TokenType.STAR || CurrentLexeme == TokenType.CHERTA || CurrentLexeme == TokenType.MENYSHE
                           || CurrentLexeme == TokenType.BOLYSHE || CurrentLexeme == TokenType.OR || CurrentLexeme == TokenType.AND
                           || CurrentLexeme == TokenType.EQUAL)
                        {
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                        else if (CurrentLexeme == TokenType.RPAR)
                        {
                            throw new Exception("Ошибка в выражении. Конец разбора");
                        }
                    }
                    if (operators.Peek() == TokenType.LPAR)
                    {
                        // Входной поток
                        if (CurrentLexeme == TokenType.ENDOFLINE)
                        {
                            throw new Exception("Ошибка в выражении. Конец разбора");
                        }
                        else if (CurrentLexeme == TokenType.LPAR || CurrentLexeme == TokenType.PLUS || CurrentLexeme == TokenType.MINUS
                           || CurrentLexeme == TokenType.STAR || CurrentLexeme == TokenType.CHERTA || CurrentLexeme == TokenType.MENYSHE
                           || CurrentLexeme == TokenType.BOLYSHE || CurrentLexeme == TokenType.OR || CurrentLexeme == TokenType.AND
                           || CurrentLexeme == TokenType.EQUAL)
                        {
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                        else if (CurrentLexeme == TokenType.RPAR)
                        {
                            operators.Pop();
                            Next();
                            continue;
                        }
                    }
                    else if (operators.Peek() == TokenType.PLUS || operators.Peek() == TokenType.MINUS || operators.Peek() == TokenType.OR)
                    {
                        // Входной поток
                        if (CurrentLexeme == TokenType.ENDOFLINE || CurrentLexeme == TokenType.RPAR || CurrentLexeme == TokenType.MENYSHE
                            || CurrentLexeme == TokenType.BOLYSHE || CurrentLexeme == TokenType.EQUAL)
                        {
                            K(operands, operators.Pop());
                        }
                        else if (CurrentLexeme == TokenType.LPAR || CurrentLexeme == TokenType.STAR || CurrentLexeme == TokenType.CHERTA
                            || CurrentLexeme == TokenType.AND)
                        {
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                        else if (CurrentLexeme == TokenType.PLUS || CurrentLexeme == TokenType.MINUS || CurrentLexeme == TokenType.OR)
                        {
                            K(operands, operators.Pop());
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                    }
                    else if (operators.Peek() == TokenType.STAR || operators.Peek() == TokenType.CHERTA || operators.Peek() == TokenType.AND)
                    {
                        // Входной поток
                        if (CurrentLexeme == TokenType.ENDOFLINE || CurrentLexeme == TokenType.PLUS || CurrentLexeme == TokenType.MINUS 
                           || CurrentLexeme == TokenType.RPAR || CurrentLexeme == TokenType.BOLYSHE || CurrentLexeme == TokenType.MENYSHE
                           || CurrentLexeme == TokenType.OR || CurrentLexeme == TokenType.EQUAL)
                        {
                            K(operands, operators.Pop());
                        }
                        else if (CurrentLexeme == TokenType.LPAR)
                        {
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                        else if (CurrentLexeme == TokenType.STAR || CurrentLexeme == TokenType.CHERTA || CurrentLexeme == TokenType.AND)
                        {
                            K(operands, operators.Pop());
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                    }
                    else if (operators.Peek() == TokenType.MENYSHE || operators.Peek() == TokenType.BOLYSHE || operators.Peek() == TokenType.EQUAL)
                    {
                        // Входной поток
                        if (CurrentLexeme == TokenType.ENDOFLINE || CurrentLexeme == TokenType.RPAR)
                        {
                            K(operands, operators.Pop());
                        }
                        else if (CurrentLexeme == TokenType.LPAR || CurrentLexeme == TokenType.PLUS || CurrentLexeme == TokenType.MINUS
                            || CurrentLexeme == TokenType.OR || CurrentLexeme == TokenType.STAR || CurrentLexeme == TokenType.CHERTA
                            || CurrentLexeme == TokenType.AND)
                        {
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                        else if (CurrentLexeme == TokenType.MENYSHE || CurrentLexeme == TokenType.BOLYSHE || CurrentLexeme == TokenType.EQUAL)
                        {
                            K(operands, operators.Pop());
                            operators.Push(CurrentLexeme);
                            Next();
                            continue;
                        }
                    }
                }
                else
                {
                    throw new Exception("Недопустимый тип лексемы в выражении");
                }
            }
        }

        #endregion

        private void R()
        {
            if (CurrentLexeme == TokenType.FOR || CurrentLexeme == TokenType.ID)
            {
                W();
            }
            return;
        }

        private void W()
        {
            Operator();
            Next();
            R();
        }

        #endregion
    }
}