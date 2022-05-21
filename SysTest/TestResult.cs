using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SysTest
{
    public class TestResult
    {
        public string description = "";
        public string name = "";
        public bool Success = false;

        public ImageSource Image
        {
            get
            {
                String imgSrc = this.Success ? @"/SysTest;component/Images/StatusOK.png" : @"/SysTest;component/Images/StatusError.png";
                return new BitmapImage(new Uri(imgSrc, UriKind.RelativeOrAbsolute));
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Description
        {
            get { return this.description; }
        }
    }
}
