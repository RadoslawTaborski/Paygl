using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PayglService.Helpers.Serializers
{
    public static class BinarySerializer<T>
    {
        public static void Serialize(string path, T obj)
        {
            if (obj == null) return;
            using (var fs = new FileStream(path, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
        }

        public static T Deserialize(string path)
        {
            var temp = default(T);

            if (!File.Exists(path)) return temp;

            using (var fs = new FileStream(path, FileMode.Open))
            {
                if (fs.Length <= 0) return temp;

                var bf = new BinaryFormatter();
                return (T)bf.Deserialize(fs);
            }
        }
    }
}
