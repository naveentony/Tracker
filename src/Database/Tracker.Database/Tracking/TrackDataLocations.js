const database = 'Tracker';
const collection = 'TrackDataLocations';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["location"],
            properties: {
                LocationName: {
                    bsonType: "string"
                },
                location: {
                    properties: {
                        type: {
                            bsonType: "string",
                            enum: ["Point"]
                        },
                        coordinates: {
                            bsonType: "number"
                        }
                    }
                }
            }
        }
    }
});

db.getCollection(collection).createIndex({
    location: "2dsphere"
});