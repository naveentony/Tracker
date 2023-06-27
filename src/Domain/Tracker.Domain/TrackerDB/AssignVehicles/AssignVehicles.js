/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'AssignVehicles';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: ["object"],
            required: ["UserId", "VehicleId", "AssignDate"],
            properties: {
                UserId: { // Referenceing to Users
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                VehicleId: { // Referenceing to DeviceVehicles
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                CreatedDate: {
                    bsonType: "date",
                    description: "must be an Datetime "
                }

            }
        }
    }
});