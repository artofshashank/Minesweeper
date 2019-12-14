using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Individual tile on the gameplay screen.
/// This is attached to Tile prefab and gets instantiated with the prefab.
/// </summary>
public class Tile : MonoBehaviour
{
    [SerializeField]
    private Text _tileHint;
    [SerializeField]
    private GameObject _mineImage;
    [SerializeField]
    private Image _cover;
    [SerializeField]
    private Sprite _flagSprite;

    public RectTransform _rectTransform { get { return GetComponent<RectTransform> ( ); } set { } }
    public bool _isMine { get; private set; }
    public bool _isCovered { get; private set; }
    public bool _isFlagged { get; private set; }
    public TileStatus _tileStatus { get; private set; } 

    public GridCoordinates _coordinates;

    private void Awake ( )
    {
        ToggleVisualStatus ( TileStatus.Empty );
        _isCovered = true;
        _cover.gameObject.SetActive ( true );
    }

    /// <summary>
    /// Toggles visual displays on the tile as per status.
    /// </summary>
    /// <param name="status">Tile status.</param>
    private void ToggleVisualStatus ( TileStatus status )
    {
        _tileStatus = status;

        switch ( status )
        {
            case TileStatus.Empty:
                _tileHint.gameObject.SetActive ( false );
                _mineImage.SetActive ( false );
                break;

            case TileStatus.Hint:
                _tileHint.gameObject.SetActive ( true );
                _mineImage.SetActive ( false );
                break;

            case TileStatus.Mine:
                _tileHint.gameObject.SetActive ( false );
                _mineImage.SetActive ( true );
                break;
        }
    }

    /// <summary>
    /// Assigns the tile as a mine.
    /// </summary>
    public void AssignMine ( )
    {
        _isMine = true;
        ToggleVisualStatus ( TileStatus.Mine );
    }

    /// <summary>
    /// Assigns the tile as a hint
    /// </summary>
    /// <param name="numberOfMines">Hint of number of mines around</param>
    public void SetTileHint ( int numberOfMines )
    {
        _tileHint.text = numberOfMines.ToString ( );
        ToggleVisualStatus ( TileStatus.Hint );
    }

    /// <summary>
    /// Uncovers the tile.
    /// </summary>
    public void Uncover ( )
    {
        _isCovered = false;
        _cover.gameObject.SetActive ( false );
    }

    /// <summary>
    /// Toggles the flag.
    /// </summary>
    public void ToggleFlag ( )
    {
        if ( !_isCovered )
            return;

        _isFlagged = !_isFlagged;
        _cover.sprite = _isFlagged ? _flagSprite : null;
    }
}
