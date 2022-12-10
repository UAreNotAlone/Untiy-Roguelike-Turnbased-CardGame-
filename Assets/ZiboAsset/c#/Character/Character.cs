using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour//把这个类的子类挂载到card prefab 上
{


    [Header("自动设置变量")]
    public GameObject CharacterPrefab;
    //public Transform currentTransform;
    [SerializeField]
    public Transform bornTransform = null;
    public MapPoint onMapPoint;

    public bool isReflectHarmOnce = false;
    public float reflectProportion = 1;

    public bool isCannotHurtOnce = false;

    public float maxHealth;
    public float defenceAbility = 0.1f;
    public float attackForce;
    public float shield;
    [SerializeField]
    public int MaxEnergy = 4;
    public int currentEnergy;
    public int RecoverEnergyATurn = 3;

    public Dictionary<Cyberborg.CyberborgType,Cyberborg> cyberborgs = new Dictionary<Cyberborg.CyberborgType,Cyberborg>();//现在该实例化后的玩家具有的所有义体
    
    [Header("手动设置变量")]
    public List<Cyberborg> cyberborgPrefabs;
    public Vector2 characterSize = new Vector2(7,16);
    //一个字典，字典三个key是确定的/ /一个添加了Cyberborg脚本和一个功能脚本的预制件；
    /// </summary>
    //赛博朋克，是否有手部，手部血量，是否有腿部，腿部血量，是否有躯干义体，
    //（没有躯干义体代表躯干为肉身），躯干义体血量
    //需要更改敌人AI中攻击模式（在攻击时先暂时随机选择一个义体），需要更改player enemy的attack和attacked接口函数（多加一个参数（攻击哪一部分）），
    //public 

    public void InitializeMapPosition()     //获取prefab
    {

        GameObject tGO = new GameObject("tGO");
        tGO.AddComponent<InistantiateCahracter>();
        tGO.GetComponent<InistantiateCahracter>().Init(gameObject, bornTransform);
        //Instantiate(this.gameObject, bornTransform.position, bornTransform.rotation);
    }
    public void setMaxHeatlth(float m)
    {
        if (m <= 0)
        {
            maxHealth = 0;
        }
        else
        {
            maxHealth = m;
        }
    }
    public void setDefenceAbility(float m)
    {
        if (m <= 0)
        {
            defenceAbility = 0;
        }
        else
        {
            defenceAbility = m;
        }
    }
    public void setAttackForce(float m)
    {
        if (m <= 0)
        {
            attackForce = 0;
        }
        else
        {
            attackForce = m;
        }
    }
    public void AddDamageableScript()
    {
        Damagable dama = gameObject.AddComponent<Damagable>();
        dama.DamageableMaxHealth = maxHealth;

        
    }
    public void ChangeCyberborg(Cyberborg cy)//在奖励给玩家义体时，需要传入一个prefab(注意是已经挂载到物体上)参数,该方法会安装义体，设置义体血条并更新玩家义体列表
    {

        if(cy == null)
        {
            Debug.Log("No cyberborg");
            return;
        }
        //注意要改变的不是pCyberborgrefabs，而玩家现有cyberborg，并注意在战斗结束后存档中cyberborgPrefabs是玩家现有cyberborgs
        if (cyberborgs[cy.cyberborgType] != null)
        {
            //被替换的同类型义体直接摧毁还是放入背包?可以考虑储存在哪里
            Destroy(cyberborgs[cy.cyberborgType]);
        }

        
        Cyberborg cyInstance = (Cyberborg) Instantiate(cy);
        //设置好持有者
        cyInstance.holdCharacter = this;
        
        cyInstance.SetHealthBar();//玩家和敌人设置好血条的方式不同,因此要先设置持有者再设置血条；
        
        cyInstance.InstallThisCyberborg();                 //设置该义体物体为角色子物体，并调整好位置；（并给角色加一些特殊效果？）
         foreach(Transform trans in cyInstance.transform.GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = 7;
        }   //暂时禁用（不可见）掉义体//或许用spriterender的禁用会更好，不过后者需要把血条也给设置一下
        
        //在这个实例化
        cyberborgs[cy.cyberborgType] = cyInstance;//更改义体列表
        //注意玩家在实例化义体之前需要读取存储信息，更改CyberborgPrefabs,()  
    }
    // Start is called before the first frame update
    public void Start()
    {
        cyberborgs.Add(Cyberborg.CyberborgType.head, null);
        cyberborgs.Add(Cyberborg.CyberborgType.arm, null);
        cyberborgs.Add(Cyberborg.CyberborgType.leg, null);//注意继承类执行该初始化
        foreach(Cyberborg cyberborg in cyberborgPrefabs)
        {
            ChangeCyberborg(cyberborg);//在初始化时注意player要先获取存档中的义体prefabs，再执行，
            
        }
        //这个在挂载子类脚本的物体上也不会执行
    }

    // Update is called once per frame
    void Update()
    {
        //注意这个方法不会在子物体中执行
       
    }
}
