using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;



public enum EnemyActionType
{
    None,
    Defense,
    Attack,
    Move,
    Buff,   //  Might not be used 
}


public class Enemy : MonoBehaviour
{
    //  Enemy's Original Data
    protected Dictionary<string, string> enemyData;
    //增加：降低的攻击力属性
    public int decreasedAttack = 0;
    //  Enemy's Position information
    public BoardUnit EnemyActiveBoardUnit;
    /*Enemy UI*/
    public EnemyActionType actionType;
    //  Enemy's UI obj
    public GameObject actionIcon_obj;
    public GameObject hpItem_obj;
    //  Enemy's UI tf.
    public Transform attackIcon_tf;
    public Transform defenseIcon_tf;
    public Transform buffIcon_tf;
    public Transform moveIcon_tf;
    //  Enemy's UI txt
    public Text hpTxt;
    public Text defendTxt;
    public Image hpImg;
    
    
    /*Enemy Own Var*/
    public int enemyDefenseValue;
    public int enemyAttackValue;
    public int enemyMAXHP;
    public int enemyCurHP;
    private List<BoardUnit> _currentPath;

    /*Enemy Own Component*/
    private SpriteRenderer _enemySelection_sr;
    public Animator enemyAnimator;
    public void InitEnemy(Dictionary<string, string> data)
    {
        //  I think u can derive everything from it
        //  which can make it more clear
        this.enemyData = data;
        

    }

    private void Start()
    {
        //  Fetch the component
        _enemySelection_sr = transform.Find("SelectionUI").GetComponent<SpriteRenderer>();
        
        
        //  Load the Enemy related UI element here.
        actionIcon_obj = UIManager.Instance.LoadActionIcon();
        hpItem_obj = UIManager.Instance.LoadHpItem();
        //  tf
        attackIcon_tf = actionIcon_obj.transform.Find("attack");
        defenseIcon_tf = actionIcon_obj.transform.Find("defense");
        buffIcon_tf = actionIcon_obj.transform.Find("fire");
        moveIcon_tf = actionIcon_obj.transform.Find("move");
        //  txt and img
        defendTxt = hpItem_obj.transform.Find("fangyu/Text").GetComponent<Text>();
        hpTxt = hpItem_obj.transform.Find("hpTxt").GetComponent<Text>();
        hpImg = hpItem_obj.transform.Find("fill").GetComponent<Image>();

        _currentPath = new List<BoardUnit>();
        
        
        actionType = EnemyActionType.None;
        
        //  Set the position of the enemy related UI
        hpItem_obj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("HpItemHolder").position + Vector3.down*0.2f);
        actionIcon_obj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("ActionIconHolder").position + Vector3.up);
        
        //  you might want to disable them.
        
        
        SetRandomAction();
        Debug.Log(actionType);
        
        //  Init Enemy's own var from Enemy's data.
        enemyAttackValue = int.Parse(enemyData["Attack"]);
        enemyDefenseValue = int.Parse(enemyData["Defend"]);
        enemyMAXHP = int.Parse(enemyData["Hp"]);
        enemyCurHP = enemyMAXHP;
        
        //  Update the UI information
        UpdataEnemyDefense();
        UpdateEnemyHp();
        
        


    }


    public void SetRandomAction()
    {
        int ran_idx = UnityEngine.Random.Range(1, 3);
        actionType = (EnemyActionType)ran_idx;
        
        //  If attack, check the range
        if (actionType == EnemyActionType.Attack && isPlayerInRange() == false)
        {
            actionType = EnemyActionType.Move;
        }
        
        //  Enable different action icon
        switch (actionType)
        {
            case EnemyActionType.None:
                break;
            case EnemyActionType.Defense:
                //  UI manage
                attackIcon_tf.gameObject.SetActive(false);
                defenseIcon_tf.gameObject.SetActive(true);
                buffIcon_tf.gameObject.SetActive(false);
                moveIcon_tf.gameObject.SetActive(false);
                
                break;
            case EnemyActionType.Attack:
                //  UI manage
                attackIcon_tf.gameObject.SetActive(true);
                defenseIcon_tf.gameObject.SetActive(false);
                buffIcon_tf.gameObject.SetActive(false);
                moveIcon_tf.gameObject.SetActive(false);
                break;
            case EnemyActionType.Move:
                //  UI manage
                attackIcon_tf.gameObject.SetActive(false);
                defenseIcon_tf.gameObject.SetActive(false);
                buffIcon_tf.gameObject.SetActive(false);
                moveIcon_tf.gameObject.SetActive(true);
                break;
            case EnemyActionType.Buff:
                //  UI manage
                attackIcon_tf.gameObject.SetActive(false);
                defenseIcon_tf.gameObject.SetActive(false);
                buffIcon_tf.gameObject.SetActive(true);
                moveIcon_tf.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    //  Update UI of Enemy HP
    public void UpdateEnemyHp()
    {
        hpTxt.text = enemyCurHP + "/" + enemyMAXHP;
        hpImg.fillAmount = (float)enemyCurHP / (float)enemyMAXHP;
    }

    //  Update UI of Enemy Defense
    public void UpdataEnemyDefense()
    {
        defendTxt.text = enemyDefenseValue.ToString();
    }


    public void OnSelectedByCard()
    {
        //  Enable the Sprite Renderer for the selectionUI.
        _enemySelection_sr.enabled = true;
    }

    public void OnUnselectedByCard()
    {
        _enemySelection_sr.enabled = false;
    }


    public void OnDamaged(int calculated_val)
    {
        //  Subtract the Shield Value.
        if (enemyDefenseValue >= calculated_val)
        {
            enemyDefenseValue -= calculated_val;
            
            //  TODO: Play OnDamaged Anim
        }
        else
        {
            calculated_val -= enemyDefenseValue;
            enemyDefenseValue = 0;
            enemyCurHP -= calculated_val;
            if (enemyCurHP <= 0)
            {
                enemyCurHP = 0;
                //  Encapsulate them into a death function.
                //  TODO: Play Death Anim.
                
                //  Remove the enemy from list.
                EnemyManager.Instance.DeleteOneEnemyFromList(this);
                
                Destroy(gameObject, 1);
                Destroy(actionIcon_obj);
                Destroy(hpItem_obj);
                
            }
            else
            {
                //  TODO: Play OnDamaged Anim
                
            }

        }
        //  Update UI
        UpdataEnemyDefense();
        UpdateEnemyHp();
        
    }

    private void MoveAlongPath(int move_val)
    {
        for (int i = 0; i < move_val; i++)
        {
            if(_currentPath.Count > 0) MoveOneBoardUnit();
        }
    }
    
    private void MoveOneBoardUnit()
    {
        //  This is called inside a update function.
        //float step = 1.0f * Time.deltaTime;
    
        float zIndex = _currentPath[0].transform.position.z;
        //[TODO] : Add flip on player when going different direction
        transform.position =
            Vector2.MoveTowards(transform.position, _currentPath[0].transform.position, 60.0f);
        transform.position =
            new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, _currentPath[0].transform.position) < 0.0001f)
        {
            EnemyManager.Instance.PositionEnemyOnBoardUnit(_currentPath[0], this);
            _currentPath.RemoveAt(0);
        }

    }

    public bool isPlayerInRange()
    {
        _currentPath =
            MapManager.Instance.FindPath(EnemyActiveBoardUnit, Player.Instance.PlayerActiveBoardUnit);
        return _currentPath.Count <= 2;
    }

    public void HideEnemyActionIcon()
    {
        attackIcon_tf.gameObject.SetActive(false);
        defenseIcon_tf.gameObject.SetActive(false);
        buffIcon_tf.gameObject.SetActive(false);
        moveIcon_tf.gameObject.SetActive(false);
    }

    public IEnumerator EnemyAI_DoAction()
    {
        HideEnemyActionIcon();
        
        //  Play Anim -> path can be included in the excel table.
        //  Seconds -> anim to play -> can also be included in the excel table.
        yield return new WaitForSeconds(0.5f);

        switch (actionType)
        {
            case EnemyActionType.None:
                break;
            case EnemyActionType.Defense:
                //  Do Defense Action (should be in a func)
                enemyDefenseValue += 1;
                UpdataEnemyDefense();
                
                //  Play corresponding effect
                
                break;
            case EnemyActionType.Attack:
                //  Do Attack Action
                //增加：减去减掉的攻击力
                enemyAttackValue -= decreasedAttack;
                FightManager.Instance.GetPlayerHit(enemyAttackValue);
                if (FightManager.Instance.isPlayerReflectHarm)
                {
                    OnDamaged(enemyAttackValue);
                    FightManager.Instance.isPlayerReflectHarm = false;
                }
                //  Play Player Been hit.
                Player.Instance.playerAnimator.SetBool("b_player_isHit", true);
                //  Shake the Camera
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            case EnemyActionType.Move:
                //  TODO: Move_val can be configured in the Excel.
                Debug.Log("[EnemyAI_DoAction] Move...");
                MoveAlongPath(1);
                break;
            case EnemyActionType.Buff:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //  Seconds -> anim to play -> can also be included in the excel table.
        yield return new WaitForSeconds(1f);
        Player.Instance.playerAnimator.SetBool("b_player_isHit", false);
        //  Play idle.
    }
    
}
