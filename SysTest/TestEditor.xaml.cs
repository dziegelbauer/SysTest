using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            DNSRecordType rt = DNSRecordType.ANY;
            if (dns_test_a.IsChecked == true)
                rt = DNSRecordType.A;
            if(dns_test_aaaa.IsChecked == true)
                rt = DNSRecordType.AAAA;

            _result = new DNSTest(dns_test_name.Text, dns_test_target.Text, rt, dns_server_name.Text);
            this.DialogResult = true;
        }

        public void OnWebSaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            uint rcode = 0;
            if(!uint.TryParse(web_test_response.Text, out rcode))
            {
                MessageBox.Show("Code is not a valid http response code.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            Uri? uri;
            UriCreationOptions opts = new UriCreationOptions();
            if (!Uri.TryCreate(web_test_url.Text, opts, out uri))
            {
                MessageBox.Show("URL is not a valid http/https URL.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            _result = new WebTest(web_test_name.Text, web_test_url.Text, uint.Parse(web_test_response.Text));
            this.DialogResult = true;
        }

        public void OnTcpSaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            ushort p;
            if (!ushort.TryParse(tcp_test_port.Text, out p))
            {
                MessageBox.Show("Invlaid port value", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                return;
            }

            _result = new TCPTest(tcp_test_name.Text, tcp_test_target.Text, p);
            this.DialogResult = true;
        }

        public void OnSvcSaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            _result = new SVCTest(svc_test_name.Text, svc_test_target.Text, svc_test_svcname.Text);
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
            switch (t.GetType())
            {
                case TestType.Icmp:                    
                    icmp_test_name.Text = data.Name;
                    icmp_test_target.Text = data.Target;
                    break;
                case TestType.Dns:
                    dns_test_name.Text = data.Name;
                    dns_test_target.Text = data.Target;
                    dns_server_name.Text = data.DNSServer;
                    break;
                case TestType.Web:
                    break;
                case TestType.TCP:
                    tcp_test_name.Text = data.Name;
                    tcp_test_target.Text = data.Target;
                    tcp_test_port.Text = data.Port.ToString();
                    break;
                case TestType.SVC:
                    svc_test_name.Text = data.Name;
                    svc_test_target.Text = data.Target;
                    svc_test_svcname.Text = data.SvcName;
                    break;
            }
        }
    }
}
