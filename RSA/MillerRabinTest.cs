using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace MillerRabin
{
    public class MillerRabinTest
    {
        public static bool TestPrimeByMillerRabin(BigInteger number)
        {
            if (number < 1)
                throw new ArgumentException("Число меньше единицы.");
            if (number % 2 == 1)
            {
                if (number == 1)
                    return true;

                BigInteger oddFactor = number - 1;
                int k = 0;
                while (oddFactor % 2 != 1)
                {
                    oddFactor /= 2;
                    k++;
                }
                BigInteger randomBig = CreateRandomBigInteger(3, number - 1);

                BigInteger b = BigIntModularPow(randomBig, oddFactor, number);

                if (b == 1 || b == number - 1)
                    return true;

                for (int i = 0; i < k; i++)
                {
                    b = BigIntModularPow(b, 2, number);
                    if (b == number - 1)
                        return true;
                }
            }
            return false;
        }

        public static BigInteger CreateRandomBigInteger(BigInteger left, BigInteger right)
        {
            if (left < 0 || right < 0)
                throw new ArgumentException("Границы должны быть неотрицательными.");
            if (left > right)
            {
                var a = right;
                right = left;
                left = a;
            }
            var leftBytes = left.ToByteArray().Length;
            var rightBytes = right.ToByteArray().Length;
            int bytesNumber;
            if (leftBytes == rightBytes)
                bytesNumber = leftBytes;
            else
                bytesNumber = RandomNumberGenerator.GetInt32(leftBytes, rightBytes);
            var generator = RandomNumberGenerator.Create();
            byte[] bytes = new byte[bytesNumber];
            BigInteger result;
            do
            {
                generator.GetBytes(bytes);
                result = new BigInteger(bytes);
            }
            while (result < left || result > right);
            return result;
        }

        public static BigInteger BigIntModularPow(BigInteger number, BigInteger degree, BigInteger modul)
        {
            if (number < 0 || degree < 0 || modul < 0)
                throw new ArgumentException("Число/a меньше нуля.");
            if (number == 0)
                return number;

            string binaryDegree = ConvertBigToBinary(degree);
            int length = binaryDegree.Length;

            BigInteger[] calculations = new BigInteger[length];
            calculations[0] = number;

            BigInteger result = BigInteger.One;
            for (int i = 1; i < calculations.Length; i++)
            {
                calculations[i] = calculations[i - 1] * calculations[i - 1] % modul;
                if (binaryDegree[length - i] == '1')
                    result *= calculations[i - 1];
            }
            result *= calculations[calculations.Length - 1];
            return result %= modul;
        }

        public static string ConvertBigToBinary(BigInteger number)
        {
            if (number < 1)
                throw new ArgumentException("Число меньше единицы.");
            byte[] bytes = number.ToByteArray();
            StringBuilder binaryBig = new StringBuilder(bytes.Length * 8);
            for (int index = bytes.Length - 1; index >= 0; index--)
            {
                var converted = Convert.ToString(bytes[index], 2);
                if (index < bytes.Length - 1)
                    converted = converted.PadLeft(8, '0');
                binaryBig.Append(converted);
            }
            return binaryBig.ToString();
        }

        public static byte[] GetBytes(int number)
        {
            int k = 1,
                n = number;
            while (n > 255)
            {
                n /= 256;
                k++;
            }

            byte[] bytes = new byte[k];
            int index;
            n = number;
            for (index = 0; n > 255; index++)
            {
                bytes[index] = Convert.ToByte(n % 256);
                n /= 256;
            }
            bytes[index] = Convert.ToByte(n);

            return bytes;
        }
    }
}