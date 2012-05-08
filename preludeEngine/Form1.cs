using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            grpRandom.Enabled = false;
        }

        private Boolean bConnected = false;
        private QRNG pqDLL = new QRNG();

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Int32 iRet = 0;

            Memo.Items.Clear();
            if (bConnected == false)
            {
                try
                {
                    if (pqDLL.CheckDLL() == false)
                    {
                        MessageBox.Show("loading library libQRNG.dll failed", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Memo.Items.Add("loading library libQRNG.dll failed");
                        Memo.Items.Add("make sure libQRNG.dll is in the same directory as CsharpQRNG.exe");
                    }
                    else
                    {
                        StringBuilder strUser = new StringBuilder(32);
                        StringBuilder strPass = new StringBuilder(32);

                        strUser.Insert(0, edtUser.Text);
                        strPass.Insert(0, edtPass.Text);
                        if (chkSSL.Checked == false)
                        {
                            iRet = QRNG.qrng_connect(strUser, strPass);
                        }
                        else
                        {
                            iRet = QRNG.qrng_connect_SSL(strUser, strPass);
                        }
                        if (iRet != 0)
                        {
                            try
                            {
                                MessageBox.Show("can't connect: " + QRNG.qrng_error_strings[iRet], "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            catch
                            {
                                MessageBox.Show("can't connect: " + Convert.ToString(iRet), "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            bConnected = true;
                            btnConnect.Text = "Disconnect";
                            grpRandom.Enabled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("loading library libQRNG.dll failed", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Memo.Items.Add("loading library libQRNG.dll failed");
                    Memo.Items.Add("make sure libQRNG.dll is in the same directory as CsharpQRNG.exe");
                }
            }
            else
            {
                QRNG.qrng_disconnect();
                bConnected = false;
                btnConnect.Text = "Connect";
                grpRandom.Enabled = false;
                edtUser.Clear();
                edtPass.Clear();
            }
        }

        private void btnInt_Click(object sender, EventArgs e)
        {
            Int32 iRet;
            Int32 i, iCreatedNumbers, iNumberOfValues;
            Int32[] iArray;
            Stopwatch sw = new Stopwatch();
            iNumberOfValues = Convert.ToInt32(edtNum.Value);
            iArray = new Int32[iNumberOfValues];
            iCreatedNumbers = 0;

            Memo.Items.Clear();
            Memo.Items.Add("get " + Convert.ToString(iNumberOfValues) + " random integer values.");
            sw.Start();
            iRet = QRNG.qrng_get_int_array(ref iArray[0], iNumberOfValues, ref iCreatedNumbers);
            sw.Stop();
            if (iRet != 0)
            {
                try
                {
                    Memo.Items.Add("can't get random numbers: " + QRNG.qrng_error_strings[iRet]);
                }
                catch
                {
                    Memo.Items.Add("can't get random numbers: " + Convert.ToString(iRet));
                }
                return;
            }
            Memo.Items.Add("got " + Convert.ToString(iCreatedNumbers) + " numbers");
            for (i = 0; i < iCreatedNumbers; i++)
            {
                Memo.Items.Add(Convert.ToString(i) + "." + Convert.ToString((char)9) + Convert.ToString(iArray[i]));
            }
            Memo.Items.Add("Finished in " + Convert.ToString(sw.ElapsedMilliseconds) + " ms");
            sw.Reset();
        }

        private void btnFloat_Click(object sender, EventArgs e)
        {
            Int32 iRet;
            Int32 i, iCreatedNumbers, iNumberOfValues;
            Double[] fArray;
            Stopwatch sw = new Stopwatch();
            iNumberOfValues = Convert.ToInt32(edtNum.Value);
            fArray = new Double[iNumberOfValues];
            iCreatedNumbers = 0;

            Memo.Items.Clear();
            Memo.Items.Add("get " + Convert.ToString(iNumberOfValues) + " random float values.");
            sw.Start();
            iRet = QRNG.qrng_get_double_array(ref fArray[0], iNumberOfValues, ref iCreatedNumbers);
            sw.Stop();
            if (iRet != 0)
            {
                try
                {
                    Memo.Items.Add("can't get random numbers: " + QRNG.qrng_error_strings[iRet]);
                }
                catch
                {
                    Memo.Items.Add("can't get random numbers: " + Convert.ToString(iRet));
                }
            }
            Memo.Items.Add("got " + Convert.ToString(iCreatedNumbers) + " numbers");
            for (i = 0; i < iCreatedNumbers; i++)
            {
                Memo.Items.Add(Convert.ToString(i) + "." + Convert.ToString((char)9) + Convert.ToString(fArray[i]));
            }
            Memo.Items.Add("Finished in " + Convert.ToString(sw.ElapsedMilliseconds) + " ms");
            sw.Reset();
        }

        private void btnTesting_Click(object sender, EventArgs e)
        {
            const UInt16 MAXROUNDS = 7;
            bool bTestRunning;
            Int32 iRet;
            Int32 i, j, iNumberOfValues, iCreatedNumbers;
            Double[] fArray;
            Double sum, aSum;
            Int64 aNum;
            Stopwatch sw = new Stopwatch();

            bTestRunning = true;
            Memo.Items.Clear();
            Application.DoEvents();
            Memo.Items.Add("Testing mean Values from 10 to " + Convert.ToString(Math.Pow(10, (MAXROUNDS - 1))));
            iNumberOfValues = Convert.ToInt32(Math.Pow(10, MAXROUNDS));
            fArray = new Double[iNumberOfValues];
            iNumberOfValues = 1;  // Count of Values to catch / test
            aNum = 0; // over all Count of Values catched / tested
            aSum = 0; // Sum over all Values
            // Main loop
            for (i = 1; i < MAXROUNDS; i++)
            {
                iCreatedNumbers = 0;
                iNumberOfValues = iNumberOfValues * 10; // increase count by an order of magnitude
                sw.Start();
                iRet = QRNG.qrng_get_double_array(ref fArray[0], iNumberOfValues, ref iCreatedNumbers);
                sw.Stop();
                if (iRet != 0)
                {
                    try
                    {
                        Memo.Items.Add("can't get random numbers: " + QRNG.qrng_error_strings[iRet]);
                    }
                    catch
                    {
                        Memo.Items.Add("can't get random numbers: " + Convert.ToString(iRet));
                    }
                }
                Memo.Items.Add("got " + Convert.ToString(iCreatedNumbers) + " numbers in " + Convert.ToString(sw.ElapsedMilliseconds) + " ms");
                sum = 0; // calculate sum over all Values
                for (j = 0; j < (iCreatedNumbers - 1); j++)
                {
                    sum = sum + fArray[j];
                    Application.DoEvents();
                    if (!bTestRunning) return;
                }
                aSum = aSum + sum; // add to over all sum
                aNum = aNum + iCreatedNumbers;  // add count to over all count
                // calculate mean
                Memo.Items.Add(Convert.ToString(iCreatedNumbers) + " Values, Mean: " + Convert.ToString(sum / iCreatedNumbers));
                Memo.Items.Add("");

            }
            // Calculate over all mean
            Memo.Items.Add("Finished total: " + Convert.ToString(aNum) + " Values, total Mean: " + Convert.ToString(aSum / aNum));
        }
    }
}
