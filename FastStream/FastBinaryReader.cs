using System;

namespace FastStream
{
    public class FastBinaryReader {
        private byte[] Bytes;
        private int Index = 0;

        public FastBinaryReader(byte[] buffer) {
            Bytes = buffer;
        }
        public FastBinaryReader(byte[] buffer, int index) {
            Bytes = buffer;
            Index = index;
        }

        public void Jump(int offset) {
            Index += offset;
        }

        public byte ReadByte() {
            return Bytes[Index++];
        }
        public byte[] ReadBytes(int count) {
            byte[] array = new byte[count];

            Buffer.BlockCopy(Bytes, Index, array, 0, count);
            Index += count;

            return array;
        }

        public sbyte ReadSByte() {
            return (sbyte)Bytes[Index++];
        }
        public sbyte[] ReadSBytes(int count) {
            sbyte[] array = new sbyte[count];

            Buffer.BlockCopy(Bytes, Index, array, 0, count);
            Index += count;

            return array;
        }

        public unsafe short ReadInt16() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(short);
                return *(short*)bytes;
            }
        }
        public short[] ReadInt16Array(int count) {
            int offset = count * sizeof(short);

            short[] array = new short[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe int ReadInt32() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(int);
                return *(int*)bytes;
            }
        }
        public unsafe int[] ReadInt32Array(int count) {
            int offset = count * sizeof(int);

            int[] array = new int[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe long ReadInt64() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(long);
                return *(long*)bytes;
            }
        }
        public long[] ReadInt64Array(int count) {
            int offset = count * sizeof(long);

            long[] array = new long[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe ushort ReadUInt16() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(ushort);
                return *(ushort*)bytes;
            }
        }
        public ushort[] ReadUInt16Array(int count) {
            int offset = count * sizeof(ushort);

            ushort[] array = new ushort[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe uint ReadUInt32() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(uint);
                return *(uint*)bytes;
            }
        }
        public uint[] ReadUInt32Array(int count) {
            int offset = count * sizeof(uint);

            uint[] array = new uint[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe ulong ReadUInt64() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(ulong);
                return *(ulong*)bytes;
            }
        }
        public ulong[] ReadUInt64Array(int count) {
            int offset = count * sizeof(ulong);

            ulong[] array = new ulong[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe float ReadFloat() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(float);
                return *(float*)bytes;
            }
        }
        public float[] ReadFloatArray(int count) {
            int offset = count * sizeof(float);

            float[] array = new float[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe double ReadDouble() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(double);
                return *(double*)bytes;
            }
        }
        public double[] ReadDoubleArray(int count) {
            int offset = count * sizeof(double);

            double[] array = new double[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe decimal ReadDecimal() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(decimal);
                return *(decimal*)bytes;
            }
        }
        public unsafe decimal[] ReadDecimalArray(int count) {
            decimal[] array = new decimal[count];

            fixed (byte* bytes = &Bytes[0]) {
                for (int i = 0; i < count; i++) {
                    array[i] = *(decimal*)&bytes[Index];

                    Index += sizeof(decimal);
                }

                return array;
            }
        }

        public bool ReadBool() {
            return Bytes[Index++] != 0;
        }
        public bool[] ReadBoolArray(int count) {
            bool[] array = new bool[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, count);
            Index += count;

            return array;
        }

        public unsafe char ReadChar() {
            fixed (byte* bytes = &Bytes[Index]) {
                Index += sizeof(char);
                return *(char*)bytes;
            }
        }
        public char[] ReadCharArray(int count) {
            int offset = count * sizeof(char);

            char[] array = new char[count];
            Buffer.BlockCopy(Bytes, Index, array, 0, offset);
            Index += offset;

            return array;
        }

        public unsafe string ReadString() {
            fixed (byte* bytes = &Bytes[0]) {
                int length = *(int*)&bytes[Index];
                Index += sizeof(int);

                char* c = (char*)&bytes[Index];
                Index += sizeof(char) * length;

                return new string(c, 0, length);
            }
        }
        public unsafe string[] ReadStringArray(int count) {
            string[] array = new string[count];

            fixed (byte* bytes = &Bytes[0]) {
                for (int i = 0; i < count; i++) {
                    int length = *(int*)&bytes[Index];
                    Index += sizeof(int);

                    char* c = (char*)&bytes[Index];
                    Index += sizeof(char) * length;

                    array[i] = new string(c, 0, length);
                }
            }

            return array;
        }
    }
}