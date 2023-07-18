namespace Tracker.Api
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v1";

        public static class Student
        {
            public const string GetAll = "/student/get-all";
            public const string IdRoute = "/{id}";
        }
        public static class TrackerType
        {
            public const string GetAll = "/VehicleType/get-all";
            public const string IdRoute = "/{id}";
        }
        public static class Account
        {
            public const string Login = "/Login";
            public const string Register = "/Register";
            public const string Logout = "/Logout";

            public const string CreateRole = "/CreateRole";
        }
        public static class Device
        {
            public const string UpdateDevice = "/UpdateDevice";
            public const string RegisterDevice = "/RegisterDevice";
            public const string DeleteDeviceById = "/DeleteDeviceById";
            public const string DeviceDetailsByUserId = "/DeviceDetailsByUserId";
            public const string DeviceDetailsByDeviceNo = "/DeviceDetailsByDeviceNo";
        }
        public static class Live
        {
            public const string LiveTracking = "/LiveTracking";
            public const string HistoryTracking = "/HistoryTracking";
        }
        public static class Import
        {
            public const string ImportData = "/ImportData";
        }
    }
}
