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

using MotionAPI;

namespace MotionAPIRegister
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        UInt32 hController = 0;
        UInt32 r = 0;  //返回结果值
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_Click_Setting(object sender, RoutedEventArgs e)
        {
            CMotionAPI.COM_DEVICE comDevice = new CMotionAPI.COM_DEVICE();
            comDevice.ComDeviceType = (UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCI_MODE;
            comDevice.PortNumber    = 1;
            comDevice.CpuNumber     = UInt16.Parse(CPU.Text);
            comDevice.NetworkNumber = 0;
            comDevice.StationNumber = 0;
            comDevice.UnitNumber    = 0;
            comDevice.IPAddress     = "";
            comDevice.Timeout       = 10000;

            r = CMotionAPI.ymcOpenController(ref comDevice, ref hController);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("打开控制器错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }

            r = CMotionAPI.ymcSetAPITimeoutValue(10000);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("设置超时错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
        }

        private void bt_Click_Set(object sender, RoutedEventArgs e)
        {
            UInt32 hRegisterML01000 = 0x00000000;
            UInt32 hRegisterML01002 = 0x00000000;   //注意,L类型寄存器名只能为偶数!!
            UInt32[] dataML = new UInt32[1];
            
            r = CMotionAPI.ymcGetRegisterDataHandle("ML01000", ref hRegisterML01000);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器引用错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }

            r = CMotionAPI.ymcGetRegisterDataHandle("ML01002", ref hRegisterML01002);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器引用错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }

            dataML[0] = UInt32.Parse(S_ML01000.Text);

            r = CMotionAPI.ymcSetRegisterData(hRegisterML01000, 1, dataML);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("设置寄存器值错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }

            dataML[0] = UInt32.Parse(S_ML01002.Text);

            r = CMotionAPI.ymcSetRegisterData(hRegisterML01002, 1, dataML);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("设置寄存器值错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
        }

        private void bt_Click_Get(object sender, RoutedEventArgs e)
        {
            UInt32 hRegisterML01000 = 0x00000000;
            UInt32 hRegisterML01002 = 0x00000000;
            UInt32[] dataML = new UInt32[1];
            UInt32 pReadDataNumber=0;

            r = CMotionAPI.ymcGetRegisterDataHandle("ML01000", ref hRegisterML01000);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器引用错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
            r = CMotionAPI.ymcGetRegisterDataHandle("ML01002", ref hRegisterML01002);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器引用错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }

            CMotionAPI.ymcGetRegisterData(hRegisterML01000, 1, dataML, ref pReadDataNumber);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器值错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
            G_ML01000.Text = dataML[0].ToString();
            CMotionAPI.ymcGetRegisterData(hRegisterML01002, 1, dataML, ref pReadDataNumber);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("获取寄存器值错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
            G_ML01002.Text = dataML[0].ToString();
        }

        private void bt_Click_Exit(object sender, RoutedEventArgs e)
        {
            r = CMotionAPI.ymcCloseController(hController);
            if (r != CMotionAPI.MP_SUCCESS)
            {
                MessageBox.Show(String.Format("关闭控制器错误 \n错误码 [ 0x{0} ]", r.ToString("X")));
                return;
            }
            this.Close();
        }
    }
}
