﻿const database = 'Tracker';
const collection = 'TrackerDataLive';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            properties: {
                IMEI: {
                    bsonType: "string"
                },
                SoftwareVersion: {
                    bsonType: "string"
                },
                ProfileName: {
                    bsonType: "string"
                },
                GPSStatus: {
                    bsonType: "bool"
                },
                SignalStrength: {
                    bsonType: "int"
                },
                location: {
                    properties: {
                        type: {
                            bsonType: "string",
                            enum: ["Point"]
                        },
                        coordinates: {
                            bsonType: ["array"]
                        }
                    }
                },
                LocationName: {
                    bsonType: "string"
                },
                Altitude: {
                    bsonType: "int"
                },
                Speed: {
                    bsonType: "int"
                },
                Direction: {
                    bsonType: "int"
                },
                Satellite: {
                    bsonType: "int"
                },
                GPSPositionAccuracyIndication: {
                    bsonType: "double"
                },
                MilageReading: {
                    bsonType: "double"
                },
                Cell: {
                    bsonType: "string"
                },
                Analog1: {
                    bsonType: "double"
                },
                Analog2: {
                    bsonType: "double"
                },
                Analog3: {
                    bsonType: "double"
                },
                Analog4: {
                    bsonType: "double"
                },
                DigitalInputLevel1: {
                    bsonType: "bool"
                },
                DigitalInputLevel2: {
                    bsonType: "bool"
                },
                DigitalInputLevel3: {
                    bsonType: "bool"
                },
                DigitalInputLevel4: {
                    bsonType: "bool"
                },
                DigitalOutputLevel1: {
                    bsonType: "bool"
                },
                DigitalOutputLevel2: {
                    bsonType: "bool"
                },
                DigitalOutputLevel3: {
                    bsonType: "bool"
                },
                DigitalOutputLevel4: {
                    bsonType: "bool"
                },
                Vehicles_VehicleId: {
                    bsonType: "objectId"
                },
                InfoNumber: {
                    bsonType: "int"
                },
                HarshDetecation: {
                    bsonType: "bool"
                },
                RFID: {
                    bsonType: "string"
                },
                IsIgnitionOn: {
                    bsonType: "bool"
                },
                BatteryVoltage: {
                    bsonType: "double"
                },
                BatteryStatus: {
                    bsonType: "bool"
                },
                PortNumber: {
                    bsonType: "int"
                },
                ParkingMode: {
                    bsonType: "bool"
                },
                IsRentEnable: {
                    bsonType: "bool"
                },
                FuelData: {
                    properties: {
                        RawVoltage: {
                            bsonType: "double"
                        },
                        Voltage: {
                            bsonType: "double"
                        },
                        FuelReading: {
                            bsonType: "double"
                        }
                    }
                },
                VehicleData: {
                    properties: {
                        VehicleStatus: {
                            bsonType: "int"
                        },
                        VehicleStatusSince: {
                            bsonType: "date"
                        },
                        VehicleRunningSince: {
                            bsonType: "date"
                        },
                        IsTripActive: {
                            bsonType: "bool"
                        },
                        ActiveTripId: {
                            bsonType: "objectId",
                            description: "must be a objectId and is required"
                        },
                        ParkingMode: {
                            bsonType: "string",
                            enum: ["Enable", "Disable"],
                            description: "must be a Enable or Disable"
                        },
                    }
                },
                CreatedDateTime: {
                    bsonType: "date",
                }
            }
        }
    }
});


db.getCollection(collection).createIndex({
    location: "2dsphere"
});

db.getCollection(collection)
    .createIndex({
        "IMEI": 1
    }, { unique: true });