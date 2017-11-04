$root = (split-path -parent $MyInvocation.MyCommand.Definition)

$version = [System.Reflection.Assembly]::LoadFile("$root\src\RubberStamp\bin\Release\RubberStamp.dll").GetName().Version
$versionStr = "{0}.{1}.{2}-RC0{3}" -f ($version.Major, $version.Minor, $version.Build, $version.Revision)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\src\RubberStamp.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\src\RubberStamp.compiled.nuspec

& $root\build\NuGet.exe pack $root\src\RubberStamp.compiled.nuspec