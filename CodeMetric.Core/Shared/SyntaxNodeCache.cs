using Microsoft.VisualStudio.CodeSense.Roslyn;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeMetric.Core.Shared
{
    public class SyntaxNodeCache
    {
        private static readonly ConcurrentDictionary<SyntaxNode, string> SyntaxDictionary = new ConcurrentDictionary<SyntaxNode, string>();

        public static void Add(SyntaxNode key, string value)
        {
            SyntaxDictionary.TryAdd(key, value);
        }

        public static void Remove(SyntaxNode key)
        {
            string value;
            SyntaxDictionary.TryRemove(key, out value);
        }

        public static void Change(SyntaxNode key, string newValue)
        {
            var existingValue = Get(key);
            if(string.IsNullOrEmpty(existingValue))
            {
                Add(key, newValue);
            }
            else
            {
                SyntaxDictionary.TryUpdate(key, newValue, existingValue);
            }
        }

        public static string Get(SyntaxNode key)
        {
            string value;
            SyntaxDictionary.TryGetValue(key, out value);
            return value;
        }
        
    }
}
