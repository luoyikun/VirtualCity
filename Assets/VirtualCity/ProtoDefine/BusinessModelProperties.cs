using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class BusinessModelProperties
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
 描述
*/
 [ProtoMember(4)]
private  string describe;

/**
 定值
*/
 [ProtoMember(5)]
private  string con;

/**
 属性类别:"0"用户可见属性，“1”,"用户不可见属性"
*/
 [ProtoMember(6)]
private  int? type;

/**
 是否生效
*/
 [ProtoMember(7)]
private  int? isEffective;

 [ProtoMember(8)]
private  string createtime;

 [ProtoMember(9)]
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

public string Describe
{

get {	 return describe ;	}

 set {	describe = value;	}

}

public string Con
{

get {	 return con ;	}

 set {	con = value;	}

}

public int? Type
{

get {	 return type==-999?null:type ;	}

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

