using shared;
using UnityEngine;

/**
 * 'Chat' state while you are waiting to start a game where you can signal that you are ready or not.
 */
public class LobbyState : ApplicationStateWithView<LobbyView>
{
    [Tooltip("Should we enter the lobby in a ready state or not?")]
    [SerializeField] private bool autoQueueForGame = false;

    public override void EnterState()
    {
        base.EnterState();
        GameListRequest gameListRequest = new GameListRequest();
        fsm.channel.SendMessage(gameListRequest);
        view.SetLobbyHeading("Welcome to the Lobby...");
        view.ClearOutput();
        view.AddOutput($"Server settings:"+fsm.channel.GetRemoteEndPoint());
        view.SetReadyToggle(autoQueueForGame);

        view.OnChatTextEntered += onTextEntered;
        view.OnReadyToggleClicked += onReadyToggleClicked;

        if (autoQueueForGame)
        {
            onReadyToggleClicked(true);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        
        view.OnChatTextEntered -= onTextEntered;
        view.OnReadyToggleClicked -= onReadyToggleClicked;
    }

    /**
     * Called when you enter text and press enter.
     */
    private void onTextEntered(string pText)
    {
        view.ClearInput();
        //Send text
        ChatMessage msg = new ChatMessage();
        msg.message = pText;
        fsm.channel.SendMessage(msg);
    }

    /**
     * Called when you click on the ready checkbox
     */
    private void onReadyToggleClicked(bool pNewValue)
    {
        ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
        msg.ready = pNewValue;
        fsm.channel.SendMessage(msg);
    }

    private void FillDropDown(string pName)
    {
        view.AddGameToDropdown(pName);
    }

    private void addOutput(string pInfo)
    {
        view.AddOutput(pInfo);
    }

    /// //////////////////////////////////////////////////////////////////
    ///                     NETWORK MESSAGE PROCESSING
    /// //////////////////////////////////////////////////////////////////

    private void Update()
    {
        receiveAndProcessNetworkMessages();
    }
    
    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent(pMessage as RoomJoinedEvent);
        else if (pMessage is LobbyInfoUpdate) handleLobbyInfoUpdate(pMessage as LobbyInfoUpdate);
        else if (pMessage is GameListAnswer) handleGameListAnswer(pMessage as GameListAnswer);
    }

    private void handleChatMessage(ChatMessage pMessage)
    {
        //just show the message
        addOutput(pMessage.message);
    }

    private void handleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        //did we move to the game room?
        if (pMessage.room == RoomJoinedEvent.Room.GAME_ROOM)
        {
            fsm.ChangeState<GameState>();
        }
    }

    private void handleLobbyInfoUpdate(LobbyInfoUpdate pMessage)
    {
        //update the lobby heading
        view.SetLobbyHeading($"Welcome to the Lobby ({pMessage.memberCount} people, {pMessage.readyCount} ready)");
    }

    private void handleGameListAnswer(GameListAnswer pMessage)
    {
        foreach (string pGame in pMessage.gameTypes.gameTypes)
        {
            FillDropDown(pGame);
        }
    }

}
