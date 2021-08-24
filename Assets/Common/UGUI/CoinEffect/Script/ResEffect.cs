using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResEffect : MonoBehaviour
{
	
	/// <summary>
    /// 生成所有数量的资源图标 间隔总时间
    /// </summary>
	public float generateTime = 1f;
	/// <summary>
    /// 扩散半径-生成资源图标后 扩散  形成不一样的轨迹，值的意义是起点到终点距离的百分比
    /// </summary>
	public float explosionRadius = 20;
	/// <summary>
    /// 飞行速度
    /// </summary>
	public float moveSpeed = 20;
	/// <summary>
    /// 旋转速度
    /// </summary>
	public float rotateSpeed = 3;



	/// <summary>
	/// 资源图标 prefab
	/// </summary>
	private GameObject[] ResPrefab;
	/// <summary>
    /// 资源图标对象池gameObject
    /// </summary>
	private Transform gameObjectPool;
	/// <summary>
    /// 资源图标对象池
    /// </summary>
	private Dictionary<int,List<Coin>> objectPool;


	private class Coin
	{
		public int type;
		public float mMoveTime;
		public Transform mTransform;
		public Vector3 rotateSpeed;
	}

	void Destroy()
	{
		if (objectPool == null) {
			return;
		}

		foreach (var coinListInfo in objectPool) {
			for (int i = coinListInfo.Value.Count - 1; i >= 0; i--) {
				Destroy (coinListInfo.Value [i].mTransform.gameObject);
				coinListInfo.Value [i].mTransform = null;
			}
		}

		objectPool = null;
		ResPrefab = null;
	}

	private void ReleaseACoin(Coin coin)
	{
		if (coin != null) {
			coin.mTransform.localPosition = Vector3.zero;
			coin.mMoveTime = 0f;
			coin.mTransform.SetParent (gameObjectPool);
			objectPool [coin.type].Add (coin);
		}

	}

	private Coin GetACoin(int type, Vector3 sourceIn)
	{
		if (objectPool == null) {
			objectPool = new Dictionary<int, List<Coin>>();
		}

		if (!objectPool.ContainsKey(type)) {
			objectPool.Add (type, new List<Coin>());
		}

		Coin coin;
		if (objectPool[type].Count == 0) {
			coin = InstantCoin (type, sourceIn);
		} else {
			coin = objectPool[type] [0];
			objectPool[type].Remove (coin);
		}
		coin.mTransform.position = sourceIn;
		coin.mTransform.SetParent (transform);
		coin.mTransform.localScale = Vector3.one;

		return coin;
	}

	private Coin InstantCoin(int type, Vector3 sourceIn)
	{
		Coin coin = new Coin();

		coin.type = type;
		coin.rotateSpeed = new Vector3(
			Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
			Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
			Mathf.Lerp(0, 360, Random.Range(0, 1.0f))) * rotateSpeed;

		GameObject go = Instantiate(ResPrefab[type], sourceIn, Random.rotationUniform) as GameObject;
		go.SetActive(true);
		coin.mTransform = go.transform;

		return coin;
	}

	private IEnumerator OnAnimation(float flyTime, int type, Vector3 sourceIn, Vector3 targetIn, int count,
		float rate, float radius, System.Action<int> onFinish)
	{
		/// <summary>
	    /// 控制点-生成资源图标后 扩散  形成不一样的轨迹
	    /// </summary>
		List<Vector3> ctrlPoints = new List<Vector3>();
		List<Coin> coins = new List<Coin>();
		float _generateTime = 0;
		float _generateCount = 0;
		float _moveSpeed = 1 / flyTime;
		bool isArriveFirst = false;

		while (true)
		{
			if (_generateCount < count)
			{
				_generateTime += Time.deltaTime;
				for (int i = 0; i < Mathf.Ceil(_generateTime / rate); i++) {
					if (_generateCount < count)
					{
						Coin coin = GetACoin(type, sourceIn);
						ctrlPoints.Add(Random.insideUnitSphere * radius);
						coins.Add(coin);
						_generateCount++;
						_generateTime -= rate;
					}
					else
					{
						break;
					}
				}
			}

				

			for (int i = coins.Count - 1; i >= 0; --i)
			{
				Coin coin = coins[i];
				coin.mTransform.position = Vector3.Lerp(Vector3.Lerp(sourceIn, sourceIn + ctrlPoints[i], coins[i].mMoveTime * 5), targetIn, coins[i].mMoveTime);
				coin.mTransform.Rotate(coin.rotateSpeed.x * Time.deltaTime, coin.rotateSpeed.y * Time.deltaTime, coin.rotateSpeed.z * Time.deltaTime);
				coin.mMoveTime = coin.mMoveTime + Mathf.Lerp(0.1f,1f,coin.mMoveTime) * _moveSpeed * Time.deltaTime;

				if (coin.mMoveTime >= 1)
				{
					if (isArriveFirst == false && onFinish != null)
			        {
			        	isArriveFirst = true;
			            onFinish(0);
			        }
					ReleaseACoin (coin);
					coins.RemoveAt(i);
					ctrlPoints.RemoveAt(i);
				}
			}

			if (_generateCount >= count && coins.Count == 0)
			{
				break;
			}

			yield return null;
		}

		if (onFinish != null)
        {
            onFinish(1);
        }
        else
        {
        	Debug.Log("onFinish onFinish onFinish == null");
        }
	}

	/// <summary>
	/// 播放特效
	/// </summary>
	/// <param name="type">类型</param>
	/// <param name="source">起点坐标-世界坐标</param>
	/// <param name="target">终点坐标-世界坐标</param>
	/// <param name="count">粒子数量</param>
	/// <param name="onFinish">完成后回调</param>
	public void Play(int type, Vector3 source, Vector3 target, int count, System.Action<int> onFinish)
	{
		Debug.Log("播放特效 Play count="+count);
		/// <summary>
	    /// 生成所有一个资源图标 所耗时间
	    /// </summary> 
		float rate = generateTime / count; 
		/// <summary>
	    /// 扩散半径
	    /// </summary>
		float radius = (target - source).magnitude * explosionRadius / 100;

		// 飞行时长
		float flyTime = (target - source).magnitude / moveSpeed;
		StartCoroutine(OnAnimation(flyTime, type, source, target, count, rate, radius, onFinish));
	}

	public void Init(GameObject _gameObjectPool, List<GameObject> prefabList)
	{
		
		if (_gameObjectPool == null) {
			return;
		}
			
		_gameObjectPool.SetActive (false);
		gameObjectPool = _gameObjectPool.transform;

		objectPool = new Dictionary<int, List<Coin>>();

		if (prefabList != null) {
			ResPrefab = new GameObject[prefabList.Count];
			for (int i = 0; i < prefabList.Count; i++) {
				ResPrefab [i] = prefabList [i];
				ResPrefab [i].SetActive (false);
			}
		}
	}

}