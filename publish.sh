dotnet publish src/ProofOfTestApi/ProofOfTestApi.csproj -o publish/ProofOfTestApi
tar -czvf publish/ProofOfTestApi.tar.gz publish/ProofOfTestApi
rm -r publish/ProofOfTestApi

dotnet publish src/IssuerApi/IssuerApi.csproj -o publish/IssuerApi
tar -czvf publish/IssuerApi.tar.gz publish/IssuerApi
rm -r publish/IssuerApi

dotnet publish src/StaticProofApi/StaticProofApi.csproj -o publish/StaticProofApi
tar -czvf publish/StaticProofApi.tar.gz publish/StaticProofApi
rm -r publish/StaticProofApi

dotnet publish src/CmsSigner/CmsSigner.csproj -o publish/Tools/CmsSigner
tar -czvf publish/CmsSigner.tar.gz publish/Tools/CmsSigner
rm -r publish/Tools/CmsSigner

dotnet publish src/IssuerInteropBenchmark/IssuerInteropBenchmark.csproj -o publish/Tools/IssuerInteropBenchmark
tar -czvf publish/IssuerInteropBenchmark.tar.gz publish/Tools/IssuerInteropBenchmark
rm -r publish/Tools/IssuerInteropBenchmark

dotnet publish src/DigitalGreenGatewayTool/DigitalGreenGatewayTool.csproj -o publish/Tools/DigitalGreenGatewayTool
tar -czvf publish/DigitalGreenGatewayTool.tar.gz publish/Tools/DigitalGreenGatewayTool
rm -r publish/Tools/DigitalGreenGatewayTool
