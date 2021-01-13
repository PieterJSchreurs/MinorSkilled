using shared;

namespace server
{
    /// <summary>
    /// This holds the game types.
    /// </summary>
    public class GameTypes : ASerializable
    {
        public string[] gameTypes = new string[2] { "TicTacToe", "Pong" };

        public override void Serialize(Packet pPacket)
        {
            for (int i = 0; i < gameTypes.Length; i++) pPacket.Write(gameTypes[i]);
        }

        public override void Deserialize(Packet pPacket)
        {
            for (int i = 0; i < gameTypes.Length; i++) gameTypes[i] = pPacket.ReadString();
        }
    }
}

