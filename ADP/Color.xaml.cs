using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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
using MediaColor = System.Windows.Media.Color;

namespace ADP
{
    /// <summary>
    /// Логика взаимодействия для Color.xaml
    /// </summary>
    public partial class Color : Window
    {
        ViewModel _vm;
        Brush[] _brushes = new Brush[10];
        Dictionary<int, Button> _colorLine_But;
        Button _butForCommon;
        private Brush[] _brusheForCommonBut = new Brush[1];
        int _par_num, num1Color, num2Color, num3Color, tag;
        bool _commonColorBut, Ok_button=false;
        int[] _num1 = new int[10];
        int[] _num2 = new int[10];
        int[] _num3 = new int[10];
        private static readonly MediaColor color_C00E0E = (MediaColor)ColorConverter.ConvertFromString("#C00E0E");
        private static readonly SKColor SKcolor_C00E0E = SKColor.Parse("#C00E0E");
        private static readonly MediaColor color_543434 = (MediaColor)ColorConverter.ConvertFromString("#543434");
        private static readonly SKColor SKcolor_543434 = SKColor.Parse("#543434");
        private static readonly MediaColor color_A74301 = (MediaColor)ColorConverter.ConvertFromString("#A74301");
        private static readonly SKColor SKcolor_A74301 = SKColor.Parse("#A74301");
        private static readonly MediaColor color_114A97 = (MediaColor)ColorConverter.ConvertFromString("#114A97");
        private static readonly SKColor SKcolor_114A97 = SKColor.Parse("#114A97");
        private Brush[] colorBut = new Brush[] { Brushes.Maroon, new SolidColorBrush(color_C00E0E), Brushes.Red, Brushes.IndianRed, Brushes.Salmon, Brushes.OrangeRed,
        new SolidColorBrush(color_543434), Brushes.SaddleBrown, Brushes.Brown, Brushes.Chocolate, Brushes.SandyBrown, Brushes.DarkOrange,
        new SolidColorBrush(color_A74301), Brushes.Orange, Brushes.Gold, Brushes.Yellow, Brushes.Khaki, Brushes.DarkKhaki,
        Brushes.Olive, Brushes.DarkGreen, Brushes.Green, Brushes.LimeGreen, Brushes.Lime, Brushes.DarkSeaGreen,
        Brushes.YellowGreen, Brushes.GreenYellow, Brushes.LightGreen, Brushes.PaleGreen, Brushes.Aquamarine, Brushes.Turquoise,
        Brushes.Teal, Brushes.LightSeaGreen, Brushes.DeepSkyBlue, Brushes.Cyan, Brushes.PaleTurquoise, Brushes.LightBlue,
        Brushes.DarkBlue, new SolidColorBrush(color_114A97), Brushes.Blue, Brushes.RoyalBlue, Brushes.DodgerBlue, Brushes.SlateBlue,
        Brushes.DarkViolet, Brushes.BlueViolet, Brushes.Violet, Brushes.Fuchsia, Brushes.DeepPink, Brushes.Pink};
        
        private SKColor[] SKcolorBut = new SKColor[] { SKColors.Maroon, SKcolor_C00E0E, SKColors.Red, SKColors.IndianRed, SKColors.Salmon, SKColors.OrangeRed,
        SKcolor_543434, SKColors.SaddleBrown, SKColors.Brown, SKColors.Chocolate, SKColors.SandyBrown, SKColors.DarkOrange,
        SKcolor_A74301, SKColors.Orange, SKColors.Gold, SKColors.Yellow, SKColors.Khaki, SKColors.DarkKhaki,
        SKColors.Olive, SKColors.DarkGreen, SKColors.Green, SKColors.LimeGreen, SKColors.Lime, SKColors.DarkSeaGreen,
        SKColors.YellowGreen, SKColors.GreenYellow, SKColors.LightGreen, SKColors.PaleGreen, SKColors.Aquamarine, SKColors.Turquoise,
        SKColors.Teal, SKColors.LightSeaGreen, SKColors.DeepSkyBlue, SKColors.Cyan, SKColors.PaleTurquoise, SKColors.LightBlue,
        SKColors.DarkBlue, SKcolor_114A97, SKColors.Blue, SKColors.RoyalBlue, SKColors.DodgerBlue, SKColors.SlateBlue,
        SKColors.DarkViolet, SKColors.BlueViolet, SKColors.Violet, SKColors.Fuchsia, SKColors.DeepPink, SKColors.Pink};
    
        public Color(Brush[] brushes, int index, int[] num1, int[] num2, int[] num3, ViewModel vm, Dictionary<int, Button> ColorLine_But, bool CommonColorBut, Button ButForCommon, Brush [] brusheForCommonBut)
        {
            InitializeComponent();
            _brushes = brushes;
            _par_num = index;
            _num1 = num1;
            _num2 = num2;
            _num3 = num3;
            num1Color = 4 + _par_num * 3;
            num2Color = 5 + _par_num * 3;
            num3Color = 6 + _par_num * 3;
            _vm = vm;
            _colorLine_But = ColorLine_But;
            _commonColorBut = CommonColorBut;
            _butForCommon = ButForCommon;
            _brusheForCommonBut = brusheForCommonBut;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void OKBut_Click(object sender, RoutedEventArgs e)
        {
            if (!_commonColorBut)
            {
                _vm.SeriesColor[num1Color] = SKcolorBut[tag];
                _vm.SeriesColor[num2Color] = SKcolorBut[tag].WithAlpha(75);
                _vm.SeriesColor[num3Color] = SKcolorBut[tag].WithAlpha(50);
                _brushes[_par_num] = colorBut[tag];
            }
            else
            {
                _brusheForCommonBut[0] = colorBut[tag];
                _vm.SeriesColor[3]= SKcolorBut[tag];
                Ok_button = true;
            }
            Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_33(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_34(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_35(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_36(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_37(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_38(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_39(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_40(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_41(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_42(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_43(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_44(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_45(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Button_Click_46(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                tag = Convert.ToInt32(button.Tag);
                ColorButLine_Click(tag);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_commonColorBut)
            ColorLineChange();
            else if (_commonColorBut && Ok_button)
            {
                _commonColorBut = false;
            }
            else
            {
                var existingLineSeries = _vm.Series1[_par_num] as LineSeries<ObservablePoint>;
                if (existingLineSeries != null)
                    existingLineSeries.Stroke = new SolidColorPaint(_vm.SeriesColor[3]);
                _butForCommon.Background = _brusheForCommonBut[0];
                _commonColorBut = false;
            }
            
        }

        private void ColorLineChange()
        {            
                _colorLine_But[_par_num].Background = _brushes[_par_num];
                try
                {
                    if (_num1[_par_num] != 0 && _num2[_par_num] != 0 && _num3[_par_num] != 0)
                    {
                        var existingLineSeries = _vm.Series1[_num1[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(_vm.SeriesColor[num1Color]);

                        existingLineSeries = _vm.Series1[_num2[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(_vm.SeriesColor[num2Color]);

                        existingLineSeries = _vm.Series1[_num3[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(_vm.SeriesColor[num3Color]);
                    }
                }
                catch { }           
        }
        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {            
            Close();
        }
       

        private void ColorButLine_Click(int tag)
        {

            if (_commonColorBut)
            {
                var existingLineSeries = _vm.Series1[_par_num] as LineSeries<ObservablePoint>;
                if (existingLineSeries != null)
                    existingLineSeries.Stroke = new SolidColorPaint(SKcolorBut[tag]);
                _butForCommon.Background = colorBut[tag];
            }
            else
            {
                _colorLine_But[_par_num].Background = colorBut[tag];
                try
                {
                    if (_num1[_par_num] != 0 && _num2[_par_num] != 0 && _num3[_par_num] != 0)
                    {
                        var existingLineSeries = _vm.Series1[_num1[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(SKcolorBut[tag]);

                        existingLineSeries = _vm.Series1[_num2[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(SKcolorBut[tag].WithAlpha(75));

                        existingLineSeries = _vm.Series1[_num3[_par_num]] as LineSeries<ObservablePoint>;
                        if (existingLineSeries != null)
                            existingLineSeries.Stroke = new SolidColorPaint(SKcolorBut[tag].WithAlpha(50));
                    }

                }
                catch { }
            }
                
            
        }

        private void Maroon_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int tag = Convert.ToInt32(button.Tag);                
                ColorButLine_Click(tag);
            }

                        
        }
    }
}
