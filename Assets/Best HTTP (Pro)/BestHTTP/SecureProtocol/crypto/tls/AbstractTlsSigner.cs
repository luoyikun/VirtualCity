#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsSigner
        :   TlsSigner
    {
        protected TlsContext mContext;

        public virtual void Init(TlsContext context)
        {
            this.mContext = context;
        }

        public virtual byte[] GenerateRawSignature(AsymmetricKeyParameterHttp privateKey, byte[] md5AndSha1)
        {
            return GenerateRawSignature(null, privateKey, md5AndSha1);
        }

        public abstract byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm,
            AsymmetricKeyParameterHttp privateKey, byte[] hash);

        public virtual bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameterHttp publicKey, byte[] md5AndSha1)
        {
            return VerifyRawSignature(null, sigBytes, publicKey, md5AndSha1);
        }

        public abstract bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes,
            AsymmetricKeyParameterHttp publicKey, byte[] hash);

        public virtual ISigner CreateSigner(AsymmetricKeyParameterHttp privateKey)
        {
            return CreateSigner(null, privateKey);
        }

        public abstract ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameterHttp privateKey);

        public virtual ISigner CreateVerifyer(AsymmetricKeyParameterHttp publicKey)
        {
            return CreateVerifyer(null, publicKey);
        }

        public abstract ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameterHttp publicKey);

        public abstract bool IsValidPublicKey(AsymmetricKeyParameterHttp publicKey);
    }
}

#endif
