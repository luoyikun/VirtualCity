using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class navtest : MonoBehaviour {
    public GameObject CharacterObj;
    public List<Transform> TargetTransfromList;
    public NavMeshAgent NavMA;
    public Vector3 TargetPosition;
    public Animator CharacterAnima;
	// Use this for initialization
	void Start () {
        NavMA = CharacterObj.GetComponent<NavMeshAgent>();
        CharacterAnima = CharacterObj.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //NavMA.destination = TargetPosition;
	}
    public void ClickGoToTargetBtn(GameObject obj)
    {
        Debug.Log(obj.name);
        for (int i = 0; i < TargetTransfromList.Count; i++)
        {
            if (TargetTransfromList[i].name == obj.name)
            {
                NavMA.destination = TargetTransfromList[i].position;
                TargetPosition = TargetTransfromList[i].position;
                //NavMA.ResetPath();
                CharacterAnima.SetBool("IsRun", true);
            }
        }
    }
    public void ClickStopNavigation()
    {
        NavMA.ResetPath();
    }
}

