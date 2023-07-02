/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'Address';
//https://stackoverflow.com/questions/59691096/mongodb-jsonschema-validation-error-in-compass-unknown-jsonschema-keyword
// The current database to use.
use(database);
//ClientName	Address	Email	MobileNo	Status	Image	DisplayTitle	DisplaySubTitle
db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["Region", "Location", "City", "postal_code"],
            properties: {
                Location: {
                    type: 'object',
                    properties: {
                        type: {
                            'enum': ['Point'],
                            description: 'Must be Point'
                        },
                        coordinates: {
                            bsonType: ['array'],
                            description: 'Contains  Latitude, Longitude'
                        }
                    }
                },
                Region: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                City: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Landmark: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                postal_code: {
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