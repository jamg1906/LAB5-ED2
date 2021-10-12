using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SDESLibrary.Cipher
{
    class CipherSDES
    {
        private string[,] SwapBox1;

        private string[,] SwapBox0;

        private BitArray Key1 { get; set; }
        private BitArray Key2 { get; set; }

        public CipherSDES()
        {
            string[,] strArrays = new string[4, 4];
            strArrays[0, 0] = "00";
            strArrays[0, 1] = "01";
            strArrays[0, 2] = "10";
            strArrays[0, 3] = "11";
            strArrays[1, 0] = "10";
            strArrays[1, 1] = "00";
            strArrays[1, 2] = "01";
            strArrays[1, 3] = "11";
            strArrays[2, 0] = "11";
            strArrays[2, 1] = "00";
            strArrays[2, 2] = "01";
            strArrays[2, 3] = "00";
            strArrays[3, 0] = "10";
            strArrays[3, 1] = "01";
            strArrays[3, 2] = "00";
            strArrays[3, 3] = "11";
            this.SwapBox1 = strArrays;
            string[,] strArrays1 = new string[4, 4];
            strArrays1[0, 0] = "01";
            strArrays1[0, 1] = "00";
            strArrays1[0, 2] = "11";
            strArrays1[0, 3] = "10";
            strArrays1[1, 0] = "11";
            strArrays1[1, 1] = "10";
            strArrays1[1, 2] = "01";
            strArrays1[1, 3] = "00";
            strArrays1[2, 0] = "00";
            strArrays1[2, 1] = "10";
            strArrays1[2, 2] = "01";
            strArrays1[2, 3] = "11";
            strArrays1[3, 0] = "11";
            strArrays1[3, 1] = "01";
            strArrays1[3, 2] = "11";
            strArrays1[3, 3] = "10";
            this.SwapBox0 = strArrays1;
        }


        private byte EncryptByte(byte character)
        {
            BitArray bitArrays = this.InitialPermutation(this.ConvertToBits(character));
            BitArray bitArrays1 = this.CopyTo(bitArrays, 0, 4);
            BitArray bitArrays2 = this.CopyTo(bitArrays, 4, 4);
            BitArray bitArrays3 = this.ExpandAndPermute(bitArrays2);
            BitArray bitArrays4 = bitArrays3.Xor(this.Key1);
            BitArray bitArrays5 = this.CopyTo(bitArrays4, 0, 4);
            BitArray bitArrays6 = this.CopyTo(bitArrays4, 4, 4);
            BitArray swapBox0 = this.GetSwapBox0(bitArrays5);
            BitArray swapBox1 = this.GetSwapBox1(bitArrays6);
            BitArray bitArrays7 = this.Permutation4(this.Concat(swapBox0, swapBox1));
            BitArray bitArrays8 = this.Concat(bitArrays2, bitArrays7.Xor(bitArrays1));
            BitArray bitArrays9 = this.CopyTo(bitArrays8, 0, 4);
            BitArray bitArrays10 = this.CopyTo(bitArrays8, 4, 4);
            BitArray bitArrays11 = this.ExpandAndPermute(bitArrays10);
            BitArray bitArrays12 = bitArrays11.Xor(this.Key2);
            bitArrays5 = this.CopyTo(bitArrays12, 0, 4);
            bitArrays6 = this.CopyTo(bitArrays12, 4, 4);
            swapBox0 = this.GetSwapBox0(bitArrays5);
            swapBox1 = this.GetSwapBox1(bitArrays6);
            bitArrays7 = this.Permutation4(this.Concat(swapBox0, swapBox1));
            BitArray bitArrays13 = this.Concat(bitArrays7.Xor(bitArrays9), bitArrays10);
            return this.ConvertToByte(this.InverseInitialPermutation(bitArrays13));
        }

        public byte[] EncryptText(byte[] content)
        {
            List<byte> nums = new List<byte>((int)content.Length);
            for (int i = 0; i < (int)content.Length; i++)
            {
                nums.Add(this.EncryptByte(content[i]));
            }
            return nums.ToArray();
        }

        private BitArray Concat(BitArray array1, BitArray array2)
        {
            BitArray bitArrays = new BitArray(array1.Length + array2.Length);
            for (int i = 0; i < array1.Length; i++)
            {
                bitArrays[i] = array1[i];
            }
            for (int j = 0; j < array2.Length; j++)
            {
                bitArrays[j + array1.Length] = array2[j];
            }
            return bitArrays;
        }

        public void SetKeys(int private_key)
        {
            BitArray bitArrays = this.ConvertKey(private_key);
            bitArrays = this.Permutation10(bitArrays);
            BitArray bitArrays1 = this.CopyTo(bitArrays, 0, 5);
            BitArray bitArrays2 = this.CopyTo(bitArrays, 5, 5);
            bitArrays1 = this.LeftShift1(bitArrays1);
            bitArrays2 = this.LeftShift1(bitArrays2);
            BitArray bitArrays3 = this.Concat(bitArrays1, bitArrays2);
            this.Key1 = this.Permutation8(bitArrays3);
            bitArrays1 = this.LeftShift2(bitArrays1);
            bitArrays2 = this.LeftShift2(bitArrays2);
            BitArray bitArrays4 = this.Concat(bitArrays1, bitArrays2);
            this.Key2 = this.Permutation8(bitArrays4);
        }

        private BitArray LeftShift1(BitArray input)
        {
            BitArray bitArrays = new BitArray(input.Length);
            bitArrays[bitArrays.Length - 1] = input[0];
            for (int i = 0; i < bitArrays.Length - 1; i++)
            {
                bitArrays[i] = input[i + 1];
            }
            return bitArrays;
        }

        private BitArray Permutation8(BitArray input)
        {
            BitArray bitArrays = new BitArray(8);
            bitArrays[0] = input[2];
            bitArrays[1] = input[4];
            bitArrays[2] = input[6];
            bitArrays[3] = input[9];
            bitArrays[4] = input[0];
            bitArrays[5] = input[5];
            bitArrays[6] = input[7];
            bitArrays[7] = input[1];
            return bitArrays;
        }

        private BitArray LeftShift2(BitArray input)
        {
            BitArray bitArrays = new BitArray(input.Length);
            bitArrays[bitArrays.Length - 2] = input[0];
            bitArrays[bitArrays.Length - 1] = input[1];
            for (int i = 0; i < bitArrays.Length - 2; i++)
            {
                bitArrays[i] = input[i + 2];
            }
            return bitArrays;
        }

        private BitArray Permutation10(BitArray input)
        {
            BitArray bitArrays = new BitArray(10);
            bitArrays[0] = input[4];
            bitArrays[1] = input[5];
            bitArrays[2] = input[2];
            bitArrays[3] = input[0];
            bitArrays[4] = input[8];
            bitArrays[5] = input[9];
            bitArrays[6] = input[1];
            bitArrays[7] = input[3];
            bitArrays[8] = input[7];
            bitArrays[9] = input[6];
            return bitArrays;
        }

        private BitArray ConvertKey(int key)
        {
            BitArray bitArrays = new BitArray(10);
            string str = Convert.ToString(key, 2).PadLeft(10, '0');
            for (int i = 0; i < bitArrays.Length; i++)
            {
                if (str[i] != '1')
                {
                    bitArrays[i] = false;
                }
                else
                {
                    bitArrays[i] = true;
                }
            }
            return bitArrays;
        }

        private BitArray ConvertToBits(byte value)
        {
            BitArray bitArrays = new BitArray(8);
            string str = Convert.ToString(value, 2).PadLeft(8, '0');
            for (int i = 0; i < bitArrays.Length; i++)
            {
                if (str[i] != '1')
                {
                    bitArrays[i] = false;
                }
                else
                {
                    bitArrays[i] = true;
                }
            }
            return bitArrays;
        }

        private byte ConvertToByte(BitArray input)
        {
            string str = "";
            for (int i = 0; i < input.Length; i++)
            {
                str = (!input[i] ? string.Concat(str, "0") : string.Concat(str, "1"));
            }
            return Convert.ToByte(Convert.ToInt32(str, 2));
        }

        private string ConvertToString(BitArray input)
        {
            string str = "";
            for (int i = 0; i < input.Length; i++)
            {
                str = (!input[i] ? string.Concat(str, "0") : string.Concat(str, "1"));
            }
            return str;
        }

        private BitArray CopyTo(BitArray input, int index, int lenght)
        {
            BitArray bitArrays = new BitArray(lenght);
            for (int i = 0; i < lenght; i++)
            {
                bitArrays[i] = input[i + index];
            }
            return bitArrays;
        }

        private BitArray ExpandAndPermute(BitArray input)
        {
            BitArray bitArrays = new BitArray(8);
            bitArrays[0] = input[1];
            bitArrays[1] = input[3];
            bitArrays[2] = input[0];
            bitArrays[3] = input[2];
            bitArrays[4] = input[3];
            bitArrays[5] = input[0];
            bitArrays[6] = input[2];
            bitArrays[7] = input[1];
            return bitArrays;
        }

        private BitArray Permutation4(BitArray input)
        {
            BitArray bitArrays = new BitArray(4);
            bitArrays[0] = input[2];
            bitArrays[1] = input[0];
            bitArrays[2] = input[3];
            bitArrays[3] = input[1];
            return bitArrays;
        }


        private BitArray GetSwapBox0(BitArray input)
        {
            BitArray bitArrays = new BitArray(2);
            bitArrays[0] = input[0];
            bitArrays[1] = input[3];
            BitArray item = new BitArray(2);
            item[0] = input[1];
            item[1] = input[2];
            string str = this.ConvertToString(bitArrays);
            string str1 = this.ConvertToString(item);
            int num = Convert.ToInt32(str, 2);
            int num1 = Convert.ToInt32(str1, 2);
            string swapBox0 = this.SwapBox0[num, num1];
            BitArray bitArrays1 = new BitArray(2);
            for (int i = 0; i < swapBox0.Length; i++)
            {
                if (swapBox0[i] != '1')
                {
                    bitArrays1[i] = false;
                }
                else
                {
                    bitArrays1[i] = true;
                }
            }
            return bitArrays1;
        }

        private BitArray GetSwapBox1(BitArray input)
        {
            BitArray bitArrays = new BitArray(2);
            bitArrays[0] = input[0];
            bitArrays[1] = input[3];
            BitArray item = new BitArray(2);
            item[0] = input[1];
            item[1] = input[2];
            string str = this.ConvertToString(bitArrays);
            string str1 = this.ConvertToString(item);
            int num = Convert.ToInt32(str, 2);
            int num1 = Convert.ToInt32(str1, 2);
            string swapBox1 = this.SwapBox1[num, num1];
            BitArray bitArrays1 = new BitArray(2);
            for (int i = 0; i < swapBox1.Length; i++)
            {
                if (swapBox1[i] != '1')
                {
                    bitArrays1[i] = false;
                }
                else
                {
                    bitArrays1[i] = true;
                }
            }
            return bitArrays1;
        }

        private BitArray InitialPermutation(BitArray input)
        {
            BitArray bitArrays = new BitArray(8);
            bitArrays[0] = input[3];
            bitArrays[1] = input[0];
            bitArrays[2] = input[4];
            bitArrays[3] = input[1];
            bitArrays[4] = input[6];
            bitArrays[5] = input[5];
            bitArrays[6] = input[2];
            bitArrays[7] = input[7];
            return bitArrays;
        }

        private BitArray InverseInitialPermutation(BitArray input)
        {
            BitArray bitArrays = new BitArray(8);
            bitArrays[0] = input[1];
            bitArrays[1] = input[3];
            bitArrays[2] = input[6];
            bitArrays[3] = input[0];
            bitArrays[4] = input[2];
            bitArrays[5] = input[5];
            bitArrays[6] = input[4];
            bitArrays[7] = input[7];
            return bitArrays;
        }

    }
}
