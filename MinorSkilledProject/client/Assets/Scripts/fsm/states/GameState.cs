using shared;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/**
 * This is where we 'play' a game.
 */
public class GameState : ApplicationStateWithView<GameView>
{
    //just for fun we keep track of how many times a player clicked the board
    //note that in the current application you have no idea whether you are player 1 or 2
    //normally it would be better to maintain this sort of info on the server if it is actually important information
    private int player1MoveCount = 0;
    private int player2MoveCount = 0;
    private Dictionary<int, string> players = new Dictionary<int, string>();

    public override void EnterState()
    {
        base.EnterState();

        view.gameBoard.OnCellClicked += _onCellClicked;
        view.exitGameButton1.onClick.AddListener(OnExitButtonClick);
    }

    private void _onCellClicked(int pCellIndex)
    {
        MakeMoveRequest makeMoveRequest = new MakeMoveRequest();
        makeMoveRequest.move = pCellIndex;

        fsm.channel.SendMessage(makeMoveRequest);
    }

    public override void ExitState()
    {
        base.ExitState();
        view.gameBoard.OnCellClicked -= _onCellClicked;
    }

    private void Update()
    {
        receiveAndProcessNetworkMessages();
    }

    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is MakeMoveResult)
        {
            handleMakeMoveResult(pMessage as MakeMoveResult);
        }
        if (pMessage is ChatMessage)
        {
            handleChatMessage(pMessage as ChatMessage);
        }
        if (pMessage is TurnNameSender)
        {
            handleTurnNameSender(pMessage as TurnNameSender);
        }
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
    }

    //Tictactoe specific.
    private void handleMakeMoveResult(MakeMoveResult pMakeMoveResult)
    {
        view.gameBoard.SetBoardData(pMakeMoveResult.boardData);
    }

    //Restart game.
    private void handleRestartGame(RestartGame pRestartGame)
    {
        //view.gameBoard.ResetBoardData();
    }
    //Tictactoe specific.
    //Handles the who has won message.
    private void handleChatMessage(ChatMessage pChatMessage)
    {
        view.gameMessageLabel1.text = pChatMessage.message;
    }

    //Tictactoe specific.
    //Handles whose turn it is.
    private void handleTurnNameSender(TurnNameSender pTurnName)
    {
        if (pTurnName.playerID == 0)
        {
            view.playerLabel1.text = $"{players[pTurnName.playerID]} it's NOT your turn.";
            view.playerLabel2.text = $"{players[pTurnName.playerID + 1]} it's your turn.";
        }
        if (pTurnName.playerID == 1)
        {
            view.playerLabel2.text = $"{players[pTurnName.playerID]} it's NOT your turn.";
            view.playerLabel1.text = $"{players[pTurnName.playerID - 1]} it's your turn.";
        }
    }

    //Adds people in game lobby to the Dictionary
    private void handleNamesInGame(NamesInGame pNamesInGame)
    {
        players.Add(pNamesInGame.playerID, pNamesInGame.playerName);
        if (pNamesInGame.playerID == 0)
        {
            view.playerLabel1.text = $"{players[pNamesInGame.playerID]}";
        }
        else
        {
            view.playerLabel2.text = $"{players[pNamesInGame.playerID]}";
        }
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
