#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class RandomDsaKCalculator
        :   IDsaKCalculator
    {
        private BigIntegerHttp q;
        private SecureRandom random;

        public virtual bool IsDeterministic
        {
            get { return false; }
        }

        public virtual void Init(BigIntegerHttp n, SecureRandom random)
        {
            this.q = n;
            this.random = random;
        }

        public virtual void Init(BigIntegerHttp n, BigIntegerHttp d, byte[] message)
        {
            throw new InvalidOperationException("Operation not supported");
        }

        public virtual BigIntegerHttp NextK()
        {
            int qBitLength = q.BitLength;

            BigIntegerHttp k;
            do
            {
                k = new BigIntegerHttp(qBitLength, random);
            }
            while (k.SignValue < 1 || k.CompareTo(q) >= 0);

            return k;
        }
    }
}

#endif
