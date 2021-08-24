using UnityEngine;
using UnityEngine.Events;

namespace Lean.Pool
{
	// This component will automatically reset a Rigidbody when it gets spawned/despawned
	public class LeanPoolable : MonoBehaviour
	{
		// Called when this poolable object is spawned
		public UnityEvent OnSpawn;

		// Called when this poolable object is despawned
		public UnityEvent OnDespawn;
	}
}