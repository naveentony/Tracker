/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'PlanType';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Name", "DeviceCost", "RenewalCost", "RenewalDays", "IsActive"],
            properties: {
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                DeviceCost: {
                    bsonType: "double",
                    description: "must be an double and is required"
                },
                RenewalCost: {
                    bsonType: "double",
                    description: "must be an double and is required"
                },
                RenewalDays: {
                    bsonType: ["int"],
                    description: "can only be one of the enum values and is required"
                },
                IsActive: {
                    bsonType: ["string"],
                    enum: ["Enable", "Disable"],
                    description: "can only be one of the enum values and is required"
                },
                IsDeleted: {
                    bsonType: ["string"],
                    enum: ["Enable", "Disable"],
                    description: "can only be one of the enum values and is required"
                },
                CreatedDate: {
                    bsonType: ["date"],
                    description: "must be an double "
                },
                UpdatedDate: {
                    bsonType: ["date"],
                    description: "must be an double "
                }

            }
        }
    }
});