using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModeMoveCard : MonoBehaviour//处理不同卡牌被选择之后的情况
{
    [Header("自动获取的变量")]
    public CardClass ThisCard;
    public Transform selectedMapPoint;
    public GetOverlayContainer GotOverlayContainer;
    public Player player;
    public LayerMask overlay;
    private MoveRange _moveRange;
    // Start is called before the first frame update
    void Start()
    {
        overlay = 1<<LayerMask.NameToLayer("Overlay");
        _moveRange = gameObject.GetComponent<MoveRange>();
        player = gameObject.GetComponent<CardClass>().holdPlayer;
        ThisCard = gameObject.GetComponent<CardClass>();
        GotOverlayContainer = GameObject.Find("GetContainer").GetComponent<GetOverlayContainer>();
    }
    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        if (ThisCard.thisCardStage == CardClass.ThisCardStage.Chosen)   //当玩家选择移动卡时，会探测鼠标位置是否在某个地图节点上
        {
            
            //Debug.Log("AttackChosenByPlayer");
            selectedMapPoint = IsMouseOnSomeOverlay();

            
            //Debug.Log("selected null is" + selectedMapPoint == null);
            //unity 不允许transform作为null 被传递参数？
            //Debug.Log("Enemy.isMouseAroundSomeEnemy" + Enemy.isMouseAroundSomeEnemy);
        }
        if (Input.GetMouseButtonDown(0) && ThisCard.thisCardStage == CardClass.ThisCardStage.Chosen && 
            _moveRange.IsCheckValidMove(selectedMapPoint,player))//注意要保证从第二阶段到第3阶段，要禁用从第一阶段到第二阶段
        {
            //如果在已经选择这张卡情况下，玩家在某个地图节点按下鼠标
            Debug.Log("use moveEffect");
            List<Transform> targetList = new List<Transform>();
            targetList.Add(selectedMapPoint);
            ThisCard.UseCauseEffect(targetList);//
        }
        if (Input.GetMouseButtonDown(1))
        {
            ThisCard.thisCardStage = CardClass.ThisCardStage.cardNotAroundMouse;
        }
    }

    public Transform IsMouseOnSomeOverlay() //射线检测鼠标是否在某个ovelay上并返回transforms
    {


        var hit = GetFocusOnTile();
        if (hit.HasValue)
        {
            //Debug.Log(hit.Value.collider.transform.position);
            return hit.Value.collider.gameObject.transform;
            
        }
        else
        {
            return null;
        }
        
    }
    
    
    public RaycastHit2D? GetFocusOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        Debug.Log("begin to get mouse on overlay");
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero,Mathf.Infinity,
            overlay);//宇洋是怎么防止其他东西被选中的
        Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            Debug.Log("The RayCast success");
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }


}
