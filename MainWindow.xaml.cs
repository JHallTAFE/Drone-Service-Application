using System.Collections.Generic;
using System.Windows;

namespace Drone_Service_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Programming Criteria 6.2
        private List<Drone> _finishedList = new List<Drone>();
        // Programming Criteria 6.3
        private Queue<Drone> _regularService = new Queue<Drone>();
        // Programming Criteria 6.4
        private Queue<Drone> _expressService = new Queue<Drone>();

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
