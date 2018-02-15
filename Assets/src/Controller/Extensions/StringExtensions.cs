using FunnyPoker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunnyPoker.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            str = str.Trim();
            if (str == "")
            {
                return false;
            }

            return true;

        }

        public static Card ToCard(this string str)
        {
            Card c = new Card(Symbol.Clubs, Value.Invalid);
            if (str.Length != 1)
            {
                return null;
            }
            else
            {
                var firstChar = str[0];
                switch (firstChar)
                {
                    case '2':
                        c.Value = Value.Two;
                        break;
                    case '3':
                        c.Value = Value.Three;
                        break;
                    case '4':
                        c.Value = Value.Four;
                        break;
                    case '5':
                        c.Value = Value.Five;
                        break;
                    case '6':
                        c.Value = Value.Six;
                        break;
                    case '7':
                        c.Value = Value.Seven;
                        break;
                    case '8':
                        c.Value = Value.Eight;
                        break;
                    case '9':
                        c.Value = Value.Nine;
                        break;
                    case 'X':
                        c.Value = Value.Ten;
                        break;
                    case 'J':
                        c.Value = Value.Jack;
                        break;
                    case 'Q':
                        c.Value = Value.Queen;
                        break;
                    case 'K':
                        c.Value = Value.King;
                        break;
                    case 'A':
                        c.Value = Value.Ace;
                        break;
                    default:
                        return null;
                }
                return c;
            }

        }
    }
}
