/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'TrackerTypes';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Name"],
            properties: {
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                CompanyName: {
                    bsonType: "string"
                },
                UpdateRates: {
                    bsonType: "string"
                },
                VRef: {
                    bsonType: "double"
                },
                Offset: {
                    bsonType: "double"
                },
                DefaultUpdateRate: {
                    bsonType: "int"
                },
                ZeroSpeed: {
                    bsonType: "double"
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
db.getCollection(collection).createIndex({ "Name": 1 }, { unique: true });