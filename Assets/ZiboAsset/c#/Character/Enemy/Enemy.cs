using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public static bool AllEnemyFinishAniamtion
    {
        set { }
        get {
            foreach (Enemy enemy in LiveEnemy)
            {
                if (enemy.enemyFinishAnimation == false)
                {
                    return false;
                }
            }
            return true;
        }
     //设置一个属性当被访问时执行函数
    }

    public static List<Enemy> LiveEnemy = new List<Enemy>();

    public bool enemyFinishAnimation
    {
        get { 
            if(enemyAnimationNumber <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        ; }
    }

    public int enemyAnimationNumber = 0;
    public Animator enemyAnimator;

    public enum EnemyStage
    { 
        MouseAround,
        notMouseAround,
        ShowAttackPart,
        attacked,
        attack
    }
    public EnemyStage enemyStage = EnemyStage.notMouseAround;

    [Header("需要设置的变量")]
    public Vector2 enemySize = new Vector2(7,6);//
    public void AttackedWhichPart()
    {
        
        
        enemyStage = Enemy.EnemyStage.ShowAttackPart;
    }
    public void Attacked(float realDamage)
    {
        if (isCannotHurtOnce)
        {
            realDamage = 0;
            isCannotHurtOnce = false;
        }
        Damagable enemyDamagable = gameObject.GetComponent<Damagable>();
        if (enemyDamagable == null)
        {
            Debug.Log(gameObject.name + "enemyDamageable does not exist");
        }
        enemyDamagable.OnHit(realDamage);//伤害部分
        enemyStage = EnemyStage.attacked;//方便后续更改
        enemyAnimationNumber += 1;
        enemyAnimator.SetTrigger("Attacked");
        Debug.Log("enemy is under attacked");//动画部分
    }
    // Start is called before the first frame update
    public void Attack(Player player, float damage)//damage由敌人AI选择的攻击方式决定//在attack和attacked里面完成伤害计算和判定
    {
        if (player.isReflectHarmOnce)
        {
            Attacked(player.reflectProportion*damage);
            player.isReflectHarmOnce = false;
        }

        enemyStage = EnemyStage.attack;
        float realDamage = damage;
        player.Attacked(realDamage);
        
        enemyAnimator.SetTrigger("Attack");//
    }
    public void AttackAI()
    {
        //写敌人攻击AI
        SendMessage("AttackAction");
    }
    
    public void BeginToMove()
    {
        enemyAnimator.SetBool("Move", true);
        enemyAnimationNumber += 1;
    }

    public void EndMove()
    {
        enemyAnimator.SetBool("Move", false);//原先把player的逻辑搬过来没有改掉这个
        enemyAnimationNumber -= 1;
    }
   
    
    public void DeathEvent()
    {
        
        enemyAnimator.SetTrigger("Death");//在Death动画后加上destroy event;以及finishThisAnimation
        LiveEnemy.Remove(this.GetComponent<Enemy>());
        
    }
    public void DestroyThisEnemy()
    {
        Debug.Log("enemy is destroyed");
        Destroy(gameObject);
    }
    public void FinishThisAnimation()
    {
        Debug.Log("finish animation");
        enemyAnimationNumber -= 1;
    }
    public void Start()
    {
        enemyAnimationNumber = 0;
        gameObject.tag = "Enemy";
        LiveEnemy.Add(GetComponent<Enemy>());
        AddDamageableScript();
        enemyAnimator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            enemyStage = EnemyStage.notMouseAround;
        }
        
    }


}
