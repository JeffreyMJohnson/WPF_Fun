using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PlayTime
{
    /// <summary>
    /// Interaction logic for MyDialog.xaml
    /// </summary>
    public partial class NewSheetDialog : Window
    {
        public string Width
        {
            get { return width.Text; }
            set { width.Text = value; }
        }

        public string Height
        {
            get { return height.Text; }
            set { height.Text = value; }

        }

        public NewSheetDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
