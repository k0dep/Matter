
using MatterCore;
using Moq;
using MtrCore.Impl.Ports;
using MtrCore.Interfaces;
using NUnit.Framework;

namespace MtrCoreTest.Ports
{
    
    [TestFixture]
    public class PortBlockTest
    {
        [Test]
        public void PortConnection()
        {
            var inPort = new InPort();
            var outPort = new OutPort();

            outPort.Connect(inPort);

            var rised = false;
            inPort.OnRecv += packet => rised = true;
            
            outPort.Transmitt(null);

            Assert.IsTrue(rised);
        }

        [Test]
        public void InPortBlock()
        {
            var portFactory = new Mock<IPortFactory>();
            portFactory.Setup(t => t.CreateInputPort()).Returns(() => new InPort());
            portFactory.Setup(t => t.CreateOutputPort()).Returns(() => new OutPort());

            var inPortBlock = new InPortBlock(2, portFactory.Object);
            var outPortBlock = new OutPortBlock(2, portFactory.Object);

            inPortBlock.Connect(0, outPortBlock, 1);

            var packet = Mock.Of<IPacket>();

            outPortBlock.Enqueue(1, packet);

            outPortBlock.Tick();

            Assert.AreEqual(1, inPortBlock.Count());
            Assert.AreEqual(packet, inPortBlock.DequeuePacket());
        }
    }
}
