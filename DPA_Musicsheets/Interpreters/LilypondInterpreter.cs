using DPA_Musicsheets.Interpreters;
using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New_models_and_patterns
{
    class LilypondInterpreter 
    {

        StaffBuilder staffBuilder;
        Dictionary<Regex,AbstractExpression> expressions;

        public LilypondInterpreter()
        {
            expressions.Add(new Regex("\\relative"),                new NewBarExpression());
            expressions.Add(new Regex("\\clef"),                    new NewBarExpression());
            expressions.Add(new Regex("\\time"),                    new NewBarExpression());
            expressions.Add(new Regex("\\tempo"),                   new NewBarExpression());
            expressions.Add(new Regex("[a-g][,'eis]*[0-9]+[.]*"),   new NewBarExpression());
            expressions.Add(new Regex("r.*?[0-9][.]*"),             new NewBarExpression());
            expressions.Add(new Regex("|"),                         new NewBarExpression());
            expressions.Add(new Regex("{"),                         new NewBarExpression());
            expressions.Add(new Regex("}"),                         new NewBarExpression());


        }



        public Staff Interpret(String content)
        {
            staffBuilder = new StaffBuilder();

            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");


            foreach (string s in content.Split(' '))
            {

                expressions.FirstOrDefault(f => f.Key.IsMatch(s)).Value?.Interpret(s);


                //    LilypondToken token = new LilypondToken()
                //    {
                //        Value = s
                //    };

                //    switch (s)
                //    {
                //        case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                //        case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                //        case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                //        case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                //        case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                //        default: token.TokenKind = LilypondTokenKind.Unknown; break;
                //    }

                //    token.Value = s;

                //    if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                //    {
                //        token.TokenKind = LilypondTokenKind.Note;
                //    }
                //    else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                //    {
                //        token.TokenKind = LilypondTokenKind.Rest;
                //    }

                //    if (tokens.Last != null)
                //    {
                //        tokens.Last.Value.NextToken = token;
                //        token.PreviousToken = tokens.Last.Value;
                //    }

                //    tokens.AddLast(token);
            }


            return staffBuilder.getStaffObject();
        }
    }
}
