namespace Tracker.GT06N.Shared
{
    public class IOUtility
    {


        public static string ConvertByteArrayToString(byte[] bytes)
        {
            var sb = new System.Text.StringBuilder("{ ");
            foreach (var b in bytes)
            {
                sb.Append(string.Format(string.Format("{0:x2} ", b)));
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        public static ushort FlipEndian(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }
    }
}
