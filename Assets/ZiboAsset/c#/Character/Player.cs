using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character//在各个角色类中实现动画的调用以及回执方法，并且判断player是否处于动画状态中来实现player的
{
    [Header("自动设置变量")]
    // Start is called before the first frame update
    public bool IsPlayerFinishAnimation = true;
    public GameMasterPlayerTurn MasterplayerTurn;
    public Animator animator;
    //为了实现强化效果
    
    
    void Start()//如果要让某个特殊职业继承player的类的话，注意把start和update的语句搬过去
    {
        currentEnergy = MaxEnergy;
        animator = GetComponent<Animator>();
        MasterplayerTurn = GameObject.Find("PlayerTurn").GetComponent<GameMasterPlayerTurn>();
        gameObject.tag = "Player";
        AddDamageableScript();
        //调用更换赛博义体方法；//Todo 完善读取义体更改角色义体prefabs:这个参数只会在开始战斗时使用
        base.Start();
    }
    public void BeginToMove()
    {
        animator.SetBool("bool_IsPlayerMove", true);
        IsPlayerFinishAnimation = false;
    }

    public void EndMove()
    {
        animator.SetBool("bool_IsPlayerMove", false);
        IsPlayerFinishAnimation = true;
    }
   
    public void Attack(Enemy enemy, float originDamage,Cyberborg attackedCyberborg,Cyberborg usedCyberborg)//origindamage由卡牌决定//注意usedCyberborg可能是null
    {
        float damage = attackForce + originDamage;
        if ( usedCyberborg != null)
        {
            damage += usedCyberborg.attack;
        }
        if(damage > enemy.shield)
        {
            damage -= enemy.shield;
            enemy.shield = 0;
        }
        else
        {
            enemy.shield -= damage;
            damage = 0;
        }
       
        if (enemy.isReflectHarmOnce)
        {
            Attacked(enemy.reflectProportion*damage,usedCyberborg);//如果敌人反伤，则在攻击时会对自己造成伤害
            enemy.isReflectHarmOnce = false;
        }
        float Realdamage = damage;
        Debug.Log(damage);
        enemy.Attacked(Realdamage, attackedCyberborg);
        //To do, generate a formula for damage and real harm
        //To do, finish the cause damage by a card parameter, then use this function in Attack card
        IsPlayerFinishAnimation = false;
        animator.SetTrigger("Attack");
    }
    public void Attacked(float realDamage,Cyberborg cyberborg)
    {
        realDamage = realDamage *(1- defenceAbility);//注意把这个东西设为小数
        if (isCannotHurtOnce)
        {
            realDamage = 0;
            isCannotHurtOnce = false;
        }
        if (cyberborg == null)
        {
            Damagable playerDamagable = gameObject.GetComponent<Damagable>();
            if (playerDamagable == null)
            {
                Debug.Log(gameObject.name + "enemyDamageable does not exist");
            }
            
            playerDamagable.OnHit(realDamage);//这部分只受到玩家defense影响
        }//伤害部分
        else
        {
            realDamage *= 1-cyberborg.defense;
            cyberborg.OnHit(realDamage);//这部分受cyber defense影响
        }
        //To do,finish the damage part when deal with the enemy AI(let enmey use this function by a int parameter  or a Enemy parameter)
        IsPlayerFinishAnimation = false;//important in  change the state of the whole battle
        animator.SetTrigger("Attacked");
    }
    public bool TryConsumeEnergy(int energy)
    {
        if(energy > currentEnergy)
        {
            Debug.Log("Player do not have enough energy");
            return false;
        }
        else
        {
            currentEnergy -= energy;
            return true;
        }
    }
    public void RecoverEnergy()
    {
        currentEnergy += RecoverEnergyATurn;
        if(currentEnergy > MaxEnergy)
        {
            currentEnergy = MaxEnergy;
        }
       //我有一个主意，设计一种卡牌，使得玩家能操纵转化血量（电量）为使用卡牌的能量；

    }
    public void Death()
    {
        IsPlayerFinishAnimation = false;
        Destroy(gameObject);
    }
    public void PlayerAnimationFinished()
    {
        Debug.Log("player finished animation");
        IsPlayerFinishAnimation = true; 
        //完善读取存档义体
        
    }


    
  
}
