using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DoubleClickControl : MonoBehaviour
{
    public static DoubleClickControl instance;
    private void Awake(){instance = this;}


    float LeftButtonCooler = 0.5f;
    float LeftButtonCount = 0;
    public bool LeftDoubleClick;

    float RightButtonCooler = 0.5f;
    float RightButtonCount = 0;
    public bool RightDoubleClick;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            if (LeftButtonCooler > 0 && LeftButtonCount == 1)
                LeftDoubleClick = true;
            else
            {
                LeftButtonCooler = 0.5f;
                LeftButtonCount += 1;
            }

        if (LeftButtonCooler > 0)
            LeftButtonCooler -= 1 * Time.deltaTime;
        else
            LeftButtonCount = 0;

        if (Input.GetKeyUp(KeyCode.A))
            LeftDoubleClick = false;

        //right
        if (Input.GetKeyDown(KeyCode.D))
            if (RightButtonCooler > 0 && RightButtonCount == 1)
                RightDoubleClick = true;
            else
            {
                RightButtonCooler = 0.5f;
                RightButtonCount += 1;
            }

        if (RightButtonCooler > 0)
            RightButtonCooler -= 1 * Time.deltaTime;
        else
            RightButtonCount = 0;

        if (Input.GetKeyUp(KeyCode.D))
            RightDoubleClick = false;
    }

}
