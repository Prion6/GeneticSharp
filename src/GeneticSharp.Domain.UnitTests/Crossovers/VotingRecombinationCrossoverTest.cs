using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework;
using NSubstitute;
using System;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class VotingRecombinationCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Constructor_ThresholdGreaterThanParentsNumber_Exception()
        {
            var actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                new VotingRecombinationCrossover(2, 3);
            });

            StringAssert.StartsWith("The threshold should be smaller or equal to the parents number.", actual.Message);
        }

        [Test]
        public void Cross_DocumentationSample_Child()
        {
            var target = new VotingRecombinationCrossover(4, 3);

            // 1 4 3 5 2 6
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome1.ReplaceGenes(0, new int[] {1,4,3,5,2,6});
            
            var child = Substitute.For<ChromosomeBase<int>>(6);
            child.GenerateGene(2).Returns(22);
            child.GenerateGene(3).Returns(33);
            child.GenerateGene(4).Returns(44);
            chromosome1.CreateNew().Returns(child);

            // 1 2 4 3 5 6
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome2.ReplaceGenes(0, new int[]{1,2,3,4,5,6});

            // 3 2 1 5 4 6
            var chromosome3 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome3.ReplaceGenes(0, new int[]{3,2,1,5,4,6});
      
            // 1 2 3 4 5 6
            var chromosome4 = Substitute.For<ChromosomeBase<int>>(6);
            chromosome4.ReplaceGenes(0, new int[]{1,2,3,4,5,6});
       
            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2, chromosome3, chromosome4 });

            Assert.AreEqual(1, actual.Count);
            var actualChild = actual[0];

            Assert.AreEqual(6, actualChild.Length);
         
            Assert.AreEqual(1, actualChild.GetGene(0));
            Assert.AreEqual(2, actualChild.GetGene(1));
            Assert.AreEqual(22, actualChild.GetGene(2));
            Assert.AreEqual(33, actualChild.GetGene(3));
            Assert.AreEqual(44, actualChild.GetGene(4));
            Assert.AreEqual(6, actualChild.GetGene(5));
        }
    }
}