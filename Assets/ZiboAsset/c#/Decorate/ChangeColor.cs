using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Color[] colors;
    public float frameNeededTOChangeColor = 60;
    public float rateOfChangeColor = 1;

    private Color nowColor;
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (true)
        {
            Color newColor = colors[((int)(i /frameNeededTOChangeColor)) % colors.Length];

            Color nowColor = this.gameObject.GetComponent<Image>().color;
            nowColor.r = Mathf.Lerp(nowColor.a, newColor.r, rateOfChangeColor * Time.deltaTime);
            nowColor.b = Mathf.Lerp(nowColor.a, newColor.b, rateOfChangeColor * Time.deltaTime);
            nowColor.g = Mathf.Lerp(nowColor.a, newColor.g, rateOfChangeColor * Time.deltaTime);
            GetComponent<Image>().color = nowColor;


            i = (i + 1);
        }
        
    }
    
}
