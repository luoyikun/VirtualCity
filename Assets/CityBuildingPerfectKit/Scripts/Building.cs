using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Framework.UI;
using Framework.Event;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          Building
///   Description:    class of building
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// if building can training unit
	// use this class
	public class GenQueItem {
		public 	Building 		building;			// owner building 
		public 	int 			unitID;				// unit id
		public	ArmyType 		at = null;			// definition of unit
		public 	int 			Count;				// allocated count of unit
		public 	float 			timeLeft = 0.0f;	// training time left of current training unit

		public GenQueItem(Building _building, int _unitID, int _Count) {
			building 	= _building;
			unitID 		= _unitID;
			at 			= TBDatabase.GetArmyType(unitID);
			Count		= _Count;
			timeLeft 	= at.TrainingTime;
		}

		public float Update(float deltaTime) {

			if(Count <= 0) return deltaTime;

			// if timeleft is lower then zero
			while(deltaTime > timeLeft) {
				deltaTime -= timeLeft;
				// unit created, so decrease count
				Count--;
				building.UnitCreated(unitID);
				timeLeft = at.TrainingTime;
				if(Count == 0) 
					return deltaTime;
			}

			timeLeft -= deltaTime;
			//Debug.Log ("GenQueItem unitID:"+unitID.ToString ()+" Count:"+Count.ToString ()+" timeLeft:"+timeLeft.ToString () );
			return 0.0f;
		}

		public int GetGenLeftTime() {
			if(Count == 0)
				return 0;
			
			return at.TrainingTime * (Count-1) + (int)timeLeft;
		}
	
	}

	// building is common class of all building
	// it has capacity relative functions and
	// production relative functions and training relative functions.
	public class Building : MonoBehaviour {

        public int m_dir = 0; //方向 0，90，180，270

		private	Vector2		tilePosOld = new Vector2(0,0);	// remember old tile pos while drag building
		public  Vector2		tilePos = new Vector2(0,0);		// tile position

        public Vector2 tileSizeOri = new Vector3(1, 1); //保存原始cell大小 

        public  Vector2		tileSize = new Vector2(1,1);	// building's size (forexample, 1x1, 2x2, 3x3 tiles)

		[HideInInspector]
		public 	bool		OnceLanded = false;				// if newly created building, oncelanded is false
		[HideInInspector]
		public 	bool		Landed = false;					// current building's land state //是否是建造完成
		public 	int			Type = 0;						// building type. distinguish building from others with this value
		public 	int			Level = 1;						// level of building

		//ganeobjects of building base prefab
		public 	GameObject	goGrid = null;					// show how many tiles building has occupied	
		public 	GameObject	goArea = null;					// yellow area 
		public 	GameObject	goArrowRoot = null;
		public 	GameObject	goArrowXMinus = null;
		public 	GameObject	goArrowXPlus = null;
		public 	GameObject	goArrowZMinus = null;
		public 	GameObject	goArrowZPlus = null;
		public 	GameObject	goCenter = null;				// building mesh
		public 	GameObject	goXInc = null;					// if building is wall, has more mesh next wall in direction x
		public 	GameObject	goZInc = null;					// if building is wall, has more mesh next wall in direction z
		public 	GameObject	goRangeIn = null;				// for tower, show range
		public 	GameObject	goRangeOut = null;
		[HideInInspector]
		public 	BEGround	ground = null;
		[HideInInspector]
		public 	bool		Landable = true;

		public 	GameObject		prefUIInfo;					// prefab for building info ui
		public 	GameObject		prefUICollect;				// prefab for resource collect ui
		private UIInfo			uiInfo = null;				
		public	BuildingType 	bt = null;					// building type class of this building
		public	BuildingDef		def = null;					// current building's def
		public	BuildingDef 	defNext = null;				// def of next level 
		public	BuildingDef 	defLast = null;				// last def of certain building type


		[HideInInspector]
		public  bool		Collectable = false;			// if building can produce resource, whether collect is enable 
		public	float		Production = 0.0f;				// production value 
		public 	float [] 	Capacity = new float[(int)PayType.Max];	// if building can store resources, this is resource count being stored

		[HideInInspector]
		public  bool		UpgradeCompleted = false;		// is upgrading completed?
		[HideInInspector]
		public	bool		InUpgrade = false;				// in upgrading?
		public 	float		UpgradeTimeTotal = 0.0f;
		public	float		UpgradeTimeLeft = 0.0f;

		//private List<int>	WorkList = new List<int>();
		//private float		WorkTimeTotal = 0.0f;
		//private float		WorkTimeLeft = 0.0f;

		[HideInInspector]
		public 	List<int>			GenUnitCount = new List<int>();
		[HideInInspector]
		public 	List<GenQueItem>	queUnitGen = new List<GenQueItem>();

        private Transform m_trans;
		void Awake () {

            m_trans = this.transform;
            // if building can hold trained units
            // set count of unit by type
            for (int i=0 ; i < 10 ; ++i) {
				GenUnitCount.Add (0);
			}
		}
		


        void Update () {

			// use BETime to revise times
			float deltaTime = BETime.deltaTime;

			// if building is in selected state, keep color animation
			if(!Landed && (goCenter != null)) {
				float fColor = Mathf.PingPong(Time.time * 1.5f, 1) * 0.5f + 0.5f;
				Color clrTemp = new Color(fColor,fColor,fColor,1);
				BEUtil.SetObjectColor(goCenter, clrTemp);
				BEUtil.SetObjectColor(goXInc, clrTemp);
				BEUtil.SetObjectColor(goZInc, clrTemp);
			}

			// if building can produce resources
			if(Landed && !InUpgrade && (def != null) && (def.eProductionType != PayType.None)) {
				Production += (float)def.ProductionRate * deltaTime;

				// check maximum capacity
				if((int)Production >= def.Capacity[(int)def.eProductionType])
					Production = def.Capacity[(int)def.eProductionType];

				// is minimum resources generated, then ser collectable flagand show dialog
				if(((int)Production >= 10) && !uiInfo.groupCollect.gameObject.activeInHierarchy) {
					Collectable = true;
					uiInfo.CollectIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
					BETween.alpha(uiInfo.groupCollect.gameObject, 0.2f, 0.0f, 1.0f);
					uiInfo.groupCollect.gameObject.SetActive(true);
					uiInfo.groupInfo.gameObject.SetActive(false);
				}

				if(Collectable) {
					// when production count is reach to max count, then change dialog color to red
					uiInfo.CollectDialog.color = (Production == def.Capacity[(int)def.eProductionType]) ? Color.red : Color.white;
				}
			}

			// if in upgrade
			if(InUpgrade) {

				// if upgrading proceed
				if(!UpgradeCompleted) {
					// decrease left time
					UpgradeTimeLeft -= deltaTime;

					// if upgrading done
					if(UpgradeTimeLeft < 0.0f) {
						UpgradeTimeLeft = 0.0f;
						UpgradeCompleted = true;

						// if building is selected, then update command dialog
						if(UICommand.Visible && (SceneTown.buildingSelected == this))
							UICommand.Show(this);
					}
				}

				// update ui info
				uiInfo.TimeLeft.text = UpgradeCompleted ? "Completed!" : BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
				uiInfo.Progress.fillAmount = (UpgradeTimeTotal-UpgradeTimeLeft)/UpgradeTimeTotal;
				uiInfo.groupProgress.alpha = 1;
				uiInfo.groupProgress.gameObject.SetActive(true);
			}

			UnitGenUpdate(deltaTime);
		}

		// get gem count to finish current upgrading
		public int GetFinishGemCount() {
			if(!InUpgrade || UpgradeCompleted) return 0;

			int	 	FinishGemCount = ((int)UpgradeTimeLeft+1)/60;
			return FinishGemCount;
		}

		public bool InUpgrading() {
			return (InUpgrade && !UpgradeCompleted) ? true : false;
		}

		// initialize building
		public void Init(int type, int level) {
			Type = type;
			Level = level;

			// delete old meshes
			if(goCenter != null) 	{ Destroy (goCenter); goCenter = null; }
			if(goXInc != null)	 	{ Destroy (goXInc); goXInc = null; }
			if(goZInc != null) 		{ Destroy (goZInc); goZInc = null; }

			bt = TBDatabase.GetBuildingType(type);

			// get mesh path type and level
			int displayLevel = (level == 0) ? 1 : level;
			string meshPath = "Prefabs/Building/"+bt.Name+"_"+displayLevel.ToString ();
			//Debug.Log ("Loading Mesh "+meshPath);

			// instantiate mesh and set to goCenter
			GameObject prefabMesh = Resources.Load (meshPath) as GameObject;
			goCenter = (GameObject)Instantiate(prefabMesh, Vector3.zero, Quaternion.identity);
			goCenter.transform.SetParent (gameObject.transform);
			goCenter.transform.localPosition = Vector3.zero;
			goCenter.transform.localRotation = Quaternion.Euler(0,-90,0);

			// if wall
			if(type == 2) {

				// create x,z side mesh
				string meshSidePath = meshPath+"Side";
				GameObject prefabMeshSide = Resources.Load (meshSidePath) as GameObject;

				goXInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
				goXInc.transform.SetParent (gameObject.transform);
				goXInc.transform.localPosition = Vector3.zero;
				goXInc.transform.localRotation = Quaternion.Euler(0,180,0); // rotate to x direction

				goZInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
				goZInc.transform.SetParent (gameObject.transform);
				goZInc.transform.localPosition = Vector3.zero;
				goZInc.transform.localRotation = Quaternion.Euler(0,90,0); // rotate to z direction
			}

			// set tile size
			tileSize = new Vector2(bt.TileX, bt.TileZ);
            tileSizeOri = new Vector2(bt.TileX, bt.TileZ);
            // set proper material to gogrid
            goGrid.GetComponent<Renderer>().material = Resources.Load ("Materials/Tile"+bt.TileX.ToString ()+"x"+bt.TileZ.ToString ()) as Material;
			goGrid.transform.localScale = new Vector3(bt.TileX, 1, bt.TileZ);
			goArrowXMinus.transform.localPosition = new Vector3(-(0.4f+(float)bt.TileX*0.5f), 0.01f, 0);
			goArrowXPlus.transform.localPosition  = new Vector3( (0.4f+(float)bt.TileX*0.5f), 0.01f, 0);
			goArrowZMinus.transform.localPosition = new Vector3(0, 0.01f, -(0.4f+(float)bt.TileX*0.5f));
			goArrowZPlus.transform.localPosition  = new Vector3(0, 0.01f,  (0.4f+(float)bt.TileX*0.5f));

			goArea.transform.localScale = new Vector3(bt.TileX, 1, bt.TileZ);

			// get ui
			if(transform.Find ("UIBuilding")) {
				uiInfo = transform.Find ("UIBuilding").transform.Find ("UIInfo").GetComponent<UIInfo>();
			}
			else {
				uiInfo = UIInGame.instance.AddInGameUI(prefUIInfo, transform, new Vector3(0,1.5f,0)).GetComponent<UIInfo>();
			}
			uiInfo.groupInfo.gameObject.SetActive(false);
			uiInfo.groupProgress.gameObject.SetActive(false);
			uiInfo.groupCollect.gameObject.SetActive(false);
			uiInfo.Name.text = TBDatabase.GetBuildingName(Type);
			uiInfo.Level.text = "Level "+displayLevel.ToString ();
			uiInfo.building = this;

			// currently not used
			goRangeIn.SetActive(false);
			goRangeOut.SetActive(false);

			// initialize values 
			tilePosOld = tilePos;
			CheckLandable();
			CheckNeighbor();
			def = TBDatabase.GetBuildingDef(Type, Level);
			defNext = bt.GetDefine(Level+1);
			defLast = bt.GetDefLast();
			UpgradeTimeTotal = (defNext != null) ? defNext.BuildTime : 0;
		}

		public void UpjustYByState() {
			Vector3 vPos = transform.localPosition;
			vPos.y = Landed ? 0.0f : 0.1f;
			transform.localPosition = vPos;
		}

        /// <summary>
        /// 移动到指定鼠标点击世界坐标
        /// </summary>
        /// <param name="vTarget"></param>
		public void Move(Vector3 vTarget) {
			tilePos = ground.GetTilePos(vTarget, tileSize); // 把鼠标点转为 格子的点，从坐下角开始 0点
			ground.Move(gameObject, tilePos, tileSize);
			//Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());
			CheckLandable();
			UpjustYByState();
		}

		public void Move(int TileX, int TileZ) {
			tilePos = new Vector2(TileX, TileZ);
			ground.Move(gameObject, tilePos, tileSize);
			//Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());
			CheckLandable();
			UpjustYByState();
		}

        
        /// <summary>
        /// 确定当前位置是否能place，可以grid绿色，不可grid红色
        /// </summary>
        // check tiles of building is vacant and set color of grid object
        public void CheckLandable() {
			bool IsVacant = ground.IsVacant(tilePos, tileSize);
			//Debug.Log ("CheckLandable Vacant"+IsVacant.ToString ());
			Color clrTemp = IsVacant ? new Color(0,1,0,0.5f) : new Color(1,0,0,0.5f);
			BEUtil.SetObjectColor(goGrid, "_TintColor", clrTemp);
			Landable = IsVacant;
		}

		public void Land(bool landed, bool animate) {

			if(Landed == landed) return;

			if(landed && !Landable) {
				if(((int)tilePosOld.x == -1) && ((int)tilePosOld.y == -1))
					return;

				tilePos = tilePosOld;
				//Debug.Log ("Land RecoverOldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
				ground.Move(gameObject, tilePos, tileSize);
				CheckLandable();

				if(!Landable) 
					return;
			}

			Landed = landed;
			ground.OccupySet(this);

			if(!Landed) {
				tilePosOld = tilePos;
				//Debug.Log ("Land Save OldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
			}
			else {
				if(!OnceLanded)
					OnceLanded = true;
			}

			CheckLandable();
			goGrid.SetActive(Landed ? false : true);
			if(goArrowRoot != null) 
				goArrowRoot.SetActive(Landed ? false : true);

			if(uiInfo != null) {
				//uiInfo.groupProgress.alpha = 0;

				if(animate) {
					if(Landed) 	{
						BETween.alpha(uiInfo.groupInfo.gameObject, 0.1f, 1.0f, 0.0f);
						BETween.enable(uiInfo.groupInfo.gameObject, 0.1f, true, false);
					}
					else {
						BETween.alpha(uiInfo.groupInfo.gameObject, 0.1f, 0.0f, 1.0f);

                        //显示模型的血条ui
                        //uiInfo.groupInfo.gameObject.SetActive(true);
                        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                        //UIManager.Instance.LoadPanel(UIPanelName.modelctrlpanel, UIManager.CanvasType.Screen);

                    }
				}
				else {
					uiInfo.groupInfo.alpha = Landed ? 0 : 1;
					uiInfo.groupInfo.gameObject.SetActive(Landed ? false : true);
				}
			}

			// if building is wall, check neighbor
			if(Type == 2)
				CheckNeighbor();

			if(Landed && (goCenter != null)) {
				//SceneTown.instance.Save();
				BEUtil.SetObjectColor(goCenter, Color.white);
				BEUtil.SetObjectColor(goXInc, Color.white);
				BEUtil.SetObjectColor(goZInc, Color.white);
			}

			UpjustYByState();
			if(!SceneTown.instance.InLoading && (BEWorkerManager.instance != null))
				BEWorkerManager.instance.OnTileInfoChanged();
		}

		public void CheckNeighbor() {
			if((goXInc == null) || (goZInc == null)) return;

			// in x and y coordination, next or prev building is samely wall, then show side mesh
			Building bdNeighbor = null;
			bdNeighbor = ground.GetBuilding((int)tilePos.x-1, (int)tilePos.y);		if((bdNeighbor != null) && (bdNeighbor.Type == 2))  bdNeighbor.CheckNeighbor();
			bdNeighbor = ground.GetBuilding((int)tilePos.x+1, (int)tilePos.y);		goXInc.SetActive((Landed && (bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
			bdNeighbor = ground.GetBuilding((int)tilePos.x,   (int)tilePos.y-1);	if((bdNeighbor != null) && (bdNeighbor.Type == 2)) bdNeighbor.CheckNeighbor();
			bdNeighbor = ground.GetBuilding((int)tilePos.x,   (int)tilePos.y+1);	goZInc.SetActive(((Landed && bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
		}

		// save building info to xml format
		public void Save(XmlDocument d) {
			XmlElement ne = d.CreateElement("Building"); 					
			ne.SetAttribute("Type", Type.ToString ());							
			ne.SetAttribute("Level", Level.ToString ());							
			ne.SetAttribute("TilePosX", tilePos.x.ToString ());							
			ne.SetAttribute("TilePosY", tilePos.y.ToString ());	

			ne.SetAttribute("InUpgrade", InUpgrade.ToString ());	
			ne.SetAttribute("UpgradeCompleted", UpgradeCompleted.ToString ());	
			ne.SetAttribute("UpgradeTimeLeft", UpgradeTimeLeft.ToString ());	

			ne.SetAttribute("Production", Production.ToString ());	

			d.DocumentElement.AppendChild (ne);

			// if barrack
			if(Type == 7) {
				for(int i=0 ; i < queUnitGen.Count ; ++i) {
					GenQueItem item = queUnitGen[i];
					XmlElement neUnit = d.CreateElement("GenQue"); 					
					neUnit.SetAttribute("unitID", item.unitID.ToString ());							
					neUnit.SetAttribute("Count", item.Count.ToString ());	
					neUnit.SetAttribute("timeLeft", item.timeLeft.ToString ());	
					ne.AppendChild (neUnit);
				}
			}
			// if army camp
			else if(Type == 8) {
				for(int i=0 ; i < 10 ; ++i) {
					XmlElement neUnit = d.CreateElement("Unit"); 					
					neUnit.SetAttribute("Type", i.ToString ());							
					neUnit.SetAttribute("Count", GenUnitCount[i].ToString ());							
					ne.AppendChild (neUnit);
				}
			}
			else {}
		}

		// load building info to xml format
		public void Load(XmlElement e) {
			Type 				= int.Parse(e.GetAttribute("Type"));
			Level 				= int.Parse(e.GetAttribute("Level"));
			tilePos.x 			= float.Parse(e.GetAttribute("TilePosX"));
			tilePos.y 			= float.Parse(e.GetAttribute("TilePosY"));

			InUpgrade			= bool.Parse(e.GetAttribute("InUpgrade"));
			UpgradeCompleted	= bool.Parse(e.GetAttribute("UpgradeCompleted"));
			UpgradeTimeLeft		= float.Parse(e.GetAttribute("UpgradeTimeLeft"));

			Production			= float.Parse(e.GetAttribute("Production"));

			ground.Move(gameObject, tilePos, tileSize);
			CheckLandable();
			Land(true, false);

			for(int i=0 ; i < 10 ; ++i) {
				GenUnitCount[i] = 0;
			}

			queUnitGen.Clear();

			XmlNodeList list = e.ChildNodes;
			foreach(XmlElement ele in list) {
				if(ele.Name == "Unit")			{	
					int type = int.Parse(ele.GetAttribute("Type"));	 		
					int Count = int.Parse(ele.GetAttribute("Count"));	
					GenUnitCount[type] = Count;
				}
				else if(ele.Name == "GenQue")			{	
					int unitID = int.Parse(ele.GetAttribute("unitID"));	 		
					int Count = int.Parse(ele.GetAttribute("Count"));	
					float timeLeft = float.Parse(ele.GetAttribute("timeLeft"));	
					
					GenQueItem item = UnitGenAdd(unitID, Count);
					item.timeLeft = timeLeft;
				}
				else {}
			}
		}
		
		// when user clicked this building
		// if building has any kind of completed job
		// do complete and return true
		public bool HasCompletedWork() {

			if(UpgradeCompleted) 	{ UpgradeEnd (); return true; }
			if(Collectable) 		{ Collect (); return true; }
		
			return false;
		}

		// collect resources
		public void Collect() {

			string textColor=""; 

			// increase resource count
			if(def.eProductionType == PayType.Elixir) 	{
				SceneTown.Elixir.ChangeDelta((double)Production);
				SceneTown.instance.CapacityCheck();
				textColor = "<color=purple>";
			}
			else if(def.eProductionType == PayType.Gold) {
				SceneTown.Gold.ChangeDelta((double)Production);
				SceneTown.instance.CapacityCheck();
				textColor = "<color=orange>";
			}
			else {}

			// show collect ui to show how many resources was collected
			UICollect script = UIInGame.instance.AddInGameUI(prefUICollect, transform, new Vector3(0,1.5f,0)).GetComponent<UICollect>();
			script.Name.text = textColor+((int)Production).ToString ()+"</color>";
			script.Init(transform, new Vector3(0,1.0f,0));

			// reset values related to production
			Collectable = false;
			Production = 0;
			// hide collect dialog
			BETween.alpha(uiInfo.groupCollect.gameObject, 0.3f, 1.0f, 0.0f);
			BETween.enable(uiInfo.groupCollect.gameObject, 0.3f, true, false);
			// save game - save game when action is occured. not program quit moment
			SceneTown.instance.Save();
		}

		// whether upgrading is enable
		public bool IsUpgradeEnable() {

			BuildingType bt = TBDatabase.GetBuildingType(Type);
			// if this level is max level
			if(bt.LevelMax <= Level) return false;
			// if no next level
			if(defNext == null) return false;

			// if user don't have enough resources to upgrade
			if(((defNext.BuildGoldPrice != 0) && (SceneTown.Gold.Target() < defNext.BuildGoldPrice)) &&
			   ((defNext.BuildElixirPrice != 0) && (SceneTown.Elixir.Target() < defNext.BuildElixirPrice))) {
					return false;
			}

			return true;
		}

		// start upgrade
		public bool Upgrade() {

			if(InUpgrade) return false;
			if(bt.LevelMax <= Level) return false;
			if(defNext == null) return false;

			// check whether user has enough resources or not
			// decrease user's resource
			PayType payTypeReturn = PayforBuild(defNext);
			if(payTypeReturn != PayType.None) {
				if(payTypeReturn == PayType.Gold) 			UIDialogMessage.Show("Insufficient Gold", "Ok", "Error");
				else if(payTypeReturn == PayType.Elixir) 	UIDialogMessage.Show("Insufficient Elixir", "Ok", "Error");
				else if(payTypeReturn == PayType.Gem) 		UIDialogMessage.Show("Insufficient Gem", "Ok", "Error");
				else {}

				return false;
			}

			// prepare upgrade
			UpgradeCompleted = false;
			InUpgrade = true;

			// if upgrade time is zero(like wall) then upgrade immediately
			if(UpgradeTimeTotal > 0) {
				UpgradeTimeLeft = UpgradeTimeTotal;
				uiInfo.TimeLeft.text = BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
				uiInfo.Progress.fillAmount = (UpgradeTimeTotal-UpgradeTimeLeft)/UpgradeTimeTotal;
				BETween.alpha(uiInfo.groupProgress.gameObject, 0.3f, 0.0f, 1.0f);
				uiInfo.groupProgress.gameObject.SetActive(true);
			}
			else {
				UpgradeEnd();
			}

			return true;
		}

		// cancel current upgrade
		public void UpgradeCancel() {
			InUpgrade = false;
			UpgradeCompleted = false;

			uiInfo.groupProgress.gameObject.SetActive(false);

			RefundBuild(defNext);
			SceneTown.instance.Save();
		}

		// when upgraded ended, user clicked this building
		public void UpgradeEnd() {

			// initialize with next level
			Init(Type, Level+1);
			InUpgrade = false;
			UpgradeCompleted = false;

			// increase experience
			SceneTown.instance.GainExp(def.RewardExp);
			// if building has capacity value, then recalc capacity of all resources
			if((def.Capacity[(int)PayType.Gold] != 0) || (def.Capacity[(int)PayType.Elixir] != 0))
				SceneTown.instance.CapacityCheck();

			// save game - save game when action is occured. not program quit moment
			SceneTown.instance.Save();
		}

		// check user has enough resources to upgrade.
		// incase yes, decrease resources and return Paytype.None
		// othewise return the type of resource needed.
		public PayType PayforBuild(BuildingDef _bd) {

			PayType payTypeReturn = PayType.None;
			if((payTypeReturn == PayType.None) && (_bd.BuildGoldPrice   != 0) && (SceneTown.Gold.Target () < _bd.BuildGoldPrice)) 		payTypeReturn = PayType.Gold;
			if((payTypeReturn == PayType.None) && (_bd.BuildElixirPrice != 0) && (SceneTown.Elixir.Target () < _bd.BuildElixirPrice))	payTypeReturn = PayType.Elixir;
			if((payTypeReturn == PayType.None) && (_bd.BuildGemPrice    != 0) && (SceneTown.Gem.Target () < _bd.BuildGemPrice))			payTypeReturn = PayType.Gem;

			if(payTypeReturn == PayType.None) {
				if(_bd.BuildGoldPrice != 0) 	SceneTown.Gold.ChangeDelta(-_bd.BuildGoldPrice);
				if(_bd.BuildElixirPrice != 0) 	SceneTown.Elixir.ChangeDelta(-_bd.BuildElixirPrice);
				if(_bd.BuildGemPrice != 0) 		SceneTown.Gem.ChangeDelta(-_bd.BuildGemPrice);

				SceneTown.instance.CapacityCheck();
			}
			
			return payTypeReturn;
		}

		// when user cancel upgrade, refund half of resources
		public void RefundBuild(BuildingDef _bd) {
			if(_bd == null) return ;
			
			// when user cancel upgrade, refund half of upgrade price 
			if(_bd.BuildGoldPrice != 0) 	{ SceneTown.Gold.ChangeDelta(_bd.BuildGoldPrice/2); }
			if(_bd.BuildElixirPrice != 0) 	{ SceneTown.Elixir.ChangeDelta(_bd.BuildElixirPrice/2); }
			if(_bd.BuildGemPrice != 0) 		{ SceneTown.Gem.ChangeDelta(_bd.BuildGemPrice/2); }
		}

		// if building can training unit
		// add unit to training aue
		public GenQueItem UnitGenAdd(int unitID, int Count) {
			//Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
			// search unit que list with given unit id
			int idx = queUnitGen.FindIndex(x => x.unitID==unitID);
			if(idx == -1) {
				GenQueItem item = new GenQueItem(this, unitID, Count);
				queUnitGen.Add (item);
				//Debug.Log ("Building::UnitGenAdd create GenQueItem unitID:"+unitID.ToString());
				return item;
			}
			else {
				GenQueItem item = queUnitGen[idx];
				item.Count+=Count;
				//Debug.Log ("Building::UnitGenAdd increase GenQueItem unitID:"+unitID.ToString()+" Count:"+item.Count.ToString());
				return item;
			}
		}

		// if user cancel training unit, decrease count of training unit
		public bool UnitGenRemove(int unitID, int Count) {
			//Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
			// search unit que list with given unit id
			int idx = queUnitGen.FindIndex(x => x.unitID==unitID);
			if(idx != -1) {
				GenQueItem item = queUnitGen[idx];
				item.Count -= Count;
				//Debug.Log ("Building::UnitGenRemove decrease unitID:"+unitID.ToString()+" Count:"+item.Count.ToString());
				if(item.Count < 0) item.Count = 0;
				return false;
			}

			//Debug.Log ("Building::UnitGenRemove decrease unitID:"+unitID.ToString()+" Not Found");
			return false;
		}

		// 
		public void UnitGenUpdate(float deltaTime) {

			for(int i=0 ; i < queUnitGen.Count ; ++i) {
				GenQueItem item = queUnitGen[i];
				if(item.Count == 0) {
					//Debug.Log ("Building::UnitGenUpdate delete "+item.unitID.ToString());
					UIDialogTraining.instance.ItemRemove(item.unitID);
					queUnitGen.RemoveAt(i);
					break;
				}
			}

			while(queUnitGen.Count > 0) {
				GenQueItem item = queUnitGen[0];
				deltaTime = item.Update (deltaTime);
				if(item.Count == 0) {
					//Debug.Log ("Building::UnitGenUpdate delete "+item.unitID.ToString());
					UIDialogTraining.instance.ItemRemove(item.unitID);
					queUnitGen.RemoveAt(0);
				}

				if(deltaTime < 0.01f) 
					return;
			}
		}

		// a unit was created
		public void UnitCreated(int unitID) {
			// find army camp with space
			// create unit
			// set unit's base camp position

			GenUnitCount[unitID]++;
			//Debug.Log ("Building::UnitCreated "+unitID.ToString()+" UnitCount:"+GenUnitCount[unitID].ToString ());
		}

		// fill building info in building info dialog
		public void UIFillProgress(ProgressInfo progress, BDInfo type) {

			if(type == BDInfo.CapacityGold) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gold);
				progress.textInfo.text = "Capacity : "+Capacity[(int)PayType.Gold].ToString("#,##0")+"/"+def.Capacity[(int)PayType.Gold].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)PayType.Gold]/(float)(def.Capacity[(int)PayType.Gold]);
			}
			else if(type == BDInfo.CapacityElixir) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Elixir);
				progress.textInfo.text = "Capacity : "+Capacity[(int)PayType.Elixir].ToString("#,##0")+"/"+def.Capacity[(int)PayType.Elixir].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)PayType.Elixir]/(float)(def.Capacity[(int)PayType.Elixir]);
			}
			else if(type == BDInfo.Capacity) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
				progress.textInfo.text = "Capacity : "+Production.ToString("#,##0")+"/"+def.Capacity[(int)def.eProductionType].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Production/(float)(def.Capacity[(int)def.eProductionType]);
			}
			else if(type == BDInfo.ProductionRate) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
				progress.textInfo.text = "ProductionRate : "+((int)(def.ProductionRate*3600.0f)).ToString("#,##0")+" per Hour";
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = def.ProductionRate/defLast.ProductionRate;
			}
			else if(type == BDInfo.HitPoint) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/Heart");
				progress.textInfo.text = "HitPoints : "+def.HitPoint.ToString("#,##0")+"/"+def.HitPoint.ToString("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = (float)def.HitPoint/(float)def.HitPoint;
			}
			else if(type == BDInfo.StorageCapacity) {
				PayType payType = PayType.None;
				for(int i=0 ; i < (int)PayType.Max ; ++i) {
					if(def.Capacity[i] != 0) {
						payType = (PayType)i;
						break;
					}
				}
				if(payType == PayType.None) return;

				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(payType);
				progress.textInfo.text = "Storage Capacity : "+Capacity[(int)payType].ToString("#,##0")+"/"+def.Capacity[(int)payType].ToString ("#,##0");
				progress.imageMiddle.fillAmount = 0.0f;
				progress.imageFront.fillAmount = Capacity[(int)payType]/(float)def.Capacity[(int)payType];
			}
			else {}
		}

		// fill building upgrading info in building upgrade ask dialog
		public void UIFillProgressWithNext(ProgressInfo progress, BDInfo type) {
			
			if(type == BDInfo.CapacityGold) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gold);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)PayType.Gold].ToString ("#,##0")+"+"+(defNext.Capacity[(int)PayType.Gold]-def.Capacity[(int)PayType.Gold]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Gold]/(float)defLast.Capacity[(int)PayType.Gold];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Gold]/(float)defLast.Capacity[(int)PayType.Gold];
			}
			else if(type == BDInfo.CapacityElixir) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Elixir);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)PayType.Elixir].ToString ("#,##0")+"+"+(defNext.Capacity[(int)PayType.Elixir]-def.Capacity[(int)PayType.Elixir]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Elixir]/(float)defLast.Capacity[(int)PayType.Elixir];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Elixir]/(float)defLast.Capacity[(int)PayType.Elixir];
			}
			else if(type == BDInfo.Capacity) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
				progress.textInfo.text = "Capacity : "+def.Capacity[(int)def.eProductionType].ToString ("#,##0")+"+"+(defNext.Capacity[(int)defNext.eProductionType]-def.Capacity[(int)def.eProductionType]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)defNext.eProductionType]/(float)defLast.Capacity[(int)defLast.eProductionType];
				progress.imageFront.fillAmount = (float)def.Capacity[(int)def.eProductionType]/(float)defLast.Capacity[(int)defLast.eProductionType];
			}
			else if(type == BDInfo.ProductionRate) {
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
				progress.textInfo.text = "ProductionRate : "+((int)(def.ProductionRate*3600.0f)).ToString("#,##0")+"+"+((int)((defNext.ProductionRate-def.ProductionRate)*3600.0f)).ToString("#,##0")+" per Hour";
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : defNext.ProductionRate/defLast.ProductionRate;
				progress.imageFront.fillAmount = def.ProductionRate/defLast.ProductionRate;
			}
			else if(type == BDInfo.HitPoint) {
				progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/Heart");
				progress.textInfo.text = "HitPoints : "+def.HitPoint.ToString("#,##0")+"+"+(defNext.HitPoint-def.HitPoint).ToString("#,##0");
				progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.HitPoint/(float)defLast.HitPoint;
				progress.imageFront.fillAmount = (float)def.HitPoint/(float)defLast.HitPoint;
			}
			else if(type == BDInfo.StorageCapacity) {
				PayType payType = PayType.None;
				for(int i=0 ; i < (int)PayType.Max ; ++i) {
					if(def.Capacity[i] != 0) {
						payType = (PayType)i;
						break;
					}
				}
				if(payType == PayType.None) return;
				
				progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(payType);
				progress.textInfo.text = "Storage Capacity : "+def.Capacity[(int)payType].ToString("#,##0")+"+"+(defNext.Capacity[(int)payType]-def.Capacity[(int)payType]).ToString ("#,##0");
				progress.imageMiddle.fillAmount = defNext.Capacity[(int)payType]/(float)defLast.Capacity[(int)payType];
				progress.imageFront.fillAmount = def.Capacity[(int)payType]/(float)defLast.Capacity[(int)payType];
			}
			else {}
		}

        void  OnEvModelRotate(EventData data)
        {
            ModelRotate();
        }
        public void ModelRotate()
        {
            m_dir++;
            if (m_dir >= 4)
            {
                m_dir = 0;
            }
            //goCenter.transform.localEulerAngles += new Vector3(0, 90, 0);
            switch (m_dir)
            {
                case 0:
                    m_trans.localEulerAngles = Vector3.zero;
                    goCenter.transform.localEulerAngles = new Vector3(0,-90,0);
                    tileSize = tileSizeOri;
                    Move((int)tilePos.x, (int)tilePos.y);
                    break;
                case 1:
                    m_trans.localEulerAngles = new Vector3(0, 90, 0);
                    goCenter.transform.localEulerAngles = new Vector3(0, -90, 0);
                    tileSize.x = tileSizeOri.y;
                    tileSize.y = tileSizeOri.x;
                    Move((int)tilePos.x, (int)tilePos.y);
                    break;
                case 2:
                    m_trans.localEulerAngles = Vector3.zero;
                    goCenter.transform.localEulerAngles =  new Vector3(0, 90, 0);;
                    tileSize = tileSizeOri;
                    Move((int)tilePos.x, (int)tilePos.y);
                    break;
                case 3:
                    m_trans.localEulerAngles = new Vector3(0, 90, 0);
                    goCenter.transform.localEulerAngles = new Vector3(0, 90, 0);
                    tileSize.x = tileSizeOri.y;
                    tileSize.y = tileSizeOri.x;
                    Move((int)tilePos.x, (int)tilePos.y);
                    break;
                default:
                    break;
            }
        }

        
	}

}