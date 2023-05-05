import subprocess
import sys
import os


action = sys.argv[1]
name = sys.argv[2]
projectType = sys.argv[3]
workingDirectory = sys.argv[4]

os.chdir(workingDirectory)

if action == "create":
    subprocess.run(["dotnet", "new", projectType, "-n", name])
    print(f"Successfully created {projectType} project named {name}")
elif action == "add":
    subprocess.run(["dotnet", "sln", "add", name + "." + projectType + ".csproj"])
    print(f"Successfully added {projectType} project named {name} to the solution")
elif action == "remove":
    subprocess.run(["dotnet", "remove", "reference", name + "." + projectType + ".csproj"])
    print(f"Successfully removed {projectType} project named {name} from the solution")
else:
    print("Invalid action provided")