using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIUnitItem
///   Description:    unit item in training dialog
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIUnitItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		private UIDialogTraining 	uiTraining = null;
		[HideInInspector]
		public 	int		 			unitID = 0;
		private ArmyType 			at = null;
		private ArmyDef 			ad = null;

		bool 	mouseDown = false;
		float 	timeMouseDown = 0.0f;
		float 	fInc = 0.0f;
		float 	fCurrent = 0.0f;

		public 	Image 		UnitIcon;
		public 	Image 		PriceIcon;
		public 	Text 		Price;
		public 	Text 		Level;

		void Start () {
		
		}
		
		void Update () {

			// while user clicked this item, add unit 
			if(mouseDown){
				timeMouseDown += Time.deltaTime * 0.1f;
				fInc += Mathf.Exp(timeMouseDown);
				fCurrent += fInc;
				if(fCurrent > 10.0f) {
					fCurrent -= 10.0f;
					uiTraining.UnitGenAdd(unitID);
				}
			}

			if(ad != null)
				ad.PriceInfoCheck(Price);
		}

		// if user clicked 'i' button on this item
		// show unit info dialog
		public void OnButtonInfo() {
			uiTraining.UnitInfo(unitID);
		}
		
		public void OnPointerDown(PointerEventData eventData) {
			//Debug.Log ("OnPointerDown");
			mouseDown = true;
			timeMouseDown = 0.0f;
			fInc = 0.0f;
			fCurrent = 0.0f;
			uiTraining.UnitGenAdd(unitID);
		}
		
		public void OnPointerUp(PointerEventData eventData) {
			mouseDown = false;
			timeMouseDown = 0;
			//Debug.Log ("OnPointerUp");
		}
		
		public void Init(UIDialogTraining _uiTraining, int _unitID) {
			uiTraining = _uiTraining;
			unitID = _unitID;
			at = TBDatabase.GetArmyType(unitID);
			ad = (at != null) ? at.GetDefine(1) : null;
			if(ad != null) {
				Price.text = ad.ResearchCost.ToString ("#,##0");
			}
		}
	}
}