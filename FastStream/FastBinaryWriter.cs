using System;
using System.IO;

namespace FastStream {
    public class FastBinaryWriter : BinaryWriter {
        public FastBinaryWriter(Stream output) : base(output){

        }

        public override unsafe void Write(string value){
            int length = value.Length;
            byte[] bytes = new byte[length * sizeof(char) + sizeof(int)];

            fixed (char* chars = value) {
                fixed (byte* tmp = &bytes[0]) {
                    for (int i = 0; i < length; i++) {
                        tmp[i * 2 + 4] = (byte)chars[i];
                        tmp[i * 2 + 5] = (byte)(chars[i] >> 8);
                    }
                }
            }

            bytes[0] = (byte)length;
            bytes[1] = (byte)(length >> 8);
            bytes[2] = (byte)(length >> 16);
            bytes[3] = (byte)(length >> 24);

            OutStream.Write(bytes, 0, bytes.Length);
        }
        public unsafe void Write(string[] array) {
            int length = 0;
            for (int i = 0; i < array.Length; i++) {
                length += array[i].Length * sizeof(char) + sizeof(int);
            }

            byte[] bytes = new byte[length];

            fixed (byte* tmp = bytes) {
                int index = 0;

                for (int i = 0; i < array.Length; i++) {
                    int stLength = array[i].Length;

                    tmp[index] = (byte)stLength;
                    tmp[index + 1] = (byte)(stLength >> 8);
                    tmp[index + 2] = (byte)(stLength >> 16);
                    tmp[index + 3] = (byte)(stLength >> 24);
                    index += sizeof(int);

                    fixed (char* chars = array[i]) {
                        for (int j = 0; j < stLength; j++) {
                            tmp[index++] = (byte)chars[j];
                            tmp[index++] = (byte)(chars[j] >> 8);
                        }
                    }
                }
            }

            OutStream.Write(bytes, 0, bytes.Length);
        }

        public override unsafe void Write(char ch){
            byte[] bytes = new byte[sizeof(char)];

            byte* b = (byte*)&ch;

            bytes[0] = b[0];
            bytes[1] = b[1];

            OutStream.Write(bytes, 0, bytes.Length);
        }
        public override unsafe void Write(char[] chars){
            byte[] bytes = new byte[chars.Length * sizeof(char)];

            fixed (char* ch = chars) {
                int index = 0;
                int length = chars.Length;

                for (int i = 0; i < length; i++) {
                    byte* b = (byte*)&ch[i];

                    bytes[index++] = b[0];
                    bytes[index++] = b[1];
                }
            }

            OutStream.Write(bytes, 0, bytes.Length);
        }
        public override unsafe void Write(char[] chars, int offset, int count){
            byte[] bytes = new byte[count * sizeof(char)];

            fixed (char* ch = chars) {
                int index = offset;
                int length = offset + count;

                for (int i = offset; i < count; i++) {
                    byte* b = (byte*)&ch[i];

                    bytes[index++] = b[0];
                    bytes[index++] = b[1];
                }
            }

            OutStream.Write(bytes, 0, bytes.Length);
        }

        public override unsafe void Write(decimal value){
            byte[] bytes = new byte[sizeof(decimal)];

            byte* b = (byte*)&value;
            for (int i = 0; i < 16; i++) {
                bytes[i] = b[i];
            }

            OutStream.Write(bytes, 0, bytes.Length);
        }
        public unsafe void Write(decimal[] array) {
            byte[] bytes = new byte[array.Length * sizeof(decimal)];

            fixed (decimal* d = array) {
                int index = 0;

                for (int i = 0; i < array.Length; i++) {
                    byte* b = (byte*)&d[i];

                    for (int j = 0; j < 16; j++) {
                        bytes[index++] = b[j];
                    }
                }
            }

            OutStream.Write(bytes, 0, bytes.Length);
        }
    }
}