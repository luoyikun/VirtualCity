#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    class DHKeyGeneratorHelper
    {
        internal static readonly DHKeyGeneratorHelper Instance = new DHKeyGeneratorHelper();

        private DHKeyGeneratorHelper()
        {
        }

        internal BigIntegerHttp CalculatePrivate(
            DHParameters	dhParams,
            SecureRandom	random)
        {
            int limit = dhParams.L;

            if (limit != 0)
            {
                int minWeight = limit >> 2;
                for (;;)
                {
                    BigIntegerHttp x = new BigIntegerHttp(limit, random).SetBit(limit - 1);
                    if (WNafUtilities.GetNafWeight(x) >= minWeight)
                    {
                        return x;
                    }
                }
            }

            BigIntegerHttp min = BigIntegerHttp.Two;
            int m = dhParams.M;
            if (m != 0)
            {
                min = BigIntegerHttp.One.ShiftLeft(m - 1);
            }

            BigIntegerHttp q = dhParams.Q;
            if (q == null)
            {
                q = dhParams.P;
            }
            BigIntegerHttp max = q.Subtract(BigIntegerHttp.Two);

            {
                int minWeight = max.BitLength >> 2;
                for (;;)
                {
                    BigIntegerHttp x = BigIntegers.CreateRandomInRange(min, max, random);
                    if (WNafUtilities.GetNafWeight(x) >= minWeight)
                    {
                        return x;
                    }
                }
            }
        }

        internal BigIntegerHttp CalculatePublic(
            DHParameters	dhParams,
            BigIntegerHttp		x)
        {
            return dhParams.G.ModPow(x, dhParams.P);
        }
    }
}

#endif
