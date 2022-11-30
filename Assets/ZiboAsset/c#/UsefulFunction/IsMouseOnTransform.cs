using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMouseOnTransform : MonoBehaviour
{
    public static bool IsMouseOnThisTransform(Transform trans)
    {
        Vector3 mousePos = Input.mousePosition;//注意此步骤获取的值是屏幕坐标
        Vector3 RealMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(RealMousePos);//这里的坐标z统统为-10！()
        RealMousePos = new Vector3(RealMousePos.x, RealMousePos.y, trans.transform.position.z);

        if (Vector3.Distance(trans.transform.position, RealMousePos) < 10)
        {

            return true;
        }
        else
        {
            return false;
        }
    }
}
