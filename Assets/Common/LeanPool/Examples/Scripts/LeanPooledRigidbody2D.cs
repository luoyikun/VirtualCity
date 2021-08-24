using UnityEngine;

namespace Lean.Pool
{
	// This component will automatically reset a Rigidbody2D when it gets spawned/despawned
	[RequireComponent(typeof(Rigidbody2D))]
	public class LeanPooledRigidbody2D : MonoBehaviour
	{
		public void ResetVelocity()
		{
			var rigidbody2D = GetComponent<Rigidbody2D>();

			rigidbody2D.velocity        = Vector2.zero;
			rigidbody2D.angularVelocity = 0.0f;
		}

		protected virtual void OnDespawn()
		{
			ResetVelocity();
		}
	}
}