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

namespace BibleToPPT
{
    /// <summary>
    /// customTemplate.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class customTemplate : Window
    {
        public customTemplate()
        {
            InitializeComponent();
        }

        private void Window_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((Button)sender).Name.ToString());

            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void template_save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("저장되었습니다.");
            Close();
        }

        private void template_cancle(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("취소합니다.");
            Close();
        }
    }
}
