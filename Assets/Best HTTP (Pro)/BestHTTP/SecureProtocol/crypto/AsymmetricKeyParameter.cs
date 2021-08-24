#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.Crypto
{
    public abstract class AsymmetricKeyParameterHttp
		: ICipherParameters
    {
        private readonly bool privateKey;

        protected AsymmetricKeyParameterHttp(
            bool privateKey)
        {
            this.privateKey = privateKey;
        }

		public bool IsPrivate
        {
            get { return privateKey; }
        }

		public override bool Equals(
			object obj)
		{
			AsymmetricKeyParameterHttp other = obj as AsymmetricKeyParameterHttp;

			if (other == null)
			{
				return false;
			}

			return Equals(other);
		}

		protected bool Equals(
			AsymmetricKeyParameterHttp other)
		{
			return privateKey == other.privateKey;
		}

		public override int GetHashCode()
		{
			return privateKey.GetHashCode();
		}
    }
}

#endif
