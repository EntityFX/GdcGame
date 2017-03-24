"C:\Program Files\MongoDB\Server\3.2\bin\mongo" --host %1 10.10.139.148 gdcame.js
"C:\Program Files\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 10.10.139.148 -d "gdcame" -c "User" --file data\User.json --type json  --jsonArray
"C:\Program Files\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 10.10.139.148 -d "gdcame" -c "Item" --file data\Item.json --type json  --jsonArray
"C:\Program Files\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 10.10.139.148 -d "gdcame" -c "Counter" --file data\Counter.json --type json  --jsonArray
