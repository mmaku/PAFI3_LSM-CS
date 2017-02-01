using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LSM_CS;

namespace PresentationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            double s = Convert.ToDouble(strike.Text);
            ModelAppki.c = new AmericanCall(1, s);
            ModelAppki.p = new Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);

            price.Text = ModelAppki.p.Price.ToString();

            ModelAppki.c = new EuropeanCall(1, s);
            ModelAppki.p = new Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);
            price_euro.Text = ModelAppki.p.Price.ToString();


            ModelAppki.c = new EuropeanPut(1, s);
            ModelAppki.p = new Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);
            price_euro_put.Text = ModelAppki.p.Price.ToString();

            ModelAppki.c = new AmericanPut(1, s);
            ModelAppki.p = new Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);
            price_put.Text = ModelAppki.p.Price.ToString();
        }
    }
}
