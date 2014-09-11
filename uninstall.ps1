write-host " *** Exchange DkimSigner UNINSTALL Script ***" -f "blue"

$EXDIR="C:\Program Files\Exchange DkimSigner" 
 
Net Stop MSExchangeTransport 
 
write-host "Disabling agent..."  -f "green"
Disable-TransportAgent -Identity "Exchange DkimSigner" 

write-host "Uninstalling agent..."  -f "green"
Uninstall-TransportAgent -Identity "Exchange DkimSigner" 
 
$overwrite = read-host "Do you want to delete all the files in '$EXDIR' (WARNING: all your keys within this directory will get deleted too)? [Y/N]"
if ($overwrite -eq "Y" -or $overwrite -eq "y") { 
	write-host "Deleting directories and files..." -f "green"
	Remove-Item $EXDIR\* -Recurse -Force -ErrorAction SilentlyContinue 
	Remove-Item $EXDIR -Recurse -Force -ErrorAction SilentlyContinue 
} else {
	write-host "Not deleting files" -f "yellow"
} 

write-host "Removing registry key for EventLog" -f "green"
if (Test-Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") {
	Remove-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM"
} else {
	write-host "Key already removed. Continuing..." -f "yellow"
}


write-host "Starting Transport..."  -f "green"
Net Start MSExchangeTransport 
 
write-host "Uninstallation complete. Check previous outputs for any errors!"  -f "yellow"