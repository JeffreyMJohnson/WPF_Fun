using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace dataBinding
{
    /// <summary>
    /// Interaction logic for WpfTreeViewInPlaceEditControl.xaml
    /// </summary>
    public partial class WpfTreeViewInPlaceEditControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool isInEditMode = false;
        public bool IsInEditMode
        {
            get { return isInEditMode; }
            set 
            { 
                isInEditMode = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("IsInEditMode"));
                }
            }
        }

        public WpfTreeViewInPlaceEditControl()
        {
            InitializeComponent();
        }

        string oldText;

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
                IsInEditMode = true;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IsInEditMode = false;
        }

        private void textBlockHeaderSelected_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsInEditMode = true;
            e.Handled = true;
        }

        private void editableTextBoxHeader_LostFocus(object sender, RoutedEventArgs e)
        {

            IsInEditMode = false;
        }

        private void editableTextBoxHeader_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.IsVisible)
            {
                tb.Focus();
                tb.SelectAll();
                oldText = tb.Text;
            }
        }

        private void editableTextBoxHeader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                IsInEditMode = false;
            if(e.Key == Key.Escape)
            {
                var tb = sender as TextBox;
                tb.Text = oldText;
                IsInEditMode = false;
            }
        }
    }
}
