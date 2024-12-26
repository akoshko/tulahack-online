# curl.exe https://tulahack.<you-name-it>/api/swagger/swagger.json -O swagger.json

docker run --rm -it -v ${PWD}:/local `
countingup/nswag swagger2tsclient `
/input:https://tulahack.<you-name-it>/api/swagger/v1/swagger.json `
/output:local/api.client.ts `
/generateClientClasses:true `
/generateContractsOutput:true `
/template:axios `
/className:GeneratedApiClient `
/configurationClass:ClientConfiguration `
/clientBaseClass:ClientBase `
/useTransformOptionsMethod:true `
/markOptionalProperties:true

docker run --rm -it -v ${PWD}:/local countingup/nswag openapi2csclient `
/input:local/dto.yaml `
/namespace:Tulahack.Dtos `
/DateType:System.DateTime /DateTimeType:System.DateTime `
/GenerateContractsOutput:true `
/ContractsOutput:local/Tulahack.Dtos.Generated.cs `
/ContractsNamespace:Tulahack.Dtos `
/GenerateClientClasses:false `
/JsonLibrary:SystemTextJson `
/GenerateExceptionClasses:false `
/GenerateAdditionalProperties:false `
/ClassStyle:Poco `
/ArrayType:System.Collections.Generic.IEnumerable
#/ClassStyle:Poco `

docker run --rm -it -v ${PWD}:/local countingup/nswag openapi2csclient `
/input:local/model.yaml `
/namespace:Tulahack.Model `
/DateType:System.DateTime /DateTimeType:System.DateTime `
/GenerateContractsOutput:true `
/ContractsOutput:local/Tulahack.Model.Generated.cs `
/ContractsNamespace:Tulahack.Model `
/GenerateClientClasses:false `
/JsonLibrary:SystemTextJson `
/GenerateExceptionClasses:false `
/GenerateAdditionalProperties:false `
/ArrayType:System.Collections.Generic.IEnumerable

mv -Force ./Tulahack.Model.Generated.cs ../
mv -Force ./Tulahack.Dtos.Generated.cs ../
mv -Force ./api.client.ts ../../../../frontend/src/api/api.client.ts

git add ../Tulahack.Model.Generated.cs
git add ../Tulahack.Dtos.Generated.cs
git add ../../../../frontend/src/api/api.client.ts

dotnet ef migrations add Migration-$(Get-Date -format 'MM_dd_yyyy_HH_mm_ss') --startup-project ../../Tulahack.API --project ../../Tulahack.API --context TulahackContext --output-dir ../Tulahack.API/Migrations
git add ../../Tulahack.API/Migrations
