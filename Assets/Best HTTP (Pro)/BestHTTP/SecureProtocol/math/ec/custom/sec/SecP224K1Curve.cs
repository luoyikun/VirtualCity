#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224K1Curve
        : AbstractFpCurve
    {
        public static readonly BigIntegerHttp q = new BigIntegerHttp(1,
            Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D"));

        private const int SECP224K1_DEFAULT_COORDS = COORD_JACOBIAN;

        protected readonly SecP224K1Point m_infinity;

        public SecP224K1Curve()
            : base(q)
        {
            this.m_infinity = new SecP224K1Point(this, null, null);

            this.m_a = FromBigInteger(BigIntegerHttp.Zero);
            this.m_b = FromBigInteger(BigIntegerHttp.ValueOf(5));
            this.m_order = new BigIntegerHttp(1, Hex.Decode("010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7"));
            this.m_cofactor = BigIntegerHttp.One;
            this.m_coord = SECP224K1_DEFAULT_COORDS;
        }

        protected override ECCurve CloneCurve()
        {
            return new SecP224K1Curve();
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
            return new SecP224K1FieldElement(x);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
        {
            return new SecP224K1Point(this, x, y, withCompression);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
        {
            return new SecP224K1Point(this, x, y, zs, withCompression);
        }
    }
}

#endif
