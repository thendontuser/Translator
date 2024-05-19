using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Lexer;

namespace Translator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PrintLexerResult(List<string> source)
        {
            resultTextBox.Text = string.Empty;
            foreach (string item in source)
            {
                resultTextBox.Text += $"{item}\n";
            }
        }

        private void PrintMatrix(List<string> matrix)
        {
            exprTextBox.Text = string.Empty;
            foreach (string item in matrix)
            {
                exprTextBox.Text += $"{item}\n";
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            string source = sourceTextBox.Text;
            Lexer lexer = new Lexer(source);
            try
            {
                List<string> res = lexer.Analyze();
                PrintLexerResult(res);
                Parser parser = new Parser(lexer.LexemeType, res);
                parser.Program();
                PrintMatrix(parser.Matrix);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sourceTextBox.Text = File.ReadAllText(dialog.FileName);
            }
        }
    }
}