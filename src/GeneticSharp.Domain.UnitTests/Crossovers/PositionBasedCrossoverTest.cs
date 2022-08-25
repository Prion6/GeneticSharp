using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class PositionBasedCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Cross_ParentWithNoOrderedGenes_Exception()
        {
            var target = new PositionBasedCrossover();

            var chromosome1 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome1.ReplaceGenes(0, new int[] {8,4,7,3,6,2,5,1,9,0});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome2.ReplaceGenes(0, new int[]{0,1,2,3,4,5,6,7,8,9});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            Assert.Catch<CrossoverException>(() =>
            {
                target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });
            }, "The Position-based Crossover (POS) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
        }

        [Test]
        public void Cross_ParentsWith8Genes_Cross()
        {
            var target = new PositionBasedCrossover();

            // 1 2 3 4 5 6 7 8
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome1.ReplaceGenes(0, new int[] {1,2,3,4,5,6,7,8});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            // 2 4 6 8 7 5 3 1
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(8);
            chromosome2.ReplaceGenes(0, new int[]{2,4,6,8,7,5,3,1});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(8));

            // Child one: 1 4 6 2 3 5 7 8
            // Child two: 4 2 3 8 7 6 5 1
            var rnd = Substitute.For<IRandomization>();
            rnd.GetInt(1, 7).Returns(3);
            rnd.GetUniqueInts(3, 0, 8).Returns(new int[] { 1, 2, 5 });
            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            var childOne = actual [0];
            var childTwo = actual [1];
            Assert.AreEqual(8, childOne.Length);
            Assert.AreEqual(8, childTwo.Length);

            Assert.AreEqual(8, childOne.GetGenes().Distinct().Count());
            Assert.AreEqual(8, childTwo.GetGenes().Distinct().Count());

            Assert.AreEqual(1, childOne.GetGene(0));
            Assert.AreEqual(4, childOne.GetGene(1));
            Assert.AreEqual(6, childOne.GetGene(2));
            Assert.AreEqual(2, childOne.GetGene(3));
            Assert.AreEqual(3, childOne.GetGene(4));
            Assert.AreEqual(5, childOne.GetGene(5));
            Assert.AreEqual(7, childOne.GetGene(6));
            Assert.AreEqual(8, childOne.GetGene(7));

            Assert.AreEqual(4, childTwo.GetGene(0));
            Assert.AreEqual(2, childTwo.GetGene(1));
            Assert.AreEqual(3, childTwo.GetGene(2));
            Assert.AreEqual(8, childTwo.GetGene(3));
            Assert.AreEqual(7, childTwo.GetGene(4));
            Assert.AreEqual(6, childTwo.GetGene(5));
            Assert.AreEqual(5, childTwo.GetGene(6));
            Assert.AreEqual(1, childTwo.GetGene(7));
        }

        [Test]
        public void Cross_ParentsWith6Genes_Cross()
        {
            var target = new PositionBasedCrossover();

            // 1 5 4 0 3 2
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome1.ReplaceGenes(0, new int[] {1,5,4,0,3,2});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(6));

            // 2 3 5 0 1 4
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome2.ReplaceGenes(0, new int[]{2,3,5,0,1,4});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(6));

            // Child one: 4 3 5 0 1 2
            // Child two: 2 5 4 0 3 1
            var rnd = Substitute.For<IRandomization>();
            rnd.GetInt(1, 5).Returns(3);
            rnd.GetUniqueInts(3, 0, 6).Returns(new int[] { 2, 4, 3 });
            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            var childOne = actual[0];
            var childTwo = actual[1];
            Assert.AreEqual(6, childOne.Length);
            Assert.AreEqual(6, childTwo.Length);

            Assert.AreEqual(6, childOne.GetGenes().Distinct().Count());
            Assert.AreEqual(6, childTwo.GetGenes().Distinct().Count());

            Assert.AreEqual(4, childOne.GetGene(0));
            Assert.AreEqual(3, childOne.GetGene(1));
            Assert.AreEqual(5, childOne.GetGene(2));
            Assert.AreEqual(0, childOne.GetGene(3));
            Assert.AreEqual(1, childOne.GetGene(4));
            Assert.AreEqual(2, childOne.GetGene(5));

            Assert.AreEqual(2, childTwo.GetGene(0));
            Assert.AreEqual(5, childTwo.GetGene(1));
            Assert.AreEqual(4, childTwo.GetGene(2));
            Assert.AreEqual(0, childTwo.GetGene(3));
            Assert.AreEqual(3, childTwo.GetGene(4));
            Assert.AreEqual(1, childTwo.GetGene(5));
        }
    }
}