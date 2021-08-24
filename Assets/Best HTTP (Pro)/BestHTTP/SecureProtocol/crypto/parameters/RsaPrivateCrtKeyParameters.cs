#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class RsaPrivateCrtKeyParametersHttp
		: RsaKeyParametersHttp
    {
        private readonly BigIntegerHttp e, p, q, dP, dQ, qInv;

		public RsaPrivateCrtKeyParametersHttp(
            BigIntegerHttp	modulus,
            BigIntegerHttp	publicExponent,
            BigIntegerHttp	privateExponent,
            BigIntegerHttp	p,
            BigIntegerHttp	q,
            BigIntegerHttp	dP,
            BigIntegerHttp	dQ,
            BigIntegerHttp	qInv)
			: base(true, modulus, privateExponent)
        {
			ValidateValue(publicExponent, "publicExponent", "exponent");
			ValidateValue(p, "p", "P value");
			ValidateValue(q, "q", "Q value");
			ValidateValue(dP, "dP", "DP value");
			ValidateValue(dQ, "dQ", "DQ value");
			ValidateValue(qInv, "qInv", "InverseQ value");

			this.e = publicExponent;
            this.p = p;
            this.q = q;
            this.dP = dP;
            this.dQ = dQ;
            this.qInv = qInv;
        }

		public BigIntegerHttp PublicExponent
        {
            get { return e; }
		}

		public BigIntegerHttp P
		{
			get { return p; }
		}

		public BigIntegerHttp Q
		{
			get { return q; }
		}

		public BigIntegerHttp DP
		{
			get { return dP; }
		}

		public BigIntegerHttp DQ
		{
			get { return dQ; }
		}

		public BigIntegerHttp QInv
		{
			get { return qInv; }
		}

		public override bool Equals(
			object obj)
		{
			if (obj == this)
				return true;

			RsaPrivateCrtKeyParametersHttp kp = obj as RsaPrivateCrtKeyParametersHttp;

			if (kp == null)
				return false;

			return kp.DP.Equals(dP)
				&& kp.DQ.Equals(dQ)
				&& kp.Exponent.Equals(this.Exponent)
				&& kp.Modulus.Equals(this.Modulus)
				&& kp.P.Equals(p)
				&& kp.Q.Equals(q)
				&& kp.PublicExponent.Equals(e)
				&& kp.QInv.Equals(qInv);
		}

		public override int GetHashCode()
		{
			return DP.GetHashCode() ^ DQ.GetHashCode() ^ Exponent.GetHashCode() ^ Modulus.GetHashCode()
				^ P.GetHashCode() ^ Q.GetHashCode() ^ PublicExponent.GetHashCode() ^ QInv.GetHashCode();
		}

		private static void ValidateValue(BigIntegerHttp x, string name, string desc)
		{
			if (x == null)
				throw new ArgumentNullException(name);
			if (x.SignValue <= 0)
				throw new ArgumentException("Not a valid RSA " + desc, name);
		}
	}
}

#endif
