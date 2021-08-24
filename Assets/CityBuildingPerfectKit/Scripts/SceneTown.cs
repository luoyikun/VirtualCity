using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BitBenderGames;
using Framework.Event;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneTown
///   Description:    main class of town 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class SceneTown : MonoBehaviour {

		public	static SceneTown instance;
		//public  Text		textLevel;

		private bool 		bInTouch = false;
		private float 		ClickAfter = 0.0f;
		private bool 		bTemporarySelect = false;
		private Vector3		vCamRootPosOld = Vector3.zero;
		private Vector3		mousePosOld = Vector3.zero;
		private Vector3		mousePosLast = Vector3.zero;
		public 	GameObject 	goCamera=null;
		public 	GameObject 	goCameraRoot=null;
		public	bool 		camPanningUse = true;
		public 	BEGround 	ground=null;
		private	bool 		Dragged = false;

		private float 		zoomMax = 128;
		private float 		zoomMin = 16;
		private float 		zoomCurrent = 64.0f;
		private float 		zoomSpeed = 4;

		public	float 		perspectiveZoomSpeed = 0.0001f;	// The rate of change of the field of view in perspective mode.
		public	float 		orthoZoomSpeed = 0.5f;   		// The rate of change of the orthographic size in orthographic mode.

		[HideInInspector]
		public	Plane 		xzPlane;


		// when game started, camera zoomin 
		private bool			InFade = true;
		private float			FadeAge = 0.0f;

		private Building		MouseClickedBuilding = null;
		private Text			HouseInfo = null;

		public	static 	bool 		isModalShow = false;
		public  static 	Building 	buildingSelected = null;
		public  static 	BENumber	Exp;
		public  static 	BENumber	Gold;
		public  static 	BENumber	Elixir;
		public  static 	BENumber	Gem;
		public  static 	BENumber	Shield;
		private static 	int 		Level = 0;
		private static 	int 		ExpTotal = 0;

		void Awake () {
			instance=this;

			// initialize BENumber class and set ui element 
			/*Exp = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 0);
			Exp.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelExp/Fill").GetComponent<Image>());

			Gold = new BENumber(BENumber.IncType.VALUE, 0, 200000, 10000); // initial gold count is 1000
			Gold.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGold/Text").GetComponent<Text>());
			Gold.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelGold/Fill").GetComponent<Image>());

			Elixir = new BENumber(BENumber.IncType.VALUE, 0, 300000, 10000); // initial elixir count is 1000	
			Elixir.AddUIText(BEUtil.GetObject("PanelOverlay/LabelElixir/Text").GetComponent<Text>());
			Elixir.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelElixir/Fill").GetComponent<Image>());

			Gem = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 1000);	// initial gem count is 100	0	
			Gem.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGem/Text").GetComponent<Text>());

			HouseInfo = BEUtil.GetObject("PanelOverlay/LabelHouse/Text").GetComponent<Text>();

			Shield = new BENumber(BENumber.IncType.TIME, 0, 100000000, 0);			
			Shield.AddUIText(BEUtil.GetObject("PanelOverlay/LabelShield/Text").GetComponent<Text>());*/

			// For camera fade animation, set cameras initial positions
			//goCameraRoot.transform.position = new Vector3(-5.5f,0,-5);
			//goCamera.transform.localPosition = new Vector3(0,0,-128.0f);
			InFade = true;
			FadeAge = 0.0f;
		}

        private void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener(Common.EventStr.ModelRotate, OnEvModelRotate);
        }

        void Start () {
            EventManager.Instance.AddEventListener(Common.EventStr.ModelRotate, OnEvModelRotate);
            Time.timeScale = 1;
			isModalShow = false;
			xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);

			// load game data from xml file
			//Load ();

			//if user new to this game add initial building
			if(true) {
				// add town hall 
				{
					Building script = BEGround.instance.BuildingAdd (0,1);
					script.Move(Vector3.zero);
					BuildingSelect(script);
					BuildingLandUnselect();
				}
				// add hut
				{
					Building script = BEGround.instance.BuildingAdd (1,1);
					script.Move(new Vector3(4,0,0));
					BuildingSelect(script);
					BuildingLandUnselect();
				}
			}

			//GainExp(0); // call this once to calculate level

			//set resource's capacity
			//CapacityCheck();

			// create workers by hut count
			int HutCount = BEGround.instance.GetBuildingCount(1);
			BEWorkerManager.instance.CreateWorker(HutCount);
			BEGround.instance.SetWorkingBuildingWorker();
		}

		// result of quit messagebox
		public void MessageBoxResult(int result) {
			BEAudioManager.SoundPlay(6);
			if(result == 0) {
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#else
				Application.Quit();
				#endif
			}
		}

		void Update () {

			// get delta time from BETime
			float deltaTime = BETime.deltaTime;

			// if user pressed escape key, show quit messagebox
			/*if (!UIDialogMessage.IsShow() && !isModalShow && Input.GetKeyDown(KeyCode.Escape)) { 
				UIDialogMessage.Show("Do you want to quit this program?", "Yes,No", "Quit?", null, (result) => { MessageBoxResult(result); } );
			}*/

			// if in camera animation 
			if(InFade) {

				//camera zoom in
				FadeAge += Time.deltaTime * 0.7f;
				if(FadeAge > 1.0f) { 
					InFade = false;
					FadeAge = 1.0f;
					zoomCurrent = 24.0f;
				}

				//goCameraRoot.transform.position = Vector3.Lerp(new Vector3(-5.5f,0,-5), Vector3.zero, FadeAge);
				//goCamera.transform.localPosition = Vector3.Lerp(new Vector3(0,0,-128.0f), new Vector3(0,0,-24.0f), FadeAge);
			}

			//Exp.Update();
			//Gold.Update();
			//Elixir.Update();
			//Gem.Update();
			//Shield.ChangeTo (Shield.Target() - (double)deltaTime);
			//Shield.Update();
			//HouseInfo.text = BEWorkerManager.instance.GetAvailableWorkerCount().ToString () +"/"+BEGround.instance.GetBuildingCount(1).ToString ();

			//if(UIDialogMessage.IsShow() || isModalShow) return;
		    //if(EventSystem.current.IsPointerOverGameObject()) return;

			if(Input.GetMouseButton(0)) {

                if (Application.isMobilePlatform && Input.touchCount > 0)
                {
                    //Debug.Log("点击UI");
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        return;
                }
                else if (EventSystem.current.IsPointerOverGameObject())
                {
                    //Debug.Log("点击UI");
                    return;
                }


                //Click MouseButton
                if (bInTouch == false) {

                    bInTouch = true;//当前点击了一个建筑
                    ClickAfter = 0.0f;
                    bTemporarySelect = false;
                    Dragged = false;
                    mousePosOld = Input.mousePosition;
                    mousePosLast = Input.mousePosition;
                    vCamRootPosOld = goCameraRoot.transform.position;

                    //when a building was selected and user drag mouse on the map
                    //check mouse drag start is over selected building or not
                    //if not do not move selected building
                    Ray ray = Camera.main.ScreenPointToRay(mousePosOld);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building"))
                    {
                        MouseClickedBuilding = BuildingFromObject(hit.collider.gameObject);
                        Debug.Log("Click Build Name:" + MouseClickedBuilding.Type);
                    }
                    else
                    {
                        MouseClickedBuilding = null;
                    }
                    
                    //Debug.Log ("Update buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));

                }
				else {
					//Mouse Button is in pressed 
					//if mouse move certain diatance
					if(Vector3.Distance (Input.mousePosition,mousePosLast) > 0.01f) {

						// set drag flag on
						if(!Dragged) {
							Dragged = true;

							// show tile grid
							if((buildingSelected != null) && (MouseClickedBuilding == buildingSelected)) {
                                //显示网格
                                BETween.alpha(ground.gameObject, 0.1f, 0.0f, 0.3f);
                                //Debug.Log ("ground alpha to 0.1");

                                //禁止摄像头的移动
                                if (DataMgr.m_buildMode == EnBuildMode.Build)
                                {
                                    MobileTouchCamera.instance.OnDragSceneObject();
                                }
                            }
						}

						mousePosLast = Input.mousePosition;

						// if selected building exist
						if((buildingSelected != null) && (MouseClickedBuilding == buildingSelected)  && DataMgr.m_buildMode == EnBuildMode.Build) {
							Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							float enter;
							xzPlane.Raycast(ray, out enter);
							Vector3 vTarget = ray.GetPoint(enter);
                            //Debug.Log("vTarget:" + vTarget.x + "," + vTarget.y + "," + vTarget.z);
                            //得到鼠标点击地面的坐标
							// move selected building
							buildingSelected.Move (vTarget);
						}
					}
					// Not Move
					else {

						if(!Dragged) {
							ClickAfter += Time.deltaTime;
							if(!bTemporarySelect && (ClickAfter > 0.5f)) {
								bTemporarySelect = true;
                                //Debug.Log ("Update2 buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
                                //按下到松手且不移动，判定为点击状态
                                Pick();
							}
						}
					}

				}
			}
            //当前没有手指按下
			else {


                //Release MouseButton
                if (bInTouch) {
					bInTouch = false;

					// if in drag state
					if(Dragged) {

						// seleted building exist
						if(buildingSelected != null) {

							// hide tile grid
							if(MouseClickedBuilding == buildingSelected) 
								BETween.alpha(ground.gameObject, 0.1f, 0.3f, 0.0f);

							if(buildingSelected.Landable && buildingSelected.OnceLanded)
								BuildingLandUnselect();
						}
					}
					else {

						if(bTemporarySelect) {
							// land building
							if((buildingSelected != null) && (MouseClickedBuilding != buildingSelected) && buildingSelected.OnceLanded)
								BuildingLandUnselect();
						}
						else {
							// land building
							if((buildingSelected != null) && (MouseClickedBuilding != buildingSelected) && buildingSelected.OnceLanded)
								BuildingLandUnselect();

							//Debug.Log ("Update3 buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
							Pick();
						}
					}
				}
			}

		}

		public void Pick() {
			//Debug.Log ("Pick buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
			//GameObject goSelectNew = null;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				//Debug.Log ("Pick"+hit.collider.gameObject.tag);
				if(hit.collider.gameObject.tag == "Ground") {
					BuildingSelect(null);
					return;
				}
				else if(hit.collider.gameObject.tag == "Building") {
					Building buildingNew = BuildingFromObject(hit.collider.gameObject);
					if(buildingNew.HasCompletedWork())
						return;

					BuildingSelect(buildingNew);
					return;
				}
				else {
				}
			}
		}

		// add exp
		public void GainExp(int exp) {
			ExpTotal += exp;
			int NewLevel = TBDatabase.GetLevel(ExpTotal);
			int LevelExpToGet = TBDatabase.GetLevelExp(NewLevel);
			int LevelExpStart = TBDatabase.GetLevelExpTotal(NewLevel);

			//SceneTown.Exp.MaxSet(LevelExpToGet);
			int ExpLeft = ExpTotal - LevelExpStart;
			//SceneTown.Exp.ChangeTo(ExpLeft);

			// if level up occured
			if((NewLevel > Level) && (Level != 0)) {
				// show levelup notify here
			}
			Level = NewLevel;
			//textLevel.text = NewLevel.ToString ();

			// save game data
			Save ();
		}

		// get building script
		// if child object was hitted, check parent 
		public Building BuildingFromObject(GameObject go) {
			Building buildingNew = go.GetComponent<Building>();
			if(buildingNew == null)  buildingNew = go.transform.parent.gameObject.GetComponent<Building>();

			return buildingNew;
		}

		// select building
		public void BuildingSelect(Building buildingNew) {

			// if user select selected building again
			bool SelectSame = (buildingNew == buildingSelected) ? true : false;

			if(buildingSelected != null) {

				// if initialy created building, then pass
				if(!buildingSelected.OnceLanded) return;
				// building can't land, then pass 
				if(!buildingSelected.Landed && !buildingSelected.Landable) return;

				// land building
				BuildingLandUnselect();
				//UICommand.Hide();
			}

			if(SelectSame) 
				return;

			buildingSelected = buildingNew;

			if(buildingSelected != null) {
				//Debug.Log ("Building Selected:"+buildingNew.gameObject.name+" OnceLanded:"+buildingNew.OnceLanded.ToString ());
				// set scale animation to newly selected building
				BETween bt = BETween.scale(buildingSelected.gameObject, 0.1f, new Vector3(1.0f,1.0f,1.0f), new Vector3(1.4f,1.4f,1.4f));
				bt.loopStyle = BETweenLoop.pingpong;
				// se tbuilding state unland
				buildingSelected.Land(false, true);

                //点击了建筑，摄像头移动到建筑的上方
                if (DataMgr.m_buildMode == EnBuildMode.Build)
                {
                    EventManager.Instance.DispatchEvent(Common.EventStr.BuildCamMove, new EventDataEx<Vector3>(buildingSelected.transform.position));
                }
			}
		}

		public void BuildingLandUnselect() {
			if(buildingSelected == null) return;

			buildingSelected.Land(true, true);
			buildingSelected = null;
			Save ();

			//UICommand.Hide();
		}

		public void BuildingDelete() {
			if(buildingSelected == null) return;

			buildingSelected.Land (false, false);
			BEGround.instance.BuildingRemove (buildingSelected);
			Destroy (buildingSelected.gameObject);
			buildingSelected = null;
			Save ();
		}

		//pause
		public void OnButtonAttack() {
			BEAudioManager.SoundPlay(6);
		}

		// user clicked shop button
		public void OnButtonShop() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.Normal);
		}

		// user clicked gem button
		public void OnButtonGemShop() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.InApp);
		}

		// user clicked house button
		public void OnButtonHouse() {
			BEAudioManager.SoundPlay(6);
			UIShop.Show(ShopType.House);
		}

		// user clicked option button
		public void OnButtonOption() {
			BEAudioManager.SoundPlay(6);
			UIOption.Show();
		}

		public void CapacityCheck() {
			int GoldCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Gold);
			//Debug.Log ("iGoldCapacityTotal:"+GoldCapacityTotal.ToString ());
			int ElixirCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Elixir);
			//Debug.Log ("ElixirCapacityTotal:"+ElixirCapacityTotal.ToString ());

			Gold.MaxSet(GoldCapacityTotal);
			if(Gold.Target() > GoldCapacityTotal) Gold.ChangeTo(GoldCapacityTotal);
			Elixir.MaxSet(ElixirCapacityTotal);
			if(Elixir.Target() > ElixirCapacityTotal) Elixir.ChangeTo(ElixirCapacityTotal);

			BEGround.instance.DistributeByCapacity(PayType.Gold, (float)Gold.Target());
			BEGround.instance.DistributeByCapacity(PayType.Elixir, (float)Elixir.Target());
		}

		// related to save and load gamedata with xml format
		bool	UseEncryption = false;
		bool	bFirstRun = false;
		string 	configFilename = "Config.dat";
		int 	ConfigVersion = 1;
		public	bool 	InLoading = false;
		
		// Do not save town when script quit.
		// save when action is occured
		// (for example, when building created, when start upgrade, when colled product, when training start)
		public void Save() {

            return;
            if (InLoading) return;

			string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<item><name>wrench</name></item>");
			{
				xmlDocument.DocumentElement.RemoveAll();
				
				// Version
				{ XmlElement ne = xmlDocument.CreateElement("ConfigVersion"); 		ne.SetAttribute("value", ConfigVersion.ToString());			xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("Time"); 				ne.SetAttribute("value", DateTime.Now.ToString());			xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("ExpTotal"); 			ne.SetAttribute("value", ExpTotal.ToString());				xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("Gem"); 				ne.SetAttribute("value", Gem.Target().ToString());			xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("Gold"); 				ne.SetAttribute("value", Gold.Target().ToString());			xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("Elixir"); 				ne.SetAttribute("value", Elixir.Target().ToString());		xmlDocument.DocumentElement.AppendChild (ne); }
				{ XmlElement ne = xmlDocument.CreateElement("Shield"); 				ne.SetAttribute("value", Shield.Target().ToString());		xmlDocument.DocumentElement.AppendChild (ne); }

				Transform trDecoRoot = BEGround.instance.trDecoRoot;
				//List<GameObject> goTiles=new List<GameObject>();
				foreach(Transform child in trDecoRoot) {
					Building script = child.gameObject.GetComponent<Building>();
					if(script != null) {
						script.Save (xmlDocument);
					}
				}
			
				// ####### Encrypt the XML ####### 
				// If you want to view the original xml file, turn of this piece of code and press play.
				if (xmlDocument.DocumentElement.ChildNodes.Count >= 1) {
					if(UseEncryption) {
						string data = BEUtil.Encrypt (xmlDocument.DocumentElement.InnerXml);
						xmlDocument.DocumentElement.RemoveAll ();
						xmlDocument.DocumentElement.InnerText = data;
					}
					xmlDocument.Save (xmlFilePath);
				}
				// ###############################
			}
		}

		public void Load() {

			string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
			if(!File.Exists(xmlFilePath)) {
				Save();
				bFirstRun = true;
			}
			else {
				bFirstRun = false;
			}
			
			InLoading = true;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlFilePath);
			
			// ####### Encrypt the XML ####### 
			// If the Xml is encrypted, so this piece of code decrypt it.
			if (xmlDocument.DocumentElement.ChildNodes.Count <= 1) {
				if(UseEncryption) {
					string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
					xmlDocument.DocumentElement.InnerXml = data;
				}
			}
			//################################


			if(xmlDocument != null) {
				XmlElement element = xmlDocument.DocumentElement;
				XmlNodeList list = element.ChildNodes;
				foreach(XmlElement ele in list) {
					if(ele.Name == "ConfigVersion")			{	ConfigVersion 	= int.Parse(ele.GetAttribute("value"));	 		}
					else if(ele.Name == "Time")				{	
						DateTime dtNow = DateTime.Now;	
						DateTime dtSaved = DateTime.Parse(ele.GetAttribute("value"));	 
						//Debug.Log ("dtNow:"+dtNow.ToString());
						//Debug.Log ("dtSaved:"+dtSaved.ToString());
						TimeSpan timeDelta = dtNow.Subtract(dtSaved);	
						//Debug.Log ("TimeSpan:"+timeDelta.ToString());
						BETime.timeAfterLastRun = timeDelta.TotalSeconds;
					}
					/*else if(ele.Name == "ExpTotal")			{	ExpTotal = int.Parse(ele.GetAttribute("value"));	 		}
					else if(ele.Name == "Gem")				{	Gem.ChangeTo(double.Parse(ele.GetAttribute("value")));	 		}
					else if(ele.Name == "Gold")				{	Gold.ChangeTo(double.Parse(ele.GetAttribute("value")));	 		}
					else if(ele.Name == "Elixir")			{	Elixir.ChangeTo(double.Parse(ele.GetAttribute("value")));	 	}
					else if(ele.Name == "Shield")			{	Shield.ChangeTo(double.Parse(ele.GetAttribute("value")));	 	}
					else if(ele.Name == "Building")			{	
						int Type = int.Parse(ele.GetAttribute("Type"));	 		
						int Level = int.Parse(ele.GetAttribute("Level"));	
						//Debug.Log ("Building Type:"+Type.ToString()+" Level:"+Level.ToString());
						Building script = BEGround.instance.BuildingAdd(Type, Level);
						script.Load (ele);
					}*/
					else {}
				}
			}

			InLoading = false;
		}

        void OnEvModelRotate(EventData data)
        {
            if (buildingSelected != null)
            {
                buildingSelected.ModelRotate();
            } 
        }

    }

}
