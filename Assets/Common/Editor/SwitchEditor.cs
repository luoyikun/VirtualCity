using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchEditor {

     
    

    // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
    


    //[MenuItem(AppConst.m_switchMenuInOrOut)]
    //public static void ToggleSimulationMode()
    //{
    //    AppConst.IsOutEditor = !AppConst.IsOutEditor;
    //}

    [MenuItem(AppConst.m_switchMenuIn)]
    public static bool SwitchMenuIn()
    {
        Menu.SetChecked(AppConst.m_switchMenuIn, true);
        Menu.SetChecked(AppConst.m_switchMenuOutTest, false);
        Menu.SetChecked(AppConst.m_switchMenuOut, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuIn, true);
        EditorPrefs.SetBool(AppConst.m_switchMenuOutTest, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuOut, false);
        return true;
    }

    [MenuItem(AppConst.m_switchMenuOut)]
    public static bool SwitchMenuOut()
    {
        Menu.SetChecked(AppConst.m_switchMenuIn, false);
        Menu.SetChecked(AppConst.m_switchMenuOutTest, false);
        Menu.SetChecked(AppConst.m_switchMenuOut, true);
        EditorPrefs.SetBool(AppConst.m_switchMenuIn, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuOutTest, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuOut, true);
        return true;
    }

    [MenuItem(AppConst.m_switchMenuOutTest)]
    public static bool SwitchMenuOutTest()
    {
        Menu.SetChecked(AppConst.m_switchMenuIn, false);
        Menu.SetChecked(AppConst.m_switchMenuOutTest, true);
        Menu.SetChecked(AppConst.m_switchMenuOut, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuIn, false);
        EditorPrefs.SetBool(AppConst.m_switchMenuOutTest, true);
        EditorPrefs.SetBool(AppConst.m_switchMenuOut, false);
        return true;
    }
}
