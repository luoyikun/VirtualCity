#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT131R2Curve
        : AbstractF2mCurve
    {
        private const int SecT131R2_DEFAULT_COORDS = COORD_LAMBDA_PROJECTIVE;

        protected readonly SecT131R2Point m_infinity;

        public SecT131R2Curve()
            : base(131, 2, 3, 8)
        {
            this.m_infinity = new SecT131R2Point(this, null, null);

            this.m_a = FromBigInteger(new BigIntegerHttp(1, Hex.Decode("03E5A88919D7CAFCBF415F07C2176573B2")));
            this.m_b = FromBigInteger(new BigIntegerHttp(1, Hex.Decode("04B8266A46C55657AC734CE38F018F2192")));
            this.m_order = new BigIntegerHttp(1, Hex.Decode("0400000000000000016954A233049BA98F"));
            this.m_cofactor = BigIntegerHttp.Two;

            this.m_coord = SecT131R2_DEFAULT_COORDS;
        }

        protected override ECCurve CloneCurve()
        {
            return new SecT131R2Curve();
        }

        public override bool SupportsCoordinateSystem(int coord)
        {
            switch (coord)
            {
            case COORD_LAMBDA_PROJECTIVE:
                return true;
            default:
                return false;
            }
        }

        public override int FieldSize
        {
            get { return 131; }
        }

        public override ECFieldElement FromBigInteger(BigIntegerHttp x)
        {
            return new SecT131FieldElement(x);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
        {
            return new SecT131R2Point(this, x, y, withCompression);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
        {
            return new SecT131R2Point(this, x, y, zs, withCompression);
        }

        public override ECPoint Infinity
        {
            get { return m_infinity; }
        }

        public override bool IsKoblitz
        {
            get { return false; }
        }

        public virtual int M
        {
            get { return 131; }
        }

        public virtual bool IsTrinomial
        {
            get { return false; }
        }

        public virtual int K1
        {
            get { return 2; }
        }

        public virtual int K2
        {
            get { return 3; }
        }

        public virtual int K3
        {
            get { return 8; }
        }
    }
}

#endif
