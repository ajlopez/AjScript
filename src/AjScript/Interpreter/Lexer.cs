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
        private const string Operators = "!~+-*/%&|^<>=\\@";
        private const string Delimiters = "()[]{},:;.";

        private static string[] otherOperators = new string[] { "++", "--", "<=", ">=", "==", "!=", "&&", "||", "*=", "/=", "%=", "+=", "-=", "===", "!==" };

        private TextReader reader;
        private Stack<Token> tokens;
        private Stack<char> chars = new Stack<char>();
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
            if (this.tokens != null && this.tokens.Count > 0)
                return this.tokens.Pop();

            char? nch;

            nch = this.NextCharSkipBlanks();

            if (!nch.HasValue)
                return null;

            char ch = nch.Value;

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

            if (Delimiters.Contains(ch))
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
            char? nch2 = this.NextChar();

            if (nch2.HasValue)
            {
                char ch2 = nch2.Value;

                string op = ch.ToString() + ch2.ToString();

                if (otherOperators.Contains(op))
                {
                    char? nch3 = this.NextChar();

                    if (nch3.HasValue)
                    {
                        char ch3 = nch3.Value;

                        string op2 = op + ch3.ToString();

                        if (otherOperators.Contains(op2))
                            return new Token()
                            {
                                TokenType = TokenType.Operator,
                                Value = op2
                            };

                        this.PushChar(ch3);
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
                TokenType = TokenType.Delimiter,
                Value = ch.ToString()
            };
        }

        private Token NextString()
        {
            StringBuilder sb = new StringBuilder();
            char? nch;
            char lastChar = (char)0;

            nch = this.NextChar();

            while (nch.HasValue && nch.Value != StringChar || lastChar == '\\')
            {
                char ch = nch.Value;

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

                nch = this.NextChar();
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextQuotedString()
        {
            StringBuilder sb = new StringBuilder();
            char? nch;
            char lastChar = (char)0;

            nch = this.NextChar();

            while (nch.HasValue && nch.Value != QuotedStringChar)
            {
                sb.Append(nch);
                lastChar = nch.Value;

                nch = this.NextChar();
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextInteger(char? nch)
        {
            string integer = nch.ToString();

            nch = this.NextChar();

            while (nch.HasValue && char.IsDigit(nch.Value))
            {
                integer += nch;
                nch = this.NextChar();
            }

            if (nch == '.')
            {
                return this.NextReal(integer);
            }

            if (nch.HasValue)
                this.PushChar(nch.Value);

            Token token = new Token();
            token.Value = integer;
            token.TokenType = TokenType.Integer;

            return token;
        }

        private Token NextReal(string integerPart)
        {
            string real = integerPart + ".";
            char? nch;

            nch = this.NextChar();

            while (nch.HasValue && char.IsDigit(nch.Value))
            {
                real += nch;
                nch = this.NextChar();
            }

            if (nch.HasValue)
                this.PushChar(nch.Value);

            Token token;

            if (real.EndsWith("."))
            {
                this.PushChar('.');
                token = new Token();
                token.Value = integerPart;
                token.TokenType = TokenType.Integer;
                return token;
            }

            token = new Token();
            token.Value = real;
            token.TokenType = TokenType.Real;

            return token;
        }

        private Token NextName(char? nch)
        {
            string name = nch.ToString();

            nch = this.NextChar();

            while (nch.HasValue && (char.IsLetterOrDigit(nch.Value) || nch.Value == '_'))
            {
                name += nch.Value;
                nch = this.NextChar();
            }

            if (nch.HasValue)
                this.PushChar(nch.Value);

            Token token = new Token();
            token.Value = name;
            token.TokenType = TokenType.Name;

            return token;
        }

        private char? NextCharSkipBlanks()
        {
            char? nch;

            nch = this.NextChar();

            while (nch.HasValue && (char.IsWhiteSpace(nch.Value) || nch.Value == '/'))
            {
                if (nch == '/')
                {
                    char? nch2 = this.NextChar();

                    if (nch2.HasValue)
                    {
                        char ch2 = nch2.Value;

                        if (ch2 == '/')
                            this.SkipToEndOfLine();
                        else if (ch2 == '*')
                            this.SkipToEndOfComment();
                        else
                        {
                            this.PushChar(ch2);
                            return nch.Value;
                        }
                    }
                }

                nch = this.NextChar();
            }

            return nch;
        }

        private void SkipToEndOfLine()
        {
            char? nch;

            nch = this.NextChar();

            while (nch.HasValue && nch != '\r' && nch != '\n')
                nch = this.NextChar();

            if (nch.HasValue)
                this.PushChar(nch.Value);
        }

        private void SkipToEndOfComment()
        {
            char? nch;

            nch = this.NextChar();

            while (nch.HasValue)
            {
                while (nch.Value != '*')
                    nch = this.NextChar();

                char? nch2 = this.NextChar();

                if (nch2.HasValue && nch2 == '/')
                    return;

                nch = nch2;
            }
        }

        private void PushChar(char ch)
        {
            this.chars.Push(ch);
        }

        private char? NextChar()
        {
            if (this.chars.Count > 0)
                return this.chars.Pop();

            int ch;

            if (this.isConsole && (this.isFirstChar || this.reader.Peek() < 0))
            {
                Console.Out.Write("> ");
                Console.Out.Flush();
            }

            this.isFirstChar = false;

            ch = this.reader.Read();

            if (ch < 0)
                return null;

            return (char)ch;
        }
    }
}

