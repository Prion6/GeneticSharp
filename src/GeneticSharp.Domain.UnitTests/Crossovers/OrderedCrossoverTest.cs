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
    public class OrderedCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Cross_ParentWithNoOrderedGenes_Exception()
        {
            var target = new OrderedCrossover();

            var chromosome1 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome1.ReplaceGenes(0, new int[] {8,4,7,3,6,2,5,1,9,0});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome2.ReplaceGenes(0, new int[]{0,1,2,3,4,5,6,7,8,9});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            Assert.Catch<CrossoverException>(() =>
            {
                target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });
            }, "The Ordered Crossover (OX1) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
        }

        [Test]
        public void Cross_ParentsWith10Genes_Cross()
        {
            var target = new OrderedCrossover();

            // 8 4 7 3 6 2 5 1 9 0
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome1.ReplaceGenes(0, new int[] {8,4,7,3,6,2,5,1,9,0});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            // 0 1 2 3 4 5 6 7 8 9
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(10);
            chromosome2.ReplaceGenes(0, new int[]{0,1,2,3,4,5,6,7,8,9});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(10));

            // Child one: 0 4 7 3 6 2 5 1 8 9 
            // Child two: 8 2 1 3 4 5 6 7 9 0
            var rnd = Substitute.For<IRandomization>();
            rnd.GetUniqueInts(2, 0, 10).Returns(new int[] { 7, 3 });
            RandomizationProvider.Current = rnd;

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(10, actual[0].Length);
            Assert.AreEqual(10, actual[1].Length);

            Assert.AreEqual(10, actual[0].GetGenes().Distinct().Count());
            Assert.AreEqual(10, actual[1].GetGenes().Distinct().Count());

            Assert.AreEqual(0, actual[0].GetGene(0));
            Assert.AreEqual(4, actual[0].GetGene(1));
            Assert.AreEqual(7, actual[0].GetGene(2));
            Assert.AreEqual(3, actual[0].GetGene(3));
            Assert.AreEqual(2, actual[0].GetGene(5));
            Assert.AreEqual(5, actual[0].GetGene(6));
            Assert.AreEqual(1, actual[0].GetGene(7));
            Assert.AreEqual(8, actual[0].GetGene(8));
            Assert.AreEqual(9, actual[0].GetGene(9));


            Assert.AreEqual(8, actual[1].GetGene(0));
            Assert.AreEqual(2, actual[1].GetGene(1));
            Assert.AreEqual(1, actual[1].GetGene(2));
            Assert.AreEqual(3, actual[1].GetGene(3));
            Assert.AreEqual(4, actual[1].GetGene(4));
            Assert.AreEqual(5, actual[1].GetGene(5));
            Assert.AreEqual(6, actual[1].GetGene(6));
            Assert.AreEqual(7, actual[1].GetGene(7));
            Assert.AreEqual(9, actual[1].GetGene(8));
            Assert.AreEqual(0, actual[1].GetGene(9));
        }
    }
}