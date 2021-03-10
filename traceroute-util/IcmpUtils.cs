namespace traceroute_util
{
    public static class IcmpUtils
    { 
        public static byte[] GetEchoIcmpPackage()
        {
            byte[] package = new byte[64]; 
            package[0] = 8; // Type
            package[1] = 0; // Code

            package[2] = 0xF7; // Checksum
            package[3] = 0xFF;
            return package;
        }

        public static int GetIcmpType(byte[] data)
        {
            return data[20];
        }
    }
}
