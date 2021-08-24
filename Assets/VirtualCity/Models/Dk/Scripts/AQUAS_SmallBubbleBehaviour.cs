using UnityEngine;
using System.Collections;

public class AQUAS_SmallBubbleBehaviour : MonoBehaviour {

    #region Variables
    public float averageUpdrift;
	public float waterLevel;

    public GameObject mainCamera;

	float updriftFactor;
    #endregion
	
	//<summary>
    //Randomizes updrift
    //</summary>
	void Start () {
		updriftFactor = Random.Range (-averageUpdrift*0.75f, averageUpdrift*0.75f);
	}
	
    //<summary>
	// Update is called once per frame
    //<summay>
	void Update () {

        transform.Translate (Vector3.up * Time.deltaTime * (averageUpdrift+updriftFactor), Space.World);

        if (mainCamera.transform.position.y > waterLevel || transform.position.y > waterLevel) {
            Destroy(this.gameObject);
        }	
	}
}
