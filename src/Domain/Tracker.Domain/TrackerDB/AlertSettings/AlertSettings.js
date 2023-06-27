/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'AlertSettings';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: ["object"],
            required: ["AssignId", "Name", "AlertType", "SMSLimit", "EmailLimit"],
            properties: {
                AssignId: { // Referenceing to AssignVehicles
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                Name: {
                    bsonType: "array",
                    description: "must be an array and is required",
                    items: {
                        bsonType: "string",
                    }
                },
                AlertType: {
                    bsonType: "array",
                    description: "must be an array and is required",
                    items: {
                        bsonType: "string",
                    }
                },
                SMSLimit: {
                    bsonType: "int",
                    description: "must be an int and is required"
                },
                EmailLimit: {
                    bsonType: "int",
                    description: "must be an int and is required"
                },
                IsActive: {
                    bsonType: "string",
                    enum: ["Enable", "Disable"],
                    description: "can only be one of the enum values and is required"
                },
                CreatedDate: {
                    bsonType: "date",
                    description: "must be an Datetime "
                },
                UpdatedDate: {
                    bsonType: "date",
                    description: "must be an Datetime "
                }

            }
        }
    }
});