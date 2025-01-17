namespace AStarResolver.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Shuffle the list based on Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to shuffle</param>
    /// <returns>The provided array in input but shuffled.</returns>
    public static T[] Shuffle<T>(this T[] array)
    {
        for (var i = array.Length - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);

            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        return array;
    }
}
