#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    internal class DHParametersHelper
    {
        private static readonly BigIntegerHttp Six = BigIntegerHttp.ValueOf(6);

        private static readonly int[][] primeLists = BigIntegerHttp.primeLists;
        private static readonly int[] primeProducts = BigIntegerHttp.primeProducts;
        private static readonly BigIntegerHttp[] BigPrimeProducts = ConstructBigPrimeProducts(primeProducts);

        private static BigIntegerHttp[] ConstructBigPrimeProducts(int[] primeProducts)
        {
            BigIntegerHttp[] bpp = new BigIntegerHttp[primeProducts.Length];
            for (int i = 0; i < bpp.Length; ++i)
            {
                bpp[i] = BigIntegerHttp.ValueOf(primeProducts[i]);
            }
            return bpp;
        }

        /*
         * Finds a pair of prime BigInteger's {p, q: p = 2q + 1}
         * 
         * (see: Handbook of Applied Cryptography 4.86)
         */
        internal static BigIntegerHttp[] GenerateSafePrimes(int size, int certainty, SecureRandom random)
        {
            BigIntegerHttp p, q;
            int qLength = size - 1;
            int minWeight = size >> 2;

            if (size <= 32)
            {
                for (;;)
                {
                    q = new BigIntegerHttp(qLength, 2, random);

                    p = q.ShiftLeft(1).Add(BigIntegerHttp.One);

                    if (!p.IsProbablePrime(certainty, true))
                        continue;

                    if (certainty > 2 && !q.IsProbablePrime(certainty, true))
                        continue;

                    break;
                }
            }
            else
            {
                // Note: Modified from Java version for speed
                for (;;)
                {
                    q = new BigIntegerHttp(qLength, 0, random);

                retry:
                    for (int i = 0; i < primeLists.Length; ++i)
                    {
                        int test = q.Remainder(BigPrimeProducts[i]).IntValue;

                        if (i == 0)
                        {
                            int rem3 = test % 3;
                            if (rem3 != 2)
                            {
                                int diff = 2 * rem3 + 2;
                                q = q.Add(BigIntegerHttp.ValueOf(diff));
                                test = (test + diff) % primeProducts[i];
                            }
                        }

                        int[] primeList = primeLists[i];
                        for (int j = 0; j < primeList.Length; ++j)
                        {
                            int prime = primeList[j];
                            int qRem = test % prime;
                            if (qRem == 0 || qRem == (prime >> 1))
                            {
                                q = q.Add(Six);
                                goto retry;
                            }
                        }
                    }

                    if (q.BitLength != qLength)
                        continue;

                    if (!q.RabinMillerTest(2, random, true))
                        continue;

                    p = q.ShiftLeft(1).Add(BigIntegerHttp.One);

                    if (!p.RabinMillerTest(certainty, random, true))
                        continue;

                    if (certainty > 2 && !q.RabinMillerTest(certainty - 2, random, true))
                        continue;

                    /*
                     * Require a minimum weight of the NAF representation, since low-weight primes may be
                     * weak against a version of the number-field-sieve for the discrete-logarithm-problem.
                     * 
                     * See "The number field sieve for integers of low weight", Oliver Schirokauer.
                     */
                    if (WNafUtilities.GetNafWeight(p) < minWeight)
                        continue;

                    break;
                }
            }

            return new BigIntegerHttp[] { p, q };
        }

        /*
         * Select a high order element of the multiplicative group Zp*
         * 
         * p and q must be s.t. p = 2*q + 1, where p and q are prime (see generateSafePrimes)
         */
        internal static BigIntegerHttp SelectGenerator(BigIntegerHttp p, BigIntegerHttp q, SecureRandom random)
        {
            BigIntegerHttp pMinusTwo = p.Subtract(BigIntegerHttp.Two);
            BigIntegerHttp g;

            /*
             * (see: Handbook of Applied Cryptography 4.80)
             */
//			do
//			{
//				g = BigIntegers.CreateRandomInRange(BigInteger.Two, pMinusTwo, random);
//			}
//			while (g.ModPow(BigInteger.Two, p).Equals(BigInteger.One)
//				|| g.ModPow(q, p).Equals(BigInteger.One));

            /*
             * RFC 2631 2.2.1.2 (and see: Handbook of Applied Cryptography 4.81)
             */
            do
            {
                BigIntegerHttp h = BigIntegers.CreateRandomInRange(BigIntegerHttp.Two, pMinusTwo, random);

                g = h.ModPow(BigIntegerHttp.Two, p);
            }
            while (g.Equals(BigIntegerHttp.One));

            return g;
        }
    }
}

#endif
