using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

namespace SysTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            result_list.DataContext = _testResults;
        }

        private TestPlan _test_plan = new TestPlan();
        private bool _tp_modified = false;
        private ObservableCollection<TestResult> _testResults = new ObservableCollection<TestResult>();

        private void OnNewTestPlan_Clicked(object sender, RoutedEventArgs e)
        {
            if(_tp_modified)
            {
                switch(MessageBox.Show("You have unsaved changes to the current test plan.  Would you like to save them before creating a new plan?", "Unsaved Changes", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        SaveFileDialog sfd = new SaveFileDialog();

                        sfd.Filter = "Test Plans (*.tp)|*.tp|All Files (*.*)|*.*";

                        if (sfd.ShowDialog() == true)
                        {
                            _test_plan.Serialize(sfd.FileName);

                            _tp_modified = false;

                            test_tree.Items.Clear();
                            _test_plan.Clear();
                        }
                        break;
                    case MessageBoxResult.No:
                        test_tree.Items.Clear();
                        _test_plan.Clear();
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnOpenTestPlan_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnSaveTestPlan_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Test Plans (*.tp)|*.tp|All Files (*.*)|*.*";

            if(sfd.ShowDialog() == true)
            {
                _test_plan.Serialize(sfd.FileName);

                _tp_modified = false;
            }
        }

        private void OnNewTest_Clicked(object sender, RoutedEventArgs e)
        {
            TestEditor te = new TestEditor();
            te.Title = "New Test...";
            if(te.ShowDialog() == true)
            {
                _tp_modified = true;
                var new_test = te.Result;
                var id = _test_plan.AddTest(new_test);
                switch(new_test.Type())
                {
                    case TestType.Icmp:
                        icmp_header.Items.Add(new TreeViewItem()
                        {
                            Tag = id.ToString(),
                            Header = new_test.ToString()
                        });
                        break;
                    case TestType.Dns:
                        dns_header.Items.Add(new TreeViewItem()
                        {
                            Tag = id.ToString(),
                            Header = new_test.ToString()
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnEditTest_Clicked(object sender, RoutedEventArgs e)
        {
            TestEditor te = new TestEditor();
            te.Title = "New Test...";
            te.ShowDialog();
        }

        private void OnDeleteTest_Clicked(object sender, RoutedEventArgs e)
        {
            TestEditor te = new TestEditor();
            te.Title = "New Test...";
            te.ShowDialog();
        }

        private async void OnRunTestPlan_Clicked(object sender, RoutedEventArgs e)
        {
            _testResults.Clear();
            //_test_plan.Clear();

            //for(var i = 0; i < 10; i++)
            //{
            //    _test_plan.AddTest(new Test());
            //}
            
            var opt = new UnboundedChannelOptions();
            opt.SingleReader = true;
            opt.SingleWriter = false;

            var channel = Channel.CreateUnbounded<TestResult>(opt);

            List<Task> threads = new List<Task>();

            foreach (var test in _test_plan.Tests())
            {
                threads.Add(Task.Factory.StartNew(async () =>
                {
                    var result = test.Value.Run();
                    await channel.Writer.WriteAsync(result);
                }));
            }

            await Task.WhenAll(threads);

            channel.Writer.Complete();

            await foreach(var result in channel.Reader.ReadAllAsync())
            {
                _testResults.Add(result);
            }
        }
    }
}
