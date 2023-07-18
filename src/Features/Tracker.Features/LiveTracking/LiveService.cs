using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Features.LiveTracking
{
    public class LiveService
    {
        private readonly CollectionProvider _provider;
        private readonly IConfiguration _configuration;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public LiveService(IConfiguration configuration, CollectionProvider provider)
        {
            _provider = provider;
            _configuration = configuration;
        }
        /// <summary>
        /// Get the status of Current Log
        /// </summary>
        /// <param name="zeroSpeed">Zero Speed of Tracker</param>
        /// <param name="TrackerData">Log object</param>
        /// <returns>Vehicle Status</returns>
        public static VehicleStatus GetCurrentTrackerStatus(TrackerTypesDto DeivceTypes, TrackerDataDto TrackerData)
        {
            VehicleStatus currentStatus = VehicleStatus.Stop;
            if (TrackerData != null)
            {
                if (DeivceTypes.Name.ToUpper() == "DEV")
                {
                    // Case for DeviceX
                    if ((DateTime.Now - TrackerData.TrackDateTime).TotalMinutes > 30)
                        currentStatus = VehicleStatus.Unreachable;
                    else if (TrackerData.Speed > DeivceTypes.ZeroSpeed)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.IsIgnitionOn == true && TrackerData.Speed > 1)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.IsIgnitionOn)
                        currentStatus = VehicleStatus.Idle;

                }
                else if (DeivceTypes.Name.ToUpper() == "GEM")
                {
                    // Case for GT06
                    if ((DateTime.Now - TrackerData.TrackDateTime).TotalMinutes > 30)
                        currentStatus = VehicleStatus.Unreachable;
                    else if (TrackerData.Speed > DeivceTypes.ZeroSpeed)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.IsIgnitionOn == true && TrackerData.Speed > 0)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.IsIgnitionOn)
                        currentStatus = VehicleStatus.Idle;
                }
                else
                {
                    if ((DateTime.Now - TrackerData.TrackDateTime).TotalMinutes > 30)
                        currentStatus = VehicleStatus.Unreachable;
                    else if (TrackerData.IsIgnitionOn && TrackerData.Speed > 0)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.Speed > DeivceTypes.ZeroSpeed)
                        currentStatus = VehicleStatus.Moving;
                    else if (TrackerData.IsIgnitionOn)
                        currentStatus = VehicleStatus.Idle;
                    else if (!TrackerData.IsIgnitionOn && TrackerData.Speed > DeivceTypes.ZeroSpeed && !(DeivceTypes.Name.ToUpper() == "MAESTRO"))// && TrackerData.SoftwareVersion=="210" )
                        currentStatus = VehicleStatus.Towed;
                    else if (DeivceTypes.Name.ToUpper() == "MAESTRO" && TrackerData.SoftwareVersion == "210")
                        currentStatus = VehicleStatus.Towed;
                }
            }
            else
                currentStatus = VehicleStatus.Unreachable;

            return currentStatus;
        }

    }
}
