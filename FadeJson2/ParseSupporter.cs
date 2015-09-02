using System.Collections.Generic;

namespace FadeJson2
{
    public class ParseSupporter
    {
        private Lexer Lexer { get; }

        public ParseSupporter(Lexer lexer) {
            Lexer = lexer;
        }

        private readonly Queue<Token> tokenQueue = new Queue<Token>();

        private Token NextToken {
            get {
                if (tokenQueue.Count == 0) {
                    return Lexer.NextToken();
                }
                return tokenQueue.Dequeue();
            }
        }

        private void RollbackToken(Token token) {
            tokenQueue.Enqueue(token);
        }

        public Token UsingToken() {
            var token = NextToken;
            if (token == null) {
                return null;
            }
            return token;
        }

        public Token UsingToken(TokenType tokenType) {
            var token = NextToken;
            if (token == null) {
                return null;
            }
            if (token.TokenType == tokenType) {
                return token;
            }
            RollbackToken(token);
            return null;
        }

        public Token UsingToken(TokenType tokenType, string value) {
            var token = NextToken;
            if (token == null) {
                return null;
            }
            if (token.TokenType == tokenType && token.Value == value) {
                return token;
            }
            RollbackToken(token);
            return null;
        }

        public Token UsingTokenExpect(TokenType tokenType) {
            var token = NextToken;
            if (token == null) {
                return null;
            }
            if (token.TokenType != tokenType) {
                return token;
            }
            RollbackToken(token);
            return null;
        }

        public bool MatchToken(TokenType tokenType, string value) {
            var token = NextToken;
            if (token == null) {
                RollbackToken(token);
                return false;
            }
            if (token.TokenType == tokenType && token.Value == value) {
                RollbackToken(token);
                return true;
            }
            RollbackToken(token);
            return false;
        }

    }
}