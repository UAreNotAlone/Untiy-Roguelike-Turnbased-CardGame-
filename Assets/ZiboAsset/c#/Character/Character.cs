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
    public float defenceAbility;
    public float attackForce;
    public float currenthealth;
    public float shield;
    [SerializeField]
    public int MaxEnergy = 4;
    public int currentEnergy;
    public int RecoverEnergyATurn = 3;
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
        gameObject.AddComponent<Damagable>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        CharacterPrefab = this.gameObject;//直接获取对应的实例
        AddDamageableScript();
        Debug.Log("the addDamageable execute");
        //这个在挂载子类脚本的物体上也不会执行
    }

    // Update is called once per frame
    void Update()
    {
        //注意这个方法不会在子物体中执行
        if (GetComponent<Damagable>() != null)
        {
            currenthealth = this.GetComponent<Damagable>()._health;
        }
    }
}
