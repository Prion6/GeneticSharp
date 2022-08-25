using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticSharp.Domain.UnitTests.Crossovers.Issues
{
    public class Issue61
    {
        public class GuessNumberChromosome : ChromosomeBase<int>
        {

            private const Int32 MaxGeneValue = 10;
            private const Int32 MinGeneValue = 0;

            public static int[] powof10 = new int[10] {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000
        };

            private readonly Int32 numberDigits;

            public GuessNumberChromosome(Int32 numberDigits) : base(numberDigits)
            {
                this.numberDigits = numberDigits;
                var initGenes = RandomizationProvider.Current.GetUniqueInts(this.Length, MinGeneValue, MaxGeneValue);
                for (int i = 0; i < this.Length; i++)
                {
                    this.ReplaceGene(i, initGenes[i]);
                }
            }

            public override IChromosome CreateNew()
            {
                return new GuessNumberChromosome(this.numberDigits);
            }

            public override object GenerateGene(int geneIndex)
            {
                return RandomizationProvider.Current.GetInt(MinGeneValue, MaxGeneValue);
            }

            public Int32 ToGuessValue()
            {
                var genes = this.GetGenes<int>();
                return ToGuessValue(genes);
            }

            public static Int32 ToGuessValue(int[] genes)
            {
                int guessValue = 0;
                for (int i = genes.Length - 1; i >= 0; i--)
                {
                    guessValue += (genes[i] * powof10[i]);
                }
                return guessValue;
            }
        }

        public class GuessNumberFitness : IFitness
        {
            private Int32 finalAns;
            private Int32 maxDiffValue;

            public GuessNumberFitness(Int32 finalAns)
            {
                this.finalAns = finalAns;
                Int32 digits = this.finalAns.ToString().Length;
                this.maxDiffValue = Math.Max(Math.Abs(this.finalAns - Int32.Parse(new string('9', digits))), this.finalAns);
            }

            public double Evaluate(IChromosome chromosome)
            {
                var genes = chromosome.GetGenes<int>();
                int guessValue = GuessNumberChromosome.ToGuessValue(genes);
                double fitness = 1.0 - (Math.Abs(this.finalAns - guessValue) / (double)this.maxDiffValue);
                return fitness;
            }
        }
    }
}
