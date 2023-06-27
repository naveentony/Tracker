const database = 'Tracker';
const collection = 'Clients';
//https://sparkbyexamples.com/mongodb/store-images-in-the-mongodb-database/#:~:text=This%20method%20involves%20reading%20the%20image%20file%20as,to%20store%20the%20image%20bytes%20as%20binary%20data.
// The current database to use.
use(database);
db.getCollection(collection).insertOne({
    'Name': 'Tracker',
    'AddressId': ObjectId('649aea2b2e5ceb4992738eaa'),
    'Logo': { 'fileName': 'tracker.png', 'file': BinData(0, '0A020F0B') },
    'GSTNumber': '1234567778',
    'InvoiceName': 'Tracker',
    'DisplayTitle': 'Tracker',
    'IsActive': 'Enable',
    'CreatedDate': new Date("2023-01-15")
})