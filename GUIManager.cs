using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager
{
    public void enableGUIMouseControl(){
        //we enable the cusor to move indepdently from the FPS camera (confined to screen) & we make it cursor visible
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void disableGUIMouseControl(){
        //we disable the cursor from being able to move independently from the FPS camera (locked instead of confined) & we make the cursor invisble
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
