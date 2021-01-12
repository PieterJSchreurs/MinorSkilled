namespace shared
{
    /**
     * Send from SERVER to all CLIENTS who's turn it is.
     */
    public class QuitGameRequest : ASerializable
    {
        public bool quit;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(quit);
        }

        public override void Deserialize(Packet pPacket)
        {
            quit = pPacket.ReadBool();
        }
    }
}
