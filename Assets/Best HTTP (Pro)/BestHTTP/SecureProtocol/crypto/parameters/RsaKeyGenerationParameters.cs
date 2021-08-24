#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RsaKeyGenerationParameters
		: KeyGenerationParameters
    {
        private readonly BigIntegerHttp publicExponent;
        private readonly int certainty;

		public RsaKeyGenerationParameters(
            BigIntegerHttp		publicExponent,
            SecureRandom	random,
            int				strength,
            int				certainty)
			: base(random, strength)
        {
            this.publicExponent = publicExponent;
            this.certainty = certainty;
        }

		public BigIntegerHttp PublicExponent
        {
			get { return publicExponent; }
        }

		public int Certainty
        {
			get { return certainty; }
        }

		public override bool Equals(
			object obj)
		{
			RsaKeyGenerationParameters other = obj as RsaKeyGenerationParameters;

			if (other == null)
			{
				return false;
			}

			return certainty == other.certainty
				&& publicExponent.Equals(other.publicExponent);
		}

		public override int GetHashCode()
		{
			return certainty.GetHashCode() ^ publicExponent.GetHashCode();
		}
    }
}

#endif
