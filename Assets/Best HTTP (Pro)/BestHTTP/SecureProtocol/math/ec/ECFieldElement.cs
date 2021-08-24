#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.Diagnostics;

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class ECFieldElement
    {
        public abstract BigIntegerHttp ToBigInteger();
        public abstract string FieldName { get; }
        public abstract int FieldSize { get; }
        public abstract ECFieldElement Add(ECFieldElement b);
        public abstract ECFieldElement AddOne();
        public abstract ECFieldElement Subtract(ECFieldElement b);
        public abstract ECFieldElement Multiply(ECFieldElement b);
        public abstract ECFieldElement Divide(ECFieldElement b);
        public abstract ECFieldElement Negate();
        public abstract ECFieldElement Square();
        public abstract ECFieldElement Invert();
        public abstract ECFieldElement Sqrt();

        public virtual int BitLength
        {
            get { return ToBigInteger().BitLength; }
        }

        public virtual bool IsOne
        {
            get { return BitLength == 1; }
        }

        public virtual bool IsZero
        {
            get { return 0 == ToBigInteger().SignValue; }
        }

        public virtual ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            return Multiply(b).Subtract(x.Multiply(y));
        }

        public virtual ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            return Multiply(b).Add(x.Multiply(y));
        }

        public virtual ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
        {
            return Square().Subtract(x.Multiply(y));
        }

        public virtual ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            return Square().Add(x.Multiply(y));
        }

        public virtual ECFieldElement SquarePow(int pow)
        {
            ECFieldElement r = this;
            for (int i = 0; i < pow; ++i)
            {
                r = r.Square();
            }
            return r;
        }

        public virtual bool TestBitZero()
        {
            return ToBigInteger().TestBit(0);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ECFieldElement);
        }

        public virtual bool Equals(ECFieldElement other)
        {
            if (this == other)
                return true;
            if (null == other)
                return false;
            return ToBigInteger().Equals(other.ToBigInteger());
        }

        public override int GetHashCode()
        {
            return ToBigInteger().GetHashCode();
        }

        public override string ToString()
        {
            return this.ToBigInteger().ToString(16);
        }

        public virtual byte[] GetEncoded()
        {
            return BigIntegers.AsUnsignedByteArray((FieldSize + 7) / 8, ToBigInteger());
        }
    }

    public class FpFieldElement
        : ECFieldElement
    {
        private readonly BigIntegerHttp q, r, x;

        internal static BigIntegerHttp CalculateResidue(BigIntegerHttp p)
        {
            int bitLength = p.BitLength;
            if (bitLength >= 96)
            {
                BigIntegerHttp firstWord = p.ShiftRight(bitLength - 64);
                if (firstWord.LongValue == -1L)
                {
                    return BigIntegerHttp.One.ShiftLeft(bitLength).Subtract(p);
                }
                if ((bitLength & 7) == 0)
                {
                    return BigIntegerHttp.One.ShiftLeft(bitLength << 1).Divide(p).Negate();
                }
            }
            return null;
        }

        public FpFieldElement(BigIntegerHttp q, BigIntegerHttp x)
            : this(q, CalculateResidue(q), x)
        {
        }

        internal FpFieldElement(BigIntegerHttp q, BigIntegerHttp r, BigIntegerHttp x)
        {
            if (x == null || x.SignValue < 0 || x.CompareTo(q) >= 0)
                throw new ArgumentException("value invalid in Fp field element", "x");

            this.q = q;
            this.r = r;
            this.x = x;
        }

        public override BigIntegerHttp ToBigInteger()
        {
            return x;
        }

        /**
         * return the field name for this field.
         *
         * @return the string "Fp".
         */
        public override string FieldName
        {
            get { return "Fp"; }
        }

        public override int FieldSize
        {
            get { return q.BitLength; }
        }

        public BigIntegerHttp Q
        {
            get { return q; }
        }

        public override ECFieldElement Add(
            ECFieldElement b)
        {
            return new FpFieldElement(q, r, ModAdd(x, b.ToBigInteger()));
        }

        public override ECFieldElement AddOne()
        {
            BigIntegerHttp x2 = x.Add(BigIntegerHttp.One);
            if (x2.CompareTo(q) == 0)
            {
                x2 = BigIntegerHttp.Zero;
            }
            return new FpFieldElement(q, r, x2);
        }

        public override ECFieldElement Subtract(
            ECFieldElement b)
        {
            return new FpFieldElement(q, r, ModSubtract(x, b.ToBigInteger()));
        }

        public override ECFieldElement Multiply(
            ECFieldElement b)
        {
            return new FpFieldElement(q, r, ModMult(x, b.ToBigInteger()));
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            BigIntegerHttp ax = this.x, bx = b.ToBigInteger(), xx = x.ToBigInteger(), yx = y.ToBigInteger();
            BigIntegerHttp ab = ax.Multiply(bx);
            BigIntegerHttp xy = xx.Multiply(yx);
            return new FpFieldElement(q, r, ModReduce(ab.Subtract(xy)));
        }

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            BigIntegerHttp ax = this.x, bx = b.ToBigInteger(), xx = x.ToBigInteger(), yx = y.ToBigInteger();
            BigIntegerHttp ab = ax.Multiply(bx);
            BigIntegerHttp xy = xx.Multiply(yx);
            BigIntegerHttp sum = ab.Add(xy);
            if (r != null && r.SignValue < 0 && sum.BitLength > (q.BitLength << 1))
            {
                sum = sum.Subtract(q.ShiftLeft(q.BitLength));
            }
            return new FpFieldElement(q, r, ModReduce(sum));
        }

        public override ECFieldElement Divide(
            ECFieldElement b)
        {
            return new FpFieldElement(q, r, ModMult(x, ModInverse(b.ToBigInteger())));
        }

        public override ECFieldElement Negate()
        {
            return x.SignValue == 0 ? this : new FpFieldElement(q, r, q.Subtract(x));
        }

        public override ECFieldElement Square()
        {
            return new FpFieldElement(q, r, ModMult(x, x));
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
        {
            BigIntegerHttp ax = this.x, xx = x.ToBigInteger(), yx = y.ToBigInteger();
            BigIntegerHttp aa = ax.Multiply(ax);
            BigIntegerHttp xy = xx.Multiply(yx);
            return new FpFieldElement(q, r, ModReduce(aa.Subtract(xy)));
        }

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            BigIntegerHttp ax = this.x, xx = x.ToBigInteger(), yx = y.ToBigInteger();
            BigIntegerHttp aa = ax.Multiply(ax);
            BigIntegerHttp xy = xx.Multiply(yx);
            BigIntegerHttp sum = aa.Add(xy);
            if (r != null && r.SignValue < 0 && sum.BitLength > (q.BitLength << 1))
            {
                sum = sum.Subtract(q.ShiftLeft(q.BitLength));
            }
            return new FpFieldElement(q, r, ModReduce(sum));
        }

        public override ECFieldElement Invert()
        {
            // TODO Modular inversion can be faster for a (Generalized) Mersenne Prime.
            return new FpFieldElement(q, r, ModInverse(x));
        }

        /**
         * return a sqrt root - the routine verifies that the calculation
         * returns the right value - if none exists it returns null.
         */
        public override ECFieldElement Sqrt()
        {
            if (IsZero || IsOne)
                return this;

            if (!q.TestBit(0))
                throw Org.BouncyCastle.Utilities.Platform.CreateNotImplementedException("even value of q");

            if (q.TestBit(1)) // q == 4m + 3
            {
                BigIntegerHttp e = q.ShiftRight(2).Add(BigIntegerHttp.One);
                return CheckSqrt(new FpFieldElement(q, r, x.ModPow(e, q)));
            }

            if (q.TestBit(2)) // q == 8m + 5
            {
                BigIntegerHttp t1 = x.ModPow(q.ShiftRight(3), q);
                BigIntegerHttp t2 = ModMult(t1, x);
                BigIntegerHttp t3 = ModMult(t2, t1);

                if (t3.Equals(BigIntegerHttp.One))
                {
                    return CheckSqrt(new FpFieldElement(q, r, t2));
                }

                // TODO This is constant and could be precomputed
                BigIntegerHttp t4 = BigIntegerHttp.Two.ModPow(q.ShiftRight(2), q);

                BigIntegerHttp y = ModMult(t2, t4);

                return CheckSqrt(new FpFieldElement(q, r, y));
            }

            // q == 8m + 1

            BigIntegerHttp legendreExponent = q.ShiftRight(1);
            if (!(x.ModPow(legendreExponent, q).Equals(BigIntegerHttp.One)))
                return null;

            BigIntegerHttp X = this.x;
            BigIntegerHttp fourX = ModDouble(ModDouble(X)); ;

            BigIntegerHttp k = legendreExponent.Add(BigIntegerHttp.One), qMinusOne = q.Subtract(BigIntegerHttp.One);

            BigIntegerHttp U, V;
            do
            {
                BigIntegerHttp P;
                do
                {
                    P = BigIntegerHttp.Arbitrary(q.BitLength);
                }
                while (P.CompareTo(q) >= 0
                    || !ModReduce(P.Multiply(P).Subtract(fourX)).ModPow(legendreExponent, q).Equals(qMinusOne));

                BigIntegerHttp[] result = LucasSequence(P, X, k);
                U = result[0];
                V = result[1];

                if (ModMult(V, V).Equals(fourX))
                {
                    return new FpFieldElement(q, r, ModHalfAbs(V));
                }
            }
            while (U.Equals(BigIntegerHttp.One) || U.Equals(qMinusOne));

            return null;
        }

        private ECFieldElement CheckSqrt(ECFieldElement z)
        {
            return z.Square().Equals(this) ? z : null;
        }

        private BigIntegerHttp[] LucasSequence(
            BigIntegerHttp	P,
            BigIntegerHttp	Q,
            BigIntegerHttp	k)
        {
            // TODO Research and apply "common-multiplicand multiplication here"

            int n = k.BitLength;
            int s = k.GetLowestSetBit();

            Debug.Assert(k.TestBit(s));

            BigIntegerHttp Uh = BigIntegerHttp.One;
            BigIntegerHttp Vl = BigIntegerHttp.Two;
            BigIntegerHttp Vh = P;
            BigIntegerHttp Ql = BigIntegerHttp.One;
            BigIntegerHttp Qh = BigIntegerHttp.One;

            for (int j = n - 1; j >= s + 1; --j)
            {
                Ql = ModMult(Ql, Qh);

                if (k.TestBit(j))
                {
                    Qh = ModMult(Ql, Q);
                    Uh = ModMult(Uh, Vh);
                    Vl = ModReduce(Vh.Multiply(Vl).Subtract(P.Multiply(Ql)));
                    Vh = ModReduce(Vh.Multiply(Vh).Subtract(Qh.ShiftLeft(1)));
                }
                else
                {
                    Qh = Ql;
                    Uh = ModReduce(Uh.Multiply(Vl).Subtract(Ql));
                    Vh = ModReduce(Vh.Multiply(Vl).Subtract(P.Multiply(Ql)));
                    Vl = ModReduce(Vl.Multiply(Vl).Subtract(Ql.ShiftLeft(1)));
                }
            }

            Ql = ModMult(Ql, Qh);
            Qh = ModMult(Ql, Q);
            Uh = ModReduce(Uh.Multiply(Vl).Subtract(Ql));
            Vl = ModReduce(Vh.Multiply(Vl).Subtract(P.Multiply(Ql)));
            Ql = ModMult(Ql, Qh);

            for (int j = 1; j <= s; ++j)
            {
                Uh = ModMult(Uh, Vl);
                Vl = ModReduce(Vl.Multiply(Vl).Subtract(Ql.ShiftLeft(1)));
                Ql = ModMult(Ql, Ql);
            }

            return new BigIntegerHttp[] { Uh, Vl };
        }

        protected virtual BigIntegerHttp ModAdd(BigIntegerHttp x1, BigIntegerHttp x2)
        {
            BigIntegerHttp x3 = x1.Add(x2);
            if (x3.CompareTo(q) >= 0)
            {
                x3 = x3.Subtract(q);
            }
            return x3;
        }

        protected virtual BigIntegerHttp ModDouble(BigIntegerHttp x)
        {
            BigIntegerHttp _2x = x.ShiftLeft(1);
            if (_2x.CompareTo(q) >= 0)
            {
                _2x = _2x.Subtract(q);
            }
            return _2x;
        }

        protected virtual BigIntegerHttp ModHalf(BigIntegerHttp x)
        {
            if (x.TestBit(0))
            {
                x = q.Add(x);
            }
            return x.ShiftRight(1);
        }

        protected virtual BigIntegerHttp ModHalfAbs(BigIntegerHttp x)
        {
            if (x.TestBit(0))
            {
                x = q.Subtract(x);
            }
            return x.ShiftRight(1);
        }

        protected virtual BigIntegerHttp ModInverse(BigIntegerHttp x)
        {
            int bits = FieldSize;
            int len = (bits + 31) >> 5;
            uint[] p = Nat.FromBigInteger(bits, q);
            uint[] n = Nat.FromBigInteger(bits, x);
            uint[] z = Nat.Create(len);
            Mod.Invert(p, n, z);
            return Nat.ToBigInteger(len, z);
        }

        protected virtual BigIntegerHttp ModMult(BigIntegerHttp x1, BigIntegerHttp x2)
        {
            return ModReduce(x1.Multiply(x2));
        }

        protected virtual BigIntegerHttp ModReduce(BigIntegerHttp x)
        {
            if (r == null)
            {
                x = x.Mod(q);
            }
            else
            {
                bool negative = x.SignValue < 0;
                if (negative)
                {
                    x = x.Abs();
                }
                int qLen = q.BitLength;
                if (r.SignValue > 0)
                {
                    BigIntegerHttp qMod = BigIntegerHttp.One.ShiftLeft(qLen);
                    bool rIsOne = r.Equals(BigIntegerHttp.One);
                    while (x.BitLength > (qLen + 1))
                    {
                        BigIntegerHttp u = x.ShiftRight(qLen);
                        BigIntegerHttp v = x.Remainder(qMod);
                        if (!rIsOne)
                        {
                            u = u.Multiply(r);
                        }
                        x = u.Add(v);
                    }
                }
                else
                {
                    int d = ((qLen - 1) & 31) + 1;
                    BigIntegerHttp mu = r.Negate();
                    BigIntegerHttp u = mu.Multiply(x.ShiftRight(qLen - d));
                    BigIntegerHttp quot = u.ShiftRight(qLen + d);
                    BigIntegerHttp v = quot.Multiply(q);
                    BigIntegerHttp bk1 = BigIntegerHttp.One.ShiftLeft(qLen + d);
                    v = v.Remainder(bk1);
                    x = x.Remainder(bk1);
                    x = x.Subtract(v);
                    if (x.SignValue < 0)
                    {
                        x = x.Add(bk1);
                    }
                }
                while (x.CompareTo(q) >= 0)
                {
                    x = x.Subtract(q);
                }
                if (negative && x.SignValue != 0)
                {
                    x = q.Subtract(x);
                }
            }
            return x;
        }

        protected virtual BigIntegerHttp ModSubtract(BigIntegerHttp x1, BigIntegerHttp x2)
        {
            BigIntegerHttp x3 = x1.Subtract(x2);
            if (x3.SignValue < 0)
            {
                x3 = x3.Add(q);
            }
            return x3;
        }

        public override bool Equals(
            object obj)
        {
            if (obj == this)
                return true;

            FpFieldElement other = obj as FpFieldElement;

            if (other == null)
                return false;

            return Equals(other);
        }

        public virtual bool Equals(
            FpFieldElement other)
        {
            return q.Equals(other.q) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return q.GetHashCode() ^ base.GetHashCode();
        }
    }

    /**
     * Class representing the Elements of the finite field
     * <code>F<sub>2<sup>m</sup></sub></code> in polynomial basis (PB)
     * representation. Both trinomial (Tpb) and pentanomial (Ppb) polynomial
     * basis representations are supported. Gaussian normal basis (GNB)
     * representation is not supported.
     */
    public class F2mFieldElement
        : ECFieldElement
    {
        /**
         * Indicates gaussian normal basis representation (GNB). Number chosen
         * according to X9.62. GNB is not implemented at present.
         */
        public const int Gnb = 1;

        /**
         * Indicates trinomial basis representation (Tpb). Number chosen
         * according to X9.62.
         */
        public const int Tpb = 2;

        /**
         * Indicates pentanomial basis representation (Ppb). Number chosen
         * according to X9.62.
         */
        public const int Ppb = 3;

        /**
         * Tpb or Ppb.
         */
        private int representation;

        /**
         * The exponent <code>m</code> of <code>F<sub>2<sup>m</sup></sub></code>.
         */
        private int m;

        private int[] ks;

        /**
         * The <code>LongArray</code> holding the bits.
         */
        private LongArray x;

        /**
            * Constructor for Ppb.
            * @param m  The exponent <code>m</code> of
            * <code>F<sub>2<sup>m</sup></sub></code>.
            * @param k1 The integer <code>k1</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.
            * @param k2 The integer <code>k2</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.
            * @param k3 The integer <code>k3</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.
            * @param x The BigInteger representing the value of the field element.
            */
        public F2mFieldElement(
            int			m,
            int			k1,
            int			k2,
            int			k3,
            BigIntegerHttp	x)
        {
            if (x == null || x.SignValue < 0 || x.BitLength > m)
                throw new ArgumentException("value invalid in F2m field element", "x");

            if ((k2 == 0) && (k3 == 0))
            {
                this.representation = Tpb;
                this.ks = new int[] { k1 };
            }
            else
            {
                if (k2 >= k3)
                    throw new ArgumentException("k2 must be smaller than k3");
                if (k2 <= 0)
                    throw new ArgumentException("k2 must be larger than 0");

                this.representation = Ppb;
                this.ks = new int[] { k1, k2, k3 };
            }

            this.m = m;
            this.x = new LongArray(x);
        }

        /**
            * Constructor for Tpb.
            * @param m  The exponent <code>m</code> of
            * <code>F<sub>2<sup>m</sup></sub></code>.
            * @param k The integer <code>k</code> where <code>x<sup>m</sup> +
            * x<sup>k</sup> + 1</code> represents the reduction
            * polynomial <code>f(z)</code>.
            * @param x The BigInteger representing the value of the field element.
            */
        public F2mFieldElement(
            int			m,
            int			k,
            BigIntegerHttp	x)
            : this(m, k, 0, 0, x)
        {
            // Set k1 to k, and set k2 and k3 to 0
        }

        private F2mFieldElement(int m, int[] ks, LongArray x)
        {
            this.m = m;
            this.representation = (ks.Length == 1) ? Tpb : Ppb;
            this.ks = ks;
            this.x = x;
        }

        public override int BitLength
        {
            get { return x.Degree(); }
        }

        public override bool IsOne
        {
            get { return x.IsOne(); }
        }

        public override bool IsZero
        {
            get { return x.IsZero(); }
        }

        public override bool TestBitZero()
        {
            return x.TestBitZero();
        }

        public override BigIntegerHttp ToBigInteger()
        {
            return x.ToBigInteger();
        }

        public override string FieldName
        {
            get { return "F2m"; }
        }

        public override int FieldSize
        {
            get { return m; }
        }

        /**
        * Checks, if the ECFieldElements <code>a</code> and <code>b</code>
        * are elements of the same field <code>F<sub>2<sup>m</sup></sub></code>
        * (having the same representation).
        * @param a field element.
        * @param b field element to be compared.
        * @throws ArgumentException if <code>a</code> and <code>b</code>
        * are not elements of the same field
        * <code>F<sub>2<sup>m</sup></sub></code> (having the same
        * representation).
        */
        public static void CheckFieldElements(
            ECFieldElement	a,
            ECFieldElement	b)
        {
            if (!(a is F2mFieldElement) || !(b is F2mFieldElement))
            {
                throw new ArgumentException("Field elements are not "
                    + "both instances of F2mFieldElement");
            }

            F2mFieldElement aF2m = (F2mFieldElement)a;
            F2mFieldElement bF2m = (F2mFieldElement)b;

            if (aF2m.representation != bF2m.representation)
            {
                // Should never occur
                throw new ArgumentException("One of the F2m field elements has incorrect representation");
            }

            if ((aF2m.m != bF2m.m) || !Arrays.AreEqual(aF2m.ks, bF2m.ks))
            {
                throw new ArgumentException("Field elements are not elements of the same field F2m");
            }
        }

        public override ECFieldElement Add(
            ECFieldElement b)
        {
            // No check performed here for performance reasons. Instead the
            // elements involved are checked in ECPoint.F2m
            // checkFieldElements(this, b);
            LongArray iarrClone = this.x.Copy();
            F2mFieldElement bF2m = (F2mFieldElement)b;
            iarrClone.AddShiftedByWords(bF2m.x, 0);
            return new F2mFieldElement(m, ks, iarrClone);
        }

        public override ECFieldElement AddOne()
        {
            return new F2mFieldElement(m, ks, x.AddOne());
        }

        public override ECFieldElement Subtract(
            ECFieldElement b)
        {
            // Addition and subtraction are the same in F2m
            return Add(b);
        }

        public override ECFieldElement Multiply(
            ECFieldElement b)
        {
            // Right-to-left comb multiplication in the LongArray
            // Input: Binary polynomials a(z) and b(z) of degree at most m-1
            // Output: c(z) = a(z) * b(z) mod f(z)

            // No check performed here for performance reasons. Instead the
            // elements involved are checked in ECPoint.F2m
            // checkFieldElements(this, b);
            return new F2mFieldElement(m, ks, x.ModMultiply(((F2mFieldElement)b).x, m, ks));
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            return MultiplyPlusProduct(b, x, y);
        }

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            LongArray ax = this.x, bx = ((F2mFieldElement)b).x, xx = ((F2mFieldElement)x).x, yx = ((F2mFieldElement)y).x;

            LongArray ab = ax.Multiply(bx, m, ks);
            LongArray xy = xx.Multiply(yx, m, ks);

            if (ab == ax || ab == bx)
            {
                ab = (LongArray)ab.Copy();
            }

            ab.AddShiftedByWords(xy, 0);
            ab.Reduce(m, ks);

            return new F2mFieldElement(m, ks, ab);
        }

        public override ECFieldElement Divide(
            ECFieldElement b)
        {
            // There may be more efficient implementations
            ECFieldElement bInv = b.Invert();
            return Multiply(bInv);
        }

        public override ECFieldElement Negate()
        {
            // -x == x holds for all x in F2m
            return this;
        }

        public override ECFieldElement Square()
        {
            return new F2mFieldElement(m, ks, x.ModSquare(m, ks));
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
        {
            return SquarePlusProduct(x, y);
        }

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            LongArray ax = this.x, xx = ((F2mFieldElement)x).x, yx = ((F2mFieldElement)y).x;

            LongArray aa = ax.Square(m, ks);
            LongArray xy = xx.Multiply(yx, m, ks);

            if (aa == ax)
            {
                aa = (LongArray)aa.Copy();
            }

            aa.AddShiftedByWords(xy, 0);
            aa.Reduce(m, ks);

            return new F2mFieldElement(m, ks, aa);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            return pow < 1 ? this : new F2mFieldElement(m, ks, x.ModSquareN(pow, m, ks));
        }

        public override ECFieldElement Invert()
        {
            return new F2mFieldElement(this.m, this.ks, this.x.ModInverse(m, ks));
        }

        public override ECFieldElement Sqrt()
        {
            return (x.IsZero() || x.IsOne()) ? this : SquarePow(m - 1);
        }

        /**
            * @return the representation of the field
            * <code>F<sub>2<sup>m</sup></sub></code>, either of
            * {@link F2mFieldElement.Tpb} (trinomial
            * basis representation) or
            * {@link F2mFieldElement.Ppb} (pentanomial
            * basis representation).
            */
        public int Representation
        {
            get { return this.representation; }
        }

        /**
            * @return the degree <code>m</code> of the reduction polynomial
            * <code>f(z)</code>.
            */
        public int M
        {
            get { return this.m; }
        }

        /**
            * @return Tpb: The integer <code>k</code> where <code>x<sup>m</sup> +
            * x<sup>k</sup> + 1</code> represents the reduction polynomial
            * <code>f(z)</code>.<br/>
            * Ppb: The integer <code>k1</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.<br/>
            */
        public int K1
        {
            get { return this.ks[0]; }
        }

        /**
            * @return Tpb: Always returns <code>0</code><br/>
            * Ppb: The integer <code>k2</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.<br/>
            */
        public int K2
        {
            get { return this.ks.Length >= 2 ? this.ks[1] : 0; }
        }

        /**
            * @return Tpb: Always set to <code>0</code><br/>
            * Ppb: The integer <code>k3</code> where <code>x<sup>m</sup> +
            * x<sup>k3</sup> + x<sup>k2</sup> + x<sup>k1</sup> + 1</code>
            * represents the reduction polynomial <code>f(z)</code>.<br/>
            */
        public int K3
        {
            get { return this.ks.Length >= 3 ? this.ks[2] : 0; }
        }

        public override bool Equals(
            object obj)
        {
            if (obj == this)
                return true;

            F2mFieldElement other = obj as F2mFieldElement;

            if (other == null)
                return false;

            return Equals(other);
        }

        public virtual bool Equals(
            F2mFieldElement other)
        {
            return ((this.m == other.m)
                && (this.representation == other.representation)
                && Arrays.AreEqual(this.ks, other.ks)
                && (this.x.Equals(other.x)));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ m ^ Arrays.GetHashCode(ks);
        }
    }
}

#endif
