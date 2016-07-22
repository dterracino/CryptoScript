using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CryptoScript.Classes
{
    public class CRC
    {
        private static readonly uint[] _crc32Table = new uint[256];
        private static readonly uint _ulPolynomial = 0x04c11db7;


        public static uint GenCRC32(string data)
        {
                    InitCrcTable();

                    uint crc = getCRC(Encoding.ASCII.GetBytes(data), Encoding.ASCII.GetBytes(data).Length);

                    return crc;
        }

        private static void InitCrcTable()
        {

            // 256 values representing ASCII character codes.

            for (uint i = 0; i <= 0xFF; i++)
            {
                _crc32Table[i] = Reflect(i, 8) << 24;

                for (uint j = 0; j < 8; j++)
                {
                    long val = _crc32Table[i] & (1 << 31);

                    if (val != 0)
                        val = _ulPolynomial;
                    else val = 0;

                    _crc32Table[i] = (_crc32Table[i] << 1) ^ (uint)val;
                }

                _crc32Table[i] = Reflect(_crc32Table[i], 32);
            }



        }

        private static uint Reflect(uint re, byte ch)
        {  // Used only by Init_CRC32_Table()

            uint value = 0;

            // Swap bit 0 for bit 7
            // bit 1 for bit 6, etc.
            for (int i = 1; i < (ch + 1); i++)
            {
                long tmp = re & 1;
                int v = ch - i;

                if (tmp != 0)
                    value |= (uint)1 << v; //(uint)(ch - i));

                re >>= 1;
            }

            return value;
        }

        private static uint getCRC(byte[] buffer, int bufsize)
        {

            uint crc = 0xffffffff;
            int len = bufsize;
            // Save the text in the buffer.

            // Perform the algorithm on each character
            // in the string, using the lookup table values.

            for (uint i = 0; i < len; i++)
                crc = (crc >> 8) ^ _crc32Table[(crc & 0xFF) ^ buffer[i]];


            // Exclusive OR the result with the beginning value.
            return crc ^ 0xffffffff;

        }


    }

    class CRC32
    {
    public class Crc32 : HashAlgorithm
    {
        public const UInt32 DefaultPolynomial = 0xedb88320;
        public const UInt32 DefaultSeed = 0xffffffff;

        private UInt32 hash;
        private UInt32 seed;
        private UInt32[] table;
        private static UInt32[] defaultTable;

        public Crc32()
        {
            table = InitializeTable(DefaultPolynomial);
            seed = DefaultSeed;
            Initialize();
        }

        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            table = InitializeTable(polynomial);
            this.seed = seed;
            Initialize();
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            hash = CalculateHash(table, hash, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize
        {
            get { return 32; }
        }

        public static UInt32 Compute(byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
                return defaultTable;

            UInt32[] createTable = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 entry = (UInt32)i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                defaultTable = createTable;

            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
        {
            UInt32 crc = seed;
            for (int i = start; i < size; i++)
                unchecked
                {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            return crc;
        }

        private byte[] UInt32ToBigEndianBytes(UInt32 x)
        {
            return new byte[] {
            (byte)((x >> 24) & 0xff),
            (byte)((x >> 16) & 0xff),
            (byte)((x >> 8) & 0xff),
            (byte)(x & 0xff)
        };
        }

        public string Get(string FilePath)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;

            using (FileStream fs = File.Open(FilePath, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

            return hash;
        }
    }
}
}
