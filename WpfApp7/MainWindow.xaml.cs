using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp7
{
    public partial class MainWindow : Window
    {
        private string display = "0";
        private string history = "";
        private bool ClearScreen = true;
        private bool decimalInserted = false;
        private string previousResult = ""; 

        public MainWindow()
        {
            InitializeComponent();
            Update();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            string num = (sender as Button).Content.ToString();

            if (display[0] == '0' && num == "0") { return; }
            if (ClearScreen) { display = ""; }

            display += num;
            ClearScreen = false;
            Update();
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClearScreen) { display = "0"; }
            if (decimalInserted) { return; }
            display += (sender as Button).Content;
            decimalInserted = true;
            ClearScreen = false;
            Update();
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (display == "" || display.EndsWith(" ") || history.EndsWith(" ")) { return; } 
            if (!ClearScreen)
            {
                history += display;
                ClearScreen = true;
                decimalInserted = false;
            }
            history += $" {(sender as Button).Content} ";
            Update();
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (history == "") { return; }

            string expression = previousResult != "" ? previousResult + history + display : history + display;

            DataTable table = new DataTable();
            try
            {
                display = table.Compute(expression, null).ToString(); 
                previousResult = display; 
                history = "";
                ClearScreen = true;
                decimalInserted = false;
                Update();
            }
            catch (Exception)
            {
                display = "Error";
                history = "";
                ClearScreen = true;
                decimalInserted = false;
                Update();
            }
        }

        private void FlipNum_Click(object sender, RoutedEventArgs e)
        {
            if (display == "0") { return; }
            float tempNum = -1 * float.Parse(display);
            display = tempNum.ToString();
            Update();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            display = "0";
            ClearScreen = true;
            Update();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            display = "0";
            history = "";
            previousResult = ""; 
            ClearScreen = true;
            Update();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (display == "0") { return; }
            if (display.Length == 1 || (display.Length == 2 && display.Contains("-")))
            {
                display = "0";
                ClearScreen = true;
            }
            else
            {
                display = display.Remove(display.Length - 1, 1);
            }
            Update();
        }

        private void SqaureNum_Click(object sender, RoutedEventArgs e)
        {
            if (display == "0") { return; }
            display = Calculate.Square(display);
            Update();
        }

        private void Update()
        {
            currentNumberTextBox.Text = display;
            previousOperationTextBox.Text = history + previousResult; 
        }

        public class Calculate
        {
            public static string Square(string input)
            {
                float temp = float.Parse(input);
                return (temp * temp).ToString();
            }
        }
    }
}