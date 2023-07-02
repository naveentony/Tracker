/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'DeviceTypes';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Name", "Description", "Command1", "Command2"],
            properties: {
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Description: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Command1: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Command2: {
                    bsonType: "string",
                    description: "must be a string"
                },
                Command3: {
                    bsonType: "string",
                    description: "must be a string"
                },
                Command4: {
                    bsonType: "string",
                    description: "must be a string"
                },
                Command5: {
                    bsonType: "string",
                    description: "must be a string"
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