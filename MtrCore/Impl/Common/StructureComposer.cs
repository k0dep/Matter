using System;
using System.Threading;

namespace MatterCore.Common
{
    public sealed class StructureComposer
    {
        public byte[] Decompose(string format, int input)
        {
            GetItemsByFormat(format);

            var buffer = new byte[format.Length];

            var shifted = 0;

            for (int i = format.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte)((input >> (shifted * 4)) & (format[i] == '1' ? 0xF : 0xFF));

                shifted += (format[i] - '0');
            }

            return buffer;
        }

        public int Compose(string format, params byte[] args)
        {
            var items = GetItemsByFormat(format);
            if(args.Length != format.Length)
                throw new ArgumentException("args");

            var result = 0;
            
            for (int i = 0; i < args.Length; i++)
            {
                result = result << 4;
                if(format[i] == '2')
                    result = result << 4;

                result |= args[i] & (format[i] == '1' ? 0xF : 0xFF);
            }

            return result;
        }

        private int GetItemsByFormat(string format)
        {
            var halfByteCounter = 0;
            foreach (var charItem in format)
            {
                if (charItem == '1')
                    halfByteCounter += 1;
                else if (charItem == '2')
                    halfByteCounter += 2;
                else
                    throw new ArgumentException("format");
            }
            if(halfByteCounter > 8)
                throw new ArgumentException("format");

            return halfByteCounter;
        }
    }
}
