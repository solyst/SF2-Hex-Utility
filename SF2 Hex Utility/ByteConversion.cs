using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SF2_Hex_Utility
{
    class ByteConversion
    {
        //------------------------------------------------------------------
        // To Byte Functions
        //------------------------------------------------------------------
        // Convert various inputs into byte[].
        #region ToByte_Functions


        static public string CleanHexString(string value)
        {
            //replace spaces, dashes, carriage returns, tabs, and the hex prefaces "0x" and "&H"
            return value.ToUpper().Trim().Replace(" ", string.Empty).Replace("-", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("0X", "").Replace("&H", "");
        }


        /// <summary>
        /// Convert a string of hex into bytes.
        /// </summary>
        /// <param name="value">Hex string.</param>
        /// <returns>Bytes corresponding to the hex string.</returns>
        static public byte[] ToBytesFromHex(string value)
        {
            try
            {
                value = CleanHexString(value);
                //fix odd length values
                if (value.Length % 2 != 0)
                    value = "0" + value;
                //convert the resulting string into hex
                //stolen from https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa
                int NumberChars = value.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars / 2; i++)
                    bytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
                return bytes;
            }
            catch (Exception)
            {
                return new byte[] { 0 };
            }
        }

        //Same as above, but will expand or truncate the result as requested.
        static public byte[] ToBytesFromHex(string value, int outputLength)
        {
            return SetHexByteLength(ToBytesFromHex(value), outputLength);
        }


        /// <summary>
        /// Convert a string of hex into bytes, with double underscores "__" being changed to NULL bytes to represent wildcard bytes.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Array of bytes corresponding to the hex string, where NULL bytes are wildcards.</returns>
        static public byte?[] ToBytesFromHexWildcard(string value)
        {
            value = value.Replace("__", "?"); //Replace every double group of underlines (representing a single byte) into a single question mark.
            value = value.Replace("_ ", "?").Replace(" _","?"); //If the user didn't use double underlines, then search for underline + space combinations and assume that meant a single byte.
            value = CleanHexString(value); //Remove spaces and other bad characters.
            char[] chars = value.ToCharArray();
            List<byte?> returnBytes = new List<byte?>();
            int i = 0;
            int old = i;
            while (i < value.Length)
            {
                if (chars[i] == '?')
                {
                    addAndConvertBytesToListFromHexString(value.Substring(old, i - old), returnBytes);
                    returnBytes.Add(null);
                    old = i + 1;
                }
                i++;
            }
            addAndConvertBytesToListFromHexString(value.Substring(old, i - old), returnBytes);
            return returnBytes.ToArray();
        }


        //Helper for ToBytesFromHexWildcard.
        //This appropriately handles the case when hexString contains zero bytes.
        static private void addAndConvertBytesToListFromHexString(string hexString, List<byte?> byteList)
        {
            //Because {0} is a valid result and also the return value for ToBytesFromHex when the hex string is invalid or empty,
            // append 255 to the beginning of the string to verify the result is in fact valid and not empty or an error.
            byte[] bytes = ToBytesFromHex("FF" + hexString);
            if (bytes[0] == 255)
            {
                foreach (byte b in bytes.Skip(1))
                {
                    byteList.Add(b);
                }
            }
        }


        /// <summary>
        /// Convert a string of ASCII characters into bytes.
        /// </summary>
        /// <param name="value">String of ASCII characters.</param>
        /// <param name="outputLength">If > 0, set the number of byte outputted to this length. Will truncate or expand the result as necessary.</param>
        /// <returns>Bytes corresponding to the ASCII characters.</returns>
        static public byte[] ToBytes(string value, int outputLength = 0)
        {
            if (outputLength > 0)
                return SetASCIIByteLength(Encoding.ASCII.GetBytes(value), outputLength);
            else
                return Encoding.ASCII.GetBytes(value);
        }


        /// <summary>
        /// Convert a signed integer into bytes.
        /// </summary>
        /// <param name="value">Signed integer.</param>
        /// <param name="outputLength">If > 0, set the number of byte outputted to this length. Will truncate or expand the result as necessary.</param>
        /// <returns>Bytes corresponding to the signed integer.</returns>
        static public byte[] ToBytes(Int64 value, int outputLength = 0)
        {
            //LittleEdian consideration from https://stackoverflow.com/questions/1318933/c-sharp-int-to-byte
            byte[] intBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            if (outputLength > 0)
                return SetIntByteLength(intBytes, outputLength);
            else
                return intBytes;
        }


        /// <summary>
        /// Convert a unsigned integer into bytes.
        /// </summary>
        /// <param name="value">Unsigned integer.</param>
        /// <param name="outputLength">If > 0, set the number of byte outputted to this length. Will truncate or expand the result as necessary.</param>
        /// <returns>Bytes corresponding to the unsigned integer.</returns>
        static public byte[] ToBytes(UInt64 value, int outputLength = 0)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            if (outputLength > 0)
                return SetUIntByteLength(intBytes, outputLength);
            else
                return intBytes;
        }


        #endregion



        //------------------------------------------------------------------
        // Set Byte Length Functions
        //------------------------------------------------------------------
        // These functions convert byte[] into the requested length.
        #region SetByteLength_Functions

        static public byte[] SetUIntByteLength(byte[] bytes, int length)
        {
            return SetBytesLength(bytes, length, 0, true);
        }

        static public byte[] SetIntByteLength(byte[] bytes, int length)
        {
            if (bytes[0] == 0)
                return SetBytesLength(bytes, length, 0, true); //pad with 0
            else
                return SetBytesLength(bytes, length, 255, true); //pad with FF
        }

        static public byte[] SetASCIIByteLength(byte[] bytes, int length)
        {
            return SetBytesLength(bytes, length, ' ', false); //pad with a space
        }

        static public byte[] SetHexByteLength(byte[] bytes, int length, bool preserveRightMostBytes = false)
        {
            return SetBytesLength(bytes, length, 0, preserveRightMostBytes); //pad with zero
        }


        //Same as below, but uses a char value instead and converts it into a byte.
        static public byte[] SetBytesLength(byte[] bytes, int length, char padValue, bool preserveRightMostBytes = false)
        {
            return SetBytesLength(bytes, length, Encoding.ASCII.GetBytes(new char[] { padValue })[0], preserveRightMostBytes);
        }


        /// <summary>
        /// Expand or truncate bytes to fit a determined length.
        /// </summary>
        /// <param name="bytes">Bytes to fit to the determiend length.</param>
        /// <param name="length">The length to which to fit the bytes.</param>
        /// <param name="padValue">When expanding the bytes, this is the value which is used for the new bytes.</param>
        /// <param name="preserveRightMostBytes">When truncating bytes, if TRUE the first bytes are dropped first. When FALSE the last bytes are dropped first.</param>
        /// <returns>The expanded or truncated result.</returns>
        static public byte[] SetBytesLength(byte[] bytes, int length, byte padValue, bool preserveRightMostBytes = false)
        {
            if (bytes.Length == length)
            {
                //Correct size
                return bytes;
            }
            else
            {
                if (bytes.Length < length)
                {
                    //Too few bytes
                    if (preserveRightMostBytes)
                        return padbytesOnLeft(bytes, length, padValue);
                    else
                        return padbytesOnRight(bytes, length, padValue);
                }
                else
                {
                    //Too many bytes
                    if (preserveRightMostBytes)
                        return bytes.Skip(bytes.Length - length).ToArray();
                    else
                        return bytes.Take(length).ToArray();
                }
            }
        }


        //Reize the array and set the new elements to padvalue.
        static private byte[] padbytesOnRight(byte[] bytes, int length, byte padvalue)
        {
            if (bytes.Length < length)
            {
                Array.Resize(ref bytes, length);
                for (int i = bytes.Length; i < length; i++)
                {
                    bytes[i] = padvalue;
                }
            }
            return bytes;
        }


        //Resize the array and move all the old elements to the new elements, replacing the original values with padvalue.
        static private byte[] padbytesOnLeft(byte[] bytes, int length, byte padvalue)
        {
            if (bytes.Length < length)
            {
                int lengthDifference = length - bytes.Length;
                Array.Resize(ref bytes, length);
                for (int i = length - 1; i >= 0; i--)
                {
                    if (i > lengthDifference - 1)
                    {
                        bytes[i] = bytes[i - lengthDifference];
                    }
                    else
                    {
                        bytes[i] = padvalue;
                    }
                }
            }
            return bytes;
        }

        #endregion



        //------------------------------------------------------------------
        // Byte to Object Functions
        //------------------------------------------------------------------
        // Convert byte[] to various outputs.
        #region ByteToObject_Functions

        
        /// <summary>
        /// Convert the provided bytes into an integer of equivalent value.
        /// </summary>
        /// <param name="bytes">Accepts byte lengths of 1, 2, 4, or 8.</param>
        /// <returns>Covert the return value into the appropriate integer size.</returns>
        static public Int64 ToInt(byte[] bytes)
        {
            byte[] intBytes = (byte[])bytes.Clone();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            switch (bytes.Length)
            {
                case 1:
                    return (sbyte)intBytes[0];
                case 2:
                    return BitConverter.ToInt16(intBytes, 0);
                case 4:
                    return BitConverter.ToInt32(intBytes, 0);
                case 8:
                    return BitConverter.ToInt64(intBytes, 0);
                default:
                    return 0;
            }
        }


        /// <summary>
        /// Convert the provided bytes into an unsigned integer of equivalent value.
        /// </summary>
        /// <param name="bytes">Accepts byte lengths of 1, 2, 4, or 8.</param>
        /// <returns>Covert the return value into the appropriate integer size.</returns>
        static public UInt64 ToUInt(byte[] bytes)
        {
            byte[] intBytes = (byte[])bytes.Clone();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            switch (bytes.Length)
            {
                case 1:
                    return intBytes[0];
                case 2:
                    return BitConverter.ToUInt16(intBytes, 0);
                case 4:
                    return BitConverter.ToUInt32(intBytes, 0);
                case 8:
                    return BitConverter.ToUInt64(intBytes, 0);
                default:
                    return 0;
            }
        }


        /// <summary>
        /// Convert the provided bytes into hex string of equivalent value.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Hex string.</returns>
        static public string ToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }


        /// <summary>
        /// Convert the provided bytes and wildcard bytes into hex string of equivalent value.
        /// </summary>
        /// <param name="bytes">Nullable array of bytes.</param>
        /// <returns>Hex string with double underscore representing wildcards.</returns>
        static public string ToHexWildcard(byte?[] bytes)
        {
            if (bytes == null)
            {
                return String.Empty;
            }
            StringBuilder sb = new StringBuilder();
            foreach (byte? b in bytes)
            {
                if (b == null)
                {
                    sb.Append("__");
                }
                else
                {
                    sb.Append(BitConverter.ToString(new byte[] { (byte)b }));
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Convert the provided bytes into an ASCII string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>ASCII string.</returns>
        static public string ToASCIIString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }


        #endregion


    }
}
