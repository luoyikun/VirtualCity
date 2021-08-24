package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_BUILTEDEVLOPMENT)
public class ReqBuildDevlopmentMessage extends Message {
    @Protobuf(order = 1)
    private Long devModelId;
    @Protobuf(order = 2)
    private String LandCode;
    /**
     * 0:金币
     * 1:钻石
     */
    @Protobuf(order = 3)
    private int costDiamond;

    public ReqBuildDevlopmentMessage() {
    }

    public ReqBuildDevlopmentMessage(Long devModelId, String landCode) {
        this.devModelId = devModelId;
        LandCode = landCode;
    }

    public Long getDevModelId() {
        return devModelId;
    }

    public void setDevModelId(Long devModelId) {
        this.devModelId = devModelId;
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
