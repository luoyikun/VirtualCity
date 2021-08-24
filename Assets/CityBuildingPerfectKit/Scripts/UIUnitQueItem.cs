using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIUnitQueItem
///   Description:    unit trainig wue item in training dialog
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIUnitQueItem : MonoBehaviour {

		private UIDialogTraining 	uiTraining = null;
		private bool 				Initialized = false;

		[HideInInspector]
		public 	GenQueItem			item = null;

		public 	Image 		UnitIcon;
		public 	Image 		Progress;
		public 	Text 		Count;
		public 	Text 		TimeLeft;

		void Update () {

			if(!Initialized) return; 
		
			// show progress and left time
			Progress.fillAmount = ((float)item.at.TrainingTime - item.timeLeft)/(float)item.at.TrainingTime;
			TimeLeft.text = BENumber.SecToString((int)item.timeLeft);
			Count.text = item.Count.ToString()+"x";
		}

		public void OnButtonClicked() {
			uiTraining.UnitGenRemove(item);
		}
		
		public void Init(UIDialogTraining _uiTraining, GenQueItem _item) {
			uiTraining = _uiTraining;
			item = _item;
			Initialized = true;
		}
	}
}