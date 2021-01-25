using System;
using System.Numerics;

namespace shared
{
    
    public class PongData : ASerializable
    {
        /// <summary>
        /// Default state positions.
        /// </summary>
        public float[] paddleLeftPos = new float[2] { -5, 0 };
        public float[] paddleRightPos = new float[2] { 5, 0 };
        public float[] ballVelocity = new float[2] { 0, 0 };
        private float _edge = 2.25f;

        public void PlayerInput(int pInput, int pPlayer)
        {
            //Left
            if (pPlayer == 0)
            {
                if (pInput == 1)
                {
                    paddleLeftPos = new float[2] { paddleLeftPos[0] + 0, paddleLeftPos[1] + 0.06f };
                }
                else if (pInput == 2)
                {
                    paddleLeftPos = new float[2] { paddleLeftPos[0] + 0, paddleLeftPos[1] -0.06f };
                }
                else
                {
                    paddleLeftPos = new float[2] { paddleLeftPos[0], paddleLeftPos[1] };
                }
                if (paddleLeftPos[1] > _edge) paddleLeftPos[1] = _edge;
                if (paddleLeftPos[1] < -_edge) paddleLeftPos[1] = -_edge;
            }
            if (pPlayer == 1)
            {
                if (pInput == 1)
                {
                    paddleRightPos = new float[2] { paddleRightPos[0] + 0, paddleRightPos[1] + 0.06f };
                }
                else if (pInput == 2)
                {
                    paddleRightPos = new float[2] { paddleRightPos[0] + 0, paddleRightPos[1] -0.06f };
                }
                else
                {
                    paddleRightPos = new float[2] { paddleRightPos[0], paddleRightPos[1] };
                }
                if(paddleRightPos[1] > _edge) paddleRightPos[1] = _edge;
                if (paddleRightPos[1] < -_edge) paddleRightPos[1] = -_edge;
            }
        }

        /// <summary>
        /// Serialize the positions of the paddles.
        /// </summary>
        /// <param name="pPacket"></param>
        public override void Serialize(Packet pPacket)
        {
            for (int i = 0; i < paddleLeftPos.Length; i++)
            {
                pPacket.Write(paddleLeftPos[i]);
            }
            for (int i = 0; i < paddleRightPos.Length; i++)
            {
                pPacket.Write(paddleRightPos[i]);
            }
        }

        /// <summary>
        /// Deserialize the positions of the paddles.
        /// </summary>
        /// <param name="pPacket"></param>
        public override void Deserialize(Packet pPacket)
        {
            for (int i = 0; i < paddleLeftPos.Length; i++)
            {
                paddleLeftPos[i] = pPacket.ReadFloat();
            }
            for (int i = 0; i < paddleRightPos.Length; i++)
            {
                paddleRightPos[i] = pPacket.ReadFloat();
            }
        }

        public override string ToString()
        {
            return GetType().Name + ":" + string.Join(",", board);
        }

        private void resetBoardData()
        {
            
        }
    }
}

