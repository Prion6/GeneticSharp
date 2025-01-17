﻿using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Extensions.Mathematic;
using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Mathematic
{
    [TestFixture()]
    [Category("Extensions")]
    public class EqualityFitnessTest
    {
        [Test()]
        public void Evaluate_DiffChromosomes_DiffFitness()
        {
            var target = new EqualityFitness();

            var chromosome = new EquationChromosome(30, 4);
            chromosome.ReplaceGene(0, 0);
            chromosome.ReplaceGene(1, 7);
            chromosome.ReplaceGene(2, -43);
            chromosome.ReplaceGene(3, 32);

            var actual = target.Evaluate(chromosome);
            Assert.Less(actual, 0);

            chromosome = new EquationChromosome(30, 4);
            chromosome.ReplaceGene(0, 17);
            chromosome.ReplaceGene(1, 7);
            chromosome.ReplaceGene(2, -43);
            chromosome.ReplaceGene(3, 32);

            actual = target.Evaluate(chromosome); 
            Assert.AreEqual(0, actual);
        }
    }
}