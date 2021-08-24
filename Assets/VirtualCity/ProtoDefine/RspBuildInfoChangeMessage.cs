using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspBuildInfoChangeMessage {
    /**消息类型，房屋数据，建筑数据，房屋部件数据和土地数据*/
[ProtoMember(1)]
    public int infoType;
[ProtoMember(2)]
    /**消息的json*/
    public string info;
    /**金币*/
[ProtoMember(3)]
    public int gold;
    /**钻石*/
[ProtoMember(4)]
    public int diamond;
    /**成功*/
[ProtoMember(5)]
    public int code;
    /**提示*/
[ProtoMember(6)]
    public string tip;

    public RspBuildInfoChangeMessage() {
    }



    public int getInfoType() {
        return infoType;
    }

    public void setInfoType(int infoType) {
        this.infoType = infoType;
    }

    public string getInfo() {
        return info;
    }

    public void setInfo(string info) {
        this.info = info;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiamond() {
        return diamond;
    }

    public void setDiamond(int diamond) {
        this.diamond = diamond;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }

    public RspBuildInfoChangeMessage(int infoType, string info, int gold, int diamond, int code, string tip) {
        this.infoType = infoType;
        this.info = info;
        this.gold = gold;
        this.diamond = diamond;
        this.code = code;
        this.tip = tip;
    }

    public string tostring() {
        return "RspBuildInfoChangeMessage{" +
                "infoType=" + infoType +
                ", info='" + info + '\'' +
                ", gold=" + gold +
                ", diamond=" + diamond +
                ", code=" + code +
                ", tip='" + tip + '\'' +
                '}';
    }
}
}