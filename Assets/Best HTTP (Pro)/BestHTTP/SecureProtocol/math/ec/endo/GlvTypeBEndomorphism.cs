#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
    public class GlvTypeBEndomorphism
        :   GlvEndomorphism
    {
        protected readonly ECCurve m_curve;
        protected readonly GlvTypeBParameters m_parameters;
        protected readonly ECPointMap m_pointMap;

        public GlvTypeBEndomorphism(ECCurve curve, GlvTypeBParameters parameters)
        {
            this.m_curve = curve;
            this.m_parameters = parameters;
            this.m_pointMap = new ScaleXPointMap(curve.FromBigInteger(parameters.Beta));
        }

        public virtual BigIntegerHttp[] DecomposeScalar(BigIntegerHttp k)
        {
            int bits = m_parameters.Bits;
            BigIntegerHttp b1 = CalculateB(k, m_parameters.G1, bits);
            BigIntegerHttp b2 = CalculateB(k, m_parameters.G2, bits);

            BigIntegerHttp[] v1 = m_parameters.V1, v2 = m_parameters.V2;
            BigIntegerHttp a = k.Subtract((b1.Multiply(v1[0])).Add(b2.Multiply(v2[0])));
            BigIntegerHttp b = (b1.Multiply(v1[1])).Add(b2.Multiply(v2[1])).Negate();

            return new BigIntegerHttp[]{ a, b };
        }

        public virtual ECPointMap PointMap
        {
            get { return m_pointMap; }
        }

        public virtual bool HasEfficientPointMap
        {
            get { return true; }
        }

        protected virtual BigIntegerHttp CalculateB(BigIntegerHttp k, BigIntegerHttp g, int t)
        {
            bool negative = (g.SignValue < 0);
            BigIntegerHttp b = k.Multiply(g.Abs());
            bool extra = b.TestBit(t - 1);
            b = b.ShiftRight(t);
            if (extra)
            {
                b = b.Add(BigIntegerHttp.One);
            }
            return negative ? b.Negate() : b;
        }
    }
}

#endif
