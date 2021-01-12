using System;

namespace shared
{
    /**
	 * Super simple board model for TicTacToe that contains the mimimal data to actually represent the board. 
	 */
    public class TicTacToeBoardData : ASerializable
    {
        //board representation in 1d array, one element for each cell
        //0 is empty, 1 is player 1, 2 is player 2
        //might be that for your game, a 2d array is actually better
        public int[] board = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /**
		 * Returns who has won.
		 */
        public int WhoHasWon()
        {
            //Horizontal
            for (int row = 0; row < 3; row++)
            {
                if (board[(row * 3) + 0] == board[(row * 3) + 1] && board[(row * 3) + 0] == board[(row * 3) + 2] && board[(row * 3) + 0] != 0)
                {
                    return board[(row * 3) + 0];
                }
            }
            //Horizontal
            for (int col = 0; col < 3; col++)
            {
                if (board[col] == board[col + 3] && board[col] == board[col + 6] && board[col] != 0)
                {
                    return board[col];
                }
            }

            //Diagonals
            if (board[0] == board[4] && board[0] == board[8] && board[0] != 0)
            {
                return board[0];
            }
            else if (board[2] == board[4] && board[2] == board[6] && board[2] != 0)
            {
                //Midle row winner.
                return board[2];
            }
            return 0;
        }

        private int getIndexNumber(int pRow, int pCol)
        {
            int index = (pRow * 3) + pCol;
            return board[index];
        }

        public override void Serialize(Packet pPacket)
        {
            for (int i = 0; i < board.Length; i++) pPacket.Write(board[i]);
        }

        public override void Deserialize(Packet pPacket)
        {
            for (int i = 0; i < board.Length; i++) board[i] = pPacket.ReadInt();
        }

        public override string ToString()
        {
            return GetType().Name + ":" + string.Join(",", board);
        }

        private void resetBoardData()
        {
            board = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
    }
}

