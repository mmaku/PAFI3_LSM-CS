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
        
        public List<String> OptionChoice
        {
            get
            {
                return new List<string> { "American Call", "American Put", "Asian Floating Strike", "European Call", "European Put" };
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
                this.rate = double.Parse(value);
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
                
                this.assetCount = Int32.Parse(value);
                vol = CreateVector.Dense<double>(this.assetCount, 0.1);
                cor = CreateMatrix.DenseDiagonal<double>(this.assetCount, 1);
                x0 = CreateVector.Dense<double>(this.assetCount, 100);
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
                this.trajNo = int.Parse(value);
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
                this.excNo = int.Parse(value);
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

                this.seed = Int32.Parse(value);
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
                this.endTime = double.Parse(value);
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
                this.degree = int.Parse(value);
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
                this.optionExp = double.Parse(value);
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
                this.optionStrike = double.Parse(value);
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
                this.price = double.Parse(value);
                NotifyOfPropertyChange(() => this.Price);
            }
        }
        //buttons handlers
        public void Volatility()
        {
            throw new NotImplementedException();
            vol = ReadVector(this.assetCount);
        }
        public void Correlation()
        {
            throw new NotImplementedException();
            cor = ReadMatrix(this.assetCount);
        }
        public void X0()
        {
            throw new NotImplementedException();
            x0 = ReadVector(this.assetCount);
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
