#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Math.Field
{
    public interface IFiniteField
    {
        BigIntegerHttp Characteristic { get; }

        int Dimension { get; }
    }
}

#endif
