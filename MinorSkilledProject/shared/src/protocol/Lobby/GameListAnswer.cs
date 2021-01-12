using System.Collections.Generic;

namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on which games are available for picking.
	 *
	 */
	public class GameListAnswer : ASerializable
	{
		private string gameNames;
		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(gameNames);
		}

		public override void Deserialize(Packet pPacket)
		{
			gameNames = pPacket.ReadString();
		}
	}
}
