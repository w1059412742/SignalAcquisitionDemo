using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalAcquisitionDemo.Styles
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StatusLight"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StatusLight;assembly=StatusLight"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class StatusLight : ButtonBase
    {
        #region 依赖属性定义

        static StatusLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusLight), new FrameworkPropertyMetadata(typeof(StatusLight)));
        }


        public Brush FalseBrush
        {
            get { return (Brush)GetValue(FalseBrushProperty); }
            set { SetValue(FalseBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FalseBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FalseBrushProperty =
            DependencyProperty.Register("FalseBrush", typeof(Brush), typeof(StatusLight), new PropertyMetadata(Brushes.Red));//new BrushConverter().ConvertFromInvariantString("#FF2D7024")));//Brushes.DarkGreen));//"#FF2D7024"));



        public Brush TrueBrush
        {
            get { return (Brush)GetValue(TrueBrushProperty); }
            set { SetValue(TrueBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrueBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrueBrushProperty =
            DependencyProperty.Register("TrueBrush", typeof(Brush), typeof(StatusLight), new PropertyMetadata(new BrushConverter().ConvertFromInvariantString("#FF1BD600")));//Brushes.LimeGreen));//"#FF1BD600"));



        public Visibility ContentVisbility
        {
            get { return (Visibility)GetValue(ContentVisbilityProperty); }
            set { SetValue(ContentVisbilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentVisbility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentVisbilityProperty =
            DependencyProperty.Register("ContentVisbility", typeof(Visibility), typeof(StatusLight), new PropertyMetadata(Visibility.Collapsed));



        public object FalseContent
        {
            get { return (object)GetValue(FalseContentProperty); }
            set { SetValue(FalseContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FalseContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FalseContentProperty =
            DependencyProperty.Register("FalseContent", typeof(object), typeof(StatusLight), new PropertyMetadata(new object()));



        public object TrueContent
        {
            get { return (object)GetValue(TrueContentProperty); }
            set { SetValue(TrueContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrueContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrueContentProperty =
            DependencyProperty.Register("TrueContent", typeof(object), typeof(StatusLight), new PropertyMetadata(new object()));



        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { OnPreviewValueChanged(Value, value); SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool), typeof(StatusLight), new PropertyMetadata(false, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StatusLight sl = d as StatusLight;
            if (sl != null)
            {
                sl.OnValueChanged((bool)e.OldValue, (bool)e.NewValue);
            }
        }


        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(StatusLight), new PropertyMetadata(true));



        public bool IsMomentary
        {
            get { return (bool)GetValue(IsMomentaryProperty); }
            set { SetValue(IsMomentaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMomentary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMomentaryProperty =
            DependencyProperty.Register("IsMomentary", typeof(bool), typeof(StatusLight), new PropertyMetadata(false));



        #endregion

        #region 事件重写
        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (!IsReadOnly)
            {
                if (ClickMode == ClickMode.Release && (IsMomentary || !IsPressed)
                    || ClickMode == ClickMode.Press && (!IsMomentary && IsPressed || IsMomentary && !IsPressed))//此处与逻辑不太符合，但是为了和NI的LED的动作一样。
                {
                    Value = !Value;
                }
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsReadOnly && ClickMode == ClickMode.Hover)
            {
                Value = !Value;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsReadOnly && ClickMode == ClickMode.Hover && IsMomentary)
            {
                Value = !Value;
            }
        }
        #endregion

        #region 自定义事件
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged",
             RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(StatusLight));

        [Description("值被更新后发生")]
        public event RoutedPropertyChangedEventHandler<bool> ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        protected virtual void OnValueChanged(bool oldValue, bool newValue)
        {
            RoutedPropertyChangedEventArgs<bool> arg =
                new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, ValueChangedEvent);
            this.RaiseEvent(arg);
        }


        public static readonly RoutedEvent PreviewValueChangedEvent =
            EventManager.RegisterRoutedEvent("PreviewValueChanged",
             RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(StatusLight));

        [Description("值被更新后发生")]
        public event RoutedPropertyChangedEventHandler<bool> PreviewValueChanged
        {
            add
            {
                this.AddHandler(PreviewValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(PreviewValueChangedEvent, value);
            }
        }

        protected virtual void OnPreviewValueChanged(bool oldValue, bool newValue)
        {
            RoutedPropertyChangedEventArgs<bool> arg =
                new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, PreviewValueChangedEvent);
            this.RaiseEvent(arg);
        }

        #endregion
    }
}
