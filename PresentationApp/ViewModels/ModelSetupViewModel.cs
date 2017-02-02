﻿using System;
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
        private double optionExp = 1;
        private double optionStrike = 100;
        private double price = 0;

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
            vol = ReadVector(this.assetCount);
        }
        public void Correlation()
        {
            cor = ReadMatrix(this.assetCount);
        }
        public void X0()
        {
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
        }
        public void Eval()
        {
            ModelAppki.p = new LSM_CS.Pricer(ModelAppki.formula, ModelAppki.market, ModelAppki.c);
            this.Price = ModelAppki.p.Price.ToString();
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
