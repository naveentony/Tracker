namespace Tracker.Features.Alerts
{
    public enum AlertNameType
    {
        SMS, Email, WhatsApp
    }
    public enum AlertType
    {
        PowerVoid, SpeedVoid, Ignition, SOS, Ignitionoff, StoppedBy30Min
    }
    public class AlertsService
    {
        private readonly CollectionProvider _provider;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public AlertsService(CollectionProvider provider) { 
        _provider = provider;
        }
        public async Task<List<Itemlist>> GetAlertNameType(string AlertId = "")
        {
            var result = new List<Itemlist>();
            var obj = new AlertSettingsDto();
            if (string.IsNullOrWhiteSpace(AlertId))
                obj = await FindAlertById(AlertId);

            var alertTypes = Enum.GetValues(typeof(AlertNameType)).Cast<AlertNameType>();
            foreach (var item in alertTypes)
            {
                result.Add(new Itemlist
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = obj.AlertName.Contains(item.ToString()) ? true : false
                }); ;
            }
            return result;
        }
        public async Task<List<Itemlist>> GetAlertTypes(string AlertId)
        {
            var result = new List<Itemlist>();
            var obj = new AlertSettingsDto();
            if (string.IsNullOrWhiteSpace(AlertId))
                obj = await FindAlertById(AlertId);

            var alertTypes = Enum.GetValues(typeof(AlertNameType)).Cast<AlertNameType>();
            foreach (var item in alertTypes)
            {
                result.Add(new Itemlist
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = obj.AlertName.Contains(item.ToString()) ? true : false
                }); ;
            }
            return result;
        }
        public async Task<AlertSettingsDto> FindAlertById(string AlertId)
        {
            var AlertDetails = _provider.GetCollection<AlertSettingsDto>(CollectionNames.AssignVehicles);
            return (await AlertDetails.FindAsync(x => x.Id == AlertId)).FirstOrDefault();
        }
        
        public async Task AddAlert(string AssignId, string AlertName, string AlertType, string VehicleId, int SMSLimit, int EmailLimit)
        {
            var payload = new AlertSettingsDto();
            var AssignVehilce = _provider.GetCollection<AlertSettingsDto>(CollectionNames.AssignVehicles);
            payload = AssignVehilce.Find(x => x.Id == AssignId).FirstOrDefault();
            if (payload is not null)
            {
                payload.AssignId = AssignId;
                payload.VehicleId = VehicleId;
                payload.AlertName.Add(AlertName);
                payload.AlertType.Add(AlertType);
                payload.SMSLimit = SMSLimit;
                payload.EmailLimit = EmailLimit;
                payload.LastUpdatedDate = DateTime.UtcNow;
            }
            else
            {
                payload.AssignId = AssignId;
                payload.VehicleId = VehicleId;
                payload.AlertName.Add(AlertName);
                payload.AlertType.Add(AlertType);
                payload.SMSLimit = SMSLimit;
                payload.EmailLimit = EmailLimit;
                payload.LastUpdatedDate = DateTime.UtcNow;
            }
            await AssignVehilce.InsertOneAsync(payload);
        }
    }
}
