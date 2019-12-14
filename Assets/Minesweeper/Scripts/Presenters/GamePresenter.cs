using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Presenter class for MVP implementation of gameplay in Minesweeper.
/// </summary>
public class GamePresenter : MonoBehaviour
{
    private GameModel _model;
    private GameView _view;
   
    /// <summary>
    /// Initializes Presenter and creates gameplay grid.
    /// </summary>
    /// <param name="model">Game Model</param>
    public void Initialize ( GameModel model)
    {
        _model = model;
        _view = GetComponentInChildren<GameView>();

        CreateGame ( );
    }

    /// <summary>
    /// Handles left click on a Tile and calls suitable function on GameView.
    /// </summary>
    /// <param name="coordinates">coordinates of tile.</param>
    /// <param name="status">status of the tile</param>
    public void HandleLeftClick ( GridCoordinates coordinates, TileStatus status )
    {
        if ( status == TileStatus.Hint )
        {
            _view.Uncover ( coordinates );
        }
        else if ( status == TileStatus.Empty )
        {
            _view.UncoverEmpty ( coordinates );
        }
        else if ( status == TileStatus.Mine )
        {
            _view.UncoverAllMines ( );
        }
    }

    /// <summary>
    /// Handles right click on a Tile and calls Flag on GameView.
    /// </summary>
    /// <param name="coordinates">coordinates of tile.</param>
    public void HandleRightClick ( GridCoordinates coordinates )
    {
        _view.AssignFlag ( coordinates );
    }

    /// <summary>
    /// Returns true if all tiles were uncovered.
    /// </summary>
    /// <returns>Bool true/false if game was/wasn't successful</returns>
    public bool IsGameSuccessful ( )
    {
        return _view.AllUncoveredSuccessfully ( );
    }

    /// <summary>
    /// Creates game.
    /// </summary>
    private void CreateGame ( )
    {
        _view.CreateGrid (_model._gridSize );
        AssignMines ( );
        AssignHints ( );
    }

    /// <summary>
    /// Randomly places mines on tiles.
    /// </summary>
    private void AssignMines ( )
    {
        List<GridCoordinates> minesCoordinates = new List<GridCoordinates> ( );
        GridCoordinates tempCoordinates;
        for ( int i = 0; i < _model._numberOfBombs; i++ )
        {
            do
            {
                tempCoordinates = GetRandomGridCoordinates ( );
            } while ( minesCoordinates.Contains ( tempCoordinates ) );

            minesCoordinates.Add ( tempCoordinates );
        }

        foreach ( GridCoordinates gc in minesCoordinates )
        {
            _view.AssignTileAsMine ( gc );
        }
    }

    /// <summary>
    /// Random tile coordinates are generated.
    /// </summary>
    /// <returns></returns>
    private GridCoordinates GetRandomGridCoordinates ( )
    {
        GridCoordinates gc;

        gc.row = Random.Range ( 0, _model._gridSize.row - 1 );
        gc.column = Random.Range(0, _model._gridSize.column - 1 );

        return gc;
    }

    /// <summary>
    /// Checks for each tile that is not a mine, whether a mine is placed in any of the 8 tiles around it, and adds a count on the tile's view.
    /// </summary>
    private void AssignHints ( )
    {
        for ( int i = 0; i < _model._gridSize.row; i++ )
        {
            for ( int j = 0; j < _model._gridSize.column; j++ )
            {
                if ( _view._gameGridTiles [ i, j ]._isMine )
                    continue;

                int mineCount = 0;

                if ( j - 1 >= 0 && _view._gameGridTiles [ i, j - 1 ]._isMine ) //left
                    mineCount++;
                if ( j + 1 < _view._gameGridTiles.GetLength ( 1 ) && _view._gameGridTiles [ i, j + 1 ]._isMine )//right
                    mineCount++;

                if ( i - 1 >= 0 )
                {
                    if ( _view._gameGridTiles [ i - 1, j ]._isMine )//up-center
                        mineCount++;
                    if ( j - 1 >= 0 && _view._gameGridTiles [ i - 1, j - 1 ]._isMine ) //up-left
                        mineCount++;
                    if ( j + 1 < _view._gameGridTiles.GetLength ( 1 ) && _view._gameGridTiles [ i - 1, j + 1 ]._isMine )//up-right
                        mineCount++;
                }

                if ( i + 1 < _view._gameGridTiles.GetLength ( 0 ) )
                {
                    if ( _view._gameGridTiles [ i + 1, j ]._isMine )//down-center
                        mineCount++;
                    if ( j - 1 >= 0 && _view._gameGridTiles [ i + 1, j - 1 ]._isMine )//down-left
                        mineCount++;
                    if ( j + 1 < _view._gameGridTiles.GetLength ( 1 ) && _view._gameGridTiles [ i + 1, j + 1 ]._isMine )//down-right
                        mineCount++;
                }

                GridCoordinates gc;
                gc.row = i;
                gc.column = j;
                if(mineCount > 0)
                    _view.AssignTileAsHint ( gc, mineCount );
            }
        }
    }
}
