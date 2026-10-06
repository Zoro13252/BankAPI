using System.Security.Cryptography;
namespace BankAPI.Services
{
    public class GenerateCardNumberService
    {
        string bin = "2200";
        public string GenerateCardNumber()
        {
            int[] res = new int[9];
            for(int i = 0; i < 9; i++)
            {
                int a = RandomNumberGenerator.GetInt32(0, 9);
                res[i] = a;
            }
            string nineNumberResult = string.Concat(res);

            string resultNumber = bin + nineNumberResult;
            int lunaNumber = CalculateLuhnCheckDigit(resultNumber);
            

            return resultNumber + lunaNumber;
        }

        static int CalculateLuhnCheckDigit(string number)
        {
            int sum = 0;
            bool alternate = true;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = number[i] - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }

            return (10 - (sum % 10)) % 10;
        }
    }
}
