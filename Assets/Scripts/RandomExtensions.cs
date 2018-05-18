using System;

namespace Application
{
	static class RandomExtensions
    {
        public static T[] Shuffle<T>(this Random rng, T[] array)
        {
			T[] result = array.Clone() as T[];
            int n = result.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = result[n];
                result[n] = result[k];
                result[k] = temp;
            }
			return result;
        }
    }
}
