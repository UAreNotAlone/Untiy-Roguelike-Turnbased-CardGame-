using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muslim : MonoBehaviour,IBuff
{
    public Character character
    {
        set { _character = value; }
        get { return _character; }
    }

    public Character _character = new Character();//使用buff时，先实例化buff，再传入setCharacter,最后设置buff参数
    public BattleGameMaster battleGameMaster;
    public void Start()
    {
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
    }
    //
    // Start is called before the first frame update

    public void BuffEffect()
    {
        foreach(Enemy enemy in battleGameMaster.enemies)
        {
            //造成真实伤害？还是让player去攻击？（反伤）真实伤害好像不会被反掉，
            enemy.Attacked(5);

        }
    }
}
