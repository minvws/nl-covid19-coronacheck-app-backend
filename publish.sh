dotnet publish src/ProofOfTestApi/ProofOfTestApi.csproj -o publish
dotnet publish src/HolderAppApi/HolderAppApi.csproj -o publish/HolderAppApi
dotnet publish src/VerifierAppApi/VerifierAppApi.csproj -o publish/VerifierAppApi
dotnet publish src/CmsSigner/CmsSigner.csproj -o publish/Tools/CmsSigner -p:PublishProfile=Publish-Linux-x64