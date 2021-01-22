namespace shared
{
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class PlayerInputResult : ASerializable
    {
        public int whoMadeTheMove = 0;
        public float[] vector = new float[4] { 0, 0, 0, 0 };
        //public PongData pongData = new PongData();

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(whoMadeTheMove);
            for (int i = 0; i < vector.Length; i++)
            {
                pPacket.Write(vector[i]);
            }
            //pPacket.Write(pongData);
        }

        public override void Deserialize(Packet pPacket)
        {
            whoMadeTheMove = pPacket.ReadInt();
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = pPacket.ReadFloat();
            }
            //pongData = pPacket.Read<PongData>();
        }
    }
}
