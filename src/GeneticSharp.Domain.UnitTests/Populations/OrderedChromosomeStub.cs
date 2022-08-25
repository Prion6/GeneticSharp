using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace GeneticSharp.Domain.UnitTests
{
    public class OrderedChromosomeStub : ChromosomeBase<int>
    {
        public OrderedChromosomeStub()
            : base(6)
        {
            var values = RandomizationProvider.Current.GetUniqueInts(6, 0, 6);
            ReplaceGene(0, values[0]);
            ReplaceGene(1, values[1]);
            ReplaceGene(2, values[2]);
            ReplaceGene(3, values[3]);
            ReplaceGene(4, values[4]);
            ReplaceGene(5, values[5]);
        }

        public override object GenerateGene(int geneIndex)
        {
            return RandomizationProvider.Current.GetInt(0, 6);
        }

        public override IChromosome CreateNew()
        {
            return new OrderedChromosomeStub();
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as OrderedChromosomeStub;
            return clone;
        }
    }
}
