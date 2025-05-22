using System;
using System.Collections;
using FastStream;

namespace BinaryFormatV2 {
    abstract class GenericTagArray : IEnumerable {
        public abstract Type Type { get; }
        public abstract int Length { get; }

        //Deserialize
        public static GenericTagArray Deserialize(FastBinaryReader reader){
            int depth = reader.ReadByte();
            Type type = BinaryHelper.GetTypeFromID(reader.ReadByte());

            return GetTags(reader, type, depth);
        }
        private static GenericTagArray GetTags(FastBinaryReader reader, Type type, int depth){
            int length = reader.ReadInt32();

            GenericTagArray iArray;

            if (depth > 1) {
                BArray[] array = new BArray[length];
                for (int j = 0; j < length; j++) {
                    array[j] = new BArray(GetTags(reader, type, depth - 1));
                }

                iArray = new GenericTagArray<BArray>(array);
            } else {
                iArray = ConvertByteArray(reader, length, type);
            }

            return iArray;
        }

        private static GenericTagArray ConvertByteArray(FastBinaryReader reader, int length, Type type){
            switch (Type.GetTypeCode(type)) {
                case TypeCode.SByte:
                    return FromArray(ConvertToArray<sbyte>(reader, length));
                case TypeCode.Int16:
                    return FromArray(ConvertToArray<short>(reader, length));
                case TypeCode.Int32:
                    return FromArray(ConvertToArray<int>(reader, length));
                case TypeCode.Int64:
                    return FromArray(ConvertToArray<long>(reader, length));
                case TypeCode.Byte:
                    return FromArray(ConvertToArray<byte>(reader, length));
                case TypeCode.UInt16:
                    return FromArray(ConvertToArray<ushort>(reader, length));
                case TypeCode.UInt32:
                    return FromArray(ConvertToArray<uint>(reader, length));
                case TypeCode.UInt64:
                    return FromArray(ConvertToArray<ulong>(reader, length));
                case TypeCode.Single:
                    return FromArray(ConvertToArray<float>(reader, length));
                case TypeCode.Double:
                    return FromArray(ConvertToArray<double>(reader, length));
                case TypeCode.Decimal:
                    return FromArray(ConvertToArray<decimal>(reader, length));
                case TypeCode.Char:
                    return FromArray(ConvertToArray<char>(reader, length));
                case TypeCode.String:
                    return FromArray(ConvertToArray<string>(reader, length));
                case TypeCode.Boolean:
                    return FromArray(ConvertToArray<bool>(reader, length));
            }

            if (type == typeof(BObject)) {
                BObject[] array = new BObject[length];
                for (int j = 0; j < length; j++) {
                    array[j] = BObject.Deserialize(reader);
                }
                return new GenericTagArray<BObject>(array);
            }

            return null;
        }
        private static U[] ConvertToArray<U>(FastBinaryReader reader, int count){
            switch (Type.GetTypeCode(typeof(U))) {
                case TypeCode.SByte:
                    return (U[])(Array)reader.ReadSBytes(count);
                case TypeCode.Int16:
                    return (U[])(Array)reader.ReadInt16Array(count);
                case TypeCode.Int32:
                    return (U[])(Array)reader.ReadInt32Array(count);
                case TypeCode.Int64:
                    return (U[])(Array)reader.ReadInt64Array(count);
                case TypeCode.Byte:
                    return (U[])(Array)reader.ReadBytes(count);
                case TypeCode.UInt16:
                    return (U[])(Array)reader.ReadUInt16Array(count);
                case TypeCode.UInt32:
                    return (U[])(Array)reader.ReadUInt32Array(count);
                case TypeCode.UInt64:
                    return (U[])(Array)reader.ReadUInt64Array(count);
                case TypeCode.Single:
                    return (U[])(Array)reader.ReadFloatArray(count);
                case TypeCode.Double:
                    return (U[])(Array)reader.ReadDoubleArray(count);
                case TypeCode.Decimal:
                    return (U[])(Array)reader.ReadDecimalArray(count);
                case TypeCode.Char:
                    return (U[])(Array)reader.ReadCharArray(count);
                case TypeCode.String:
                    return (U[])(Array)reader.ReadStringArray(count);
                case TypeCode.Boolean:
                    return (U[])(Array)reader.ReadBoolArray(count);
            }

            return new U[0];
        }
        private static GenericTagArray FromArray<U>(U[] array){
            return new GenericTagArray<U>(array);
        }

        public abstract int GetDimensions(out Type type);

        //Serialize
        public abstract void Serialize(FastBinaryWriter writer);

        //Default methods
        public abstract override string ToString();
        public abstract GenericTagArray Clone();
        public abstract bool Equals(GenericTagArray tag);

        public abstract IEnumerator GetEnumerator();
    }

    class GenericTagArray<T> : GenericTagArray {
        public T[] Values { get; set; }
        public override int Length { get => Values.Length; }
        public override Type Type => typeof(T);

        public GenericTagArray(T[] array){
            Values = array;
        }

        //Serialize
        public override void Serialize(FastBinaryWriter writer){
            writer.Write(Values.Length);

            switch (Type.GetTypeCode(Type)) {
                case TypeCode.String:
                    writer.Write(Values.To<string>());
                    return;
                case TypeCode.Decimal:
                    writer.Write(Values.To<decimal>());
                    return;
                case TypeCode.SByte:
                    writer.Write(Values.To<byte>());
                    return;
                case TypeCode.Byte:
                    writer.Write(Values.To<byte>());
                    return;
            }

            if (Type == typeof(BArray)) {
                BArray[] arr = Values.To<BArray>();
                for (int i = 0; i < arr.Length; i++) {
                    arr[i].Serialize(writer);
                }
                return;
            } else if (Type == typeof(BObject)) {
                BObject[] arr = Values.To<BObject>();
                for (int i = 0; i < arr.Length; i++) {
                    arr[i].Serialize(writer);
                }
                return;
            }

            byte[] array = new byte[Length * BinaryHelper.Size[typeof(T)]];
            Buffer.BlockCopy(Values, 0, array, 0, array.Length);
            writer.Write(array);
        }
        public override int GetDimensions(out Type type){
            if (Type == typeof(BArray)) return Values.To<BArray>()[0].GetDimensions(out type) + 1;

            type = Type;
            return 1;
        }

        //Default methods
        public override string ToString(){
            string output = "[\n";

            string body;
            string[] children = new string[Values.Length];

            if (Type == typeof(BArray)) {
                BArray[] array = Values.To<BArray>();
                for (int i = 0; i < array.Length; i++) {
                    children[i] = array[i].ToString();
                }
            } else if (Type == typeof(BObject)) {
                BObject[] array = Values.To<BObject>();
                for (int i = 0; i < array.Length; i++) {
                    children[i] = array[i].ToString();
                }
            } else {
                for (int i = 0; i < Values.Length; i++) {
                    children[i] = Values[i].ConvertToString();
                }
            }

            body = String.Join(",\n", children);

            output += body.Tab(1) + "\n]";

            return output;
        }
        public override GenericTagArray Clone(){
            if (Type == typeof(BArray)) {
                BArray[] a = Values.To<BArray>();
                BArray[] tagArray = new BArray[a.Length];
                for (int i = 0; i < tagArray.Length; i++) {
                    tagArray[i] = (BArray)a[i].Clone();
                }
                return new GenericTagArray<BArray>(tagArray);
            } else if (Type == typeof(BObject)) {
                BObject[] a = Values.To<BObject>();
                BObject[] objectArray = new BObject[a.Length];
                for (int i = 0; i < objectArray.Length; i++) {
                    objectArray[i] = (BObject)a[i].Clone();
                }
                return new GenericTagArray<BObject>(objectArray);
            }

            T[] array = new T[Values.Length];
            for (int i = 0; i < array.Length; i++) {
                array[i] = Values[i];
            }

            return new GenericTagArray<T>(array);
        }
        public override bool Equals(GenericTagArray tag){
            if (tag.GetType() != typeof(GenericTagArray<T>)) return false;

            T[] array = ((GenericTagArray<T>)tag).Values;
            if (Values.Length != array.Length) return false;

            if (Type == typeof(BArray)) {
                BArray[] a = Values.To<BArray>();
                BArray[] b = array.To<BArray>();
                for (int i = 0; i < a.Length; i++) {
                    if (!a[i].Equals(b[i])) return false;
                }
            } else if (Type == typeof(BObject)) {
                BObject[] a = Values.To<BObject>();
                BObject[] b = array.To<BObject>();
                for (int i = 0; i < a.Length; i++) {
                    if (!a[i].Equals(b[i])) return false;
                }
            } else {
                for (int i = 0; i < Values.Length; i++) {
                    if (!Values[i].Equals(array[i])) return false;
                }
            }

            return true;
        }

        public override IEnumerator GetEnumerator() => Values.GetEnumerator();
    }
}