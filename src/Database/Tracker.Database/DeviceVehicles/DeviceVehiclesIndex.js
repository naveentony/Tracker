/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'DeviceVehicles';

// The current database to use.
use(database);

db.getCollection('DeviceVehicles')
    .createIndex({ "DeviceNo": 1, "VehicleNo": 1, "SimNo": 1 }, { unique: true });