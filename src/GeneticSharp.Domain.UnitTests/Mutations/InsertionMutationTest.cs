using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Infrastructure.Framework.Texts;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Mutations
{
    [TestFixture()]
    [Category("Mutations")]
    class InsertionMutationTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test()]
        public void Mutate_LessThanThreeGenes_Exception()
        {
            var target = new InsertionMutation();
            var chromosome = Substitute.For<ChromosomeBase<int>>(2);
            chromosome.ReplaceGenes(0, new int[]{1});

            Assert.Catch<MutationException>(() =>
            {
                target.Mutate(chromosome, 0);
            }, "A chromosome should have, at least, 3 genes. {0} has only 2 gene.".With(chromosome.GetType().Name));
        }

        [Test()]
        public void Mutate_NoProbality_NoInsertion()
        {
            var target = new InsertionMutation();
            var chromosome = Substitute.For<ChromosomeBase<int>>(4);
            chromosome.ReplaceGenes(0, new int[]{1,2,3,4});

            var rnd = Substitute.For<IRandomization>();
            rnd.GetDouble().Returns(0.1);
            RandomizationProvider.Current = rnd;

            target.Mutate(chromosome, 0);

            Assert.AreEqual(4, chromosome.Length);
            Assert.AreEqual(1, chromosome.GetGene(0));
            Assert.AreEqual(2, chromosome.GetGene(1));
            Assert.AreEqual(3, chromosome.GetGene(2));
            Assert.AreEqual(4, chromosome.GetGene(3));
        }

        [Test()]
        public void Mutate_ValidChromosome_Insertion_To_Left()
        {
            var target = new InsertionMutation();
            var chromosome = Substitute.For<ChromosomeBase<int>>(8);
            chromosome.ReplaceGenes(0, new int[]{1,2,3,4,5,6,7,8});

            var rnd = Substitute.For<IRandomization>();
            rnd.GetUniqueInts(2, 0, 8).Returns(new int[] { 1, 6 });
            rnd.GetDouble().Returns(0);
            RandomizationProvider.Current = rnd;
            target.Mutate(chromosome, 1);

            Assert.AreEqual(8, chromosome.Length);
            Assert.AreEqual(1, chromosome.GetGene(0));
            Assert.AreEqual(3, chromosome.GetGene(1));
            Assert.AreEqual(4, chromosome.GetGene(2));
            Assert.AreEqual(5, chromosome.GetGene(3));
            Assert.AreEqual(6, chromosome.GetGene(4));
            Assert.AreEqual(7, chromosome.GetGene(5));
            Assert.AreEqual(2, chromosome.GetGene(6));
            Assert.AreEqual(8, chromosome.GetGene(7));
        }

        [Test()]
        public void Mutate_ValidChromosome_Insertion_To_Right()
        {
            var target = new InsertionMutation();
            var chromosome = Substitute.For<ChromosomeBase<int>>(8);
            chromosome.ReplaceGenes(0, new int[]{1,2,3,4,5,6,7,8});

            var rnd = Substitute.For<IRandomization>();
            rnd.GetUniqueInts(2, 0, 8).Returns(new int[] { 1, 6 });
            rnd.GetDouble().Returns(1);
            RandomizationProvider.Current = rnd;
            target.Mutate(chromosome, 1);

            Assert.AreEqual(8, chromosome.Length);
            Assert.AreEqual(1, chromosome.GetGene(0));
            Assert.AreEqual(7, chromosome.GetGene(1));
            Assert.AreEqual(2, chromosome.GetGene(2));
            Assert.AreEqual(3, chromosome.GetGene(3));
            Assert.AreEqual(4, chromosome.GetGene(4));
            Assert.AreEqual(5, chromosome.GetGene(5));
            Assert.AreEqual(6, chromosome.GetGene(6));
            Assert.AreEqual(8, chromosome.GetGene(7));
        }
    }
}
