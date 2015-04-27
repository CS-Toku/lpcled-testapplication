using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace TestDisplay
{
    public partial class Form1 : Form
    {
        public SerialPort display;

        public Form1()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            comboBox1.Refresh();
            comboBox1.Text = "";
            comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;

            textBox1.MaxLength = 16;
            textBox2.MaxLength = 16;

            display = null;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;
            display = new SerialPort(comboBox1.SelectedItem.ToString(), 115200, Parity.None, 8, StopBits.One);
            try
            {
                display.Open();
                button1.Enabled = false;
                button2.Enabled = true;
                comboBox1.Enabled = false;
                label3.Text = comboBox1.SelectedItem.ToString();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString(), "Error!!");
                display.Dispose();
                display = null;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            display.Close();
            button1.Enabled = true;
            button2.Enabled = false;
            display = null;
            comboBox1.Enabled = true;
            label3.Text = "16文字まで";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            comboBox1.Refresh();
            comboBox1.Text = "";
            comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] up = new byte[] {(byte)'\a'};
            byte[] down = new byte[] {(byte)'\n'};
            byte[] clear = new byte[] {(byte)'\f'};
            byte[] line1 = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(textBox1.Text);
            byte[] line2 = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(textBox2.Text);
            display.Write(clear, 0, clear.Length);
            display.Write(up, 0, up.Length);
            display.Write(line1, 0, line1.Length);
            display.Write(down, 0, down.Length);
            display.Write(line2, 0, line2.Length);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (display != null)
                display.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && display != null)
            {
                if (display.IsOpen)
                {
                    byte[] up = new byte[] { (byte)'\a' };
                    byte[] space = new byte[] { (byte)' ' };
                    byte[] line1 = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(textBox1.Text);
                    display.Write(up, 0, up.Length);
                    display.Write(line1, 0, line1.Length);
                    for (int i = 16 - line1.Length; i != 0; i--)
                    {
                        display.Write(space, 0, 1);
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && display != null)
            {
                if (display.IsOpen)
                {
                    byte[] down = new byte[] { (byte)'\n' };
                    byte[] space = new byte[] { (byte)' ' };
                    byte[] line2 = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(textBox2.Text);
                    display.Write(down, 0, down.Length);
                    display.Write(line2, 0, line2.Length);
                    for (int i = 16 - line2.Length; i != 0; i--)
                    {
                        display.Write(space, 0, 1);
                    }
                }
            }
        }

    }
}
