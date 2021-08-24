using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCreateOrderMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;
    /**订单支付信息*/
[ProtoMember(3)]
    public string payInfo;
[ProtoMember(4)]
    public double sMoney;
[ProtoMember(5)]
    public double money;

    public RspCreateOrderMessage() {
    }

    public RspCreateOrderMessage(int code, string tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspCreateOrderMessage(int code, string tips, string payInfo, double sMoney, double money) {
        this.code = code;
        this.tips = tips;
        this.payInfo = payInfo;
        this.sMoney = sMoney;
        this.money = money;
    }

    public double getsMoney() {
        return sMoney;
    }

    public void setsMoney(double sMoney) {
        this.sMoney = sMoney;
    }

    public double getMoney() {
        return money;
    }

    public void setMoney(double money) {
        this.money = money;
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

    public string getPayInfo() {
        return payInfo;
    }

    public void setPayInfo(string payInfo) {
        this.payInfo = payInfo;
    }
}
}