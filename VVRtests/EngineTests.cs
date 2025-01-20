using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVR.Technical;
using VVR.Vehicles.VehicleComponents;

namespace VVR.Tests
{
    [TestClass]
    public class EngineTests
    {
        [DataTestMethod]
        [DataRow(4, 2.0f, Configuration.Inline, EngineType.NaturallyAspirated, (2.0f / 4 + 4) * GlobalConsts.ENGINESIZINGCONST)]
        [DataRow(6, 3.0f, Configuration.Flat, EngineType.NaturallyAspirated, (3.0f / 6 + 6) * GlobalConsts.ENGINESIZINGCONST)]
        [DataRow(8, 4.0f, Configuration.V, EngineType.Supercharged, (4.0f / 8 + 8) * GlobalConsts.VENGINESIZEBIAS * GlobalConsts.ENGINESIZINGCONST + GlobalConsts.FORCEDINDUCTIONSIZE)]
        public void TestCalculateSize(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedSize)
        {
            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualSize = engine.size;
            Assert.AreEqual(expectedSize, actualSize, 0.01f);
        }

        [DataTestMethod]
        [DataRow(4, 2.0f, Configuration.Inline, EngineType.NaturallyAspirated, (4 / 2.0f + 2.0f) * GlobalConsts.ENGINEWEIGHTCONST)]
        [DataRow(8, 4.0f, Configuration.V, EngineType.Supercharged, (8 / 4.0f + 4.0f) * GlobalConsts.VENGINEMASSBIAS * GlobalConsts.ENGINEWEIGHTCONST + GlobalConsts.FORCEDINDUCTIONMASSSC)]
        [DataRow(6, 3.0f, Configuration.Flat, EngineType.Turbocharged, (6 / 3.0f + 3.0f) * GlobalConsts.ENGINEWEIGHTCONST + GlobalConsts.FORCEDINDUCTIONMASSTC)]
        public void TestCalculateWeight(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedWeight)
        {
            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualWeight = engine.engineWeight;
            Assert.AreEqual(expectedWeight, actualWeight, 0.01f);
        }

        [DataTestMethod]
        [DataRow(4, 2.0f, Configuration.Inline, EngineType.Turbocharged, (4 * 2.0f) * GlobalConsts.HORSEPOWERMULTIPLIER + GlobalConsts.TURBOBONUS)]
        [DataRow(6, 3.0f, Configuration.Flat, EngineType.Supercharged, (6 * 3.0f) * GlobalConsts.HORSEPOWERMULTIPLIER + GlobalConsts.SUPERCHARGERBONUS)]
        public void TestCalculateHorsePower(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedHorsePower)
        {

            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualHorsePower = engine.horsePower;
            Assert.AreEqual(expectedHorsePower, actualHorsePower, 0.01f);
        }

        [DataTestMethod]
        [DataRow(4, 2.5f, Configuration.Inline, EngineType.NaturallyAspirated, 2.5f * GlobalConsts.TORQMULTIPLIER)]
        [DataRow(6, 3.5f, Configuration.Flat, EngineType.Turbocharged, 3.5f * GlobalConsts.TORQMULTIPLIER + GlobalConsts.TURBOBONUSTORQ)]
        public void TestCalculateTorque(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedTorque)
        {
            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualTorque = engine.torque;
            Assert.AreEqual(expectedTorque, actualTorque, 0.01f);
        }

        [DataTestMethod]
        [DataRow(12, 5.0f, Configuration.V, EngineType.NaturallyAspirated, 100.0f - 12)]
        [DataRow(6, 3.0f, Configuration.Flat, EngineType.Supercharged, 100.0f - GlobalConsts.FLATRELIABILITYDEBUF - GlobalConsts.FORCEDINDUCTIONDEBUF - 6)]
        public void TestCalculateReliability(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedReliability)
        {
            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualReliability = engine.reliability;
            Assert.AreEqual(expectedReliability, actualReliability, 0.01f);
        }

        [DataTestMethod]
        [DataRow(6, 3.5f, Configuration.Flat, EngineType.Supercharged, 15.0f)]
        [DataRow(4, 2.0f, Configuration.Inline, EngineType.Turbocharged, 9.0f)]
        public void TestCalculateFuelConsumption(int cylinderAmmount, float displacement, Configuration config, EngineType type, float expectedFuelConsumption)
        {
            var engine = new Engine(cylinderAmmount, displacement, config, type);
            var actualFuelConsumption = engine.fuelconsumption;
            Assert.AreEqual(expectedFuelConsumption, actualFuelConsumption, 0.01f);
        }
    }
}
