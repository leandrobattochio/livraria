using System.Text.RegularExpressions;

namespace Livraria.Core.Extensions;

public static class StringExtensions
{
    public static string FormatCep(this string cep)
    {
        return Regex.Replace(cep, @"\D", "");
    }
    
    public static string FormatCnpj(this string input)
    {
        return input.Replace("/", "").Replace(".", "").Replace("-", "");
    }
}