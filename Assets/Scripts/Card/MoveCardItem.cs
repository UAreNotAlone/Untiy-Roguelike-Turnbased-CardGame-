using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCardItem : CardItem, IPointerDownHandler
{
    /*Variables needed*/
    private BoardUnit _boardUnitHitByRay;
    private List<BoardUnit> _inRangeUnits;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //  Play the sound
        AudioManager.Instance.PlayEffectByName("Cards/draw");
        
        //  Show the LineUI for convenience
        UIManager.Instance.ShowUIByName<LineUI>("LineUI");
        //  Set the Start Position for the LineUI
        UIManager.Instance.GetUIScript<LineUI>("LineUI").SetLineStartPosition(transform.GetComponent<RectTransform>().anchoredPosition);
        
        
        //  Hide the Cursor
        Cursor.visible = false;
        //  Stop all the coroutines for safety
        StopAllCoroutines();
        //  Activate Mouse related Coroutines.
        StartCoroutine(OnMouseDownRight(eventData));
    }


    IEnumerator OnMouseDownRight(PointerEventData pData)
    {
        //  Check for valid.
        while (true)
        {
            //  If right key is being pressed again, jumped out of the loop.
            if (Input.GetMouseButton(1))
            {
                break;
            }

            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform.parent.GetComponent<RectTransform>(),
                    pData.position,
                    pData.pressEventCamera,
                    out pos))
            {
                //  Display range
                _inRangeUnits = MapManager.Instance.ShowInRangUnit(int.Parse(cardData["Arg0"]));
                //  Check whether mouse points to the monster.
                UIManager.Instance.GetUIScript<LineUI>("LineUI").SetLineEndPosition(pos);
                CheckRayToBoard();

            }

            yield return null;

        }
        
        //  The period is done
        Cursor.visible = true;
        UIManager.Instance.CloseUIByName("LineUI");
        
    }

    private void CheckRayToBoard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Debug.Log("Ray...");
        
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("BoardUnit")))
        {
            _boardUnitHitByRay = hit.transform.GetComponent<BoardUnit>();
            Debug.Log("Board Hitted: " + _boardUnitHitByRay.grid2DLocation);
            if (_inRangeUnits.Contains(_boardUnitHitByRay))
            {
                _boardUnitHitByRay.OnSelectedByCard();
            }
            
            //  Player wants to move to this unit
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                Cursor.visible = true;
                UIManager.Instance.CloseUIByName("LineUI");
                if (_inRangeUnits.Contains(_boardUnitHitByRay) && TryUseCard_Mana() == true)
                {
                    //  Play Audio effect
                    AudioManager.Instance.PlayEffectByName("Effect/sword");
                    
                    //  Play Work Anim.
                    Player.Instance.PlayMoveAnim();
                    //  Move.
                    PlayerManager.Instance.PositionPlayerOnBoardUnit(_boardUnitHitByRay, Player.Instance);
                }
                _boardUnitHitByRay.OnUnselectedByCard();
                _boardUnitHitByRay = null;
            }
        }
        else
        {
            if (_boardUnitHitByRay != null)
            {
                _boardUnitHitByRay.OnUnselectedByCard();
                _boardUnitHitByRay = null;
            }
        }
    }
}
