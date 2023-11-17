using System;
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

        // Programming Criteria 6.5
        private void AddNewItem()
        {
            if (IsFilled() && double.TryParse(ServiceCost.Text, out double _serviceCost))
            {
                // To do: Increment Service Tag as per Programming Criteria 6.11
                var droneToAdd = new Drone();
                droneToAdd.SetClientName(ClientName.Text);
                droneToAdd.SetDroneModel(DroneModel.Text);
                droneToAdd.SetServiceProblem(ServiceProblem.Text);
                droneToAdd.SetServiceTag(int.Parse(ServiceTag.Text));
                if (GetServicePriority() == "ExpressService")
                {
                    // Programming Criteria 6.6
                    droneToAdd.SetServiceCost(_serviceCost * 1.15);
                    _expressService.Enqueue(droneToAdd);
                }
                else // Assume regular service
                {
                    droneToAdd.SetServiceCost(_serviceCost);
                    _regularService.Enqueue(droneToAdd);
                }
                IncrementServiceTag();
                DisplayServiceQueue();
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
        // Programming Criteria 6.7
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
        private void DisplayServiceQueue()
        {
            ListViewServiceRegular.Items.Clear();
            ListViewServiceExpress.Items.Clear();

            // Programming Criteria 6.8
            foreach (var drone in _regularService)
            {
                ListViewServiceRegular.Items.Add(new
                {
                    clientName = drone.GetClientName(),
                    droneModel = drone.GetDroneModel(),
                    serviceCost = drone.GetServiceCost(),
                    serviceProblem = drone.GetServiceProblem(),
                    serviceTag = drone.GetServiceTag()
                });
            }
            // Programming Criteria 6.9
            foreach (var drone in _expressService)
            {
                ListViewServiceExpress.Items.Add(new
                {
                    clientName = drone.GetClientName(),
                    droneModel = drone.GetDroneModel(),
                    serviceCost = drone.GetServiceCost(),
                    serviceProblem = drone.GetServiceProblem(),
                    serviceTag = drone.GetServiceTag()
                });
            }
        }

        private void IncrementServiceTag()
        {
            if (int.TryParse(ServiceTag.Text, out int _serviceTag) && _serviceTag > 890)
            {
                _serviceTag = 900;
            }
            else
                _serviceTag += 10;
            ServiceTag.Text = _serviceTag.ToString();
        }

        private string GetSelectedServiceTab()
        {
            TabItem? selectedTab = ServiceTabs.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                var tabName = selectedTab.Header.ToString() ?? String.Empty;
                return tabName;
            }
            return String.Empty;
        }
        // Programming Criteria 6.12 & 6.13 Part B
        private void DisplayDrone(Drone drone)
        {
            ClientName.Text = drone.GetClientName();
            DroneModel.Text = drone.GetDroneModel();
            ServiceProblem.Text = drone.GetServiceProblem();
            //ServiceCost.Text = drone.GetServiceCost().ToString();
            //ServiceTag.Text = drone.GetServiceTag().ToString();
        }
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }

        private void FinishService_Click(object sender, RoutedEventArgs e)
        {
            // To-do: 6.14/6.15
        }
        private void ListViewService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = -1;
            Drone? drone = null;
            // Programming Criteria 6.12 Part A
            if (GetSelectedServiceTab() == "Regular")
            {
                selectedIndex = ListViewServiceRegular.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    drone = _regularService.ElementAt(selectedIndex);
                }
            }
            // Programming Criteria 6.13 Part A
            else if (GetSelectedServiceTab() == "Express")
            {
                selectedIndex = ListViewServiceExpress.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    drone = _expressService.ElementAt(selectedIndex);
                }
            }
            if (drone != null)
            {
                DisplayDrone(drone);
            }
        }
    }
}
