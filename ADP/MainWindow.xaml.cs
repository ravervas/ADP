using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using Microsoft.Win32;

namespace ADP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

   
    public partial class MainWindow : System.Windows.Window
    {
        public ViewModel vm;
        private string path, FN;
        private const double lamdaKa1Cr= 2.28962;
        private const double lamdaKa2Cr = 2.29351;
        private List<string> name = new List<string>();
        private List<double> Y_Values = new List<double>();
        private List<double> X_values = new List<double>();
        private List<double> Y_Baseline = new List<double>();
        private List<double> Y_Red = new List<double>();
        private List<double> X_BL_pix = new List<double>();
        private List<double> Y_BL_pix = new List<double>();
        private List<int> radioButClick = new List<int>();
        private int[] sh_click = new int[10];
        private int[] w_click = new int[10];
        private int[] c_click = new int[10];
        private int[] s_click = new int[10];
        public Brush[] brushes = new Brush[] {Brushes.Red, Brushes.Green, Brushes.Orange, Brushes.Turquoise, Brushes.Maroon, Brushes.DarkGreen, Brushes.OrangeRed, Brushes.Fuchsia, Brushes.DarkBlue };
        public Brush[] brusheForCommonBut = new Brush[] { Brushes.Blue };
        int[] num1 = new int[10];
        int[] num2 = new int[10];
        int[] num3 = new int[10];
        private int N_name, MouseRange, N_Values, k, b, vs, ve, ii, N, i_max, N_arr, coef, col_i, nj, x1_fwhm_i, x2_fwhm_i, num0, par_num, num1Color, num2Color, num3Color,  col_line=-1, numOfLeftLine, numOfRightLine, numRedLine, numSumLine, radioButCommonClick;
        private double lamda, lamdaKa1, lamdaKa2, MouseDown_Y, MouseDown_X, MouseMove_Y, MouseMove_X, Y_min, X_min, Y_B, a0, a1, a2, a3, par_a, par_b, ValMax, I1_2, x1_fwhm, x2_fwhm, mu, SStot, SSres, R2, Sv, Ycp, mu_final, R2_final, Sv_final, Voigt, Voigt1, Voigt2, x1, x2, addPar;
        private bool ButDisplay = false, Difract = false, But_range = false, MouseDown = false, BS = false, CaptPoint = false, CursorCross = false, alphaDuble=true, RectSect=false, DashLine=false, MD_slider=false, TB_focus=false, ColorButForCommon = false;        
        //List<UIElement> DelLineNames = new List<UIElement>();        
        //List<UIElement> ParLineNames = new List<UIElement>();
        //List<UIElement> RadioButNames = new List<UIElement>();
        //List<UIElement> GBNames = new List<UIElement>();
        Dictionary<string, Button> DelLineNames = new Dictionary<string, Button>();
        Dictionary<string, Button> AddLineNames = new Dictionary<string, Button>();
        Dictionary<string, Button> ParLineNames = new Dictionary<string, Button>();
        Dictionary<string, RadioButton> RadioButNames = new Dictionary<string, RadioButton>();
        Dictionary<string, GroupBox> GBNames = new Dictionary<string, GroupBox>();
        Dictionary<string, Expander> ExpanderName = new Dictionary<string, Expander>();
        Dictionary<int, TextBox> textBox_s_num = new Dictionary<int, TextBox>();
        Dictionary<int, TextBox> textBox_c_num = new Dictionary<int, TextBox>();
        Dictionary<int, TextBox> textBox_w_num = new Dictionary<int, TextBox>();
        Dictionary<int, TextBox> textBox_sh_num = new Dictionary<int, TextBox>();
        Dictionary<int, TextBox> textBox_dist_num = new Dictionary<int, TextBox>();
        Dictionary<int, Slider> slider_s_num = new Dictionary<int, Slider>();
        Dictionary<int, Slider> slider_c_num = new Dictionary<int, Slider>();
        Dictionary<int, Slider> slider_w_num = new Dictionary<int, Slider>();
        Dictionary<int, Slider> slider_sh_num = new Dictionary<int, Slider>();
        Dictionary<int, CheckBox> checkBox_num = new Dictionary<int, CheckBox>();
        Dictionary<int, Button> colorLine_But = new Dictionary<int, Button>();
        double[] X_BL = new double[50];
        double[] Y_BL = new double[50];
        double[] X_P;
        double[] Y_P;
        double[,] array;
        double[] array_Y;
        double[] a_sp;
        double[] b_sp;
        double[] c_sp;
        double[] d_sp;
        double[] remnant;
        double[] par_mu_final = new double[10];
        double[] par_x1 = new double[10];
        double[] par_par_b = new double[10];
        double[] par_Sv_final = new double[10];
        public double[] lamdaKa1List = new double[] { 2.28970, 1.93604, 1.78897, 1.54056, 0.70930 };
        public double[] lamdaKa2List = new double[] { 2.29361, 1.93998, 1.79285, 1.54439, 0.71359 };
        Matrix<double> mat_X;
        Matrix<double> mat_T;
        Matrix<double> mat_Y;
        Matrix<double> mat_coef_SP;
        ExportData export;
        Color openCW;

        //private void Window_Activated(object sender, EventArgs e)
        //{
          //  var exportWindow = Application.Current.Windows.OfType<ExportData>().FirstOrDefault();
           // if (exportWindow != null)
            //    export.Focus();
        //}
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var exportWindow = Application.Current.Windows.OfType<ExportData>().FirstOrDefault(); // Условие на то, что дочернее окно уже открыто
            if (exportWindow==null)
            {
                try
                {                    
                    export = new ExportData(FN, lamdaKa1, lamdaKa2, vs, ve, X_values.Count - 1, X_values, Y_Values, par_mu_final, par_x1, par_par_b, par_Sv_final);
                    export.Owner = Application.Current.MainWindow; // Это позволяет оставлять дочернее окно открытым и видимым в случае, когда сворачиваешь основное окно и после вовращается на него
                    export.DrawCheckBox(col_line);
                    export.ShowDialog(); // Если использовать ShowDialog вместо Show, то пока открыта дочернее окно, основное окно неактивно 
                }
                catch { }
                                               
            }
        }


        private void RemoveLine(int par_num)
        {

            if (par_num < col_line)
            {
                for (int i = par_num; i < col_line; i++)
                {
                    par_mu_final[i] = par_mu_final[i + 1];
                    par_x1[i] = par_x1[i + 1];
                    par_par_b[i] = par_par_b[i + 1];
                    par_Sv_final[i] = par_Sv_final[i + 1];
                    DrawingLine(num1[i], num2[i], num3[i], i);
                }
                for (int i = 0; i < 3; i++)
                    vm.RemoveSeries(vm.Series1, vm.Series1[num1[col_line]]);
                numSumLine -= 3;
                RemoveGB(col_line);
                RemoveExpander(col_line + 1);
                sh_click[col_line] = 0;
                c_click[col_line] = 0;
                w_click[col_line] = 0;
                s_click[col_line] = 0;
                col_line -= 1;
                if (col_line == 0)
                {
                    vm.RemoveSeries(vm.Series1, vm.Series1[numSumLine]);
                    RemoveRadioButCommon();
                }
                    DrawingSumLine();

                
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    vm.RemoveSeries(vm.Series1, vm.Series1[num1[par_num]]);
                
                RemoveGB(col_line);
                RemoveExpander(col_line+1);
                sh_click[col_line] = 0;
                c_click[col_line] = 0;
                w_click[col_line] = 0;
                s_click[col_line] = 0;
                col_line -= 1;
                if (col_line > -1)
                    numSumLine -= 3;
                if (col_line == 0)
                {
                    vm.RemoveSeries(vm.Series1, vm.Series1[numSumLine]);
                    RemoveRadioButCommon();
                }
                if (col_line == -1)
                    Optimization.IsEnabled = false; 
                    
                

                DrawingSumLine();                
                
            }

        }

        private void RemoveLinesAsChangeArea()
        {
            try 
            {                
                for (int i=col_line;i>=0;i--)
                {
                    if (i == 1)
                        vm.RemoveSeries(vm.Series1, vm.Series1[numSumLine]);
                    for (int j = 0; j < 3; j++)
                        vm.RemoveSeries(vm.Series1, vm.Series1[num1[i]]);
                    RemoveGB(i);
                    RemoveExpander(i+1);
                    sh_click[i] = 0;
                    c_click[i] = 0;
                    w_click[i] = 0;
                    s_click[i] = 0;
                    col_line = -1;
                    RemoveRadioButCommon();
                }
            
            }
            catch
            {

            }
        }

        private void RemoveRadioButCommon()
        {
            SepInReg.Visibility = Visibility.Hidden;
            TextBlockCommon.Visibility = Visibility.Hidden;
            RadioButForCommon.Visibility = Visibility.Hidden;
            ButForCommon.Visibility = Visibility.Hidden;
            radioButCommonClick = 0;
            RadioButForCommon.IsChecked = true;
        }

        private void RemoveGB(int col_line)
        {
            try
            {
                int index = col_line + 2;
                if (GBNames.TryGetValue("GB_" + index, out GroupBox groupBox) && RadioButNames.TryGetValue("RadioBut" + index, out RadioButton radioButton) && AddLineNames.TryGetValue("AddLine" + index, out Button addLine) && DelLineNames.TryGetValue("DelLine" + index, out Button delLineFirst) && ParLineNames.TryGetValue("ParLine" + index, out Button parLineFirst))
                { 
                    StackPanelForLines.Children.Remove(groupBox);
                    GBNames.Remove(groupBox.Name);
                    RadioButNames.Remove(radioButton.Name);
                    AddLineNames.Remove(addLine.Name);
                    DelLineNames.Remove(delLineFirst.Name);
                    ParLineNames.Remove(parLineFirst.Name);
                }
                if (DelLineNames.TryGetValue("DelLine" + (index - 1), out Button delLine) && ParLineNames.TryGetValue("ParLine" + (index - 1), out Button parLine) && RadioButNames.TryGetValue("RadioBut" + (index - 1), out RadioButton radioBut))
                {
                    delLine.Visibility = Visibility.Hidden;
                    parLine.Visibility = Visibility.Hidden;
                    radioBut.IsChecked = false;
                }
                //DelLineNames[col_line].Visibility = Visibility.Hidden;              
                //ParLineNames[col_line].Visibility = Visibility.Hidden;
                radioButClick[col_line] = 0;
                //if (RadioButNames[col_line] is RadioButton radioButton)
                   // radioButton.IsChecked = false;
                //GBNames[col_line+1].Visibility = Visibility.Hidden;
            }
            catch
            {

            }
        }

        private void DeleteLine()
        {

                try
                {
                    do
                    {
                        vm.RemoveSeries(vm.Series1, vm.Series1[0]);
                    } while (vm.Series1.Count != 0);
                }
                catch
                {
                    
                }
                try
                {
                    do
                    {
                        vm.RemoveSeries(vm.Series2, vm.Series2[0]);
                    } while (vm.Series2.Count != 0);
                }
                catch
                {
                    
                }
                

            try
            {
                for (int i = 0; i < 3; i++)
                    vm.RemoveSeries(vm.Series1, vm.Series1[num1[par_num]]);
            }
            catch
            {
                
            }
           

            for (int i=0;i<=col_line;i++)
            {
                RemoveGB(i);
                RemoveExpander(i + 1);
                sh_click[i] = 0;
                c_click[i] = 0;
                w_click[i] = 0;
                s_click[i] = 0;                
            }
            RemoveRadioButCommon();

            R_reg.Text = " ";
            R_reg_dub.Text = " ";

            
            

        }

        private void RadioButForCommon_Click(object sender, RoutedEventArgs e)
        {
            ++radioButCommonClick;
            {
                switch(radioButCommonClick)
                {
                    case 1:
                        {
                            vm.Series1[numSumLine].IsVisible = false;
                            RadioButForCommon.IsChecked = false;
                        }
                        break;

                    case 2:
                        {
                            vm.Series1[numSumLine].IsVisible = true;
                            RadioButForCommon.IsChecked = true;
                            radioButCommonClick = 0;
                        }
                        break;
                }
            }
        }
        public void RadioButtonClick(ref List<int> Click, int par_num, int index)
        {
            ++Click[par_num];
            {
                switch(Click[par_num])
                {
                    case 1:
                        {
                            //if (RadioButNames[par_num] is RadioButton radioButton)
                            //  radioButton.IsChecked = false;
                            if (RadioButNames.TryGetValue("RadioBut" + index, out RadioButton radioButton))
                                radioButton.IsChecked = false;
                                vm.Series1[num1[par_num]].IsVisible = false;
                            vm.Series1[num2[par_num]].IsVisible = false;
                            vm.Series1[num3[par_num]].IsVisible = false;
                            checkBox_num[par_num].IsEnabled = false;
                        }
                        break;

                    case 2:
                        {
                            // if (RadioButNames[par_num] is RadioButton radioButton)
                            // radioButton.IsChecked = true;
                            if (RadioButNames.TryGetValue("DelLine" + index, out RadioButton radioButton))
                                radioButton.IsChecked = true;
                            vm.Series1[num1[par_num]].IsVisible = true;
                            if (checkBox_num[par_num].IsChecked == true)
                            {
                                vm.Series1[num2[par_num]].IsVisible = true;
                                vm.Series1[num3[par_num]].IsVisible = true;
                            }
                            checkBox_num[par_num].IsEnabled = true;
                            Click[par_num] = 0;
                        }
                        break;
                }
            }
        }
        

        private void RadioBut2_Click(object sender, RoutedEventArgs e)
        {
           // if (DelLine2.Visibility != Visibility.Visible)
           //     RadioBut2.IsChecked = false;
           // else
                //RadioButtonClick(ref radioButClick, 1);
        }

        private void RadioBut3_Click(object sender, RoutedEventArgs e)
        {
            //if (DelLine3.Visibility != Visibility.Visible)
               // RadioBut3.IsChecked = false;
            //else               
                //RadioButtonClick(ref radioButClick, 2);
        }
        public void LamdaDef(double value)
        {
            double[] var = new double[lamdaKa1List.Length];
            for (int i=0;i<lamdaKa1List.Length;i++)            
                var[i] = Math.Abs(value - lamdaKa1List[i]);
            double minValue = var.Min();
            int minIndex = var.ToList().IndexOf(minValue);
            cb.SelectedIndex = minIndex;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lamdaKa1 = lamdaKa1List[cb.SelectedIndex];
            lamdaKa2 = lamdaKa2List[cb.SelectedIndex];
            lamda = (2 * lamdaKa1 + lamdaKa2) / 3;
        }

        private void Optimization_Click(object sender, RoutedEventArgs e)
        {
            
            //LevenbergMarquardtInerationMethod;

            var xData = new DenseVector(new double[ve - vs + 1]);
            var yData = new DenseVector(new double[ve - vs + 1]);

            for (int i = 0; i <= ve - vs; i++)
            {
                xData[i] = X_values[i + vs];
                yData[i] = Y_Values[i + vs];
            }


            Vector<double> Model(Vector<double> parameters, Vector<double> x)
            {
                var y = CreateVector.Dense<double>(x.Count);
                double mu_par;
                double x1_par;
                double b_par;
                double Sv_par;
                int z;
                for (int i = 0; i < x.Count; i++)
                {
                    y[i] = 0;
                    z = 0;
                    for (int j = 0; j <= col_line; j++)
                    {
                        if (textBox_sh_num[j].IsEnabled == true)
                        {
                            mu_par = parameters[z];
                            z++;
                        }
                        else
                            mu_par = par_mu_final[j];

                        if (textBox_c_num[j].IsEnabled == true)
                        {
                            x1_par = parameters[z];
                            z++;
                        }
                        else
                            x1_par = par_x1[j];

                        if (textBox_w_num[j].IsEnabled == true)
                        {
                            b_par = parameters[z];
                            z++;
                        }
                        else
                            b_par = par_par_b[j];

                        if (textBox_s_num[j].IsEnabled == true)
                        {
                            Sv_par = parameters[z];
                            z++;
                        }
                        else
                            Sv_par = par_Sv_final[j];

                        y[i] += (mu_par / (b_par * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(x[i] - x1_par, 2) / (2 * Math.Pow(b_par, 2))) + (1 - mu_par) / (Math.PI * b_par * (1 + Math.Pow((x[i] - x1_par) / b_par, 2))) + 0.5 * (mu_par / (b_par * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(x[i] - Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(x1_par / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2), 2) / (2 * Math.Pow(b_par, 2))) + (1 - mu_par) / (Math.PI * b_par * (1 + Math.Pow((x[i] - Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(x1_par / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2)) / b_par, 2))))) * Sv_par;
                        //y[i] += (parameters[0 + 4 * j] / (parameters[2 + 4 * j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(x[i] - parameters[1 + 4 * j], 2) / (2 * Math.Pow(parameters[2 + 4 * j], 2))) + (1 - parameters[0 + 4 * j]) / (Math.PI * parameters[2 + 4 * j] * (1 + Math.Pow((x[i] - parameters[1 + 4 * j]) / parameters[2 + 4 * j], 2))) + 0.5 * (parameters[0 + 4 * j] / (parameters[2 + 4 * j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(x[i] - Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(parameters[1 + 4 * j] / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2), 2) / (2 * Math.Pow(parameters[2 + 4 * j], 2))) + (1 - parameters[0 + 4 * j]) / (Math.PI * parameters[2 + 4 * j] * (1 + Math.Pow((x[i] - Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(parameters[1 + 4 * j] / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2)) / parameters[2 + 4 * j], 2))))) * parameters[3 + 4 * j];                        
                    }
                }
                return y;
            }



            var start = new DenseVector(new double[4 + 4 * col_line]);
            int zi = 0;
            for (int i = 0; i <= col_line; i++)
            {
                if (textBox_sh_num[i].IsEnabled == true)
                {
                    start[zi] = par_mu_final[i];
                    zi++;
                }
                if (textBox_c_num[i].IsEnabled == true)
                {
                    start[zi]= par_x1[i];
                    zi++;
                }
                if (textBox_w_num[i].IsEnabled == true)
                {
                    start[zi]= par_par_b[i];
                    zi++;
                }
                if (textBox_s_num[i].IsEnabled == true)
                {
                    start[zi]= par_Sv_final[i];
                    zi++;
                }
                //start[0 + 4 * i] = par_mu_final[i];
                //start[1 + 4 * i] = par_x1[i];
                //start[2 + 4 * i] = par_par_b[i];
                //start[3 + 4 * i] = par_Sv_final[i];
            }
            var objective = ObjectiveFunction.NonlinearModel(Model, xData, yData);
            var solver = new LevenbergMarquardtMinimizer(maximumIterations: 10000);
            var result = solver.FindMinimum(objective, start);

            zi = 0;
            for (int i = 0; i <= col_line; i++)
            {
                if (textBox_sh_num[i].IsEnabled == true)
                {
                    par_mu_final[i] = result.MinimizingPoint[zi];
                    zi++;
                }
                if (textBox_c_num[i].IsEnabled == true)
                {
                    par_x1[i] = result.MinimizingPoint[zi];
                    zi++;
                }
                if (textBox_w_num[i].IsEnabled == true)
                {
                    par_par_b[i] = result.MinimizingPoint[zi];
                    zi++;
                }
                if (textBox_s_num[i].IsEnabled == true)
                {
                    par_Sv_final[i] = result.MinimizingPoint[zi];
                    zi++;
                }
                if (par_mu_final[i] < 0)
                    par_mu_final[i] = 0;
                if (par_mu_final[i] > 1)
                    par_mu_final[i] = 1;
                DrawingLine(num1[i], num2[i], num3[i], i);
                textBox_sh_num[i].Text = Math.Round(par_mu_final[i], 3).ToString();
                textBox_c_num[i].Text = Math.Round(par_x1[i], 2).ToString();
                textBox_w_num[i].Text = Math.Round(par_par_b[i], 3).ToString();
                textBox_s_num[i].Text = Math.Round(par_Sv_final[i], 2).ToString();
                textBox_dist_num[i].Text = Math.Round(lamdaKa1 / (2 * Math.Sin(par_x1[i] / 2 * Math.PI / 180)), 3).ToString();
            }

            DrawingSumLine();

        }

        private void DrawingSumLine()
        {
            var list = new List<ObservablePoint>();
            var list1 = new List<ObservablePoint>();
            vm.Series2[0].Values = list;
            if (col_line > 0)
                vm.Series1[numSumLine].Values = list1;

            SSres = 0;

            for (int i = vs; i <= ve; i++)
            {
                Voigt = 0;
                for (int j = 0; j <= col_line; j++)
                {
                    x2 = Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(par_x1[j] / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2);
                    Voigt += (par_mu_final[j] / (par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - par_x1[j], 2) / (2 * Math.Pow(par_par_b[j], 2))) + (1 - par_mu_final[j]) / (Math.PI * par_par_b[j] * (1 + Math.Pow((X_values[i] - par_x1[j]) / par_par_b[j], 2))) + 0.5 * (par_mu_final[j] / (par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(par_par_b[j], 2))) + (1 - par_mu_final[j]) / (Math.PI * par_par_b[j] * (1 + Math.Pow((X_values[i] - x2) / par_par_b[j], 2))))) * par_Sv_final[j];
                }
                //if (DelLine2.Visibility == Visibility.Visible)
                    list1.Add(new ObservablePoint(X_values[i], Voigt));
                remnant[i - vs] = Y_Values[i] - Voigt;
                list.Add(new ObservablePoint(X_values[i], remnant[i - vs]));
                SSres = SSres + Math.Pow(Y_Values[i] - Voigt, 2);
            }

            R2 = 1 - SSres / SStot;
            if (col_line < 0)
            {
                R_reg.Text = " ";
                R_reg_dub.Text = R_reg.Text;
            }
            else
            {
                R_reg.Text = "R² = " + Math.Round(R2, 4).ToString();
                R_reg_dub.Text = R_reg.Text;
            }
        }
        private void DrawingLine(int num1, int num2, int num3, int num)
        {
            var list = new List<ObservablePoint>();
            var list1 = new List<ObservablePoint>();
            var list2 = new List<ObservablePoint>();

            vm.Series1[num1].Values = list;
            vm.Series1[num2].Values = list1;
            vm.Series1[num3].Values = list2;

            mu_final = par_mu_final[num];
            x1 = par_x1[num];
            x2 = Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(x1 / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2);
            par_b = par_par_b[num];
            Sv_final = par_Sv_final[num];

            for (int i = vs; i <= ve; i++)
            {
                Voigt1 = mu_final / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x1, 2) / (2 * Math.Pow(par_b, 2))) + (1 - mu_final) / (Math.PI * par_b * (1 + Math.Pow((X_values[i] - x1) / par_b, 2)));
                Voigt2 = 0.5 * (mu_final / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(par_b, 2))) + (1 - mu_final) / (Math.PI * par_b * (1 + Math.Pow((X_values[i] - x2) / par_b, 2))));
                Voigt = (Voigt1 + Voigt2) * Sv_final;
                list.Add(new ObservablePoint(X_values[i], Voigt));
                list1.Add(new ObservablePoint(X_values[i], Voigt1 * Sv_final));
                list2.Add(new ObservablePoint(X_values[i], Voigt2 * Sv_final));

            }
        }


        private void AddLine()
        {
            //if (DelLine2.Visibility != Visibility.Visible)
            if (DelLineNames.TryGetValue("DelLine2", out Button button))
                if (button.Visibility==Visibility.Hidden)
            {                
                ValMax = Y_Values[vs];
                Ycp = Y_Values[vs];

                remnant = new double[ve - vs + 1];

                for (int i = vs + 1; i <= ve; i++)
                {
                    if (Y_Values[i] > ValMax)
                    {
                        ValMax = Y_Values[i];
                        //if (!alphaDuble)
                        par_a = X_values[i];
                        i_max = i;
                    }

                    Ycp = Ycp + Y_Values[i];
                }

                Ycp = Ycp / (ve - vs + 1);

                SStot = 0;
                for (int i = vs; i <= ve; i++)
                    SStot = SStot + Math.Pow(Y_Values[i] - Ycp, 2);

                I1_2 = ValMax / 2;

                int k = i_max - 1;

                do
                {

                    x1_fwhm = X_values[k];
                    x1_fwhm_i = k;
                    k -= 1;
                } while (Y_Values[k] > I1_2);

                k = i_max + 1;
                do
                {
                    x2_fwhm = X_values[k];
                    x2_fwhm_i = k;
                    k += 1;
                } while (Y_Values[k] > I1_2);
            }
            else
            {
                ValMax = remnant[0];

                for (int i = vs + 1; i <= ve; i++)
                {
                    if (remnant[i - vs] > ValMax)
                    {
                        ValMax = remnant[i - vs];
                        par_a = X_values[i];
                        i_max = i;
                    }

                }

                I1_2 = ValMax / 2;

                int k = i_max - 1;
                do
                {

                    x1_fwhm = X_values[k];
                    x1_fwhm_i = k;
                    k -= 1;
                } while (remnant[k - vs] > I1_2);



                k = i_max + 1;
                do
                {
                    x2_fwhm = X_values[k];
                    x2_fwhm_i = k;
                    k += 1;
                } while (remnant[k - vs] > I1_2);
            }

            par_b = (x2_fwhm - x1_fwhm) / 2;


            if (!alphaDuble) // Требует доработки
            {
                var list = new List<ObservablePoint>();
                var list1 = new List<ObservablePoint>();
               // vm.Series[5].Values = list;
                vm.Series2[0].Values = list1;
                FunVoigt(par_a, par_b);

                for (int i = vs; i <= ve; i++)
                {
                    Voigt = mu_final / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - par_a, 2) / (2 * Math.Pow(par_b, 2))) + (1 - mu_final) / ((Math.PI * par_b) * (1 + Math.Pow((X_values[i] - par_a) / par_b, 2)));
                    //list.Add(new ObservablePoint(X_values[i], S1 / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - par_a, 2) / (2 * Math.Pow(par_b, 2)))));
                    list.Add(new ObservablePoint(X_values[i], Voigt * Sv_final));
                    list1.Add(new ObservablePoint(X_values[i], Y_Values[i] - Voigt * Sv_final));
                }
            }

            if (alphaDuble)
            {
                // Расчет центра тяжести

                //sum_Y = Y_Values[x1_fwhm_i];
                //sum_YX = Y_Values[x1_fwhm_i] * X_values[x1_fwhm_i];

                //for (int i = x1_fwhm_i + 1; i <= x2_fwhm_i; i++)
                //{
                //  sum_Y += Y_Values[i];
                // sum_YX += Y_Values[i] * X_values[i];
                //}

                //par_a = sum_YX / sum_Y;

                // конец 

                FunVoigtAddPeak(par_a, par_b, num1Color, num2Color, num3Color, num0, par_num);
               

            }


        }
      

        public void FunVoigtAddPeak(double a, double b, int num1Color, int num2Color, int num3Color, int num0, int parnum)
        {
            var list = new List<ObservablePoint>();
            var list1 = new List<ObservablePoint>();
            var list2 = new List<ObservablePoint>();
            var list3 = new List<ObservablePoint>();
            var list4 = new List<ObservablePoint>();

            if (col_line > 1)
                vm.RemoveSeries(vm.Series1, vm.Series1[numSumLine]);
            

            num1[parnum] = vm.Series1.Count;
            num2[parnum] = vm.Series1.Count+1;
            num3[parnum] = vm.Series1.Count+2;
            
            vm.Series2[num0].Values = list3;

            

            x1 = a;
            x2 = Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(x1/2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI,2);
           

            // без подгона мю

            mu_final = 0.5;

                     SSres = 0;

                 Voigt = mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i_max] - x1, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i_max] - x1) / b, 2))) + 0.5 * (mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i_max] - x2, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i_max] - x2) / b, 2))));
                 Sv = ValMax / Voigt;

            if (col_line == 0)
            {
                for (int i = vs; i <= ve; i++)
                {
                    Voigt = (mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x1, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x1) / b, 2))) + 0.5 * (mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x2) / b, 2))))) * Sv;
                    //if (DelLine2.Visibility != Visibility.Visible)
                    SSres = SSres + Math.Pow(Y_Values[i] - Voigt, 2);
                }
                //if (DelLine2.Visibility != Visibility.Visible)
                R2_final = 1 - SSres / SStot;
            }

            Sv_final = Sv;

            par_mu_final[parnum] = mu_final;
            par_x1[parnum] = x1;
            par_par_b[parnum] = b;
            par_Sv_final[parnum] = Sv_final;

            textBox_sh_num[parnum].Text = Math.Round(mu_final, 3).ToString();
            textBox_c_num[parnum].Text = Math.Round(x1, 2).ToString();
            textBox_w_num[parnum].Text = Math.Round(b, 3).ToString();
            textBox_s_num[parnum].Text = Math.Round(Sv_final,2).ToString();
            textBox_dist_num[parnum].Text = Math.Round(lamdaKa1 / (2 * Math.Sin(x1 / 2 * Math.PI / 180)), 3).ToString();
            


            for (int i = vs; i <= ve; i++)
            {
                Voigt1 = mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x1, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x1) / b, 2)));
                Voigt2 = 0.5 * (mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x2) / b, 2))));
                Voigt = mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x1, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x1) / b, 2))) + 0.5 * (mu_final / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(b, 2))) + (1 - mu_final) / (Math.PI * b * (1 + Math.Pow((X_values[i] - x2) / b, 2))));
               //if (DelLineNames.TryGetValue("DelLine" + (parnum+1), out Button delLine))
                 //if (delLine.Visibility==Visibility.Hidden)
                 if (col_line<=0)
                remnant[i - vs] = Y_Values[i] - Voigt * Sv_final;
                else
                    remnant[i-vs]=remnant[i-vs]- Voigt * Sv_final;
                list.Add(new ObservablePoint(X_values[i], Voigt * Sv_final));
                list1.Add(new ObservablePoint(X_values[i], Voigt1 * Sv_final));
                list2.Add(new ObservablePoint(X_values[i], Voigt2 * Sv_final));
                list3.Add(new ObservablePoint(X_values[i], remnant[i-vs]));
                //if (DelLineNames.TryGetValue("DelLine" + parnum, out Button delLineButton))
                    //if (delLineButton.Visibility == Visibility.Visible)
                    if (col_line>0)
                {                   
                    Voigt = 0;                    
                    for (int j = 0; j <= col_line; j++)
                    {
                        x2 = Math.Round(2 * Math.Asin(lamdaKa2 * Math.Sin(par_x1[j] / 2 * Math.PI / 180) / lamdaKa1) * 180 / Math.PI, 2);
                        Voigt += (par_mu_final[j] / (par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - par_x1[j], 2) / (2 * Math.Pow(par_par_b[j], 2))) + (1 - par_mu_final[j]) / (Math.PI * par_par_b[j] * (1 + Math.Pow((X_values[i] - par_x1[j]) / par_par_b[j], 2))) + 0.5 * (par_mu_final[j] / (par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(par_par_b[j], 2))) + (1 - par_mu_final[j]) / (Math.PI * par_par_b[j] * (1 + Math.Pow((X_values[i] - x2) / b, 2))))) * par_Sv_final[j];
                    }
                    SSres = SSres + Math.Pow(Y_Values[i] - Voigt, 2);                    
                    list4.Add(new ObservablePoint(X_values[i], Voigt));
                }             
            }
            //if (DelLineNames.TryGetValue("DelLine" + parnum, out Button button))
               // if (button.Visibility == Visibility.Visible)
               if (col_line>0)
                R2_final = 1 - SSres / SStot;
            R_reg.Text = "R² = " + Math.Round(R2_final, 4).ToString();
            R_reg_dub.Text = R_reg.Text;

                vm.AddNewLineSeries(list, num1Color, vm.Series1, num1[parnum]);
                vm.AddNewLineSeries(list1, num2Color, vm.Series1, num2[parnum]);
                vm.AddNewLineSeries(list2, num3Color, vm.Series1, num3[parnum]);

                if (col_line>=1)
                  {
                     vm.AddNewLineSeries(list4, 3, vm.Series1, vm.Series1.Count);
                     numSumLine = vm.Series1.Count - 1;
                  }

            
        }
       
        public void FunVoigt(double a, double b)
        {
            for (mu = 0; mu <= 1; mu = mu + 0.025)
            {
                SSres = 0;
                Voigt = mu / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i_max] - a, 2) / (2 * Math.Pow(b, 2))) + (1 - mu) / (Math.PI * b * (1 + Math.Pow((X_values[i_max] - a) / b, 2)));
                Sv = ValMax / Voigt;
                for (int i = vs; i <= ve; i++)
                {
                    Voigt = (mu / (b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - a, 2) / (2 * Math.Pow(b, 2))) + (1 - mu) / (Math.PI * b * (1 + Math.Pow((X_values[i] - a) / b, 2)))) * Sv;
                    SSres = SSres + Math.Pow(Y_Values[i] - Voigt, 2);
                }
                R2 = 1 - SSres / SStot;
                if (R2_final < R2)
                {
                    R2_final = R2;
                    mu_final = mu;
                    Sv_final = Sv;
                }

            }
        }
        
        private void ToggleButton_Grid_Unchecked(object sender, RoutedEventArgs e)
        {
            vm.XAxes[0].ShowSeparatorLines = false;
            vm.YAxes[0].ShowSeparatorLines = false;
        }

        
        private void ToggleButton_Grid_Checked(object sender, RoutedEventArgs e)
        {
            vm.XAxes[0].ShowSeparatorLines = true;
            vm.YAxes[0].ShowSeparatorLines = true;
        }

        
        private void MyChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Scale.IsChecked == true)
            {
                MouseDown = false;
                vm.RemoveRect(vm.Sections1[0]);
                RectSect = false;
                
                if (MouseDown_X < MouseMove_X)
                {
                    vm.XAxes[0].MinLimit = MouseDown_X;
                    vm.XAxes[0].MaxLimit = MouseMove_X;
                    vm.XAxes2[0].MinLimit = MouseDown_X;
                    vm.XAxes2[0].MaxLimit = MouseMove_X;
                }
                else
                {
                    vm.XAxes[0].MinLimit = MouseMove_X;
                    vm.XAxes[0].MaxLimit = MouseDown_X;
                    vm.XAxes2[0].MinLimit = MouseMove_X;
                    vm.XAxes2[0].MaxLimit = MouseDown_X;
                }
                if (MouseDown_Y < MouseMove_Y)
                {
                    vm.YAxes[0].MaxLimit = MouseMove_Y;
                    vm.YAxes[0].MinLimit = MouseDown_Y;
                }
                else
                {
                    vm.YAxes[0].MaxLimit = MouseDown_Y;
                    vm.YAxes[0].MinLimit = MouseMove_Y;
                }
                vm.XAxes[0].MinStep = double.NaN;

                if (vm.YAxes[0].MinLimit < 0)
                    vm.YAxes[0].MinLimit = 0;

            }
            if (CaptPoint)
            {
                 for (int i = 0; i < N - 1; i++)
                   mat_X[i, i * 4] = 1;
                 col_i = 0;
                 coef = 0;
                 for (int i = N - 1; i < (N - 1) * 2; i++)
                  {
                    mat_X[i, coef] = 1;
                    mat_X[i, coef + 1] = X_P[col_i + 1] - X_P[col_i];
                    mat_X[i, coef + 2] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                    mat_X[i, coef + 3] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 3);
                    coef = coef + 4;
                    col_i = col_i + 1;
                 }
                 col_i = 0;
                 coef = 1;
                 for (int i = (N - 1) * 2; i < (N - 1) * 3 - 1; i++)
                 {
                   mat_X[i, coef] = 1;
                   mat_X[i, coef + 1] = 2 * (X_P[col_i + 1] - X_P[col_i]);
                   mat_X[i, coef + 2] = 3 * Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                   mat_X[i, coef + 4] = -1;
                   coef = coef + 4;
                   col_i = col_i + 1;
                  }
                  col_i = 0;
                   coef = 2;
                  for (int i = (N - 1) * 3 - 1; i < (N - 1) * 4 - 2; i++)
                  {
                    mat_X[i, coef] = 2;
                    mat_X[i, coef + 1] = 6 * (X_P[col_i + 1] - X_P[col_i]);
                    mat_X[i, coef + 4] = -2;
                    coef = coef + 4;
                    col_i = col_i + 1;
                 }

                   mat_X[N_arr - 2, 2] = 2;
                   mat_X[N_arr - 2, 3] = 6 * (X_P[1] - X_P[0]);
                   mat_X[N_arr - 1, N_arr - 2] = 2;
                   mat_X[N_arr - 1, N_arr - 1] = 6 * (X_P[N - 1] - X_P[N - 2]);

                   for (int i = 0; i < N - 1; i++)
                      mat_Y[i, 0] = Y_P[i];
                  col_i = 1;
                  for (int i = N - 1; i < (N - 1) * 2; i++)
                  {
                     mat_Y[i, 0] = Y_P[col_i];
                    col_i = col_i + 1;
                  }

                  mat_T = mat_X.Inverse();

                  for (int i = 0; i < N_arr; i++)
                     for (int j = 0; j < 1; j++)
                   {
                     mat_coef_SP[i, j] = 0;
                    for (int k = 0; k < N_arr; k++)
                       mat_coef_SP[i, j] += mat_T[i, k] * mat_Y[k, j];
                 }
                  coef = 0;
                  for (int i = 0; i < N - 1; i++)
                  {
                    a_sp[i] = mat_coef_SP[coef, 0];
                    coef += 1;
                    b_sp[i] = mat_coef_SP[coef, 0];
                    coef += 1;
                    c_sp[i] = mat_coef_SP[coef, 0];
                    coef += 1;
                   d_sp[i] = mat_coef_SP[coef, 0];
                   coef += 1;
                 }
                 for (int i = 0; i < N_Values; i++)
                  {
                    for (int j = 1; j < N; j++)
                       if (X_values[i] < X_P[j])
                     {
                       nj = j;
                      break;
                  }
                  Y_B = a_sp[nj - 1] + b_sp[nj - 1] * (X_values[i] - X_P[nj - 1]) + c_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 2) + d_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 3);
                  Y_Red.Insert(i, Y_B);
                  if (X_values[i] > X_P[N - 1])
                  {
                     Y_B = a_sp[N - 2] + b_sp[N - 2] * (X_values[i] - X_P[N - 2]) + c_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 2) + d_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 3);
                     Y_Red.Insert(i, Y_B);
                 }
                 }
                 var list = new List<ObservablePoint>();
                  vm.Series1[numRedLine].Values = list;
                 for (int i = 0; i < N_Values; i++)
                   list.Add(new ObservablePoint(X_values[i], Y_Red[i]));
                CaptPoint = false;
            }
        }

        
        private void MyChart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Difract)
            {                
                
                MouseDown = true;
                var chart = (LiveChartsCore.SkiaSharpView.WPF.CartesianChart)sender;
                var mouseCoordinate = e.GetPosition(chart);
                var scaledPoint = chart.ScalePixelsToData(new LvcPointD(mouseCoordinate.X, mouseCoordinate.Y));
                MouseDown_X = Math.Round(scaledPoint.X, 2);
                MouseDown_Y = scaledPoint.Y;
                

                if (Range.IsChecked == true)
                {
                    ++MouseRange;
                    {
                        switch (MouseRange)
                        {
                            case 1:
                                {
                                    vm.AddDashLine(MouseDown_X, MouseDown_X);
                                    //vm.Sections[1].Xi = MouseDown_X;
                                    //vm.Sections[1].Xj = MouseDown_X;
                                    break;
                                }
                            case 2:
                                {
                                    But_range = true;

                                    RemoveLinesAsChangeArea();
                                    while (vm.Series1.Count != 1)
                                        vm.RemoveSeries(vm.Series1, vm.Series1[1]);
                                  
                                    vs = Convert.ToInt32(vm.Sections1[0].Xi * k + b);
                                    ve = Convert.ToInt32(MouseDown_X * k + b);
                                    if (ve < vs)
                                    {
                                        int P = ve;
                                        ve = vs;
                                        vs = P;
                                    }

                                    var list = new List<ObservablePoint>();

                                    vm.Series1[0].Values = list;
                                    vm.Series2[0].Values = list;

                                    for (int i = vs; i <= ve; i++)
                                    {
                                        list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                                    }                                                                       

                                    list = new List<ObservablePoint>();

                                    for (int i = 0; i < vs; i+=10)
                                    {
                                        list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                                    }


                                        numOfLeftLine = vm.Series1.Count;
                                        vm.AddNewLineSeries(list, 1, vm.Series1, numOfLeftLine);
                                        vm.AddNewLineSeries(list, 1, vm.Series2, 1);


                                    list = new List<ObservablePoint>();

                                    for (int i = ve + 1; i < N_Values; i+=10)
                                    {
                                        list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                                    }

                                        numOfRightLine = vm.Series1.Count;
                                        vm.AddNewLineSeries(list, 1, vm.Series1, numOfRightLine);
                                        vm.AddNewLineSeries(list, 1, vm.Series2, 2);


                                    for (int i = 0; i < 2; i++)                                   
                                        vm.RemoveRect(vm.Sections1[0]);
                                    DashLine = false;
                                    //vm.Sections[1].Xi = -50;
                                    //vm.Sections[1].Xj = -50;
                                    //vm.Sections[2].Xi = -50;
                                    //vm.Sections[2].Xj = -50;
                                    MouseRange = 0;
                                    MouseDown = false;
                                    Range.IsChecked = false;
                                    TextBlockStatus.Text = "Идет выполнение...";
                                    
                                    break;

                                }

                        }

                    }
                }
                if (UserBaseline.IsChecked == true)
                {

                    for (int i = 0; i < N; i++)
                    {
                        var scaledPixel = chart.ScaleDataToPixels(new LvcPointD(X_P[i], Y_P[i]));
                        X_BL_pix.Insert(i, scaledPixel.X);
                        Y_BL_pix.Insert(i, scaledPixel.Y);
                        if (Math.Pow(mouseCoordinate.X - X_BL_pix[i], 2) + Math.Pow(mouseCoordinate.Y - Y_BL_pix[i], 2) < 64)
                        {
                            ii = i;
                            CaptPoint = true;
                            break;
                        }
                        
                    }

                }
            }
        }

        
        private void CutBaseline_Click(object sender, RoutedEventArgs e)
        {
            BS = true;
            
            for (int i = 0; i < N_Values; i++) 
            {               
                Y_Baseline.Insert(i, Y_Values[i] - Y_Red[i]);
            }

            var list = new List<ObservablePoint>();
            
            if (vs>0)
            {

                vm.Series1[1].Values = list;
                vm.Series2[numOfLeftLine].Values = list;

                for (int i = 0; i < vs; i++)
                {
                    Y_Values.Insert(i, Y_Baseline[i]);
                    list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                }

                list = new List<ObservablePoint>();
                vm.Series1[2].Values = list;
                vm.Series2[numOfRightLine].Values = list;
                for (int i = ve + 1; i < N_Values; i++)
                {
                    Y_Values.Insert(i, Y_Baseline[i]);
                    list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                }

            }

            list = new List<ObservablePoint>();
            vm.Series1[0].Values = list;
            vm.Series2[0].Values = list;

            for (int i = vs; i <= ve; i++)
            {
                Y_Values.Insert(i, Y_Baseline[i]);
                list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
            }

            

            TextBlockStatus.Text = "Идет выполнение...";
            UserBaseline.IsChecked = false;
            CutBaseline.IsEnabled = false;
        }

        private void MyChart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                if (UserBaseline.IsChecked == true)
                {
                    var chart = (LiveChartsCore.SkiaSharpView.WPF.CartesianChart)sender;
                    var mouseCoordinate = e.GetPosition(chart);
                    var scaledPoint = chart.ScalePixelsToData(new LvcPointD(mouseCoordinate.X, mouseCoordinate.Y));
                    X_BL[N] = Math.Round(scaledPoint.X, 2);
                    Y_BL[N] = scaledPoint.Y;
                    N = N + 1;
                    X_P = new double[N];
                    Y_P = new double[N];

                    for (int i = 0; i < N; i++)
                    {
                        X_P[i] = X_BL[i];
                        Y_P[i] = Y_BL[i];
                    }

                    for (int i=0;i<N;i++)
                       for (int j=0;j<N-1;j++)
                         {
                            if (X_P[j + 1] < X_P[j])
                            {
                                double p_X = X_P[j + 1];
                                X_P[j + 1] = X_P[j];
                                X_P[j] = p_X;
                                double p_Y = Y_P[j + 1];
                                Y_P[j + 1] = Y_P[j];
                                Y_P[j] = p_Y;
                            }
                        }


                    N_arr = (N - 1) * 4;
                    array = new double[N_arr, N_arr];
                    array_Y = new double[N_arr];
                    a_sp = new double[N - 1];
                    b_sp = new double[N - 1];
                    c_sp = new double[N - 1];
                    d_sp = new double[N - 1];
                    mat_X = Matrix<double>.Build.Dense(N_arr, N_arr);
                    mat_T = Matrix<double>.Build.Dense(N_arr, N_arr);
                    mat_Y = Matrix<double>.Build.Dense(N_arr, 1);
                    mat_coef_SP = Matrix<double>.Build.Dense(N_arr, 1);
                    for (int i = 0; i < N - 1; i++)
                        mat_X[i, i * 4] = 1;
                    col_i = 0;
                    coef = 0;
                    for (int i = N - 1; i < (N - 1) * 2; i++)
                    {
                        mat_X[i, coef] = 1;
                        mat_X[i, coef + 1] = X_P[col_i + 1] - X_P[col_i];
                        mat_X[i, coef + 2] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                        mat_X[i, coef + 3] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 3);
                        coef = coef + 4;
                        col_i = col_i + 1;
                    }
                    col_i = 0;
                    coef = 1;
                    for (int i = (N - 1) * 2; i < (N - 1) * 3 - 1; i++)
                    {
                        mat_X[i, coef] = 1;
                        mat_X[i, coef + 1] = 2 * (X_P[col_i + 1] - X_P[col_i]);
                        mat_X[i, coef + 2] = 3 * Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                        mat_X[i, coef + 4] = -1;
                        coef = coef + 4;
                        col_i = col_i + 1;
                    }
                    col_i = 0;
                    coef = 2;
                    for (int i = (N - 1) * 3 - 1; i < (N - 1) * 4 - 2; i++)
                    {
                        mat_X[i, coef] = 2;
                        mat_X[i, coef + 1] = 6 * (X_P[col_i + 1] - X_P[col_i]);
                        mat_X[i, coef + 4] = -2;
                        coef = coef + 4;
                        col_i = col_i + 1;
                    }

                    mat_X[N_arr - 2, 2] = 2;
                    mat_X[N_arr - 2, 3] = 6 * (X_P[1] - X_P[0]);
                    mat_X[N_arr - 1, N_arr - 2] = 2;
                    mat_X[N_arr - 1, N_arr - 1] = 6 * (X_P[N - 1] - X_P[N - 2]);

                    for (int i = 0; i < N - 1; i++)
                        mat_Y[i, 0] = Y_P[i];
                    col_i = 1;
                    for (int i = N - 1; i < (N - 1) * 2; i++)
                    {
                        mat_Y[i, 0] = Y_P[col_i];
                        col_i = col_i + 1;
                    }

                    mat_T = mat_X.Inverse();

                    for (int i = 0; i < N_arr; i++)
                        for (int j = 0; j < 1; j++)
                        {
                            mat_coef_SP[i, j] = 0;
                            for (int k = 0; k < N_arr; k++)
                                mat_coef_SP[i, j] += mat_T[i, k] * mat_Y[k, j];
                        }
                    coef = 0;
                    for (int i = 0; i < N - 1; i++)
                    {
                        a_sp[i] = mat_coef_SP[coef, 0];
                        coef += 1;
                        b_sp[i] = mat_coef_SP[coef, 0];
                        coef += 1;
                        c_sp[i] = mat_coef_SP[coef, 0];
                        coef += 1;
                        d_sp[i] = mat_coef_SP[coef, 0];
                        coef += 1;
                    }

                    var list = new List<ObservablePoint>();
                    vm.Series1[numRedLine-1].Values = list;
                    for (int i = 0; i < N; i++)                    
                        list.Add(new ObservablePoint(X_P[i], Y_P[i]));
                    for (int i = 0; i < N_Values; i++)
                     {
                     for (int j = 1; j < N; j++)
                      if (X_values[i] < X_P[j])
                      {
                         nj = j;
                       break;
                     }
                     Y_B = a_sp[nj - 1] + b_sp[nj - 1] * (X_values[i] - X_P[nj - 1]) + c_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 2) + d_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 3);
                     Y_Red.Insert(i, Y_B);
                     if (X_values[i] > X_P[N - 1])
                     {
                     Y_B = a_sp[N - 2] + b_sp[N - 2] * (X_values[i] - X_P[N - 2]) + c_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 2) + d_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 3);
                    Y_Red.Insert(i, Y_B);
                    }
                    }
                     list = new List<ObservablePoint>();
                     vm.Series1[numRedLine].Values = list;
                     for (int i = 0; i < N_Values; i++)
                     list.Add(new ObservablePoint(X_values[i], Y_Red[i]));
                }
            }
        }
        

        
        private void UserBaseline_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Difract)
            {
                for (int i=0;i<2;i++)
                vm.RemoveSeries(vm.Series1, vm.Series1[numRedLine-1]);
                CutBaseline.IsEnabled = false;
            }
        }

        
        private void UserBaseline_Checked(object sender, RoutedEventArgs e)
        {
            if (Difract)
            {
                if (Scale.IsChecked == true)
                    Scale.IsChecked = false;
                
                if (Range.IsChecked == true)
                {
                    Range.IsChecked = false;
                    MouseRange = 0;
                    MouseDown = false;
                    for (int i = 0; i < 2; i++)
                        try
                        {
                            vm.RemoveRect(vm.Sections1[0]);
                        }
                        catch
                        {
                            continue;
                        }
                    DashLine = false;
                    // vm.Sections[1].Xi = -50;
                    // vm.Sections[1].Xj = -50;
                    // vm.Sections[2].Xi = -50;
                    // vm.Sections[2].Xj = -50;
                }

                BS = true;
                N = 10;
                for (int i = 1; i < N_Values / 11; i++)
                {
                    if (Y_Values[i + 1] < Y_Values[i])
                    {
                        Y_min = Y_Values[i + 1];
                        X_min = X_values[i + 1];
                        ii = i + 1;
                        break;
                    }
                }
                for (int i = ii; i < N_Values / 11; i++)
                {
                    if (Y_Values[i + 1] < Y_min)
                    {
                        Y_min = Y_Values[i + 1];
                        X_min = X_values[i + 1];
                    }
                }
                Y_BL[0] = Y_min;
                X_BL[0] = X_min;

                for (int j = 2; j < 11; j++)
                {
                    Y_min = Y_Values[(j - 1) * N_Values / 11];
                    X_min = X_values[(j - 1) * N_Values / 11];
                    for (int i = (j - 1) * N_Values / 11; i < j * N_Values / 11; i++)
                    {
                        if (Y_Values[i + 1] < Y_min)
                        {
                            Y_min = Y_Values[i + 1];
                            X_min = X_values[i + 1];
                        }
                    }
                    Y_BL[j - 1] = Y_min;
                    X_BL[j - 1] = X_min;
                }
                
                X_P = new double[N];
                Y_P = new double[N];
                for (int i=0;i<N;i++)
                {
                    X_P[i] = X_BL[i];
                    Y_P[i] = Y_BL[i];                    
                }

                N_arr = (N - 1) * 4;
                array = new double[N_arr, N_arr];
                array_Y = new double[N_arr];
                a_sp = new double[N - 1];
                b_sp = new double[N - 1];
                c_sp = new double[N - 1];
                d_sp = new double[N - 1];                
                mat_X= Matrix<double>.Build.Dense(N_arr, N_arr);
                mat_T = Matrix<double>.Build.Dense(N_arr, N_arr);
                mat_Y = Matrix<double>.Build.Dense(N_arr, 1);
                mat_coef_SP = Matrix<double>.Build.Dense(N_arr, 1);
                for (int i = 0; i < N-1; i++)
                    mat_X[i, i * 4] = 1;
                col_i = 0;
                coef = 0;
                for (int i=N-1;i<(N-1)*2;i++)
                {
                    mat_X[i, coef] = 1;
                    mat_X[i, coef + 1] = X_P[col_i+1] - X_P[col_i];
                    mat_X[i, coef + 2] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                    mat_X[i, coef + 3] = Math.Pow(X_P[col_i + 1] - X_P[col_i], 3);
                    coef = coef + 4;
                    col_i = col_i + 1;
                }
                col_i = 0;
                coef = 1;
                for (int i= (N - 1) * 2;i< (N - 1) * 3-1;i++)
                {
                    mat_X[i, coef] = 1;
                    mat_X[i, coef + 1] = 2 * (X_P[col_i + 1] - X_P[col_i]);
                    mat_X[i,coef+2]=3* Math.Pow(X_P[col_i + 1] - X_P[col_i], 2);
                    mat_X[i, coef + 4] = -1;
                    coef = coef + 4;
                    col_i = col_i + 1;
                }
                col_i = 0;
                coef = 2;
                for (int i= (N - 1) * 3 - 1;i< (N - 1) * 4 - 2;i++)
                {
                    mat_X[i, coef] = 2;
                    mat_X[i, coef + 1] = 6 * (X_P[col_i + 1] - X_P[col_i]);
                    mat_X[i, coef + 4] = -2;
                    coef = coef + 4;
                    col_i = col_i + 1;
                }

                mat_X[N_arr - 2, 2] = 2;
                mat_X[N_arr - 2, 3] = 6 * (X_P[1] - X_P[0]);
                mat_X[N_arr - 1, N_arr - 2] = 2;
                mat_X[N_arr-1,N_arr-1]= 6 * (X_P[N-1] - X_P[N-2]);

                for (int i = 0; i < N - 1; i++)
                    mat_Y[i,0] = Y_P[i];
                col_i = 1;
                for (int i=N-1;i<(N-1)*2;i++)
                {
                    mat_Y[i,0] = Y_P[col_i];
                    col_i = col_i + 1;
                }

                mat_T = mat_X.Inverse();

                for (int i = 0; i < N_arr; i++)
                    for (int j = 0; j < 1; j++)
                    {
                        mat_coef_SP[i, j] = 0;
                        for (int k = 0; k < N_arr; k++)
                            mat_coef_SP[i, j] += mat_T[i, k] * mat_Y[k, j];
                    }
                coef = 0;
                for (int i=0;i<N-1;i++)
                {
                    a_sp[i] = mat_coef_SP[coef, 0];
                    coef += 1;
                    b_sp[i]= mat_coef_SP[coef, 0];
                    coef += 1;
                    c_sp[i]= mat_coef_SP[coef, 0];
                    coef += 1;
                    d_sp[i]= mat_coef_SP[coef, 0];
                    coef += 1;
                }
                


                //double[] p = Fit.Polynomial(X_P, Y_P, 6);
                //a6 = p[6];
                //a5 = p[5];
                //a4 = p[4];
                //a3 = p[3];
                //a2 = p[2];
                //a1 = p[1];
                //a0 = p[0];

                 var list = new List<ObservablePoint>();

                 for (int i = 0; i < 10; i++)
                   list.Add(new ObservablePoint(X_P[i], Y_P[i]));

                vm.AddNewScatterSeries(list);

                for (int i = 0; i < N_Values; i++)
                 {
                    for (int j = 1; j < N; j++)
                        if (X_values[i] < X_P[j])
                        {
                            nj = j;
                            break;
                        }
                    Y_B = a_sp[nj - 1] + b_sp[nj - 1] * (X_values[i] - X_P[nj - 1]) + c_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 2) + d_sp[nj - 1] * Math.Pow(X_values[i] - X_P[nj - 1], 3);
                  Y_Red.Insert(i, Y_B);
                    if (X_values[i]>X_P[N-1])
                    {
                        Y_B = a_sp[N - 2] + b_sp[N - 2] * (X_values[i] - X_P[N - 2]) + c_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 2) + d_sp[N - 2] * Math.Pow(X_values[i] - X_P[N - 2], 3);
                        Y_Red.Insert(i, Y_B);
                    }
                 }
                 list = new List<ObservablePoint>();
                 for (int i = 0; i < N_Values; i++)
                   list.Add(new ObservablePoint(X_values[i], Y_Red[i]));
                vm.AddNewLineSeries(list, 4, vm.Series1, vm.Series1.Count);
                numRedLine = vm.Series1.Count - 1;
                CutBaseline.IsEnabled = true;
            }
            
        }


        private void MainBaseline_Click(object sender, RoutedEventArgs e)
        {
            if (Difract)
            {
                if (UserBaseline.IsChecked == true)
                    UserBaseline.IsChecked = false;
                BS = true;
                Y_P = new double[10];
                X_P = new double[10];
                for (int i = 1; i < N_Values / 11; i++)
                {
                    if (Y_Values[i + 1] < Y_Values[i])
                    {
                        Y_min = Y_Values[i + 1];
                        X_min = X_values[i + 1];
                        ii = i + 1;
                        break;
                    }
                }
                for (int i = ii; i < N_Values / 11; i++)
                {
                    if (Y_Values[i + 1] < Y_min)
                    {
                        Y_min = Y_Values[i + 1];
                        X_min = X_values[i + 1];
                    }
                }
                Y_P[0] = Y_min;
                X_P[0] = X_min;
                
                 for (int j = 2; j < 11; j++)
                 {
                   Y_min = Y_Values[(j - 1) * N_Values / 11];
                   X_min = X_values[(j - 1) * N_Values / 11];
                   for (int i = (j - 1) * N_Values / 11; i < j * N_Values / 11; i++)
                  {
                    if (Y_Values[i + 1] < Y_min)
                   {
                     Y_min = Y_Values[i + 1];
                   X_min = X_values[i + 1];
                  }
                 }
                  Y_P[j - 1] = Y_min;
                  X_P[j - 1] = X_min;
                 }
                               
                double [] p = Fit.Polynomial(X_P, Y_P, 3);                
                a3 = p[3];
                a2 = p[2];
                a1 = p[1];
                a0 = p[0];

                for (int i=0; i<N_Values; i++)  
                {
                    Y_B = a3 * Math.Pow(X_values[i], 3) + a2 * Math.Pow(X_values[i], 2) + a1 * X_values[i] + a0;                    
                    Y_Baseline.Insert(i, Y_Values[i]- Y_B);                  
                }
                
                var list = new List<ObservablePoint>();

                if (vs > 0)
                {
                    vm.Series1[numOfLeftLine].Values = list;
                    vm.Series2[1].Values = list;

                    for (int i = 0; i < vs; i++)
                    {
                        Y_Values.Insert(i, Y_Baseline[i]);
                        list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                    }

                    list = new List<ObservablePoint>();
                    vm.Series1[numOfRightLine].Values = list;
                    vm.Series2[2].Values = list;
                    for (int i = ve + 1; i < N_Values; i++)
                    {
                        Y_Values.Insert(i, Y_Baseline[i]);
                        list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                    }

                }

                list = new List<ObservablePoint>();
                vm.Series1[0].Values = list;
                vm.Series2[0].Values = list;

                for (int i = vs; i <= ve; i++)
                {
                    Y_Values.Insert(i, Y_Baseline[i]);
                    list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                }

                

                TextBlockStatus.Text = "Идет выполнение...";
                
            }

        }

        

        

        private void Range_Click(object sender, RoutedEventArgs e)
        {
            if (Difract)
            {
                if (Scale.IsChecked == true)
                    Scale.IsChecked = false;
                if (Range.IsChecked == false)
                {
                    MouseRange = 0;
                    MouseDown = false;
                    for (int i=0;i<2;i++)
                        try
                        {
                            vm.RemoveRect(vm.Sections1[0]);
                        }
                        catch
                        {
                            continue;
                        }
                    DashLine = false;
                    //vm.Sections[1].Xi = -50;
                    //vm.Sections[1].Xj = -50;
                    //vm.Sections[2].Xi = -50;
                    //vm.Sections[2].Xj = -50;
                }
                if (UserBaseline.IsChecked == true)
                    UserBaseline.IsChecked = false;
            }
        }

        private void Scale_Click(object sender, RoutedEventArgs e)
        {
            if (Range.IsChecked == true)
            {
                Range.IsChecked = false;
                MouseRange = 0;
                MouseDown = false;
                if (DashLine)
                {
                    for (int i = 0; i < 2; i++)
                        try
                        {
                            vm.RemoveRect(vm.Sections1[0]);
                        }
                        catch
                        {
                            continue;
                        }
                    DashLine = false;
                }
                //vm.Sections[1].Xi = -50;
                //vm.Sections[1].Xj = -50;
                //vm.Sections[2].Xi = -50;
                //vm.Sections[2].Xj = -50;
            }
            if (UserBaseline.IsChecked == true)
                UserBaseline.IsChecked = false;
        }

        private void ButForCommon_Click(object sender, RoutedEventArgs e)
        {
            ColorButForCommon = true;
            OpenColorWindow(numSumLine);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Difract)
            {
                vm.XAxes[0].MinLimit = X_values[0];
                vm.XAxes[0].MaxLimit = X_values[N_Values - 1];
                vm.XAxes[0].MinStep = 20;
                vm.YAxes[0].MaxLimit = null;
                vm.YAxes[0].MinLimit = 0;
                vm.XAxes2[0].MinLimit = X_values[0];
                vm.XAxes2[0].MaxLimit = X_values[N_Values - 1];
            }
        }


        private void MyChart_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (Difract)
            {
                var chart = (LiveChartsCore.SkiaSharpView.WPF.CartesianChart)sender;
                var mouseCoordinate = e.GetPosition(chart);
                var scaledPoint = chart.ScalePixelsToData(new LvcPointD(mouseCoordinate.X, mouseCoordinate.Y));                
                MouseMove_X = scaledPoint.X;
                MouseMove_Y = scaledPoint.Y;
                       if (MouseMove_X > vm.XAxes[0].MinLimit && MouseMove_X < vm.XAxes[0].MaxLimit)
                {
                    theta.Text = ("2Θ:  " + Math.Round(MouseMove_X, 2) + "°").ToString();
                    interDist.Text = ("d:  " + Math.Round(lamdaKa1 / (2 * Math.Sin(MouseMove_X / 2 * Math.PI / 180)), 3) + " Å").ToString();
                }

                if (Scale.IsChecked == true && MouseDown == true)
                {
                    if (!RectSect)
                    {
                        vm.AddNewRectangularSection(MouseDown_X, MouseMove_X, MouseDown_Y, MouseMove_Y);
                        RectSect = true;
                    }
                    else
                    {
                        vm.Sections1[0].Xi = MouseDown_X;
                        vm.Sections1[0].Xj = MouseMove_X;
                        vm.Sections1[0].Yi = MouseDown_Y;
                        vm.Sections1[0].Yj = MouseMove_Y;
                    }
                }

                if (Range.IsChecked==true)
                {
                    if (!DashLine)
                    {
                        vm.AddDashLine(MouseMove_X, MouseMove_X);
                        DashLine = true;
                    }
                    else
                    {
                        vm.Sections1[vm.Sections1.Count-1].Xi = MouseMove_X;
                        vm.Sections1[vm.Sections1.Count - 1].Xj = MouseMove_X;
                    }
                    
                    
                }
                if (UserBaseline.IsChecked==true)
                {

                    for (int i = 0; i < N; i++)
                    {
                        CursorCross = false;
                        var scaledPixel = chart.ScaleDataToPixels(new LvcPointD(X_P[i], Y_P[i]));
                        X_BL_pix.Insert(i, scaledPixel.X);
                        Y_BL_pix.Insert(i, scaledPixel.Y);
                        if (Math.Pow(mouseCoordinate.X-X_BL_pix[i],2)+Math.Pow(mouseCoordinate.Y-Y_BL_pix[i],2)<64)
                        {
                            MyChart.Cursor = Cursors.Cross;
                            CursorCross = true;
                            break;
                        }
                        
                    }
                    if (!CursorCross)
                        MyChart.Cursor = Cursors.Arrow;


                    if (CaptPoint)
                    {

                        X_P[ii] = Math.Round(scaledPoint.X, 2);
                        Y_P[ii]= scaledPoint.Y;

                        for (int i = 0; i < N; i++)
                            for (int j = 0; j < N - 1; j++)
                            {
                                if (X_P[j + 1] < X_P[j])
                                {
                                    double p_X = X_P[j + 1];
                                    X_P[j + 1] = X_P[j];
                                    X_P[j] = p_X;
                                    double p_Y = Y_P[j + 1];
                                    Y_P[j + 1] = Y_P[j];
                                    Y_P[j] = p_Y;
                                }
                            }

                        var list = new List<ObservablePoint>();
                        vm.Series1[numRedLine-1].Values = list;
                        for (int i = 0; i < N; i++)
                        {
                            list.Add(new ObservablePoint(X_P[i], Y_P[i]));
                            X_BL[i] = X_P[i];
                            Y_BL[i] = Y_P[i];
                        }
                       
                    }
                    
                }


            }
        }


        private void MyChart_UpdateFinished(LiveChartsCore.Kernel.Sketches.IChartView<LiveChartsCore.SkiaSharpView.Drawing.SkiaSharpDrawingContext> chart)
        {
            if (ButDisplay)
            {
                TextBlockStatus.Text = FN;                
                ButDisplay = false;
                Difract = true;
            }
            if (But_range)
            {
                TextBlockStatus.Text = FN;
                But_range = false;
            }
            if (BS)
            {
                TextBlockStatus.Text = FN;
                BS = false;
            }

        }

        private void RemoveExpander(int col_line)
        {
            if (ExpanderName.TryGetValue("Expander" + col_line, out Expander expander))
            {
                StackPanelForExpander.Children.Remove(expander);
                ExpanderName.Remove(expander.Name);
            }
        }

        private void DrawExpander(int parnum)
        {
            Expander expander = new Expander
            {
                Header = "Линия " + (parnum + 1),
                Name = "Expander" + (parnum + 1),
                Style = (Style)FindResource("MyCoolExpanderStyle"),
                Tag = parnum
            };
            StackPanelForExpander.Children.Add(expander);
            ExpanderName.Add(expander.Name, expander);

            StackPanel sp = new StackPanel
            {
                Orientation=Orientation.Vertical
            };
            expander.Content = sp;

            DockPanel DP_square = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)                
            };
            sp.Children.Add(DP_square);

            TextBlock textBlock_s = new TextBlock
            {
                Width=30,
                HorizontalAlignment=HorizontalAlignment.Center,
                TextAlignment=TextAlignment.Center,
                Margin=new Thickness(2),
                ToolTip="Площадь",
                Text="S:"
            };
            DP_square.Children.Add(textBlock_s);

            TextBox textBox_s = new TextBox
            {
                Width=45,
                Height=22,
                BorderThickness=new Thickness(1),
                BorderBrush=Brushes.Silver,
                TextAlignment=TextAlignment.Center,
                //VerticalContentAlignment=VerticalAlignment.Center,
                VerticalAlignment=VerticalAlignment.Center,
                Margin=new Thickness(2,0,2,0),
                Text="0.1240",
                Tag=parnum
            };
            DP_square.Children.Add(textBox_s);
            textBox_s_num[parnum] = textBox_s;
            textBox_s.PreviewKeyDown += TextBox_s_PreviewKeyDown;
            textBox_s.PreviewTextInput += TextBox_s_PreviewTextInput;
            CommandBinding textBox_s_pasteCommandBinding = new CommandBinding(ApplicationCommands.Paste);
            textBox_s_pasteCommandBinding.Executed += TextBox_s_pasteCommandBinding_Executed;
            textBox_s.CommandBindings.Add(textBox_s_pasteCommandBinding);
            textBox_s.PreviewDragOver += TextBox_s_PreviewDragOver;
            textBox_s.Drop += TextBox_s_Drop;
            textBox_s.TextChanged += TextBox_s_TextChanged;
            textBox_s.PreviewGotKeyboardFocus += TextBox_s_PreviewGotKeyboardFocus;
            textBox_s.PreviewLostKeyboardFocus += TextBox_s_PreviewLostKeyboardFocus;

            Button button_s = new Button
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag=parnum
            };
            Image image_s = new Image();
            image_s.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(image_s, BitmapScalingMode.HighQuality);
            button_s.Content = image_s;
            DP_square.Children.Add(button_s);
            button_s.Click += Button_s_Click;

            Slider slider_s = new Slider
            {
                Minimum = -100,
                Maximum=100,
                Value=0,
                Margin=new Thickness(2),
                Height=20,
                VerticalAlignment=VerticalAlignment.Center,
                Style=(Style)FindResource("CustomSliderStyle"),
                Tag=par_num
            };
            DP_square.Children.Add(slider_s);
            slider_s_num[parnum] = slider_s;
            slider_s.ValueChanged += Slider_s_ValueChanged;
            slider_s.PreviewMouseLeftButtonDown += Slider_s_PreviewMouseLeftButtonDown;
            slider_s.PreviewMouseLeftButtonUp += Slider_s_PreviewMouseLeftButtonUp;
            slider_s.PreviewKeyDown += Slider_s_PreviewKeyDown;

            DockPanel DP_center = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            sp.Children.Add(DP_center);

            TextBlock textBlock_c = new TextBlock
            {
                Width = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2),
                ToolTip = "Положение максимума",
                Text = "2Θ:"
            };
            DP_center.Children.Add(textBlock_c);

            TextBox textBox_c = new TextBox
            {
                Width = 45,
                Height = 22,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Silver,
                TextAlignment = TextAlignment.Center,
                //VerticalContentAlignment=VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 0, 2, 0),
                Text = "0.1240",
                Tag = parnum
            };
            DP_center.Children.Add(textBox_c);
            textBox_c_num[parnum] = textBox_c;
            textBox_c.PreviewKeyDown += TextBox_c_PreviewKeyDown;
            textBox_c.PreviewTextInput += TextBox_c_PreviewTextInput;
            CommandBinding textBox_c_pasteCommandBinding = new CommandBinding(ApplicationCommands.Paste);
            textBox_c_pasteCommandBinding.Executed += TextBox_c_pasteCommandBinding_Executed;
            textBox_c.CommandBindings.Add(textBox_c_pasteCommandBinding);
            textBox_c.PreviewDragOver += TextBox_c_PreviewDragOver;
            textBox_c.Drop += TextBox_c_Drop;
            textBox_c.TextChanged += TextBox_c_TextChanged;
            textBox_c.PreviewGotKeyboardFocus += TextBox_c_PreviewGotKeyboardFocus;
            textBox_c.PreviewLostKeyboardFocus += TextBox_c_PreviewLostKeyboardFocus;

            Button button_c = new Button
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag=parnum
            };
            Image image_c = new Image();
            image_c.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(image_c, BitmapScalingMode.HighQuality);
            button_c.Content = image_c;
            DP_center.Children.Add(button_c);
            button_c.Click += Button_c_Click;

            Slider slider_c = new Slider
            {
                Minimum = -100,
                Maximum = 100,
                Margin = new Thickness(2),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)FindResource("CustomSliderStyle"),
                Tag=parnum                
            };
            DP_center.Children.Add(slider_c);
            slider_c_num[parnum] = slider_c;
            slider_c.ValueChanged += Slider_c_ValueChanged;
            slider_c.PreviewMouseLeftButtonDown += Slider_c_PreviewMouseLeftButtonDown;
            slider_c.PreviewMouseLeftButtonUp += Slider_c_PreviewMouseLeftButtonUp;
            slider_c.PreviewKeyDown += Slider_c_PreviewKeyDown;

            DockPanel DP_width = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            sp.Children.Add(DP_width);

            TextBlock textBlock_w = new TextBlock
            {
                Width = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2),
                ToolTip = "Ширина",
                Text = "σ:"
            };
            DP_width.Children.Add(textBlock_w);

            TextBox textBox_w = new TextBox
            {
                Width = 45,
                Height = 22,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Silver,
                TextAlignment = TextAlignment.Center,
                //VerticalContentAlignment=VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 0, 2, 0),
                Text = "0.1240",
                Tag = parnum
            };
            DP_width.Children.Add(textBox_w);
            textBox_w_num[parnum] = textBox_w;
            textBox_w.PreviewKeyDown += TextBox_w_PreviewKeyDown;
            textBox_w.PreviewTextInput += TextBox_w_PreviewTextInput;
            CommandBinding textBox_w_pasteCommanBinding = new CommandBinding(ApplicationCommands.Paste);
            textBox_w_pasteCommanBinding.Executed += TextBox_w_pasteCommanBinding_Executed;
            textBox_w.CommandBindings.Add(textBox_w_pasteCommanBinding);
            textBox_w.PreviewDragOver += TextBox_w_PreviewDragOver;
            textBox_w.Drop += TextBox_w_Drop;
            textBox_w.TextChanged += TextBox_w_TextChanged;
            textBox_w.PreviewGotKeyboardFocus += TextBox_w_PreviewGotKeyboardFocus;
            textBox_w.PreviewLostKeyboardFocus += TextBox_w_PreviewLostKeyboardFocus;

            Button button_w = new Button
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag=parnum
            };
            Image image_w = new Image();
            image_w.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(image_w, BitmapScalingMode.HighQuality);
            button_w.Content = image_w;
            DP_width.Children.Add(button_w);
            button_w.Click += Button_w_Click;

            Slider slider_w = new Slider
            {
                Minimum = -100,
                Maximum = 100,
                Margin = new Thickness(2),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)FindResource("CustomSliderStyle"),
                Tag=parnum
            };
            DP_width.Children.Add(slider_w);
            slider_w_num[parnum] = slider_w;
            slider_w.ValueChanged += Slider_w_ValueChanged;
            slider_w.PreviewMouseLeftButtonDown += Slider_w_PreviewMouseLeftButtonDown;
            slider_w.PreviewMouseLeftButtonUp += Slider_w_PreviewMouseLeftButtonUp;
            slider_w.PreviewKeyDown += Slider_w_PreviewKeyDown;

            DockPanel DP_shape = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            sp.Children.Add(DP_shape);

            TextBlock textBlock_sh = new TextBlock
            {
                Width = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2),
                ToolTip = "Форма",
                Text = "μ:"
            };
            DP_shape.Children.Add(textBlock_sh);

            TextBox textBox_sh = new TextBox
            {
                Width = 45,
                Height = 22,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Silver,
                TextAlignment = TextAlignment.Center,
                //VerticalContentAlignment=VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 0, 2, 0),
                Text = "0.1240",
                Tag = parnum
            };
            DP_shape.Children.Add(textBox_sh);
            textBox_sh_num[parnum] = textBox_sh;
            textBox_sh.PreviewKeyDown += TextBox_sh_PreviewKeyDown;
            textBox_sh.PreviewTextInput += TextBox_sh_PreviewTextInput;
            CommandBinding pasteCommandBinding = new CommandBinding(ApplicationCommands.Paste);
            pasteCommandBinding.Executed += PasteCommandBinding_Executed;
            textBox_sh.CommandBindings.Add(pasteCommandBinding);
            textBox_sh.PreviewDragOver += TextBox_sh_PreviewDragOver;
            textBox_sh.Drop += TextBox_sh_Drop;
            textBox_sh.TextChanged += TextBox_sh_TextChanged;
            textBox_sh.PreviewGotKeyboardFocus += TextBox_sh_PreviewGotKeyboardFocus;
            textBox_sh.PreviewLostKeyboardFocus += TextBox_sh_PreviewLostKeyboardFocus;

            Button button_sh = new Button
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag=parnum
            };
            Image image_sh = new Image();
            image_sh.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(image_sh, BitmapScalingMode.HighQuality);
            button_sh.Content = image_sh;
            DP_shape.Children.Add(button_sh);
            button_sh.Click += Button_sh_Click;

            Slider slider_sh = new Slider
            {
                Minimum = -100,
                Maximum = 100,
                Margin = new Thickness(2),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)FindResource("CustomSliderStyle"),
                Tag=parnum
            };
            DP_shape.Children.Add(slider_sh);
            slider_sh_num[parnum] = slider_sh;
            slider_sh.ValueChanged += Slider_sh_ValueChanged;
            slider_sh.PreviewMouseLeftButtonDown += Slider_sh_PreviewMouseLeftButtonDown;
            slider_sh.PreviewMouseLeftButtonUp += Slider_sh_PreviewMouseLeftButtonUp;
            slider_sh.PreviewKeyDown += Slider_sh_PreviewKeyDown;

            DockPanel DP_sep = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            sp.Children.Add(DP_sep);
            
            Separator separator = new Separator
            {
                Margin=new Thickness(0,5,0,5),                
                Background=Brushes.Silver               
            };
            DP_sep.Children.Add(separator);

            //DockPanel DP_iteratom_dist = new DockPanel
            //{
            //  Margin = new Thickness(0, 0, 0, 5)
            //};
            //sp.Children.Add(DP_iteratom_dist);

            StackPanel SP_iteratom_dist = new StackPanel
            {
                Orientation=Orientation.Horizontal,
                Margin= new Thickness(0, 0, 0, 5)
            };
            sp.Children.Add(SP_iteratom_dist);

            TextBlock textBlock_dist = new TextBlock
            {
                Width = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2),
                ToolTip = "Межплоскостное расстояние",
                Text = "d:"
            };
            SP_iteratom_dist.Children.Add(textBlock_dist);
            
            TextBox textBox_dist = new TextBox
            {
                Width = 45,
                Height = 22,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Silver,
                TextAlignment = TextAlignment.Center,
                IsReadOnly=true,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 0, 2, 0),
                Text = "0.1240",
                Tag = parnum
            };
            SP_iteratom_dist.Children.Add(textBox_dist);
            textBox_dist_num[parnum] = textBox_dist;

            Separator sepAfterDist = new Separator
            {
                Margin = new Thickness(25,0,25,0),
                Background=Brushes.Silver,
                LayoutTransform=new RotateTransform(90)
            };
            SP_iteratom_dist.Children.Add(sepAfterDist);

            CheckBox checkBox = new CheckBox
            {
                Margin = new Thickness(0,0,0,0),
                Tag=parnum,
                IsChecked=true,
                VerticalAlignment=VerticalAlignment.Center
            };
            SP_iteratom_dist.Children.Add(checkBox);
            checkBox_num[parnum] = checkBox;
            checkBox.Click += CheckBox_Click;

            TextBlock textBlock_Ka2 = new TextBlock
            {
                Width = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2),
                ToolTip = "Отображение Кα2 линий",
                Text = "Кα2"
            };
            SP_iteratom_dist.Children.Add(textBlock_Ka2);
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox clickedCheckBox)
            {
                int checkBoxButIndex = (int)clickedCheckBox.Tag;
                if (clickedCheckBox.IsChecked == true)
                {
                    vm.Series1[num2[checkBoxButIndex]].IsVisible = true;
                    vm.Series1[num3[checkBoxButIndex]].IsVisible = true;
                }
                else
                {
                    vm.Series1[num2[checkBoxButIndex]].IsVisible = false;
                    vm.Series1[num3[checkBoxButIndex]].IsVisible = false;
                }
            }
        }


        private void Button_s_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int parnum = (int)button.Tag;
                ++s_click[parnum];
                switch (s_click[parnum])
                {
                    case 1:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Red-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_s_num[parnum].IsEnabled = false;
                            slider_s_num[parnum].IsEnabled = false;
                        }
                        break;

                    case 2:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_s_num[parnum].IsEnabled = true;
                            slider_s_num[parnum].IsEnabled = true;
                            s_click[parnum] = 0;
                        }
                        break;
                }

            }
        }

        private void Button_c_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int parnum = (int)button.Tag;
                ++c_click[parnum];
                switch (c_click[parnum])
                {
                    case 1:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Red-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_c_num[parnum].IsEnabled = false;
                            slider_c_num[parnum].IsEnabled = false;
                        }
                        break;

                    case 2:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_c_num[parnum].IsEnabled = true;
                            slider_c_num[parnum].IsEnabled = true;
                            c_click[parnum] = 0;
                        }
                        break;
                }

            }
        }

        private void Button_w_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int parnum = (int)button.Tag;
                ++w_click[parnum];
                switch (w_click[parnum])
                {
                    case 1:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Red-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_w_num[parnum].IsEnabled = false;
                            slider_w_num[parnum].IsEnabled = false;
                        }
                        break;

                    case 2:
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            button.Content = image;
                            textBox_w_num[parnum].IsEnabled = true;
                            slider_w_num[parnum].IsEnabled = true;
                            w_click[parnum] = 0;
                        }
                        break;
                }

            }
        }

        private void Button_sh_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int parnum = (int)button.Tag;
                  ++sh_click[parnum];                    
                      switch(sh_click[parnum])
                    {
                      case 1:
                        {
                            Image image_sh = new Image();
                            image_sh.Source = new BitmapImage(new Uri("Red-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image_sh, BitmapScalingMode.HighQuality);
                            button.Content = image_sh;
                            textBox_sh_num[parnum].IsEnabled = false;
                            slider_sh_num[parnum].IsEnabled = false;
                        }
                        break;

                    case 2:
                        {
                            Image image_sh = new Image();
                            image_sh.Source = new BitmapImage(new Uri("Green-lock.png", UriKind.Relative));
                            RenderOptions.SetBitmapScalingMode(image_sh, BitmapScalingMode.HighQuality);
                            button.Content = image_sh;
                            textBox_sh_num[parnum].IsEnabled = true;
                            slider_sh_num[parnum].IsEnabled = true;
                            sh_click[parnum] = 0;
                        }
                        break;
                    }
                    
            }

        }

        private void Slider_sh_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Slider slider)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        if (slider.Value > slider.Minimum)
                        {
                            MD_slider = true;
                            addPar = par_mu_final[(int)slider.Tag];
                            slider.Value -= 0.1; // Adjust decrement as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;

                    case Key.Right:
                        if (slider.Value < slider.Maximum)
                        {
                            MD_slider = true;
                            addPar = par_mu_final[(int)slider.Tag];
                            slider.Value += 0.1; // Adjust increment as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;
                }
            }
        }

        private void Slider_sh_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
            {
                MD_slider = false;
                slider.Value = 0;
            }
        }

        private void Slider_sh_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
                addPar = par_mu_final[(int)slider.Tag];
            MD_slider = true;
        }

        private void Slider_sh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider)
            {
                if (!MD_slider)
                {
                    slider.Value = 0;
                    addPar = par_mu_final[par_num];
                }
                else
                {
                    par_num = (int)slider.Tag;
                    par_mu_final[par_num] = addPar + (double)slider.Value * 0.01;
                    if (par_mu_final[par_num] < 0)
                        par_mu_final[par_num] = 0;
                    if (par_mu_final[par_num] > 1)
                        par_mu_final[par_num] = 1;
                    textBox_sh_num[par_num].Text = par_mu_final[par_num].ToString("0.000");                    
                    DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                    DrawingSumLine();
                }
            }
        }

        private void TextBox_sh_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = false;
        }

        private void TextBox_sh_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = true;
        }

        private void TextBox_sh_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && TB_focus)
            {
                par_num = (int)textBox.Tag;
                if (double.Parse(textBox.Text) < 0)
                    textBox.Text = 0.ToString();
                if (double.Parse(textBox.Text) > 1)
                    textBox.Text = 1.ToString();
                par_mu_final[par_num] = double.Parse(textBox.Text);
                DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                DrawingSumLine();
            }
        }

        private void Slider_w_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Slider slider)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        if (slider.Value > slider.Minimum)
                        {
                            MD_slider = true;
                            addPar = par_par_b[(int)slider.Tag];
                            slider.Value -= 1; // Adjust decrement as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;

                    case Key.Right:
                        if (slider.Value < slider.Maximum)
                        {
                            MD_slider = true;
                            addPar = par_par_b[(int)slider.Tag];
                            slider.Value += 1; // Adjust increment as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;
                }
            }
        }

        private void Slider_w_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
            {
                MD_slider = false;
                slider.Value = 0;
            }
        }

        private void Slider_w_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
                addPar = par_par_b[(int)slider.Tag];
            MD_slider = true;
        }

        private void Slider_w_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider)
            {
                if (!MD_slider)
                {
                    slider.Value = 0;
                    addPar = par_par_b[par_num];
                }
                else
                {
                    par_num = (int)slider.Tag;
                    par_par_b[par_num] = addPar + (double)slider.Value * 0.001;
                    if (par_par_b[par_num] < 0)
                        par_par_b[par_num] = 0;
                    textBox_w_num[par_num].Text = par_par_b[par_num].ToString("0.000");
                    DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                    DrawingSumLine();
                }
            }
        }

        private void TextBox_w_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = false;
        }

        private void TextBox_w_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = true;
        }

        private void TextBox_w_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && TB_focus)
            {
                par_num = (int)textBox.Tag;
                par_par_b[par_num] = double.Parse(textBox.Text);
                DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                DrawingSumLine();
            }
        }

        private void Slider_c_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Slider slider)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        if (slider.Value > slider.Minimum)
                        {
                            MD_slider = true;
                            addPar = par_x1[(int)slider.Tag];
                            slider.Value -= 1; // Adjust decrement as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;

                    case Key.Right:
                        if (slider.Value < slider.Maximum)
                        {
                            MD_slider = true;
                            addPar = par_x1[(int)slider.Tag];
                            slider.Value += 1; // Adjust increment as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;
                }
            }
        }

        private void Slider_c_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
            {
                MD_slider = false;
                slider.Value = 0;
            }
        }

        private void Slider_c_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
                addPar = par_x1[(int)slider.Tag];
            MD_slider = true;
        }

        private void Slider_c_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider)
            {
                if (!MD_slider)
                {
                    slider.Value = 0;
                    addPar = par_x1[par_num];
                }
                else
                {
                    par_num = (int)slider.Tag;
                    par_x1[par_num] = addPar + (double)slider.Value*0.01;
                    if (par_x1[par_num] < 0)
                        par_x1[par_num] = 0;
                    textBox_c_num[par_num].Text = par_x1[par_num].ToString("0.00");
                    textBox_dist_num[par_num].Text = Math.Round(lamdaKa1 / (2 * Math.Sin(par_x1[par_num] / 2 * Math.PI / 180)), 3).ToString();
                    DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                    DrawingSumLine();
                }
            }
        }

        private void TextBox_c_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = false;
        }

        private void TextBox_c_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = true;
        }

        private void TextBox_c_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && TB_focus)
            {
                par_num = (int)textBox.Tag;
                par_x1[par_num] = double.Parse(textBox.Text);
                textBox_dist_num[par_num].Text = Math.Round(lamdaKa1 / (2 * Math.Sin(par_x1[par_num] / 2 * Math.PI / 180)), 3).ToString();
                DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                DrawingSumLine();
            }
        }

        private void Slider_s_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Slider slider)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        if (slider.Value > slider.Minimum)
                        {
                            MD_slider = true;
                            addPar = par_Sv_final[(int)slider.Tag];
                            slider.Value -= 0.01; // Adjust decrement as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;

                    case Key.Right:
                        if (slider.Value < slider.Maximum)
                        {
                            MD_slider = true;
                            addPar = par_Sv_final[(int)slider.Tag];
                            slider.Value += 0.01; // Adjust increment as needed
                            MD_slider = false;
                            slider.Value = 0;
                        }
                        break;
                }
            }
        }
      

        private void TextBox_s_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = false;
        }

        private void TextBox_s_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TB_focus = true;
        }

        private void TextBox_s_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && TB_focus)
            {
                par_num = (int)textBox.Tag;
                par_Sv_final[par_num] = double.Parse(textBox.Text);
                DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                DrawingSumLine();
            }    
        }

        private void Slider_s_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
            {
                MD_slider = false;
                slider.Value = 0; 
            }
        }

        private void Slider_s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
                addPar = par_Sv_final[(int)slider.Tag];
            MD_slider = true;
        }
        

        private void Slider_s_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider)
            {
                if (!MD_slider)
                {
                    slider.Value = 0;
                    addPar = par_Sv_final[par_num];
                }
                else
                {
                        par_num = (int)slider.Tag;                    
                        par_Sv_final[par_num] = addPar + (double)slider.Value;
                        if (par_Sv_final[par_num] < 0)
                            par_Sv_final[par_num] = 0;
                        textBox_s_num[par_num].Text = par_Sv_final[par_num].ToString("0.00");
                        DrawingLine(num1[par_num], num2[par_num], num3[par_num], par_num);
                        DrawingSumLine();         
                }
            }
        }

        private void TextBox_s_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_s_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_s_pasteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_s_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            PreviewTextInput(sender, e);
        }

        private void TextBox_s_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PreviewKeyDown(sender, e);
        }

        private void TextBox_c_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_c_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_c_pasteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_c_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            PreviewTextInput(sender, e);
        }

        private void TextBox_c_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PreviewKeyDown(sender, e);
        }

        private void TextBox_w_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true; 
        }

        private void TextBox_w_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_w_pasteCommanBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_w_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            PreviewTextInput(sender, e);
        }

        private void TextBox_w_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PreviewKeyDown(sender, e);
        }

        private void TextBox_sh_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_sh_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void PasteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_sh_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            PreviewTextInput(sender, e);
        }

        private new void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!char.IsDigit(e.Text, 0) && (e.Text != ","))
            {
                e.Handled = true;
            }

            // Allow only one comma
            if (e.Text == "," && textBox.Text.Contains(","))
            {
                e.Handled = true; // Disallow input if a comma already exists
            }
        }

        private void TextBox_sh_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PreviewKeyDown(sender, e);
        }

        public void DrawGroupBox(int parnum)
        {
            parnum += 1;
            
            GroupBox gb = new GroupBox
            {
                Name = "GB_" + parnum,
                Header = "Линия " + parnum,
                Height = 45,
                Margin = new Thickness(1, 0, 1, 0)
            };
            StackPanelForLines.Children.Add(gb);
            //if (!GBNames.TryGetValue("GB_" + parnum, out GroupBox groupBox))
            GBNames.Add(gb.Name, gb);


            StackPanel sp = new StackPanel
            {
             Name = "SP_" + parnum,
             Orientation = Orientation.Horizontal   
            };

            gb.Content = sp;

            RadioButton radioButton = new RadioButton
            {
                Name = "RadioBut" + parnum,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = parnum-1                
            };           
            radioButton.Click += RadioButton_Click;
            sp.Children.Add(radioButton);
            //if (!RadioButNames.TryGetValue("RadioBut" + parnum, out RadioButton radioBut))
            RadioButNames.Add(radioButton.Name, radioButton);

            Button ColorLine = new Button
            {
                Name = "ColorLine" + parnum,
                Width = 20,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Background = brushes[parnum - 1],
                Margin = new Thickness(4, 0, 2, 0),
                ToolTip = "Изменить цвет",
                Tag=parnum-1,                
            };
            sp.Children.Add(ColorLine);
            ColorLine.Click += ColorLine_Click;
            colorLine_But[parnum - 1] = ColorLine;

            Button AddLine = new Button
            {
                Name= "AddLine" + parnum,
                Width=98,
                Margin=new Thickness(10, 0, 2, 0),
                Content="Добавить",
                Tag=parnum-1
            };
            AddLine.Click += AddLine_Click;
            sp.Children.Add(AddLine);
            //if (!AddLineNames.TryGetValue("AddLine" + parnum , out Button addLine))
            AddLineNames.Add(AddLine.Name, AddLine);

            Button DelLine = new Button
            {
                Name = "DelLine" + parnum,
                Width = 98,
                Margin = new Thickness(10, 0, 2, 0),
                Visibility = Visibility.Hidden,
                Content = "Удалить",
                Tag = parnum-1
            };
            DelLine.Click += DelLine_Click;
            sp.Children.Add(DelLine);
            //if (!DelLineNames.TryGetValue("DelLine" + parnum, out Button delLine))
            DelLineNames.Add(DelLine.Name, DelLine);

            Button ParLine = new Button
            {
                Name = "ParLine" + parnum,
                Width = 13,
                Margin = new Thickness(8, 0, 2, 0),
                Visibility=Visibility.Hidden,
                Tag=parnum-1,
                ToolTip="Параметры функции"
            };
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("три-точки-редак.png", UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            ParLine.Content = image;
            ParLine.Click += ParLine_Click;
            sp.Children.Add(ParLine);
            //if (!ParLineNames.TryGetValue("ParLine" + parnum, out Button parLine))
            ParLineNames.Add(ParLine.Name, ParLine);


        }

        private void ColorLine_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int index = (int)button.Tag;
                OpenColorWindow(index);
            }
        }

        private void OpenColorWindow(int index)
        {
            var openColorWindow = Application.Current.Windows.OfType<ExportData>().FirstOrDefault(); // Условие на то, что дочернее окно уже открыто
            if (openColorWindow == null)
            {
                try
                {
                    openCW = new Color(brushes, index, num1, num2, num3, vm, colorLine_But, ColorButForCommon, ButForCommon, brusheForCommonBut);
                    openCW.Owner = Application.Current.MainWindow; // Это позволяет оставлять дочернее окно открытым и видимым в случае, когда сворачиваешь основное окно и после вовращается на него                   
                    openCW.ShowDialog(); // Если использовать ShowDialog вместо Show, то пока открыта дочернее окно, основное окно неактивно 
                    ColorButForCommon = false;
                }
                catch { }

            }
        }

        private void ParLine_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button parLine)
            {
                int ind = (int)parLine.Tag+1;
                tabControl.SelectedIndex = 1;
                if (ExpanderName.TryGetValue("Expander" + ind, out Expander expander))
                    expander.IsExpanded = true;
            }
            //ind++;
            //DrawGroupBox(ind);
            // Force the UI to update before checking
            //Dispatcher.Invoke(() => CheckScrollBar(), System.Windows.Threading.DispatcherPriority.Background);
            //CheckScrollBar(MyScrollBar);            
        }

        private void DelLine_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                par_num = (int)clickedButton.Tag;

                if (col_line == 0)
                {
                    var list3 = new List<ObservablePoint>();
                    vm.Series2[0].Values = list3;
                    for (int i = vs; i <= ve; i++)
                    {
                        list3.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                    }                   
                }
                RemoveLine(par_num);
                Dispatcher.Invoke(() => CheckScrollBar(par_num), System.Windows.Threading.DispatcherPriority.Background);

            }
        }

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button clickedButton && Difract)
            {
                // Retrieve the Tag or other properties of the button
                par_num = (int)clickedButton.Tag;
                int index = par_num + 1;
                
                    if (radioButClick.Count == par_num)
                        radioButClick.Insert(par_num, 0);

                    if (par_num==1)
                {
                    SepInReg.Visibility = Visibility.Visible;
                    TextBlockCommon.Visibility = Visibility.Visible;
                    RadioButForCommon.Visibility = Visibility.Visible;
                    ButForCommon.Visibility = Visibility.Visible;
                }

                    if (DelLineNames.TryGetValue("DelLine" + index, out Button delLine) && ParLineNames.TryGetValue("ParLine" + index, out Button parLine) && RadioButNames.TryGetValue("RadioBut" + index, out RadioButton radioBut))
                        if (delLine.Visibility != Visibility.Visible)
                        {
                            delLine.Visibility = Visibility.Visible;
                            parLine.Visibility = Visibility.Visible;
                            radioBut.IsChecked = true;
                            // DelLine1.Visibility = Visibility.Visible;
                            // ParLine1.Visibility = Visibility.Visible;
                            // RadioBut1.IsChecked = true;
                            num1Color = 4 + par_num * 3;
                            num2Color = 5 + par_num * 3;
                            num3Color = 6 + par_num * 3;
                            num0 = 0;
                            col_line += 1;

                            if (!Optimization.IsEnabled)
                                Optimization.IsEnabled = true;
                            if (par_num<8)                         
                            DrawGroupBox(par_num + 1);
                            DrawExpander(par_num);
                            AddLine();
                            Dispatcher.Invoke(() => CheckScrollBar(index), System.Windows.Threading.DispatcherPriority.Background);
                        }

            }
            
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckScrollBar(1);
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton clickedRadioButton)
            {
                int radioButIndex = (int)clickedRadioButton.Tag;
                int index = radioButIndex + 1;
                if (DelLineNames.TryGetValue("DelLine" + index, out Button delLine))
                    if (delLine.Visibility != Visibility.Visible)
                    {
                        if (RadioButNames.TryGetValue("RadioBut" + index, out RadioButton radioButton))
                            radioButton.IsChecked = false;
                    }
                    else
                        RadioButtonClick(ref radioButClick, radioButIndex, index);
            }
            
            

           // private void RadioBut1_Click(object sender, RoutedEventArgs e)
           // {
                // if (DelLine1.Visibility != Visibility.Visible)           
                //   RadioBut1.IsChecked = false;
                // else
             //   RadioButtonClick(ref radioButClick, 0);
           // }
        }    

        private void CheckScrollBar(int index)
        {
            if (MyScrollBar.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                if (AddLineNames.TryGetValue("AddLine" + index, out Button addLine))
                    if (addLine.Width == 98)
                    {
                        for (int i = 0; i < 10; i++)
                            try
                            {
                                int j = i + 1;
                                if (AddLineNames.TryGetValue("AddLine" + j, out Button button))
                                    button.Width = 92;
                                if (DelLineNames.TryGetValue("DelLine" + j, out Button button1))
                                    button1.Width = 92;

                            }
                            catch { }
                    }
            }
            else
            {
                if (AddLineNames.TryGetValue("AddLine1", out Button addLine))
                    if (addLine.Width==92)
                    {
                        for (int i = 0; i < 10; i++)
                            try
                            {
                                int j = i + 1;
                                if (AddLineNames.TryGetValue("AddLine" + j, out Button button))
                                    button.Width = 98;
                                if (DelLineNames.TryGetValue("DelLine" + j, out Button button1))
                                    button1.Width = 98;

                            }
                            catch { }
                    }
            }
            
        }
        
  

        public MainWindow()
        {
            InitializeComponent();

            vm = new ViewModel();
            DataContext = vm;  // Без данной строки не происходит обновление графика

            //for (int i = 0; i < 4; i++)
            //{
                DrawGroupBox(0);
            //DrawExpander(0);


            //if (MyScrollBar.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            //MessageBox.Show("Виден");
            //}
            //FindNames(0);


            //Реализовать путем динамического добавления 

            //for (int i=0;i<1;i++)
            //{
            // int num = i + 1;
            // var name = FindName("DelLine" + num) as UIElement;
            // DelLineNames.Add(name);
            // name = FindName("ParLine" + num) as UIElement;
            // ParLineNames.Add(name);
            // name = FindName("RadioBut" + num) as UIElement;
            // RadioButNames.Add(name);
            // name = FindName("GB" + num) as UIElement;
            // GBNames.Add(name);
            //}

            //for (int i=0;i<0;i++)
            // {
            //   if (i+1<10)
            //  GBNames[i+1].Visibility = Visibility.Hidden;
            // DelLineNames[i].Visibility = Visibility.Hidden;
            // ParLineNames[i].Visibility = Visibility.Hidden;
            // }



        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void btnOpenFile_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.dat";
            ofd.Filter = "Текстовый файл (*.dat)|*.dat";
            if (ofd.ShowDialog()==true)
                try
                {                    
                    List<string> lines = new List<string>();                    
                    path=Path.GetDirectoryName(ofd.FileName);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] files = dir.GetFiles("*.dat");
                    ListBox.Items.Clear();
                    N_name = 0;
                    foreach (FileInfo fi in files)
                    {
                        ListBox.Items.Add(fi.Name);
                        name.Insert(N_name, fi.FullName);
                        N_name = N_name + 1;
                    }
                    
                    
                }
                catch
                {
                   MessageBox.Show("Невозможно отобразить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }


        private void display_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                N_Values = 0;
                numOfLeftLine = 0;
                numOfRightLine = 0;
                numSumLine = 0;
                vs = 0;
                if (X_values.Count != 0)
                {
                    X_values.Clear();
                    Y_Values.Clear();
                }

                
                StreamReader reader = new StreamReader(name[ListBox.SelectedIndex]);
                string s = reader.ReadLine();                                               
                string col = s.Replace('.', ',').Split(' ')[4];
                lamda = Convert.ToDouble(col);
                FN = ListBox.SelectedItem.ToString();
                s=reader.ReadLine();
                while (s!=null)
                {
                    col = s.Replace('.', ',').Split(' ')[0];
                    X_values.Insert(N_Values, Convert.ToDouble(col));
                    col= s.Replace('.', ',').Split(' ')[1];
                    double value = Convert.ToDouble(col);
                    Y_Values.Insert(N_Values, value);
                    N_Values = N_Values + 1;
                    s = reader.ReadLine();
                }
                LamdaDef(lamda);
                vm.XAxes[0].MinLimit = X_values[0];
                vm.XAxes[0].MaxLimit = X_values[N_Values-1];                
                vm.YAxes[0].MaxLimit = null;
                vm.YAxes[0].MinStep = double.NaN;
                k = Convert.ToInt32((N_Values - 1) / (X_values[N_Values - 1] - X_values[0]));
                b = Convert.ToInt32(1-X_values[0] * k);
                vs = 0; 
                ve= N_Values-1;
                DeleteLine();
                col_line = -1;
                var list = new List<ObservablePoint>();
                
                for (int i = 0; i < N_Values; i++)
                {
                    list.Add(new ObservablePoint(X_values[i], Y_Values[i]));
                }

                vm.AddNewLineSeries(list, 0, vm.Series1, 0);
                vm.AddNewLineSeries(list, 2, vm.Series2, 0);
                
                vm.XAxes2[0].MinLimit = X_values[0];
                vm.XAxes2[0].MaxLimit = X_values[N_Values - 1];

                Scale.IsEnabled = true;
                Range.IsEnabled = true;
                UserBaseline.IsEnabled = true;

                ButDisplay = true;
                TextBlockStatus.Text = "Идет выполнение...";                
            }
            catch
            {
                MessageBox.Show("Невозможно отобразить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                
                DeleteLine();
                Scale.IsChecked = false;
                Scale.IsEnabled = false;
                Range.IsChecked = false;
                Range.IsEnabled = false;
                UserBaseline.IsChecked = false;
                UserBaseline.IsEnabled = false;
                Difract = false;
            }
        }
    }
}
