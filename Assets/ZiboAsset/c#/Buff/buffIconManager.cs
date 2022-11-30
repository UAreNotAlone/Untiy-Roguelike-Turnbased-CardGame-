using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class buffIconManager : MonoBehaviour
{
    public List<Transform> buffs = new List<Transform>();
    public Transform buffTransform;
    public float buffDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        buffTransform = GameObject.Find("BuffTransform").transform;

    }

   public void RemoveBuff(Transform buff)
    {
        buffs.Remove(buff);
        Destroy(buff.gameObject);
        ReArrangeBuff(buffs);
    }
    public void AddBuff(Transform buff)
    {
        buffs.Add(buff);
        ReArrangeBuff(buffs);
    }

    public void ReArrangeBuff(List<Transform> Buffs)
    {
        for(int i = 0; i < Buffs.Count; i++)
        {
            Buffs[i].GetComponent<Buff>().GotoGradually(buffTransform.position + i * buffDistance * Vector3.up);
        }
    }
}
