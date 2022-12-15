using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// Some of the logic is taken away from here to the FightManager
/// Might be move back to here when further modification
/// </summary>
public class Player : MonoBehaviour
{
    /*Player's Singleton*/
    //  Since only one player.
    public static Player Instance; 
    /*Player's Data*/
    protected Dictionary<string, string> playerData;
    
    /*Player's Position information*/
    public BoardUnit PlayerActiveBoardUnit;
    
    /*Player's Component*/
    public Animator playerAnimator;
    
    
    
    public void InitPlayer(Dictionary<string, string> data)
    {
        this.playerData = data;
    }

    private void Start()
    {
        //  Fetch and Init Component.
        playerAnimator = transform.GetComponent<Animator>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayAttackAnim()
    {
        StartCoroutine(PlayerAttackAnim());
    }

    IEnumerator PlayerAttackAnim()
    {
        playerAnimator.SetBool("b_player_isAttack", true);
        yield return new WaitForSeconds(2.0f);
        playerAnimator.SetBool("b_player_isAttack", false);
    }
    
    public void PlayMoveAnim()
    {
        StartCoroutine(PlayerMoveAnim());
    }

    IEnumerator PlayerMoveAnim()
    {
        playerAnimator.SetBool("b_player_isMove", true);
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("b_player_isMove", false);
    }

    public bool isEnemyInRange(Enemy tar, int Range)
    {
        var path = MapManager.Instance.FindPath(PlayerActiveBoardUnit, tar.EnemyActiveBoardUnit);
        return path.Count <= Range;
    }
}
