using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text;

namespace SensitiveInfoDetectorApp;
public class Program
{
    static void Main(string[] args)
    {
        var detector = new SensitiveInfoDetector();

        //Lista de casos de prueba.
        string[] testCases = {
            "Mi tarjeta es 1234-5678-9101-1121",
            "Mi cuenta bancaria es 332-66-788",
            "Texto sin información sensible",
            "Múltiples casos: 1234 5678 9101 1121 y 332-66-788"
        };

        foreach (var test in testCases)
        {
            Console.WriteLine("Original: " + test);
            Console.WriteLine("Censurado: " + detector.DetectAndCensor(test));
            Console.WriteLine();
        }

        Console.ReadLine();
    }
    public class SensitiveInfoDetector
    {
        private readonly string _culture;

        public SensitiveInfoDetector(string culture = Culture.English)
        {
            _culture = culture;
        }

        // Método para detectar y censurar información sensible en el texto.
        public string DetectAndCensor(string text)
        {
            // Detectar y censurar tarjetas de crédito
            text = Regex.Replace(text, @"\b(?:\d{4}[-\s]?){3}\d{4}\b", match => CensorCreditCard(match.Value));

            // Detectar y censurar cuentas bancarias
            text = Regex.Replace(text, @"\b\d{3}[-\s]?\d{2}[-\s]?\d{3}\b", match => CensorBankAccount(match.Value));

            return text;
        }

        // Método para censurar números de tarjetas de crédito.
        private string CensorCreditCard(string cardNumber)
        {
            // Elimina guiones y espacios del número de tarjeta.
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");
            // Retorna el número de tarjeta censurado, mostrando solo los últimos 4 dígitos.
            return new string('*', cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4);
        }

        // Método para censurar números de cuentas bancarias.
        private string CensorBankAccount(string accountNumber)
        {
            // Elimina guiones y espacios del número de cuenta.
            accountNumber = accountNumber.Replace("-", "").Replace(" ", "");
            // Retorna el número de cuenta censurado, mostrando los primeros 2 y los últimos 2 dígitos.
            return accountNumber.Substring(0, 2) + new string('*', accountNumber.Length - 4) + accountNumber.Substring(accountNumber.Length - 2);
        }
    }
}
