using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class ThreeParentCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Cross_ThreeParents_OneChildren()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(4);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3,4});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(4));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(4);
            chromosome2.ReplaceGenes(0, new int[]{1,5,6,4});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(4));

            var chromosome3 = Substitute.For<ChromosomeBase<int>>(4);
            chromosome3.ReplaceGenes(0, new int[]{ 10, 11, 12, 13});
            chromosome3.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(4));

            var parents = new List<IChromosome>() { chromosome1, chromosome2, chromosome3 };

            var target = new ThreeParentCrossover();

            var actual = target.Cross(parents);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(4, actual[0].Length);

            Assert.AreEqual(1, actual[0].GetGene(0));
            Assert.AreEqual(11, actual[0].GetGene(1));
            Assert.AreEqual(12, actual[0].GetGene(2));
            Assert.AreEqual(4, actual[0].GetGene(3));
        }
    }
}