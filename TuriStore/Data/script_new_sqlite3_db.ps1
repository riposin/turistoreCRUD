<#
This is script expects sqlite3.exe to be in the same folder than itself.
The script checks if the database file exists; If so, it will try to rename it;
in case of error, an error will be displayed.
#>

$file = 'turistore.sqlite3'
$ext = '.db'
$dtime = Get-Date -Format "yyyyMMddHHmmss"
$origname = $file + $ext
$newname = $file + $dtime + $ext
$scriptname = 'DataBase.sqlite3.sql'
$cwd = Get-Location
<#
If (Test-Path $file){Remove-Item $file}
#>

Write-Output "!!!!!!!!!!!!!!!!!----- README -----!!!!!!!!!!!!!!!!!`r`n"
Write-Output "In order the application to work/start, these following adjustments must be made.`r`n`r`n"
<# Write-Output "1 - In 'web.config' file set the DataSource of the connectionString 'visitsEntities' to 'Data Source=|DataDirectory|visits.db'. Save & Close.`r`n" #>
Write-Output "1 - Enjoy."
Write-Output "2 - Save & Close.`r`n`r`n"
read-host "Press any key to continue with the db creation"

$CreateFile = $True

If ([System.IO.File]::Exists("$cwd\$origname")) {
	Try {
		$FileStream = [System.IO.File]::Open("$cwd\$origname",'Open','Write')
		$FileStream.Close()
		$FileStream.Dispose()
		<#
		Remove-Item "$cwd\$origname"
		#>
		Rename-Item "$cwd\$origname" -NewName $newname
		Write-Output "The existing database $origname was renamed to $newname.`r`n"
	} Catch {
		$CreateFile = $False
		Write-Output "The database $origname could not be renamed, probably because it is in use.`r`n"
		<#Write-Output "Error: $($_.Exception.Message)"#>
		read-host "Press any key to exit"
	}
}
if ($CreateFile){
	<#
	$PSDefaultParameterValues['*:Encoding'] = 'UTF8'
	[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
	Get-Content .\db_script_new.sqlite -Encoding UTF8 | .\sqlite3.exe
	#>
	$PSDefaultParameterValues['*:Encoding'] = 'UTF8'
	[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
	Get-Content .\$scriptname -Encoding UTF8 | .\sqlite3.exe
	Write-Output "The new and empty database $origname was created.`r`n"
	read-host "Press any key to exit"
}
