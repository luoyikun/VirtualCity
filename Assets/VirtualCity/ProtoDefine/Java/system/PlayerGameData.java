package com.kingston.jforgame.server.game.system.entity;

import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.server.game.building.entity.House;
import com.kingston.jforgame.server.game.building.entity.HouseParts;
import com.kingston.jforgame.server.game.building.entity.Land;

import java.util.List;
import java.util.Map;

public class PlayerGameData {

    private volatile Map<String,Devlopments> devlopmentMap;

    private volatile Map<String, Land> landMap;

    private volatile Map<String, House> houseMap;

    private volatile List<HouseParts> housePartList;

    public Map<String, Devlopments> getDevlopmentMap() {
        return devlopmentMap;
    }

    public void setDevlopmentMap(Map<String, Devlopments> devlopmentMap) {
        this.devlopmentMap = devlopmentMap;
    }

    public Map<String, Land> getLandMap() {
        return landMap;
    }

    public void setLandMap(Map<String, Land> landMap) {
        this.landMap = landMap;
    }

    public Map<String, House> getHouseMap() {
        return houseMap;
    }

    public void setHouseMap(Map<String, House> houseMap) {
        this.houseMap = houseMap;
    }

    public List<HouseParts> getHousePartList() {
        return housePartList;
    }

    public void setHousePartList(List<HouseParts> housePartList) {
        this.housePartList = housePartList;
    }
}
