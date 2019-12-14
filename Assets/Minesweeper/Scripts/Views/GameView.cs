using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View class for MVP implementation of gameplay in Minesweeper.
/// </summary>
public class GameView : MonoBehaviour
{
    public Tile _tilePrefab;
    public RectTransform _gridParent;

    public int _spacingX;
    public int _spacingY;

    public Tile [,] _gameGridTiles { get; set; }

    private Vector2 _tileDimensions;

    /// <summary>
    /// Creates a grid with given gridSize(row, column) and Tile prefabs.
    /// </summary>
    /// <param name="gridSize"></param>
    public void CreateGrid ( GridCoordinates gridSize)
    {
        _gameGridTiles = new Tile [ gridSize.row, gridSize.column ];
        _tileDimensions = _tilePrefab._rectTransform.sizeDelta;

        Vector2 lastTilePosition = Vector2.zero;
        Vector2 gridSpacing = Vector2.zero;

        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                _gameGridTiles [ i, j ] = Instantiate<Tile> ( _tilePrefab, _gridParent );
                _gameGridTiles [ i, j ]._rectTransform.anchoredPosition = new Vector2 ( lastTilePosition.x + gridSpacing.x, -( lastTilePosition.y + gridSpacing.y ) );

                _gameGridTiles [ i, j ]._coordinates.row = i;
                _gameGridTiles [ i, j ]._coordinates.column = j;

                lastTilePosition.x += _tileDimensions.x;
                gridSpacing.x += _spacingX;
            }
            lastTilePosition.x = 0;
            lastTilePosition.y += _tileDimensions.y;
            gridSpacing.x = 0;
            gridSpacing.y += _spacingY;
        }
    }

    /// <summary>
    /// Checks if all tiles were uncovered yet.
    /// </summary>
    /// <returns></returns>
    public bool AllUncoveredSuccessfully ( )
    {
        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                if ( _gameGridTiles [ i, j ]._isCovered && !_gameGridTiles [ i, j ]._isMine )
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Assigns the tile on passed gridcoordinate as a mine.
    /// </summary>
    /// <param name="gridID">coordinates of the tile</param>
    public void AssignTileAsMine ( GridCoordinates gridID )
    {
        if ( !CheckGridCellValid( gridID ) )
            return;

        _gameGridTiles [ gridID.row, gridID.column ].AssignMine ( );
    }

    /// <summary>
    /// Assigns the tile on passed gridcoordinate as a hint.
    /// </summary>
    /// <param name="gridID">coordinates of the tile</param>
    /// <param name="numberOfMinesAround"></param>
    public void AssignTileAsHint ( GridCoordinates gridID, int numberOfMinesAround )
    {
        if ( !CheckGridCellValid ( gridID ) )
            return;

        _gameGridTiles [ gridID.row, gridID.column ].SetTileHint ( numberOfMinesAround );
    }

    /// <summary>
    /// Displays flag on the tile with given coordinates.
    /// </summary>
    /// <param name="gridID">coordinates of the tile</param>
    public void AssignFlag ( GridCoordinates gridID)
    {
        if ( !CheckGridCellValid ( gridID ) )
            return;

        _gameGridTiles [ gridID.row, gridID.column ].ToggleFlag ( );
    }

    /// <summary>
    /// Uncovers the tile on the given coordinates.
    /// </summary>
    /// <param name="gridID"></param>
    public void Uncover ( GridCoordinates gridID )
    {
        if ( !CheckGridCellValid ( gridID ) )
            return;

        _gameGridTiles [ gridID.row, gridID.column ].Uncover ( );
    }

    /// <summary>
    /// Uncovers an empty tile along with all empty tiles connected to it.
    /// </summary>
    /// <param name="gridID"></param>
    public void UncoverEmpty ( GridCoordinates gridID )
    {
        if ( !CheckGridCellValid ( gridID ) )
            return;

        if ( !_gameGridTiles [ gridID.row, gridID.column ]._isCovered )
            return;

        if ( _gameGridTiles [ gridID.row, gridID.column ]._tileStatus == TileStatus.Hint )
            return;

        Uncover ( gridID );

        GridCoordinates gridLeft = gridID;
        gridLeft.column -= 1;
        GridCoordinates gridRight = gridID;
        gridRight.column += 1;
        GridCoordinates gridUp = gridID;
        gridUp.row -= 1;
        GridCoordinates gridDown = gridID;
        gridDown.row += 1;

        UncoverEmpty ( gridLeft );
        UncoverEmpty ( gridRight );
        UncoverEmpty ( gridUp );
        UncoverEmpty ( gridDown );
    }

    /// <summary>
    /// Shows all mines after losing game.
    /// </summary>
    public void UncoverAllMines ( )
    {
        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                if ( _gameGridTiles [ i, j ]._isMine )
                {
                    GridCoordinates coordinates;
                    coordinates.column = j;
                    coordinates.row = i;
                    Uncover ( coordinates );
                }
            }
        }
    }

    /// <summary>
    /// Basic check used by all functions in View to see if the passed coordinates are valid or not.
    /// </summary>
    /// <param name="gridID"></param>
    /// <returns></returns>
    private bool CheckGridCellValid ( GridCoordinates gridID )
    {
        return gridID.row >= 0 && gridID.column >= 0 && gridID.row < _gameGridTiles.GetLength ( 0 ) && gridID.column < _gameGridTiles.GetLength ( 1 );
    }
}
