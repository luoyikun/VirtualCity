package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_BUYHOUSEPART)
public class ReqBuyHousePartMessage extends Message {
    @Protobuf(order = 1)
    private long housePartsId;
    @Protobuf(order = 2)
    private int number;
    /**
     * 0:金币
     * 1:钻石
     */
    @Protobuf(order = 3)
    private int costDiamond;

    public long getHousePartsId() {
        return housePartsId;
    }

    public void setHousePartsId(long housePartsId) {
        this.housePartsId = housePartsId;
    }

    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
