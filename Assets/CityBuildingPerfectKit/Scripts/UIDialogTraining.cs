using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogTraining
///   Description:    class for unit training
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class UIDialogTraining : UIDialogBase {
		
		public static UIDialogTraining instance;

		private Building 		building = null;
		private BuildingType 	bt = null;
		private BuildingDef 	bd = null;

		public	Text 				TrainingInfo;
		public	Text 				CapacityInfo;
		public	Text 				TimeLeft;
		public	Text 				GemCount;
		public	GameObject 			prefabUnitItem;
		public	GameObject 			prefabUnitQueItem;
		public	RectTransform 		rtUnitQueList;
		public	RectTransform 		rtUnitList;
		public	RectTransform 		rtUnitInfo;
		public 	UIDialogUnitInfo	scriptUnitInfo;

		public 	List<UIUnitQueItem>	queItems = new List<UIUnitQueItem>();

		void Awake () {
			instance=this;
		}
		
		void Start () {
			gameObject.SetActive(false);
		}
		
		void Update () {

			if (!UIDialogMessage.IsShow() && Input.GetKeyDown(KeyCode.Escape)) { 
				_Hide();
			}

			int GenCountTotal = 0;
			int GenTimeTotal = 0;
			for(int i=0 ; i < building.queUnitGen.Count ; ++i) {
				GenQueItem item = building.queUnitGen[i];
				GenCountTotal += item.Count;
				GenTimeTotal += item.GetGenLeftTime();
			}

			TrainingInfo.text = "Train Troops "+GenCountTotal.ToString ()+"/"+bd.TrainingQueueMax.ToString ();
			CapacityInfo.text = "Troop capacity after training: 51 / 200";
			TimeLeft.text = BENumber.SecToString(GenTimeTotal);
			GemCount.text = "10,000";
		}
		
		void Reset () {

			queItems.Clear();
			bd = null;
			bt = TBDatabase.GetBuildingType(7);//building.Type);
			bd = bt.GetDefine(1);//building.Level);

			// delete old items of each content
			for(int j=rtUnitQueList.childCount-1;j>=0;j--){
				Destroy (rtUnitQueList.GetChild(j).gameObject);
			}
			for(int i=0 ; i < building.queUnitGen.Count ; ++i) {
				GenQueItem item = building.queUnitGen[i];

				GameObject go = (GameObject)Instantiate(prefabUnitQueItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitQueList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(i*-100, 0);
				
				UIUnitQueItem script = go.GetComponent<UIUnitQueItem>();
				script.Init(this, item);
				queItems.Add (script);
				Debug.Log ("queItems.Add "+queItems.Count.ToString()+"unitID:"+item.unitID.ToString());
			}

			// delete old items of each content
			for(int j=rtUnitList.childCount-1;j>=0;j--){
				Destroy (rtUnitList.GetChild(j).gameObject);
			}
			int sz = TBDatabase.GetArmyTypeCount();
			for(int i=0 ; i < sz ; ++i) {
				int col = i/2;
				int row = i%2;

				GameObject go = (GameObject)Instantiate(prefabUnitItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(col*160, row*-160);
				
				UIUnitItem script = go.GetComponent<UIUnitItem>();
				script.Init(this, i);
			}
			rtUnitList.sizeDelta = new Vector3(160*((sz+1)/2), 310);
		}

		public void ItemRemove(int unitID) {
			Debug.Log ("ItemRemove "+unitID.ToString());
			Debug.Log ("queItems.Count "+queItems.Count.ToString());

			for(int i=0 ; i < queItems.Count ; ++i) {

				UIUnitQueItem uiItem = queItems[i];
				if(uiItem.item.unitID != unitID) continue;

				for(int j=i+1 ; j < queItems.Count ; ++j) {
					RectTransform rt = queItems[j].gameObject.GetComponent<RectTransform>();
					rt.anchoredPosition = new Vector2((j-1)*-100, 0);
				}
					
				Debug.Log ("queItems.RemoveAt "+i.ToString()+"unitID:"+uiItem.item.unitID.ToString());

				queItems.RemoveAt(i);
				Destroy (uiItem.gameObject);
				return;
			}
		}

		public void UnitInfo(int unitID) {
			Debug.Log ("UnitInfo "+unitID.ToString());
			UIDialogUnitInfo.Show(unitID);
		}
		
		public void UnitGenAdd(int unitID) {
			Debug.Log ("UnitCreate "+unitID.ToString());

			GenQueItem item = building.UnitGenAdd(unitID, 1);

			// search unit que list with given unit id
			int idx = queItems.FindIndex(x => x.item.unitID==unitID);
			Debug.Log ("UnitCreate idx:"+idx.ToString());
			if(idx == -1) {
				GameObject go = (GameObject)Instantiate(prefabUnitQueItem, Vector3.zero, Quaternion.identity);
				go.transform.SetParent (rtUnitQueList);
				go.transform.localScale = Vector3.one;
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(queItems.Count*-100, 0);

				UIUnitQueItem script = go.GetComponent<UIUnitQueItem>();
				script.Init(this, item);
				queItems.Add (script);
				Debug.Log ("queItems.Add "+queItems.Count.ToString()+"unitID:"+item.unitID.ToString());
			}
		}

		public void UnitGenRemove(GenQueItem item) {
			item.Count -= 1;
		}

		public void UnitCreated(int unitID) {
			Debug.Log ("UnitCreated "+unitID.ToString());
			//UnitCount[unitID]++;
			Debug.Log ("UnitID:"+unitID.ToString ()+ " Count:"+unitID.ToString());
		}
		
		public void OnButtonInstant() {
			Debug.Log ("OnButtonInstant ");
			//_Hide();
		}
		
		public void OnButtonOk() {
			_Hide();
		}

		public void _Show(Building script) {
			
			building = script;
			
			Reset();
			UIDialogUnitInfo.Show(-1);
			ShowProcess();
		}
		
		public static void Show(Building script) 	{ instance._Show(script); }
		public static void Hide() 					{ instance._Hide(); }
	}
}