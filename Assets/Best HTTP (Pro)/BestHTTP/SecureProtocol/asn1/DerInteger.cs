#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
using System;

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public class DerInteger
        : Asn1Object
    {
        private readonly byte[] bytes;

        /**
         * return an integer from the passed in object
         *
         * @exception ArgumentException if the object cannot be converted.
         */
        public static DerInteger GetInstance(
            object obj)
        {
            if (obj == null || obj is DerInteger)
            {
                return (DerInteger)obj;
            }

            throw new ArgumentException("illegal object in GetInstance: " + Org.BouncyCastle.Utilities.Platform.GetTypeName(obj));
        }

        /**
         * return an Integer from a tagged object.
         *
         * @param obj the tagged object holding the object we want
         * @param isExplicit true if the object is meant to be explicitly
         *              tagged false otherwise.
         * @exception ArgumentException if the tagged object cannot
         *               be converted.
         */
        public static DerInteger GetInstance(
            Asn1TaggedObject	obj,
            bool				isExplicit)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

			Asn1Object o = obj.GetObject();

			if (isExplicit || o is DerInteger)
			{
				return GetInstance(o);
			}

			return new DerInteger(Asn1OctetString.GetInstance(o).GetOctets());
        }

		public DerInteger(
            int value)
        {
            bytes = BigIntegerHttp.ValueOf(value).ToByteArray();
        }

		public DerInteger(
            BigIntegerHttp value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

			bytes = value.ToByteArray();
        }

		public DerInteger(
            byte[] bytes)
        {
            this.bytes = bytes;
        }

		public BigIntegerHttp Value
        {
            get { return new BigIntegerHttp(bytes); }
        }

		/**
         * in some cases positive values Get crammed into a space,
         * that's not quite big enough...
         */
        public BigIntegerHttp PositiveValue
        {
            get { return new BigIntegerHttp(1, bytes); }
        }

        internal override void Encode(
            DerOutputStream derOut)
        {
            derOut.WriteEncoded(Asn1Tags.Integer, bytes);
        }

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(bytes);
        }

		protected override bool Asn1Equals(
			Asn1Object asn1Object)
		{
			DerInteger other = asn1Object as DerInteger;

			if (other == null)
				return false;

			return Arrays.AreEqual(this.bytes, other.bytes);
        }

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}

#endif
