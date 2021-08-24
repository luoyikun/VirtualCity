using UnityEngine;
using System.Collections.Generic;

namespace Lean.Pool
{
	public static class LeanPool
	{
		// The reference between a spawned GameObject and its pool
		public static Dictionary<GameObject, LeanGameObjectPool> Links = new Dictionary<GameObject, LeanGameObjectPool>();

		// These methods allows you to spawn prefabs via Component with varying levels of transform data
		public static T Spawn<T>(T prefab)
			where T : Component
		{
			return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation)
			where T : Component
		{
			return Spawn(prefab, position, rotation, null);
		}

		public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent)
			where T : Component
		{
			// Clone this prefabs's GameObject
			var gameObject = prefab != null ? prefab.gameObject : null;
			var clone      = Spawn(gameObject, position, rotation, parent);

			// Return the same component from the clone
			return clone != null ? clone.GetComponent<T>() : null;
		}

		// These methods allows you to spawn prefabs via GameObject with varying levels of transform data
		public static GameObject Spawn(GameObject prefab)
		{
			return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return Spawn(prefab, position, rotation, null);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			if (prefab != null)
			{
				// Find the pool that handles this prefab
				var pool = LeanGameObjectPool.FindPoolByPrefab(prefab);

				// Create a new pool for this prefab?
				if (pool == null)
				{
					pool = new GameObject("LeanPool (" + prefab.name + ")").AddComponent<LeanGameObjectPool>();
                    //if (poolPar != null)
                    //{
                    //    pool.transform.parent = poolPar;
                    //}
					pool.Prefab = prefab;
				}

				// Try and spawn a clone from this pool
				var clone = pool.Spawn(position, rotation, parent);

				if (clone != null)
				{
					// If this clone was recycled, recycle the link too
					if (pool.Recycle == true && pool.Spawned >= pool.Capacity)
					{
						var existingPool = default(LeanGameObjectPool);

						if (Links.TryGetValue(clone, out existingPool) == true)
						{
							if (existingPool != pool)
							{
								Links.Remove(clone);
							}
							else
							{
								return clone.gameObject;
							}
						}
					}

					// Associate this clone with this pool
					Links.Add(clone, pool);

					return clone.gameObject;
				}
			}
			else
			{
				Debug.LogError("Attempting to spawn a null prefab");
			}

			return null;
		}

        public static GameObject SpawnObj(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,Transform poolPar = null)
        {
            if (prefab != null)
            {
                // Find the pool that handles this prefab
                var pool = LeanGameObjectPool.FindPoolByPrefab(prefab);

                // Create a new pool for this prefab?
                if (pool == null)
                {
                    pool = new GameObject("LeanPool (" + prefab.name + ")").AddComponent<LeanGameObjectPool>();
                    if (poolPar != null)
                    {
                        pool.transform.parent = poolPar;
                    }
                    pool.Prefab = prefab;
                }

                // Try and spawn a clone from this pool
                var clone = pool.Spawn(position, rotation, parent);

                if (clone != null)
                {
                    // If this clone was recycled, recycle the link too
                    if (pool.Recycle == true && pool.Spawned >= pool.Capacity)
                    {
                        var existingPool = default(LeanGameObjectPool);

                        if (Links.TryGetValue(clone, out existingPool) == true)
                        {
                            if (existingPool != pool)
                            {
                                Links.Remove(clone);
                            }
                            else
                            {
                                return clone.gameObject;
                            }
                        }
                    }

                    // Associate this clone with this pool
                    Links.Add(clone, pool);

                    return clone.gameObject;
                }
            }
            else
            {
                Debug.LogError("Attempting to spawn a null prefab");
            }

            return null;
        }

        // This will despawn all pool clones
        public static void DespawnAll()
		{
			for (var i = LeanGameObjectPool.Instances.Count - 1; i >= 0; i--)
			{
				LeanGameObjectPool.Instances[i].DespawnAll();
			}

			Links.Clear();
		}

		// This allows you to despawn a clone via Component, with optional delay
		public static void Despawn(Component clone, float delay = 0.0f)
		{
			if (clone != null) Despawn(clone.gameObject, delay);
		}

		// This allows you to despawn a clone via GameObject, with optional delay
		public static void Despawn(GameObject clone, float delay = 0.0f)
		{
			if (clone != null)
			{
				var pool = default(LeanGameObjectPool);

				// Try and find the pool associated with this clone
				if (Links.TryGetValue(clone, out pool) == true)
				{
					// Remove the association
					Links.Remove(clone);

					pool.Despawn(clone, delay);
				}
				else
				{
					pool = LeanGameObjectPool.FindPoolByClone(clone);

					if (pool != null)
					{
						pool.Despawn(clone, delay);
					}
					else
					{
						Debug.LogWarning("You're attempting to despawn a gameObject that wasn't spawned from this pool", clone);

						// Fall back to normal destroying
						Object.Destroy(clone);
					}
				}
			}
			else
			{
				Debug.LogWarning("You're attempting to despawn a null gameObject", clone);
			}
		}
	}
}