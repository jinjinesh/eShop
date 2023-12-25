#!/bin/bash

# Define the paths to your .NET Web API projects
projectPaths=(
    "./src/OrderService",
    "./src/ProductApi",
    "./src/NotificationService1",
    "./src/NotificationService2"
)
base_path=$(pwd)
echo $base_path

# Loop through each project path and build & start the projects in parallel
for path in "${projectPaths[@]}"; do
    echo "Building  project at $path"
    cd $path
    # ./Build-And-Run-Script.sh &

    gnome-terminal -- /bin/bash -c "./Build-And-Run-Script.sh; exec bash" &

    cd $base_path
done

wait

echo "Projects are building and starting in parallel..."
