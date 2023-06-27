/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

// The current database to use.
use('Tracker');

db.runCommand({
    collMod: "DeviceVehicles",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["DeviceNo", "VehicleNo", "SimNo", "VehicleId", "AdminID", "InstallationDate", "ExpiryDate", "TrackerId", "IsRelayEnabled", "IsACConnected"],
            properties: {
                DeviceNo: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                VehicleNo: {
                    bsonType: "string",
                    minLength: 12,
                    description: "must be a string and is required"
                },
                SimNo: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                VehicleId: {
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                SalesPerson: {
                    bsonType: "string",
                    description: "must be a string"
                },
                Customer: {
                    bsonType: "string",
                    description: "must be a string"
                },
                VehicleModel: {
                    bsonType: "string",
                    description: "must be a string"
                },
                AdminID: {
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                TimeZone: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                SpeedLimit: {
                    bsonType: "int",
                    description: "must be a int"
                },
                InstallationDate: {
                    bsonType: "date",
                    description: "must be a Date  and is required"
                },
                ExpiryDate: {
                    bsonType: "date",
                    description: "must be a Date and is required"
                },
                DataLimit: {
                    bsonType: "int",
                    description: "must be a int  and is required"
                },
                TrackerId: {
                    bsonType: "objectId",
                    description: "must be a objectId and is required"
                },
                IsRelayEnabled: {
                    enum: ["Enable", "Disable"],
                    description: "must be Enable or Disable"
                },
                IsACConnected: {
                    enum: ["Enable", "Disable"],
                    description: "must be Enable or Disable"
                },
                IsFuelConnected: {
                    enum: ["Enable", "Disable"],
                    description: "must be Enable or Disable"
                },
                IsMagnetConnected: {
                    enum: ["Enable", "Disable"],
                    description: "must be Enable or Disable"
                },
                AmountStatus: {
                    bsonType: "string",
                    description: "must be a string is required"
                },
                RenewalAmount: {
                    bsonType: "decimal"
                },
                RenewalDays: {
                    bsonType: "int"
                }
            }
        }
    }
})
