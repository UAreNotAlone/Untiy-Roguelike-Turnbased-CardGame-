using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClip : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cardClipposition;
    private void Start()
    {
        cardClipposition = this.transform;
    }
}
