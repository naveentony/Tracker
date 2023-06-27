/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'Clients';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Name", "AddressId", "GSTNumber", "InvoiceName"],
            properties: {
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                AddressId: {
                    bsonType: "objectId", //referencing to the Address.
                    description: "must be an objectId and is required"
                },
                Logo: {
                    bsonType: "object",
                    description: 'Contains fileName, file',
                    properties: {
                        fileName: {
                            bsonType: "string"
                        },
                        file: {
                            bsonType: "binData",
                        }
                    }
                },
                GSTNumber: {
                    bsonType: "string",
                    description: "must be an string and is required"
                },
                InvoiceName: {
                    bsonType: "string",
                    description: "must be an string and is required"
                },
                DisplayTitle: {
                    bsonType: "string",
                    description: "must be an string and is required"
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