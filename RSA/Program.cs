using System.Numerics;
using MillerRabin;

namespace RSAProgram
{
    class Program
    {
        static void Main()
        {
        }
    }

    public class RSA
    {
        static string Alphabet = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public BigInteger PrimesProduct { get; private set; }
        public BigInteger EulerFunction { get; private set; }
        public BigInteger PublicKey { get; private set; }
        public BigInteger PrivateKey { get; private set; }

        public static RSA GenerateRSAParams()
        {
            var rsaParams = new RSA();

            var firstPrimeNumber = GeneratePrimeByBits(100, 1000000);
            var secondPrimeNumber = GeneratePrimeByBits(100, 1000000);

            rsaParams.PrimesProduct = firstPrimeNumber * secondPrimeNumber;
            rsaParams.EulerFunction = (firstPrimeNumber - 1) * (secondPrimeNumber - 1);
            rsaParams.PublicKey = GeneratePublicKey(rsaParams.EulerFunction);
            rsaParams.PrivateKey = CalculatePrivateKey(rsaParams.PublicKey, rsaParams.EulerFunction);

            return rsaParams;
        }

        public string EncodeByRSA(string message)
        {
            string encodedText = "";
            for (int i = 0; i < message.Length; i++)
            {
                string letter = message[i].ToString().ToUpper();
                int symbolIndex = Alphabet.IndexOf(letter);
                if (symbolIndex < 0 || symbolIndex >= Alphabet.Length)
                    throw new ArgumentException("Текст содержит неподдерживаемые символы");
                if (symbolIndex == 0)
                    symbolIndex = Alphabet.Length;
                encodedText += MillerRabinTest.BigIntModularPow(symbolIndex, PublicKey, PrimesProduct);
                if (i != message.Length - 1)
                    encodedText += " ";
            }
            return encodedText;
        }

        public string DecodeByRSA(string message)
        {
            string decodedText = "";
            string[] parts = message.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                BigInteger textPart = BigInteger.Parse(parts[i]);
                int newIndex = (int)MillerRabinTest.BigIntModularPow(textPart, PrivateKey, PrimesProduct);
                if (newIndex == Alphabet.Length)
                    newIndex = 0;
                if (newIndex < 0 || newIndex >= Alphabet.Length)
                {
                    throw new ArgumentException($"newIndex is {newIndex}, textPart is {textPart}, i Index is {i}"); ;
                }
                else 
                    decodedText += Alphabet[newIndex].ToString();
            }
            return decodedText;
        }

        public static BigInteger GeneratePublicKey(BigInteger eulerFunction)
        {
            BigInteger publicKey;
            do
            { // генерация взаимнопростого с eulerFunction
                publicKey = MillerRabinTest.CreateRandomBigInteger(3, eulerFunction - 1); ;
            }
            while (BigInteger.GreatestCommonDivisor(publicKey, eulerFunction) != 1);
            return publicKey;
        }

        public static BigInteger CalculatePrivateKey(BigInteger publicKey, BigInteger eulerFunction)
        {
            BigInteger random = MillerRabinTest.CreateRandomBigInteger(3, 1000),
                privateKey = eulerFunction * random;
            while (privateKey * publicKey % eulerFunction != 1)
            {
                privateKey++;
            }
            return privateKey;
        }

        public static BigInteger GeneratePrimeByBits(BigInteger left, BigInteger right)
        {
            BigInteger result;
            do
            {
                result = MillerRabinTest.CreateRandomBigInteger(left, right);
            }
            while (result % 2 == 0 || MillerRabinTest.TestPrimeByMillerRabin(result) == false);
            return result;
        }
    }
}