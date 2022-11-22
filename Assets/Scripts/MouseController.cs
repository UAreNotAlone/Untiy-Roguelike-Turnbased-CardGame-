using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject playerPrefab;
    public float speed = 1.0f;
    private PlayerInfo _playerInfo;

    private PathFinder _pathFinder;
    

   private List<OverlayTile> _path;
    // Start is called before the first frame update
    void Start()
    {
        _pathFinder = new PathFinder();
        _path = new List<OverlayTile>();
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        var focusedTileHit = GetFocusOnTile();
        if (focusedTileHit.HasValue)
        {
            //  [TODO]: Need some modification -> if mouse down, no more cursor will be spawn elsewhere.
            //      ->  this can be done by adding an instance of this class called _mouseCursor.
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder =    
                overlayTile.GetComponent<SpriteRenderer>().sortingOrder + 1;
            
            //  Activate this tile if a mouse down is detected.
            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.showTail();
                if (_playerInfo == null)
                {
                    _playerInfo = Instantiate(playerPrefab).GetComponent<PlayerInfo>();
                    PositionCharacterOnTile(overlayTile);
                }
                else
                {
                    _path = _pathFinder.FindPath(_playerInfo.playerActiveTile, overlayTile);
                    if (_path.Count == 0)
                    {
                        Debug.Log("[LateUpdate]: Nothing is in the returned _path");
                    }
                }
            }
        }
        else
        {
            Debug.Log("[Mouse Controller]: The RayCast failed.");
        }

        //  [TODO] Should have more constrain here
        if (_path.Count > 0)
        {
            MoveAlongPath();
            _playerInfo.player_animator.SetBool("bool_IsPlayerMove", true);
        }
        else
        {
            _playerInfo.player_animator.SetBool("bool_IsPlayerMove", false);
        }

    }

    //  [TODO] This method should be implemented in some class related to our player.
    private void MoveAlongPath()
    {
        //  This is called inside a update function.
        float step = speed * Time.deltaTime;
    
        float zIndex = _path[0].transform.position.z;
        //[TODO] : Add flip on player when going different direction
        _playerInfo.transform.position =
            Vector2.MoveTowards(_playerInfo.transform.position, _path[0].transform.position, step);
        _playerInfo.transform.position =
            new Vector3(_playerInfo.transform.position.x, _playerInfo.transform.position.y, zIndex);

        if (Vector2.Distance(_playerInfo.transform.position, _path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(_path[0]);
            _path.RemoveAt(0);
        }

    }

    
    public RaycastHit2D? GetFocusOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);


        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }


    public void PositionCharacterOnTile(OverlayTile tile)
    {
        if(_playerInfo == null) return;
        
        _playerInfo.transform.position = tile.transform.position;
        _playerInfo.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        _playerInfo.playerActiveTile = tile;
        


    }
    
}
