using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace SignalAcquisitionDemo.Styles
{    
    public class RecSwitch : CheckBox
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(RecSwitch), new PropertyMetadata("下行"));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty CheckedTextProperty = DependencyProperty.Register(
            "CheckedText", typeof(string), typeof(RecSwitch), new PropertyMetadata("上行"));
        /// <summary>
        /// 选中状态文本
        /// </summary>
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(RecSwitch), new PropertyMetadata(Brushes.WhiteSmoke));
        /// <summary>
        /// 选中状态前景样式
        /// </summary>
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedBackgroundProperty =
            DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(RecSwitch), new PropertyMetadata(Brushes.LimeGreen));
        /// <summary>
        /// 选中状态背景色
        /// </summary>
        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        static RecSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RecSwitch), new FrameworkPropertyMetadata(typeof(RecSwitch)));
        }

        public RecSwitch()
        {
            this.MouseRightButtonDown += RecSwitch_MouseRightButtonUp;
            this.PreviewMouseLeftButtonDown += RecSwitch_PreviewMouseLeftButtonDownHandler;
        }

        
        //添加右键事件
        private void RecSwitch_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Focusable = !this.Focusable;
            this.Opacity = this.Focusable ? 1 : 0.3;
        }

        private void RecSwitch_PreviewMouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (!this.Focusable)
            {
                e.Handled = true;
            }
        }

    }
}
