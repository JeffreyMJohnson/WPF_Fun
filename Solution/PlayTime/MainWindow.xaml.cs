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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace PlayTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProcessFileOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image Files|*.png;*.bmp;*.jpg";

            //show the box
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = dialog.FileNames;
                string text = "";

                for (int i = 0; i < fileNames.Length; i++)
                {
                    text += fileNames[i] + "\n";
                }
                txtBox.Text = text;
            }
        }

        private void mWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                mWindow.Close();
            }
        }
    }
}
