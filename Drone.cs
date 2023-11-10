using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Drone_Service_Application
{
    internal class Drone
    {
        /// <summary>
        /// Name of the client.
        /// </summary>
        private string _clientName;
        /// <summary>
        /// Model of the drone.
        /// </summary>
        private string _droneModel;
        /// <summary>
        /// Problem the drone is experiencing.
        /// </summary>
        private string _serviceProblem;
        /// <summary>
        /// Cost of service in dollars.
        /// </summary>
        private double _serviceCost;
        private int _serviceTag;

        public Drone()
        {
            _clientName = string.Empty;
            _droneModel = string.Empty;
            _serviceProblem = string.Empty;
            _serviceCost = 0.00;
            _serviceTag = 100;
        }

        #region accessors
        public string GetClientName()
        {
            return _clientName;
        }
        public string GetDroneModel()
        {
            return _droneModel;
        }
        public string GetServiceProblem()
        {
            return _serviceProblem;
        }
        public double GetServiceCost()
        {
            return _serviceCost;
        }
        public int GetServiceTag()
        {
            return _serviceTag;
        }
        public void SetClientName(string clientName)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            _clientName = ti.ToTitleCase(clientName);
        }
        public void SetDroneModel(string droneModel)
        {
            _droneModel = droneModel;
        }
        public void SetServiceProblem(string serviceProblem)
        {
            if (!string.IsNullOrWhiteSpace(serviceProblem))
            {
                if (serviceProblem.Length == 1)
                {
                    serviceProblem = serviceProblem.ToUpper();
                }
                else // Input is > 1 character
                {
                    // Capitalise first letter
                    serviceProblem = Char.ToUpper(serviceProblem[0]) + serviceProblem[1..];
                    // Sentence case any words after a full stop and space.
                    serviceProblem = Regex.Replace(serviceProblem, @"\.\s[a-z]", match => ". " + match.Groups[1].Value.ToUpper());
                }
            }
            _serviceProblem = serviceProblem;
        }
        public void SetServiceCost(double serviceCost)
        {
            _serviceCost = Math.Abs(Math.Round(serviceCost, 2, MidpointRounding.ToZero));
        }
        public void SetServiceTag(int serviceTag)
        {
            _serviceTag = serviceTag;
        }
        #endregion
        /// <summary>
        /// Gets the client name and service cost in a formatted string.
        /// </summary>
        /// <returns>String containing "ClientName: $ServiceCost"</returns>
        public string DisplayDetails()
        {
            return string.Format("{0}: ${1:N2}", _clientName, _serviceCost);
        }
    }
}
