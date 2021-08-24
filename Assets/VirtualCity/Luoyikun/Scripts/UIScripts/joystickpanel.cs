using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickpanel : UGUIPanel {

    public VariableJoystick m_joy;
    // Use this for initialization
    void Start () {
		
	}
	
    public override void OnOpen()
    {
        //m_joy.Reset();
    }

    public override void OnClose()
    {
        m_joy.Reset();
    }
}
