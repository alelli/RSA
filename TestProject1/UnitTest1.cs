using System.Numerics;

namespace RSAProgram
{
    [TestClass]
    public class UnitTest1
    {
        static RSA rsaParams = RSA.GenerateRSAParams();

        [TestMethod]
        public void PublicKeyTest()
        {
            Assert.AreEqual(BigInteger.GreatestCommonDivisor(rsaParams.PublicKey, rsaParams.EulerFunction), 1);
        }

        [TestMethod]
        public void PrivateKeyTest()
        {
            Assert.AreEqual(rsaParams.PrivateKey * rsaParams.PublicKey % rsaParams.EulerFunction, 1);
        }

        [TestMethod]
        public void TextsAreEqualTest()
        {
            var startText = "how are you doing";
            for (int j = 0; j < 10; j++)
            {
                var encodeResult = rsaParams.EncodeByRSA(startText);
                var decodeResult = rsaParams.DecodeByRSA(encodeResult);

                Assert.AreEqual(startText.ToUpper(), decodeResult);
            }
        }

        [TestMethod]
        public void UnsupportedSymbolsTest()
        {
            var startText = "hello!";
            for (int j = 0; j < 10; j++)
            {
                var encodeResult = rsaParams.EncodeByRSA(startText);
                var decodeResult = rsaParams.DecodeByRSA(encodeResult);

                Assert.AreEqual(startText.ToUpper(), decodeResult);
            }
        }
    }
}