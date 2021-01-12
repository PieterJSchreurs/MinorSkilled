namespace shared
{
    /**
     * Send from SERVER to all CLIENTS who's turn it is.
     */
    public class TurnPlayerID : ASerializable
    {
        public int playerID;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playerID);
        }

        public override void Deserialize(Packet pPacket)
        {
            playerID = pPacket.ReadInt();
        }
    }
}
