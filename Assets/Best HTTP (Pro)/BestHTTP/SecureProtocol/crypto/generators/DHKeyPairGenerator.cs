#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
    /**
     * a Diffie-Hellman key pair generator.
     *
     * This generates keys consistent for use in the MTI/A0 key agreement protocol
     * as described in "Handbook of Applied Cryptography", Pages 516-519.
     */
    public class DHKeyPairGenerator
		: IAsymmetricCipherKeyPairGenerator
    {
		private DHKeyGenerationParameters param;

		public virtual void Init(
			KeyGenerationParameters parameters)
        {
            this.param = (DHKeyGenerationParameters)parameters;
        }

		public virtual AsymmetricCipherKeyPairHttp GenerateKeyPair()
        {
			DHKeyGeneratorHelper helper = DHKeyGeneratorHelper.Instance;
			DHParameters dhp = param.Parameters;

			BigIntegerHttp x = helper.CalculatePrivate(dhp, param.Random);
			BigIntegerHttp y = helper.CalculatePublic(dhp, x);

			return new AsymmetricCipherKeyPairHttp(
                new DHPublicKeyParameters(y, dhp),
                new DHPrivateKeyParameters(x, dhp));
        }
    }
}

#endif
