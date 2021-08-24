using UnityEngine;
using System.Collections;

public class AQUAS_BubbleBehaviour : MonoBehaviour {

    #region Variables
    public float averageUpdrift;
	public float waterLevel;

    public GameObject mainCamera;
	public GameObject smallBubble;
	int smallBubbleCount;
	int maxSmallBubbleCount;

    AQUAS_SmallBubbleBehaviour smallBubbleBehaviour;
    #endregion

    //Initialization
	void Start () {
		maxSmallBubbleCount = (int)Random.Range (20, 30);
		smallBubbleCount = 0;

        smallBubbleBehaviour = smallBubble.GetComponent<AQUAS_SmallBubbleBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.up * Time.deltaTime * averageUpdrift, Space.World);

        SmallBubbleSpawner();

        if (mainCamera.transform.position.y > waterLevel || transform.position.y > waterLevel) 
        {
            Destroy(this.gameObject);
        }
	}

    //<summary>
    //Spawns small bubbles according
    //Small bubbles parameters & randomization are based on bubble parameters but are not directly controllable
    //</summary>
    void SmallBubbleSpawner() {
        if (smallBubbleCount <= maxSmallBubbleCount)
        {
            smallBubble.transform.localScale = transform.localScale * Random.Range(0.05f, 0.2f);

            smallBubbleBehaviour.averageUpdrift = averageUpdrift * 0.5f;
            smallBubbleBehaviour.waterLevel = waterLevel;
            smallBubbleBehaviour.mainCamera = mainCamera;

            Instantiate(smallBubble, new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y - Random.Range(0.01f, 1), transform.position.z + Random.Range(-0.1f, 0.1f)), Quaternion.identity);
            
            smallBubbleCount += 1;
        }
    }
}
