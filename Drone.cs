namespace Drone_Service_Application
{
    internal class Drone
    {
        private string _clientName;
        private string _droneModel;
        private string _serviceProblem;
        private double _serviceCost;
        private int _serviceTag;

        public Drone()
        {
            _clientName = string.Empty;
            _droneModel = string.Empty;
            _serviceProblem = string.Empty;
            _serviceCost = 0;
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
            _clientName = clientName;
        }
        public void SetDroneModel(string droneModel)
        {
            _droneModel = droneModel;
        }
        public void SetServiceProblem(string serviceProblem)
        {
            _serviceProblem = serviceProblem;
        }
        public void SetServiceCost(double serviceCost)
        {
            _serviceCost = serviceCost;
        }
        public void SetServiceTag(int serviceTag)
        {
            _serviceTag = serviceTag;
        }
        #endregion
    }
}
