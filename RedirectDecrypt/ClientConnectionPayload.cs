using System;
using System.Net;
using System.Linq;
using System.Text;

namespace RedirectDecrypt
{
    public abstract class ClientConnectionPayload
    {
        public ClientConnectionPayload()
        {
            Address = new byte[16];
            AddressType = 1;
            Hmac = new byte[20];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (AddressType != 3)
            {
                var addressBytes = Address;
                if (AddressType != 2)
                    addressBytes = (new ArraySegment<byte>(Address, 0, 4)).ToArray();

                IPAddress addr = new IPAddress(addressBytes);
                sb.AppendFormat("Redirected to: {0}:{1}", addr.ToString(), Port);
                sb.AppendLine();
                sb.AppendFormat("Xor magic: {0}", XorMagic);
                sb.AppendLine();
                sb.AppendFormat("Pi digits: {0}", Program.ByteArrayToHexString(PiDigits));
                sb.AppendLine();
                if (PanamaKey != null)
                {
                    sb.AppendFormat("Panama seed: {0}", Program.ByteArrayToHexString(PanamaKey));
                    sb.AppendLine();
                }
                if (Adler32 != 0)
                {
                    sb.AppendFormat("Adler32: {0}", Adler32.ToString("X8"));
                    sb.AppendLine();
                }
                sb.AppendFormat(Encoding.ASCII.GetString(Haiku));
                sb.AppendLine();
                sb.AppendLine("HMAC: " + Program.ByteArrayToHexString(Hmac));
            }

            return sb.ToString();
        }

        public byte[] Address;
        public uint AddressType;
        public byte XorMagic;
        public ushort Port;
        public byte[] Haiku;
        public byte[] Hmac;
        public byte[] PanamaKey;
        public byte[] PiDigits;
        public uint Adler32;
    }
}
