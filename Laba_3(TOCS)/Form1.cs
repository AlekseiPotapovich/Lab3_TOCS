using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_3_TOCS_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //if (richTextBox1.Text.Length <= 21)
            //    button1.Enabled = true;
            //else
            //    button1.Enabled = false;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string input1 = richTextBox1.Text;
            string[] masInput = input1.Split('\n');
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            for (int index = 0; index < masInput.Length; index++)
            {
                string input = masInput[index];
                int quntity = CRC.QuantityOfPacage(input);
                
                int newQuntity = 0;
                int size = 176, length = (input.Length * 8);
                int size1, lenght2 = input.Length;
                while (quntity != 0)
                {
                    CRC crcInput = new CRC();

                    byte[] byteInput = Encoding.ASCII.GetBytes(CRC.ManyPackage(input, newQuntity));
                    byteInput = CRC.CheckInputLenght(byteInput);
                    BitArray bitArrayInput = CRC.ConvertByteToBitArray(byteInput);
                    string polynom = crcInput.GetBasePoly(bitArrayInput.Length);
                    BitArray dataBitArray = crcInput.Encode(bitArrayInput);

                    richTextBox3.Text += "Input = ";


                    richTextBox3.Text += Encoding.ASCII.GetString(byteInput) + " - ";
                    //richTextBox3.Text += Environment.NewLine;
                    //richTextBox3.Text += "Input in binary = ";
                    //for (int i = 0; i < byteInput.Length; i++)
                    //{
                    //    if (bitArrayInput[i])
                    //        richTextBox3.Text += 1;
                    //    else
                    //        richTextBox3.Text += 0;
                    //}
                    //richTextBox3.Text += byteInput.;
                    //richTextBox3.Text += Environment.NewLine;
                    richTextBox3.Text += " Polynom = ";
                    richTextBox3.Text += polynom + " ";
                    //richTextBox3.Text += Environment.NewLine;
                    richTextBox3.Text += "CRC = ";
                    string endOfCRC = "";
                    string crc = Encoding.ASCII.GetString(CRC.ConverBitArrayToByte(dataBitArray));
                    crc = crc.Substring(0, 21);
                    
                    for (int index1 = 168; index1 < dataBitArray.Length; index1++)
                        if(dataBitArray[index1] == true)
                        {
                            endOfCRC += "1";
                        }
                        else
                        {
                            endOfCRC += "0";
                        }
                    richTextBox3.Text += crc + endOfCRC +Environment.NewLine;
                    //richTextBox3.Text += Environment.NewLine;

                    CRC crcOutput = new CRC();
                    //byte[] outputByteArray = CRC.ConverBitArrayToByte(dataTransfer);
                    //BitArray outputBitArray = CRC.ConvertByteToBitArray(outputByteArray);
                    //CRC.ConverBitArrayToByte(crcOutput.Decode(outputBitArray, input.Length * 8));


                    if (length <= 176)
                        size = length;
                    else
                        length -= size;

                    BitArray bitArrayOutput = crcOutput.Decode(dataBitArray, (byteInput.Length * 8));
                    string output = Encoding.ASCII.GetString(CRC.ConverBitArrayToByte(bitArrayOutput));

                    if (lenght2 >= 21)
                    {
                        size1 = 21;
                        lenght2 -= 21;
                    }

                    else
                    {
                        size1 = lenght2;

                    }

                    output = output.Substring(0, size1);
                    richTextBox2.Text += output;

                    if (masInput.Length > 1)
                        richTextBox2.Text += Environment.NewLine;


                    quntity--;
                    newQuntity++;
                }
            }
        }
    }
}
