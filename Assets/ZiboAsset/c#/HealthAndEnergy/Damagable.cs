using UnityEngine;
public class Damagable : MonoBehaviour, IDamagable
{

    public Transform can;
    public GameObject healthText;
    public BattleGameMaster gameMaster;

    public float _health = 3f;
    public GameObject playHealthBar;
    public GameObject enemyHealthBar;

    public GameObject thisHealthBar;
    public Transform Bar;

    public float DamageableMaxHealth = 10;
    public void Start()
    {

        gameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        can = GameObject.Find("Canvas").transform;
        healthText = Resources.Load<GameObject>("Text");
        playHealthBar = Resources.Load<GameObject>("PlayerHealthBar");
        enemyHealthBar = Resources.Load<GameObject>("EnemyHealthBar");

        if (gameObject.tag == "Player")
        {
            Transform trans = GameObject.Find("PlayerHealthBarTransform").transform;
            thisHealthBar = (GameObject)Instantiate(playHealthBar, trans.position, trans.rotation);
            thisHealthBar.transform.SetParent(trans);
            _health = this.gameObject.GetComponent<Character>().maxHealth;
            Bar = thisHealthBar.transform.GetChild(1);

        }
        else if (gameObject.tag == "Enemy")
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            thisHealthBar = (GameObject)Instantiate(enemyHealthBar,
                enemy.transform.position + new Vector3(-0.5f * enemy.enemySize.x, enemy.enemySize.y + 1, 0), transform.rotation);//
            thisHealthBar.transform.SetParent(enemy.transform);
            _health = this.gameObject.GetComponent<Character>().maxHealth;
            Bar = thisHealthBar.transform.GetChild(1);
        }


    }
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            if (value <= 0)
            {
                _health = 0;
            }
            else if(value >= DamageableMaxHealth)
            {
                _health = DamageableMaxHealth;//在这里初始化时会用Health = _health初始化，当为0 时会执行虚函数
            }
            else
            {
                _health = value;
            }
            
            ;//注意value是赋值符号右边的结果
            //Debug.Log(_health);
            
            if (_health > 0)
            {
                Hurt();//Hurt动画？
            }
            if (_health <= 0)
            {
                Debug.Log("调用了death函数");
                Death();
            }
        }
    }

    public virtual void Death()//这里需要调用Enemy或player的death方法
    {
        if (gameObject.tag == "Player")
        {
            GetComponent<Player>().Death();
        }
        else if (gameObject.tag == "Enemy")
        {
            gameMaster.enemies.Remove(this.gameObject.GetComponent<Enemy>());
            GetComponent<Enemy>().DeathEvent();//先调用动画再在动画结束时使用动画事件
        }
        //当为躯干血条时，
        //Destroy(this.gameObject);
    }

    public void Hurt()
    {


    }


    public void OnHit(float damage)
    {

        RectTransform rec = Instantiate(healthText).GetComponent<RectTransform>();
        rec.position = Camera.main.WorldToScreenPoint(transform.position);
        rec.SetParent(can);

        //Debug.Log(transform.position);
        rec.gameObject.GetComponent<Text>().text.text = damage.ToString();
        //Debug.Log(rec.gameObject.name);
        rec.SetParent(can);
      
        Health -= damage;
        //Debug.Log(damage);
        //血条更新
        if (GetComponent<Cyberborg>() != null)
        {
            Bar.localScale = new Vector3((Health) / gameObject.GetComponent<Cyberborg>().maxBorgHealth,
             Bar.localScale.y, Bar.localScale.z);
        }
        else if (GetComponent<Character>() != null)
        {
            Bar.localScale = new Vector3(Health/ gameObject.GetComponent<Character>().maxHealth,
             Bar.localScale.y, Bar.localScale.z);
        }
    }
    public void CureHealth(float cure)
    {
        RectTransform rec = Instantiate(healthText).GetComponent<RectTransform>();
        rec.position = Camera.main.WorldToScreenPoint(transform.position);
        rec.SetParent(can);

        //Debug.Log(transform.position);
        rec.gameObject.GetComponent<Text>().text.text = cure.ToString();
        rec.gameObject.GetComponent<Text>().text.color = new Color(200, 0, 0);
        //Debug.Log(rec.gameObject.name);
        rec.SetParent(can);

        Health += cure;
    }
}

