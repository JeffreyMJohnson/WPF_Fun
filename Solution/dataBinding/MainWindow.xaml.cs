using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace dataBinding
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
    }

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if(!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }

    class TreeViewParentItem : NotifyPropertyChanged
    {
        //name for the parent item
        string name;
        public string Name 
        {
            get { return name; }
            set { SetField(ref name, value); }
        }

        public ObservableCollection<string> TreeViewChildrenItems { get; set; }

        public TreeViewParentItem(string name)
        {
            Name = name;
            TreeViewChildrenItems = new ObservableCollection<string>();
            TreeViewChildrenItems.Add("first child");
            TreeViewChildrenItems.Add("second child");
        }
    }

    class TreeViewDocument : ObservableCollection<TreeViewParentItem>
    {
        public TreeViewDocument()
        {
            Add(new TreeViewParentItem("First Parent Item"));
            Add(new TreeViewParentItem("Second Parent Item"));
            Add(new TreeViewParentItem("Third Parent Item"));
        }
    }
}
