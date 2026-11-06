using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ADP
{
    /// <summary>
    /// Логика взаимодействия для ExportData.xaml
    /// </summary>
    public partial class ExportData : Window
    {
        int _col_line, _n_values, _vs, _ve;
        double _lamdaKa1, _lamdaKa2, _step, mu_final, x1, x2, par_b, Sv_final, Voigt, Voigt1, Voigt2;
        string _fileName;
        StreamWriter str;
        List<double> X_values = new List<double>();
        List<double> Y_values = new List<double>();
        double[] _par_mu_final = new double[10];
        double[] _par_x1 = new double[10];
        double[] _par_par_b = new double[10];
        double[] _par_Sv_final = new double[10];
        
       
        public ExportData(string FN, double lamdaKa1, double lamdaKa2, int vs, int ve, int N_values, List<double> X_Values, List<double> Y_Values, double[] par_mu_final, double[] par_x1, double[] par_par_b, double[] par_Sv_final)
        {
            InitializeComponent();
            _fileName = FN;
            //min_x_value = min_x;
            X_values = X_Values;
            Y_values = Y_Values;
            _step = X_values[1] - X_values[0];
            //max_x_value = max_x;
            _lamdaKa1 = lamdaKa1;
            _lamdaKa2 = lamdaKa2;
            _vs = vs;
            _ve = ve;
            _n_values = N_values;
            _par_mu_final = par_mu_final;
            _par_x1 = par_x1;
            _par_par_b = par_par_b;
            _par_Sv_final = par_Sv_final;
        }

        public void DrawCheckBox(int col_line)
        {
            _col_line = col_line;

            for (int i=0;i<=col_line;i++)
            {
                int num = i + 1;
                CheckBox cb = new CheckBox();
                cb.Content = "Линия " + num;
                cb.Name = "CheckBox_" + num;
                cb.IsChecked = false;
                cb.Margin = new Thickness(5);
                MyStackPanel.Children.Add(cb);

                CheckBox cbKa = new CheckBox();
                cbKa.Content = "Kα дублетность";
                cbKa.Name = "KaOfLine_" + num;
                cbKa.IsChecked = false;
                cbKa.Margin = new Thickness(15, 5, 5, 5);
                MyStackPanel.Children.Add(cbKa);
            }

            if (col_line>=1)
            {
                CheckBox cb_sum = new CheckBox();
                cb_sum.Content = "Сумма";
                cb_sum.Name = "CheckBox_Sum";
                cb_sum.IsChecked = false;
                cb_sum.Margin = new Thickness(5);
                MyStackPanel.Children.Add(cb_sum);
            }

            CheckBox cbp = new CheckBox();
            cbp.Content = "Параметры съемки";
            cbp.Name = "CheckBox_Parameters";
            cbp.IsChecked = false;
            cbp.Margin = new Thickness(5, 10, 5, 5);
            MyStackPanel.Children.Add(cbp);
        }

        public CheckBox FindCheckBoxByName(string name, StackPanel stackPanel) // попробовать по Name
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is CheckBox cb && cb.Name.ToString() == name) // cb.Tag="CheckBox_" + num;
                {
                    return cb;
                }
            }
            return null; // Return null if not found
        }

        // Usage example
        public void AccessCheckBox()
        {
            var cb = FindCheckBoxByName("CheckBox_3", MyStackPanel);
            if (cb != null)
            {
                bool isChecked = cb.IsChecked.GetValueOrDefault();
            }
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void strDefaultData(int minx, int maxx)
        {
            str.Write(Math.Round(X_values[minx], 4).ToString().Replace(',', '.') + " ");
            str.Write(Math.Round(_step, 4).ToString().Replace(',', '.') + " ");
            str.Write(Math.Round(X_values[maxx], 4).ToString().Replace(',', '.') + " ");
            str.Write("Lwave:" + " ");
            str.Write(Math.Round(_lamdaKa1, 4).ToString().Replace(',', '.'));
            str.WriteLine();
        }

        private void countVoigt(int i, int j, out double Voigt1, out double Voigt2, out double Voigt)
        {
            mu_final = _par_mu_final[j];
            x1 = _par_x1[j];
            x2 = Math.Round(2 * Math.Asin(_lamdaKa2 * Math.Sin(x1 / 2 * Math.PI / 180) / _lamdaKa1) * 180 / Math.PI, 2);
            par_b = _par_par_b[j];
            Sv_final = _par_Sv_final[j];
            Voigt1 = mu_final / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x1, 2) / (2 * Math.Pow(par_b, 2))) + (1 - mu_final) / (Math.PI * par_b * (1 + Math.Pow((X_values[i] - x1) / par_b, 2)));
            Voigt2 = 0.5 * (mu_final / (par_b * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(par_b, 2))) + (1 - mu_final) / (Math.PI * par_b * (1 + Math.Pow((X_values[i] - x2) / par_b, 2))));
            Voigt = (Voigt1 + Voigt2) * Sv_final;           
        }

        private void RecordLines(int i)
        {
            
            for (int j = 0; j <= _col_line; j++)
            {
                string name = "CheckBox_" + (j + 1).ToString();
                var cb1 = FindCheckBoxByName(name, MyStackPanel);
                if (cb1.IsChecked == true)
                {
                    countVoigt(i, j, out Voigt1, out Voigt2, out Voigt);
                    str.Write(Math.Round(Voigt, 2).ToString().Replace(',', '.') + " ");
                }
                name = "KaOfLine_" + (j + 1).ToString();
                cb1 = FindCheckBoxByName(name, MyStackPanel);
                if (cb1.IsChecked == true)
                {
                    countVoigt(i, j, out Voigt1, out Voigt2, out Voigt);
                    str.Write(Math.Round(Voigt1 * Sv_final, 2).ToString().Replace(',', '.') + " ");
                    str.Write(Math.Round(Voigt2 * Sv_final, 2).ToString().Replace(',', '.') + " ");
                }

            }
            if (_col_line > 0)
            {
                var cb = FindCheckBoxByName("CheckBox_Sum", MyStackPanel);
                if (cb.IsChecked == true)
                {
                    Voigt = 0;
                    for (int j = 0; j <= _col_line; j++)
                    {
                        x2 = Math.Round(2 * Math.Asin(_lamdaKa2 * Math.Sin(_par_x1[j] / 2 * Math.PI / 180) / _lamdaKa1) * 180 / Math.PI, 2);
                        Voigt += (_par_mu_final[j] / (_par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - _par_x1[j], 2) / (2 * Math.Pow(_par_par_b[j], 2))) + (1 - _par_mu_final[j]) / (Math.PI * _par_par_b[j] * (1 + Math.Pow((X_values[i] - _par_x1[j]) / _par_par_b[j], 2))) + 0.5 * (_par_mu_final[j] / (_par_par_b[j] * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * Math.Pow(X_values[i] - x2, 2) / (2 * Math.Pow(_par_par_b[j], 2))) + (1 - _par_mu_final[j]) / (Math.PI * _par_par_b[j] * (1 + Math.Pow((X_values[i] - x2) / _par_par_b[j], 2))))) * _par_Sv_final[j];
                    }
                    str.Write(Math.Round(Voigt, 2).ToString().Replace(',', '.') + " ");
                }
            }
            str.WriteLine();
        }

        private void OKBut_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;
            sfd.DefaultExt = "*.dat";
            sfd.Filter = "dat files (*.dat)|*.dat";
            sfd.FileName = _fileName;
            if (sfd.ShowDialog() == true)
                try
                {
                    using (str = new StreamWriter(sfd.FileName))                  
                        try
                        {
                            var cb = FindCheckBoxByName("CheckBox_Parameters", MyStackPanel);

                            if (cb.IsChecked == true && MainCheckBox.IsChecked == true)
                                strDefaultData(0, _n_values);
                            if (cb.IsChecked == true && MainCheckBox.IsChecked == false)
                                strDefaultData(_vs, _ve);
                            
                            if (MainCheckBox.IsChecked==true)
                            {
                                //start_index = _vs;
                                //end_index = _ve;
                                for (int i=0;i<=_n_values;i++)
                                {
                                    str.Write(X_values[i].ToString().Replace(',', '.') + " ");
                                    str.Write(Math.Round(Y_values[i],2).ToString().Replace(',', '.') + " ");
                                    RecordLines(i);
                                }
                                
                            }
                            else
                            {
                                for (int i = _vs; i <= _ve; i++)
                                {
                                    str.Write(X_values[i].ToString().Replace(',', '.') + " ");
                                    RecordLines(i);
                                }
                            }
                            
                            // запись по столбцам


                            MessageBox.Show("Успешно сохранено!", "Сообщение!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    
                }
                catch
                {
                    MessageBox.Show("Процесс не может получить доступ к файлу, т.к. этот файл используется другим процессом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            CancelBut_Click(sender, e);
        }
    }
}
