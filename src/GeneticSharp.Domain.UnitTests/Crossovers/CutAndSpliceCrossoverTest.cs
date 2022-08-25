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
    public class CutAndSpliceCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Cross_ParentsWithSameLength_Cross()
        {
            var target = new CutAndSpliceCrossover();
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(4);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3,4,});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(4));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(4);
            chromosome2.ReplaceGenes(0, new int[]{1,2,3,4});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(4));

            var rnd = Substitute.For<IRandomization>();
            rnd.GetInt(1, 4).Returns(1, 3);

            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual[0].Length);
            Assert.AreEqual(6, actual[1].Length);

            Assert.AreEqual(1, actual[0].GetGene(0));
            Assert.AreEqual(2, actual[0].GetGene(1));

            Assert.AreEqual(5, actual[1].GetGene(0));
            Assert.AreEqual(6, actual[1].GetGene(1));
            Assert.AreEqual(7, actual[1].GetGene(2));
            Assert.AreEqual(8, actual[1].GetGene(3));
            Assert.AreEqual(3, actual[1].GetGene(4));
            Assert.AreEqual(4, actual[1].GetGene(5));
        }

        [Test]
        public void Cross_ParentsWithDiffLength_Cross()
        {
            var target = new CutAndSpliceCrossover();
            var chromosome1 = Substitute.ForPartsOf<ChromosomeBase<int>>(4);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3,4});
            chromosome1.CreateNew().Returns(Substitute.ForPartsOf<ChromosomeBase<int>>(4));

            var chromosome2 = Substitute.ForPartsOf<ChromosomeBase<int>>(5);
            chromosome2.ReplaceGenes(0, new int[]{5,6,7,8,9});
            chromosome2.CreateNew().Returns(Substitute.ForPartsOf<ChromosomeBase<int>>(5));

            var rnd = Substitute.For<IRandomization>();
            rnd.GetInt(1, 4).Returns(2);
            rnd.GetInt(1, 5).Returns(2);

            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(5, actual[0].Length);
            Assert.AreEqual(4, actual[1].Length);

            Assert.AreEqual(1, actual[0].GetGene(0));
            Assert.AreEqual(2, actual[0].GetGene(1));
            Assert.AreEqual(3, actual[0].GetGene(2));
            Assert.AreEqual(8, actual[0].GetGene(3));
            Assert.AreEqual(9, actual[0].GetGene(4));

            Assert.AreEqual(5, actual[1].GetGene(0));
            Assert.AreEqual(6, actual[1].GetGene(1));
            Assert.AreEqual(7, actual[1].GetGene(2));
            Assert.AreEqual(4, actual[1].GetGene(3));
        }
    }
}