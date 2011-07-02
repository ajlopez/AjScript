namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer : IDisposable
    {
        private const char StringChar = '"';
        private const char QuotedStringChar = '\'';
        private const string Operators = "!~+-*/%&|^<>=.\\@";
        private const string Separators = "()[]{},:;";

        private static string[] otherOperators = new string[] { "++", "--", "<=", ">=", "==", "!=", "&&", "||", "*=", "/=", "%=", "+=", "-=", "===", "!==" };

        private TextReader reader;
        private Stack<Token> tokens;
        private char lastChar;
        private bool hasChar;
        private bool isConsole;
        private bool isFirstChar = true;

        public Lexer(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.reader = new StringReader(text);
            this.isConsole = false;
        }

        public Lexer(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.reader = reader;
            this.isConsole = reader.Equals(Console.In);
        }

        public Token PeekToken()
        {
            Token token = this.NextToken();

            this.PushToken(token);

            return token;
        }

        public Token NextToken()
        {
            if (this.tokens != null && this.tokens.Count>0)
                return this.tokens.Pop();

            char ch;

            try
            {
                ch = this.NextCharSkipBlanks();
            }
            catch (EndOfInputException)
            {
                return null;
            }

            if (char.IsDigit(ch))
            {
                return this.NextInteger(ch);
            }

            if (char.IsLetter(ch) || ch == '_')
            {
                return this.NextName(ch);
            }

            if (ch == StringChar)
            {
                return this.NextString();
            }

            if (ch == QuotedStringChar)
            {
                return this.NextQuotedString();
            }

            if (Separators.Contains(ch))
            {
                return this.NextSeparator(ch);
            }

            if (Operators.Contains(ch))
            {
                return this.NextOperator(ch);
            }

            throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Unknown input '{0}'", ch));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool dispose)
        {
            if (dispose && this.reader != null)
            {
                this.reader.Dispose();
            }
        }

        public void PushToken(Token token)
        {
            if (this.tokens == null)
                this.tokens = new Stack<Token>();

            this.tokens.Push(token);
        }

        private Token NextOperator(char ch)
        {
            char ch2;

            try
            {
                ch2 = this.NextChar();

                string op = ch.ToString() + ch2.ToString();

                if (otherOperators.Contains(op))
                {
                    try
                    {
                        char ch3 = this.NextChar();

                        string op2 = op + ch3.ToString();

                        if (otherOperators.Contains(op2))
                            return new Token()
                            {
                                TokenType = TokenType.Operator,
                                Value = op2
                            };

                        this.PushChar(ch3);
                    }
                    catch (EndOfInputException)
                    {
                    }

                    return new Token()
                    {
                        TokenType = TokenType.Operator,
                        Value = op
                    };
                }
                else
                {
                    this.PushChar(ch2);
                }
            }
            catch (EndOfInputException)
            {
            }

            return new Token()
            {
                TokenType = TokenType.Operator,
                Value = ch.ToString()
            };
        }

        private Token NextSeparator(char ch)
        {
            return new Token()
            {
                TokenType = TokenType.Separator,
                Value = ch.ToString()
            };
        }

        private Token NextString()
        {
            StringBuilder sb = new StringBuilder();
            char ch;
            char lastChar = (char)0;

            ch = this.NextChar();

            while (ch != StringChar || lastChar == '\\')
            {
                if (lastChar == '\\')
                {
                    switch (ch)
                    {
                        case 't':
                            sb.Length--;
                            sb.Append('\t');
                            break;
                        case 'a':
                            sb.Length--;
                            sb.Append('\a');
                            break;
                        case 'b':
                            sb.Length--;
                            sb.Append('\b');
                            break;
                        case 'e':
                            sb.Length--;
                            sb.Append((char)27);
                            break;
                        case 'f':
                            sb.Length--;
                            sb.Append('\f');
                            break;
                        case 'n':
                            sb.Length--;
                            sb.Append('\n');
                            break;
                        case 'r':
                            sb.Length--;
                            sb.Append('\r');
                            break;
                        case 'v':
                            sb.Length--;
                            sb.Append('\v');
                            break;
                        case '\\':
                            break;
                        default:
                            sb.Length--;
                            sb.Append(ch);
                            break;
                    }

                    lastChar = (char)0;
                }
                else
                {
                    sb.Append(ch);
                    lastChar = ch;
                }

                ch = this.NextChar();
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextQuotedString()
        {
            StringBuilder sb = new StringBuilder();
            char ch;
            char lastChar = (char)0;

            ch = this.NextChar();

            while (ch != QuotedStringChar)
            {
                sb.Append(ch);
                lastChar = ch;

                ch = this.NextChar();
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextInteger(char ch)
        {
            string integer = ch.ToString();

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    integer += ch;
                    ch = this.NextChar();
                }

                if (ch == '.')
                {
                    return this.NextReal(integer);
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = integer;
            token.TokenType = TokenType.Integer;

            return token;
        }

        private Token NextReal(string integerPart)
        {
            string real = integerPart + ".";
            char ch;

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    real += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = real;
            token.TokenType = TokenType.Real;

            return token;
        }

        private Token NextName(char ch)
        {
            string name = ch.ToString();

            try
            {
                ch = this.NextChar();

                while (char.IsLetterOrDigit(ch) || ch == '_')
                {
                    name += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = name;
            token.TokenType = TokenType.Name;

            if (name == "true" || name == "false")
                token.TokenType = TokenType.Boolean;

            if (name == "null" || name == "undefined")
                token.TokenType = TokenType.Object;

            return token;
        }

        private char NextCharSkipBlanks()
        {
            char ch;

            ch = this.NextChar();

            while (char.IsWhiteSpace(ch) || ch=='/')
            {
                if (ch == '/')
                {
                    char ch2 = this.NextChar();

                    if (ch2 == '/')
                        this.SkipToEndOfLine();
                    else if (ch2 == '*')
                        this.SkipToEndOfComment();
                    else
                    {
                        this.PushChar(ch2);
                        return ch;
                    }
                }

                ch = this.NextChar();
            }

            return ch;
        }

        private void SkipToEndOfLine()
        {
            char ch;

            ch = this.NextChar();

            while (ch != '\r' && ch != '\n')
                ch = this.NextChar();

            this.PushChar(ch);
        }

        private void SkipToEndOfComment()
        {
            char ch;

            ch = this.NextChar();

            while (true)
            {
                while (ch != '*')
                    ch = this.NextChar();

                char ch2 = this.NextChar();

                if (ch2 == '/')
                    return;

                ch = ch2;
            }
        }

        private void PushChar(char ch)
        {
            this.lastChar = ch;
            this.hasChar = true;
        }

        private char NextChar()
        {
            if (this.hasChar)
            {
                this.hasChar = false;
                return this.lastChar;
            }

            int ch;

            if (this.isConsole && (this.isFirstChar || this.reader.Peek() < 0))
            {
                Console.Out.Write("> ");
                Console.Out.Flush();
            }

            this.isFirstChar = false;

            ch = this.reader.Read();

            if (ch < 0)
            {
                throw new EndOfInputException();
            }

            return Convert.ToChar(ch);
        }
    }
}

