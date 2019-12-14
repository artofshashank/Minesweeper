using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Game manager.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GamePresenter _gamePresenter;
    [SerializeField]
    private GameDetailsPresenter _detailsPresenter;

    private GameModel _gameModel;

    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager _instance;

    private void Awake ( )
    {
        if ( _instance == null )
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Initial setting up.
    /// </summary>
    private void Start ( )
    {
        _detailsPresenter._startGame += InitializeGame;
        _detailsPresenter._restartGame += RestartGame;

        GameDetailsModel gameDetailsModel = new GameDetailsModel ( );
        gameDetailsModel._isGameInactive = true;
        gameDetailsModel._timerInSeconds = 0;
        _detailsPresenter.Initialize ( gameDetailsModel);
    }

    /// <summary>
    /// Game is initialized with given difficulty.
    /// New GameModel is created accordingly and passed to GamePresenter.
    /// </summary>
    /// <param name="difficulty">Game Difficulty</param>
    private void InitializeGame ( GameDifficulty difficulty)
    {
        _gameModel = new GameModel ( );
        switch ( difficulty )
        {
            case GameDifficulty.Easy:
                _gameModel._gridSize.row = 5;
                _gameModel._gridSize.column = 3;
                _gameModel._numberOfBombs = 3;
                break;

            case GameDifficulty.Medium:
                _gameModel._gridSize.row = 7;
                _gameModel._gridSize.column = 5;
                _gameModel._numberOfBombs = 6;
                break;

            case GameDifficulty.Hard:
                _gameModel._gridSize.row = 10;
                _gameModel._gridSize.column = 6;
                _gameModel._numberOfBombs = 8;
                break;

            default:
                break;
        }        

        _gamePresenter.Initialize ( _gameModel );
        _detailsPresenter.DisplayMineCount ( _gameModel._numberOfBombs );
    }


    /// <summary>
    /// Restarts game by calling the scene again from scratch.
    /// </summary>
    private void RestartGame ( )
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( "MineSweeper" );
    }


    /// <summary>
    /// Left click on any Tile.
    /// Tiles call this directly through TileInput class.
    /// Function also checks whether the game is over yet.
    /// Otherwise info sent to Game Presenter for processing ahead.
    /// </summary>
    /// <param name="coordinates">coordinates of the tile.</param>
    /// <param name="tileStatus">status of the tile</param>
    public void TileLeftClicked ( GridCoordinates coordinates, TileStatus tileStatus )
    {
        _gamePresenter.HandleLeftClick ( coordinates, tileStatus );

        CheckGameStatus (tileStatus );
    }

    /// <summary>
    /// Right click on any Tile.
    /// Tiles call this directly through TileInput class.
    /// Info sent to Game Presenter for processing ahead.
    /// </summary>
    /// <param name="coordinates">coordinates of the tile.</param>
    /// <param name="tileStatus">status of the tile</param>
    public void TileRightClicked ( GridCoordinates coordinates )
    {
        _gamePresenter.HandleRightClick ( coordinates );
    }


    /// <summary>
    /// Checks if the game is over yet.
    /// Calls End game if a mine was hit or all tiles were uncovered.
    /// </summary>
    /// <param name="tileStatus"></param>
    private void CheckGameStatus ( TileStatus tileStatus )
    {
        if ( tileStatus == TileStatus.Mine )
        {
            _detailsPresenter.EndGame ( false );
        }
        else if ( _gamePresenter.IsGameSuccessful ( ) )
        {
            _detailsPresenter.EndGame ( true );
        }        
    }
}
