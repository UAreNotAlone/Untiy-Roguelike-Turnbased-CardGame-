using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Basier Line
public class LineUI : UIBase
{

    public void SetLineStartPosition(Vector2 pos)
    {
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
    }


    public void SetLineEndPosition(Vector2 pos)
    {
        transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = pos;
        
        //  Draw The Bezier -> Encap it to func
        Vector3 start = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        Vector3 end = pos;
        Vector3 mid = Vector3.zero;
        mid.x = start.x;
        mid.y = (start.y + end.y) * 0.5f;
        //  Dir between start and end
        Vector3 dir = (end - start).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        //  Set the degree of the end pointer
        transform.GetChild(transform.childCount - 1).eulerAngles = new Vector3(0, 0, angle - 90);
        
        //  Set the curve
        for (int i = transform.childCount - 1; i >= 0  ; i--)
        {
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition =
                GetBezierCurve(start, mid, end, i / (float)transform.childCount);

            if (i != transform.childCount - 1)
            {
                dir = (transform.GetChild(i + 1).GetComponent<RectTransform>().anchoredPosition -
                       transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition).normalized;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.GetChild(i).eulerAngles = new Vector3(0, 0, angle - 90);
            }
        }

    }

    public Vector3 GetBezierCurve(Vector3 start, Vector3 mid, Vector3 end, float t)
    {
        return (1.0f - t) * (1.0f - t) * start + 2.0f * t * (1 - t) * mid + t * t * end;
    }
}
