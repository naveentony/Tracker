//To delete all records 
db.Vehicles.deleteMany({ name: {$exists: false} })

//Update Collection

db.Vehicles.updateOne(
    { IMEI: "358657100287718" },
    { $set: { UserId: "73c5c313-ede4-42e3-ac20-8b81738a949b" } },
    { upsert: true });