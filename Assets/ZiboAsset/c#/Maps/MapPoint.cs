using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public List<MapPoint> aroundMapPoints = new List<MapPoint>();//这里只做了声明没有定义
    public List<Vector2> mapVectors;

    private List<Vector2> CurrentBestChoice;
    public float CurrentBestRessult = Mathf.Infinity;

    void Start()
    {
        //MarkMapPoint(aroundMapPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MarkMapPoint(List<MapPoint> mapPoints,List<Vector2> Directions)
    {
        List<Vector2> VectorList = new List<Vector2>();//注意null是没有分配空间，不是没有元素
        Vector2 Added1 = new Vector2(1, 1);
        VectorList.Add(Added1);
        Vector2 Added2 = new Vector2(1, -1);
        VectorList.Add(Added2);
        Vector2 Added3 = new Vector2(-1, 1);
        VectorList.Add(Added3);
        Vector2 Added4 = new Vector2(-1, -1);
        VectorList.Add(Added4);
        VectorList = Directions;
       if(Directions.Count < mapPoints.Count)
        {
            Debug.Log("Not Enough Directions");
            return;
        }
        float sumLength = 0;
        int LengthCount = 0;
        //Debug.Log(VectorList[3]);
        for (int i = 0; i < mapPoints.Count; i++)
        {
            sumLength += Vector2.Distance(mapPoints[i].transform.position, transform.position);
            LengthCount += 1;
        }
        float AvgLength = sumLength / LengthCount;
        
        for (int i = 0; i < 4; i++)
        {
            VectorList[i] *=   AvgLength;
            
        }
        //Debug.Log(VectorList[2]);
        GetBestChioce(new List<Vector2>(), VectorList, LengthCount);
        mapVectors = CurrentBestChoice;
        for(int i = 0; i < mapVectors.Count; i++)
        {
            mapVectors[i] /= AvgLength;
        }


    }
    public void GetBestChioce(List<Vector2> preVectorList, List<Vector2> VectorChoiceList,int i)
    {
        
        if(i == 0)
        {
            
            float distance = ListDistance(preVectorList);
            if(distance < CurrentBestRessult)
            {
                
                CurrentBestRessult = distance;
                CurrentBestChoice = preVectorList;
            }
            if (gameObject.name == "MapPoint(1)")
            {
                Debug.Log(CurrentBestRessult);
                Debug.Log(preVectorList[0]);
                Debug.Log(preVectorList[1]);
                Debug.Log("end");

            }
            return;
            
        }
        else
        {
            
            int j = VectorChoiceList.Count;
            for (int r = 0; r < j; r++)
            {
                List<Vector2> preVectorCopy = new List<Vector2>();
                preVectorList.ForEach(i => preVectorCopy.Add(i));
                preVectorCopy.Add(VectorChoiceList[r]);
                
                List<Vector2> VectorCopy2 = new List<Vector2>();
                VectorChoiceList.ForEach(i => VectorCopy2.Add(i));
                VectorCopy2.Remove(VectorChoiceList[r]);

                GetBestChioce(preVectorCopy, VectorCopy2, i - 1);
            }
            
        }

    }
    public float ListDistance(List<Vector2> list1)
    {
        float totalDistance=0;
        for(int i = 0; i < list1.Count; i++)
        {
            totalDistance += Vector2.Distance(list1[i], aroundMapPoints[i].transform.position - transform.position);

        }
        return totalDistance;
    }
   
}
