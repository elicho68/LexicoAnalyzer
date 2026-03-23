using System.Text.RegularExpressions;
using LexicoAnalyzer.Web.Data;
using LexicoAnalyzer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LexicoAnalyzer.Web.Services
{
    public class LexicalAnalyzerService : ILexicalAnalyzerService
    {
        private readonly ApplicationDbContext _context;

        public LexicalAnalyzerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LexicalAnalysisResult> AnalyzeAsync(string input)
        {
            var reservedWords = await _context.ReservedWords
                .Where(x => x.IsActive)
                .Select(x => x.Word)
                .ToListAsync();

            var delimiters = await _context.Delimiters
                .Where(x => x.IsActive)
                .Select(x => x.Symbol)
                .ToListAsync();

            var operators = await _context.OperatorSymbols
                .Where(x => x.IsActive)
                .Select(x => x.Symbol)
                .ToListAsync();

            var errorCatalog = await _context.LexicalErrorCatalog
                .Where(x => x.IsActive)
                .ToListAsync();

            var result = new LexicalAnalysisResult
            {
                Input = input
            };

            string emptyInputMessage = GetErrorDescription(errorCatalog, "EMPTY_INPUT", "La cadena está vacía.");
            string invalidNumberMessage = GetErrorDescription(errorCatalog, "INVALID_NUMBER", "Token inválido: número mal formado.");
            string unknownCharacterMessage = GetErrorDescription(errorCatalog, "UNKNOWN_CHARACTER", "Carácter no reconocido.");

            if (string.IsNullOrWhiteSpace(input))
            {
                result.IsValid = false;
                result.Errors.Add(new LexicalError
                {
                    Lexeme = "",
                    Position = 0,
                    Message = emptyInputMessage
                });

                result.TotalTokens = 0;
                result.ValidTokens = 0;
                result.InvalidTokens = 1;
                return result;
            }

            int i = 0;
            int order = 1;

            while (i < input.Length)
            {
                char current = input[i];

                if (char.IsWhiteSpace(current))
                {
                    i++;
                    continue;
                }

                int startPosition = i;

                if (i + 1 < input.Length)
                {
                    string twoChar = input.Substring(i, 2);
                    if (operators.Contains(twoChar))
                    {
                        result.Tokens.Add(new LexicalToken
                        {
                            Order = order++,
                            Lexeme = twoChar,
                            Type = TokenType.Operator,
                            IsValid = true,
                            Position = startPosition
                        });

                        i += 2;
                        continue;
                    }
                }

                string oneChar = current.ToString();

                if (operators.Contains(oneChar))
                {
                    result.Tokens.Add(new LexicalToken
                    {
                        Order = order++,
                        Lexeme = oneChar,
                        Type = TokenType.Operator,
                        IsValid = true,
                        Position = startPosition
                    });

                    i++;
                    continue;
                }

                if (delimiters.Contains(oneChar))
                {
                    result.Tokens.Add(new LexicalToken
                    {
                        Order = order++,
                        Lexeme = oneChar,
                        Type = TokenType.Delimiter,
                        IsValid = true,
                        Position = startPosition
                    });

                    i++;
                    continue;
                }

                if (char.IsLetter(current))
                {
                    string lexeme = ReadIdentifier(input, ref i);

                    TokenType tokenType = reservedWords.Contains(lexeme)
                        ? TokenType.ReservedWord
                        : TokenType.Identifier;

                    result.Tokens.Add(new LexicalToken
                    {
                        Order = order++,
                        Lexeme = lexeme,
                        Type = tokenType,
                        IsValid = true,
                        Position = startPosition
                    });

                    continue;
                }

                if (char.IsDigit(current))
                {
                    string lexeme = ReadNumberOrInvalid(input, ref i);

                    bool isNumber = Regex.IsMatch(lexeme, @"^[0-9]+$");

                    if (isNumber)
                    {
                        result.Tokens.Add(new LexicalToken
                        {
                            Order = order++,
                            Lexeme = lexeme,
                            Type = TokenType.Number,
                            IsValid = true,
                            Position = startPosition
                        });
                    }
                    else
                    {
                        result.Tokens.Add(new LexicalToken
                        {
                            Order = order++,
                            Lexeme = lexeme,
                            Type = TokenType.Invalid,
                            IsValid = false,
                            Position = startPosition,
                            ErrorMessage = invalidNumberMessage
                        });

                        result.Errors.Add(new LexicalError
                        {
                            Lexeme = lexeme,
                            Position = startPosition,
                            Message = invalidNumberMessage
                        });
                    }

                    continue;
                }

                result.Tokens.Add(new LexicalToken
                {
                    Order = order++,
                    Lexeme = oneChar,
                    Type = TokenType.Invalid,
                    IsValid = false,
                    Position = startPosition,
                    ErrorMessage = unknownCharacterMessage
                });

                result.Errors.Add(new LexicalError
                {
                    Lexeme = oneChar,
                    Position = startPosition,
                    Message = unknownCharacterMessage
                });

                i++;
            }

            result.TotalTokens = result.Tokens.Count;
            result.ValidTokens = result.Tokens.Count(x => x.IsValid);
            result.InvalidTokens = result.Tokens.Count(x => !x.IsValid);
            result.IsValid = result.InvalidTokens == 0;

            return result;
        }

        private string ReadIdentifier(string input, ref int index)
        {
            int start = index;

            while (index < input.Length &&
                   char.IsLetterOrDigit(input[index]))
            {
                index++;
            }

            return input.Substring(start, index - start);
        }

        private string ReadNumberOrInvalid(string input, ref int index)
        {
            int start = index;

            while (index < input.Length &&
                   char.IsLetterOrDigit(input[index]))
            {
                index++;
            }

            return input.Substring(start, index - start);
        }

        private string GetErrorDescription(List<LexicalErrorCatalog> catalog, string code, string defaultMessage)
        {
            return catalog
                .FirstOrDefault(x => x.Code == code)?.Description
                ?? defaultMessage;
        }
    }
}