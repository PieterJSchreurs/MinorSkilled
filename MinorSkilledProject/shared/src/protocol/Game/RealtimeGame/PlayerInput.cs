namespace shared
{
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class PlayerInput : ASerializable
    {
        public int playerInput;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playerInput);
        }

        public override void Deserialize(Packet pPacket)
        {
            playerInput = pPacket.ReadInt();
        }
    }
}
