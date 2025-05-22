using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BinaryFormatV2;

namespace UI_Test {
    class Program {
        [STAThread]
        static void Main(string[] args)
        {
            BObject obj = GetDemoObject();
            BObject obj2 = obj;

            DateTime time = DateTime.Now;
            for (int i = 0; i < 100000; i++) {
                obj2 = BObject.FromBytes(obj.ToBytes());
            }
            Console.WriteLine((DateTime.Now - time).TotalSeconds + " seconds");

            //string output = obj2.ToString();
            //Clipboard.SetText(output);

            //Console.WriteLine(output);
            Console.WriteLine(obj.Equals(obj2));

            Console.ReadLine();
        }

        private static BObject GetDemoObject() {
            BObject obj = new BObject();

            obj.Add("SByte", (sbyte)100);
            obj.Add("Int16", (short)17000);
            obj.Add("Int32", int.MaxValue);
            obj.Add("Int64", long.MaxValue);
            obj.Add("Byte", (byte)255);
            obj.Add("UInt16", (ushort)40000);
            obj.Add("UInt32", uint.MaxValue);
            obj.Add("UInt64", ulong.MaxValue);
            obj.Add("Float", 64564.15F);
            obj.Add("Double", 102.659456548);
            obj.Add("Decimal", decimal.MaxValue);
            obj.Add("Char", 'a');
            obj.Add("String", "TestString");
            obj.Add("Bool", true);

            BObject arrayObj = new BObject();

            BArray array = new string[] { "ABCDE", "ABCDE" };
            BArray jaggedArray = new BArray[] { new string[] { "ABCDE", "ABCDE" }, new string[] { "ABCDE", "ABCDE" } };

            arrayObj.Add("Array", array);
            arrayObj.Add("JaggedArray", jaggedArray);

            BObject objArrayObj = new BObject();
            objArrayObj.Add("Weather", "Sunny");
            objArrayObj.Add("Temperature", 98.5F);
            objArrayObj.Add("Humidity", "65%");

            BArray objArray = new BObject[] { (BObject)objArrayObj.Clone(), (BObject)objArrayObj.Clone() };

            arrayObj.Add("ObjectArray", objArray);

            obj.Add("Object", arrayObj);

            return obj;
        }
        private static BObject GetDemoArrayObject() {
            BObject obj = new BObject();

            obj.Add("SByte", (BArray)new sbyte[5].Populate((sbyte)100));
            obj.Add("Int16", (BArray)new short[5].Populate((short)17000));
            obj.Add("Int32", (BArray)new int[5].Populate(int.MaxValue));
            obj.Add("Int64", (BArray)new long[5].Populate(long.MaxValue));
            obj.Add("Byte", (BArray)new byte[5].Populate((byte)255));
            obj.Add("UInt16", (BArray)new ushort[5].Populate((ushort)40000));
            obj.Add("UInt32", (BArray)new uint[5].Populate(uint.MaxValue));
            obj.Add("UInt64", (BArray)new long[5].Populate(long.MaxValue));
            obj.Add("Float", (BArray)new float[5].Populate(64564.15F));
            obj.Add("Double", (BArray)new double[5].Populate(102.659456548));
            obj.Add("Decimal", (BArray)new decimal[5].Populate(decimal.MaxValue));
            obj.Add("Char", (BArray)new char[5].Populate('a'));
            obj.Add("String", (BArray)new string[5].Populate("TestString"));
            obj.Add("Bool", (BArray)new bool[5].Populate(true));

            BArray jaggedArray = new BArray[] { new string[] { "ABCDE", "ABCDE" }, new string[] { "ABCDE", "ABCDE" } };
            obj.Add("JaggedArray", jaggedArray);

            BObject objArrayObj = new BObject();
            objArrayObj.Add("Weather", "Sunny");
            objArrayObj.Add("Temperature", 98.5F);
            objArrayObj.Add("Humidity", "65%");

            BArray objArray = new BObject[] { (BObject)objArrayObj.Clone(), (BObject)objArrayObj.Clone() };
            obj.Add("ObjectArray", objArray);

            return obj;
        }

        private static BObject GetStringDemoObject() {
            BObject obj = new BObject();

            string text = File.ReadAllText(@"D:\Bush.txt");
            obj.Add("String", text);

            //byte[] bytes = File.ReadAllBytes(@"D:\Bush.txt");
            //obj.Add("Bytes", (BArray)bytes);

            //string[] text = File.ReadAllLines(@"D:\Bush.txt");
            //obj.Add("StringArray", (BArray)text);

            return obj;
        }
    }
}