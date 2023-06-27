const database = 'Tracker';
const collection = 'Address';

// The current database to use.
use(database);
db.getCollection(collection).insertOne({
    'Location': { 'type': 'Point', 'coordinates': [82.3455, 73.3455] },
    'Region': 'India',
    'City': 'Guntur',
    'Landmark': 'Repalle',
    'postal_code': '522265',
    'CreatedDate': new Date("2023-01-15")
});