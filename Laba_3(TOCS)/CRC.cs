using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_3_TOCS_
{
    public class CRC
    {
       

        int r;
        int S;
        int b;

        

        /// <summary>
        /// Construct.
        /// </summary>
        /// <param name="r">Number of errors detected.</param>
        /// <param name="b">Number of errors corrected.</param>
        public CRC(int r = 1, int b = 1)
        {
            this.r = r;
            this.b = b;
            this.S = r;
        }
        public static int QuantityOfPacage(string input)
        {
            int quantity = 0;
            quantity = (int)Math.Ceiling((double)input.Length / 21);

            return quantity;
        }
        public static string ManyPackage(string input, int quantity)
        {
            string newInput = null;
            int length = 0; 
            int startIndex = quantity * 21;

            //int size = input.Length - 21;
            if ((input.Length - startIndex) >= 21)
                newInput = input.Substring(startIndex, 21);
            else
            {
                length = input.Length - startIndex;
                newInput = input.Substring(startIndex, length);
            }
                
            
            return newInput;

        }
        public static byte[] CheckInputLenght(byte[] input)
        {
            int size = input.Length;
            byte[] newByteArray = { 35,35,35,35,35, 35, 35, 35, 35, 35 , 35, 35, 35, 35, 35, 35, 35, 35, 35, 35,35 };
            
            int i = 0;
            while(i < 21)
            {
                
                if(i < size)
                    newByteArray[i] = input[i];
                i++;
               
            }
            return newByteArray;
        }
        private static BitArray GetBasePolyByPower(int k)
        {
            //return ConvertStringToBitArray(new string[] {
            //    "11",
            //    "111",
            //    "1011",
            //    "10011",
            //    "100101",
            //    "100011",
            //    "10001001",
            //    "111100111",
            //    "1000101101",
            //    "111100001011",
            //    "1101110100111"
            //}[k - 1]);
            return ConvertStringToBitArray("111100111");
        }
        public string GetBasePoly(int inputLength)
        {
            int k = GetK(inputLength);
            BitArray basePoly = GetBasePolyByPower(k);
            return ConvertBitArrayToString(basePoly, basePoly.Length);
        }
        public string Encode(string input)
        {
            BitArray bsInput = ConvertStringToBitArray(input);
            BitArray bsResult = Encode(bsInput);
            return ConvertBitArrayToString(bsResult, bsResult.Length);
        }
        public byte[] Encode(byte[] byteInput)
        {
            BitArray bsInput = new BitArray(byteInput);
            BitArray bsResult = Encode(bsInput);
            return ConverBitArrayToByte(bsResult);
        }
        public BitArray Encode(BitArray bsInput)
        {
            //int k = GetK(bsInput.Length);
            //BitArray basePoly = GetBasePolyByPower(k);
            //BitArray bsSource = BitArrayAdd(bsInput, new BitArray(k));
            //BitArray bsRemainder = Div(bsSource, basePoly);
            //BitArray bsResult = BitArrayAdd(bsInput, bsRemainder);
            
            int k = GetK(bsInput.Length);
            BitArray basePoly = GetBasePolyByPower(k);
            BitArray bsSource = BitArrayAdd(bsInput, new BitArray(k));
            BitArray bsRemainder = Div(bsSource, basePoly);
            BitArray bsResult = BitArrayAdd(bsInput, bsRemainder);
            return bsResult;
        }
        public string Decode(string dataTransfer, int inputLength)
        {
            BitArray bsRecived = ConvertStringToBitArray(dataTransfer);
            bsRecived = Decode(bsRecived, inputLength);
            return ConvertBitArrayToString(bsRecived, bsRecived.Length);
        }
        public byte[] Decode(byte[] dataTransfer)
        {
            BitArray bsRecived = new BitArray(dataTransfer);
            bsRecived = Decode(bsRecived, dataTransfer.Length);
            return ConverBitArrayToByte(bsRecived);
        }
        public BitArray Decode(BitArray bsTransfer, int inputLength)
        {
            int k = GetK(inputLength);
            BitArray basePoly = GetBasePolyByPower(k);
            BitArray bsRecived = bsTransfer;
            BitArray check = Div(bsRecived, basePoly);
            int w = GetWeight(check);
            int shiftCount = 0;
            
            while (w > S)
            {
                BitArray bsRecivedOld1 = (BitArray)bsRecived.Clone();
                bsRecived = ShiftLeft(bsRecived);
                check = Div(bsRecived, basePoly);
                w = GetWeight(check);
                shiftCount++;
            }
            bsRecived = Xor(bsRecived, check);
            bsRecived = ShiftRight(bsRecived, shiftCount);
            bsRecived = SubBitArray(bsRecived, 0, inputLength);

            return bsRecived;
        }
        public int GetDmin() => r + 1;
        /// <param name="m">Input length.</param>
        /// <returns></returns>
        public int GetK(int m)
        {
            return (int)Math.Ceiling(Math.Log((double)(int)(m + 1 + Math.Ceiling(Math.Log((double)(m + 1), 2))), 2));
        }
      
        public static string ConvertBitArrayToString(BitArray myList, int myWidth)
        {
            string s = "";
            int i = myWidth;

            foreach (bool obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                }
                i--;
                s += obj ? '1' : '0';
            }
            return s;
        }
        //public byte[] ConvertBitArrayToByte(BitArray btArray)
        //{
        //    byte[] byteArray
        //    return byteArray;

        //}
        
        public static BitArray ConvertStringToBitArray(string s)
        {
            BitArray bs = new BitArray(s.Length);

            for (int i = 0; i < s.Length; i++)
                bs[i] = s[i] == '1' ? true : false;

            return bs;
        }
        public static BitArray ConvertByteToBitArray(byte[] byteInput)
        {
            BitArray bitArray = new BitArray(byteInput);
            return bitArray;
        }
        public static byte[] ConverBitArrayToByte(BitArray bitArray)
        {

            //int numBytes = (bitArray.Length - 1)/ 8 + 1;
            int numBytes = (int)Math.Ceiling((double)bitArray.Length / 8);

            byte[] bytes = new byte[numBytes];
            bitArray.CopyTo(bytes, 0);

            return bytes;

        }
        public static string ConvertByteToString(byte[] bt)
        {
            string s = Encoding.ASCII.GetString(bt);
            return s;
        }
        static BitArray SubBitArray(BitArray bs, int startIndex, int length)
        {
            if (startIndex + length > bs.Length || startIndex <= -1 || length <= -1)
                throw new Exception("Error of lenght");

            BitArray result = new BitArray(length);
            for (int i = startIndex, j = 0; i < startIndex + length; i++, j++)
                result[j] = bs[i];

            return result;
        }
        static BitArray SubBitArray(BitArray bs, int startIndex)
        {
            if (startIndex <= -1)
                throw new Exception("Error of lenght");

            BitArray result = new BitArray(bs.Length - startIndex);
            for (int i = startIndex, j = 0; i < bs.Length; i++, j++)
                result[j] = bs[i];

            return result;
        }
        
        static BitArray BitArrayAdd(BitArray bs, bool element)
        {
            BitArray result = new BitArray(bs.Length + 1);
            for (int i = 0; i < bs.Length; i++)
                result[i] = bs[i];
            result[bs.Length] = element;

            return result;
        }
        static int GetWeight(BitArray bs)
        {
            int w = 0;
            foreach (bool b in bs)
                if (b)
                    w++;
            return w;
        }
        static BitArray BitArrayAdd(BitArray bs, BitArray bs2)
        {
            int resultLength = bs.Length + bs2.Length;
            BitArray result = new BitArray(resultLength);
            for (int i = 0; i < bs.Length; i++)
                result[i] = bs[i];
            for (int i = bs.Length, j = 0; i < resultLength; i++, j++)
                result[i] = bs2[j];
            return result;
        }
        static BitArray Xor(BitArray bs1, BitArray bs2)
        {
            if (bs1.Length > bs2.Length)
            {
                BitArray result = new BitArray(bs1.Length);
                for (int i = 0; i < bs1.Length - bs2.Length; i++)
                    result[i] = bs1[i];
                for (int i = bs1.Length - bs2.Length, j = 0; i < bs1.Length; i++, j++)
                    result[i] = bs1[i] ^ bs2[j];
                return result;
            }
            else if (bs1.Length < bs2.Length)
            {
                BitArray temp = bs2;
                bs2 = bs1;
                bs1 = temp;
                return Xor(bs1, bs2);
            }
            else
                return bs1.Xor(bs2);
        }
        static BitArray ShiftLeft(BitArray bs)
        {
            BitArray result = (BitArray)bs.Clone();
            bool temp = result[0];
            for (int i = 0; i < result.Length - 1; i++)
                result[i] = result[i + 1];
            result[result.Length - 1] = temp;
            return result;
        }
        static BitArray ShiftRight(BitArray bs)
        {
            BitArray result = (BitArray)bs.Clone();
            if (result.Length > 1)
            {
                var tmp = result[result.Length - 1];
                for (var i = result.Length - 1; i != 0; --i)
                    result[i] = result[i - 1];
                result[0] = tmp;
            }
            return result;
        }
        static BitArray ShiftRight(BitArray bs, int count)
        {
            BitArray result = (BitArray)bs.Clone();
            for (int i = 0; i < count; i++)
                result = ShiftRight(result);
            return result;
        }
    
        static BitArray Div(BitArray bs1, BitArray bs2)
        {
            BitArray bsSource = bs1;
            int sourceLength = bs1.Length;


            BitArray bsBy = bs2;
            int byLength = bsBy.Length;
            int i = 0;
            //int bsByDec = ConvertBitArrayToInt32(bs2);

            BitArray temp = SubBitArray(bsSource, i, byLength);

            while (true)
            {
                if (temp[0])
                {
                    temp = temp.Xor(bsBy);
                }
                do
                {
                    i++;
                    temp = SubBitArray(temp, 1);
                    if (i + byLength - 1 < sourceLength)
                        temp = BitArrayAdd(temp, bsSource[i + byLength - 1]);
                    else
                        return temp;
                } while (!temp[0]);
            }
        }
    }
}
