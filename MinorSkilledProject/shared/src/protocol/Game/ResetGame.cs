namespace shared
{
    /**
     * Send from SERVER to all CLIENTS who's turn it is.
     */
    public class RestartGame : ASerializable
    {
        public bool restart;
        
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(restart);
        }

        public override void Deserialize(Packet pPacket)
        {
            restart = pPacket.ReadBool();
        }
    }
}
