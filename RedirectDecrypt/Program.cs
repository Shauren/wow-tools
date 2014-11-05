using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RedirectDecrypt
{
    class Program
    {
        private struct EncryptedData
        {
            private string _clientVersion;

            public string ClientVersion
            {
                get { return _clientVersion; }
                set
                {
                    _clientVersion = value;
                    switch (value)
                    {
                        case "4.3.4":
                            Type = typeof(ClientConnectionPayload434);
                            break;
                        case "5.3.0":
                            Type = typeof(ClientConnectionPayload530);
                            break;
                        case "6.0.3":
                            Type = typeof(ClientConnectionPayload603);
                            break;
                    }
                }
            }

            public Type Type { get; private set; }

            public byte[] Data { get; set; }
        }

        public static string ByteArrayToHexString(byte[] arr)
        {
            return BitConverter.ToString(arr).Replace("-", "");
        }

        public static byte[] HexStringToByteArray(String hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }

        public static byte[] RSAPublicDecrypt(byte[] cipherData, RSAParameters rsaParams)
        {
            if (cipherData == null)
                throw new ArgumentNullException("cipherData");

            BigInteger numEncData = new BigInteger(cipherData);
            BigInteger exponent = new BigInteger(rsaParams.Exponent);
            BigInteger modulus = new BigInteger(rsaParams.Modulus);

            return BigInteger.ModPow(numEncData, exponent, modulus).ToByteArray();
        }

        static void Main(string[] args)
        {
            var rsaParams = new RSAParameters();
            rsaParams.Exponent = new byte[]
            {
                0x01, 0x00, 0x01, 0x00
            };

            rsaParams.Modulus = new byte[]
            {
                0x91, 0xD5, 0x9B, 0xB7, 0xD4, 0xE1, 0x83, 0xA5, 0x22, 0x2B, 0x5F, 0x38, 0xF4, 0xB8, 0x86, 0xFF,
                0x32, 0x84, 0x38, 0x2D, 0x99, 0x38, 0x8F, 0xBA, 0xF3, 0xC9, 0x22, 0x5D, 0x51, 0x73, 0x1E, 0x28,
                0x87, 0x24, 0x8F, 0xB5, 0xC9, 0xB0, 0x7C, 0x95, 0xD0, 0x6B, 0x5B, 0xF4, 0x94, 0xC5, 0x94, 0x9D,
                0xFA, 0x6F, 0x47, 0x3A, 0xA3, 0x86, 0xC0, 0xA8, 0x37, 0xF3, 0x9B, 0xEF, 0x2F, 0xC1, 0xFB, 0xB3,
                0xF4, 0x1C, 0x2B, 0x0E, 0xD3, 0x6D, 0x88, 0xBB, 0x02, 0xE0, 0x4E, 0x63, 0xFA, 0x76, 0xE3, 0x43,
                0xF9, 0x01, 0xFD, 0x23, 0x5E, 0x6A, 0x0B, 0x14, 0xEC, 0x5E, 0x91, 0x34, 0x0D, 0x0B, 0x4F, 0xA3,
                0x5A, 0x46, 0xC5, 0x5E, 0xDC, 0xB5, 0xCD, 0xC1, 0x47, 0x6B, 0x59, 0xCA, 0xFA, 0xA9, 0xBE, 0x24,
                0x9F, 0xF5, 0x05, 0x6B, 0xBB, 0x67, 0x8B, 0xB7, 0xE4, 0x3A, 0x43, 0x00, 0x5C, 0x1C, 0xB7, 0xCA,
                0x98, 0x90, 0x79, 0x77, 0x6D, 0x05, 0x4F, 0x83, 0xCC, 0xAC, 0x06, 0x2E, 0x50, 0x11, 0x87, 0x23,
                0xD8, 0xA6, 0xF7, 0x6F, 0x7A, 0x59, 0x87, 0xA6, 0xDE, 0x5D, 0xD8, 0xEC, 0x44, 0xBE, 0x45, 0x31,
                0x7F, 0x8A, 0xF0, 0x58, 0x89, 0x53, 0x74, 0xDF, 0xCC, 0xAD, 0x01, 0x24, 0xD8, 0x19, 0x65, 0x1C,
                0x25, 0xD3, 0xE1, 0x6B, 0x8B, 0xDA, 0xFE, 0x1D, 0xA4, 0x2C, 0x8B, 0x25, 0xED, 0x7C, 0xFF, 0x6A,
                0xE0, 0x63, 0xD0, 0x52, 0x20, 0x7E, 0x62, 0x49, 0xD2, 0xB3, 0x6B, 0xCC, 0x91, 0x69, 0xA5, 0x08,
                0x8B, 0x69, 0x65, 0xFF, 0xB9, 0xC9, 0x17, 0x02, 0x5D, 0xD8, 0x8E, 0x1A, 0x63, 0xD9, 0x2A, 0x7F,
                0xDB, 0xE3, 0xF8, 0x76, 0x6D, 0xEA, 0x0E, 0x36, 0x98, 0x78, 0x19, 0xC5, 0x87, 0xBA, 0x6C, 0x20,
                0xB6, 0x08, 0x44, 0x04, 0x4C, 0xA8, 0xD5, 0xB6, 0x9D, 0x4D, 0x00, 0x20, 0x40, 0x00, 0x90, 0x04
            };

            var toDecrypt = new List<EncryptedData>();
            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "4.3.4",
                // hex string from WPP output
                Data = HexStringToByteArray("8ABF5E34296D64AA51D4B09F9EB6014999B0153A56A5900BACBF18A0D87CF74193D17778163A6727DA6F947AF8C2F6C6AB9E767320757C76C8BF9E298D8EF266DA5C272BE1D3A60003301F46DEC28BEAD8AF15B98D696AF75722D07F97333C1C8C16E07BFBB5DA69178D1FC18BC96377C86C85799C96FB890B33AAA901E36F7CD9CBDEE172846F2139F699560EF8CDD5954441D980D257BB37948A63D7F3B4014ED3565892D139CDEC1982FF09655C899DBA919D36B9DD9C796F22653805FCCD65C1782D1BE68E3F3DF11EB4F6EAEEB410DD6A6C47BB8CE33B1D85E56E73AEABD2E78B97EEE9699E85B806387B7149FFE290E60D8DE9AA1756B8A4FB78516602")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "4.3.4",
                // hex string from WPP output
                Data = HexStringToByteArray("0E72CD6E98C0CEB89507FE7853EB85A2DB2314E3A58B51B4CFFE1AA965E8E7F1A7BE10792D0DC053DE614A56466B7C92E3BD6BAB89861009FD772888C3A4A7336559620951B1202529F8FB3F5F6093FFED7F1758548FB4F1777D243D7879A3A8C8F447820BF74A2E36C92A423BC6E35A07144CE2EDA3659E88EE89AD7AA5E5A135A4D1E2068CED83CE8D530F39D88AF408F911309559F81C6C450A4279B68688A2199DE7CCE90FACCFAD9A3DE8A0480B855ED46359D8F95D62DEC0337D5BCCEFA83ECE94B8B3C2A102C371EFDADCA089BDD203318DAFCC3479370B2D4CACD5DC018A3B445E4AB83FD1D44820A3FA09E2C2790465EBBB27EBCE2ABA408839F201")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "4.3.4",
                // hex string from WPP output
                Data = HexStringToByteArray("D5513A5860D837D1A7B3AFDD72E2803611646E501CD31A837746F6B03D9C6B57E79D389FDEDBCEDF971CC4A94957D202837B09CA7A7ADE70ADED06DFDFA79AF6664F9F139548E0AE8D2E44F88364EF34FA16C918ED6486B1CD34E2E8F45C9E126C871F46D8B2E71D6379E112ADB16C500F2E4F9D58FDA3330B4F8520DEC924B998702E6297C56D68454DE8DB12561DAC1A0035CB88721F04C50D8E04FC31F0F407B58665562E639C8870917A85E5C73FBA95AB2842FAC9B859D10E040AFA7B2D3EF6DC3671514D3C51D813EBE0677A9811B276137029B1B7404BB8C80CDE43E8F52D2F8D7D7B8B27FD06E73465021CF0BED01F64469B909F8D4274F281297A04")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "4.3.4",
                // hex string from WPP output
                Data = HexStringToByteArray("95C928E20340538106992950BC448DB67A18DF7BF4D834C3A083A3CD824164D65EAD87F4DF1EF0BD3004B5CA37680081D97B44EA37798A713084478F908DC156C1B3DBF9C182A6BAA5E814EC15C68AF97271BEA328BE946285720022DF750B3E7EE84700771468273E1390FFEC3C07581CF8832BC27DE52054BBACB03899CA6E2C290729BEE68007FD83AF85E16B1783FC6F9D3A9C153062B9750FC101AE547912FAD6B6D52038E271FBC661BB14C91C6B27FCFC5909C64670BCFBFFD571D3C1DB0C37B708ACBAA67D44664D52485EA059B2D92614B5DD5789462BEE3588EE22C40429596420188DB88ECB1127E88D1DDDBB3AE97293E9CDCB74E4DF030BE600")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "5.3.0",
                // dumped from client memory
                Data = new byte[]
                {
                    0x6F, 0xC2, 0x13, 0x62, 0x56, 0x7F, 0xD1, 0x72, 0xDA, 0x36, 0xA0, 0xF3, 0xBB, 0x5D, 0x5E, 0x91,
                    0xDA, 0xCB, 0x4A, 0x8B, 0xD2, 0x5B, 0x81, 0xB7, 0x2C, 0x18, 0x79, 0x85, 0xC0, 0xD9, 0x23, 0x7F,
                    0xA9, 0xC9, 0x34, 0x44, 0x5A, 0xF8, 0xF8, 0x8D, 0xD6, 0x53, 0x0C, 0x29, 0x3E, 0x5C, 0x40, 0x55,
                    0x78, 0x4C, 0xD9, 0x5E, 0x3E, 0x52, 0xB4, 0x78, 0xF4, 0xEE, 0x5B, 0xEC, 0x18, 0x84, 0x3C, 0x5A,
                    0x35, 0xB8, 0xFA, 0xC6, 0xBC, 0x3C, 0xDB, 0x18, 0xB9, 0x72, 0x38, 0x8D, 0x04, 0xDB, 0x9C, 0x33,
                    0x1F, 0x9A, 0xD1, 0xE5, 0xC7, 0x99, 0xD0, 0xD0, 0x45, 0xFE, 0x5A, 0x7D, 0xBC, 0x18, 0x2C, 0x40,
                    0xDE, 0xFB, 0x89, 0xA8, 0xD2, 0xF3, 0xA4, 0x17, 0xA9, 0xF4, 0xB9, 0x7A, 0x21, 0xD0, 0xB9, 0x62,
                    0x43, 0x2F, 0x13, 0x0A, 0xB8, 0xCC, 0xE9, 0x60, 0x35, 0xD0, 0x93, 0xB1, 0x94, 0x99, 0x76, 0x6C,
                    0x4E, 0x14, 0xAF, 0xBD, 0x42, 0x5B, 0xFD, 0xC7, 0x2F, 0xF6, 0x53, 0xB2, 0x65, 0x93, 0x5D, 0xC2,
                    0x92, 0xC1, 0xDF, 0x2D, 0xAF, 0xE0, 0xD3, 0x5D, 0x17, 0xF1, 0x82, 0xBF, 0xBD, 0xCF, 0x91, 0x68,
                    0x4E, 0x10, 0x69, 0xF3, 0xF7, 0xA1, 0x2F, 0x75, 0xED, 0xEE, 0xF0, 0xF4, 0xED, 0x10, 0x18, 0x74,
                    0xDB, 0x72, 0x19, 0xDC, 0x27, 0xB7, 0x2E, 0x63, 0x52, 0x0F, 0xC4, 0x49, 0x0D, 0xE2, 0xE2, 0xF6,
                    0xB5, 0x4F, 0x57, 0xB9, 0x63, 0x64, 0x64, 0x3B, 0x42, 0x08, 0xCF, 0xDB, 0xE9, 0xD6, 0x9E, 0x70,
                    0x8D, 0x37, 0x0B, 0x5F, 0x4F, 0xE5, 0xE1, 0xD7, 0x50, 0x25, 0xBA, 0xF7, 0x24, 0x0B, 0x9F, 0x62,
                    0x91, 0x67, 0xCF, 0x51, 0xEC, 0x55, 0xA4, 0xA9, 0x8A, 0xB7, 0x94, 0xBF, 0xBF, 0x28, 0x1B, 0x3C,
                    0x50, 0xCB, 0x6E, 0x8F, 0xE8, 0x54, 0x97, 0xEF, 0xD7, 0x01, 0xAB, 0x4E, 0x50, 0x73, 0x96, 0x01
                }
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "6.0.3",
                Data = HexStringToByteArray("4A338ACA6F43DFE25B4FDDB5589A974422CF1E2520F97CA3B84F29F1DDF9617035FEBCE6E0B73EEFD6B00FB204FB9551FBF8D74978C4ACD501E551D0ACC7731F549E86B343A56B5BE19A56163EEF0C3E562E18C90F12F597C89CFB1DD8F0BBACA4BAF6EAD448EC379EEA14512BCF075CCCB466189850C5F0C8315118B8F66D534500D497541B703C9F1759ED458C3AB894814DCD487149EB743ADDDAA6187EC9FF6646C46384AA6B86836474B4DE79B05B75C7338A7393FD1812D8F8210250AD763E8EEC9D45F1998372ED746E42DB2A4924D157E3682D6254558E1CB6C63FB4379B23F5E15FE95DCE82D8D123704AD521720E428C6481BE3E68EA0462EB7E01")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "6.0.3",
                Data = HexStringToByteArray("04D722F679F59753E17FBF4C2F5D3E83C76B8580C653EC5A86FC331CBAEEC8EAEF520DF0192CA0E067AE97000C9D06357442293198C9879C9AB4D38E920D406166F054ABCCE0553C5D5E3339395412AC98050D4DB19D06FDF337A6A4E260D46AFE869F30E47A156312D3F6047EC676066B68E27FA9B166025CBE5D0D498E051F9D0B08A6F842FE0DDE8398E95B5047A2F6C6E6A6F1D0D94EAB1EC86590AE247145AA7428CBB7B885E48F66B5A1594E8E489E49D36882DB412AAC2C44D26B5C56D2D382564A09909B347834600E07322D589113D8780ED68D0DC9D0970580997B8304EA1FA4532C41261FB2B9DAC357BF5D8FEA2A38CC40FB4DE0BA67D3F77E03")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "6.0.3",
                Data = HexStringToByteArray("99EFD4FE24EA474EAC2744E14244DDF3D51ACB196AA61C480B9A13D0EDDB6A23387538795D75F8B5B700E0BF49A0175B3450E8707BDEE3812C7F6025B43FE4DEB747A06C80355A1E9CB2B9F7FBB7C44DA589E5B71241D9847F2C6689F26107914611AC578A0AE9070A1E0AB9F0202105A7F49C1BE1D3AB65F7E72E43D8EA02E3E8D0B778448B2D5F33DF9432795B161F17C121DDE6B000416414A579713E26F01B92C725FB5F35A71D478F9CD77A143AB5A231F603047A2419D82E23D9CCCBAE5A6CCEA7BE837E4DDBA5B6F53AD75B06283D130D865006CF344928614C4730753B61C57650AAA1F62A75ABA3A4B978B89A75785653A5DF9AE88CDFE1630DC301")
            });

            toDecrypt.Add(new EncryptedData()
            {
                ClientVersion = "6.0.3",
                Data = HexStringToByteArray("D674B4C55A95B8210C1B9CC730D804C89514BCC8B9AE98090AE10505FEE983BD12487B6E254500655FE2E2C32B720DEC690BCB46BB09E7D6D78659DC4AB6063EECE3017ABF95DA53DC16A203FFF2287B594493C62C6014DB56D4A44CEA3E8E7ECC4C22BF4974BFE39F7F2835CDB92866DF0B9DB06EB0633E78B0B5EEBD516C7BBE52AD2509E217CE93A484C279B764EEC98E6D1CEBDAF47EDAD228473CC2E2687158BD0CC101EA2F47F63ACCDB72928B9B85807ED29E8848F39370A9FF4BDA0744987D7C0BDC62962DFEEB4A3C6CE7DA66F1548E98BE7EA715A6B5311839E45F2D182FE9F8E42DC80D02B26B5E0B7858E66586953A45BA91A4DA894CD2D74100")
            });

            foreach (var block in toDecrypt)
            {
                var decrypt = RSAPublicDecrypt(block.Data, rsaParams);
                var payload = (ClientConnectionPayload)Activator.CreateInstance(block.Type, new object[] { decrypt });
                Console.WriteLine("Version: {0}", block.ClientVersion);
                Console.WriteLine(payload.ToString());
            }

            Console.ReadKey();
        }
    }
}
