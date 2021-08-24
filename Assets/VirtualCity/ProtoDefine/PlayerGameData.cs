using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class PlayerGameData {

    public  Dictionary<string,Devlopments> devlopmentMap;

    public  Dictionary<string, Land> landMap;

    public  Dictionary<string, House> houseMap;

    public  List<HouseParts> housePartList;

    public Dictionary<string, Devlopments> getDevlopmentMap() {
        return devlopmentMap;
    }

    public void setDevlopmentMap(Dictionary<string, Devlopments> devlopmentMap) {
        this.devlopmentMap = devlopmentMap;
    }

    public Dictionary<string, Land> getLandMap() {
        return landMap;
    }

    public void setLandMap(Dictionary<string, Land> landMap) {
        this.landMap = landMap;
    }

    public Dictionary<string, House> getHouseMap() {
        return houseMap;
    }

    public void setHouseMap(Dictionary<string, House> houseMap) {
        this.houseMap = houseMap;
    }

    public List<HouseParts> getHousePartList() {
        return housePartList;
    }

    public void setHousePartList(List<HouseParts> housePartList) {
        this.housePartList = housePartList;
    }
}
}