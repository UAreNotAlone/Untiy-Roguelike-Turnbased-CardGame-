using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{


    public OverlayTile playerActiveTile;
    public Animator player_animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player_animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
