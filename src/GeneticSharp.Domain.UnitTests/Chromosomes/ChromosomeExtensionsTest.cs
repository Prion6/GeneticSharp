using System;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Infrastructure.Framework.Texts;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Chromosomes
{
    [TestFixture()]
    [Category("Chromosomes")]
    public class ChromosomeExtensionsTest
    {
        [Test()]
        public void AnyChromosomeHasRepeatedGene_NonRepeatedGene_False()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3});

            var chromosomes = new List<IChromosome>() { chromosome1 };
            Assert.IsFalse(chromosomes.AnyHasRepeatedGene());

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome2.ReplaceGenes(0, new int[]{1,2,3});

            chromosomes.Add(chromosome2);
            Assert.IsFalse(chromosomes.AnyHasRepeatedGene());
        }

        [Test()]
        public void AnyChromosomeHasRepeatedGene_RepeatedGene_True()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3});

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes(0, new int[]{1,2,3});

            var chromosomes = new List<IChromosome>() { chromosome1, chromosome2 };

            Assert.IsTrue(chromosomes.AnyHasRepeatedGene());
        }

        [Test]
        public void ValidateGenes_GenesWithNullValue_Exception()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);

            Assert.Catch<InvalidOperationException> (
                () => {
                    chromosome1.ValidateGenes();
            }, "The chromosome '{0}' is generating genes with null value.".With(chromosome1.GetType().Name));
        }

        [Test]
        public void ValidateGenes_AllGenesWithValue_NoException()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes (0, new int[] { 1,2,3 });

            chromosome1.ValidateGenes();
        }

        [Test]
        public void ValidateGenes_ChromosomesWithGenesWithNullValue_Exception()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes (0, new int[] { 1,2,3 });
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(3);

            Assert.Catch<InvalidOperationException>(
                () => {
                    (new List<IChromosome>() { chromosome1, chromosome2 }).ValidateGenes();
            }, "The chromosome '{0}' is generating genes with null value.".With(chromosome2.GetType().Name));
        }

        [Test]
        public void ValidateGenes_ChromosomesWithAllGenesWithValue_NoException()
        {
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome1.ReplaceGenes (0, new int[] { 1,2,3 });
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(3);
            chromosome2.ReplaceGenes (0, new int[] {1,2,3 });

            (new List<IChromosome>() { chromosome1, chromosome2 }).ValidateGenes();
        }
    }
}