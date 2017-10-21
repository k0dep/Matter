using System;

namespace MatterCore.Routes
{
    public class SwitchZeroRoute : IRoute
    {
        public float Epsilon { get; set; }
        public bool DataIsFloat { get; set; }

        public int ZeroEqualLabel { get; set; }
        public int ZeroEqualPort { get; set; }

        public int NotZeroEqualLabel { get; set; }
        public int NotZeroEqualPort { get; set; }

        public SwitchZeroRoute(int zeroEqualLabel, int zeroEqualPort, int notZeroEqualLabel, int notZeroEqualPort, bool dataIsFloat = false, float epsilon = 0.000001f)
        {
            ZeroEqualLabel = zeroEqualLabel;
            ZeroEqualPort = zeroEqualPort;
            NotZeroEqualLabel = notZeroEqualLabel;
            NotZeroEqualPort = notZeroEqualPort;
            Epsilon = epsilon;
        }

        public void Route(IPacket packet, ICore core)
        {
            var port = NotZeroEqualPort;
            if (IsZero(packet.Data))
            {
                port = ZeroEqualPort;
                packet.Label = ZeroEqualLabel;
            }
            else
                packet.Label = NotZeroEqualLabel;


            core.OutPorts.Enqueue(port, packet);
        }

        public bool IsZero(int data)
        {
            if (DataIsFloat)
                return Math.Abs(BitConverter.ToSingle(BitConverter.GetBytes(data), 0)) < Epsilon;
            return data == 0;
        }
    }
}