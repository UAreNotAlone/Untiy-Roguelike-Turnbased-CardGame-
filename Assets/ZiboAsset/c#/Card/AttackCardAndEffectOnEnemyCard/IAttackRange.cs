using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackRange 
{
    // Start is called before the first frame update
    public bool isInAttackRange(MapPoint mapPoint,Character chara);
}
