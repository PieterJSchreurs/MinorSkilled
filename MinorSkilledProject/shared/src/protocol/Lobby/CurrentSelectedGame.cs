using server;
using System.Collections.Generic;

namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on which games are available for picking.
	 *
	 */
	public class CurrentSelectedGame : ASerializable
	{

		public GameTypes _gameType;
		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(_gameType);
		}

		public override void Deserialize(Packet pPacket)
		{
			_gameType = pPacket.Read<GameTypes>();
		}
	}
}
