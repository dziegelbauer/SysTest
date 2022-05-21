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
    public enum TestTab
    {
        ICMP,
        DNS,
        Web,
        TCP,
        SVC
    }
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

        public void OnCancelBtn_Clicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void OnDnsSaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            _result = new DNSTest(dns_test_name.Text, dns_test_target.Text, DNSRecordType.ANY);
            this.DialogResult = true;
        }

        public void DisableAll()
        {
            IcmpTab.IsEnabled = false;
            DnsTab.IsEnabled = false;
            WebTab.IsEnabled = false;
            TCPTab.IsEnabled = false;
            SVCTab.IsEnabled = false;
        }

        public void EnableTab(TestTab tab)
        {
            switch(tab)
            {
                case TestTab.ICMP:
                    IcmpTab.IsEnabled = true;
                    tab_frame.SelectedItem = IcmpTab;
                    break;
                case TestTab.DNS:
                    DnsTab.IsEnabled = true;
                    tab_frame.SelectedItem = DnsTab;
                    break;
                case TestTab.Web:
                    WebTab.IsEnabled = true;
                    tab_frame.SelectedItem = WebTab;
                    break;
                case TestTab.TCP:
                    TCPTab.IsEnabled = true;
                    tab_frame.SelectedItem = TCPTab;
                    break;
                case TestTab.SVC:
                    SVCTab.IsEnabled = true;
                    tab_frame.SelectedItem = SVCTab;
                    break;
            }
        }

        public void LoadTest(Test t)
        {
            var data = t.Serialize();
            switch (t.Type())
            {
                case TestType.Icmp:                    
                    icmp_test_name.Text = data.Name;
                    icmp_test_target.Text = data.Target;
                    break;
                case TestType.Dns:
                    dns_test_name.Text = data.Name;
                    dns_test_target.Text = data.Target;
                    break;
                case TestType.Web:
                    break;
                case TestType.TCP:
                    break;
                case TestType.SVC:
                    break;
            }
        }
    }
}
