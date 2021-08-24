#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	public class RsaBlindingParameters
		: ICipherParameters
	{
		private readonly RsaKeyParametersHttp	publicKey;
		private readonly BigIntegerHttp			blindingFactor;

		public RsaBlindingParameters(
			RsaKeyParametersHttp	publicKey,
			BigIntegerHttp			blindingFactor)
		{
			if (publicKey.IsPrivate)
				throw new ArgumentException("RSA parameters should be for a public key");

			this.publicKey = publicKey;
			this.blindingFactor = blindingFactor;
		}

		public RsaKeyParametersHttp PublicKey
		{
			get { return publicKey; }
		}

		public BigIntegerHttp BlindingFactor
		{
			get { return blindingFactor; }
		}
	}
}

#endif
