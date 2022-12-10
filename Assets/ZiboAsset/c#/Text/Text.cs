using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Text : MonoBehaviour
{

    public float timeToLive = 1f;
    public float livingTime = 0f;
    public Vector3 direction = Vector3.up;
    public Color startColor;
    public TextMeshProUGUI text;
    public RectTransform rec;
    // Start is called before the first frame update
    private void Awake()
    {
        rec = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();//在实例化后
    }
    void Start()
    {
        //ebug.Log("start");
        rec = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
        livingTime = 0f;
        startColor = text.color;


    }

    // Update is called once per frame
    void Update()
    {
        livingTime += Time.deltaTime;
        text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - livingTime / timeToLive);
        rec.position += direction * Time.deltaTime;

        if (livingTime >= timeToLive)
        {
            Destroy(text.gameObject);
        }
    }
}
