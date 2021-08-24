using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class UserProperties
{
 [ProtoMember(1)]
private  long? id;

/**
 版本号
*/
 [ProtoMember(2)]
private  int? version;

/**
 名称
*/
 [ProtoMember(3)]
private  string name;

/**
 中文名
*/
 [ProtoMember(4)]
private  string cnName;

/**
 描述
*/
 [ProtoMember(5)]
private  string describe;

/**
 最大值
*/
 [ProtoMember(6)]
private  string max;

/**
 最小值
*/
 [ProtoMember(7)]
private  string min;

/**
 定值
*/
 [ProtoMember(8)]
private  string con;

/**
 属性类别
*/
 [ProtoMember(9)]
private  string type;

/**
 是否生效
*/
 [ProtoMember(10)]
private  int? isEffective;

 [ProtoMember(11)]
private  string createtime;

 [ProtoMember(12)]
private  string updatetime;

public long? Id
{

get {	 return id==-999?null:id ;	}

 set {	id = value;	}

}

public int? Version
{

get {	 return version==-999?null:version ;	}

 set {	version = value;	}

}

public string Name
{

get {	 return name ;	}

 set {	name = value;	}

}

public string CnName
{

get {	 return cnName ;	}

 set {	cnName = value;	}

}

public string Describe
{

get {	 return describe ;	}

 set {	describe = value;	}

}

public string Max
{

get {	 return max ;	}

 set {	max = value;	}

}

public string Min
{

get {	 return min ;	}

 set {	min = value;	}

}

public string Con
{

get {	 return con ;	}

 set {	con = value;	}

}

public string Type
{

get {	 return type ;	}

 set {	type = value;	}

}

public int? IsEffective
{

get {	 return isEffective==-999?null:isEffective ;	}

 set {	isEffective = value;	}

}

public string Createtime
{

get {	 return createtime ;	}

 set {	createtime = value;	}

}

public string Updatetime
{

get {	 return updatetime ;	}

 set {	updatetime = value;	}

}

}

}

