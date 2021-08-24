using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIDialogUnitInfo
///   Description:    class for unit info
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIDialogUnitInfo : MonoBehaviour {
		
		private static UIDialogUnitInfo instance;
		
		private int 			unitID = -1;
		private ArmyType 		at = null;
		//private ArmyDef 		ad = null;

		public	UIDialogTraining	scriptUITraining;	
		public	Image 				Dialog;
		public	Text 				textTitle;
		public	Text 				textLevel;
		public	Image 				imgIcon;
		public	ProgressInfo []		progresses;
		public	GameObject 			goInfo;
		public	Text 				InfoTitle;
		public	Text 				InfoText;
		
		void Awake () {
			instance=this;
		}
		
		void Start () {

		}
		
		void Update () {
			
		}
		
		void Reset () {
			if(unitID == -1) return;

			at = TBDatabase.GetArmyType(unitID);
			//ad = (at != null) ? at.GetDefine(1) : null;

			if(at != null) {
				textTitle.text = at.Name;
				//textLevel.text = "Level "+building.Level.ToString ();
				//imgIcon.sprite = ;
			}
		}
		
		public void OnButtonBack() {
			scriptUITraining.UnitInfo(-1);
		}
		
		public void OnButtonExit() {
			scriptUITraining._Hide();
		}
		
		public void _Show(int _unitID) {
			
			unitID = _unitID;
			Reset();
			Dialog.GetComponent<RectTransform>().anchoredPosition = (unitID == -1) ? new Vector2(0,-720) : new Vector2(0,0);
			//gameObject.SetActive(true);
			//SceneTown.isModalShow = true;
			//Dialog.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
			//BETween.scale(Dialog.gameObject, 0.2f, new Vector3(0.7f,0.7f,0.7f), new Vector3(1,1,1)).method = BETweenMethod.easeOutBack;
			//BETween.alpha(gameObject, 0.2f, 0.0f, 0.5f).method = BETweenMethod.easeOut;
		}

		public void _Hide() {
			//BETween.scale(Dialog.gameObject, 0.2f, new Vector3(1,1,1), new Vector3(0.7f,0.7f,0.7f)).method = BETweenMethod.easeOut;
			//BETween.alpha(gameObject, 0.2f, 0.5f, 0.0f).method = BETweenMethod.easeOut;
			//BETween.enable(gameObject, 0.01f, false).delay = 0.4f;
			//gameObject.SetActive(false);
			//UIDialogTraining.Hide();
			//SceneTown.isModalShow = false;
		}
		
		public static void Show(int _unitID) 	{ instance._Show(_unitID); }
		public static void Hide() 				{ instance._Hide(); }
	}
}