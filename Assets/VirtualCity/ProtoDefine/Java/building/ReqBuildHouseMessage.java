package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_BUILTEHOUSE)
public class ReqBuildHouseMessage extends Message {
    @Protobuf(order = 1)
    private Long houseModelId;
    @Protobuf(order = 2)
    private String LandCode;
    /**
     * 0:金币
     * 1:钻石
     */
    @Protobuf(order = 3)
    private int costDiamond;

    public ReqBuildHouseMessage() {
    }

    public ReqBuildHouseMessage(Long houseModelId, String landCode) {
        this.houseModelId = houseModelId;
        LandCode = landCode;
    }

    public Long getHouseModelId() {
        return houseModelId;
    }

    public void setHouseModelId(Long houseModelId) {
        this.houseModelId = houseModelId;
    }

    public String getLandCode() {
        return LandCode;
    }

    public void setLandCode(String landCode) {
        LandCode = landCode;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
