﻿
using Tracker.Features.Device.DeviceTypes;
using Tracker.ImportData.Managers;

namespace Tracker.Features.Manage.ImportData
{
    public class ImportResult
    {
        public string Id { get; set; } = string.Empty;
    }
    public class ImportDataFromSql : IRequest<OperationResult<IEnumerable<ImportResult>>>
    {
        public string Id { get; set; } = string.Empty;
    }
    public class ImportDataHandler
        : IRequestHandler<ImportDataFromSql, OperationResult<IEnumerable<ImportResult>>>
    {

        private readonly ICollectionProvider _prov;
        private readonly DeviceTypesService _deviceTypesService;
        public ImportDataHandler(ICollectionProvider provider, DeviceTypesService deviceTypesService)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));
            _deviceTypesService = deviceTypesService;

        }
        public async Task<OperationResult<IEnumerable<ImportResult>>> Handle(ImportDataFromSql request,
                CancellationToken cancellationToken)
        {

            var result = new OperationResult<IEnumerable<ImportResult>>();
            DBManager db = new DBManager();
            //var Data = db.LoadTrackerTypesData();
            //await _deviceTypesService.AddTrackerTypeData(Data);

            //Vehicles
            //var Data = db.VehiclesDtoData();
            //await _deviceTypesService.AddVehiclesData(Data);

            //TrackerLive
            var Data = db.TrackerDataLive();
            await _deviceTypesService.AddTrackerDataLive(Data);
            //foreach (var type in Data)
            //{
            //    await _deviceTypesService.AddNewDevice(type);
            //}
            return result;
        }


    }


}
