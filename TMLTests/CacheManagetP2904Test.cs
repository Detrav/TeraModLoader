using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detrav.TeraModLoader.Core.P2904;
namespace TMLTests
{
    [TestClass]
    public class CacheManagetP2904Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            CacheManager cacheManager = new CacheManager();
            Assert.IsNull(cacheManager.getNpc(0, 1),"Такого нас не должно быть");
            Assert.IsTrue(11510 == cacheManager.teraNpcs.Count,"Количество как на сайте");
        }
    }
}