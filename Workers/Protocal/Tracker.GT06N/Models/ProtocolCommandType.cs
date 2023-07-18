namespace Tracker.GT06N.Models
{
    public enum ProtocolCommandType
    {
        None,
        LoginMessage,
        LocationMessage,
        AlarmMessage,
        StatusMessage,
        Acknowledgement,
    }
    public class Vehicle
    {
        public string DeviceNo { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
