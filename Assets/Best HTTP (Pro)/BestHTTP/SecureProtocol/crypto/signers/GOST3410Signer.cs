#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	/**
	 * Gost R 34.10-94 Signature Algorithm
	 */
	public class Gost3410Signer
		: IDsa
	{
		private Gost3410KeyParameters key;
		private SecureRandom random;

        public virtual string AlgorithmName
		{
			get { return "GOST3410"; }
		}

        public virtual void Init(
			bool				forSigning,
			ICipherParameters	parameters)
		{
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom rParam = (ParametersWithRandom)parameters;

					this.random = rParam.Random;
					parameters = rParam.Parameters;
				}
				else
				{
					this.random = new SecureRandom();
				}

				if (!(parameters is Gost3410PrivateKeyParameters))
					throw new InvalidKeyException("GOST3410 private key required for signing");

				this.key = (Gost3410PrivateKeyParameters) parameters;
			}
			else
			{
				if (!(parameters is Gost3410PublicKeyParameters))
					throw new InvalidKeyException("GOST3410 public key required for signing");

				this.key = (Gost3410PublicKeyParameters) parameters;
			}
		}

		/**
		 * generate a signature for the given message using the key we were
		 * initialised with. For conventional Gost3410 the message should be a Gost3411
		 * hash of the message of interest.
		 *
		 * @param message the message that will be verified later.
		 */
        public virtual BigIntegerHttp[] GenerateSignature(
			byte[] message)
		{
			byte[] mRev = new byte[message.Length]; // conversion is little-endian
			for (int i = 0; i != mRev.Length; i++)
			{
				mRev[i] = message[mRev.Length - 1 - i];
			}

			BigIntegerHttp m = new BigIntegerHttp(1, mRev);
			Gost3410Parameters parameters = key.Parameters;
			BigIntegerHttp k;

			do
			{
				k = new BigIntegerHttp(parameters.Q.BitLength, random);
			}
			while (k.CompareTo(parameters.Q) >= 0);

			BigIntegerHttp r = parameters.A.ModPow(k, parameters.P).Mod(parameters.Q);

			BigIntegerHttp s = k.Multiply(m).
				Add(((Gost3410PrivateKeyParameters)key).X.Multiply(r)).
				Mod(parameters.Q);

			return new BigIntegerHttp[]{ r, s };
		}

		/**
		 * return true if the value r and s represent a Gost3410 signature for
		 * the passed in message for standard Gost3410 the message should be a
		 * Gost3411 hash of the real message to be verified.
		 */
        public virtual bool VerifySignature(
			byte[]		message,
			BigIntegerHttp	r,
			BigIntegerHttp	s)
		{
			byte[] mRev = new byte[message.Length]; // conversion is little-endian
			for (int i = 0; i != mRev.Length; i++)
			{
				mRev[i] = message[mRev.Length - 1 - i];
			}

			BigIntegerHttp m = new BigIntegerHttp(1, mRev);
			Gost3410Parameters parameters = key.Parameters;

			if (r.SignValue < 0 || parameters.Q.CompareTo(r) <= 0)
			{
				return false;
			}

			if (s.SignValue < 0 || parameters.Q.CompareTo(s) <= 0)
			{
				return false;
			}

			BigIntegerHttp v = m.ModPow(parameters.Q.Subtract(BigIntegerHttp.Two), parameters.Q);

			BigIntegerHttp z1 = s.Multiply(v).Mod(parameters.Q);
			BigIntegerHttp z2 = (parameters.Q.Subtract(r)).Multiply(v).Mod(parameters.Q);

			z1 = parameters.A.ModPow(z1, parameters.P);
			z2 = ((Gost3410PublicKeyParameters)key).Y.ModPow(z2, parameters.P);

			BigIntegerHttp u = z1.Multiply(z2).Mod(parameters.P).Mod(parameters.Q);

			return u.Equals(r);
		}
	}
}

#endif
