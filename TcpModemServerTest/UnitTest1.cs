using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TcpModemServer;

namespace TcpModemServerTest
{
    [TestClass]
    public class SessionIdPoolTest
    {
        private SessionIdPool _testee;

        [TestInitialize]
        public void Setup()
        {
            _testee = new SessionIdPool();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void ShouldAcquieSessionId()
        {
            var expected = 1;

            var actual = _testee.Acquire();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldAcquieNextSessionIdSequentially()
        {
            var expected = 2;

            _testee.Acquire();
            var actual = _testee.Acquire();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldWrapupWhenTheMaxSessionIdIsReqched()
        {
            var expected = 1;

            for (int i = 0; i < 32; i++)
            {
                var id =_testee.Acquire();
                _testee.Release(id);
            }

            var actual = _testee.Acquire();

            Assert.AreEqual(expected, actual);
        }
    }
}
