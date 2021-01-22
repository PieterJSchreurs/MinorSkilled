using System;
using System.Numerics;

namespace shared
{
    
    public class PongData : ASerializable
    {
        public float[] paddleLeftVelocity = new float[2];
        public float[] paddleRightVelocity = new float[2];
        //public float[] ballVelocity = new float[3];

        public void PlayerInput(int pInput, int pPlayer)
        {
            //Left
            if (pPlayer == 0)
            {
                if (pInput == 1)
                {
                    paddleLeftVelocity = new float[2] { 0, 10 };
                }
                else if (pInput == 2)
                {
                    paddleLeftVelocity = new float[2] { 0, -10 };
                }
                else
                {
                    paddleLeftVelocity = new float[2] { 0, 0 };
                }
            }
            if (pPlayer == 1)
            {
                if (pInput == 1)
                {
                    paddleRightVelocity = new float[2] { 0, 10 };
                }
                else if (pInput == 2)
                {
                    paddleRightVelocity = new float[2] { 0, -10 };
                }
                else
                {
                    paddleRightVelocity = new float[2] { 0, 0 };
                }
            }
        }

        public override void Serialize(Packet pPacket)
        {
            for (int i = 0; i < paddleLeftVelocity.Length; i++)
            {
                pPacket.Write(paddleLeftVelocity[i]);
            }
            for (int i = 0; i < paddleRightVelocity.Length; i++)
            {
                pPacket.Write(paddleRightVelocity[i]);
            }
        }

        public override void Deserialize(Packet pPacket)
        {
            for (int i = 0; i < paddleLeftVelocity.Length; i++)
            {
                paddleLeftVelocity[i] = pPacket.ReadFloat();
            }
            for (int i = 0; i < paddleRightVelocity.Length; i++)
            {
                paddleRightVelocity[i] = pPacket.ReadFloat();
            }
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        private void resetBoardData()
        {
            
        }
    }
}

