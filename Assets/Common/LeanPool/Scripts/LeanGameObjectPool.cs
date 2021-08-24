using UnityEngine;
using System.Collections.Generic;

namespace Lean.Pool
{
	// This component allows you to pool GameObjects, allowing for a very fast instantiate/destroy alternative
	public class LeanGameObjectPool : MonoBehaviour
	{
		[System.Serializable]
		public class Clone
		{
			public GameObject   GameObject;
			public Transform    Transform;
			public LeanPoolable Poolable;
		}

		[System.Serializable]
		public class Delay
		{
			public GameObject GameObject;
			public float      Life;
		}

		public enum NotificationType
		{
			None,
			SendMessage,
			BroadcastMessage,
			PoolableEvent
		}

		// All activle and enabled pools in the scene
		public static List<LeanGameObjectPool> Instances = new List<LeanGameObjectPool>();

		[Tooltip("The prefab this pool controls")]
		public GameObject Prefab;

		[Tooltip("Should this pool send messages to the clones when they're spawned/despawned?")]
		public NotificationType Notification = NotificationType.SendMessage;

		[Tooltip("Should this pool preload some clones?")]
		public int Preload;

		[Tooltip("Should this pool have a maximum amount of spawnable clones?")]
		public int Capacity;

		[Tooltip("If the pool reaches capacity, should new spawns force older ones to despawn?")]
		public bool Recycle;

		[Tooltip("Should this pool be marked as DontDestroyOnLoad?")]
		public bool Persist;

		[Tooltip("Should the spawned cloned have the clone index appended to their name?")]
		public bool Stamp;

		[Tooltip("Should detected issues be output to the console?")]
		public bool Warnings = true;

		// All the currently spawned prefab instances
		[SerializeField]
		private List<Clone> spawnedClones;

		// All the currently despawned prefab instances
		[SerializeField]
		private List<Clone> despawnedClones;

		// All the delayed destruction objects
		[SerializeField]
		private List<Delay> delays;

		// Find the pool responsible for handling the specified prefab
		public static LeanGameObjectPool FindPoolByPrefab(GameObject prefab)
		{
			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var pool = Instances[i];

				if (pool.Prefab == prefab)
				{
					return pool;
				}
			}

			return null;
		}

		public static LeanGameObjectPool FindPoolByClone(GameObject gameObject)
		{
			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var pool = Instances[i];

				if (pool.spawnedClones != null)
				{
					for (var j = pool.spawnedClones.Count - 1; j >= 0; j--)
					{
						var clone = pool.spawnedClones[j];

						if (clone.GameObject == gameObject)
						{
							return pool;
						}
					}
				}
			}

			return null;
		}

		// Returns the amount of spawned clones
		public int Spawned
		{
			get
			{
				return spawnedClones != null ? spawnedClones.Count : 0;
			}
		}

		// Returns the amount of despawned clones
		public int Despawned
		{
			get
			{
				return despawnedClones != null ? despawnedClones.Count : 0;
			}
		}

		// Returns the total amount of spawned and despawned clones
		public int Total
		{
			get
			{
				return Spawned + Despawned;
			}
		}

		// This will either spawn a previously despanwed/preloaded clone, recycle one, create a new one, or return null
		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			if (Prefab != null)
			{
				// Spawn a previously despanwed/preloaded clone?
				if (despawnedClones != null)
				{
					// Loop through all despawnedClones until one is found
					while (despawnedClones.Count > 0)
					{
						var index = despawnedClones.Count - 1;
						var clone = despawnedClones[index];
						
						despawnedClones.RemoveAt(index);

						if (clone.GameObject != null)
						{
							SpawnClone(clone, position, rotation, parent);

							return clone.GameObject;
						}

						if (Warnings == true) Debug.LogWarning("This pool contained a null despawned clone, did you accidentally destroy it?", this);
					}
				}

				// Make a new clone?
				if (Capacity <= 0 || Total < Capacity)
				{
					var clone = CreateClone(position, rotation, parent);

					// Add clone to spawned list
					if (spawnedClones == null)
					{
						spawnedClones = new List<Clone>();
					}

					spawnedClones.Add(clone);

					// Messages?
					InvokeOnSpawn(clone);

					return clone.GameObject;
				}

				// Recycle?
				if (Recycle == true && spawnedClones != null)
				{
					// Loop through all spawnedClones from the front (oldest) until one is found
					while (spawnedClones.Count > 0)
					{
						var clone = spawnedClones[0];

						spawnedClones.RemoveAt(0);

						if (clone != null)
						{
							InvokeOnDespawn(clone);

							clone.GameObject.SetActive(false);

							SpawnClone(clone, position, rotation, parent);

							return clone.GameObject;
						}

						if (Warnings == true) Debug.LogWarning("This pool contained a null spawned clone, did you accidentally destroy it?", this);
					}
				}
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("You're attempting to spawn from a pool with a null prefab", this);
			}

			return null;
		}

		// This allows you to access the spawned clone at the specified index
		public GameObject GetSpawned(int index)
		{
			return index >= 0 && index < spawnedClones.Count ? spawnedClones[index].GameObject : null;
		}

		// This allows you to access the despawned clone at the specified index
		public GameObject GetDespawned(int index)
		{
			return index >= 0 && index < despawnedClones.Count ? despawnedClones[index].GameObject : null;
		}

		[ContextMenu("Despawn All")]
		public void DespawnAll()
		{
			for (var i = spawnedClones.Count - 1; i >= 0; i--)
			{
				DespawnNow(spawnedClones[i].GameObject);
			}
		}

		// This will either instantly despawn the specified gameObject, or delay despawn it after t seconds
		public void Despawn(GameObject cloneGameObject, float t = 0.0f)
		{
			if (cloneGameObject != null)
			{
				// Delay the despawn?
				if (t > 0.0f)
				{
					DespawnWithDelay(cloneGameObject, t);
				}
				// Despawn now?
				else
				{
					DespawnNow(cloneGameObject);
				}
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("You're attempting to despawn a null gameObject", this);
			}
		}

		// This method will create an additional prefab clone and add it to the despawned list
		[ContextMenu("Preload One More")]
		public void PreloadOneMore()
		{
			if (Prefab != null)
			{
				// Create clone
				var clone = CreateClone(Vector3.zero, Quaternion.identity, null);

				// Add clone to despawned list
				if (despawnedClones == null)
				{
					despawnedClones = new List<Clone>();
				}

				despawnedClones.Add(clone);

				// Deactivate it
				clone.GameObject.SetActive(false);

				// Move it under this GO
				clone.Transform.SetParent(transform, false);

				if (Warnings == true && Capacity > 0 && Total > Capacity) Debug.LogWarning("You've preloaded more than the pool capacity, please verify you're preloading the intended amount", this);
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("Attempting to preload a null prefab", this);
			}
		}

		// This will preload the pool until the 
		[ContextMenu("Preload All")]
		public void PreloadAll()
		{
			if (Preload > 0)
			{
				if (Prefab != null)
				{
					for (var i = Total; i < Preload; i++)
					{
						PreloadOneMore();
					}
				}
				else if (Warnings == true)
				{
					if (Warnings == true) Debug.LogWarning("Attempting to preload a null prefab", this);
				}
			}
		}

		protected virtual void Awake()
		{
			PreloadAll();

			if (Persist == true)
			{
				DontDestroyOnLoad(this);
			}
		}

		protected virtual void OnEnable()
		{
			Instances.Add(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(this);
		}

		protected virtual void Update()
		{
			// Decay the life of all delayed destruction calls
			if (delays != null)
			{
				for (var i = delays.Count - 1; i >= 0; i--)
				{
					var delay = delays[i];

					delay.Life -= Time.deltaTime;

					// Skip to next one?
					if (delay.Life > 0.0f)
					{
						continue;
					}

					// Finally despawn it after delay
					if (delay.GameObject != null)
					{
						Despawn(delay.GameObject);
					}
					else
					{
						if (Warnings == true) Debug.LogWarning("Attempting to update the delayed destruction of a prefab clone that no longer exists, did you accidentally delete it?", this);
					}

					// Remove and pool delay
					delays.RemoveAt(i); LeanClassPool<Delay>.Despawn(delay);
				}
			}
		}

		private void DespawnWithDelay(GameObject cloneGameObject, float t)
		{
			// If this object is already marked for delayed despawn, ignore
			if (delays != null)
			{
				for (var i = delays.Count - 1; i >= 0; i--)
				{
					var delay = delays[i];

					if (delay.GameObject == cloneGameObject)
					{
						if (Warnings == true) Debug.LogWarning("You're attempting to delay despawn a gameObject that has already been marked for delay despawn", cloneGameObject);

						return;
					}
				}
			}
			else
			{
				delays = new List<Delay>();
			}
					
			// Create delay
			var newDelay = LeanClassPool<Delay>.Spawn() ?? new Delay();

			newDelay.GameObject = cloneGameObject;
			newDelay.Life       = t;

			delays.Add(newDelay);
		}

		private void DespawnNow(GameObject cloneGameObject)
		{
			// Find the clone associated with this gameObject
			if (spawnedClones != null)
			{
				for (var i = spawnedClones.Count - 1; i >= 0; i--)
				{
					var clone = spawnedClones[i];

					if (clone.GameObject == cloneGameObject)
					{
						// Remove clone from spawned list
						spawnedClones.RemoveAt(i);

						// Add clone to despawned list
						if (despawnedClones == null)
						{
							despawnedClones = new List<Clone>();
						}

						despawnedClones.Add(clone);

						// Messages?
						InvokeOnDespawn(clone);

						// Deactivate it
						clone.GameObject.SetActive(false);

						// Move it under this GO
						clone.Transform.SetParent(transform, false);

						return;
					}
				}
			}

			if (Warnings == true) Debug.LogWarning("You're attempting to despawn a gameObject that wasn't spawned from this pool, make sure your Spawn and Despawn calls match", cloneGameObject);
		}

		private Clone CreateClone(Vector3 position, Quaternion rotation, Transform parent)
		{
			var clone = new Clone();

			clone.GameObject = (GameObject)Instantiate(Prefab, position, rotation);
			clone.Transform  = clone.GameObject.transform;

			if (Stamp == true)
			{
				clone.GameObject.name = Prefab.name + " " + Total;
			}
			else
			{
				clone.GameObject.name = Prefab.name;
			}

			clone.Transform.SetParent(parent, false);

			return clone;
		}

		private void SpawnClone(Clone clone, Vector3 position, Quaternion rotation, Transform parent)
		{
			// Add clone to spawned list
			if (spawnedClones == null)
			{
				spawnedClones = new List<Clone>();
			}

			spawnedClones.Add(clone);

			// Update transform of clone
			var cloneTransform = clone.Transform;

			cloneTransform.localPosition = position;
			cloneTransform.localRotation = rotation;

			cloneTransform.SetParent(parent, false);

			// Activate clone
			clone.GameObject.SetActive(true);

			// Notifications
			InvokeOnSpawn(clone);
		}

		private static LeanPoolable GetOrCachePoolable(Clone clone)
		{
			var poolable = clone.Poolable;

			if (clone.Poolable == null)
			{
				poolable = clone.Poolable = clone.GameObject.GetComponent<LeanPoolable>();
			}

			return poolable;
		}

		private void InvokeOnSpawn(Clone clone)
		{
			switch (Notification)
			{
				case NotificationType.SendMessage: clone.GameObject.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.BroadcastMessage: clone.GameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.PoolableEvent: var poolable = GetOrCachePoolable(clone); if (poolable != null && poolable.OnSpawn != null) poolable.OnSpawn.Invoke(); break;
			}
		}

		private void InvokeOnDespawn(Clone clone)
		{
			switch (Notification)
			{
				case NotificationType.SendMessage: clone.GameObject.SendMessage("OnDespawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.BroadcastMessage: clone.GameObject.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.PoolableEvent: var poolable = GetOrCachePoolable(clone); if (poolable != null && poolable.OnDespawn != null) poolable.OnDespawn.Invoke(); break;
			}
		}
	}
}