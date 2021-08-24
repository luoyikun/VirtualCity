using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspBuildInfoMessage {
[ProtoMember(1)]
    public Dictionary<string,Land> landMap;
[ProtoMember(2)]
    public Dictionary<string,Devlopments> devlopmentsMap;
[ProtoMember(3)]
    public Dictionary<string,House> housesMap;
[ProtoMember(4)]
    public int code;
[ProtoMember(5)]
    public string tips;
[ProtoMember(6)]
    public long zan;


    public Dictionary<string, Land> getLandMap() {
        return landMap;
    }

    public void setLandMap(Dictionary<string, Land> landMap) {
        this.landMap = landMap;
    }

    public Dictionary<string, Devlopments> getDevlopmentsMap() {
        return devlopmentsMap;
    }

    public void setDevlopmentsMap(Dictionary<string, Devlopments> devlopmentsMap) {
        this.devlopmentsMap = devlopmentsMap;
    }

    public Dictionary<string, House> getHousesMap() {
        return housesMap;
    }

    public void setHousesMap(Dictionary<string, House> housesMap) {
        this.housesMap = housesMap;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
        this.tips = tips;
    }

    public long getZan() {
        return zan;
    }

    public void setZan(long zan) {
        this.zan = zan;
    }
}
}