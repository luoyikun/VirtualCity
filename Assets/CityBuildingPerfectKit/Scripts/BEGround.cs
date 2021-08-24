using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEGround
///   Description:    class about tiled map for create and manage buildings 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BEGround : MonoBehaviour {

		public	static BEGround instance;
		
		public 	Vector2 		UnitSize = Vector2.one;	// actual width, height size of one tile
		public 	Vector2 		GridSize;				// map size 
		public 	int 			SubGridSize = 2;		// how many sub tiles in on tile (sub tiles are used when AStar path finding)
		public	BEAStar			AStar;					// a star path finding class
		public	BEWorkerManager	WorkerManager;			// control worker (build building, wondering map)
		public	Transform 		trDecoRoot = null;

		public 	Building [,] 			Cells;			// 2-dimensional array of each tile(cell)
		public 	List<List<Building>>	Buildings = new List<List<Building>>();	// array of buildings categorized by building type

		void Awake () {
			instance=this;

			// initialize cells
			Cells = new Building[(int)GridSize.x,(int)GridSize.y];
			for(int y=0 ; y < (int)GridSize.y ; ++y) {
				for(int x=0 ; x < (int)GridSize.x ; ++x) {
					Cells[x,y] = null;
					// if cell is blank(not occupied) value is null
					// otherwise, cell has script of occupying building
				}
			}

			// assume max building type count to 20
			for(int i=0 ; i < 20 ; ++i) {
				Buildings.Add (new List<Building>());
			}

			// initialzize A Star map
			AStar.Init((int)GridSize.x*SubGridSize, (int)GridSize.y*SubGridSize, 1.0f/(float)SubGridSize);
		}

		void Start () {
		}
		
		void Update () {
		}

		// get AStar tile from building script
		public AStarTile GetBuidingAStarTile(Building building) {
			int x = (int)building.tilePos.x * BEGround.instance.SubGridSize;
			int y = (int)building.tilePos.y * BEGround.instance.SubGridSize;
			return AStar.tiles[x,y];
		}

		// get boundary of map
		public Vector2 GetBorder(Vector2 tileSize) {
			Vector2 vReturn = Vector2.zero;
			vReturn.x = (GridSize.x-tileSize.x) * -0.5f * UnitSize.x;
			vReturn.y = (GridSize.y-tileSize.y) * -0.5f * UnitSize.y;
			return vReturn;
		}

		// ove gameobject to given tilepos and tile size(1x1,2x2,...)
		public void Move(GameObject go, Vector2 tilePos, Vector2 tileSize) {
			Vector2 border = GetBorder(tileSize);
			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;
			
			if(go != null) {
				//Debug.Log ("BEGround Tile:"+tilePos.x.ToString ()+","+tilePos.y.ToString());
				Vector3 localPos = Vector3.zero;
				localPos.x = border.x+(int)(tilePos.x+0.5f)*UnitSize.x;
				localPos.y = 0.01f;
				localPos.z = border.y+(int)(tilePos.y+0.5f)*UnitSize.y;
				go.transform.position = localPos;
				//go.transform.rotation = transform.rotation;
			}
		}

		// get tilepos(x,y coordinate index) from actual position ansd tilesize(1x1,2x2,...)
		public Vector2 GetTilePos(Vector3 vTarget, Vector2 tileSize) {
			Vector2 tilePos = Vector2.zero;
			Vector3 posLocal = vTarget;//transform.InverseTransformPoint(pos);
			Vector2 border = GetBorder(tileSize);
			posLocal.x = Mathf.Clamp(posLocal.x, border.x, -border.x);
			posLocal.z = Mathf.Clamp(posLocal.z, border.y, -border.y);
			tilePos.x = (int)(posLocal.x - border.x)/(int)UnitSize.x;
			tilePos.y = (int)(posLocal.z - border.y)/(int)UnitSize.y;
			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;

			return tilePos;
		}

		// get actual position from tilepos(x,y coordinate index)
		public Vector3 TilePosToWorldPos(Vector2 tilePos) {

			Vector2 tileSize = Vector2.one;
			Vector2 border = GetBorder(tileSize);
			if(tilePos.x > GridSize.x-tileSize.x) tilePos.x = GridSize.x-tileSize.x;
			if(tilePos.y > GridSize.y-tileSize.y) tilePos.y = GridSize.y-tileSize.y;
			
			Vector3 localPos = Vector3.zero;
			localPos.x = border.x+(int)(tilePos.x+0.5f)*UnitSize.x;
			localPos.y = 0.01f;
			localPos.z = border.y+(int)(tilePos.y+0.5f)*UnitSize.y;	

			return localPos;
		}

		// move building to proper position 
		// which has enough blank tiles to cover building's tilesize and nearest from current screen center
		// this function used when new building is created, set initial position to building
		public bool MoveToVacantTilePos(Building bd) {
			List<Vector2> posTile = new List<Vector2>();

			for(int y=0 ; y < (int)GridSize.y ; ++y) {
				for(int x=0 ; x < (int)GridSize.x ; ++x) {

					bool bOccupied = false;
					for(int v=0 ; v < (int)bd.tileSize.y ; ++v) {
						for(int u=0 ; u < (int)bd.tileSize.x ; ++u) {

							if(x+u >= (int)GridSize.x) continue;
							if(y+v >= (int)GridSize.y) continue;
							if(Cells[x+u,y+v] != null) {
								bOccupied = true;
								break;
							}
						}

						if(bOccupied) break;
					}

					if(bOccupied) {
						continue;
					}
					else {
						// has enough blank tiles cover builsing's tile size
						posTile.Add(new Vector2(x,y));
					}
				}
			}

			// no vacant tilepos found
			if(posTile.Count == 0)
				return false;

			// get xzplane position with camera raycast 
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			float enter;
			SceneTown.instance.xzPlane.Raycast(ray, out enter);
			Vector3 vTarget = ray.GetPoint(enter);
			Vector2 TileCameraCenter = GetTilePos(vTarget, bd.tileSize);

			// sort nearest tile from screen center
			int iFind = -1;
			float fDistMin = 0.0f;
			for(int j=0 ; j < posTile.Count ; ++j) {
				float fDist = Vector2.Distance(posTile[j], TileCameraCenter);
				if((j==0) || (fDist < fDistMin)) {
					iFind = j;
					fDistMin = fDist;
				}
			}

			//iFind = 0;
			// sort listed point from camera canter to plane intersection
			bd.tilePos = posTile[iFind];
			Move(bd.gameObject, bd.tilePos, bd.tileSize);

			// if new building is outside of frustrum
			// move camera to show building
			Vector3 vPosCamera = TilePosToWorldPos(bd.tilePos);
			GameObject.Find ("CameraRoot").transform.position = vPosCamera;
			return true;
		}

		//check tile is vacant with given tilepos and tilesize
		public bool IsVacant(Vector2 tilePos, Vector2 tileSize) {
			for(int y=0 ; y < (int)tileSize.y ; ++y) {
				for(int x=0 ; x < (int)tileSize.x ; ++x) {
					if(Cells[(int)tilePos.x+x,(int)tilePos.y+y] != null)
						return false;
				}
			}

			return true;
		}

		// write tile cell was occupied by building
		public void OccupySet(Building bd) {
			for(int y=0 ; y < (int)bd.tileSize.y ; ++y) {
				for(int x=0 ; x < (int)bd.tileSize.x ; ++x) {
					Cells[(int)bd.tilePos.x+x,(int)bd.tilePos.y+y] = bd.Landed ? bd : null;

					//set subtile value for AStar map
					if(AStar.tiles != null) {
						int asx = SubGridSize * ((int)bd.tilePos.x+x);
						int asy = SubGridSize * ((int)bd.tilePos.y+y);
						for(int yy=0 ; yy < SubGridSize ; ++yy) {
							for(int xx=0 ; xx < SubGridSize ; ++xx) {

								// like COC, worker moves through wall, so wall building not set it's tile occupation info to AStar
								if((bd.Type == 2) || ((x==0) && (xx==0)) || ((x==(int)bd.tileSize.x-1) && (xx==SubGridSize-1)) || ((y==0) && (yy==0)) || ((y==(int)bd.tileSize.y-1) && (yy==SubGridSize-1)))
									AStar.tiles[asx+xx,asy+yy].type = 0;
								else
									AStar.tiles[asx+xx,asy+yy].type = bd.Landed ? 1 : 0;
							}
						}
					}
				}
			}
		}

		// get building with tile x, y coorsinate index
		public Building GetBuilding(int x, int y) {
			if((x<0) || ((int)GridSize.x<=x)) return null;
			if((y<0) || ((int)GridSize.y<=y)) return null;

			return Cells[x, y];
		}

		// list whole buildinge created.
		// used when debugging
		public void BuildingListing() {
			Debug.Log ("BEGround::BuildingListing");
			for(int i=0 ; i < 10 ; ++i) {
				for(int j=0 ; j < Buildings[i].Count ; ++j) {
					Debug.Log ("Building Type:"+i.ToString ()+ " "+Buildings[i][j].gameObject.name);
				}
			}
		}

		// get building count with given building type
		public int GetBuildingCount(int BuildingType) {
			return Buildings[BuildingType].Count;
		}

		// get max count of given building type from database
		public int GetBuildingCountMax(int BuildingType) {
			BuildingType 	bt = TBDatabase.GetBuildingType(BuildingType);
			Building 		buildingTown = Buildings[0][0];
			return bt.MaxCount[buildingTown.Level-1];
		}

		// create new building with type and level
		public Building BuildingAdd(int type, int level) {
			//Debug.Log ("BEGround::BuildingAdd");

			// if previous selected building is exist, unselect that building
			// because newly created building must be in selection state
			if(SceneTown.buildingSelected != null) {
				SceneTown.instance.BuildingLandUnselect();
			}

			// create building base from resource
			// each buildings are combination of buildingbase and building mesh
			GameObject goBuildingBase = Resources.Load ("Prefabs/Building/BuildingBase") as GameObject;
			GameObject go = (GameObject)Instantiate(goBuildingBase, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (trDecoRoot);

			Building script = go.GetComponent<Building>();
			script.ground = this;
			// initialize building
			script.Init (type, level);

			// add building to array
			Buildings[type].Add (script);

			return script;
		}

		// remove building
		public void BuildingRemove (Building script) {
			//Debug.Log ("BEGround::BuildingRemove");
			int idx = Buildings[script.Type].FindIndex(x => x==script);
			Debug.Log ("idx:"+idx.ToString());
			if(idx != -1) {
				Buildings[script.Type].RemoveAt(idx);
			}
			//BuildingListing();
		}

		// get total resource capacity with given resource type
		public int GetCapacityTotal(PayType type) {
			int iReturn = 0;
			for(int i=0 ; i < 10 ; ++i) {
				for(int j=0 ; j < Buildings[i].Count ; ++j) {

					// exclude in creation building
					if(Buildings[i][j].Level == 0) continue;

					// exclude production building such as gold mine, because gols mine has it's own capacity 
					if(Buildings[i][j].def.eProductionType == type) continue;

					iReturn += Buildings[i][j].def.Capacity[(int)type];
				}
			}

			return iReturn;
		}

		// when user collect resources from resource generation buildings(like gold mine, elixir extracter)
		// distribute resources to storage buildings by their capacity ratio
		public void DistributeByCapacity(PayType type, float value) {

			// find all storage buildings can store given resourcetype
			int CapacitySum = 0;
			List<Building> capacitylist = new List<Building>();

			for(int i=0 ; i < 10 ; ++i) {
				for(int j=0 ; j < Buildings[i].Count ; ++j) {
					
					// exclude in creation building
					if(Buildings[i][j].Level == 0) continue;
					
					// exclude production building such as gold mine, because gols mine has it's own capacity 
					if(Buildings[i][j].def.eProductionType == type) continue;

					capacitylist.Add (Buildings[i][j]);
					CapacitySum += Buildings[i][j].def.Capacity[(int)type];
				}
			}

			//distribute resources
			float ValueStart = value;
			for(int i=0 ; i < capacitylist.Count ; ++i) {

				float SetValue = 0.0f;

				// in normal case, add resource with their capacity ratio
				if(i != capacitylist.Count-1) {
					float fRatio = (float)(capacitylist[i].def.Capacity[(int)type])/(float)CapacitySum;
					SetValue = value * fRatio;
					ValueStart -= SetValue;
				}
				// if it is last storage, give all rest resources
				else {
					SetValue = ValueStart;
				}

				capacitylist[i].Capacity[(int)type] = SetValue;
			}
		}

		// when game started, set workers to working(upgrading)buildings
		public void SetWorkingBuildingWorker() {
			for(int i=0 ; i < 10 ; ++i) {
				for(int j=0 ; j < Buildings[i].Count ; ++j) {
					
					// exclude if building is not working
					if(!Buildings[i][j].InUpgrading()) continue;

					// get free worker
					BEWorker Worker = BEWorkerManager.instance.GetAvailableWorker();
					if(Worker != null) {

						AStarTile tile = GetBuidingAStarTile(Buildings[i][j]);
						// set worker position to building
						Worker.SetPosition(tile.x, tile.y);
						// set worker state to work
						Worker.SetWork(Buildings[i][j]);
					}
				}
			}

		}
	}

}