using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]




public class Goods {





[ProtoMember(1)]
	public long? id;

	/**
	 商品名称
	 */
[ProtoMember(2)]
	public string name;

	/**
	 商品描述图片地址
	 */
[ProtoMember(3)]
	public string infoPicture;

	/**
	 商品描述文字信息
	 */
[ProtoMember(4)]
	public string infoText;

	/**
	 商品类别
	 */
[ProtoMember(5)]
	public string type;

	/**
	 商铺id
	 */
[ProtoMember(6)]
	public long shopsId;

	/**
	 封面图片地址
	 */
[ProtoMember(7)]
	public string coverPicture;

	/**
	 计量单位
	 */
[ProtoMember(8)]
	public string unit;

	/**
	 版本
	 */
[ProtoMember(9)]
	public int version;

	/**
	 是否上架“0”未上架，“1”已上架
	 */
[ProtoMember(10)]
	public int isPublic;

	/**
	 商家id
	 */
[ProtoMember(11)]
	public long businessId;

	/**
	 商家名称
	 */
[ProtoMember(12)]
	public string businessName;

	/**
	 最低价格
	 */
[ProtoMember(13)]
	public double priceMin;

	/**
	 总销量
	 */
[ProtoMember(14)]
	public int salesTotle;

	/**
	 月销量
	 */
[ProtoMember(15)]
	public int salesMonth;

	/**
	 商品满意度：一颗星 0分，两颗星25分，三颗星50分，四颗星75分，五颗星100分
	 */
[ProtoMember(16)]
	public double commnetScore;

	/**
	 评论总数
	 */
[ProtoMember(17)]
	public int commentTotle;

	/**
	 是否有模型部件
	 */
[ProtoMember(18)]
	public int hasMoudle;

[ProtoMember(19)]
	public string createtime;

[ProtoMember(20)]
	public string updatetime;

	public long? getId() {
		return id;
	}

	public void setId(long? id) {
		this.id = id;
	}

	public string getName() {
		return name;
	}

	public void setName(string name) {
		this.name = name;
	}

	public string getInfoPicture() {
		return infoPicture;
	}

	public void setInfoPicture(string infoPicture) {
		this.infoPicture = infoPicture;
	}

	public string getInfoText() {
		return infoText;
	}

	public void setInfoText(string infoText) {
		this.infoText = infoText;
	}

	public string getType() {
		return type;
	}

	public void setType(string type) {
		this.type = type;
	}

	public long getShopsId() {
		return shopsId;
	}

	public void setShopsId(long shopsId) {
		this.shopsId = shopsId;
	}

	public string getCoverPicture() {
		return coverPicture;
	}

	public void setCoverPicture(string coverPicture) {
		this.coverPicture = coverPicture;
	}

	public string getUnit() {
		return unit;
	}

	public void setUnit(string unit) {
		this.unit = unit;
	}

	public int getVersion() {
		return version;
	}

	public void setVersion(int version) {
		this.version = version;
	}

	public int getIsPublic() {
		return isPublic;
	}

	public void setIsPublic(int isPublic) {
		this.isPublic = isPublic;
	}

	public long getBusinessId() {
		return businessId;
	}

	public void setBusinessId(long businessId) {
		this.businessId = businessId;
	}

	public string getBusinessName() {
		return businessName;
	}

	public void setBusinessName(string businessName) {
		this.businessName = businessName;
	}

	public double getPriceMin() {
		return priceMin;
	}

	public void setPriceMin(double priceMin) {
		this.priceMin = priceMin;
	}

	public int getSalesTotle() {
		return salesTotle;
	}

	public void setSalesTotle(int salesTotle) {
		this.salesTotle = salesTotle;
	}

	public int getSalesMonth() {
		return salesMonth;
	}

	public void setSalesMonth(int salesMonth) {
		this.salesMonth = salesMonth;
	}

	public double getCommnetScore() {
		return commnetScore;
	}

	public void setCommnetScore(double commnetScore) {
		this.commnetScore = commnetScore;
	}

	public int getCommentTotle() {
		return commentTotle;
	}

	public void setCommentTotle(int commentTotle) {
		this.commentTotle = commentTotle;
	}

	public int getHasMoudle() {
		return hasMoudle;
	}

	public void setHasMoudle(int hasMoudle) {
		this.hasMoudle = hasMoudle;
	}

	public string getCreatetime() {
		return createtime;
	}

	public void setCreatetime(string createtime) {
		this.createtime = createtime;
	}

	public string getUpdatetime() {
		return updatetime;
	}

	public void setUpdatetime(string updatetime) {
		this.updatetime = updatetime;
	}


}


}