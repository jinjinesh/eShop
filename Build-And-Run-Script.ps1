# Define the paths to your .NET Web API projects
$projectPaths = @(
    ".\src\OrderService",
	".\src\ProductApi",
	".\src\NotificationService1",
	".\src\NotificationService2"
)

# Loop through each project path and build & start the projects in parallel
foreach ($path in $projectPaths) {
    Start-Process powershell -ArgumentList "cd '$path'; .\Build-And-Run-Script.ps1"
	Start-Sleep -Seconds 5
}

Write-Host "Projects are building and starting in parallel..."
