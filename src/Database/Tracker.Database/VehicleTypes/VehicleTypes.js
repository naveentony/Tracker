/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'VehicleTypes';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Name", "Amount", "Status"],
            properties: {
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Amount: {
                    bsonType: "Double",
                    description: "must be a Double and is required"
                },
                IsActive: {
                    bsonType: ["string"],
                    enum: ["Enable", "Disable"],
                    description: "can only be one of the enum values and is required"
                },
                CreatedDate: {
                    bsonType: ["date"],
                    description: "must be an date "
                },
                UpdatedDate: {
                    bsonType: ["date"],
                    description: "must be an date "
                }
            }
        }
    }
});
