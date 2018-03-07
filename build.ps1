$tag = $args[0];
pushd ./src/Api
$packageJson = Get-Content 'appsettings.json' -raw | ConvertFrom-Json;
$packageJson.version=$tag
$packageJson | ConvertTo-Json  | set-content 'appsettings.json'
popd