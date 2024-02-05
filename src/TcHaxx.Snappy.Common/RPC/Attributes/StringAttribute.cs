using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.Common.RPC.Attributes
{
    /// <summary>
    /// Defines the expected max string length and encoding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public class StringAttribute : Attribute
    {
        public uint Length;
        public StringEncoding Encoding;

        public StringAttribute(uint length, StringEncoding stringEncoding = StringEncoding.UTF8)
        {
            if (length == 0)
            {
                throw new ArgumentNullException(nameof(length), "Length of 0 is not allowed.");
            }
            if (length > Constants.MAX_STRING_PARAMETER_LENGTH)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, $"Maximum string-length is {Constants.MAX_STRING_PARAMETER_LENGTH}.");
            }
            Length = length;
            Encoding = stringEncoding;
        }
        public Encoding GetEncoding()
        {
            return Encoding switch
            {
                StringEncoding.UNICODE => System.Text.Encoding.Unicode,
                StringEncoding.ASCII => System.Text.Encoding.ASCII,
                _ => System.Text.Encoding.UTF8
            };
        }
    }
}
