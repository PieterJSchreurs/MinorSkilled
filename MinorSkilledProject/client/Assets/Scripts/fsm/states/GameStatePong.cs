using server;
using shared;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * This is where we 'play' a game.
 */
public class GameStatePong : ApplicationStateWithView<GameView>
{
    //Game state for the live game.
    private Dictionary<int, string> players = new Dictionary<int, string>();
    public Rigidbody2D _rigidBodyLeft;
    public Rigidbody2D _rigidBodyRight;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    private bool _isMoving = true;

    public override void EnterState()
    {
        base.EnterState();
        view.exitGameButton2.onClick.AddListener(OnExitButtonClick);
        view._gamePongObject.SetActive(true);
        view._gameTicTacToeObject.SetActive(false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        receiveAndProcessNetworkMessages();
        if (Input.GetKey(moveUp))
        {
            PlayerInput playerInput = new PlayerInput();
            playerInput.playerInput = 1;
            fsm.channel.SendMessage(playerInput);

        }
        if (Input.GetKey(moveDown))
        {
            PlayerInput playerInput = new PlayerInput();
            playerInput.playerInput = 2;
            fsm.channel.SendMessage(playerInput);

        }
        else if (Input.GetKeyUp(moveDown) || Input.GetKeyUp(moveUp))
        {
            PlayerInput playerInput = new PlayerInput();
            playerInput.playerInput = 0;
            fsm.channel.SendMessage(playerInput);
        }
    }

    /// <summary>
    /// Handles incoming network messages after it checks which protocol it is.
    /// </summary>
    /// <param name="pMessage"></param>
    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is NamesInGame)
        {
            handleNamesInGame(pMessage as NamesInGame);
        }
        if (pMessage is QuitGameRequest)
        {
            handleQuitGame(pMessage as QuitGameRequest);
        }
        if (pMessage is RestartGame)
        {
            handleRestartGame(pMessage as RestartGame);
        }
        if (pMessage is PlayerInputResult)
        {
            handlePlayerInputResult(pMessage as PlayerInputResult);
        }
    }

    /// <summary>
    /// Updates the paddle positions with the position given by the server.
    /// </summary>
    /// <param name="pMessage"></param>
    private void handlePlayerInputResult(PlayerInputResult pMessage)
    {   
        _rigidBodyLeft.position = new Vector2(pMessage._data.paddleLeftPos[0], pMessage._data.paddleLeftPos[1]);
        _rigidBodyRight.position = new Vector2(pMessage._data.paddleRightPos[0], pMessage._data.paddleRightPos[1]);
    }

    //Restart game.
    private void handleRestartGame(RestartGame pRestartGame)
    {

    }

    //Adds people in game lobby to the Dictionary
    private void handleNamesInGame(NamesInGame pNamesInGame)
    {
        players.Add(pNamesInGame.playerID, pNamesInGame.playerName);
    }

    private void handleQuitGame(QuitGameRequest pQuitGameRequest)
    {

    }

    private void OnExitButtonClick()
    {
        players.Clear();
        QuitGameRequest quitRequest = new QuitGameRequest();
        quitRequest.quit = true;
        view.gameBoard.ResetBoardData();
        fsm.channel.SendMessage(quitRequest);
        fsm.ChangeState<LobbyState>();
    }
}
