using server;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on which games are available for picking.
	 *
	 */
	public class GameListAnswer : ASerializable
	{

		public GameTypes gameTypes;
		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(gameTypes);
		}

		public override void Deserialize(Packet pPacket)
		{
			gameTypes = pPacket.Read<GameTypes>();
		}
	}
}
