write-host " *** Exchange DkimSigner Install Script ***" -f "blue"
write-host "Please select your Exchange Version from the following list:" -f "cyan"
write-host "[1] Exchange 2007 SP3" -f "cyan"
write-host "[2] Exchange 2010 (no Service Pack)" -f "cyan"
write-host "[3] Exchange 2010 SP1" -f "cyan"
write-host "[4] Exchange 2010 SP2" -f "cyan"
write-host "[5] Exchange 2010 SP3" -f "cyan"
write-host "[6] Exchange 2013" -f "cyan"

write-host ""
do { 
	$version = read-host "Your selection"
	if ($version -lt 1 -or $version -gt 5) {
		write-host "Invalid selection. Please input the number in the squares." -f "red"
	} 
} until ($version -ge 1 -and $version -le 5) 

$EXDIR="C:\Program Files\Exchange DkimSigner" 
if ($version -eq 1) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2007 SP3"
} elseif ($version -eq 2) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010"
} elseif ($version -eq 3) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 SP1"
} elseif ($version -eq 4) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 S23"
} elseif ($version -eq 5) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 SP3"
} elseif ($version -eq 6) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2013"
}

write-host "Creating registry key for EventLog" -f "green"
if (Test-Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") {
	write-host "Registry key for EventLog already exists. Continuing..." -f "yellow"
} else {
	New-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM"
}


net stop MSExchangeTransport 
 
write-host "Creating install directory: '$EXDIR' and copying data from '$SRCDIR'"  -f "green"
new-item -Type Directory -path $EXDIR -ErrorAction SilentlyContinue 

copy-item "$SRCDIR\ExchangeDkimSigner.dll" $EXDIR -force 
$overwrite = read-host "Do you want to copy (and overwrite) the config file: '$SRCDIR\ExchangeDkimSigner.dll'? [Y/N]"
if ($overwrite -eq "Y" -or $overwrite -eq "y") {
	copy-item "$SRCDIR\ExchangeDkimSigner.dll.config" $EXDIR -force
} else {
	write-host "Not copying config file" -f "yellow"
}

read-host "Now open '$EXDIR\ExchangeDkimSigner.dll.config' to configure Exchange DkimSigner.\nDon't forget to setup all the keys! When done and saved press 'Return'"

write-host "Registering agent" -f "green"
Install-TransportAgent -Name "Exchange DkimSigner" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "$EXDIR\ExchangeDkimSigner.dll"

write-host "Enabling agent" -f "green" 
enable-transportagent -Identity "Exchange DkimSigner" 
get-transportagent 
 
write-host "Starting Edge Transport" -f "green" 
net start MSExchangeTransport 
 
write-host "Installation complete. Check previous outputs for any errors!" -f "yellow" 