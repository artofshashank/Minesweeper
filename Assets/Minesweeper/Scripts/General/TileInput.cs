using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// Tile Input class. Attached to Tile prefab.
/// Uses Unity's IPointerClickHandler interface to implement pointer click.
/// Calls GameManager left/right click functions directly.
/// </summary>
public class TileInput : MonoBehaviour, IPointerClickHandler
{
    private Tile _tile;
    private void Awake ( )
    {
        _tile = GetComponent<Tile> ( );
    }
    public void OnPointerClick ( PointerEventData eventData )
    {
        if ( eventData.button == PointerEventData.InputButton.Left )
        {
            GameManager._instance.TileLeftClicked ( _tile._coordinates, _tile._tileStatus );
        }
        else if ( eventData.button == PointerEventData.InputButton.Right )
        {
            GameManager._instance.TileRightClicked ( _tile._coordinates );
        }
    }
}
