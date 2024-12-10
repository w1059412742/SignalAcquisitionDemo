using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SignalAcquisitionDemo.Styles
{    
    public class BulletButton : CheckBox
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(BulletButton), new PropertyMetadata("Off"));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty CheckedTextProperty = DependencyProperty.Register(
            "CheckedText", typeof(string), typeof(BulletButton), new PropertyMetadata("On"));
        /// <summary>
        /// 选中状态文本
        /// </summary>
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(BulletButton), new PropertyMetadata(Brushes.WhiteSmoke));
        /// <summary>
        /// 选中状态前景样式
        /// </summary>
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedBackgroundProperty =
            DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(BulletButton), new PropertyMetadata(Brushes.LimeGreen));
        /// <summary>
        /// 选中状态背景色
        /// </summary>
        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        static BulletButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BulletButton), new FrameworkPropertyMetadata(typeof(BulletButton)));
        }

        public BulletButton()
        {
            //this.MouseRightButtonDown += BulletButton_MouseRightButtonUp;
        }

        //添加右键事件
        private void BulletButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //this.Focusable = !this.Focusable;
        }
        //重写OnClick事件
        protected override void OnClick()
        {
            //if (this.Focusable)
            //{
            //    this.IsChecked = !this.IsChecked;
            //}
            base.OnClick();
        }
    }
}
