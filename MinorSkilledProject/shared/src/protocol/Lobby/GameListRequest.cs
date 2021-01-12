namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on which games are available for picking.
	 *
	 */
	public class GameListRequest : ASerializable
	{
		bool request;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(request);
		}

		public override void Deserialize(Packet pPacket)
		{
			request = pPacket.ReadBool();
		}
	}
}
