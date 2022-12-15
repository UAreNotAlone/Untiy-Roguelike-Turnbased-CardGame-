using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackCardItem : CardItem, IPointerDownHandler
{
    private Enemy _enemyHitByRay;
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
                //  Check whether mouse points to the monster.
                UIManager.Instance.GetUIScript<LineUI>("LineUI").SetLineEndPosition(pos);
  
                CheckRayToEnemy();

            }

            yield return null;

        }
        
        //  The period is done
        Cursor.visible = true;
        UIManager.Instance.CloseUIByName("LineUI");
    }

    public void CheckRayToEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //  Get In Range Units.
        List<BoardUnit> inRangeUnits = MapManager.Instance.ShowInRangUnit(int.Parse(cardData["Range"]));
        RaycastHit hit;
        //Debug.Log("RayCasting...");
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
        {
            _enemyHitByRay = hit.transform.GetComponent<Enemy>();
            Debug.Log("Enemy Hitted: " + _enemyHitByRay.name);
            _enemyHitByRay.OnSelectedByCard();
            
            //  Player wants to use this attack card.
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                Cursor.visible = true;
                //  Close the LineUI
                UIManager.Instance.CloseUIByName("LineUI");
                //  TODO Check IsEnemyInrange.
                if (TryUseCard_Range(_enemyHitByRay) == true)
                {
                    if (TryUseCard_Mana() == true)
                    {
                        //  Play effect
                        PlayCardEffect(_enemyHitByRay.transform.position);
                    
                        //  Play Audio effect
                        AudioManager.Instance.PlayEffectByName("Effect/sword");
                    
                        //  Play Player Attack Anim
                        Player.Instance.PlayAttackAnim();
                    
                        //  Calculate the Attack Value
                        int effect_val = int.Parse(cardData["Arg0"]);
                        //  TODO:   Add this value with Player's attributes.
                        _enemyHitByRay.OnDamaged(effect_val);
                        //  Play Player Attack Anim
                        //Player.Instance.playerAnimator.SetBool("b_player_isAttack", false);
                        
                        MapManager.Instance.HideInRangUnit(int.Parse(cardData["Range"]));
                        
                    }
                    
                    
                }
                //  Not been Selected
                //  TODO: Move out the loop.
                _enemyHitByRay.OnUnselectedByCard();
                _enemyHitByRay = null;
            }
        }
        else
        {
            //  The mouse is not pointing to a enemy.
            if (_enemyHitByRay != null)
            {
                _enemyHitByRay.OnUnselectedByCard();
                
                _enemyHitByRay = null;
            }
        }
    }
}
