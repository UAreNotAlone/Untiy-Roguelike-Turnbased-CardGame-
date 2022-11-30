using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEnergyBall : MonoBehaviour
{
    public float length = 10;
    
    public List<GameObject> ShownBalls = new List<GameObject>();
    public GameObject EnergyBallPrefab;
    private BattleGameMaster gameMaster;
    private Transform energyBalls;

    // Start is called before the first
    // frame update
    public void Start()
    {
        EnergyBallPrefab = (GameObject)Resources.Load("EnergyBall");
        gameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        energyBalls = GameObject.Find("EnergyBalls").transform;
    }
    public void Update()
    {
        showEnergyBall(gameMaster.player,energyBalls);
    }
    public void showEnergyBall(Player player,Transform trans)
    {
        if (ShownBalls.Count == player.currentEnergy) return;
        while (ShownBalls.Count > player.currentEnergy)
        {
            GameObject ball = ShownBalls[ShownBalls.Count-1];
            ShownBalls.Remove(ball);
            Destroy(ball);

        }
        while (ShownBalls.Count < player.currentEnergy)
        {
            int i = ShownBalls.Count;
            GameObject ball = (GameObject)Instantiate(EnergyBallPrefab, trans.position + new Vector3(i * length, 0, 0), trans.rotation);
            ball.transform.SetParent(transform);
            ShownBalls.Add(ball);
            
        }
    }

}
