﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Text;
using Windows.Foundation.Metadata;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace UnityUWPBTLEPlugin
{
    public static class GattConvert
    {
        [DefaultOverload()]
        public static IBuffer ToIBuffer(string data)
        {
            DataWriter writer = new DataWriter();
            writer.WriteString(data);
            return writer.DetachBuffer();
        }

        public static IBuffer ToIBufferFromHexString(string data)
        {
            data = data.Replace("-", "");
            int NumberChars = data.Length;
            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            DataWriter writer = new DataWriter();
            writer.WriteBytes(bytes);
            return writer.DetachBuffer();
        }
        public static IBuffer ToIBuffer(Int32 data)
        {
            DataWriter writer = new DataWriter();
            writer.WriteInt32(data);
            return writer.DetachBuffer();
        }

        public static string ToUTF8String(IBuffer buffer)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            return Encoding.UTF8.GetString(data);
        }

        public static string ToUTF16String(IBuffer buffer)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            return Encoding.Unicode.GetString(data);
        }

        public static int ToInt32(IBuffer buffer)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            data = GetBytes(data, 4);
            return BitConverter.ToInt32(data, 0);
        }

        public static string ToHexString(IBuffer buffer)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            return BitConverter.ToString(data);
        }

        /// <summary>
        /// Takes an input array of bytes and returns an array with more zeros in the front
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns>A byte array with more zeros in front"/></returns>
        private static byte[] GetBytes(byte[] input, int length)
        {
            byte[] ret = new byte[length];

            if (input.Length >= length)
            {
                ret = input;
            }

            int offset = length - input.Length;
            for (int i = 0; i < input.Length; i++)
            {
                ret[offset + i] = input[i];
            }

            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(ret);
            }

            return ret;
        }


    }
}
