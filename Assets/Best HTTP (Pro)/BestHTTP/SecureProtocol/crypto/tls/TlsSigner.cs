#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsSigner
    {
        void Init(TlsContext context);

        byte[] GenerateRawSignature(AsymmetricKeyParameterHttp privateKey, byte[] md5AndSha1);

        byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm,
            AsymmetricKeyParameterHttp privateKey, byte[] hash);

        bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameterHttp publicKey, byte[] md5AndSha1);

        bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes,
            AsymmetricKeyParameterHttp publicKey, byte[] hash);

        ISigner CreateSigner(AsymmetricKeyParameterHttp privateKey);

        ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameterHttp privateKey);

        ISigner CreateVerifyer(AsymmetricKeyParameterHttp publicKey);

        ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameterHttp publicKey);

        bool IsValidPublicKey(AsymmetricKeyParameterHttp publicKey);
    }
}

#endif
