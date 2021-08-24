using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class JavaToCs : EditorWindow
{
    [MenuItem("DoJavaToCs/DoJavaToCs")]
    static void DoJavaToCs()
    {
        string fullPath = "Assets/VirtualCity/JavaDefine";  //路径

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".java"))
                {
                    string fileOnlyName = files[i].Name.Replace(".java", "");

                    StringBuilder tempBuilder = new StringBuilder();
                    tempBuilder.Append("using System.Collections.Generic;");
                    tempBuilder.AppendLine();
                    tempBuilder.Append("namespace ProtoDefine {");
                    tempBuilder.AppendLine();

                    var strs = File.ReadAllLines(files[i].FullName);
                    for (int lineIdx = 0; lineIdx < strs.Length; lineIdx++)
                    {
                        if (strs[lineIdx].Contains("import") || strs[lineIdx].Contains("package") || strs[lineIdx].Contains("Protobuf"))
                        {
                            continue;
                        }
                        //strs[lineIdx] = strs[lineIdx].Replace("Long", "long?");
                        //strs[lineIdx] = strs[lineIdx].Replace("Double", "double?");
                        //strs[lineIdx] = strs[lineIdx].Replace("String", "string");
                        //strs[lineIdx] = strs[lineIdx].Replace("Integer", "int?");
                        //strs[lineIdx] = strs[lineIdx].Replace("Short", "short?");
                        //strs[lineIdx] = strs[lineIdx].Replace("Map", "Dictionary");

                        tempBuilder.Append(strs[lineIdx]);
                        tempBuilder.Replace("Long", "long?");
                        tempBuilder.Replace("Double", "double?");
                        tempBuilder.Replace("String", "string");
                        tempBuilder.Replace("Integer", "int?");
                        tempBuilder.Replace("Short", "short?");
                        tempBuilder.Replace(" Map", " Dictionary");
                        tempBuilder.Replace("(Map", "(Dictionary");
                        tempBuilder.Replace("final", "");
                        tempBuilder.Replace("static", "const");
                        tempBuilder.Replace("private", "public");
                        tempBuilder.Replace("volatile", "");


                        tempBuilder.AppendLine();
                    }
                    tempBuilder.Append("}");

                    string tempFilePath = string.Format("{0}/{1}.cs", fullPath + "/CSDefine", fileOnlyName);
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                    File.WriteAllText(tempFilePath, tempBuilder.ToString());
                }
            }

            
        }

        Debug.Log("JavaToCs转换完成");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }


    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".java"))
            {
                //var strs = File.ReadAllLines(filename);//读取文件的所有行，并将数据读取到定义好的字符数组strs中，一行存一个单元
                //for (int i = 0; i < strs.Length; i++)
                //{
                //    m_Str += strs[i];//读取每一行，并连起来
                //    m_Str += "\n";//每一行末尾换行
                //}
            }
            
        }
        foreach (string dir in dirs)
        {
            //paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

}
