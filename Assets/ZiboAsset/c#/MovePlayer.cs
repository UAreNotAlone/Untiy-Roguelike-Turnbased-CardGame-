using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Player player;
    private List<OverlayTile> _path;
    private float speed; 
    public bool ToMove = false;
    public void MoveAlongPath(Player p_player, List<OverlayTile> p_path,float p_speed = 50)
    {
        player = p_player;
        if(p_player == null)
        {
            Debug.Log("No player No move");
        }
        _path = p_path;
        speed = p_speed;
        ToMove = true;
    }
    private bool ReachDestination()
    {
        if (_path.Count == 0) return true;
        if(_path.Count == 1 && Vector2.Distance(player.transform.position, _path[0].transform.position) < 0.001f)
        {
            return true;
        }
        return false;
    }
    public void FaceTowards(Player p_player, Transform trans)
    {
        if (trans.position.x > p_player.transform.position.x)
        {
            p_player.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (trans.position.x < p_player.transform.position.x)
        {
            p_player.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void Update()
    {
        if (ToMove)
        {
            FaceTowards(player, _path[0].transform);

            if (player.animator.GetBool("bool_IsPlayerMove") == false)
            {
                player.BeginToMove();//animation}
            }

                float step = speed * Time.deltaTime;
            //Debug.Log(player.transform.position);
            //Debug.Log(Vector2.MoveTowards(player.transform.position, _path[0].transform.position, step));
            Vector2 tar =  
                Vector2.MoveTowards( player.transform.position, _path[0].transform.position, step);
            player.transform.position = new Vector3(tar.x, tar.y, _path[0].transform.position.z);
            //用二维向量赋值位置失败
            //Debug.Log(step);
            //Debug.Log(tar.x);
            //Debug.Log(Vector2.MoveTowards(player.transform.position, _path[0].transform.position, step));
            //Debug.Log(player.transform.position);
            //Debug.Log(_path[0].transform.position);
            //Debug.Log(player.name);

            if (Vector2.Distance(player.transform.position, _path[0].transform.position) < 0.01f)
            {
                Debug.Log("reach pone point");
                player.GetComponent<SpriteRenderer>().sortingOrder = _path[0].GetComponent<SpriteRenderer>().sortingOrder + 1;
                _path.RemoveAt(0);
            }
            if(ReachDestination())
            {
                Debug.Log("reach destination");
                player.EndMove();
                ToMove = false;
            }
        }
    }


}
