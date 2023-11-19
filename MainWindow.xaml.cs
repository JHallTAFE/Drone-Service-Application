using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            DataObject.AddPastingHandler(ServiceCost, ServiceCostOnPaste_Event);
            StatusBarText.Text = "Ready to work!";
        }

        // Programming Criteria 6.5
        /// <summary>
        /// Attempts to create a new drone object and add it to the appropriate queue.
        /// </summary>
        private void AddNewItem()
        {
            if (IsFilled() && double.TryParse(ServiceCost.Text, out double _serviceCost))
            {
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
                    StatusBarText.Text = String.Format("{0}'s drone {1} added to the express queue!", droneToAdd.GetClientName(), droneToAdd.GetDroneModel());
                }
                else // Assume regular service
                {
                    droneToAdd.SetServiceCost(_serviceCost);
                    _regularService.Enqueue(droneToAdd);
                    StatusBarText.Text = String.Format("{0}'s drone {1} added to the regular queue!", droneToAdd.GetClientName(), droneToAdd.GetDroneModel());
                }
                IncrementServiceTag();
                DisplayServiceQueue();
                ClearBoxes();
            }
            else
            {
                StatusBarText.Text = "One or more boxes were left blank or invalid. Could not add drone.";
            }
        }
        /// <summary>
        /// Checks if all the input boxes are filled to create a new drone with.
        /// </summary>
        /// <returns>true if all the boxes are filled, false otherwise.</returns>
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
        /// <summary>
        /// Gets the service priority radio box currently checked.
        /// </summary>
        /// <returns>The name of the checked radio box, or string.Empty if no radio button is selected.</returns>
        private string GetServicePriority()
        {
            var serviceButtons = Service_Priority.FindLogicalChildren<RadioButton>();
            foreach (var radioButton in serviceButtons)
            {
                if (radioButton.IsChecked == true)
                    return radioButton.Name;
            }
            return string.Empty;
        }
        /// <summary>
        /// Refreshes the display for both queues and the list box.
        /// </summary>
        private void DisplayServiceQueue()
        {
            ListViewServiceRegular.Items.Clear();
            ListViewServiceExpress.Items.Clear();
            FinishedItems.Items.Clear();

            // Programming Criteria 6.8
            foreach (var drone in _regularService)
            {
                ListViewServiceRegular.Items.Add(new
                {
                    clientName = drone.GetClientName(),
                    droneModel = drone.GetDroneModel(),
                    serviceCost = drone.GetServiceCost().ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-AU")),
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
                    serviceCost = drone.GetServiceCost().ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-AU")),
                    serviceProblem = drone.GetServiceProblem(),
                    serviceTag = drone.GetServiceTag()
                });
            }
            // Programming Criteria 6.14 & 6.15 Part B
            foreach (var drone in _finishedList)
            {
                FinishedItems.Items.Add(drone.DisplayDetails());
            }
        }
        // Programming Criteria 6.11
        /// <summary>
        /// Increments the service tag by 10, to a maximum of 900.
        /// </summary>
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
        /// <summary>
        /// Gets the selected tab displaying the queue.
        /// </summary>
        /// <returns>The name of the tab selected, or string.Empty if no tab is selected.</returns>
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
        /// <summary>
        /// Displays the client name, drone model and service problem of the given drone in the text boxes.
        /// </summary>
        /// <param name="drone">The drone to display.</param>
        private void DisplayDrone(Drone drone)
        {
            ClientName.Text = drone.GetClientName();
            DroneModel.Text = drone.GetDroneModel();
            ServiceProblem.Text = drone.GetServiceProblem();
            //ServiceCost.Text = drone.GetServiceCost().ToString();
            //ServiceTag.Text = drone.GetServiceTag().ToString();
        }
        // Programming Criteria 6.16 Part B
        /// <summary>
        /// Removes the drone with the given index from the finished items list.
        /// </summary>
        /// <param name="index">Index of the drone to remove.</param>
        private void FinishDroneService(int index)
        {
            // If index is within the list
            if (index >= 0 && FinishedItems.Items.Count > index)
            {
                var removedDrone = _finishedList[index];
                FinishedItems.Items.RemoveAt(index);
                _finishedList.RemoveAt(index);
                StatusBarText.Text = String.Format("{0}'s drone has been collected!", removedDrone.GetClientName());
            }
        }
        // Programming Criteria 6.17
        /// <summary>
        /// Clears the input boxes.
        /// </summary>
        private void ClearBoxes()
        {
            ClientName.Clear();
            DroneModel.Clear();
            ServiceProblem.Clear();
            ServiceCost.Clear();
            RegularService.IsChecked = true;
        }
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }

        private void FinishService_Click(object sender, RoutedEventArgs e)
        {
            // Programming Criteria 6.14 Part A
            if (GetSelectedServiceTab() == "Regular" && _regularService.Count > 0)
            {
                var drone = _regularService.Dequeue();
                _finishedList.Add(drone);
                DisplayServiceQueue();
                StatusBarText.Text = String.Format("Finished service on {0}'s drone.", drone.GetClientName());
            }
            // Programming Criteria 6.15 Part A
            else if (GetSelectedServiceTab() == "Express" & _expressService.Count > 0)
            {
                var drone = _expressService.Dequeue();
                _finishedList.Add(drone);
                DisplayServiceQueue();
                StatusBarText.Text = String.Format("Finished service on {0}'s drone.", drone.GetClientName());
            }
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
        // Programming Criteria 6.16 Part A
        private void RemoveDrone_Event(object sender, EventArgs e)
        {
            if (FinishedItems.SelectedIndex >= 0)
            {
                FinishDroneService(FinishedItems.SelectedIndex);
            }
        }
        // Programming Criteria 6.10 Part A
        private void ServiceCost_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = @"^[0-9]*(\.[0-9]{0,2})?$";
            Regex regexObj = new(regex);
            string finalText = ServiceCost.Text.Insert(ServiceCost.CaretIndex, e.Text);

            // If user would press an input that makes the result invalid
            if (!regexObj.IsMatch(finalText))
            {
                e.Handled = true;
            }
        }
        // Programming Criteria 6.10 Part B
        private void ServiceCostOnPaste_Event(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                var regex = @"^[0-9]*(\.[0-9]{0,2})?$";
                Regex regexObj = new(regex);
                string finalText = ServiceCost.Text.Insert(ServiceCost.CaretIndex, pasteText);

                // If paste contents are not valid or if the end result would not be valid
                if (!regexObj.IsMatch(finalText))
                {
                    e.CancelCommand();
                }
            }
        }
    }
}
