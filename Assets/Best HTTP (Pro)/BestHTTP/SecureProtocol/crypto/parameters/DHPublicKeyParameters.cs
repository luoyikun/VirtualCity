#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHPublicKeyParameters
		: DHKeyParameters
    {
        private readonly BigIntegerHttp y;

		public DHPublicKeyParameters(
            BigIntegerHttp		y,
            DHParameters	parameters)
			: base(false, parameters)
        {
			if (y == null)
				throw new ArgumentNullException("y");

			this.y = y;
        }

		public DHPublicKeyParameters(
            BigIntegerHttp			y,
            DHParameters		parameters,
		    DerObjectIdentifier	algorithmOid)
			: base(false, parameters, algorithmOid)
        {
			if (y == null)
				throw new ArgumentNullException("y");

			this.y = y;
        }

        public BigIntegerHttp Y
        {
            get { return y; }
        }

		public override bool Equals(
			object  obj)
        {
			if (obj == this)
				return true;

			DHPublicKeyParameters other = obj as DHPublicKeyParameters;

			if (other == null)
				return false;

			return Equals(other);
        }

		protected bool Equals(
			DHPublicKeyParameters other)
		{
			return y.Equals(other.y) && base.Equals(other);
		}

		public override int GetHashCode()
        {
            return y.GetHashCode() ^ base.GetHashCode();
        }
    }
}

#endif
