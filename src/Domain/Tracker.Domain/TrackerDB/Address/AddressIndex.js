const database = 'Tracker';
const collection = 'Address';

// The current database to use.
use(database);
db.getCollection(collection).createIndex({ "Location": "2dsphere" }, { unique: true });