using GalaSoft.MvvmLight;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ADP
{
    public partial class ViewModel : ObservableObject
    {
        private static readonly SKColor LightGrey = new SKColor(217, 217, 214);       
        private static readonly SKColor CornflowerBlue = new SKColor(100, 149, 237);
        public SKColor[] SeriesColor = new SKColor[] { SKColors.Black, LightGrey, CornflowerBlue, SKColors.Blue, SKColors.Red, SKColors.Red.WithAlpha(75), 
            SKColors.Red.WithAlpha(50), SKColors.Green, SKColors.Green.WithAlpha(75), SKColors.Green.WithAlpha(50),
        SKColors.Orange, SKColors.Orange.WithAlpha(75), SKColors.Orange.WithAlpha(50),
        SKColors.Turquoise, SKColors.Turquoise.WithAlpha(75), SKColors.Turquoise.WithAlpha(50),
        SKColors.Maroon, SKColors.Maroon.WithAlpha(75), SKColors.Maroon.WithAlpha(50),
        SKColors.DarkGreen, SKColors.DarkGreen.WithAlpha(75), SKColors.DarkGreen.WithAlpha(50),
        SKColors.OrangeRed, SKColors.OrangeRed.WithAlpha(75), SKColors.OrangeRed.WithAlpha(50),
        SKColors.Fuchsia, SKColors.Fuchsia.WithAlpha(75), SKColors.Fuchsia.WithAlpha(50),
        SKColors.DarkBlue, SKColors.DarkBlue.WithAlpha(75), SKColors.DarkBlue.WithAlpha(50)};

        public ObservableCollection<ISeries> Series1 { get; set; } = new ObservableCollection<ISeries> { };
        //public Dictionary<int, SolidColorPaint> seriesStrokes = new Dictionary<int, SolidColorPaint>();

        //new LineSeries<ObservablePoint>
        // {
        //   Values = new ObservablePoint[] { new ObservablePoint(0, 0) },
        //  Stroke = new SolidColorPaint(SKColors.Black, 1),
        // Fill = null,
        // GeometrySize = 0,
        // LineSmoothness = 0,
        //}


        public ObservableCollection<ISeries> Series2 { get; set; } = new ObservableCollection<ISeries> { };

        public void AddNewLineSeries(List<ObservablePoint> list, int numcolor, ObservableCollection<ISeries> series, int numline)
        {
            //var strokePaint = new SolidColorPaint(SeriesColor[numcolor], 1);
            //seriesStrokes[numline] = strokePaint;

            if (numline == series.Count)
            {
                var newLineSeries = new LineSeries<ObservablePoint>
                {
                    Values = list, // new ObservablePoint[] { new ObservablePoint(0, 0) }, // Initialize with some values
                    Stroke = new SolidColorPaint(SeriesColor[numcolor], 1), // Customize as needed
                    //Stroke=strokePaint,
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0,
                };

                series.Add(newLineSeries);
            }
            else
            {
                var existingLineSeries = series[numline] as LineSeries<ObservablePoint>;
                existingLineSeries.Values = list;
            }
        }

        public void AddNewScatterSeries(List<ObservablePoint> list)
        {
            var ScatterSeries = new ScatterSeries<ObservablePoint>
            {
                Values=list,
                Stroke = new SolidColorPaint
                    {
                      Color=SKColors.Red,
                      StrokeThickness=2,                        
                    },
                  GeometrySize=8,
                  Fill=new SolidColorPaint(SKColors.Red.WithAlpha(25))
            };

            Series1.Add(ScatterSeries);
        }

        public void RemoveSeries(ObservableCollection<ISeries> series, ISeries seriesToRemove)
        {
            if (series.Contains(seriesToRemove))
            {
                series.Remove(seriesToRemove);
            }
        }

       // public ISeries[] Series { get; set; } =
       // {

         //       new LineSeries<ObservablePoint>() // Series[0]
           //     {
             //       Values = new ObservablePoint[]
               //     {
                 //      new ObservablePoint(0, 0),
                  //  },

                //    Stroke= new SolidColorPaint(SKColors.Black,1),
                //    Fill=null,
                //    GeometrySize=0,
                //    LineSmoothness=0,
              //  },

             //   new LineSeries<ObservablePoint>() // Series[1]
             //   {
               //     Values = new ObservablePoint[]
                  //  {
                 //      new ObservablePoint(0, 0),
                 //   },
                 //   Stroke= new SolidColorPaint(LightGrey,1),
                 //   Fill=null,
                 //   GeometrySize=0,
                 //   LineSmoothness=0,
               // },
              //  new LineSeries<ObservablePoint>()  // Series[2]
              //  {
                //    Values = new ObservablePoint[]
                //    {
                  //     new ObservablePoint(0, 0),
                  //  },
                 //   Stroke= new SolidColorPaint(LightGrey,1),
                 //   Fill=null,
                 //   GeometrySize=0,
                 //   LineSmoothness=0,
                //},

              //  new LineSeries<ObservablePoint>() // Series[3]
              //  {
                //    Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                  //  },
                    //Stroke=new SolidColorPaint(SKColors.Red),
                  //  Fill=null,
                  //  LineSmoothness=0,
                  //  GeometrySize=0,
               // },

             //   new ScatterSeries<ObservablePoint>() // Series[4]
              //  {
                //    Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                 //   },
                 //   Stroke = new SolidColorPaint
                //    {
                  //      Color=SKColors.Red,
                    //    StrokeThickness=2,                        
                   // },
                  //  GeometrySize=8,
                  //  Fill=new SolidColorPaint(SKColors.Red.WithAlpha(25))
               // },

             //   new LineSeries<ObservablePoint>() // series[5] 
             //   {
               //     Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                 //   },
                   // Stroke=new SolidColorPaint(SKColors.Red) { StrokeThickness=1},
                  //  Fill=null,
                  //  LineSmoothness=0,
                  //  GeometrySize=0,                    
               // },

              //  new LineSeries<ObservablePoint>()  // series[6]
              //  {
                //    Values=new ObservablePoint[]
                 //   {
                   //     new ObservablePoint(-50,-50)
                  //  },
                 //   Stroke=new SolidColorPaint(SKColors.Red.WithAlpha(75)),
                 //   Fill=null,
                 //   LineSmoothness=0,
                 //   GeometrySize=0,
               // },
                
          //  new LineSeries<ObservablePoint>()  // series[7]
            //    {
              //      Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                  //  },
                 //   Stroke=new SolidColorPaint(SKColors.Red.WithAlpha(50)),
                 //   Fill=null,
                //    LineSmoothness=0,
                //    GeometrySize=0,
               // },

          //  new LineSeries<ObservablePoint>() // series[8] 
            //    {
              //      Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                 //   },
                 //   Stroke=new SolidColorPaint(SKColors.Green) { StrokeThickness=1},
                 //   Fill=null,
                 //   LineSmoothness=0,
                 //   GeometrySize=0,
               // },

              //  new LineSeries<ObservablePoint>()  // series[9]
             //   {
               //     Values=new ObservablePoint[]
                 //   {
                   //     new ObservablePoint(-50,-50)
                  //  },
                 //   Stroke=new SolidColorPaint(SKColors.Green.WithAlpha(75)),
                 //   Fill=null,
                 //   LineSmoothness=0,
                 //   GeometrySize=0,
               // },

           // new LineSeries<ObservablePoint>()  // series[10]
             //   {
               //     Values=new ObservablePoint[]
                 //   {
                   //     new ObservablePoint(-50,-50)
                  //  },
                 //   Stroke=new SolidColorPaint(SKColors.Green.WithAlpha(50)),
                   // Fill=null,
                  //  LineSmoothness=0,
                  //  GeometrySize=0,
              //  },

          //  new LineSeries<ObservablePoint>()  // series[11]  Общая линия
            //    {
              //      Values=new ObservablePoint[]
                //    {
                  //      new ObservablePoint(-50,-50)
                  //  },
                 //   Stroke=new SolidColorPaint(SKColors.Blue),
                 //   Fill=null,
                 //   LineSmoothness=0,
                 //   GeometrySize=0,
               // },


       // };

        public Axis[] XAxes { get; set; } =
        {
                new Axis
                {
                    Name="2Θ, градусы",
                    NamePaint=new SolidColorPaint(SKColors.Black),
                    NameTextSize = 12,
                    Padding = new Padding(5, 10, 5, 0), // положение числовых значений на оси
                    LabelsPaint=new SolidColorPaint(SKColors.Black),
                    TextSize=12,
                    MinLimit=0,
                    MaxLimit=150,
                    MinStep=20,
                    ShowSeparatorLines=false,
                    DrawTicksPath=true,
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color=LightGrey,
                        StrokeThickness=1
                    },
                    
                    //SubseparatorsCount = 5,
                   // ZeroPaint=new SolidColorPaint
                   // {
                    //    Color=black,
                    //    StrokeThickness=1,
                   // },                   
                    TicksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    },
                    SubticksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    }
                }

            };

        public Axis[] YAxes { get; set; } =
        {
                new Axis
                {
                    TextSize=12,
                    Padding = new Padding(5, 0, 10, 0),
                    LabelsPaint=new SolidColorPaint(SKColors.Black),
                    MinLimit=0,
                    MaxLimit=1000,
                    MinStep=200,
                    ShowSeparatorLines=false,
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color=LightGrey,
                        StrokeThickness=1
                    },
                    DrawTicksPath=true,
                    ZeroPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    },
                    SubseparatorsCount=1,
                    TicksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    },
                    SubticksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    }
                }
        };

        public ObservableCollection<RectangularSection> Sections1 { get; set; } = new ObservableCollection<RectangularSection> { };

        public void AddNewRectangularSection(double Xi, double Xj, double Yi, double Yj)
        {
            var newRectSect = new RectangularSection
            {
                Xi = Xi,
                Xj = Xj,
                Yi = Yi,
                Yj = Yj,
                Stroke = new SolidColorPaint(SKColors.Black, 1)
            };
            Sections1.Add(newRectSect);
        }

        public void RemoveRect(RectangularSection RectToRemove)
        {
            if (Sections1.Contains(RectToRemove))
            {
                Sections1.Remove(RectToRemove);
            }
        }

        public void AddDashLine (double Xi, double Xj)
        {
            var newDashLine = new RectangularSection
            {
                Xi=Xi,
                Xj=Xj,
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.Black,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect(new float[] { 9, 9 })
                }
            };

            Sections1.Add(newDashLine);
        }




       // public RectangularSection[] Sections { get; set; } = new RectangularSection[]
       // {
         //   new RectangularSection
          //  {
            //                Xi=0,
              //              Xj=0,
                //            Yi=0,
                  //          Yj=0,
                            //Fill = new SolidColorPaint(new SKColor(255, 205, 210)),
                    //        Stroke = new SolidColorPaint(SKColors.Black, 1)
           // },

          //  new RectangularSection
          //  {
            //    Xi=-50, Xj=-50,
              //  Stroke=new SolidColorPaint
                //            {
                  //              Color=SKColors.Black,
                    //            StrokeThickness=1,
                      //          PathEffect=new DashEffect(new float[] {9 , 9} )
                        //    }
          //  },

          //  new RectangularSection
            //{
              //  Xi=-50, Xj=-50,
              //  Stroke=new SolidColorPaint
                //            {
                  //              Color=SKColors.Black,
                    //            StrokeThickness=1,
                      //          PathEffect=new DashEffect(new float[] {9 , 9} )
                        //    }
          //  }
       // };

        //public ISeries[] Series2 { get; set; } =
        //{

            //    new LineSeries<ObservablePoint>()
              //  {
                //    Values = new ObservablePoint[]
                  //  {
                    //   new ObservablePoint(0, 0),
                   // },
                   // Stroke= new SolidColorPaint(CornflowerBlue, 1),
                   // Fill=null,
                   // GeometrySize=0,
                   // LineSmoothness=0,
                //},
                //new LineSeries<ObservablePoint>()
               // {
                 //   Values = new ObservablePoint[]
                 //   {
                   //    new ObservablePoint(0, 0),
                   // },
                   // Stroke= new SolidColorPaint(LightGrey,1),
                  //  Fill=null,
                  //  GeometrySize=0,
                   // LineSmoothness=0,
               // },
               // new LineSeries<ObservablePoint>()
              //  {
                //    Values = new ObservablePoint[]
                 //   {
                   //    new ObservablePoint(0, 0),
                  //  },
                  //  Stroke= new SolidColorPaint(LightGrey,1),
                  //  Fill=null,
                   // GeometrySize=0,
                  //  LineSmoothness=0,
              //  }
       // };

        public Axis[] XAxes2 { get; set; } =
        {
                new Axis
                {
                    TextSize=0,
                    MinLimit=0,
                    MaxLimit=150,
                    MinStep=20,
                    //DrawTicksPath=true,
                    ShowSeparatorLines=false,
                    //TicksPaint=new SolidColorPaint
                    //{
                     //   Color=SKColors.Black,
                     //   StrokeThickness=1
                    //}
                    ZeroPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                       StrokeThickness=1
                    }
                }

            };

        public Axis[] YAxes2 { get; set; } =
        {
                new Axis
                {
                    TextSize=12,
                    Padding = new Padding(5, 0, 10, 0),
                    LabelsPaint=new SolidColorPaint(SKColors.Black),
                    DrawTicksPath=true,
                    SubseparatorsCount=1,
                    ShowSeparatorLines=false,
                    TicksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    },
                    SubticksPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                        StrokeThickness=1
                    },
                    ZeroPaint=new SolidColorPaint
                    {
                        Color=SKColors.Black,
                       StrokeThickness=1
                    }
                }
        };
    }
}
