const database = 'Tracker';
const collection = 'DeviceTypes';

// The current database to use.
use(database);
db.getCollection(collection).createIndex({ "Name": 1 }, { unique: true });