using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Controls;

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
            RegularService.IsChecked = true;
        }

        private void AddNewItem()
        {
            if (IsFilled() && double.TryParse(ServiceCost.Text, out double _serviceCost))
            {
                // To do: Increment Service Tag as per Programming Criteria 6.11
                var droneToAdd = new Drone();
                droneToAdd.SetClientName(ClientName.Text);
                droneToAdd.SetDroneModel(DroneModel.Text);
                droneToAdd.SetServiceProblem(ServiceProblem.Text);
                if (GetServicePriority() == "ExpressService")
                {
                    droneToAdd.SetServiceCost(_serviceCost * 1.15);
                    _expressService.Enqueue(droneToAdd);
                }
                else // Assume regular service
                {
                    droneToAdd.SetServiceCost(_serviceCost);
                    _regularService.Enqueue(droneToAdd);
                }
            }
            else
            {
                // To-do Status strip feedback about filling in boxes
            }
        }
        private bool IsFilled()
        {
            if (string.IsNullOrWhiteSpace(ClientName.Text)
                || string.IsNullOrWhiteSpace(DroneModel.Text)
                || string.IsNullOrWhiteSpace(ServiceProblem.Text)
                || string.IsNullOrWhiteSpace(ServiceCost.Text)
                || string.IsNullOrEmpty(GetServicePriority()))
            {
                return false;
            }
            else
                return true;
        }
        private string GetServicePriority()
        {
            // var serviceButtons = LogicalTreeHelper.GetChildren(Service_Priority).OfType<RadioButton>();
            var serviceButtons = Service_Priority.FindLogicalChildren<RadioButton>();
            foreach (var radioButton in serviceButtons)
            {
                if (radioButton.IsChecked == true)
                    return radioButton.Name;
            }
            return string.Empty;
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }
    }
}
