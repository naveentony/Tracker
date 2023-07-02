
const database = 'Tracker';
const collection = 'PlanType';

// The current database to use.
use(database);
//NewDeviceCost	RenewalCost	RenewalDays	IsActive	IsDeleted


// Insert a few documents into the sales collection.
db.getCollection('PlanType').insertMany([
    { 'Name': 'Normal', 'DeviceCost': Double(1000.00), 'RenewalCost': Double(500.00), 'RenewalDays': 365, 'IsActive': 'Enable', 'IsDeleted': 'Disable', 'CreatedDate': new Date("2023-06-17") },
    { 'Name': 'Silver', 'DeviceCost': Double(1000.00), 'RenewalCost': Double(800.00), 'RenewalDays': 365, 'IsActive': 'Enable', 'IsDeleted': 'Disable', 'CreatedDate': new Date("2023-06-17") },
    { 'Name': 'Gold', 'DeviceCost': Double(1000.00), 'RenewalCost': Double(500.00), 'RenewalDays': 365, 'IsActive': 'Enable', 'IsDeleted': 'Disable', 'CreatedDate': new Date("2023-06-17") }
]);