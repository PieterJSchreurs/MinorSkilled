using server;

namespace shared
{
    /**
     * Send from CLIENT to SERVER to request enabling/disabling the ready status.
     */
    public class ChangeReadyStatusRequest : ASerializable
    {
        public bool ready = false;
        public int gameSelected = 0;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(ready);
            pPacket.Write(gameSelected);
        }

        public override void Deserialize(Packet pPacket)
        {
            ready = pPacket.ReadBool();
            gameSelected = pPacket.ReadInt();
        }
    }
}
