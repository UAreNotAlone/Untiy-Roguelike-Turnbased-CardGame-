using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyberborg : Damagable//玩家更换cyberborg是通过character里面的changecyber实现，存储玩家义体数据可以通过访问character里面的cyberborgs完成
{
    public enum CyberborgType
    {
        head,
        arm,
        leg
    }

    public Character holdCharacter;
   //注意当某个角色继承角色类时，其中参数可以在具体角色中设置具体参数，例如义体，在总角色类设置相应的义体prefabs参数，再加入这个脚本，（资源，脚本，实例化）
    //这种设计不利于更换装备，
    Dictionary<CyberborgType, float> arrange = new Dictionary<CyberborgType, float>();
    [Header("手动设置变量")]
    public CyberborgType cyberborgType = CyberborgType.head;
    public float maxBorgHealth = 20f;
    public float attack = 10;//一次攻击的总攻击力取决于卡牌，卡牌使用的义体，和玩家的基本属性
    public float defense = 0.2f;//一次防御的防御力取决于敌人攻击的部位和该部位的防御力和玩家自身的防御力
    // Start is called before the first frame update
  
   
    public void Awake()
    {
        base.Start();
        arrange.Add(CyberborgType.head, -15);
        arrange.Add(CyberborgType.arm, -30);
        arrange.Add(CyberborgType.leg, -45);
        Health = maxBorgHealth;
        DamageableMaxHealth = maxBorgHealth;//调整damageable组件的current health和maxhealth
        gameObject.AddComponent<NoSpecialEffect>();
       
        //holdCharacter = gameMaster.enemies[0];//实验语句，删去//是否要在这里完成实例化血条以及设置好父物体禁用物体等操作//

        //以上只实现了加入血条成为cyber义体物体的子物体，并可以对cyber义体物体进行摧毁；s
    }

    public void Start()//为什么这里不执行了
    {
        
    }
    //重写死亡方法
    public override void Death()//也可以不重写，注意在调用时
    {
        
            //注意在实例化子类对象后，在对应对象脚本执行过程中，父类会调用子类重写的方法
        base.Death();
        
        //holdCharacter.cyberborgs[cyberborgType] = null;//清除持有玩家的义体列表中的该义体
        Destroy(this.gameObject);//由damageable父类脚本执行，//想象整个父类脚本嵌入该脚本并且死亡方法被重写

    }
    public void InstallThisCyberborg()
    {
            transform.SetParent(holdCharacter.transform);
            transform.position = holdCharacter.transform.position + new Vector3(-0.5f * holdCharacter.characterSize.x,
                holdCharacter.characterSize.y + 1 + arrange[cyberborgType] * holdCharacter.characterSize.y / 45f, 0);
            
       
    }

    public void SetHealthBar()
    {
        playHealthBar = Resources.Load<GameObject>("PlayerHealthBar");
        enemyHealthBar = Resources.Load<GameObject>("EnemyHealthBar");
        if (holdCharacter.gameObject.tag == "Player")
        {
            Transform trans = GameObject.Find("PlayerHealthBarTransform").transform;
            thisHealthBar = (GameObject)Instantiate(playHealthBar, trans.position, trans.rotation);
            
            thisHealthBar.transform.SetParent(trans.transform);//血条会把这个义体认作父类
            thisHealthBar.transform.position = trans.transform.position + new Vector3(0,
               arrange[cyberborgType], 0);
            //Debug.Log(arrange.Count);

            _health = holdCharacter.gameObject.GetComponent<Character>().maxHealth;
            Bar = thisHealthBar.transform.GetChild(1);
        }
        else if (holdCharacter.gameObject.tag == "Enemy")
        {
            Enemy enemy = holdCharacter.gameObject.GetComponent<Enemy>();
            thisHealthBar = (GameObject)Instantiate(enemyHealthBar);//
            thisHealthBar.transform.SetParent(gameObject.transform);//注意血条作为义体的子物体而非角色的子物体
            thisHealthBar.transform.localPosition = Vector3.zero;//记得把enemy的pivot调整到底部
            
            _health = holdCharacter.gameObject.GetComponent<Character>().maxHealth;
            Bar = thisHealthBar.transform.GetChild(1);
        }
        
        //注意此时获取义体血条 并对敌人在附近 对玩家在左上角实例化
        //注意
    }
    public void UseCyberborgFuntion()
    {
        ISpeciaEffect[] sps = GetComponents<ISpeciaEffect>();
        foreach(ISpeciaEffect sp in sps)
        {
            sp.SpecialEffect();
        }
    }
    // Update is called once per frame
    
}
