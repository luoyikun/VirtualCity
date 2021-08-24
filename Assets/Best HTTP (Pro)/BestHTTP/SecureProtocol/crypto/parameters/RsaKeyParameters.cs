#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	public class RsaKeyParametersHttp
		: AsymmetricKeyParameterHttp
    {
        private readonly BigIntegerHttp modulus;
        private readonly BigIntegerHttp exponent;

		public RsaKeyParametersHttp(
            bool		isPrivate,
            BigIntegerHttp	modulus,
            BigIntegerHttp	exponent)
			: base(isPrivate)
        {
			if (modulus == null)
				throw new ArgumentNullException("modulus");
			if (exponent == null)
				throw new ArgumentNullException("exponent");
			if (modulus.SignValue <= 0)
				throw new ArgumentException("Not a valid RSA modulus", "modulus");
			if (exponent.SignValue <= 0)
				throw new ArgumentException("Not a valid RSA exponent", "exponent");

			this.modulus = modulus;
			this.exponent = exponent;
        }

		public BigIntegerHttp Modulus
        {
            get { return modulus; }
        }

		public BigIntegerHttp Exponent
        {
            get { return exponent; }
        }

		public override bool Equals(
			object obj)
        {
            RsaKeyParametersHttp kp = obj as RsaKeyParametersHttp;

			if (kp == null)
			{
				return false;
			}

			return kp.IsPrivate == this.IsPrivate
				&& kp.Modulus.Equals(this.modulus)
				&& kp.Exponent.Equals(this.exponent);
        }

		public override int GetHashCode()
        {
            return modulus.GetHashCode() ^ exponent.GetHashCode() ^ IsPrivate.GetHashCode();
        }
    }
}

#endif
