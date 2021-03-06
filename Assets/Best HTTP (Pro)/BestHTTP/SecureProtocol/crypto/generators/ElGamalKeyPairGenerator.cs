#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
    /**
     * a ElGamal key pair generator.
     * <p>
     * This Generates keys consistent for use with ElGamal as described in
     * page 164 of "Handbook of Applied Cryptography".</p>
     */
    public class ElGamalKeyPairGenerator
		: IAsymmetricCipherKeyPairGenerator
    {
        private ElGamalKeyGenerationParameters param;

        public void Init(
			KeyGenerationParameters parameters)
        {
            this.param = (ElGamalKeyGenerationParameters) parameters;
        }

        public AsymmetricCipherKeyPairHttp GenerateKeyPair()
        {
			DHKeyGeneratorHelper helper = DHKeyGeneratorHelper.Instance;
			ElGamalParameters egp = param.Parameters;
			DHParameters dhp = new DHParameters(egp.P, egp.G, null, 0, egp.L);

			BigIntegerHttp x = helper.CalculatePrivate(dhp, param.Random);
			BigIntegerHttp y = helper.CalculatePublic(dhp, x);

			return new AsymmetricCipherKeyPairHttp(
                new ElGamalPublicKeyParameters(y, egp),
                new ElGamalPrivateKeyParameters(x, egp));
        }
    }

}

#endif
