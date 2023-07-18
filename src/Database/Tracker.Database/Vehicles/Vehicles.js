/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

const database = 'Tracker';
const collection = 'Vehicles';

// The current database to use.
use(database);

db.createCollection(collection, {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["IMEI", "RegistrationNumber", "SimNo"],
            properties: {
                IMEI: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                RegistrationNumber: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                SimNo: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                VehicleType: {
                    bsonType: "string"
                },
                Manufacturer: {
                    bsonType: "string"
                },
                VehicleModel: {
                    bsonType: "string"
                },
                Year: {
                    bsonType: "int"
                },
                ServiceProvider: {
                    bsonType: "string"
                },
                LastServicedOn: {
                    bsonType: "date"
                },
                NextServiceAt: {
                    bsonType: "int"
                },
                PUCExpiryDate: {
                    bsonType: "date"
                },
                TargetUtilizationPerDay: {
                    bsonType: "int"
                },
                SpeedLimit: {
                    bsonType: "int"
                },
                InsuranceExpiryDate: {
                    bsonType: "date"
                },
                PermitExpiryDate: {
                    bsonType: "date"
                },
                NextServiceDate: {
                    bsonType: "date"
                },
                InstallationDate: {
                    bsonType: "date"
                },
                ExpiryDate: {
                    bsonType: "date"
                },
                TemperatureHigh: {
                    bsonType: "double"
                },
                TemperatureLow: {
                    bsonType: "double"
                },
                CurrentAmount: {
                    bsonType: "double"
                },
                GrasePeriod: {
                    bsonType: "int"
                },
                DataLimit: {
                    bsonType: "int"
                },
                IsRelayEnabled: {
                    bsonType: "bool"
                },
                IsACConnected: {
                    bsonType: "bool"
                },
                IsFuelConnected: {
                    bsonType: "bool"
                },
                IsMagnetConnected: {
                    bsonType: "bool"
                },
                IsRentEnabled: {
                    bsonType: "bool"
                },
                AmountStatus: {
                    bsonType: "string",
                    enum: ["Paid", "Pending", "Processing"],
                    description: "must be Paid or Pending,Processing"
                },
                PamentType: {
                    bsonType: "string",
                    enum: ["Credit", "Debit"],
                    description: "must be Credit or Debit"
                },
                RenewalAmount: {
                    bsonType: "double"
                },
                RenewalDays: {
                    bsonType: "int"
                },
                IsDeleted: {
                    bsonType: "bool"
                },
                fuelinfo: {
                    description: 'Contains fuel info',
                    properties: {
                        IsFuelReadingInverse: {
                            bsonType: "string",
                            enum: ["Enable", "Disable"],
                            description: "must be Enable or Disable"
                        },
                        FuelTankCapacityLitres: {
                            bsonType: "double"
                        },
                        VMax: {
                            bsonType: "double"
                        },
                        VMin: {
                            bsonType: "double"
                        }
                    }
                },

                Mileage: {
                    bsonType: "double"
                },
                Users_Id: {
                    bsonType: "binData"
                },
                TrackerTypes_Id: {
                    bsonType: "objectId"
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
db.getCollection(collection)
    .createIndex({
        "IMEI": 1, "SimNo": 1, "RegistrationNumber": 1
    }, { unique: true });