use('Tracker');

db.getCollection('PlanType')
    .createIndex({ "Name": 1 }, { unique: true });
