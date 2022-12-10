using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleGameMaster : MonoBehaviour
{


    [SerializeField]
    public static bool IsPlayerTurn = true;
    public GameMasterPlayerTurn playerTurnController;
    public bool Attackacted = false;

    public Player player;
    public List<Enemy> enemies;

    public int currentTurn = 1; //注意，一个回合的概念包含玩家回合和在这个玩家回合之后的敌人回合
    [Header("手设变量")]
    
    public Player[] playerPrefabs;
    public Transform playerTransform;

    public List<Enemy> EnemyPrefabs;
    public List<Transform> enemyTransforms;

    public List<CardClip> cardClips;//生成卡牌的地方
    public int numberOfCardATurn;
    public List<CardClass> playerTotalCard;

    // Start is called before the first frame update
    void Awake()
    {
        currentTurn = 0;
        InitializeCharacter();
        //初始化//实际上playerTotalCard的初始化可以通过获取resources内的prefabs执行
        //，这个放置可以再加载场景时同时进行，从而实现用不同的卡组加载战斗场景
        Attackacted = true;
        IsPlayerTurn = true;
        playerTurnController = GameObject.Find("PlayerTurn").GetComponent<GameMasterPlayerTurn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerTurn == true)
        {//注意保证初始化后这个要先执行，（合理性）
            if(Attackacted == true) //这个代表只在回合阶段的转换处执行函数
            { 

                if (IsInvoking(nameof(IsPerformingAnimation)))
                {
                CancelInvoke(nameof(IsPerformingAnimation));
                }
                currentTurn++;//在这里进入下一回合
                Debug.Log("currentTurn" + currentTurn);
                playerTurnController.enabled = true;
                Attackacted = false;
            }
        }
        else
        {
            if (Attackacted == false)
            {
                
                playerTurnController.GetComponent<GameMasterPlayerTurn>().enabled = false;
                Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.AttackAI();
                }
                InvokeRepeating(nameof(IsPerformingAnimation), 0.1f, 0.5f);
                Attackacted = true;
            }
        }

        CheckWin();
    }
    public void InitializeCharacter()
    {
        for (int i = 0; i < enemyTransforms.Count; i++)
        {

            enemies.Add(Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)], enemyTransforms[i].position, enemyTransforms[i].rotation));
            GameObject empty = new GameObject(); empty.AddComponent<InistantiateCahracter>();
            empty.GetComponent<InistantiateCahracter>().Init(enemies[i].gameObject, enemyTransforms[i]);
            enemies[i].transform.SetParent(enemyTransforms[i]);
            //Debug.Log("real enmey"+enemies[i].transform.position);

        }
        player = Instantiate(playerPrefabs[0], playerTransform.position, playerTransform.rotation).GetComponent<Player>();
        player.transform.SetParent(playerTransform);
        GameObject empty2 = new GameObject(); empty2.AddComponent<InistantiateCahracter>();
        empty2.GetComponent<InistantiateCahracter>().Init(player.gameObject, playerTransform);//initial position
    }
   
    public void CheckWin()
    {
        if (enemies.Count == 0)
        {
            Win();
        }
    }
    public void Win()
    {
        SceneManager.LoadScene("win");
    }
    public void EndPlayerTurn()
    {
        if(playerTurnController.stage == GameMasterPlayerTurn.Stage.NoChooseCard ||
            playerTurnController.stage == GameMasterPlayerTurn.Stage.ChosenCard)
        {
            IsPlayerTurn = false;
        }
        Debug.Log("click");
    }
    public void IsPerformingAnimation()
    {
        Debug.Log("Judging Animation in turn end");
        if(Enemy.AllEnemyFinishAniamtion && player.IsPlayerFinishAnimation)
        {
            if(IsPlayerTurn == false)
            {
                IsPlayerTurn = true;
            }
        }
    }

}
