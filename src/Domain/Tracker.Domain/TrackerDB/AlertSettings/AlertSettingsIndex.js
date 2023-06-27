use('Tracker');

db.getCollection('AlertSettings')
    .createIndex({ "Name": 1 }, { unique: true });
