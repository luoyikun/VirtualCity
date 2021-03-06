#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.IO;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ServerDHParams
    {
        protected readonly DHPublicKeyParameters mPublicKey;

        public ServerDHParams(DHPublicKeyParameters publicKey)
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");

            this.mPublicKey = publicKey;
        }

        public virtual DHPublicKeyParameters PublicKey
        {
            get { return mPublicKey; }
        }

        /**
         * Encode this {@link ServerDHParams} to a {@link Stream}.
         * 
         * @param output
         *            the {@link Stream} to encode to.
         * @throws IOException
         */
        public virtual void Encode(Stream output)
        {
            DHParameters dhParameters = mPublicKey.Parameters;
            BigIntegerHttp Ys = mPublicKey.Y;

            TlsDHUtilities.WriteDHParameter(dhParameters.P, output);
            TlsDHUtilities.WriteDHParameter(dhParameters.G, output);
            TlsDHUtilities.WriteDHParameter(Ys, output);
        }

        /**
         * Parse a {@link ServerDHParams} from a {@link Stream}.
         * 
         * @param input
         *            the {@link Stream} to parse from.
         * @return a {@link ServerDHParams} object.
         * @throws IOException
         */
        public static ServerDHParams Parse(Stream input)
        {
            BigIntegerHttp p = TlsDHUtilities.ReadDHParameter(input);
            BigIntegerHttp g = TlsDHUtilities.ReadDHParameter(input);
            BigIntegerHttp Ys = TlsDHUtilities.ReadDHParameter(input);

            return new ServerDHParams(
                TlsDHUtilities.ValidateDHPublicKey(new DHPublicKeyParameters(Ys, new DHParameters(p, g))));
        }
    }
}

#endif
