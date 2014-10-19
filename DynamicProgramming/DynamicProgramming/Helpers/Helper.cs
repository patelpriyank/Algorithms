using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.Helpers
{
    public static class Helper
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static string Print(int[] array)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
                builder.Append(array[i] + ", ");

            return builder.ToString();
        }
    }
}
