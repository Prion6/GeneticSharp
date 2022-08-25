using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture()]
    [Category("Crossovers")]
    public class PartiallyMappedCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Cross_ParentWithNoOrderedGenes_Exception()
        {
            var target = new PartiallyMappedCrossover();

            var chromosome1 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome1.ReplaceGenes(0, new int[] {1,2,3,4,5,6,7,8});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome2.ReplaceGenes(0, new int[]{3,7,5,1,1,8,2,3});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            Assert.Catch <CrossoverException>(() =>
            {
                target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });
            }, "The Partially Mapped Crossover (PMX) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
        }

        [Test]
        public void Cross_ParentsWith8Genes_Cross()
        {
            var target = new PartiallyMappedCrossover();

            // 1 2 3 4 5 6 7 8 
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome1.ReplaceGenes(0, new int[] {1,2,3,4,5,6,7,8});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            // 3 7 5 1 6 8 2 4
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome2.ReplaceGenes(0, new int[]{3,7,5,1,6,8,2,4});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            var rnd = Substitute.For<IRandomization>();
            rnd.GetUniqueInts(2, 0, 8).Returns(new int[] { 5, 3 });
            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(8, actual[0].Length);
            Assert.AreEqual(8, actual[1].Length);

            //Assert.AreEqual(8, actual[0].GetGenes().Distinct().Count());
            //Assert.AreEqual(8, actual[1].GetGenes().Distinct().Count());

            // offspring 1: (4 2 3 1 6 8 7 5)
            Assert.AreEqual(4, actual[0].GetGene(0));
            Assert.AreEqual(2, actual[0].GetGene(1));
            Assert.AreEqual(3, actual[0].GetGene(2));
            Assert.AreEqual(1, actual[0].GetGene(3));
            Assert.AreEqual(6, actual[0].GetGene(4));
            Assert.AreEqual(8, actual[0].GetGene(5));
            Assert.AreEqual(7, actual[0].GetGene(6));
            Assert.AreEqual(5, actual[0].GetGene(7));

            //            // offspring 2: (3 7 8 4 5 6 2 1)
            Assert.AreEqual(3, actual[1].GetGene(0));
            Assert.AreEqual(7, actual[1].GetGene(1));
            Assert.AreEqual(8, actual[1].GetGene(2));
            Assert.AreEqual(4, actual[1].GetGene(3));
            Assert.AreEqual(5, actual[1].GetGene(4));
            Assert.AreEqual(6, actual[1].GetGene(5));
            Assert.AreEqual(2, actual[1].GetGene(6));
            Assert.AreEqual(1, actual[1].GetGene(7));
        }
    }
}

