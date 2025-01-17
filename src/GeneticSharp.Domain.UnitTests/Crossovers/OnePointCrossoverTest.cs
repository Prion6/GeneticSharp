﻿using System;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using NUnit.Framework;
using NSubstitute;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class OnePointCrossoverTest
    {
        [Test]
        public void Cross_LessGenesThenSwapPoint_Exception()
        {
            var target = new OnePointCrossover(1);
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(2);
            var chromosome2 = Substitute.For<ChromosomeBase<int>>(2);

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.Cross(new List<IChromosome>() {
                    chromosome1,
                    chromosome2
                });
            }, "The swap point index is 1, but there is only 2 genes. The swap should result at least one gene to each side.");
        }

        [Test]
        public void Cross_ParentsWithTwoGenes_Cross()
        {
            var target = new OnePointCrossover(0);
            var chromosome1 = Substitute.For<ChromosomeBase<int>>(2);
            chromosome1.ReplaceGenes(0, new int[]{1,2});
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(2));

            var chromosome2 = Substitute.For<ChromosomeBase<int>>(2);
            chromosome2.ReplaceGenes(0, new int[]{3,4});
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase<int>>(2));

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual[0].Length);
            Assert.AreEqual(2, actual[1].Length);

            Assert.AreEqual(1, actual[0].GetGene(0));
            Assert.AreEqual(4, actual[0].GetGene(1));

            Assert.AreEqual(3, actual[1].GetGene(0));
            Assert.AreEqual(2, actual[1].GetGene(1));
        }
    }
}