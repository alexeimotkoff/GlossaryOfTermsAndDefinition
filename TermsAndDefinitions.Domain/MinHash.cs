using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsAndDefinitions.Domain
{
    class MinHash
    {
        Random rnd = new Random(1337);
        private Encoding windowsEnc = Encoding.GetEncoding("windows-1251");       
        private uint[] hashFunc_a;
        private uint[] hashFunc_b;
        private int ROWSINBAND = 10;
        int m_numBands;
        private int numHashFunctions = 200;

        public MinHash()
        {
            m_numBands = numHashFunctions / ROWSINBAND;
            GenerateHashFunctionsArgs();          
        }        

        public int NumHashFunctions
        {
            get { return numHashFunctions; }
        }

        private void GenerateHashFunctionsArgs()
        {
            hashFunc_a = new uint[numHashFunctions];
            hashFunc_b = new uint[numHashFunctions];
            // will get the same hash functions each time since the same random number seed is used
           
            for (int i = 0; i < numHashFunctions; i++)
            {
                uint a = 0;
                // parameter a is an odd positive
                while (a % 2 == 0 || a <= 0)
                    a = (uint)rnd.Next();
                int maxb = (int)Math.Pow(2, UniverseBits);
                uint b = (uint)rnd.Next(0, maxb);
                hashFunc_a[i] = a;
                hashFunc_b[i] = b;
            }
        }

        public  int UniverseBits { get; set; } = 30;

        private uint QHash(uint x, uint a, uint b)
        {
            return (a * x + b) >> (32 - UniverseBits);
        }

        public int[] GetSignature(string text, int shinglesCount)
        {
            var textWords = Canonize(text);

            var BaseSignature = textWords.Select(word => StringToUInt32(word));

            uint[] minHashes = new uint[numHashFunctions];
            for (int h = 0; h < numHashFunctions; h++)
            {
                minHashes[h] = int.MaxValue;
            }

            foreach (uint value in BaseSignature)
            {
                for (int h = 0; h < numHashFunctions; h++)
                {
                    uint hash = QHash(value, hashFunc_a[h], hashFunc_b[h]);
                    minHashes[h] = Math.Min(minHashes[h], hash);
                }
            }
            return minHashes.Select(x => unchecked((int)x)).ToArray();
        }

        public int[] GetBuckets(int[] signature)
        {
            int[] buckets = new int[m_numBands];
            for (int i = 0; i < m_numBands; i++)
            {                //combine all 5 MH values and then hash get its hashcode
                int sum = 0;

                for (int j = 0; j < ROWSINBAND; j++)
                {
                    sum += signature[i * ROWSINBAND + j];
                }
                buckets[i] = sum;
            }
            return buckets;
        }

        public double Similarity(int[] firstSignature, int[] secondSignature)
        {
            int equal_count = 0;
            for (int i = 0; i < numHashFunctions; i++)
            {
                if (secondSignature[i] == firstSignature[i])
                    equal_count++;
            }

            return equal_count / (float)numHashFunctions;
        }
        
        private uint StringToUInt32(string word)
        {
            byte[] buffer = windowsEnc.GetBytes(word);
            return ConvertLittleEndian(buffer);
        }

        private uint ConvertLittleEndian(byte[] array)
        {
            int pos = 0;
            uint result = 0;
            foreach (byte by in array)
            {
                result |= ((uint)by) << pos;
                pos += 8;
            }
            return result;
        }

        private IEnumerable<string> Canonize(string text)
        {
            return Canonizator.GetCanonizedTextWords(text);
        }
        
    }
}

