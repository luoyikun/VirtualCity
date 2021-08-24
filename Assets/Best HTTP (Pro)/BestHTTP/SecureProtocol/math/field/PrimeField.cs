#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Math.Field
{
    internal class PrimeField
        : IFiniteField
    {
        protected readonly BigIntegerHttp characteristic;

        internal PrimeField(BigIntegerHttp characteristic)
        {
            this.characteristic = characteristic;
        }

        public virtual BigIntegerHttp Characteristic
        {
            get { return characteristic; }
        }

        public virtual int Dimension
        {
            get { return 1; }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            PrimeField other = obj as PrimeField;
            if (null == other)
            {
                return false;
            }
            return characteristic.Equals(other.characteristic);
        }

        public override int GetHashCode()
        {
            return characteristic.GetHashCode();
        }
    }
}

#endif
