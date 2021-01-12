namespace shared
{
    /**
     * Send from SERVER to all CLIENTS about all the names in the game.
     */
    public class NamesInGame : ASerializable
    {
        public int playerID;
        public string playerName;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playerID);
            pPacket.Write(playerName);
        }

        public override void Deserialize(Packet pPacket)
        {
            playerID = pPacket.ReadInt();
            playerName = pPacket.ReadString();
        }
    }
}
