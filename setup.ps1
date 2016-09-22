$storageDir = $pwd
$webclient = New-Object System.Net.WebClient
$url = "http://storage.googleapis.com/nativeclient-mirror/nacl/nacl_sdk/nacl_sdk.zip"
$file = "$storageDir\nacl_sdk.zip"
$webclient.DownloadFile($url,$file)

Expand-Archive -Path $file -DestinationPath "..\" -Force 