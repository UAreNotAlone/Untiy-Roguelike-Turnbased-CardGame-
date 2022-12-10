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
    [Header("自动设置变量")]
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
    [Header("自动设置变量")]
    public int enemyAnimationNumber = 0;
    public Animator enemyAnimator;

    public enum EnemyStage
    { 
        MouseAround,
        notMouseAround,
        ShowAttackPart,
        attackedPartChosen,
        attacked,
        attack
    }
    public EnemyStage enemyStage = EnemyStage.notMouseAround;

    public bool ActiveCyberborg = false;
    public Cyberborg chosenCyberBorg;
    public List<Transform> listCy = new List<Transform>();
    
    public Vector2 enemySize;//
    
    public void Start()
    {
        enemyAnimationNumber = 0;
        gameObject.tag = "Enemy";
        LiveEnemy.Add(GetComponent<Enemy>());
        AddDamageableScript();
        enemyAnimator = GetComponent<Animator>();
        enemySize = characterSize;
        //注意敌人的义体类型由开发者设置
        base.Start();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            enemyStage = EnemyStage.notMouseAround;//当取消卡牌选择时记得把这个enemy的状态调整回来
        }
        if(enemyStage == EnemyStage.ShowAttackPart)//在被攻击阶段展示义体，否则关闭义体
        {
            if (!ActiveCyberborg)
            {
                foreach (Cyberborg.CyberborgType cyType in cyberborgs.Keys)
                {
                    if (cyberborgs[cyType] != null)
                    {
                        foreach (Transform trans in cyberborgs[cyType].transform.GetComponentsInChildren<Transform>())
                        {
                            trans.gameObject.layer = 0;
                        }
                    }
                }
                ActiveCyberborg = true;
            }
            CheckChoosingAttackPart();

        }
        else
        {
            if (ActiveCyberborg)
            {
                foreach (Cyberborg.CyberborgType cyType in cyberborgs.Keys)
                {
                    if (cyberborgs[cyType] != null)
                    {
                        foreach (Transform trans in cyberborgs[cyType].transform.GetComponentsInChildren<Transform>())
                        {
                            trans.gameObject.layer = 7;
                        }
                    }
                }
                ActiveCyberborg = false;
            }
        }
        

    }
    public void AttackedWhichPart()
    {
        enemyStage = Enemy.EnemyStage.ShowAttackPart;
    }
    public bool isClosedToCyberCriterion(Vector3 v1,Vector3 v2)
    {

        if (Vector2.Distance(v1, v2) < 10)
        {

            return true;
        }
        else
        {
            return false;
        }
    }
    public void CheckChoosingAttackPart()//实际上这一部分也可以在Cyberborg内完成，
    {
        //获取非空义体\
        listCy = new List<Transform>();
        foreach (Cyberborg.CyberborgType cyType in cyberborgs.Keys)
        {
            if (cyberborgs[cyType]!= null)
            {
                listCy.Add(cyberborgs[cyType].transform);
            }
        }
        if(listCy.Count == 0)
        {
            Debug.Log("an enemy live without cyberborg");
            enemyStage = EnemyStage.attackedPartChosen;//没有义体直接进入下一阶段
        }

        Transform CyberborgAroundMouse = IsMouseOnTransform.IsMouseAroundSomeTransform(listCy, isClosedToCyberCriterion);
        if (CyberborgAroundMouse != null && Input.GetMouseButton(0)&& enemyStage == EnemyStage.ShowAttackPart)
        {
            chosenCyberBorg = CyberborgAroundMouse.GetComponent<Cyberborg>();//选中义体
            //进入下一阶段
            enemyStage = EnemyStage.attackedPartChosen;

        }
    }
    public void Attacked(float realDamage,Cyberborg cyberborg)
    {
        //Debug.Log(realDamage);
        realDamage *= (1 - defenceAbility);
        if (isCannotHurtOnce)
        {
            realDamage = 0;
            isCannotHurtOnce = false;
        }

        if (cyberborg == null)
        {
            Damagable enemyDamagable = gameObject.GetComponent<Damagable>();
            if (enemyDamagable == null)
            {
                Debug.Log(gameObject.name + "enemyDamageable does not exist");
            }
           // Debug.Log(realDamage);
            enemyDamagable.OnHit(realDamage);//伤害部分
        }
        else
        {
            realDamage *= (1 - cyberborg.defense);
            //Debug.Log(realDamage);
            cyberborg.OnHit(realDamage);//受到cyber defence影响
        }
        enemyStage = EnemyStage.attacked;//方便后续更改
        enemyAnimationNumber += 1;
        enemyAnimator.SetTrigger("Attacked");
        Debug.Log("enemy is under attacked");//动画部分
    }
    // Start is called before the first frame update
    public void Attack(Player player, float originDamage,Cyberborg attackedCyberborg,Cyberborg usedCyberborg)//damage由敌人AI选择的攻击方式决定//在attack和attacked里面完成伤害计算和判定
    {
        float damage = attackForce + originDamage;
        if (usedCyberborg != null)
        {
            damage += usedCyberborg.attack;
        }//计算总伤害

        if (damage > player.shield)
        {
            damage -= player.shield;
            player.shield = 0;
        }
        else
        {
            player.shield -= damage;
            damage = 0;
        }//计算护盾抵消伤害
        if (player.isReflectHarmOnce)
        {
            Attacked(player.reflectProportion*damage, usedCyberborg);
            player.isReflectHarmOnce = false;
        }

        enemyStage = EnemyStage.attack;
        float realDamage = damage;
        player.Attacked(realDamage, attackedCyberborg);
        
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
    public void EndAttack()
    {
        enemyStage = EnemyStage.notMouseAround;
    }
   public void EndAttacked()
    {
        enemyStage = EnemyStage.notMouseAround;
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
    


}
