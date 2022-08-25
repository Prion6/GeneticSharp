using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Mutations
{
    [TestFixture()]
    [Category("Mutations")]
    public class UniformMutationTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test()]
        public void Mutate_NoIndexes_RandomOneIndex()
        {
            var target = new UniformMutation();
            var chromosome = Substitute.For<ChromosomeBase<int>>(3);
            chromosome.ReplaceGenes(0, new int[]{1,1,1});

            chromosome.GenerateGene(1).Returns(0);
            RandomizationProvider.Current = Substitute.For<IRandomization>();
            RandomizationProvider.Current.GetInts(1, 0, 3).Returns(new int[] { 1 });

            target.Mutate(chromosome, 1);
            Assert.AreEqual(1, chromosome.GetGene(0));
            Assert.AreEqual(0, chromosome.GetGene(1));
            Assert.AreEqual(1, chromosome.GetGene(2));
        }

        [Test()]
        public void Mutate_InvalidIndexes_Exception()
        {
            var target = new UniformMutation(0, 3);
            var chromosome = Substitute.For<ChromosomeBase<int>>(3);
            chromosome.ReplaceGenes(0, new int[]{1,1,1});

            chromosome.GenerateGene(0).Returns(0);

            RandomizationProvider.Current = Substitute.For<IRandomization>();

            Assert.Catch<MutationException>(() =>
            {
                target.Mutate(chromosome, 1);
            }, "The chromosome has no gene on index 3. The chromosome genes length is 3.");
        }

        [Test()]
        public void Mutate_Indexes_RandomIndexes()
        {
            var target = new UniformMutation(0, 2);
            var chromosome = Substitute.For<ChromosomeBase<int>>(3);
            chromosome.ReplaceGenes(0, new int[]{1,1,1});

            chromosome.GenerateGene(0).Returns(0);
            chromosome.GenerateGene(2).Returns(10);
            RandomizationProvider.Current = Substitute.For<IRandomization>();

            target.Mutate(chromosome, 1);
            Assert.AreEqual(0, chromosome.GetGene(0));
            Assert.AreEqual(1, chromosome.GetGene(1));
            Assert.AreEqual(10, chromosome.GetGene(2));

        }

        [Test()]
        public void Mutate_AllGenesMutablesTrue_AllGenesMutaed()
        {
            var target = new UniformMutation(true);
            var chromosome = Substitute.For<ChromosomeBase<int>>(3);
            chromosome.ReplaceGenes(0, new int[]{1,1,1});

            chromosome.GenerateGene(0).Returns(0);
            chromosome.GenerateGene(1).Returns(10);
            chromosome.GenerateGene(2).Returns(20);
            RandomizationProvider.Current = Substitute.For<IRandomization>();
         
            target.Mutate(chromosome, 1);
            Assert.AreEqual(0, chromosome.GetGene(0));
            Assert.AreEqual(10, chromosome.GetGene(1));
            Assert.AreEqual(20, chromosome.GetGene(2));
        }
    }
}

