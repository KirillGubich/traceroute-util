namespace traceroute_util
{
    public class Icmp
    {
        private Icmp()
        {
        }

        public static byte[] GetEchoIcmpPackage()
        {
            byte[] package = new byte[64];
            package[0] = 8;
            package[1] = 0;
            package[2] = 0xF7;
            package[3] = 0xFF;
            return package;
        }

        public static int GetIcmpType(byte[] data)
        {
            return data[20];
        }
    }
}
