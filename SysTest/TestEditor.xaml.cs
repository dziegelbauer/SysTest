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

namespace SysTest
{
    /// <summary>
    /// Interaction logic for TestEditor.xaml
    /// </summary>
    public partial class TestEditor : Window
    {
        Test _result;

        public Test Result
        {
            get { return _result; }
        }
        public TestEditor()
        {
            InitializeComponent();
            _result = new Test();
        }

        public void OnIcmpSaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            _result = new ICMPTest(icmp_test_name.Text, icmp_test_target.Text);
            this.DialogResult = true;
        }
    }
}
