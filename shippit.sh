rm -rf ./pub-linux
rm -rf ./pub-windows
rm -rf ./pub-mac
dotnet publish ./src/SAROS/SAROS.vbproj -o ./pub-linux -c Release --sc -p:PublishSingleFile=true -r linux-x64
dotnet publish ./src/SAROS/SAROS.vbproj -o ./pub-windows -c Release --sc -p:PublishSingleFile=true -r win-x64
dotnet publish ./src/SAROS/SAROS.vbproj -o ./pub-mac -c Release --sc -p:PublishSingleFile=true -r osx-x64
butler push pub-windows thegrumpygamedev/murder-hobo-of-splorr:windows
butler push pub-linux thegrumpygamedev/murder-hobo-of-splorr:linux
butler push pub-mac thegrumpygamedev/murder-hobo-of-splorr:mac
git add -A
git commit -m "shipped it!"