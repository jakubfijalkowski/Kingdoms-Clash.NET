#----------------------------------------------------
# Skrypt, który kompiluje grę i umieszcza ją wraz ze wszystkimi zasobami w katalogu Deploy/Kingdoms Clash.NET.$version
# Parametry:
#   [string] version - ciąg znaków informujący o wersji
#   [switch] compress - czy tworzyć archiwum
#----------------------------------------------------
param(
	[string]$version = "",
	[switch]$compress = $false,
	[switch]$appendArch = $false,
	[string]$msBuildPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe",
	[string]$buildConfiguration = "Release",
	[string]$buildArch = "x86",
	[string]$7z = "7z"
	);

#Tworzymy folder wyjściowy
Write-Host "Preparing..."	
$outDir = "Deploy\Kingdoms Clash.NET." + $version + "\";
if($appendArch)
{
	$outDir += "." + $buildArch
}

if(-not (Test-Path $outDir))
{
	New-Item $outDir -Type Directory | Out-Null;
}
Write-Host "Done";
Write-Host;

#Budujemy
Write-Host "Building..."
&$msBuildPath /nologo /noconlog /fl /flp:"LogFile=Deploy/build errors.log;errorsonly" /p:"Configuration=$buildConfiguration" -p:"Architecture=$buildArch"
if($LastExitCode -ne 0)
{
	Write-Host "Errors occured, see Deploy/build errors.log";
	return;
}
Write-Host "Done";
Write-Host;

#Kopiujemy to, co zbudowaliśmy
Write-Host "Copying...";
Write-Host "Binaries"
$inDir = "Bin\" + $buildArch + "\" + $buildConfiguration + "\";
$filesToCopy = Get-ChildItem -Path $inDir | Where {$_.Name -match "(^.+exe$)|(^.+dll)$|(^.+config)$" -and -not $_.Name.EndsWith("vshost.exe")};
foreach($f in $filesToCopy)
{
	Write-Host "`t" $f.Name
	Copy-Item $f.FullName ($outDir + $f.Name);
}
Write-Host "Content"

$pathToStrip = (pwd).Path + "\"
$content = Get-ChildItem -Path "Content" -Recurse
foreach($c in $content)
{
	if($c -is [System.IO.DirectoryInfo])
	{
		$newFolder = $outDir + $c.FullName.Replace($pathToStrip, "");
		if(-not (Test-Path $newFolder))
		{
			New-Item $newFolder -Type Directory | Out-Null;
		}
	}
	else
	{
		$newFile = $c.FullName.Replace($pathToStrip, "");
		Write-Host "`t" $newFile
		Copy-Item $c.FullName ($outDir + $newFile);
	}
}
Write-Host "Done";
Write-Host;

if($compress)
{
	Write-Host "Compressing..."
	$archive = [System.IO.Path]::GetFullPath($outDir.Substring(0, $outDir.Length - 1) + ".zip");
	if(Test-Path $archive)
	{
		Remove-Item $archive
	}
	
	&$7z a $archive ([System.IO.Path]::GetFullPath($outDir));
	
	Write-Host "Done"
}