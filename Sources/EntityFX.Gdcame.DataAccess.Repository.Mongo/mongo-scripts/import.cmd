"C:\Program Files (x86)\MongoDB\Server\3.2\bin\mongo" --host %1 gdcame gdcame.js
"C:\Program Files (x86)\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 -d "gdcame" -c "User" --file data\User.json --type json  --jsonArray
"C:\Program Files (x86)\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 -d "gdcame" -c "Item" --file data\Item.json --type json  --jsonArray
"C:\Program Files (x86)\MongoDB\Server\3.2\bin\mongoimport" -v --host %1 -d "gdcame" -c "Counter" --file data\Counter.json --type json  --jsonArray