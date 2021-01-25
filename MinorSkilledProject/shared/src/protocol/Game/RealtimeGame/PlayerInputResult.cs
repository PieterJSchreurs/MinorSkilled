namespace shared
{
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class PlayerInputResult : ASerializable
    {
        public PongData _data = new PongData();
        //public PongData pongData = new PongData();

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(_data);
            //for (int i = 0; i < _data.paddleLeftPos.Length; i++)
            //{
            //    pPacket.Write(_data.paddleRightPos[i]);
            //}
            
        }

        public override void Deserialize(Packet pPacket)
        {
            _data = pPacket.Read<PongData>();
            //for (int i = 0; i < _data.paddleLeftPos.Length; i++)
            //{
            //    _data.paddleLeftPos[i] = pPacket.ReadFloat();
            //}
            ////pongData = pPacket.Read<PongData>();
        }
    }
}
