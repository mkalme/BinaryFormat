using System;
using System.Collections;
using FastStream;

namespace BinaryFormatV2 {
    public class BArray : BTag, IEnumerable {
        internal GenericTagArray _container { get; set; }
        public int Length { get => _container.Length; }
        public Type Type => _container.Type;

        public T[] GetArray<T>(){
            return ((GenericTagArray<T>)_container).Values;
        }

        internal BArray(GenericTagArray container) {
            _container = container;
        }

        public static implicit operator BArray(sbyte[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(short[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(int[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(long[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(byte[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(ushort[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(uint[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(ulong[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(float[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(double[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(decimal[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(char[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(string[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(bool[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(BArray[] values){
            return FromOperator(values);
        }
        public static implicit operator BArray(BObject[] values){
            return FromOperator(values);
        }

        public static explicit operator sbyte[](BArray values){
            return FromArray<sbyte>(values);
        }
        public static explicit operator short[](BArray values){
            return FromArray<short>(values);
        }
        public static explicit operator int[](BArray values){
            return FromArray<int>(values);
        }
        public static explicit operator long[](BArray values){
            return FromArray<long>(values);
        }
        public static explicit operator byte[](BArray values){
            return FromArray<byte>(values);
        }
        public static explicit operator ushort[](BArray values){
            return FromArray<ushort>(values);
        }
        public static explicit operator uint[](BArray values){
            return FromArray<uint>(values);
        }
        public static explicit operator ulong[](BArray values){
            return FromArray<ulong>(values);
        }
        public static explicit operator float[](BArray values){
            return FromArray<float>(values);
        }
        public static explicit operator double[](BArray values){
            return FromArray<double>(values);
        }
        public static explicit operator decimal[](BArray values){
            return FromArray<decimal>(values);
        }
        public static explicit operator char[](BArray values){
            return FromArray<char>(values);
        }
        public static explicit operator string[](BArray values){
            return FromArray<string>(values);
        }
        public static explicit operator bool[](BArray values){
            return FromArray<bool>(values);
        }
        public static explicit operator BArray[](BArray values){
            return FromArray<BArray>(values);
        }
        public static explicit operator BObject[](BArray values){
            return FromArray<BObject>(values);
        }

        private static BArray FromOperator<T>(T[] values){
            return new BArray(new GenericTagArray<T>(values));
        }
        private static T[] FromArray<T>(BArray values) {
            return ((GenericTagArray<T>)values._container).Values;
        }

        //Deserialize
        internal static BArray Deserialize(FastBinaryReader reader) {
            return new BArray(GenericTagArray.Deserialize(reader));
        }

        //Serialize
        internal override void Serialize(FastBinaryWriter writer){
            _container.Serialize(writer);
        }
        internal override void SerializeID(FastBinaryWriter writer){
            writer.Write(GetType().GetTypeID());

            Type type;
            writer.Write((byte)GetDimensions(out type));
            writer.Write(type.GetTypeID());
        }
        public int GetDimensions(out Type type) => _container.GetDimensions(out type);

        //Default methods
        public override string ToString() => _container.ToString();
        public override BTag Clone(){
            return new BArray(_container.Clone());
        }
        public override bool Equals(BTag tag) {
            if (tag.GetType() != typeof(BArray)) return false;

            return _container.Equals(((BArray)tag)._container);
        }

        public IEnumerator GetEnumerator() => _container.GetEnumerator();
    }
}