#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DsaParameters
		: ICipherParameters
    {
        private readonly BigIntegerHttp p, q , g;
        private readonly DsaValidationParameters validation;

		public DsaParameters(
            BigIntegerHttp	p,
            BigIntegerHttp	q,
            BigIntegerHttp	g)
			: this(p, q, g, null)
        {
        }

		public DsaParameters(
            BigIntegerHttp				p,
            BigIntegerHttp				q,
            BigIntegerHttp				g,
            DsaValidationParameters	parameters)
        {
			if (p == null)
				throw new ArgumentNullException("p");
			if (q == null)
				throw new ArgumentNullException("q");
			if (g == null)
				throw new ArgumentNullException("g");

			this.p = p;
            this.q = q;
			this.g = g;
			this.validation = parameters;
        }

        public BigIntegerHttp P
        {
            get { return p; }
        }

		public BigIntegerHttp Q
        {
            get { return q; }
        }

		public BigIntegerHttp G
        {
            get { return g; }
        }

		public DsaValidationParameters ValidationParameters
        {
			get { return validation; }
        }

		public override bool Equals(
			object obj)
        {
			if (obj == this)
				return true;

			DsaParameters other = obj as DsaParameters;

			if (other == null)
				return false;

			return Equals(other);
        }

		protected bool Equals(
			DsaParameters other)
		{
			return p.Equals(other.p) && q.Equals(other.q) && g.Equals(other.g);
		}

		public override int GetHashCode()
        {
			return p.GetHashCode() ^ q.GetHashCode() ^ g.GetHashCode();
        }
    }
}

#endif
