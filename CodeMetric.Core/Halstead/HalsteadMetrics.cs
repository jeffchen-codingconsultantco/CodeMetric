﻿using System;

namespace CodeMetric.Core.Halstead
{
    internal sealed class HalsteadMetrics : IHalsteadMetrics
    {
        public static readonly IHalsteadMetrics GenericInstanceGetPropertyMetrics;
        public static readonly IHalsteadMetrics GenericInstanceSetPropertyMetrics;
        public static readonly IHalsteadMetrics GenericStaticGetPropertyMetrics;
        public static readonly IHalsteadMetrics GenericStaticSetPropertyMetrics;

        static HalsteadMetrics()
        {
            GenericInstanceSetPropertyMetrics = new HalsteadMetrics(5, 3, 4, 3);
            GenericStaticSetPropertyMetrics = new HalsteadMetrics(4, 3, 3, 3);
            GenericInstanceGetPropertyMetrics = new HalsteadMetrics(3, 2, 3, 2);
            GenericStaticGetPropertyMetrics = new HalsteadMetrics(2, 1, 2, 1);
        }

        public HalsteadMetrics(int numOperands, int numOperators, int numUniqueOperands, int numUniqueOperators)
        {
            NumberOfOperands = numOperands;
            NumberOfOperators = numOperators;
            NumberOfUniqueOperands = numUniqueOperands;
            NumberOfUniqueOperators = numUniqueOperators;
        }

        public int NumberOfOperands { get; private set; }

        public int NumberOfOperators { get; private set; }

        public int NumberOfUniqueOperands { get; private set; }

        public int NumberOfUniqueOperators { get; private set; }

        public IHalsteadMetrics Merge(IHalsteadMetrics other)
        {
            if(other == null)
            {
                return this;
            }

            return new HalsteadMetrics(
                NumberOfOperands + other.NumberOfOperands,
                NumberOfOperators + other.NumberOfOperators,
                NumberOfUniqueOperands + other.NumberOfUniqueOperands,
                NumberOfUniqueOperators + other.NumberOfUniqueOperators);
        }

        public int GetBugs()
        {
            var volume = GetVolume();

            return (int)(volume / 3000);
        }

        public double GetDifficulty()
        {
            return NumberOfUniqueOperands == 0
                ? 0
                : ((NumberOfUniqueOperators / 2.0) * (NumberOfOperands / ((double)NumberOfUniqueOperands)));
        }

        public TimeSpan GetEffort()
        {
            var effort = GetDifficulty() * GetVolume();
            return TimeSpan.FromSeconds(effort / 18.0);
        }

        public int GetLength()
        {
            return NumberOfOperators + NumberOfOperands;
        }

        public int GetVocabulary()
        {
            return NumberOfUniqueOperators + NumberOfUniqueOperands;
        }

        public double GetVolume()
        {
            const double NewBase = 2.0;
            double vocabulary = GetVocabulary();
            double length = GetLength();
            if(vocabulary.Equals(0.0))
            {
                return 0.0;
            }

            return length * Math.Log(vocabulary, NewBase);
        }
    }
}
