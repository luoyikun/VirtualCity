#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP160K1Curve
        : AbstractFpCurve
    {
        public static readonly BigIntegerHttp q = SecP160R2Curve.q;

        private const int SECP160K1_DEFAULT_COORDS = COORD_JACOBIAN;

        protected readonly SecP160K1Point m_infinity;

        public SecP160K1Curve()
            : base(q)
        {
            this.m_infinity = new SecP160K1Point(this, null, null);

            this.m_a = FromBigInteger(BigIntegerHttp.Zero);
            this.m_b = FromBigInteger(BigIntegerHttp.ValueOf(7));
            this.m_order = new BigIntegerHttp(1, Hex.Decode("0100000000000000000001B8FA16DFAB9ACA16B6B3"));
            this.m_cofactor = BigIntegerHttp.One;
            this.m_coord = SECP160K1_DEFAULT_COORDS;
        }

        protected override ECCurve CloneCurve()
        {
            return new SecP160K1Curve();
        }

        public override bool SupportsCoordinateSystem(int coord)
        {
            switch (coord)
            {
            case COORD_JACOBIAN:
                return true;
            default:
                return false;
            }
        }

        public virtual BigIntegerHttp Q
        {
            get { return q; }
        }

        public override ECPoint Infinity
        {
            get { return m_infinity; }
        }

        public override int FieldSize
        {
            get { return q.BitLength; }
        }

        public override ECFieldElement FromBigInteger(BigIntegerHttp x)
        {
            return new SecP160R2FieldElement(x);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
        {
            return new SecP160K1Point(this, x, y, withCompression);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
        {
            return new SecP160K1Point(this, x, y, zs, withCompression);
        }
    }
}

#endif
