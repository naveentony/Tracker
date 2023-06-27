use('Tracker');

db.getCollection('AssignVehicles')
    .createIndex({ "UserId": 1,"VehicleId" :1}, { unique: true });