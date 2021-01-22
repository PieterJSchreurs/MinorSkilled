using shared;
using System.Numerics;

namespace server
{
    /// <summary>
    /// 
    /// </summary>
    public class VectorSerializeable : ASerializable
    {
        public float[] vector = new float[2] { 0, 0};
        private float[,] vector2D = new float[0, 0];

        public override void Serialize(Packet pPacket)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                pPacket.Write(vector[i]);
            }
           
        }

        public override void Deserialize(Packet pPacket)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = pPacket.ReadFloat();
            }
        }

        public float[,] convertTo3D(float[] pValue)
        {
            vector2D = new float[,] { { pValue[0], pValue[1] }  };
            return vector2D;
        }
    }
}

