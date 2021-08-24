#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    /**
     * a DSA key pair generator.
     *
     * This Generates DSA keys in line with the method described
     * in <i>FIPS 186-3 B.1 FFC Key Pair Generation</i>.
     */
    public class DsaKeyPairGenerator
        : IAsymmetricCipherKeyPairGenerator
    {
        private static readonly BigIntegerHttp One = BigIntegerHttp.One;

        private DsaKeyGenerationParameters param;

        public void Init(
            KeyGenerationParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            // Note: If we start accepting instances of KeyGenerationParameters,
            // must apply constraint checking on strength (see DsaParametersGenerator.Init)

            this.param = (DsaKeyGenerationParameters) parameters;
        }

        public AsymmetricCipherKeyPairHttp GenerateKeyPair()
        {
            DsaParameters dsaParams = param.Parameters;

            BigIntegerHttp x = GeneratePrivateKey(dsaParams.Q, param.Random);
            BigIntegerHttp y = CalculatePublicKey(dsaParams.P, dsaParams.G, x);

            return new AsymmetricCipherKeyPairHttp(
                new DsaPublicKeyParameters(y, dsaParams),
                new DsaPrivateKeyParameters(x, dsaParams));
        }

        private static BigIntegerHttp GeneratePrivateKey(BigIntegerHttp q, SecureRandom random)
        {
            // B.1.2 Key Pair Generation by Testing Candidates
            int minWeight = q.BitLength >> 2;
            for (;;)
            {
                // TODO Prefer this method? (change test cases that used fixed random)
                // B.1.1 Key Pair Generation Using Extra Random Bits
                //BigInteger x = new BigInteger(q.BitLength + 64, random).Mod(q.Subtract(One)).Add(One);

                BigIntegerHttp x = BigIntegers.CreateRandomInRange(One, q.Subtract(One), random);
                if (WNafUtilities.GetNafWeight(x) >= minWeight)
                {
                    return x;
                }
            }
        }

        private static BigIntegerHttp CalculatePublicKey(BigIntegerHttp p, BigIntegerHttp g, BigIntegerHttp x)
        {
            return g.ModPow(x, p);
        }
    }
}

#endif
