using shared;
using System.Collections.Generic;

namespace server
{
    /**
	 * The LobbyRoom is a little bit more extensive than the LoginRoom.
	 * In this room clients change their 'ready status'.
	 * If enough people are ready, they are automatically moved to the GameRoom to play a Game (assuming a game is not already in play).
	 */
    class LobbyRoom : SimpleRoom
    {
        //this list keeps tracks of which players are ready to play a game, this is a subset of the people in this room
        private List<TcpMessageChannel> _readyMembers = new List<TcpMessageChannel>();
        public LobbyRoom(TCPGameServer pOwner) : base(pOwner)
        { 
        }

        protected override void addMember(TcpMessageChannel pMember)
        {
            base.addMember(pMember);

            //tell the member it has joined the lobby
            RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
            roomJoinedEvent.room = RoomJoinedEvent.Room.LOBBY_ROOM;
            pMember.SendMessage(roomJoinedEvent);

            //print some info in the lobby (can be made more applicable to the current member that joined)
            ChatMessage simpleMessage = new ChatMessage();
            simpleMessage.message = _server.GetPlayerInfo(pMember).userName + " has joined the lobby!";
            sendToAll(simpleMessage);

            //send information to all clients that the lobby count has changed
            sendLobbyUpdateCount();
        }

        /**
		 * Override removeMember so that our ready count and lobby count is updated (and sent to all clients)
		 * anytime we remove a member.
		 */
        protected override void removeMember(TcpMessageChannel pMember)
        {
            base.removeMember(pMember);
            _readyMembers.Remove(pMember);

            sendLobbyUpdateCount();
        }

        protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
        {
            if (pMessage is ChangeReadyStatusRequest) handleReadyNotification(pMessage as ChangeReadyStatusRequest, pSender);
            else if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage, pSender);
            else if (pMessage is GameListRequest) handleGameTypes(pMessage as GameListRequest, pSender);
        }

        /// <summary>
        /// Handles the setting ready of the player and sending them to the rooms.
        /// TO-DO: Handle the individuals not queuing for the same game.
        /// </summary>
        /// <param name="pReadyNotification"></param>
        /// <param name="pSender"></param>
        private void handleReadyNotification(ChangeReadyStatusRequest pReadyNotification, TcpMessageChannel pSender)
        {
            //if the given client was not marked as ready yet, mark the client as ready
            if (pReadyNotification.ready)
            {
                if (!_readyMembers.Contains(pSender)) _readyMembers.Add(pSender);
            }
            else //if the client is no longer ready, unmark it as ready
            {
                _readyMembers.Remove(pSender);
            }

            //do we have enough people for a game and is the gameroom not already full (hacky, part of the assignment to 'fix')
            //Potential: Look at the number of players needed for a room and set the count to that.
            if (_readyMembers.Count >= 2)
            {
                TcpMessageChannel player1 = _readyMembers[0];
                TcpMessageChannel player2 = _readyMembers[1];
                removeMember(player1);
                removeMember(player2);
                _server.StartGameRoom().StartGame(player1, player2);
            }

            //(un)ready-ing / starting a game changes the lobby/ready count so send out an update
            //to all clients still in the lobby
            sendLobbyUpdateCount();
        }

        /// <summary>
        /// Sends the game types to the client.
        /// </summary>
        /// <param name="gameListRequest"></param>
        /// <param name="pSender"></param>
        private void handleGameTypes(GameListRequest gameListRequest ,TcpMessageChannel pSender)
        {
            GameListAnswer gameListAnswer = new GameListAnswer();
            gameListAnswer.gameTypes = new GameTypes();
            gameListAnswer.gameTypes.gameTypes = new string[] { "TicTacToe" , "Pong" };
            pSender.SendMessage(gameListAnswer);
        }

        private void handleChatMessage(ChatMessage pChatMessage, TcpMessageChannel pSender)
        {
            sendChatMessage(pChatMessage, pSender);
        }

        private void sendLobbyUpdateCount()
        {
            LobbyInfoUpdate lobbyInfoMessage = new LobbyInfoUpdate();
            lobbyInfoMessage.memberCount = memberCount;
            lobbyInfoMessage.readyCount = _readyMembers.Count;
            sendToAll(lobbyInfoMessage);
        }

        private void RestartGame()
        {
            RestartGame restartGame = new RestartGame();
            restartGame.restart = true;
            sendToAll(restartGame);
        }

    }
}
