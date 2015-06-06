using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detrav.TeraModLoader.Core.P2904;
namespace TMLTests
{
    [TestClass]
    public class CacheManagetP2904Test
    {
        [TestMethod]
        public void TestДляПравильнойЗагрузкиБазыНИПов()
        {
            CacheManager cacheManager = new CacheManager();
            Assert.IsNull(cacheManager.getNpc(0, 1),"Такого нас не должно быть");
            Assert.IsTrue(11510 == cacheManager.teraNpcs.Count,"Количество как на сайте");
            bool test = false;
            foreach (var pair in cacheManager.teraNpcs)
            {
                if (pair.Value.type == Detrav.TeraApi.Enums.NpcType.Elite)
                {
                    test = true;
                    break;
                }
            }
            Assert.IsTrue(test, "Есть Elite");
            test = false;
            foreach (var pair in cacheManager.teraNpcs)
            {
                if (pair.Value.type == Detrav.TeraApi.Enums.NpcType.Normal)
                {
                    test = true;
                    break;
                }
            }
            Assert.IsTrue(test, "Есть Normal");
        }
    }
}