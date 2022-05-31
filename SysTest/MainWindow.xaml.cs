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

        private void ClearTests()
        {
            _tp_modified = false;

            ((TreeViewItem)test_tree.Items[0]).Items.Clear();
            ((TreeViewItem)test_tree.Items[1]).Items.Clear();
            ((TreeViewItem)test_tree.Items[2]).Items.Clear();
            ((TreeViewItem)test_tree.Items[3]).Items.Clear();
            ((TreeViewItem)test_tree.Items[4]).Items.Clear();
            _test_plan.Clear();
        }

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

                            ClearTests();
                        }
                        break;
                    case MessageBoxResult.No:
                        ClearTests();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ClearTests();
            }
        }

        private void OnOpenTestPlan_Clicked(object sender, RoutedEventArgs e)
        {
            if (_tp_modified)
            {
                switch (MessageBox.Show("You have unsaved changes to the current test plan.  Would you like to save them before creating a new plan?", "Unsaved Changes", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        SaveFileDialog sfd = new SaveFileDialog();

                        sfd.Filter = "Test Plans (*.tp)|*.tp|All Files (*.*)|*.*";

                        if (sfd.ShowDialog() == true)
                        {
                            _test_plan.Serialize(sfd.FileName);

                            _tp_modified = false;
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                    default:
                        break;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Test Plans (*.tp)|*.tp|All Files (*.*)|*.*";

            if(ofd.ShowDialog() == true)
            {
                if(_test_plan.Deserialize(ofd.FileName))
                {
                    var test_list = _test_plan.Tests();

                    foreach(var kvp in test_list)
                    {
                        var test = kvp.Value;
                        var id = kvp.Key;

                        switch(test.GetType())
                        {
                            case TestType.Icmp:
                                icmp_header.Items.Add(new TreeViewItem()
                                {
                                    Tag = id.ToString(),
                                    Header = test.ToString()
                                });
                                break;
                            case TestType.Dns:
                                dns_header.Items.Add(new TreeViewItem()
                                {
                                    Tag = id.ToString(),
                                    Header = test.ToString()
                                });
                                break;
                            case TestType.Web:
                                web_header.Items.Add(new TreeViewItem()
                                {
                                    Tag = id.ToString(),
                                    Header = test.ToString(),
                                });
                                break;
                            case TestType.TCP:
                                tcp_header.Items.Add(new TreeViewItem()
                                {
                                    Tag = id.ToString(),
                                    Header = test.ToString()
                                });
                                break;
                            case TestType.SVC:
                                svc_header.Items.Add(new TreeViewItem()
                                {
                                    Tag = id.ToString(),
                                    Header = test.ToString()
                                });
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
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
                switch(new_test.GetType())
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
                    case TestType.Web:
                        web_header.Items.Add(new TreeViewItem()
                        {
                            Tag = id.ToString(),
                            Header = new_test.ToString(),
                        });
                        break;
                    case TestType.TCP:
                        tcp_header.Items.Add(new TreeViewItem()
                        {
                            Tag = id.ToString(),
                            Header = id.ToString(),
                        });
                        break;
                    case TestType.SVC:
                        svc_header.Items.Add(new TreeViewItem()
                        {
                            Tag = id.ToString(),
                            Header = id.ToString(),
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnEditTest_Clicked(object sender, RoutedEventArgs e)
        {
            if (test_tree.SelectedItem != null && test_tree?.SelectedValue != null)
            {
                TestEditor te = new TestEditor();

                var test_id = new Guid();
                if(Guid.TryParse(test_tree.SelectedValue.ToString(), out test_id))
                {
                    te.DisableAll();
                    switch (_test_plan.Tests()[test_id].GetType())
                    {
                        case TestType.Icmp:
                            te.EnableTab(TestTab.ICMP);
                            break;
                        case TestType.Dns:
                            te.EnableTab(TestTab.DNS);
                            break;
                        case TestType.Web:
                            te.EnableTab(TestTab.Web);
                            break;
                        case TestType.TCP:
                            te.EnableTab(TestTab.TCP);
                            break;
                        case TestType.SVC:
                            te.EnableTab(TestTab.SVC);
                            break;
                        default:
                            break;
                    }
                    te.LoadTest(_test_plan.Tests()[test_id]);
                    te.Title = "Edit Test...";
                    if (te.ShowDialog() == true)
                    {
                        _test_plan.Tests()[test_id] = te.Result;
                        ((TreeViewItem)test_tree.SelectedItem).Header = te.Result.ToString();
                    }
                }
            }
        }

        private void OnDeleteTest_Clicked(object sender, RoutedEventArgs e)
        {
            if(test_tree.SelectedItem != null && test_tree?.SelectedValue != null)
            {
                var test_id = new Guid();
                if(Guid.TryParse(test_tree.SelectedValue.ToString(), out test_id))
                {
                    _test_plan.Tests().Remove(test_id);
                    var item = (TreeViewItem)test_tree.SelectedItem;
                    (item.Parent as TreeViewItem)?.Items.Remove(item);
                    _tp_modified = true;
                }
            }
        }

        private async void OnRunTestPlan_Clicked(object sender, RoutedEventArgs e)
        {
            _testResults.Clear();
                        
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

        public void OnExportResult_Clicked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
