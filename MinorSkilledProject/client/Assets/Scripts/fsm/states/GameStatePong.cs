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
        view.exitGameButton1.onClick.AddListener(OnExitButtonClick);
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


    private void FixedUpdate()
    {

    }


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

    private void handlePlayerInputResult(PlayerInputResult pMessage)
    {
        float[] vect = new float[4] { pMessage.vector[0], pMessage.vector[1], pMessage.vector[2], pMessage.vector[3] };       
        _rigidBodyLeft.velocity = new Vector2(vect[0], vect[1]);
        _rigidBodyRight.velocity = new Vector2(vect[2], vect[3]);
    }

    //Restart game.
    private void handleRestartGame(RestartGame pRestartGame)
    {
        //view.gameBoard.ResetBoardData();
    }

    //Adds people in game lobby to the Dictionary
    private void handleNamesInGame(NamesInGame pNamesInGame)
    {
        players.Add(pNamesInGame.playerID, pNamesInGame.playerName);
        //if (pNamesInGame.playerID == 0)
        //{
        //    view.playerLabel1.text = $"{players[pNamesInGame.playerID]}";
        //}
        //else
        //{
        //    view.playerLabel2.text = $"{players[pNamesInGame.playerID]}";
        //}
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
