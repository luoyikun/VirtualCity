using UnityEngine;

namespace Lean.Pool
{
	// This component allows you to reset a Rigidbody's velocity via Messages or via Poolable
	[RequireComponent(typeof(Rigidbody))]
	public class LeanPooledRigidbody : MonoBehaviour
	{
		public void ResetVelocity()
		{
			var rigidbody = GetComponent<Rigidbody>();

			rigidbody.velocity        = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}

		protected virtual void OnDespawn()
		{
			ResetVelocity();
		}
	}
}