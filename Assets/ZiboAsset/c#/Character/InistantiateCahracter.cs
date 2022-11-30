using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InistantiateCahracter :MonoBehaviour
{
    public List<OverlayTile> overlays = null;
    public GameObject gameObj;
    public Transform trans;
    public bool begin;
    public BattleGameMaster battleMaster;
    public void Start()
    {
        battleMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        StartCoroutine(ContinueInitial());
    }

    IEnumerator ContinueInitial()
    {
        overlays =new List<OverlayTile> (GameObject.Find("OverlayContainer").GetComponentsInChildren<OverlayTile>());
        while (overlays == null)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("fail to find overlays by Init");
            overlays = new List<OverlayTile>(GameObject.Find("OverlayContainer").GetComponentsInChildren<OverlayTile>());
        }
        yield return 0;
    }
    // Start is called before the first frame update
    public void Init(GameObject p_prefab,Transform p_trans)
    {
        
        gameObj = p_prefab;
        begin = true;
        trans = p_trans;
        
    }

    public void Update()
    {
        if( begin == true && overlays != null)
        {

            if(trans == null)
            {
                Debug.Log("null tranform");
                trans = overlays[UnityEngine.Random.Range(0, overlays.Count)].transform;
            }

                float currentSmallestDistance = Mathf.Infinity;
                OverlayTile currentClosetOlt = null;
                foreach (OverlayTile olt in overlays)
                {
                    if (Vector2.Distance(gameObj.transform.position, olt.transform.position) < currentSmallestDistance)
                    {
                        currentSmallestDistance = Vector2.Distance(trans.position, olt.transform.position);
                        currentClosetOlt = olt;
                    //Debug.Log(currentSmallestDistance);
                    
                    }
                }
            //Debug.Log(currentClosetOlt.transform.position);
            //Debug.Log(gameObj.transform.position);
                //Debug.Log(currentSmallestDistance);
            
                gameObj.GetComponent<Character>().onMapPoint = currentClosetOlt.transform.GetComponent<MapPoint>();
                //Instantiate(prefab, currentClosetOlt.transform.position, currentClosetOlt.transform.rotation);
                Destroy(gameObject);
           
            

        }
        
    }
}
