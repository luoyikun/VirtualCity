#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Crypto
{
    /**
     * a holding class for public/private parameter pairs.
     */
    public class AsymmetricCipherKeyPairHttp
    {
        private readonly AsymmetricKeyParameterHttp publicParameter;
        private readonly AsymmetricKeyParameterHttp privateParameter;

		/**
         * basic constructor.
         *
         * @param publicParam a public key parameters object.
         * @param privateParam the corresponding private key parameters.
         */
        public AsymmetricCipherKeyPairHttp(
            AsymmetricKeyParameterHttp    publicParameter,
            AsymmetricKeyParameterHttp    privateParameter)
        {
			if (publicParameter.IsPrivate)
				throw new ArgumentException("Expected a public key", "publicParameter");
			if (!privateParameter.IsPrivate)
				throw new ArgumentException("Expected a private key", "privateParameter");

			this.publicParameter = publicParameter;
            this.privateParameter = privateParameter;
        }

		/**
         * return the public key parameters.
         *
         * @return the public key parameters.
         */
        public AsymmetricKeyParameterHttp Public
        {
            get { return publicParameter; }
        }

		/**
         * return the private key parameters.
         *
         * @return the private key parameters.
         */
        public AsymmetricKeyParameterHttp Private
        {
            get { return privateParameter; }
        }
    }
}

#endif
