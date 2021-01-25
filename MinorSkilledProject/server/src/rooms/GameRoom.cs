using shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace server
{
    /// <summary>
    /// This is a gameroom in which a Game can run.
    /// </summary>
    class GameRoom : Room
    {
        public bool IsGameInPlay { get; private set; }
        //wraps the board to play on...
        private TicTacToeBoard _board = new TicTacToeBoard();
        private PongData _pongData = new PongData();
        private List<TcpMessageChannel> _players = new List<TcpMessageChannel>();
        private TCPGameServer _gameServer;
        private PlayerInputResult _playerInputResult = new PlayerInputResult();
        private bool _gameOver = false;
        private bool _gameAborted = false;
        private bool _updated = false;

        public GameRoom(TCPGameServer pOwner) : base(pOwner)
        {
            _gameServer = pOwner;
        }

        public void StartGame(TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2)
        {
            if (IsGameInPlay) throw new Exception("Programmer error duuuude.");
            if (_gameAborted)
            { _board = new TicTacToeBoard(); _gameAborted = false; _pongData = new PongData(); }

            _players.Add(pPlayer1);
            _players.Add(pPlayer2);
            IsGameInPlay = true;
            addMember(pPlayer1);
            addMember(pPlayer2);
            sendPeopleInGame();
            resetBoard();
        }

        protected override void addMember(TcpMessageChannel pMember)
        {
            base.addMember(pMember);
            //notify client he has joined a game room 
            RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
            roomJoinedEvent.room = RoomJoinedEvent.Room.GAME_ROOM;
            pMember.SendMessage(roomJoinedEvent);
        }

        private void resetBoard()
        {
            //Resets the board.
        }

        private void sendPeopleInGame()
        {
            foreach (TcpMessageChannel pPlayer in _players)
            {
                NamesInGame namesInGame = new NamesInGame();
                namesInGame.playerID = indexOfMember(pPlayer);
                namesInGame.playerName = _gameServer.GetPlayerInfo(pPlayer).userName;
                sendToAll(namesInGame);
            }
        }

        public override void Update()
        {
            //demo of how we can tell people have left the game...
            int oldMemberCount = memberCount;
            base.Update();
            int newMemberCount = memberCount;

            if (oldMemberCount != newMemberCount)
            {
                Log.LogInfo("People left the game...", this);
            }
            if (!_updated)
            {
                UpdatePlayersBoard();
                _updated = true;
            }
        }

        /// <summary>
        /// Handles incoming data messages.
        /// Checks of which type the incoming message is and sents it to accordingly to the right handler.
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pSender"></param>
        protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
        {
            if (pMessage is MakeMoveRequest)
            {
                handleMakeMoveRequest(pMessage as MakeMoveRequest, pSender);
            }
            else if (pMessage is QuitGameRequest)
            {
                handleQuitRequest(pMessage as QuitGameRequest, pSender);
            }
            else if (pMessage is PlayerInput)
            {
                handlePlayerInput(pMessage as PlayerInput, pSender);
            }
        }

        /// <summary>
        /// Only fires when a user input is handled.
        /// Gets the playerinput and parses it to the board.
        /// </summary>
        /// <param name="pPlayerInput"></param>
        /// <param name="pSender"></param>
        private void handlePlayerInput(PlayerInput pPlayerInput, TcpMessageChannel pSender)
        {
            _pongData.PlayerInput(pPlayerInput.playerInput, _players.IndexOf(pSender));
            _updated = false;
        }

        /// <summary>
        /// Looks if the last player is gone from the server and deletes the room to saves resources.
        /// Otherwise it should handle a pop up to the player.
        /// </summary>
        /// <param name="quitGameRequest"></param>
        /// <param name="pSender"></param>
        private void handleQuitRequest(QuitGameRequest quitGameRequest, TcpMessageChannel pSender)
        {
            _players.Remove(pSender);
            removeMember(pSender);
            _server.GetLobbyRoom().AddMember(pSender);
            _gameAborted = true;
            //Remove game 
            if (_players.Count <= 0)
            {
                this.IsGameInPlay = false;
                _server.RemoveGameRoom(this);

            }
        }

        /// <summary>
        /// The server received a request to make a move from a player.
        /// It will in turn try to make the move onto the board data.
        /// If the move is allowed, it will then return the result of the move.
        /// 
        /// It will also inform the player via a ChatMessage if a player has won or the move was invalid.
        /// Updates whose turn it is.
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pSender"></param>
        private void handleMakeMoveRequest(MakeMoveRequest pMessage, TcpMessageChannel pSender)
        {
            if (!_gameOver)
            {
                //we have two players, so index of sender is 0 or 1, which means playerID becomes 1 or 2
                int playerID = indexOfMember(pSender) + 1;
                //Is it players turn?
                bool canMakeMove = _board.isValidMove(pMessage.move, indexOfMember(pSender));

                if (canMakeMove)
                {
                    //make the requested move (0-8) on the board for the player
                    _board.MakeMove(pMessage.move, playerID);
                    //and send the result of the boardstate back to all clients
                    MakeMoveResult makeMoveResult = new MakeMoveResult();
                    makeMoveResult.whoMadeTheMove = playerID;
                    makeMoveResult.boardData = _board.GetBoardData();
                    sendToAll(makeMoveResult);
                    TurnNameSender turnNameSender = new TurnNameSender();
                    turnNameSender.playerID = _board.GetCurrentPlayer();
                    sendToAll(turnNameSender);
                    if (_board.HasWon())
                    {
                        //Handle player won.
                        ChatMessage msg = new ChatMessage();
                        msg.message = (_gameServer.GetPlayerInfo(_players[_board.GetBoardData().WhoHasWon() - 1]).userName + " has won.");
                        sendToAll(msg);
                        _gameOver = true;
                    } else
                    {
                        //Clear chatbox.
                        ChatMessage msg = new ChatMessage();
                        msg.message = ("");
                        sendToAll(msg);
                    }
                }
                else
                {
                    ChatMessage msg = new ChatMessage();
                    msg.message = ("Invalid move");
                    sendChatMessage(msg, _players[_board.GetCurrentPlayer()]);
                }
            }
        }

        /// <summary>
        /// Sends the updated data of the pong board to all players.
        /// </summary>
        private void UpdatePlayersBoard()
        {
            _playerInputResult._data = _pongData;
            sendToAll(_playerInputResult);
        }
    }
}
