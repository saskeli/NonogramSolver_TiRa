using GameLib;
using SolverLib;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Util;

namespace NonogramSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _solveBW = new BackgroundWorker();
        private BackgroundWorker _fillBW = new BackgroundWorker();
        private int _waitTime = 150;
        private bool _running = false;
        private List<Result> _resultQueue;
        public MainWindow()
        {
            InitializeComponent();

            _solveBW.WorkerReportsProgress = false;
            _solveBW.RunWorkerCompleted += _solveBW_RunWorkerCompleted;
            _solveBW.DoWork += _solveBW_DoWork;

            _fillBW.WorkerReportsProgress = true;
            _fillBW.RunWorkerCompleted += _fillBW_RunWorkerCompleted;
            _fillBW.ProgressChanged += _fillBW_ProgressChanged;
            _fillBW.DoWork += _fillBW_DoWork;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (_running) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _running = true;
            Nonogram ng = NonoGramFactory.ParseFromFile(((DataObject)e.Data).GetFileDropList()[0]);
            MakeGrid(ng);
            stateBox.Text = "Solving...";
            _solveBW.RunWorkerAsync(ng);
            e.Handled = true;
        }

        private void MakeGrid(Nonogram ng)
        {
            wGrid.Children.Clear();
            wGrid.RowDefinitions.Clear();
            wGrid.ColumnDefinitions.Clear();
            GridLength gl = new GridLength(15);
            for (int i = 0; i < ng.Width; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = gl;
                wGrid.ColumnDefinitions.Add(cd);
            }
            
            for (int i = 0; i < ng.Height; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = gl;
                wGrid.RowDefinitions.Add(rd);
            }
        }

        private void _solveBW_DoWork(object sender, DoWorkEventArgs e)
        {
            ISolver ss = new SerialSolver();
            ss.Run((Nonogram)e.Argument);
            e.Result = ss;
        }

        private void _solveBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ISolver solver = (ISolver)e.Result;
            if (solver.Solved())
            {
                stateBox.Text = "Solved in " + solver.BenchTime().TotalMilliseconds + "ms.";
                _resultQueue = solver.Results();
                _fillBW.RunWorkerAsync(_resultQueue.Count);
            }
            
        }

        private void _fillBW_DoWork(object sender, DoWorkEventArgs e)
        {
            int resLen = (int)e.Argument;
            for (int i = 0; i < resLen; i++)
            {
                _fillBW.ReportProgress(i);
                Thread.Sleep(_waitTime);
            }
        }

        private void _fillBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Result res = _resultQueue[e.ProgressPercentage];
            Rectangle r = new Rectangle();
            r.Fill = res.State ? Brushes.Black : Brushes.White;
            Grid.SetColumn(r, res.Column);
            Grid.SetRow(r, res.Row);
            wGrid.Children.Add(r);
        }

        private void _fillBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _running = false;
        }

        private void Grid_PreviewDrag(object sender, DragEventArgs e)
        {
            if (_running)
            {
                e.Effects = DragDropEffects.None;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }
    }
}
