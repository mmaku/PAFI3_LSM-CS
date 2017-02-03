using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using MathNet.Numerics.LinearAlgebra;
using System.Runtime.InteropServices;


namespace PresentationApp.ViewModels
{
    public class ModelSetupViewModel : PropertyChangedBase
    {
        // fields
        private bool hasAvg;
        private double rate = 0.05;
        private int assetCount = 1;
        private int trajNo = 100000;
        private Vector<double> vol = CreateVector.Dense<double>(1, 0.2);
        private Matrix<double> cor = CreateMatrix.Dense<double>(1, 1, 1);
        private int excNo = 50;
        private int seed = 34;
        private double endTime = 1;
        private Vector<double> x0 = CreateVector.Dense<double>(1, 100);
        private int degree = 3;
        private string optionType = "American Call";
        private double optionExp = 1;
        private double optionStrike = 100;
        private double price = 0;
        private string selectedFormula = "Polynomial";

        private double vol1 = 0.2;
        private double vol2 = 0.2;
        private double vol3 = 0.2;

        private double cor12 = 0;
        private double cor13 = 0;
        private double cor23 = 0;

        private double x01 = 100;
        private double x02 = 100;
        private double x03 = 100;
        
        public List<String> OptionChoice
        {
            get
            {
                return new List<string> { "American Call", "American Put", "Asian Floating Strike", "Binary American Call", "Binary American Call Multi Asset",  "European Call", "European Put" };
            }
        }
        public List<String> FormulaChoice
        {
            get
            {
                return new List<string> { "Polynomial", "Two Variables Mix" };
            }
        }
        public string SelectedFormulaChoice
        {
            get
            {
                return this.selectedFormula;
            }
            set
            {
                this.selectedFormula = value;
                NotifyOfPropertyChange(() => SelectedFormulaChoice);
            }
        }
        public string SelectedOptionChoice
        {
            get
            {
                return this.optionType;
            }
            set
            {
                this.optionType = value;
                this.NotifyOfPropertyChange(() => this.SelectedOptionChoice);
            }
        }
        //data binding
        public bool HasAvg
        {
            get
            {
                return this.hasAvg;
            }
            set
            {
                this.hasAvg = value;
                NotifyOfPropertyChange(() => this.HasAvg);
            }
        }
        public string Rate
        {
            get
            {
                return this.rate.ToString();
            }
            set
            {
                double.TryParse(value, out rate);
                this.NotifyOfPropertyChange(() => this.Rate);
            }
        }
        public string AssetCount
        {
            get
            {
                return this.assetCount.ToString();
            }
            set
            {
                
                bool success =Int32.TryParse(value, out assetCount);
                if (success)
                {
                    vol = VectorUpdate(vol1, vol2, vol3);
                    x0 = VectorUpdate(x01, x02, x03);
                    cor = CorUpdate(cor12, cor13, cor23);
                }
                
                this.NotifyOfPropertyChange(() => this.AssetCount);

                
            }
        }
        public string TrajNo
        {
            get
            {
                return this.trajNo.ToString();
            }
            set
            {
                int.TryParse(value, out trajNo);
                NotifyOfPropertyChange(() => this.TrajNo);
            }
        }
        public string ExcNo
        {
            get
            {
                return this.excNo.ToString();
            }
            set
            {
                int.TryParse(value, out excNo);
                NotifyOfPropertyChange(() => this.ExcNo);
            }
        }
        public string Seed
        {
            get
            {
                return this.seed.ToString();
            }
            set
            {

                Int32.TryParse(value, out seed);
                this.NotifyOfPropertyChange(() => this.Seed);

            }
        }
        public string EndTime
        {
            get
            {
                return this.endTime.ToString();
            }
            set
            {
                double.TryParse(value, out endTime);
                this.NotifyOfPropertyChange(() => this.EndTime);
            }
        }
        public string Degree
        {
            get
            {
                return this.degree.ToString();
            }
            set
            {
                int.TryParse(value, out degree);
                NotifyOfPropertyChange(() => this.Degree);
            }
        }
        public string OptionExp
        {
            get
            {
                return this.optionExp.ToString();
            }
            set
            {
                double.TryParse(value, out optionExp);
                NotifyOfPropertyChange(() => this.OptionExp);
            }
        }
        public string OptionStrike
        {
            get
            {
                return this.optionStrike.ToString();
            }
            set
            {
                double.TryParse(value, out optionStrike);
                NotifyOfPropertyChange(() => this.OptionStrike);
            }
        }
        public string Price
        {
            get
            {
                return this.price.ToString();
            }
            set
            {
                double.TryParse(value, out price);
                NotifyOfPropertyChange(() => this.Price);
            }
        }
        public string Vol1
        {
            get
            {
                return this.vol1.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out vol1);
                if (success)
                {
                    this.vol[0] = this.vol1;
                }
                NotifyOfPropertyChange(() => this.Vol1);
            }

        }
        public string Vol2
        {
            get
            {
                return this.vol2.ToString();
            }
            set
            {
                double.TryParse(value, out vol2);
                if (assetCount > 1)
                {
                    this.vol[1] = this.vol2;
                }
                NotifyOfPropertyChange(() => this.Vol2);
            }

        }
        public string Vol3
        {
            get
            {
                return this.vol3.ToString();
            }
            set
            {
                double.TryParse(value, out vol3);
                if (assetCount > 2)
                {
                    this.vol[2] = this.vol3;
                }
                NotifyOfPropertyChange(() => this.Vol3);
            }

        }
        public string X01
        {
            get
            {
                return this.x01.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out x01);
                if (success)
                {
                    this.x0[0] = this.x01;
                }
                
                NotifyOfPropertyChange(() => this.X01);
            }

        }
        public string X02
        {
            get
            {
                return this.x02.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out x02);
                if (success)
                {
                    if (assetCount > 1)
                    {
                        this.x0[1] = this.x02;
                    }
                }
                
                NotifyOfPropertyChange(() => this.X02);
            }

        }
        public string X03
        {
            get
            {
                return this.x03.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out x03);
                if (assetCount > 2 && success)
                {
                    this.x0[2] = this.x03;
                }
                NotifyOfPropertyChange(() => this.X03);
            }

        }
        public string Cor12
        {
            get
            {
                return this.cor12.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out cor12);
                if (assetCount > 1 && success)
                {
                    this.cor[0, 1] = this.cor12;
                    this.cor[1, 0] = this.cor12;
                }
                NotifyOfPropertyChange(() => this.Cor12);
            }

        }
        public string Cor13
        {
            get
            {
                return this.cor13.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out cor13);
                if (assetCount > 2 && success)
                {
                    this.cor[0,2] = this.cor13;
                    this.cor[2, 0] = this.cor13;
                }
                NotifyOfPropertyChange(() => this.Cor13);
            }

        }
        public string Cor23
        {
            get
            {
                return this.cor23.ToString();
            }
            set
            {
                bool success = double.TryParse(value, out cor23);
                if (assetCount > 2 && success)
                {
                    this.cor[1,2] = this.cor23;
                    this.cor[2, 1] = this.cor23;
                }
                NotifyOfPropertyChange(() => this.Cor23);
            }

        }
        
        public Vector<double> VectorUpdate (double val1, double val2, double val3)
        {
            Vector<double> result = CreateVector.Dense<double>(assetCount, 0);
            result[0] = val1;
            if (assetCount > 1)
            {
                result[1] = val2;
                if (assetCount > 2)
                {
                    result[2] = val3;
                }
            }
            return result;
            
        }
        public Matrix<double> CorUpdate(double val12, double val13, double val23)
        {
            Matrix<double> result = CreateMatrix.Dense<double>(assetCount, assetCount, 1);
            if (assetCount > 1)
            {
                result[0,1] = val12;
                result[1,0] = val12;
                if (assetCount > 2)
                {
                    result[2, 1] = val23;
                    result[1, 2] = val23;
                    result[2, 0] = val23;
                    result[0, 2] = val23;
                }
            }
            return result;

        }
        
        public void SetModel()
        {
            
            if (hasAvg)
            {
                ModelAppki.market = new LSM_CS.ModelAsianAvgFixedRate(rate,
                                            seed,
                                            trajNo,
                                            excNo,
                                            endTime,
                                            CreateVector.Dense<double>(assetCount, rate),
                                            vol,
                                            x0,
                                            cor);
            }
            else
            {
                ModelAppki.market = new LSM_CS.ModelSimpleFixedRate(rate,
                                            seed,
                                            trajNo,
                                            excNo,
                                            endTime,
                                            CreateVector.Dense<double>(assetCount, rate),
                                            vol,
                                            x0,
                                            cor);
            }
            System.Windows.MessageBox.Show("Model changed");
        }
        public void SetOption()
        {
            switch (optionType)
            {
                case "American Call":
                    ModelAppki.c = new LSM_CS.AmericanCall(optionExp, optionStrike);
                    break;
                case "American Put":
                    ModelAppki.c = new LSM_CS.AmericanPut(optionExp, optionStrike);
                    break;
                case "Asian Floating Strike":
                    ModelAppki.c = new LSM_CS.AsianAmericanFloatingStrikeCall(optionExp);
                    break;
                case "Binary American Call":
                    ModelAppki.c = new LSM_CS.BinaryAbove(optionExp, optionStrike);
                    break;
                case "Binary American Call Multi Asset":
                    ModelAppki.c = new LSM_CS.BinaryAboveMultipleAssets(optionExp, optionStrike);
                    break;
                case "European Call":
                    ModelAppki.c = new LSM_CS.EuropeanCall(optionExp, optionStrike);
                    break;
                case "European Put":
                    ModelAppki.c = new LSM_CS.EuropeanPut(optionExp, optionStrike);
                    break;
            }
            System.Windows.MessageBox.Show("Option changed");
        }
        public void SetFormula()
        {
            switch (selectedFormula)
            {
                case "Polynomial":
                    ModelAppki.formula = new LSM_CS.PolyRegression(degree);
                    break;
                case "Two Variables Mix":
                    ModelAppki.formula = new LSM_CS.BivariableRegression(degree, degree, degree);
                    break;
            }
            System.Windows.MessageBox.Show("Formula changed");
        }
        public void Eval()
        {
            ModelAppki.p = new LSM_CS.Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);
            this.Price = ModelAppki.p.Price.ToString("0.##");
            System.Windows.MessageBox.Show("Done");
        }

        private Vector<double> ReadVector(int length)
        {
            Vector<double> result = CreateVector.Dense<double>(length, 0.1);
            for (int i = 0; i < length; i++)
            {
                
                //Console.WriteLine("Enter element: " + (i+1).ToString());
               // result[i] = double.Parse(Console.ReadLine());
            }
            return result;
        }
        private Matrix<double> ReadMatrix(int rank)
        {
            Matrix<double> result = CreateMatrix.DenseDiagonal<double>(rank, 1);
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j<rank; j++)
                {
                    Console.WriteLine("Enter element: " + (i + 1).ToString() + ", " + (j + 1).ToString());
                    result[i, j] = double.Parse(Console.ReadLine());
                }
            }
            return result;
        }

    }
}
