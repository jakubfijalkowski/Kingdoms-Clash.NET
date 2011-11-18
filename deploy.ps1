#----------------------------------------------------
# Skrypt, który kompiluje grę i umieszcza ją wraz ze wszystkimi zasobami w katalogu Deploy/Kingdoms Clash.NET.$version
# Parametry:
#   [string] version - ciąg znaków informujący o wersji
#   [switch] compress - czy tworzyć archiwum
#   [switch] appendArch - czy dodawać do fodleru wynikowego architekturę
#   [string] msBuildPath - ścieżka do msbuild
#   [string] buildConfiguration - konfiguracja
#   [string] buildArch - architektura
#   [string] 7z - ścieżka/polecenie do 7z.exe
#----------------------------------------------------
param(
	[string]$version = "",
	[switch]$compress = $true,
	[switch]$appendArch = $true,
	[string]$msBuildPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe",
	[string]$buildConfiguration = "Release",
	[string]$buildArch = "Any CPU",
	[string]$7z = "C:\Program Files\7-Zip\7z.exe"
	);
	
if($version -eq $null)
{
	$version = "";
}

#Tworzymy folder wyjściowy
Write-Host "Preparing..."	
$outDir = "Deploy\Kingdoms_Clash.NET";
if($version -ne "")
{
	$outDir += "." + $version;
}
if($appendArch)
{
	$outDir += "." + $buildArch;
}
$outDir += "\";

if(-not (Test-Path $outDir))
{
	New-Item $outDir -Type Directory | Out-Null;
}
Write-Host "Done";
Write-Host;

#Budujemy
Write-Host "Building..."
&$msBuildPath "Kingdoms Clash.NET.sln" /nologo /noconlog /fl /flp:"LogFile=Deploy/build errors.log;errorsonly" /p:"Configuration=$buildConfiguration" -p:"Architecture=""$buildArch"""
if($LastExitCode -ne 0)
{
	Write-Host "Errors occured, see Deploy/build errors.log";
	return;
}
Write-Host "Done";
Write-Host;

#Kopiujemy to potrzebne pliki
Write-Host "Copying...";

#Binarki
Write-Host "Binaries";
$inDir = "Bin\" + $buildArch + "\" + $buildConfiguration + "\";
$filesToCopy = Get-ChildItem -Path $inDir | Where {$_.Name -match "(^.+exe$)|(^.+dll)$" -and -not $_.Name.EndsWith("vshost.exe")};
foreach($f in $filesToCopy)
{
	Write-Host "`t" $f.Name
	Copy-Item $f.FullName ($outDir + $f.Name);
}
Write-Host;

#Pliki konfiguracyjne(gra ma pierwszeństwo!)
Write-Host "Configurations";
[System.IO.FileInfo[]]$configurationsToCopy;
if($buildConfiguration -eq "Release")
{
	$configurationsToCopy += Get-ChildItem -Path "Src\ClashEngine.NET\" -Filter "*.Release.config"
	$configurationsToCopy += Get-ChildItem -Path "Src\Kingdoms Clash.NET\" -Filter "*.Release.config"
}
else
{
	$configurationsToCopy += Get-ChildItem -Path "Src\ClashEngine.NET\" | Where { $_.Name.EndsWith(".config") -and -not $_.Name.EndsWith("Release.config") }
	$configurationsToCopy += Get-ChildItem -Path "Src\Kingdoms Clash.NET\" | Where { $_.Name.EndsWith(".config") -and -not $_.Name.EndsWith("Release.config") }
}
foreach($f in $configurationsToCopy)
{
	Write-Host "`t" $f.Name
	Copy-Item $f.FullName ($outDir + $f.Name.Replace("Release.config", "config"));
}
Write-Host;

#Jeśli mamy podaną wersję i plik README.$version istnieje - kopiujemy go.
if($version -ne "")
{
	$readmeFile = "README." + $version;
	if(Test-Path $readmeFile)
	{
		Write-Host "README file"
		Copy-Item $readmeFile ($outDir + "README");
		Write-Host;
	}
}

#Zawartość
Write-Host "Content";
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

#Kompresujemy, jeśli trzeba
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