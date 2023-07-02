const database = 'Tracker';
const collection = 'VehicleTypes';

// The current database to use.
use(database);
db.getCollection(collection).createIndex({ "Name": 1 }, { unique: true });