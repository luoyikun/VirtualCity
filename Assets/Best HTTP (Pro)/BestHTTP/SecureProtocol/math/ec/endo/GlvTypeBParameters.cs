#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
    public class GlvTypeBParameters
    {
        protected readonly BigIntegerHttp m_beta;
        protected readonly BigIntegerHttp m_lambda;
        protected readonly BigIntegerHttp[] m_v1, m_v2;
        protected readonly BigIntegerHttp m_g1, m_g2;
        protected readonly int m_bits;

        public GlvTypeBParameters(BigIntegerHttp beta, BigIntegerHttp lambda, BigIntegerHttp[] v1, BigIntegerHttp[] v2,
            BigIntegerHttp g1, BigIntegerHttp g2, int bits)
        {
            this.m_beta = beta;
            this.m_lambda = lambda;
            this.m_v1 = v1;
            this.m_v2 = v2;
            this.m_g1 = g1;
            this.m_g2 = g2;
            this.m_bits = bits;
        }

        public virtual BigIntegerHttp Beta
        {
            get { return m_beta; }
        }

        public virtual BigIntegerHttp Lambda
        {
            get { return m_lambda; }
        }

        public virtual BigIntegerHttp[] V1
        {
            get { return m_v1; }
        }

        public virtual BigIntegerHttp[] V2
        {
            get { return m_v2; }
        }

        public virtual BigIntegerHttp G1
        {
            get { return m_g1; }
        }

        public virtual BigIntegerHttp G2
        {
            get { return m_g2; }
        }

        public virtual int Bits
        {
            get { return m_bits; }
        }
    }
}

#endif
