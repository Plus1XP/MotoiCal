using Microsoft.VisualStudio.TestTools.UnitTesting;

using MotoiCal.Models;
using MotoiCal.Services;

namespace MotoiCal.UnitTests
{
    [TestClass]
    public class ScraperServiceTests
    {
        [TestMethod]
        public void GetSeriesCollection_ReturnCollection_Formula1()
        {
            Formula1 formula1 = new Formula1();
            ScraperService motorSport = new ScraperService();

            int expected = 21;

            int actual = motorSport.GetSeriesCollection(formula1).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSeriesCollection_MotoGPCollectionCount_Returns20()
        {
            MotoGP motoGP = new MotoGP();
            ScraperService motorSport = new ScraperService();

            int expected = 20;

            int actual = motorSport.GetSeriesCollection(motoGP).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSeriesCollection_WorldSBKShouldReturnCollection()
        {
            WorldSBK worldSBK = new WorldSBK();
            ScraperService motorSport = new ScraperService();

            int expected = 12;

            int actual = motorSport.GetSeriesCollection(worldSBK).Count;

            Assert.AreEqual(expected, actual);
        }
    }
}
